using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 拉取文件客户端
    /// </summary>
    public class PullFileClient : FileSynchronousClient<PullFile, PullFileStateEnum>
    {
        /// <summary>
        /// 拉取文件客户端接口
        /// </summary>
        internal readonly IPullFileClientSocketEvent Client;
        /// <summary>
        /// 拉取目录处理文件集合
        /// </summary>
        private LeftArray<PullFile> pathFiles;
        /// <summary>
        /// 拉取文件客户端
        /// </summary>
        /// <param name="client">拉取文件客户端接口</param>
        /// <param name="isDelete">当服务端不存在时，是否删除客户端文件与目录</param>
        /// <param name="concurrency">同步并发数</param>
        public PullFileClient(IPullFileClientSocketEvent client, bool isDelete = true, int concurrency = 16) : base(isDelete, concurrency)
        {
            Client = client;
            pathFiles.SetEmpty();
        }
        /// <summary>
        /// 开始拉取目录文件
        /// </summary>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="clientPath">客户端路径</param>
        /// <returns></returns>
        public async Task<PullFileStateEnum> PullDirectory(string serverPath, string clientPath)
        {
            PullFileStateEnum state;
            bool isWaitCompleted = false;
            await synchronousLock.EnterAsync();
            try
            {
                isSynchronousPath = 1;
                pathErrorCount = fileErrorCount = 0;
                await synchronous(new DirectoryInfo(clientPath), serverPath);
                isWaitCompleted = true;
                await completedLock.EnterAsync();
                synchronousState = (pathErrorCount | fileErrorCount) == 0 ? PullFileStateEnum.Success : PullFileStateEnum.Completed;
            }
            finally
            {
                if (!isWaitCompleted) await completedLock.EnterAsync();
                state = synchronousState;
                synchronousLock.Exit();
            }
            return state;
        }
        /// <summary>
        /// 开始拉取文件
        /// </summary>
        /// <param name="serverFileName">服务端文件名称</param>
        /// <param name="clientFileName">客户端文件名称</param>
        /// <returns></returns>
        public async Task<PullFileStateEnum> PullFile(string serverFileName, string clientFileName)
        {
            PullFileStateEnum state;
            await synchronousLock.EnterAsync();
            try
            {
                FileInfo clientFile = new FileInfo(clientFileName);
                await AutoCSer.Common.TryCreateDirectory(clientFile.Directory.notNull());
                CommandClientReturnValue<SynchronousFileInfo> fileInfoResult = await Client.PullFileClient.GetFile(serverFileName);
                if (fileInfoResult.IsSuccess)
                {
                    SynchronousFileInfo fileInfo = fileInfoResult.Value;
                    if (fileInfo.Name != null)
                    {
                        if (await AutoCSer.Common.FileExists(clientFile) && clientFile.LastWriteTimeUtc == fileInfo.LastWriteTime && clientFile.Length == fileInfo.Length)
                        {
                            synchronousState = PullFileStateEnum.Success;
                        }
                        else
                        {
                            pathErrorCount = fileErrorCount = 0;
                            await new PullFile(this, clientFile, ref fileInfo).Synchronous();
                            synchronousState = (pathErrorCount | fileErrorCount) == 0 ? PullFileStateEnum.Success : PullFileStateEnum.Completed;
                        }
                    }
                    else
                    {
                        if (IsDelete) await AutoCSer.Common.TryDeleteFile(clientFile);
                        synchronousState = PullFileStateEnum.Success;
                    }
                }
                else onFileError(clientFile, serverFileName, PullFileStateEnum.CallFail);
            }
            finally
            {
                state = synchronousState;
                synchronousLock.Exit();
            }
            return state;
        }
        /// <summary>
        /// 拉取目录处理
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
                        clientFiles.Clear();
                        clientDirectorys.Clear();
                        if (IsDelete)
                        {
                            if (await AutoCSer.Common.DirectoryExists(clientDirectory))
                            {
                                foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(clientDirectory))
                                {
                                    clientFiles.Set(file.Name, file);
                                }
                                foreach (DirectoryInfo directory in await AutoCSer.Common.GetDirectories(clientDirectory))
                                {
                                    clientDirectorys.Set(directory.Name, directory);
                                }
                            }
                            else await AutoCSer.Common.TryCreateDirectory(clientDirectory);
                        }
                        else await AutoCSer.Common.TryCreateDirectory(clientDirectory);

                        pathFiles.Length = 0;
                        string clientPath = clientDirectory.FullName;
                        var fileCommand = await Client.PullFileClient.GetFiles(serverPath);
                        bool isSuccess;
                        if (fileCommand != null)
                        {
                            while (await fileCommand.MoveNext())
                            {
                                var file = default(FileInfo);
                                SynchronousFileInfo fileInfo = fileCommand.Current;
                                if (IsDelete) clientFiles.Remove(fileInfo.Name.notNull(), out file);
                                if (file == null) file = new FileInfo(Path.Combine(clientPath, fileInfo.Name.notNull()));
                                if (!await AutoCSer.Common.FileExists(file) || file.LastWriteTimeUtc != fileInfo.LastWriteTime || file.Length != fileInfo.Length)
                                {
                                    pathFiles.Add(new PullFile(this, file, ref fileInfo));
                                }
                            }
                            isSuccess = fileCommand.ReturnType == CommandClientReturnTypeEnum.Success;
                        }
                        else isSuccess = false;
                        if (isSuccess && IsDelete)
                        {
                            foreach (FileInfo file in clientFiles.Values) await AutoCSer.Common.DeleteFile(file);
                        }
                        while (pathFiles.Length != 0)
                        {
                            PullFile pullFile = pathFiles.Array[--pathFiles.Length];
                            Monitor.Enter(fileLock);
                            if (synchronousFileCount != files.Length)
                            {
                                pullFile.FileIndex = synchronousFileCount;
                                files[synchronousFileCount++] = pullFile;
                                Monitor.Exit(fileLock);
                            }
                            else
                            {
                                try
                                {
                                    ++pathFiles.Length;
                                    waitFiles.Add(pathFiles);
                                }
                                finally { Monitor.Exit(fileLock); }
                                break;
                            }
                            pullFile.Synchronous().NotWait();
                        }

                        bool isCallSuccess = isSuccess;
                        var directoryCommand = await Client.PullFileClient.GetDirectoryNames(serverPath);
                        if (directoryCommand != null)
                        {
                            while (await directoryCommand.MoveNext())
                            {
                                DirectoryName directoryName = directoryCommand.Current;
                                Monitor.Enter(fileLock);
                                try
                                {
                                    waitPaths.Add(new KeyValue<DirectoryInfo, string>(new DirectoryInfo(Path.Combine(clientPath, directoryName.Name)), directoryName.FullName));
                                }
                                finally { Monitor.Exit(fileLock); }
                                if (IsDelete) clientDirectorys.Remove(directoryName.Name);
                            }
                            isSuccess = directoryCommand.ReturnType == CommandClientReturnTypeEnum.Success;
                        }
                        else isSuccess = false;
                        if (isSuccess && IsDelete)
                        {
                            foreach (DirectoryInfo directory in clientDirectorys.Values) await AutoCSer.Common.TryDeleteDirectory(directory);
                        }

                        if (!(isSuccess & isCallSuccess)) onPathError(clientDirectory, serverPath, PullFileStateEnum.CallFail);
                    }
                    catch (Exception exception)
                    {
                        onPathError(clientDirectory, serverPath, PullFileStateEnum.ClientException);
                        await AutoCSer.LogHelper.Exception(exception);
                    }
                    KeyValue<DirectoryInfo, string> path;
                    Monitor.Enter(fileLock);
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
