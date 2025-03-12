using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct NodeIdentity
    {
        /// <summary>
        /// 服务端节点
        /// </summary>
#if NetStandard21
        internal ServerNode? Node;
#else
        internal ServerNode Node;
#endif
        /// <summary>
        /// 节点标识
        /// </summary>
        internal uint Identity;
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerNode? Get(uint identity)
#else
        internal ServerNode Get(uint identity)
#endif
        {
            return this.Identity == identity ? Node : null;
        }
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerNode? CheckGet(uint identity)
#else
        internal ServerNode CheckGet(uint identity)
#endif
        {
            if (this.Identity == identity)
            {
                var node = this.Node;
                if (node != null && node.Index.Identity == identity) return node;
            }
            return null;
        }
        /// <summary>
        /// 设置节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Set(ServerNode node, uint identity)
        {
            if (this.Identity == (identity | NodeIndex.FreeIdentity))
            {
                this.Node = node;
                this.Identity &= (NodeIndex.FreeIdentity ^ uint.MaxValue);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerNode? GetRemove(uint identity)
#else
        internal ServerNode GetRemove(uint identity)
#endif
        {
            if (this.Identity == identity)
            {
                var node = this.Node;
                if ((++this.Identity & NodeIndex.FreeIdentity) != 0) this.Identity = 0;
                this.Node = null;
                return node;
            }
            return null;
        }
        /// <summary>
        /// 设置空闲节点标识，用于创建节点预申请
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint GetFreeIdentity()
        {
            return Identity |= NodeIndex.FreeIdentity;
        }
        /// <summary>
        /// 设置空闲节点标识，用于创建节点预申请
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint GetCreateIdentity()
        {
            uint identity = Identity;
            Identity |= NodeIndex.FreeIdentity;
            return identity;
        }
        /// <summary>
        /// 判断空闲节点标识是否匹配
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckFreeIdentity(ref uint identity)
        {
            return this.Identity == (identity |= NodeIndex.FreeIdentity);
        }
        /// <summary>
        /// 释放空闲节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal bool FreeIdentity(uint identity)
        {
            if (this.Identity == (identity | NodeIndex.FreeIdentity))
            {
                this.Identity -= (NodeIndex.FreeIdentity - 1);
                if ((++this.Identity & NodeIndex.FreeIdentity) != 0) this.Identity = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 初始化创建节点设置节点标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool SetFreeIdentity(uint identity)
        {
            if (Node == null)
            {
                this.Identity = identity | NodeIndex.FreeIdentity;
                return true;
            }
            return false;
        }
    }
}
