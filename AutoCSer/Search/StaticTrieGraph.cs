using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的 Trie 图
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public abstract class StaticTrieGraph<T> : IDisposable
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// 子节点
        /// </summary>
        protected AutoCSer.Memory.Pointer nodes;
        /// <summary>
        /// 根节点
        /// </summary>
        protected int boot;
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            CancelBuilder();
            int boot = this.boot;
            if (boot != 0)
            {
                this.boot = 0;
                Monitor.Enter(NodePool.Lock);
                try
                {
                    NodePool.Pool[boot >> ArrayPool.ArraySizeBit][boot & ArrayPool.ArraySizeAnd].Free();
                    NodePool.FreeNoLock(boot);
                }
                finally { Monitor.Exit(NodePool.Lock); }
            }
        }
        /// <summary>
        /// 取消创建
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelBuilder()
        {
            Unmanaged.Free(ref nodes);
        }

        /// <summary>
        /// 节点池
        /// </summary>
        internal static ArrayPool<StaticTrieGraphNode<T>> NodePool;
        /// <summary>
        /// 获取可用节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>节点索引</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetNodeIndex(out StaticTrieGraphNode<T>[] nodes)
        {
            int index = NodePool.GetNoLock(out nodes);
            nodes[index & ArrayPool.ArraySizeAnd].Reset();
            return index;
        }
        static StaticTrieGraph()
        {
            NodePool = new ArrayPool<StaticTrieGraphNode<T>>(256, 1);
            NodePool.Pool[0][0].Nodes = EmptyArray<KeyValue<T, int>>.Array;
        }
    }
}
