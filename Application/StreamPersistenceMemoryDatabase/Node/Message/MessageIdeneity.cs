using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The identifier of the message being processed
    /// 正在处理的消息标识
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct MessageIdeneity
    {
        /// <summary>
        /// Message unique number (Unique number within the node)
        /// 消息唯一编号（节点内唯一编号）
        /// </summary>
        internal long Identity;
        /// <summary>
        /// The index position of the message array
        /// 消息数组索引位置
        /// </summary>
        internal int ArrayIndex;
        /// <summary>
        /// Message flags
        /// 消息标记
        /// </summary>
        internal MessageFlagsEnum Flags;
        /// <summary>
        /// Set the unique number of the new message
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
