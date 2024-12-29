using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点索引信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct NodeIndex : IEquatable<NodeIndex>
    {
        /// <summary>
        /// 空闲节点标识
        /// </summary>
        internal const uint FreeIdentity = 0x80000000U;

        /// <summary>
        /// 节点索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 节点标识
        /// </summary>
        internal uint Identity;
        /// <summary>
        /// 节点索引信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="identity"></param>
        internal NodeIndex(int index, uint identity)
        {
            Index = index;
            Identity = identity;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="state"></param>
        public NodeIndex(CallStateEnum state)
        {
            Index = -(int)(byte)state;
            Identity = 0;
        }
        /// <summary>
        /// 获取节点空闲标记
        /// </summary>
        /// <returns>是否空闲节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetFree()
        {
            if ((Identity & FreeIdentity) == 0) return false;
            Identity &= (FreeIdentity ^ uint.MaxValue);
            return true;
        }
        /// <summary>
        /// 获取调用状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CallStateEnum GetState()
        {
            return Index >= 0 ? CallStateEnum.Success : (CallStateEnum)(byte)-Index;
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(NodeIndex other)
        {
            return (((uint)Index ^ (uint)other.Index) | (Identity ^ other.Identity)) == 0;
        }
    }
}
