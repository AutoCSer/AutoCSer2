using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 正在处理的消息标识
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct MessageIdeneity
    {
        /// <summary>
        /// 消息唯一编号（节点内唯一编号）
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 消息数组索引位置
        /// </summary>
        internal int ArrayIndex;
        /// <summary>
        /// 消息标记
        /// </summary>
        internal MessageFlagsEnum Flags;
        /// <summary>
        /// 设置新消息唯一编号
        /// </summary>
        /// <param name="currentIdentity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNew(long currentIdentity)
        {
            Identity = currentIdentity;
            Flags = MessageFlagsEnum.None;
        }
    }
}
