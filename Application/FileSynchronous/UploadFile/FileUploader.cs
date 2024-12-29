using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传
    /// </summary>
    internal sealed class FileUploader : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private readonly UploadFileService service;
        /// <summary>
        /// 上传索引与路径信息
        /// </summary>
        internal UploaderInfo UploaderInfo;
        /// <summary>
        /// 上传操作访问锁
        /// </summary>
        private readonly object uploadLock;
        /// <summary>
        /// 上传文件集合
        /// </summary>
        private UploadFileIdentity[] files;
        /// <summary>
        /// 最后一次上传操作时间戳
        /// </summary>
        private long uploadTimestamp;
        /// <summary>
        /// 空闲文件上传索引集合
        /// </summary>
        private LeftArray<int> freeIndexs;
        /// <summary>
        /// 当前分配的上传文件索引
        /// </summary>
        private int fileIndex;
        /// <summary>
        /// 上传完成文件集合
        /// </summary>
        private LeftArray<KeyValue<FileInfo, FileInfo>> uploadCompletedFiles;
        /// <summary>
        /// 等待删除的文件集合
        /// </summary>
        private LeftArray<KeyValue<FileInfo, FileInfo>> deleteFiles;
        /// <summary>
        /// 等待删除的目录集合
        /// </summary>
        private LeftArray<KeyValue<DirectoryInfo, string>> deleteDirectorys;
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="service">文件上传服务</param>
        /// <param name="directory">上传根目录</param>
        /// <param name="backupDirectory">上传备份根目录</param>
        internal FileUploader(UploadFileService service, DirectoryInfo directory, DirectoryInfo backupDirectory)
            : base(SecondTimer.TaskArray, service.TimeoutSeconds, SecondTimerTaskThreadModeEnum.Synchronous, SecondTimerKeepModeEnum.After, service.TimeoutSeconds)
        {
            this.service = service;
            UploaderInfo.Set(directory, backupDirectory);
            uploadLock = new object();
            freeIndexs.SetEmpty();
            uploadCompletedFiles.SetEmpty();
            deleteFiles.SetEmpty();
            deleteDirectorys.SetEmpty();
            files = new UploadFileIdentity[16];
            if (service.AppendUploader(this))
            {
                uploadTimestamp = Stopwatch.GetTimestamp();
                TryAppendTaskArray();
            }
        }
        /// <summary>
        /// 检查上传目录是否冲突
        /// </summary>
        /// <param name="uploaderInfo"></param>
        /// <returns></returns>
        internal UploadFileStateEnum CheckCreate(ref UploaderInfo uploaderInfo)
        {
            if (uploadTimestamp + service.TimeoutTimestamp > Stopwatch.GetTimestamp())
            {
                if (UploaderInfo.Path != uploaderInfo.Path)
                {
                    return UploaderInfo.BackupPath != uploaderInfo.BackupPath ? UploadFileStateEnum.Success : UploadFileStateEnum.NotSupportBackupPath;
                }
                if (UploaderInfo.BackupPath != uploaderInfo.BackupPath) return UploadFileStateEnum.PathUploading;
            }
            service.RemoveUploader(this).NotWait();
            return UploadFileStateEnum.Success;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        internal async Task<CommandServerSendOnly> Free()
        {
            KeepSeconds = 0;
            while (fileIndex != 0)
            {
                Monitor.Enter(uploadLock);
                if (fileIndex != 0)
                {
                    UploadFileIdentity uploadFile = files[--fileIndex];
                    Monitor.Exit(uploadLock);
                    if (uploadFile.FileStream != null)
                    {
                        try
                        {
                            await uploadFile.FileStream.DisposeAsync();
                            uploadFile.SetLastWriteTime();
                        }
                        catch { }
                    }
                }
                else Monitor.Exit(uploadLock);
            }
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal override void OnTimer() 
        {
            if (uploadTimestamp + service.TimeoutTimestamp <= Stopwatch.GetTimestamp()) service.RemoveUploader(this).NotWait();
        }
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取文件信息集合回调委托</param>
        /// <returns></returns>
        internal async Task GetFiles(string path, CommandServerKeepCallbackCount<UploadFileInfo> callback)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            string backupPath = UploaderInfo.BackupPath.notNull();
            DirectoryInfo backupDirectory = new DirectoryInfo(string.IsNullOrEmpty(path) ? backupPath : System.IO.Path.Combine(backupPath, path));
            var backupFiles = default(Dictionary<string, FileInfo>);
            if (await AutoCSer.Common.DirectoryExists(backupDirectory))
            {
                FileInfo[] files = await AutoCSer.Common.DirectoryGetFiles(backupDirectory);
                if (files.Length != 0)
                {
                    backupFiles = DictionaryCreator.CreateAny<string, FileInfo>(files.Length);
                    foreach (FileInfo file in files)
                    {
                        backupFiles.Add(file.Name, file);
                    }
                }
            }
            string uploadPath = UploaderInfo.Path.notNull();
            DirectoryInfo directory = new DirectoryInfo(string.IsNullOrEmpty(path) ? uploadPath : System.IO.Path.Combine(uploadPath, path));
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                var backupFile = default(FileInfo);
                foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory))
                {
                    if (backupFiles != null && backupFiles.Remove(file.Name, out backupFile))
                    {
                        if (!await callback.CallbackAsync(new UploadFileInfo(backupFile, backupPath, true, true)))
                        {
                            uploadTimestamp = Stopwatch.GetTimestamp();
                            return;
                        }
                    }
                    else
                    {
                        if (!await callback.CallbackAsync(new UploadFileInfo(file, uploadPath, true, false)))
                        {
                            uploadTimestamp = Stopwatch.GetTimestamp();
                            return;
                        }
                    }
                }
            }
            if (backupFiles != null)
            {
                foreach (FileInfo file in backupFiles.Values)
                {
                    if (!await callback.CallbackAsync(new UploadFileInfo(file, backupPath, false, true)))
                    {
                        uploadTimestamp = Stopwatch.GetTimestamp();
                        return;
                    }
                }
            }
            uploadTimestamp = Stopwatch.GetTimestamp();
        }
        /// <summary>
        /// 获取指定文件信息集合
        /// </summary>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        internal async Task<UploadFileInfo> GetFile(string fileName)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            string backupPath = UploaderInfo.BackupPath.notNull();
            FileInfo backupFile = new FileInfo(System.IO.Path.Combine(backupPath, fileName)), file = new FileInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), fileName));
            bool isBackupFile = await AutoCSer.Common.FileExists(backupFile), isFile = await AutoCSer.Common.FileExists(file);
            return new UploadFileInfo(backupFile, backupPath, isFile, isBackupFile);
        }
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        internal async Task GetDirectoryNames(string path, CommandServerKeepCallbackCount<DirectoryName> callback)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            string uploadPath = UploaderInfo.Path.notNull();
            DirectoryInfo directory = new DirectoryInfo(string.IsNullOrEmpty(path) ? uploadPath : System.IO.Path.Combine(uploadPath, path));
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                foreach (DirectoryInfo directoryInfo in await AutoCSer.Common.GetDirectories(directory))
                {
                    if (!await callback.CallbackAsync(new DirectoryName(directoryInfo, uploadPath)))
                    {
                        uploadTimestamp = Stopwatch.GetTimestamp();
                        return;
                    }
                }
            }
            uploadTimestamp = Stopwatch.GetTimestamp();
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task CreateDirectory(string path)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            return AutoCSer.Common.TryCreateDirectory(new DirectoryInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), path)));
        }
        /// <summary>
        /// 创建上传文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="serverFileLength"></param>
        /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
        internal async Task<UploadFileIndex> CreateFile(SynchronousFileInfo fileInfo, long serverFileLength)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            bool isCreate = false;
            var writeStream = default(FileStream);
            FileInfo backupFile = new FileInfo(System.IO.Path.Combine(UploaderInfo.BackupPath.notNull(), fileInfo.FullName));
            try
            {
                bool isFile = await AutoCSer.Common.FileExists(backupFile);
                if (isFile)
                {
                    if (backupFile.LastWriteTimeUtc != fileInfo.LastWriteTime || backupFile.Length != serverFileLength)
                    {
                        await AutoCSer.Common.DeleteFile(backupFile);
                        if (serverFileLength == 0) isFile = false;
                        else return new UploadFileIndex(UploadFileStateEnum.FileInfoNotMatch);
                    }
                    else
                    {
                        writeStream = await AutoCSer.Common.CreateFileStream(backupFile.FullName, FileMode.Open, FileAccess.Write);
                        if (writeStream.Length == serverFileLength && new FileInfo(backupFile.FullName).LastWriteTimeUtc == fileInfo.LastWriteTime)
                        {
                            if (serverFileLength != 0) await AutoCSer.Common.Seek(writeStream, 0, SeekOrigin.End);
                        }
                        else
                        {
                            await writeStream.DisposeAsync();
                            writeStream = null;
                            await AutoCSer.Common.DeleteFile(backupFile);
                            if (serverFileLength == 0) isFile = false;
                            else return new UploadFileIndex(UploadFileStateEnum.FileInfoNotMatch);
                        }
                    }
                }
                else await AutoCSer.Common.TryCreateDirectory(backupFile.Directory.notNull());
                if (!isFile)
                {
                    writeStream = await AutoCSer.Common.CreateFileStream(backupFile.FullName, FileMode.CreateNew, FileAccess.Write);
                    if(fileInfo.Length == 0)
                    {
                        await writeStream.DisposeAsync();
                        writeStream = null;
                        backupFile.LastWriteTimeUtc = fileInfo.LastWriteTime;
                        await appendCompletedFile(fileInfo, backupFile);
                        return default(UploadFileIndex);
                    }
                }

                FileInfo file = new FileInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), fileInfo.FullName));
                await AutoCSer.Common.TryCreateDirectory(file.Directory.notNull());

                uint identity;
                int index;
                Monitor.Enter(uploadLock);
                if (!freeIndexs.TryPop(out index))
                {
                    if (fileIndex != files.Length) index = fileIndex++;
                    else
                    {
                        try
                        {
                            files = AutoCSer.Common.GetCopyArray(files, fileIndex << 1);
                            identity = files[index = fileIndex++].Set(writeStream.notNull(), backupFile, file, ref fileInfo);
                        }
                        finally { Monitor.Exit(uploadLock); }
                        isCreate = true;
                        return new UploadFileIndex(index, identity);
                    }
                }
                identity = files[index].Set(writeStream.notNull(), backupFile, file, ref fileInfo);
                Monitor.Exit(uploadLock);
                isCreate = true;
                return new UploadFileIndex(index, identity);
            }
            finally
            {
                if (writeStream != null && !isCreate) await writeStream.DisposeAsync();
                uploadTimestamp = Stopwatch.GetTimestamp();
            }
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        internal async Task<CommandServerSendOnly> RemoveFile(UploadFileIndex fileIndex)
        {
            int index = fileIndex.Index;
            if ((uint)index < (uint)this.fileIndex)
            {
                var backupFile = default(FileInfo);
                DateTime lastWriteTime;
                Monitor.Enter(uploadLock);
                var fileStream = files[index].Remove(fileIndex.Identity, out backupFile, out lastWriteTime);
                if (fileStream != null)
                {
                    if (freeIndexs.TryAdd(index))
                    {
                        Monitor.Exit(uploadLock);
                        await fileStream.DisposeAsync();
                        backupFile.notNull().LastWriteTimeUtc = lastWriteTime;
                    }
                    else
                    {
                        try
                        {
                            freeIndexs.Add(index);
                        }
                        finally
                        {
                            Monitor.Exit(uploadLock);
                            await fileStream.DisposeAsync();
                            backupFile.notNull().LastWriteTimeUtc = lastWriteTime;
                        }
                    }
                }
                else Monitor.Exit(uploadLock);
            }
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// 上传文件写入数据
        /// </summary>
        /// <param name="buffer">文件上传数据</param>
        /// <returns></returns>
        internal UploadFileStateEnum UploadFileData(UploadFileBuffer buffer)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            int index = buffer.FileIndex.Index;
            if ((uint)index < (uint)this.fileIndex)
            {
                long fileLength;
                Monitor.Enter(uploadLock);
                var fileStream = files[index].Get(buffer.FileIndex.Identity, out fileLength);
                Monitor.Exit(uploadLock);
                if (fileStream != null)
                {
                    try
                    {
                        fileStream.Write(buffer.Buffer.Array, buffer.Buffer.Start, buffer.Buffer.Length);
                        if (fileStream.Length == fileLength)
                        {
                            FileInfo backupFile;
                            appendCompletedFile(files[index].Completed(out backupFile), backupFile);
                            Monitor.Enter(uploadLock);
                            if (freeIndexs.TryAdd(index)) Monitor.Exit(uploadLock);
                            else
                            {
                                try
                                {
                                    freeIndexs.Add(index);
                                }
                                finally { Monitor.Exit(uploadLock); }
                            }
                        }
                        uploadTimestamp = Stopwatch.GetTimestamp();
                        return UploadFileStateEnum.Success;
                    }
                    catch(Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    uploadTimestamp = Stopwatch.GetTimestamp();
                    return UploadFileStateEnum.ServerException;
                }
            }
            return UploadFileStateEnum.NotFoundUploadFile;
        }
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        internal async Task<UploadFileInfo> AppendCompletedFile(SynchronousFileInfo fileInfo)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            FileInfo backupFile = new FileInfo(System.IO.Path.Combine(UploaderInfo.BackupPath.notNull(), fileInfo.FullName));
            if(await AutoCSer.Common.FileExists(backupFile))
            {
                if (backupFile.LastWriteTimeUtc == fileInfo.LastWriteTime && backupFile.Length == fileInfo.Length)
                {
                    await appendCompletedFile(fileInfo, backupFile);
                    return new UploadFileInfo { State = UploadFileStateEnum.Success };
                }
                return new UploadFileInfo(backupFile, UploaderInfo.BackupPath.notNull(), false, true);
            }
            return default(UploadFileInfo);
        }
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="backupFile"></param>
        /// <returns></returns>
        private async Task appendCompletedFile(SynchronousFileInfo fileInfo, FileInfo backupFile)
        {
            FileInfo file = new FileInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), fileInfo.FullName));
            await AutoCSer.Common.TryCreateDirectory(file.Directory.notNull());
            appendCompletedFile(file, backupFile);
            uploadTimestamp = Stopwatch.GetTimestamp();
        }
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="backupFile"></param>
        private void appendCompletedFile(FileInfo file, FileInfo backupFile)
        {
            KeyValue<FileInfo, FileInfo> uploadCompletedFile = new KeyValue<FileInfo, FileInfo>(file, backupFile);
            Monitor.Enter(uploadLock);
            if (uploadCompletedFiles.TryAdd(uploadCompletedFile)) Monitor.Exit(uploadLock);
            else
            {
                try
                {
                    uploadCompletedFiles.Add(uploadCompletedFile);
                }
                finally { Monitor.Exit(uploadLock); }
            }
        }
        /// <summary>
        /// 添加待删除文件
        /// </summary>
        /// <param name="fileName">相对路径文件名称</param>
        /// <returns></returns>
        internal async Task<bool> AppendDeleteFile(string fileName)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            FileInfo file = new FileInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), fileName));
            if (await AutoCSer.Common.FileExists(file))
            {
                FileInfo backupFile = new FileInfo(System.IO.Path.Combine(UploaderInfo.BackupPath.notNull(), fileName));
                if (!await AutoCSer.Common.TryDeleteFile(backupFile)) await AutoCSer.Common.TryCreateDirectory(backupFile.Directory.notNull());
                KeyValue<FileInfo, FileInfo> deleteFile = new KeyValue<FileInfo, FileInfo>(file, backupFile);
                Monitor.Enter(uploadLock);
                if (deleteFiles.TryAdd(deleteFile)) Monitor.Exit(uploadLock);
                else
                {
                    try
                    {
                        deleteFiles.Add(deleteFile);
                    }
                    finally { Monitor.Exit(uploadLock); }
                }
            }
            uploadTimestamp = Stopwatch.GetTimestamp();
            return true;
        }
        /// <summary>
        /// 添加待删除目录
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        internal async Task<bool> AppendDeleteDirectory(string path)
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            DirectoryInfo directory = new DirectoryInfo(System.IO.Path.Combine(UploaderInfo.Path.notNull(), path));
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                DirectoryInfo backupDirectory = new DirectoryInfo(System.IO.Path.Combine(UploaderInfo.BackupPath.notNull(), path));
                await AutoCSer.Common.TryDeleteDirectory(backupDirectory);
                KeyValue<DirectoryInfo, string> deleteDirectory = new KeyValue<DirectoryInfo, string>(directory, backupDirectory.FullName);
                Monitor.Enter(uploadLock);
                if (deleteDirectorys.TryAdd(deleteDirectory)) Monitor.Exit(uploadLock);
                else
                {
                    try
                    {
                        deleteDirectorys.Add(deleteDirectory);
                    }
                    finally { Monitor.Exit(uploadLock); }
                }
            }
            uploadTimestamp = Stopwatch.GetTimestamp();
            return true;
        }
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Completed()
        {
            uploadTimestamp = Stopwatch.GetTimestamp();
            Monitor.Enter(uploadLock);
            for(int index = fileIndex; index != 0;)
            {
                if(files[--index].FileStream != null)
                {
                    Monitor.Exit(uploadLock);
                    return false;
                }
            }
            KeyValue<FileInfo, FileInfo>[] uploadCompletedFileArray = uploadCompletedFiles.Array;
            KeyValue<FileInfo, FileInfo>[] deleteFileArray = deleteFiles.Array;
            KeyValue<DirectoryInfo, string>[] deleteDirectoryArray = deleteDirectorys.Array;
            int uploadCompletedFileCount = uploadCompletedFiles.Length, deleteFileCount = deleteFiles.Length, deleteDirectoryCount = deleteDirectorys.Length;
            uploadCompletedFiles.SetEmpty();
            deleteFiles.SetEmpty();
            deleteDirectorys.SetEmpty();
            Monitor.Exit(uploadLock);

            uploadTimestamp = Stopwatch.GetTimestamp();
            while (uploadCompletedFileCount != 0)
            {
                KeyValue<FileInfo, FileInfo> uploadFile = uploadCompletedFileArray[--uploadCompletedFileCount];
                if (await AutoCSer.Common.FileExists(uploadFile.Key))
                {
                    string fileName = uploadFile.Key.FullName, backupFileName = uploadFile.Value.FullName, movBbackupFileName = backupFileName + AutoCSer.Threading.SecondTimer.Now.ToString(".yyyyMMddHHmmss") + ".bak";
                    await AutoCSer.Common.FileMove(uploadFile.Key, movBbackupFileName);
                    await AutoCSer.Common.FileMove(uploadFile.Value, fileName);
                    await AutoCSer.Common.FileMove(movBbackupFileName, backupFileName);
                }
                else
                {
                    await AutoCSer.Common.FileMove(uploadFile.Value, uploadFile.Key.FullName);
                }
            }
            while (deleteFileCount != 0)
            {
                KeyValue<FileInfo, FileInfo> deleteFile = deleteFileArray[--deleteFileCount];
                await AutoCSer.Common.FileMove(deleteFile.Key, deleteFile.Value.FullName);
            }
            while (deleteDirectoryCount != 0)
            {
                KeyValue<DirectoryInfo, string> deleteDirectory = deleteDirectoryArray[--deleteDirectoryCount];
                await AutoCSer.Common.DirectoryMove(deleteDirectory.Key, deleteDirectory.Value);
            }
            return true;
        }
    }
}
