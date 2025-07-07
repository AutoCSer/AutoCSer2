using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.ConditionData
{
    /// <summary>
    /// 初始化加载数据回调
    /// </summary>
    /// <typeparam name="NT">接口类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    internal abstract class LoadCallback<NT, KT, VT> : ReadWriteQueueNode
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        private readonly ConditionDataNode<NT, KT, VT> node;
        /// <summary>
        /// The data collection
        /// 数据集合
        /// </summary>
        internal VT[] Values;
        /// <summary>
        /// 回调等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim waitLock;
        /// <summary>
        /// 新数据数量
        /// </summary>
        internal int NewCount;
        /// <summary>
        /// 初始化加载数据回调
        /// </summary>
        /// <param name="node">非索引条件查询数据节点</param>
        /// <param name="values">Data collection
        /// 数据集合</param>
        internal LoadCallback(ConditionDataNode<NT, KT, VT> node, VT[] values)
        {
            this.node = node;
            Values = values;
            waitLock = new System.Threading.SemaphoreSlim(0, 1);
        }
        /// <summary>
        /// 检查关键字
        /// </summary>
        public override void RunTask()
        {
            NewCount = -1;
            try
            {
                int index = 0;
                foreach (VT value in Values)
                {
                    if (!node.Contains(value)) Values[index++] = value;
                }
                NewCount = index;
            }
            finally { waitLock.Release(); }
        }
        /// <summary>
        /// 等待关键字检查完成
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task Wait()
        {
            return waitLock.WaitAsync();
        }
    }
    /// <summary>
    /// 初始化加载数据回调
    /// </summary>
    /// <typeparam name="NT">接口类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    internal sealed class LoadCallback<NT, KT, VT, CT> : LoadCallback<NT, KT, VT>
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
        where CT : class
    {
        /// <summary>
        /// 创建非索引条件查询数据返回参数集合
        /// </summary>
        internal readonly ResponseResultAwaiter[] CreateResponses;
        /// <summary>
        /// 初始化加载数据回调
        /// </summary>
        /// <param name="node">非索引条件查询数据节点</param>
        /// <param name="values">Data collection
        /// 数据集合</param>
        internal LoadCallback(ConditionDataNode<NT, KT, VT> node, VT[] values) : base(node, values)
        {
            CreateResponses = new ResponseResultAwaiter[values.Length];
        }
    }
}
