using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.FileSynchronous
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
        internal string Name;
        /// <summary>
        /// 文件名称（包含路径）
        /// </summary>
        internal string FullName;
        /// <summary>
        /// 文件长度
        /// </summary>
        internal long Length;
        /// <summary>
        /// 最后修改日期
        /// </summary>
        internal DateTime LastWriteTime;
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="fileInfo"></param>
        internal SynchronousFileInfo(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;
            Length = fileInfo.Length;
            LastWriteTime = fileInfo.LastWriteTimeUtc;
        }
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="path"></param>
        internal SynchronousFileInfo(FileInfo fileInfo, string path)
        {
            Name = fileInfo.Name;
            FullName = fileInfo.FullName.Substring(path.Length);
            Length = fileInfo.Length;
            LastWriteTime = fileInfo.LastWriteTimeUtc;
        }
        /// <summary>
        /// 设置客户端文件长度
        /// </summary>
        /// <param name="length">文件长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLength(long length)
        {
            Name = null;
            Length = length;
        }
        /// <summary>
        /// 设置上传文件长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long SetUploadLength(long length)
        {
            long value = Length;
            Length = length;
            return value;
        }
        /// <summary>
        /// 设置上传文件最后修改日期
        /// </summary>
        /// <param name="lastWriteTime"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLastWriteTime(DateTime lastWriteTime)
        {
            if (LastWriteTime != lastWriteTime)
            {
                Length = 0;
                LastWriteTime = lastWriteTime;
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
            FullName = serverFileName;
            LastWriteTime = clientFile.LastWriteTime;
        }
    }
}
