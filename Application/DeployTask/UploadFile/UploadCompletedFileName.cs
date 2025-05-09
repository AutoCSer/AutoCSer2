using System;
using System.IO;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传完成文件名称信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct UploadCompletedFileName
    {
        /// <summary>
        /// 目标文件名称
        /// </summary>
        internal readonly string FileName;
        /// <summary>
        /// 备份文件名称
        /// </summary>
        internal readonly string BackupFileName;
        /// <summary>
        /// 文件上传完成文件名称信息
        /// </summary>
        /// <param name="files"></param>
        internal UploadCompletedFileName(KeyValue<FileInfo, FileInfo> files)
        {
            FileName = files.Key.FullName;
            BackupFileName = files.Value.FullName;
        }
        /// <summary>
        /// 文件上传完成文件名称信息
        /// </summary>
        /// <param name="directorys"></param>
        internal UploadCompletedFileName(KeyValue<DirectoryInfo, string> directorys)
        {
            FileName = directorys.Key.FullName;
            BackupFileName = directorys.Value;
        }
    }
}
