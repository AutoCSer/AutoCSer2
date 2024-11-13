using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 客户端写文件操作
    /// </summary>
    public sealed class PullFile : SynchronousFile
    {
        /// <summary>
        /// 拉取文件客户端
        /// </summary>
        private readonly PullFileClient client;
        /// <summary>
        /// 客户端写入文件流
        /// </summary>
#if NetStandard21
        private FileStream? writeStream;
#else
        private FileStream writeStream;
#endif
        /// <summary>
        /// 获取文件数据命令
        /// </summary>
#if NetStandard21
        private EnumeratorCommand<PullFileBuffer?>? getFileDataCommand;
#else
        private EnumeratorCommand<PullFileBuffer> getFileDataCommand;
#endif
        /// <summary>
        /// 文件读取状态
        /// </summary>
        private int fileReadState;
        /// <summary>
        /// 客户端写文件操作
        /// </summary>
        /// <param name="client">拉取文件客户端</param>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="fileInfo">文件信息</param>
        internal PullFile(PullFileClient client, FileInfo clientFile, ref SynchronousFileInfo fileInfo) : base(clientFile, ref fileInfo)
        {
            this.client = client;
        }
        /// <summary>
        /// 开始拉取文件
        /// </summary>
        /// <returns></returns>
        internal override async Task Synchronous()
        {
            try
            {
                bool isFile = await AutoCSer.Common.Config.FileExists(ClientFile);
                if (isFile)
                {
                    if (ClientFile.LastWriteTimeUtc == FileInfo.LastWriteTime)
                    {
                        if (ClientFile.Length == FileInfo.Length)
                        {
                            fileReadState = (byte)PullFileStateEnum.Success;
                            return;
                        }
                        isFile = ClientFile.Length < FileInfo.Length;
                    }
                    else isFile = false;
                    if (!isFile) await AutoCSer.Common.Config.DeleteFile(ClientFile);
                }
                bool isFileChanged;
                do
                {
                    if (FileInfo.Length == 0)
                    {
                        using (FileStream fileStream = await AutoCSer.Common.Config.CreateFileStream(ClientFile.FullName, FileMode.CreateNew, FileAccess.Write)) AutoCSer.Common.EmptyFunction();
                        ClientFile.LastWriteTimeUtc = FileInfo.LastWriteTime;
                        fileReadState = (byte)PullFileStateEnum.Success;
                        return;
                    }
                    PullFileBuffer readFileBuffer = new PullFileBuffer(this);
                    if (isFile)
                    {
                        writeStream = await AutoCSer.Common.Config.CreateFileStream(ClientFile.FullName, FileMode.Open, FileAccess.Write);
                        await AutoCSer.Common.Config.Seek(writeStream, 0, SeekOrigin.End);
                        FileInfo.SetLength(writeStream.Length);
                    }
                    else
                    {
                        writeStream = await AutoCSer.Common.Config.CreateFileStream(ClientFile.FullName, FileMode.CreateNew, FileAccess.Write);
                        FileInfo.SetLength(0);
                    }
                    fileReadState = (byte)PullFileStateEnum.Success;
                    getFileDataCommand = await client.Client.PullFileClient.GetFileData(readFileBuffer, FileInfo);
                    isFileChanged = false;
                    if (getFileDataCommand != null)
                    {
                        while (await getFileDataCommand.MoveNext())
                        {
                            if (getFileDataCommand.Current == null)
                            {
                                fileReadState = (byte)PullFileStateEnum.CallFail;
                                return;
                            }
                        }
                        if (getFileDataCommand.ReturnType == CommandClientReturnTypeEnum.Success)
                        {
                            switch (fileReadState)
                            {
                                case (byte)PullFileStateEnum.Success: return;
                                case (byte)PullFileStateEnum.NotExist:
#if NetStandard21
                                    await writeStream.DisposeAsync();
#else
                                    writeStream.Dispose();
#endif
                                    writeStream = null;
                                    if (client.IsDelete) await AutoCSer.Common.Config.DeleteFile(ClientFile);
                                    fileReadState = (byte)PullFileStateEnum.Success;
                                    return;
                                case (byte)PullFileStateEnum.LengthLess:
                                case (byte)PullFileStateEnum.LastWriteTimeNotMatch:
#if NetStandard21
                                    await writeStream.DisposeAsync();
#else
                                    writeStream.Dispose();
#endif
                                    writeStream = null;
                                    await AutoCSer.Common.Config.DeleteFile(ClientFile);
                                    CommandClientReturnValue<SynchronousFileInfo> fileInfo = await client.Client.PullFileClient.GetFile(FileInfo.FullName);
                                    if (fileInfo.IsSuccess)
                                    {
                                        FileInfo = fileInfo.Value;
                                        if (FileInfo.FullName != null)
                                        {
                                            isFile = false;
                                            isFileChanged = true;
                                            break;
                                        }
                                        fileReadState = (byte)PullFileStateEnum.Success;
                                    }
                                    else fileReadState = (byte)PullFileStateEnum.CallFail;
                                    return;
                            }
                        }
                        else fileReadState = (byte)PullFileStateEnum.CallFail;
                    }
                    else fileReadState = (byte)PullFileStateEnum.CallFail;
                }
                while (isFileChanged);
            }
            catch (Exception exception)
            {
                fileReadState = (byte)PullFileStateEnum.ClientException;
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally
            {
                if (fileReadState == (byte)PullFileStateEnum.Success) client.Completed(this);
                else client.onFileError(ClientFile, FileInfo.FullName, (PullFileStateEnum)(byte)fileReadState);
                if (writeStream != null)
                {
#if NetStandard21
                    await writeStream.DisposeAsync();
#else
                    writeStream.Dispose();
#endif
                    ClientFile.LastWriteTimeUtc = FileInfo.LastWriteTime;
                }
            }
        }
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ref SubArray<byte> buffer)
        {
            writeStream.notNull().Write(buffer.Array, buffer.Start, buffer.Length);
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Error(int state)
        {
            fileReadState = state;
            getFileDataCommand.notNull().CancelKeepCallback(CommandClientReturnTypeEnum.ClientDeserializeError, null);
        }
    }
}
