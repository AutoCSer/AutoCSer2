using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct BlockInfo
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public long StartIndex;
        /// <summary>
        /// 结束位置
        /// </summary>
        public long EndIndex;
        /// <summary>
        /// 磁盘块信息
        /// </summary>
        /// <param name="block"></param>
        public BlockInfo(Block block) 
        {
            StartIndex = block.StartIndex;
            EndIndex = block.Position;
        }
    }
}
