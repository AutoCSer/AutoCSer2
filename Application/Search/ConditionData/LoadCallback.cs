using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
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
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    internal sealed class LoadCallback<NT, KT, VT, CT> : QueueTaskNode
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IConditionData
#else
        where KT : IEquatable<KT>
        where VT : IConditionData
#endif
        where CT : class
    {
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        private readonly ConditionDataNode<NT, KT, VT, CT> node;
        /// <summary>
        /// 创建非索引条件查询数据返回参数集合
        /// </summary>
        internal readonly ResponseParameterAwaiter<ConditionDataUpdateStateEnum>[] CreateResponses;
        /// <summary>
        /// 数据关键字集合
        /// </summary>
        internal KT[] Keys;
        /// <summary>
        /// 回调等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim waitLock;
        /// <summary>
        /// 新关键字数量
        /// </summary>
        internal int NewCount;
        /// <summary>
        /// 初始化加载数据回调
        /// </summary>
        /// <param name="node">非索引条件查询数据节点</param>
        /// <param name="keys">数据关键字集合</param>
        internal LoadCallback(ConditionDataNode<NT, KT, VT, CT> node, KT[] keys)
        {
            this.node = node;
            Keys = keys;
            CreateResponses = new ResponseParameterAwaiter<ConditionDataUpdateStateEnum>[keys.Length];
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
                foreach (KT key in Keys)
                {
                    if (!node.ContainsKey(key)) Keys[index++] = key;
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
}
