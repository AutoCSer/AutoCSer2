using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块信息
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct BlockInfo
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public long StartIndex;
        /// <summary>
        /// 结束位置
        /// </summary>
        public long EndIndex;
#if !AOT
        /// <summary>
        /// 磁盘块信息
        /// </summary>
        /// <param name="block"></param>
        public BlockInfo(Block block) 
        {
            StartIndex = block.StartIndex;
            EndIndex = block.Position;
        }
#endif
    }
}
