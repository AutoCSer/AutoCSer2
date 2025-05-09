using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 上传索引与路径信息
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct UploaderInfo
    {
        /// <summary>
        /// 上传文件索引信息
        /// </summary>
        internal UploadFileIndex Index;
        /// <summary>
        /// 上传根路径
        /// </summary>
#if NetStandard21
        internal string? Path;
#else
        internal string Path;
#endif
        /// <summary>
        /// 上传备份根路径
        /// </summary>
#if NetStandard21
        internal string? BackupPath;
#else
        internal string BackupPath;
#endif
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
