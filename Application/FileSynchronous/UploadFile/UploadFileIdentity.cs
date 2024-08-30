using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 上传文件标识
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct UploadFileIdentity
    {
        /// <summary>
        /// 写入文件流
        /// </summary>
        internal FileStream FileStream;
        /// <summary>
        /// 写入文件信息
        /// </summary>
        private FileInfo backupFile;
        /// <summary>
        /// 文件信息
        /// </summary>
        private FileInfo file;
        /// <summary>
        /// 文件长度
        /// </summary>
        private long fileLength;
        /// <summary>
        /// 最后修改日期
        /// </summary>
        private DateTime lastWriteTime;
        /// <summary>
        /// 上传文件标识
        /// </summary>
        private uint identity;
        /// <summary>
        /// 设置上传文件信息
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="backupFile"></param>
        /// <param name="file"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint Set(FileStream fileStream, FileInfo backupFile, FileInfo file, ref SynchronousFileInfo fileInfo)
        {
            FileStream = fileStream;
            this.backupFile = backupFile;
            this.file = file;
            fileLength = fileInfo.Length;
            lastWriteTime = fileInfo.LastWriteTime;
            return identity;
        }
        /// <summary>
        /// 获取写入文件流
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fileLength"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal FileStream Get(uint identity, out long fileLength)
        {
            fileLength = this.fileLength;
            return identity == this.identity ? FileStream : null;
        }
        /// <summary>
        /// 文件上传完成
        /// </summary>
        /// <param name="backupFile"></param>
        /// <returns></returns>
        internal FileInfo Completed(out FileInfo backupFile)
        {
            ++identity;
            FileStream.Dispose();
            backupFile = this.backupFile;
            this.backupFile.LastWriteTimeUtc = lastWriteTime;
            FileStream = null;
            return file;
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="backupFile"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        internal FileStream Remove(uint identity, out FileInfo backupFile, out DateTime lastWriteTime)
        {
            if (this.identity == identity)
            {
                FileStream fileStream = FileStream;
                backupFile = this.backupFile;
                lastWriteTime = this.lastWriteTime;
                ++this.identity;
                return fileStream;
            }
            backupFile = null;
            lastWriteTime = default(DateTime);
            return null;
        }
        /// <summary>
        /// 设置最后修改日期
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLastWriteTime()
        {
            backupFile.LastWriteTimeUtc = lastWriteTime;
        }
    }
}
