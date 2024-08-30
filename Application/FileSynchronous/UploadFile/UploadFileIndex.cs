using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 上传文件索引信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct UploadFileIndex
    {
        /// <summary>
        /// 上传文件索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 上传文件标识
        /// </summary>
        internal uint Identity;
        /// <summary>
        /// 上传文件索引信息
        /// </summary>
        /// <param name="index">上传文件索引</param>
        /// <param name="identity">上传文件标识</param>
        internal UploadFileIndex(int index, uint identity)
        {
            Index = index;
            Identity = identity;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="state"></param>
        internal UploadFileIndex(UploadFileStateEnum state)
        {
            Index = -(int)(byte)state;
            Identity = 0;
        }
    }
}
