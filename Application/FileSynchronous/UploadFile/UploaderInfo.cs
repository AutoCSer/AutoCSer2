using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 上传索引与路径信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct UploaderInfo
    {
        /// <summary>
        /// 上传文件索引信息
        /// </summary>
        internal UploadFileIndex Index;
        /// <summary>
        /// 上传根路径
        /// </summary>
        internal string Path;
        /// <summary>
        /// 上传备份根路径
        /// </summary>
        internal string BackupPath;
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="state"></param>
        internal UploaderInfo(UploadFileStateEnum state)
        {
            Index = new UploadFileIndex(state);
            Path = BackupPath = null;
        }
        /// <summary>
        /// 设置目录信息
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="backupDirectory"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(DirectoryInfo directory, DirectoryInfo backupDirectory)
        {
            Path = directory.fullName();
            BackupPath = backupDirectory.fullName();
        }
    }
}
