using System;
using System.Collections.Generic;
using System.IO;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件最后修改时间
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct FileTime
    {
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime LastWriteTimeUtc;
        /// <summary>
        /// 文件字节数
        /// </summary>
        public long Length;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        /// <param name="file"></param>
        public FileTime(FileInfo file)
        {
            LastWriteTimeUtc = file.LastWriteTimeUtc;
            Length = file.Length;
            FileName = file.Name;
        }

        /// <summary>
        /// 检查返回匹配的文件
        /// </summary>
        /// <param name="checkFileDictionary"></param>
        /// <param name="switchFileDictionary"></param>
        /// <returns></returns>
        internal FileInfo Check(Dictionary<HashString, FileInfo> checkFileDictionary, Dictionary<HashString, FileInfo> switchFileDictionary)
        {
            HashString fileName = FileName;
            FileName = null;
            FileInfo file;
            if (checkFileDictionary != null && checkFileDictionary.TryGetValue(fileName, out file)
                && file.Length == Length && file.LastWriteTimeUtc == LastWriteTimeUtc) return file;
            if (switchFileDictionary != null && switchFileDictionary.TryGetValue(fileName, out file)
                && file.Length == Length && file.LastWriteTimeUtc == LastWriteTimeUtc) return file;
            return null;
        }
    }
}
