using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
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
        /// 创建文件上传操作对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">上传根目录</param>
        /// <param name="backupPath">备份文件根目录</param>
        /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
        public virtual async Task<UploaderInfo> CreateUploader(CommandServerSocket socket, string path, string backupPath)
        {
            DirectoryInfo directory = new DirectoryInfo(path), backupDirectory;
            await AutoCSer.Common.TryCreateDirectory(directory);
            if (string.IsNullOrEmpty(backupPath))
            {
                backupDirectory = new DirectoryInfo(Path.Combine(this.backupPath, AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMddHHmmss.") + ((uint)Interlocked.Increment(ref backupDirectoryIdentity)).toHex()));
                await AutoCSer.Common.TryCreateDirectory(backupDirectory);
                return new FileUploader(this, directory, backupDirectory).UploaderInfo;
            }
            backupDirectory = new DirectoryInfo(backupPath);
            if (backupDirectory.Parent.notNull().FullName == this.backupPath && await AutoCSer.Common.DirectoryExists(backupDirectory))
            {
                return new FileUploader(this, directory, backupDirectory).UploaderInfo;
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
        public virtual async Task<CommandServerSendOnly> RemoveUploader(CommandServerSocket socket, UploadFileIndex uploaderIndex)
        {
            if ((uint)uploaderIndex.Index < (uint)this.uploaderIndex)
            {
                Monitor.Enter(uploaderLock);
                var fileUploader = uploaders[uploaderIndex.Index].Get(uploaderIndex.Identity);
                if (fileUploader != null)
                {
                    removeUploader(uploaderIndex.Index);
                    await fileUploader.Free();
                    return CommandServerSendOnly.Null;
                }
                Monitor.Exit(uploaderLock);
            }
            return CommandServerSendOnly.Null;
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
        internal async Task RemoveUploader(FileUploader fileUploader)
        {
            UploadFileIndex index = fileUploader.UploaderInfo.Index;
            Monitor.Enter(uploaderLock);
            if (object.ReferenceEquals(uploaders[index.Index].Get(index.Identity), fileUploader))
            {
                removeUploader(index.Index);
                await fileUploader.Free();
            }
            else Monitor.Exit(uploaderLock);
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
        public virtual async Task GetFiles(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<UploadFileInfo> callback)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) await uploader.GetFiles(path, callback);
        }
        /// <summary>
        /// 获取指定文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        public virtual async Task<UploadFileInfo> GetFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) await uploader.GetFile(fileName);
            return new UploadFileInfo { State = UploadFileStateEnum.NotFoundUploader };
        }
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        public virtual async Task GetDirectoryNames(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<DirectoryName> callback)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) await uploader.GetDirectoryNames(path, callback);
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
        public virtual async Task<UploadFileIndex> CreateFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo, long serverFileLength)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return await uploader.CreateFile(fileInfo, serverFileLength);
            return new UploadFileIndex(UploadFileStateEnum.NotFoundUploader);
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">文件上传索引信息</param>
        /// <param name="fileIndex">上传文件索引信息</param>
        /// <returns></returns>
        public virtual async Task<CommandServerSendOnly> RemoveFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, UploadFileIndex fileIndex)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) await uploader.RemoveFile(fileIndex);
            return CommandServerSendOnly.Null;
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
        public virtual async Task<UploadFileInfo> AppendCompletedFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null) return await uploader.AppendCompletedFile(fileInfo);
            return new UploadFileInfo { State = UploadFileStateEnum.NotFoundUploader };
        }
        /// <summary>
        /// 添加待删除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径文件名称</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        public virtual async Task<bool> AppendDeleteFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null)
            {
                await uploader.AppendDeleteFile(fileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加待删除目录
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        public virtual async Task<bool> AppendDeleteDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path)
        {
            var uploader = getFileUploader(uploaderIndex);
            if (uploader != null)
            {
                await uploader.AppendDeleteDirectory(path);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
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
    }
}
