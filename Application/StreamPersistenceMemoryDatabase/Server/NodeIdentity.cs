﻿using System;
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
        internal ServerNode Node;
        /// <summary>
        /// 节点标识
        /// </summary>
        private uint identity;
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerNode Get(uint identity)
        {
            return this.identity == identity ? Node : null;
        }
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerNode CheckGet(uint identity)
        {
            if (this.identity == identity)
            {
                ServerNode node = this.Node;
                if (node.Index.Identity == identity) return node;
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
            if (this.identity == (identity | NodeIndex.FreeIdentity))
            {
                this.Node = node;
                this.identity &= (NodeIndex.FreeIdentity ^ uint.MaxValue);
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
        internal ServerNode GetRemove(uint identity)
        {
            if (this.identity == identity)
            {
                ServerNode node = this.Node;
                if ((++this.identity & NodeIndex.FreeIdentity) != 0) this.identity = 0;
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
            return identity |= NodeIndex.FreeIdentity;
        }
        /// <summary>
        /// 判断空闲节点标识是否匹配
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckFreeIdentity(ref uint identity)
        {
            return this.identity == (identity |= NodeIndex.FreeIdentity);
        }
        /// <summary>
        /// 释放空闲节点
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal bool FreeIdentity(uint identity)
        {
            if (this.identity == (identity | NodeIndex.FreeIdentity))
            {
                this.identity -= (NodeIndex.FreeIdentity - 1);
                if ((++this.identity & NodeIndex.FreeIdentity) != 0) this.identity = 0;
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
                this.identity = identity | NodeIndex.FreeIdentity;
                return true;
            }
            return false;
        }
    }
}