using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传客户端
    /// </summary>
    internal sealed class UploadFileClient
    {
        /// <summary>
        /// 客户端任务创建器
        /// </summary>
        private readonly DeployTaskClientBuilder clientBuilder;
        /// <summary>
        /// 服务端路径
        /// </summary>
        private readonly string serverPath;
        /// <summary>
        /// 上传文件批次号
        /// </summary>
        private readonly int uploadIndex;
        /// <summary>
        /// 上传任务数量
        /// </summary>
        private int uploadTaskCount;
        /// <summary>
        /// 上传任务等待锁
        /// </summary>
        private readonly SemaphoreSlim uploadTaskLock = new SemaphoreSlim(0, 1);
        /// <summary>
        /// 缓冲区集合
        /// </summary>
        private LeftArray<byte[]> buffers;
        /// <summary>
        /// 缓冲区集合访问锁
        /// </summary>
        private readonly object bufferLock = new object();
        /// <summary>
        /// 上传文件结果
        /// </summary>
        private DeployTaskUploadFileResult result;
        /// <summary>
        /// 文件上传客户端
        /// </summary>
        /// <param name="clientBuilder">客户端任务创建器</param>
        /// <param name="serverPath">服务端路径</param>
        internal UploadFileClient(DeployTaskClientBuilder clientBuilder, string serverPath)
        {
            this.clientBuilder = clientBuilder;
            this.serverPath = serverPath;
            uploadIndex = Interlocked.Increment(ref clientBuilder.UploadIndex);
        }
        /// <summary>
        /// 上传文件目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal async Task<DeployTaskUploadFileResult> UploadFileAsync(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                uploadTaskCount = 1;
                await uploadFileAsync(directory, string.Empty);
                if (Interlocked.Decrement(ref uploadTaskCount) != 0) await uploadTaskLock.WaitAsync();
            }
            else result.State = DeployTaskUploadFileStateEnum.NotFoundDirectory;
            return result;
        }
        /// <summary>
        /// 上传文件目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path">服务端相对路径</param>
        /// <returns></returns>
        private async Task uploadFileAsync(DirectoryInfo directory, string path)
        {
            FileInfo[] files = await AutoCSer.Common.DirectoryGetFiles(directory);
            if (files.Length == 0) return;
            FileTime[] fileTimes = files.getArray(p => new FileTime(p));
            var differents = await clientBuilder.Client.Client.DeployTaskClient.GetDifferent(clientBuilder.Identity, uploadIndex, serverPath, path, fileTimes);
            if (!differents.IsSuccess)
            {
                result.Set(DeployTaskUploadFileStateEnum.GetDifferentError, differents.ReturnType);
                return;
            }
            if(differents.Value == null)
            {
                result.Set(DeployTaskUploadFileStateEnum.NotFoundTaskOrException, CommandClientReturnTypeEnum.Success);
                return;
            }
            int fileIndex = 0;
            foreach (bool different in differents.Value)
            {
                if (different)
                {
                    Interlocked.Increment(ref uploadTaskCount);
                    uploadFileAsync(path, files[fileIndex], fileTimes[fileIndex]).NotWait();
                    if (result.State != DeployTaskUploadFileStateEnum.Success) return;
                }
                ++fileIndex;
            }
            foreach (DirectoryInfo nextDirectory in await AutoCSer.Common.GetDirectories(directory))
            {
                await uploadFileAsync(nextDirectory, path.Length == 0 ? path : Path.Combine(path, nextDirectory.Name));
                if (result.State != DeployTaskUploadFileStateEnum.Success) return;
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="fileTime"></param>
        /// <returns></returns>
        private async Task uploadFileAsync(string path, FileInfo file, FileTime fileTime)
        {
            var buffer = default(byte[]);
            await clientBuilder.Client.UploadFileLock.WaitAsync();
            try
            {
                if (result.State != DeployTaskUploadFileStateEnum.Success) return;
#if NetStandard21
                await using (FileStream fileStream = await AutoCSer.Common.CreateFileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
#else
                using (FileStream fileStream = await AutoCSer.Common.CreateFileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
#endif
                {
                    if (fileStream.Length != fileTime.Length)
                    {
                        result.State = DeployTaskUploadFileStateEnum.FileLengthChanged;
                        return;
                    }
                    await AutoCSer.Common.RefreshFileInfo(file);
                    if (file.LastWriteTimeUtc != fileTime.LastWriteTimeUtc)
                    {
                        result.State = DeployTaskUploadFileStateEnum.FileTimeChanged;
                        return;
                    }
                    CommandClientReturnValue<CreateUploadFileResult> createResult = await clientBuilder.Client.Client.DeployTaskClient.CreateUploadFile(clientBuilder.Identity, uploadIndex, path, fileTime);
                    if (result.State != DeployTaskUploadFileStateEnum.Success) return;
                    if (!createResult.IsSuccess)
                    {
                        result.Set(DeployTaskUploadFileStateEnum.CreateUploadFileError, createResult.ReturnType);
                        return;
                    }
                    if (createResult.Value.BufferSize == 0)
                    {
                        result.State = DeployTaskUploadFileStateEnum.CreateUploadFileUnknownError;
                        return;
                    }
                    if (createResult.Value.Length == fileTime.Length) return;
                    if (createResult.Value.Length != 0)
                    {
                        await AutoCSer.Common.Seek(fileStream, createResult.Value.Length, SeekOrigin.Begin);
                        fileTime.Length -= createResult.Value.Length;
                    }
                    UploadFileBuffer uploadFileBuffer = new UploadFileBuffer(buffer = getBuffer());
                    int bufferSize = Math.Min(buffer.Length, createResult.Value.BufferSize);
                    while (fileTime.Length > 0)
                    {
                        uploadFileBuffer.Length = await fileStream.ReadAsync(buffer, 0, bufferSize);
                        if (result.State != DeployTaskUploadFileStateEnum.Success) return;
                        CommandClientReturnValue<bool> writeResult = await clientBuilder.Client.Client.DeployTaskClient.WriteUploadFile(clientBuilder.Identity, createResult.Value.Index, uploadFileBuffer);
                        if (result.State != DeployTaskUploadFileStateEnum.Success) return;
                        if (!writeResult.IsSuccess)
                        {
                            result.Set(DeployTaskUploadFileStateEnum.WriteUploadFileError, writeResult.ReturnType);
                            return;
                        }
                        if (!writeResult.Value)
                        {
                            result.State = DeployTaskUploadFileStateEnum.WriteUploadFileUnknownError;
                            return;
                        }
                        fileTime.Length -= uploadFileBuffer.Length;
                    }
                }
                Monitor.Enter(bufferLock);
                if (buffers.TryAdd(buffer)) Monitor.Exit(bufferLock);
                else
                {
                    try
                    {
                        buffers.Add(buffer);
                    }
                    finally { Monitor.Exit(bufferLock); }
                }
            }
            finally 
            {
                clientBuilder.Client.UploadFileLock.Release();
                if (Interlocked.Decrement(ref uploadTaskCount) == 0) uploadTaskLock.Release();
            }
        }
        /// <summary>
        /// 获取文件上传缓冲区
        /// </summary>
        /// <returns></returns>
        private byte[] getBuffer()
        {
            var buffer = default(byte[]);
            Monitor.Enter(bufferLock);
            if (buffers.TryPop(out buffer))
            {
                Monitor.Exit(bufferLock);
                return buffer;
            }
            Monitor.Exit(bufferLock);
            return AutoCSer.Common.GetUninitializedArray<byte>(clientBuilder.Client.UploadFileBufferSize);
        }
        /// <summary>
        /// 添加复制文件步骤
        /// </summary>
        /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
        /// <param name="isBackup">是否备份历史文件</param>
        /// <returns></returns>
#if NetStandard21
        public ReturnCommand<DeployTaskAppendResult> AppendCopyFile(string? checkSwitchFileName = null, bool isBackup = true)
#else
        public ReturnCommand<DeployTaskAppendResult> AppendCopyFile(string checkSwitchFileName = null, bool isBackup = true)
#endif
        {
            return clientBuilder.Client.Client.DeployTaskClient.AppendCopyUploadFile(clientBuilder.Identity, uploadIndex, serverPath, checkSwitchFileName ?? string.Empty, isBackup);
        }
    }
}
