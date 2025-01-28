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
    public struct UploadFileInfo
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        internal SynchronousFileInfo FileInfo;
        /// <summary>
        /// 是否备份文件
        /// </summary>
        internal bool IsBackup;
        /// <summary>
        /// 文件是否存在
        /// </summary>
        internal bool IsFile;
        /// <summary>
        /// 文件上传状态
        /// </summary>
        internal UploadFileStateEnum State;
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="path"></param>
        /// <param name="isFile"></param>
        /// <param name="isBackup"></param>
        internal UploadFileInfo(FileInfo fileInfo, string path, bool isFile, bool isBackup)
        {
            FileInfo = new SynchronousFileInfo(fileInfo, path);
            IsFile = isFile;
            IsBackup = isBackup;
            State = UploadFileStateEnum.Success;
        }
    }
}
