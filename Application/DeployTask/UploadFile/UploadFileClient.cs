using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传客户端
    /// </summary>
    public class UploadFileClient : FileSynchronousClient<UploadFile, UploadFileStateEnum>
    {
        /// <summary>
        /// 文件上传客户端接口
        /// </summary>
        internal readonly IUploadFileClientSocketEvent Client;
        /// <summary>
        /// 客户端目录
        /// </summary>
        private readonly DirectoryInfo directory;
        /// <summary>
        /// 扩展名称集合
        /// </summary>
        private readonly HashSet<string> extensions;
        /// <summary>
        /// 完成操作数据文件名称
        /// </summary>
        private string completedFileName;
        /// <summary>
        /// 上传文件最后移动文件操作任务
        /// </summary>
        public UploadCompletedTask UploadCompletedTask
        {
            get
            {
                return new UploadCompletedTask(completedFileName);
            }
        }
        /// <summary>
        /// 上传索引与路径信息
        /// </summary>
        internal UploaderInfo UploaderInfo;
        /// <summary>
        /// 缓冲区池字节数
        /// </summary>
        internal readonly int BufferSize;
        /// <summary>
        /// 是否将上传完成操作数据写入文件
        /// </summary>
        private readonly bool isCaseExtension;
        /// <summary>
        /// 是否将上传完成操作数据写入文件
        /// </summary>
        private readonly bool isGetCompletedFileName;
        /// <summary>
        /// 是否拼接服务端路径
        /// </summary>
        private byte isCombinePath;
        /// <summary>
        /// 文件上传客户端
        /// </summary>
        /// <param name="client">文件上传客户端接口</param>
        /// <param name="clientPath">客户端路径</param>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="extensions">扩展名称集合，扩展名称包括小数点，比如 .txt</param>
        /// <param name="isCaseExtension">扩展名匹配是否区分大小写</param>
        /// <param name="isGetCompletedFileName">是否将上传完成操作数据写入文件</param>
        /// <param name="isDelete">当客户端不存在时，是否删除服务端文件与目录</param>
        /// <param name="concurrency">同步并发数</param>
#if NetStandard21
        public UploadFileClient(IUploadFileClientSocketEvent client, string clientPath, string serverPath, string[]? extensions, bool isCaseExtension, bool isGetCompletedFileName = false, bool isDelete = true, int concurrency = 16) : base(isDelete, concurrency)
#else
        public UploadFileClient(IUploadFileClientSocketEvent client, string clientPath, string serverPath, string[] extensions, bool isCaseExtension, bool isGetCompletedFileName = false, bool isDelete = true, int concurrency = 16) : base(isDelete, concurrency)
#endif
        {
            Client = client;
            directory = new DirectoryInfo(clientPath);
            UploaderInfo.Path = serverPath;
            this.isCaseExtension = isCaseExtension;
            this.extensions = new HashSet<string>();
            if (extensions != null)
            {
                if (isCaseExtension)
                {
                    foreach (string extension in extensions) this.extensions.Add(extension);
                }
                else
                {
                    foreach (string extension in extensions) this.extensions.Add(extension.toLower());
                }
            }
            this.isGetCompletedFileName = isGetCompletedFileName;
            isCombinePath = 2;
            BufferSize = ((CommandClientSocketEvent)client).Client.GetSendBufferPool().Size;
            completedFileName = string.Empty;
        }
        /// <summary>
        /// 开始上传目录文件
        /// </summary>
        /// <param name="checkSuccessState">默认为 true 表示状态为 Success 时直接返回，设置为 false 表示重新上传操作</param>
        /// <returns></returns>
        public async Task<UploadFileStateEnum> Upload(bool checkSuccessState = true)
        {
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                UploadFileStateEnum state;
                bool isUploader = false, isWaitCompleted = false;
                CommandClientReturnValue<UploaderInfo> uploaderInfo = default(CommandClientReturnValue<UploaderInfo>);
                await synchronousLock.EnterAsync();
                try
                {
                    if (synchronousState == UploadFileStateEnum.Success && !checkSuccessState) synchronousState = UploadFileStateEnum.Unknown;
                    if (synchronousState != UploadFileStateEnum.Success)
                    {
                        uploaderInfo = await Client.UploadFileClient.CreateUploader(this.UploaderInfo.Path.notNull(), this.UploaderInfo.BackupPath ?? string.Empty, extensions.ToArray(), isCaseExtension);
                        if (check(ref uploaderInfo))
                        {
                            isUploader = true;
                            isSynchronousPath = 1;
                            await synchronous(directory, string.Empty);
                            isWaitCompleted = true;
                            await completedLock.EnterAsync();
                            await completed();
                        }
                    }
                }
                finally
                {
                    if (isUploader && !isWaitCompleted) await completedLock.EnterAsync();
                    state = synchronousState;
                    synchronousLock.Exit();
                    if (isUploader) Client.UploadFileClient.RemoveUploader(uploaderInfo.Value.Index).Discard();
                }
                return state;
            }
            return UploadFileStateEnum.NotFoundDirectory;
        }
        /// <summary>
        /// 检查上传索引与路径信息
        /// </summary>
        /// <param name="uploaderInfo"></param>
        /// <returns></returns>
        private bool check(ref CommandClientReturnValue<UploaderInfo> uploaderInfo)
        {
            if (uploaderInfo.IsSuccess)
            {
                if (uploaderInfo.Value.Index.Index >= 0)
                {
                    UploaderInfo = uploaderInfo.Value;
                    synchronousState = UploadFileStateEnum.Unknown;
                    pathErrorCount = fileErrorCount = 0;
                    return true;
                }
                else synchronousState = (UploadFileStateEnum)(byte)(uint)-uploaderInfo.Value.Index.Index;
            }
            else synchronousState = UploadFileStateEnum.CallFail;
            return false;
        }
        /// <summary>
        /// 上传操作完成
        /// </summary>
        /// <returns></returns>
        private async Task completed()
        {
            if ((pathErrorCount | fileErrorCount) == 0)
            {
                if (isGetCompletedFileName)
                {
                    CommandClientReturnValue<string> completedFileName = await Client.UploadFileClient.GetCompletedFileName(UploaderInfo.Index);
                    if (completedFileName.IsSuccess)
                    {
                        if (!string.IsNullOrEmpty(completedFileName.Value))
                        {
                            this.completedFileName = completedFileName.Value;
                            synchronousState = UploadFileStateEnum.Success;
                        }
                        else synchronousState = UploadFileStateEnum.NotCompleted;
                    }
                    else synchronousState = UploadFileStateEnum.CallFail;
                }
                else
                {
                    CommandClientReturnValue<bool> isCompleted = await Client.UploadFileClient.Completed(UploaderInfo.Index);
                    if (isCompleted.IsSuccess)
                    {
                        if (isCompleted.Value) synchronousState = UploadFileStateEnum.Success;
                        else synchronousState = UploadFileStateEnum.NotCompleted;
                    }
                    else synchronousState = UploadFileStateEnum.CallFail;
                }
            }
            else synchronousState = UploadFileStateEnum.Completed;
        }
        /// <summary>
        /// 开始上传文件
        /// </summary>
        /// <param name="fileName">客户端相对路径文件名称</param>
        /// <param name="serverFileName">服务端相对路径文件名称，默认为 null 表示与客户端相同</param>
        /// <param name="checkSuccessState">默认为 true 表示状态为 Success 时直接返回，设置为 false 表示重新上传操作</param>
        /// <returns></returns>
#if NetStandard21
        public async Task<UploadFileStateEnum> Upload(string fileName, string? serverFileName = null, bool checkSuccessState = true)
#else
        public async Task<UploadFileStateEnum> Upload(string fileName, string serverFileName = null, bool checkSuccessState = true)
#endif
        {
            FileInfo file = new FileInfo(Path.Combine(directory.FullName, fileName));
            if (await AutoCSer.Common.FileExists(file))
            {
                UploadFileStateEnum state;
                bool isUploader = false;
                CommandClientReturnValue<UploaderInfo> uploaderInfo = default(CommandClientReturnValue<UploaderInfo>);
                await synchronousLock.EnterAsync();
                try
                {
                    if (synchronousState == UploadFileStateEnum.Success && !checkSuccessState) synchronousState = UploadFileStateEnum.Unknown;
                    if (synchronousState != UploadFileStateEnum.Success)
                    {
                        uploaderInfo = await Client.UploadFileClient.CreateUploader(this.UploaderInfo.Path.notNull(), this.UploaderInfo.BackupPath ?? string.Empty, extensions.ToArray(), isCaseExtension);
                        if (check(ref uploaderInfo))
                        {
                            isUploader = true;
                            CommandClientReturnValue<UploadFileInfo> uploadFileInfo = await Client.UploadFileClient.GetFile(UploaderInfo.Index, string.IsNullOrEmpty(serverFileName) ? fileName : serverFileName);
                            if (uploadFileInfo.IsSuccess)
                            {
                                if (uploadFileInfo.Value.State == UploadFileStateEnum.Success)
                                {
                                    UploadFileInfo fileInfo = uploadFileInfo.Value;
                                    if (fileInfo.IsFile | fileInfo.IsBackup)
                                    {
                                        bool isCompleted = false;
                                        while (file.Length == fileInfo.FileInfo.Length && file.LastWriteTimeUtc == fileInfo.FileInfo.LastWriteTime && !isCompleted)
                                        {
                                            isCompleted = true;
                                            if (fileInfo.IsBackup)
                                            {
                                                CommandClientReturnValue<UploadFileInfo> appendResult = await Client.UploadFileClient.AppendCompletedFile(UploaderInfo.Index, fileInfo.FileInfo);
                                                if (appendResult.IsSuccess)
                                                {
                                                    if(appendResult.Value.State == UploadFileStateEnum.Success)
                                                    {
                                                        if (appendResult.Value.FileInfo.Name != null)
                                                        {
                                                            fileInfo = appendResult.Value;
                                                            isCompleted = false;
                                                        }
                                                    }
                                                    else synchronousState = appendResult.Value.State;
                                                }
                                                else synchronousState = UploadFileStateEnum.CallFail;
                                            }
                                        }
                                        if (!isCompleted) await new UploadFile(this, file, ref fileInfo.FileInfo).Synchronous();
                                    }
                                    else await new UploadFile(this, file, fileInfo.FileInfo.FullName).Synchronous();
                                    await completed();
                                }
                                else synchronousState = uploadFileInfo.Value.State;
                            }
                            else synchronousState = UploadFileStateEnum.CallFail;
                        }
                    }
                }
                finally
                {
                    state = synchronousState;
                    synchronousLock.Exit();
                    if (isUploader) Client.UploadFileClient.RemoveUploader(uploaderInfo.Value.Index).Discard();
                }
                return state;
            }
            return UploadFileStateEnum.NotFoundFile;
        }
        /// <summary>
        /// 匹配文件扩展名
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool isExtension(FileInfo file)
        {
            if (extensions.Count == 0) return true;
            if (isCaseExtension) return extensions.Contains(file.Extension);
            return extensions.Contains(file.Extension.toLower());
        }
        /// <summary>
        /// 上传目录处理
        /// </summary>
        /// <param name="clientDirectory"></param>
        /// <param name="serverPath"></param>
        /// <returns></returns>
        protected override async Task synchronous(DirectoryInfo clientDirectory, string serverPath)
        {
            try
            {
                do
                {
                    try
                    {
                        string clientPath = clientDirectory.FullName;
                        clientFiles.Empty();
                        foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(clientDirectory))
                        {
                            if (isExtension(file)) clientFiles.Set(file.Name, file);
                        }
                        var fileCommand = await Client.UploadFileClient.GetFiles(UploaderInfo.Index, serverPath);
                        bool isSuccess;
                        if (fileCommand != null)
                        {
                            var file = default(FileInfo);
                            while (await fileCommand.MoveNext())
                            {
                                UploadFileInfo fileInfo = fileCommand.Current;
                                string fileName = fileInfo.FileInfo.Name.notNull();
                                if (clientFiles.Remove(fileName, out file))
                                {
                                    bool isCompleted = false;
                                    while (file.Length == fileInfo.FileInfo.Length && file.LastWriteTimeUtc == fileInfo.FileInfo.LastWriteTime && !isCompleted)
                                    {
                                        isCompleted = true;
                                        if (fileInfo.IsBackup)
                                        {
                                            CommandClientReturnValue<UploadFileInfo> appendResult = await Client.UploadFileClient.AppendCompletedFile(UploaderInfo.Index, fileInfo.FileInfo);
                                            if (appendResult.IsSuccess)
                                            {
                                                if (appendResult.Value.State == UploadFileStateEnum.Success)
                                                {
                                                    if (appendResult.Value.FileInfo.Name != null)
                                                    {
                                                        fileInfo = appendResult.Value;
                                                        isCompleted = false;
                                                    }
                                                }
                                                else onFileError(new FileInfo(Path.Combine(clientPath, fileName)), fileInfo.FileInfo.FullName, appendResult.Value.State);
                                            }
                                            else onFileError(new FileInfo(Path.Combine(clientPath, fileName)), fileInfo.FileInfo.FullName, UploadFileStateEnum.CallFail);
                                        }
                                    }
                                    if (!isCompleted) appendFile(new UploadFile(this, file, ref fileInfo.FileInfo));
                                }
                                else if (IsDelete && fileInfo.IsFile)
                                {
                                    CommandClientReturnValue<bool> isDelete = await Client.UploadFileClient.AppendDeleteFile(UploaderInfo.Index, fileInfo.FileInfo.FullName);
                                    if (!isDelete.IsSuccess || !isDelete.Value) onFileError(new FileInfo(Path.Combine(clientPath, fileName)), fileInfo.FileInfo.FullName, UploadFileStateEnum.CallFail);
                                }
                            }
                            isSuccess = fileCommand.ReturnType == CommandClientReturnTypeEnum.Success;
                        }
                        else isSuccess = false;
                        foreach (FileInfo file in clientFiles.Values)
                        {
                            var fileName = file.Name;
                            if (!string.IsNullOrEmpty(serverPath))
                            {
                                if (isCombinePath == 0) fileName = Path.Combine(serverPath, fileName);
                                else
                                {
                                    CommandClientReturnValue<string> combinePath = await Client.UploadFileClient.CombinePath(serverPath, fileName);
                                    if (combinePath.IsSuccess)
                                    {
                                        if (isCombinePath == 2) isCombinePath = combinePath.Value == Path.Combine(serverPath, fileName) ? (byte)0 : (byte)1;
                                        fileName = combinePath.Value;
                                    }
                                    else
                                    {
                                        onFileError(new FileInfo(Path.Combine(clientPath, fileName)), Path.Combine(serverPath, fileName), UploadFileStateEnum.CallFail);
                                        fileName = null;
                                    }
                                }
                            }
                            if (fileName != null) appendFile(new UploadFile(this, file, fileName));
                        }

                        clientDirectorys.Empty();
                        foreach (DirectoryInfo directory in await AutoCSer.Common.GetDirectories(clientDirectory)) clientDirectorys.Set(directory.Name, directory);
                        var directoryCommand = await Client.UploadFileClient.GetDirectoryNames(UploaderInfo.Index, serverPath);
                        if (directoryCommand != null)
                        {
                            while (await directoryCommand.MoveNext())
                            {
                                DirectoryName directoryName = directoryCommand.Current;
                                if (clientDirectorys.Remove(directoryName.Name))
                                {
                                    Monitor.Enter(fileLock);
                                    try
                                    {
                                        waitPaths.Add(new KeyValue<DirectoryInfo, string>(new DirectoryInfo(Path.Combine(clientPath, directoryName.Name)), directoryName.FullName));
                                    }
                                    finally { Monitor.Exit(fileLock); }
                                }
                                else if (IsDelete)
                                {
                                    CommandClientReturnValue<bool> isDelete = await Client.UploadFileClient.AppendDeleteDirectory(UploaderInfo.Index, directoryName.FullName);
                                    if (!isDelete.IsSuccess || !isDelete.Value) onPathError(new DirectoryInfo(Path.Combine(clientPath, directoryName.Name)), directoryName.FullName, UploadFileStateEnum.CallFail);
                                }
                            }
                            isSuccess = directoryCommand.ReturnType == CommandClientReturnTypeEnum.Success;
                        }
                        else isSuccess = false;
                        foreach (DirectoryInfo directory in clientDirectorys.Values)
                        {
                            CommandClientReturnValue<string> combinePath = await Client.UploadFileClient.CreateDirectory(UploaderInfo.Index, serverPath, directory.Name);
                            if (combinePath.IsSuccess && combinePath.Value != null)
                            {
                                Monitor.Enter(fileLock);
                                try
                                {
                                    waitPaths.Add(new KeyValue<DirectoryInfo, string>(new DirectoryInfo(Path.Combine(clientPath, directory.Name)), combinePath.Value));
                                }
                                finally { Monitor.Exit(fileLock); }
                            }
                            else onPathError(new DirectoryInfo(Path.Combine(clientPath, directory.Name)), Path.Combine(serverPath, directory.Name), UploadFileStateEnum.CallFail);
                        }

                        if (!isSuccess) onPathError(clientDirectory, serverPath, UploadFileStateEnum.CallFail);
                    }
                    catch (Exception exception)
                    {
                        onPathError(clientDirectory, serverPath, UploadFileStateEnum.ClientException);
                        await AutoCSer.LogHelper.Exception(exception);
                    }
                    Monitor.Enter(fileLock);
                    KeyValue<DirectoryInfo, string> path;
                    if (synchronousFileCount != files.Length && waitPaths.TryPop(out path))
                    {
                        clientDirectory = path.Key;
                        serverPath = path.Value;
                        Monitor.Exit(fileLock);
                    }
                    else
                    {
                        Monitor.Exit(fileLock);
                        return;
                    }
                }
                while (true);
            }
            finally
            {
                if (synchronousFileCount == 0 && waitPaths.Length == 0) completedLock.Exit();
                Interlocked.Exchange(ref isSynchronousPath, 0);
            }
        }
    }
}
