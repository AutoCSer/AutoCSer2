using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.ConditionData
{
    /// <summary>
    /// 初始化加载数据回调
    /// </summary>
    /// <typeparam name="NT">接口类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    internal sealed class LocalLoadCallback<NT, KT, VT, CT> : LoadCallback<NT, KT, VT>
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
        internal readonly LocalServiceQueueNode<LocalResult>[] CreateResponses;
        /// <summary>
        /// 初始化加载数据回调
        /// </summary>
        /// <param name="node">非索引条件查询数据节点</param>
        /// <param name="values">Data collection
        /// 数据集合</param>
        internal LocalLoadCallback(ConditionDataNode<NT, KT, VT> node, VT[] values) : base(node, values)
        {
            CreateResponses = new LocalServiceQueueNode<LocalResult>[values.Length];
        }
    }
}
