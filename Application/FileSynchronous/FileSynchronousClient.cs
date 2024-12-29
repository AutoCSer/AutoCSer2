using AutoCSer.CommandService.FileSynchronous;
using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 文件同步客户端
    /// </summary>
    public abstract class FileSynchronousClient
    {
        /// <summary>
        /// 上传文件集合访问锁
        /// </summary>
        protected readonly object fileLock;
        /// <summary>
        /// 文件同步访问锁
        /// </summary>
        protected AutoCSer.Threading.SemaphoreSlimLock synchronousLock;
        /// <summary>
        /// 同步完成等待锁
        /// </summary>
        protected AutoCSer.Threading.SemaphoreSlimLock completedLock;
        /// <summary>
        /// 未同步路径集合
        /// </summary>
        protected LeftArray<KeyValue<DirectoryInfo, string>> waitPaths;
        /// <summary>
        /// 同步目录处理客户端文件集合
        /// </summary>
        protected ReusableDictionary<string, FileInfo> clientFiles;
        /// <summary>
        /// 同步目录处理客户端目录集合
        /// </summary>
        protected ReusableDictionary<string, DirectoryInfo> clientDirectorys;
        /// <summary>
        /// 正在同步的文件数量
        /// </summary>
        protected int synchronousFileCount;
        /// <summary>
        /// 是否正在处理同步目录
        /// </summary>
        protected int isSynchronousPath;
        /// <summary>
        /// 目录同步错误次数
        /// </summary>
        protected int pathErrorCount;
        /// <summary>
        /// 文件同步错误次数
        /// </summary>
        protected int fileErrorCount;
        /// <summary>
        /// 是否同步删除不存在的文件与目录
        /// </summary>
        internal readonly bool IsDelete;
        /// <summary>
        /// 文件同步客户端
        /// </summary>
        /// <param name="isDelete">是否同步删除不存在的文件与目录</param>
        protected FileSynchronousClient(bool isDelete)
        {
            this.IsDelete = isDelete;
            fileLock = new object();
            synchronousLock = new AutoCSer.Threading.SemaphoreSlimLock(1, 1);
            completedLock = new AutoCSer.Threading.SemaphoreSlimLock(0, 1);
            clientFiles = new ReusableDictionary<string, FileInfo>();
            clientDirectorys = new ReusableDictionary<string, DirectoryInfo>();
            waitPaths.SetEmpty();
        }
        /// <summary>
        /// 文件同步完成
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        protected virtual void onCompleted(FileInfo clientFile, string serverFileName) { }
    }
    /// <summary>
    /// 文件同步客户端
    /// </summary>
    /// <typeparam name="FT">同步文件处理类型</typeparam>
    /// <typeparam name="ST">同步状态类型</typeparam>
    public abstract class FileSynchronousClient<FT, ST> : FileSynchronousClient
        where FT : SynchronousFile
        where ST : struct
    {
        /// <summary>
        /// 正在同步的文件
        /// </summary>
#if NetStandard21
        protected readonly FT?[] files;
#else
        protected readonly FT[] files;
#endif
        /// <summary>
        /// 未同步文件集合
        /// </summary>
        protected LeftArray<FT> waitFiles;
        /// <summary>
        /// 最后一次文件同步错误状态
        /// </summary>
        protected ST synchronousState;
        /// <summary>
        /// 最后一次文件同步错误状态
        /// </summary>
        public ST SynchronousState { get { return synchronousState; } }
        /// <summary>
        /// 文件同步客户端
        /// </summary>
        /// <param name="isDelete">是否同步删除不存在的文件与目录</param>
        /// <param name="concurrency">同步并发数</param>
        protected FileSynchronousClient(bool isDelete, int concurrency) : base(isDelete)
        {
            files = new FT[Math.Max(concurrency, 1)];
            waitFiles.SetEmpty();
        }
        /// <summary>
        /// 同步目录处理
        /// </summary>
        /// <param name="clientDirectory"></param>
        /// <param name="serverPath"></param>
        /// <returns></returns>
        protected abstract Task synchronous(DirectoryInfo clientDirectory, string serverPath);
        /// <summary>
        /// 添加客户端同步文件
        /// </summary>
        /// <param name="file"></param>
        protected void appendFile(FT file)
        {
            Monitor.Enter(fileLock);
            if (synchronousFileCount != files.Length)
            {
                file.FileIndex = synchronousFileCount;
                files[synchronousFileCount++] = file;
                Monitor.Exit(fileLock);
                file.Synchronous().NotWait();
            }
            else
            {
                try
                {
                    waitFiles.Add(file);
                }
                finally { Monitor.Exit(fileLock); }
            }
        }
        /// <summary>
        /// 文件上传完成
        /// </summary>
        /// <param name="file"></param>
        internal void Completed(FT file)
        {
            if (file.FileIndex >= 0)
            {
                var nextUploadFile = default(FT);
                KeyValue<DirectoryInfo, string> directory = default(KeyValue<DirectoryInfo, string>);
                Monitor.Enter(fileLock);
                int index = file.FileIndex;
                if (waitFiles.TryPop(out nextUploadFile))
                {
                    nextUploadFile.FileIndex = index;
                    files[index] = nextUploadFile;
                    Monitor.Exit(fileLock);
                    nextUploadFile.Synchronous().NotWait();
                }
                else
                {
                    if (index != --synchronousFileCount)
                    {
                        nextUploadFile = files[synchronousFileCount].notNull();
                        nextUploadFile.FileIndex = index;
                        files[index] = nextUploadFile;
                    }
                    files[synchronousFileCount] = null;
                    if (Interlocked.CompareExchange(ref isSynchronousPath, 1, 0) == 0 && !waitPaths.TryPop(out directory))
                    {
                        if (synchronousFileCount == 0) completedLock.Exit();
                        else Interlocked.Exchange(ref isSynchronousPath, 0);
                    }
                    Monitor.Exit(fileLock);
                    if (directory.Key != null) synchronous(directory.Key, directory.Value.notNull()).NotWait();
                }
            }
            onCompleted(file.ClientFile, file.FileInfo.FullName);
        }
        /// <summary>
        /// 目录上传错误计数
        /// </summary>
        /// <param name="state">错误状态</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onPathError(ST state)
        {
            Interlocked.Increment(ref pathErrorCount);
            synchronousState = state;
        }
        /// <summary>
        /// 目录上传错误
        /// </summary>
        /// <param name="clientDirectory">客户端目录信息</param>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="state">错误状态</param>
        protected virtual void onPathError(DirectoryInfo clientDirectory, string serverPath, ST state)
        {
            onPathError(state);
        }
        /// <summary>
        /// 文件上传错误计数
        /// </summary>
        /// <param name="state">错误状态</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onFileError(ST state)
        {
            Interlocked.Increment(ref fileErrorCount);
            synchronousState = state;
        }
        /// <summary>
        /// 文件上传错误
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        /// <param name="state">错误状态</param>
        protected internal virtual void onFileError(FileInfo clientFile, string serverFileName, ST state)
        {
            onFileError(state);
        }
    }
}
