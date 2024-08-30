using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 客户端上传文件
    /// </summary>
    public sealed class UploadFile : SynchronousFile
    {
        /// <summary>
        /// 文件上传客户端
        /// </summary>
        private readonly UploadFileClient client;
        /// <summary>
        /// 文件上传数据
        /// </summary>
        private UploadFileBuffer uploadFileBuffer;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        private ByteArrayBuffer buffer;
        /// <summary>
        /// 读取文件字节数
        /// </summary>
        private int bufferSize;
        /// <summary>
        /// 客户端读文件操作
        /// </summary>
        /// <param name="client">文件上传客户端</param>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="fileInfo">文件信息</param>
        internal UploadFile(UploadFileClient client, FileInfo clientFile, ref SynchronousFileInfo fileInfo) : base(clientFile, ref fileInfo)
        {
            this.client = client;
            FileInfo.SetLastWriteTime(clientFile.LastWriteTime);
        }
        /// <summary>
        /// 客户端读文件操作
        /// </summary>
        /// <param name="client">文件上传客户端</param>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        internal UploadFile(UploadFileClient client, FileInfo clientFile, string serverFileName) : base(clientFile, serverFileName)
        {
            this.client = client;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        internal override async Task Synchronous()
        {
            int uploadState = (byte)UploadFileStateEnum.ClientException;
            CommandClientReturnValue<UploadFileIndex> fileIndex = default(CommandClientReturnValue<UploadFileIndex>);
            try
            {
                long serverFileLength = FileInfo.SetUploadLength(ClientFile.Length);
                fileIndex = await client.Client.UploadFileClient.CreateFile(client.UploaderInfo.Index, FileInfo, serverFileLength);
                if (fileIndex.IsSuccess)
                {
                    if (fileIndex.Value.Index >= 0)
                    {
                        if (ClientFile.Length != 0)
                        {
                            long unreadSize = ClientFile.Length - serverFileLength;
                            int bufferSize = (int)Math.Min(unreadSize, client.BufferSize);
#if DotNet45 || NetStandard2
                            using (FileStream fileStream = await AutoCSer.Common.Config.CreateFileStream(ClientFile.FullName, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.SequentialScan))
#else
                            await using (FileStream fileStream = await AutoCSer.Common.Config.CreateFileStream(ClientFile.FullName, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.SequentialScan))
#endif
                            {
                                unreadSize = fileStream.Length;
                                if (serverFileLength != 0)
                                {
                                    await AutoCSer.Common.Config.Seek(fileStream, serverFileLength, SeekOrigin.Begin);
                                    unreadSize -= serverFileLength;
                                }
                                buffer = ByteArrayPool.GetBuffer(bufferSize);
                                uploadFileBuffer = new UploadFileBuffer(client.UploaderInfo.Index, fileIndex.Value);
                                bufferSize = buffer.Buffer.BufferSize;
                                byte[] bufferArray = buffer.Buffer.Buffer;
                                do
                                {
                                    int readSize = await fileStream.ReadAsync(bufferArray, buffer.StartIndex + this.bufferSize, bufferSize - this.bufferSize);
                                    this.bufferSize += readSize;
                                    uploadState = (int)await upload();
                                    if (uploadState != (byte)UploadFileStateEnum.Success) return;
                                    unreadSize -= readSize;
                                }
                                while (unreadSize > 0);
                                while (this.bufferSize > 0)
                                {
                                    uploadState = (int)await upload();
                                    if (uploadState != (byte)UploadFileStateEnum.Success) return;
                                }
                                uploadState = (byte)UploadFileStateEnum.Success;
                                fileIndex = default(CommandClientReturnValue<UploadFileIndex>);
                            }
                        }
                        else
                        {
                            uploadState = (byte)UploadFileStateEnum.Success;
                            fileIndex = default(CommandClientReturnValue<UploadFileIndex>);
                        }
                    }
                    else uploadState = -fileIndex.Value.Index;
                }
                else uploadState = (byte)UploadFileStateEnum.CallFail;
            }
            catch(Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally
            {
                buffer.Free();
                if (fileIndex.IsSuccess && fileIndex.Value.Index >= 0) client.Client.UploadFileClient.RemoveFile(client.UploaderInfo.Index, fileIndex.Value).Discard();
                if (uploadState == (byte)UploadFileStateEnum.Success) client.Completed(this);
                else client.onFileError(ClientFile, FileInfo.FullName, (UploadFileStateEnum)(byte)(uint)uploadState);
            }
        }
        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <returns></returns>
        private async Task<UploadFileStateEnum> upload()
        {
            byte[] bufferArray = buffer.Buffer.Buffer;
            uploadFileBuffer.Buffer.Set(bufferArray, buffer.StartIndex, bufferSize);
            CommandClientReturnValue<UploadFileStateEnum> state = await client.Client.UploadFileClient.UploadFileData(uploadFileBuffer);
            if (!state.IsSuccess) return UploadFileStateEnum.CallFail;
            if (state.Value != UploadFileStateEnum.Success) return state.Value;
            if ((bufferSize -= uploadFileBuffer.SerializeSize) != 0)
            {
                AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + uploadFileBuffer.SerializeSize, bufferArray, buffer.StartIndex, bufferSize);
            }
            return UploadFileStateEnum.Success;
        }
    }
}
