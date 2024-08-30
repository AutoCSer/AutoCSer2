using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 初始化上传文件结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CreateUploadFileResult
    {
        /// <summary>
        /// 当前文件位置
        /// </summary>
        public long Length;
        /// <summary>
        /// 服务端文件流索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 上传文件缓冲区字节大小
        /// </summary>
        public int BufferSize;

        /// <summary>
        /// 设置当前文件位置
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        internal void Set(long length, int index)
        {
            Length = length;
            Index = index;
            BufferSize = UploadFileBuffer.DeployTaskConfig.UploadFileBufferSize;
        }
    }
}
