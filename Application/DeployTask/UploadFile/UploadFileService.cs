using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    public class UploadFileService : IUploadFileService
    {
        /// <summary>
        /// 备份目录编号
        /// </summary>
        private static int backupDirectoryIdentity;

        /// <summary>
        /// 备份文件目录
        /// </summary>
        private readonly string backupPath;
        /// <summary>
        /// 文件上传操作超时时间戳
        /// </summary>
        internal readonly long TimeoutTimestamp;
        /// <summary>
        /// 文件上传集合
        /// </summary>
        private UploaderIdentity[] uploaders;
        /// <summary>
        /// 文件上传集合访问锁
        /// </summary>
        private readonly object uploaderLock;
        /// <summary>
        /// 空闲文件上传索引集合
        /// </summary>
        private LeftArray<int> freeIndexs;
        /// <summary>
        /// 文件上传操作超时检查秒数
        /// </summary>
        internal readonly int TimeoutSeconds;
        /// <summary>
        /// 当前分配的文件上传索引
        /// </summary>
        private int uploaderIndex;
        /// <summary>
        /// 文件上传服务
        /// </summary>
        /// <param name="config">文件上传服务端配置</param>
        public UploadFileService(UploadFileServiceConfig config)
        {
            TimeoutSeconds = Math.Max(config.TimeoutSeconds, 1);
            TimeoutTimestamp = AutoCSer.Date.GetTimestampBySeconds(TimeoutSeconds);
            backupPath = new DirectoryInfo(config.BackupPath).FullName;
            uploaders = new UploaderIdentity[Math.Max(config.Capacity, 1)];
            uploaderLock = new object();
            freeIndexs.SetEmpty();
        }
        /// <summary>
        /// 根据上传类型获取文件上传路径
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="type">上传类型</param>
        /// <returns></returns>
        public virtual string GetPath(CommandServerSocket socket, string type)
        {
            return Path.Combine(AutoCSer.Common.ApplicationDirectory.FullName, type);
        }
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string CombinePath(string left, string right)
        {
            return Path.Combine(left, right);
        }
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="pathArray"></param>
        /// <returns></returns>
        public string CombinePathArray(string[] pathArray)
        {
            return Path.Combine(pathArray);
        }
        /// <summary>
        /// 创建文件上传操作对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">上传根目录</param>
        /// <param name="backupPath">备份文件根目录</param>
        /// <param name="extensions">扩展名称集合，扩展名称包括小数点，比如 .txt</param>
        /// <param name="isCaseExtension">扩展名匹配是否区分大小写</param>
        /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
        public virtual async Task<UploaderInfo> CreateUploader(CommandServerSocket socket, string path, string backupPath, string[] extensions, bool isCaseExtension)
        {
            DirectoryInfo directory = new DirectoryInfo(path), backupDirectory;
            await AutoCSer.Common.TryCreateDirectory(directory);
            if (string.IsNullOrEmpty(backupPath))
            {
                backupDirectory = new DirectoryInfo(Path.Combine(this.backupPath, AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMddHHmmss.") + ((uint)Interlocked.Increment(ref backupDirectoryIdentity)).toHex()));
                await AutoCSer.Common.TryCreateDirectory(backupDirectory);
                return new FileUploader(this, directory, backupDirectory, extensions, isCaseExtension).UploaderInfo;
            }
            backupDirectory = new DirectoryInfo(backupPath);
            if (backupDirectory.Parent.notNull().FullName == this.backupPath && await AutoCSer.Common.DirectoryExists(backupDirectory))
            {
                return new FileUploader(this, directory, backupDirectory, extensions, isCaseExtension).UploaderInfo;
            }
            return new UploaderInfo(UploadFileStateEnum.NotSupportBackupPath);
        }
        /// <summary>
        /// 添加文件上传操作对象
        /// </summary>
        /// <param name="fileUploader"></param>
        /// <returns></returns>
        internal bool AppendUploader(FileUploader fileUploader)
        {
            Monitor.Enter(uploaderLock);
            int index = uploaderIndex;
            while (index != 0)
            {
                var checkFileUploader = uploaders[--index].Uploader;
                if (checkFileUploader != null)
                {
                    UploadFileStateEnum state = checkFileUploader.CheckCreate(ref fileUploader.UploaderInfo);
                    if (state != UploadFileStateEnum.Success)
                    {
                        Monitor.Exit(uploaderLock);
                        fileUploader.UploaderInfo.Index.Index = -(int)(byte)state;
                        return false;
                    }
                }
            }
            if (!freeIndexs.TryPop(out index))
            {
                if (uploaderIndex != uploaders.Length) index = uploaderIndex++;
                else
                {
                    try
                    {
                        uploaders = AutoCSer.Common.GetCopyArray(uploaders, uploaderIndex << 1);
                        uploaders[uploaderIndex].Set(fileUploader);
                        fileUploader.UploaderInfo.Index.Index = uploaderIndex++;
                    }
                    finally { Monitor.Exit(uploaderLock); }
                    return true;
                }
            }
            uploaders[index].Set(fileUploader);
            fileUploader.UploaderInfo.Index.Index = index;
            Monitor.Exit(uploaderLock);
            return true;
        }
        /// <summary>
        /// 移除文件上传实例
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns></returns>
        public virtual Task<CommandServerSendOnly> RemoveUploader(CommandServerSocket socket, UploadFileIndex uploaderIndex)
        {
            if ((uint)uploaderIndex.Index < (uint)this.uploaderIndex)
            {
                Monitor.Enter(uploaderLock);
                var fileUploader = uploaders[uploaderIndex.Index].Get(uploaderIndex.Identity);
                if (fileUploader != null)
                {
                    removeUploader(uploaderIndex.Index);
                    return fileUploader.Free();
                }
                Monitor.Exit(uploaderLock);
            }
            return CommandServerSendOnly.NullTask;
        }
        /// <summary>
        /// 移除文件上传实例
        /// </summary>
        /// <param name="uploaderIndex"></param>
        private void removeUploader(int uploaderIndex)
        {
            if (freeIndexs.TryAdd(uploaderIndex))
            {
                uploaders[uploaderIndex].Free();
                Monitor.Exit(uploaderLock);
            }
            else
            {
                try
                {
                    freeIndexs.Add(uploaderIndex);
                    uploaders[uploaderIndex].Free();
                }
                finally { Monitor.Exit(uploaderLock); }
            }
        }
        /// <summary>
        /// 移除文件上传实例
        /// </summary>
        /// <param name="fileUploader"></param>
        /// <returns></returns>
        internal Task RemoveUploader(FileUploader fileUploader)
        {
            UploadFileIndex index = fileUploader.UploaderInfo.Index;
            Monitor.Enter(uploaderLock);
            if (object.ReferenceEquals(uploaders[index.Index].Get(index.Identity), fileUploader))
            {
                removeUploader(index.Index);
                return fileUploader.Free();
            }
            Monitor.Exit(uploaderLock);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 获取文件上传操作对象
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns></returns>
#if NetStandard21
        private FileUploader? getFileUploader(UploadFileIndex uploaderIndex)
#else
        private FileUploader getFileUploader(UploadFileIndex uploaderIndex)
#endif
        {
            if ((uint)uploaderIndex.Index < (uint)this.uploaderIndex)
            {
                Monitor.Enter(uploaderLock);
                var uploader = uploaders[uploaderIndex.Index].Get(uploaderIndex.Identity);
                Monitor.Exit(uploaderLock);
                return uploader;
            }
            return null;
        }
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取文件信息集合回调委托</param>
        /// <returns></returns>
        public virtual Task GetFiles(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<UploadFileInfo> callback)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.GetFiles(path, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 获取指定文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        public virtual Task<UploadFileInfo> GetFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.GetFile(fileName);
            return Task.FromResult(new UploadFileInfo { State = UploadFileStateEnum.NotFoundUploader });
        }
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        public virtual Task GetDirectoryNames(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<DirectoryName> callback)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.GetDirectoryNames(path, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="directoryName">目录名称</param>
        /// <returns>返回 null 表示失败</returns>
#if NetStandard21
        public virtual async Task<string?> CreateDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, string directoryName)
#else
        public virtual async Task<string> CreateDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, string directoryName)
#endif
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null)
            {
                path = string.IsNullOrEmpty(path) ? directoryName : Path.Combine(path, directoryName);
                await uploader.CreateDirectory(path);
                return path;
            }
            return null;
        }
        /// <summary>
        /// 创建上传文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="serverFileLength">服务端文件匹配长度</param>
        /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
        public virtual Task<UploadFileIndex> CreateFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo, long serverFileLength)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.CreateFile(fileInfo, serverFileLength);
            return Task.FromResult(new UploadFileIndex(UploadFileStateEnum.NotFoundUploader));
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">文件上传索引信息</param>
        /// <param name="fileIndex">上传文件索引信息</param>
        /// <returns></returns>
        public virtual Task<CommandServerSendOnly> RemoveFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, UploadFileIndex fileIndex)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.RemoveFile(fileIndex);
            return CommandServerSendOnly.NullTask;
        }
        /// <summary>
        /// 上传文件写入数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public virtual UploadFileStateEnum UploadFileData(CommandServerSocket socket, UploadFileBuffer buffer)
        {
            return buffer.State;
        }
        /// <summary>
        /// 上传文件写入数据
        /// </summary>
        /// <param name="buffer">文件上传数据</param>
        /// <returns></returns>
        internal UploadFileStateEnum UploadFileData(UploadFileBuffer buffer)
        {
            var uploader = getFileUploader(buffer.UploaderIndex);
            if (uploader != null) return uploader.UploadFileData(buffer);
            return UploadFileStateEnum.NotFoundUploader;
        }
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">对比文件信息</param>
        /// <returns></returns>
        public virtual Task<UploadFileInfo> AppendCompletedFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.AppendCompletedFile(fileInfo);
            return Task.FromResult(new UploadFileInfo { State = UploadFileStateEnum.NotFoundUploader });
        }
        /// <summary>
        /// 添加待删除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径文件名称</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        public virtual Task<bool> AppendDeleteFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.AppendDeleteFile(fileName);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 添加待删除目录
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        public virtual Task<bool> AppendDeleteDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return uploader.AppendDeleteDirectory(path);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象，或者上传未结束</returns>
        public virtual async Task<bool> Completed(CommandServerSocket socket, UploadFileIndex uploaderIndex)
        {
            var fileUploader = getFileUploader(uploaderIndex);
            if (fileUploader != null && await fileUploader.Completed())
            {
                Monitor.Enter(uploaderLock);
                if (object.ReferenceEquals(uploaders[uploaderIndex.Index].Get(uploaderIndex.Identity), fileUploader))
                {
                    removeUploader(uploaderIndex.Index);
                    await fileUploader.Free();
                }
                else Monitor.Exit(uploaderLock);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 将上传完成操作数据写入文件并返回文件名称
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回空字符串表示没有找到文件上传操作对象，或者上传未结束</returns>
        public virtual async Task<string> GetCompletedFileName(CommandServerSocket socket, UploadFileIndex uploaderIndex)
        {
            var fileUploader = getFileUploader(uploaderIndex);
            if (fileUploader != null)
            {
                string fileName = await fileUploader.GetUploadCompletedFile();
                if (fileName.Length != 0)
                {
                    Monitor.Enter(uploaderLock);
                    if (object.ReferenceEquals(uploaders[uploaderIndex.Index].Get(uploaderIndex.Identity), fileUploader))
                    {
                        removeUploader(uploaderIndex.Index);
                        await fileUploader.Free();
                    }
                    else Monitor.Exit(uploaderLock);
                    return fileName;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取切换进程的上传目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">默认上传目录</param>
        /// <param name="switchPath">默认切换进程上传目录</param>
        /// <param name="fileName">切换进程文件相对路径</param>
        /// <returns></returns>
        public virtual async Task<SynchronousFileInfo> GetSwitchProcessPathFileInfo(CommandServerSocket socket, string path, string switchPath, string fileName)
        {
            if (await AutoCSer.Common.TryCreateDirectory(path)) return default(SynchronousFileInfo);
            FileInfo fileInfo = new FileInfo(Path.Combine(path, fileName));
            if (!await AutoCSer.Common.FileExists(fileInfo)) return default(SynchronousFileInfo);
            if (await AutoCSer.Common.TryCreateDirectory(switchPath)) return new SynchronousFileInfo(switchPath, fileInfo);
            FileInfo switchFileInfo = new FileInfo(Path.Combine(switchPath, fileName));
            if (!await AutoCSer.Common.FileExists(switchFileInfo) || switchFileInfo.LastWriteTimeUtc < fileInfo.LastWriteTimeUtc) return new SynchronousFileInfo(switchPath, fileInfo);
            if (switchFileInfo.LastWriteTimeUtc == fileInfo.LastWriteTimeUtc && switchFileInfo.Length < fileInfo.Length) return new SynchronousFileInfo(switchPath, fileInfo);
            return new SynchronousFileInfo(path, switchFileInfo);
        }
    }
}
