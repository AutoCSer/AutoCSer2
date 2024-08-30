using System;
using System.IO;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 目录名称
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct DirectoryName
    {
        /// <summary>
        /// 目录名称
        /// </summary>
        internal string Name;
        /// <summary>
        /// 目录名称（包含路径）
        /// </summary>
        internal string FullName;
        /// <summary>
        /// 目录名称
        /// </summary>
        /// <param name="directoryInfo"></param>
        internal DirectoryName(DirectoryInfo directoryInfo)
        {
            Name = directoryInfo.Name;
            FullName = directoryInfo.FullName;
        }
        /// <summary>
        /// 目录名称
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="path"></param>
        internal DirectoryName(DirectoryInfo directoryInfo, string path)
        {
            Name = directoryInfo.Name;
            FullName = directoryInfo.FullName.Substring(path.Length);
        }
    }
}
