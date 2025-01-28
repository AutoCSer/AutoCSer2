using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct SynchronousFileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
#if NetStandard21
        internal string? Name;
#else
        internal string Name;
#endif
        /// <summary>
        /// 文件名称（包含路径）/ 上传目录
        /// </summary>
        private string fullName;
        /// <summary>
        /// 文件名称（包含路径）/ 上传目录
        /// </summary>
        public string FullName { get { return fullName; } }
        /// <summary>
        /// 文件长度
        /// </summary>
        private long length;
        /// <summary>
        /// 文件长度
        /// </summary>
        public long Length { get { return length; } }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        private DateTime lastWriteTime;
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime LastWriteTime { get { return lastWriteTime; } }
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="fileInfo"></param>
        internal SynchronousFileInfo(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            this.fullName = fileInfo.FullName;
            this.length = fileInfo.Length;
            lastWriteTime = fileInfo.LastWriteTimeUtc;
        }
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="path"></param>
        internal SynchronousFileInfo(FileInfo fileInfo, string path)
        {
            Name = fileInfo.Name;
            this.fullName = fileInfo.FullName.Substring(path.Length);
            this.length = fileInfo.Length;
            lastWriteTime = fileInfo.LastWriteTimeUtc;
        }
        /// <summary>
        /// 上传目录与文件信息
        /// </summary>
        /// <param name="uploadPath">上传目录</param>
        /// <param name="fileInfo">最后修改的切换进程文件信息</param>
        internal SynchronousFileInfo(string uploadPath, FileInfo fileInfo)
        {
            Name = null;
            this.fullName = uploadPath;
            this.length = fileInfo.Length;
            lastWriteTime = fileInfo.LastWriteTimeUtc;
        }
        /// <summary>
        /// 设置客户端文件长度
        /// </summary>
        /// <param name="length">文件长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLength(long length)
        {
            Name = null;
            this.length = length;
        }
        /// <summary>
        /// 设置上传文件长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long SetUploadLength(long length)
        {
            long value = this.length;
            this.length = length;
            return value;
        }
        /// <summary>
        /// 设置上传文件最后修改日期
        /// </summary>
        /// <param name="lastWriteTime"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLastWriteTime(DateTime lastWriteTime)
        {
            if (this.lastWriteTime != lastWriteTime)
            {
                this.length = 0;
                this.lastWriteTime = lastWriteTime;
            }
        }
        /// <summary>
        /// 设置上传文件信息
        /// </summary>
        /// <param name="clientFile"></param>
        /// <param name="serverFileName"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetUpload(FileInfo clientFile, string serverFileName)
        {
            Name = clientFile.Name;
            this.fullName = serverFileName;
            this.lastWriteTime = clientFile.LastWriteTime;
        }
    }
}
