using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 下载备份文件缓冲区
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct DatabaseBackupDownloadBuffer
    {
        /// <summary>
        /// 缓冲区
        /// </summary>
        public byte[] Buffer;
        /// <summary>
        /// 字节数量
        /// </summary>
        public int Size;
    }
}
