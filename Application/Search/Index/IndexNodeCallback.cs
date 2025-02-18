using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引重新计算匹配数据关键字数据回调
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    internal sealed class IndexNodeCallback<KT, VT> : AutoCSer.Threading.QueueTaskNode
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 关键字索引节点
        /// </summary>
        private readonly IndexNode<KT, VT> node;
        /// <summary>
        /// 关键字索引
        /// </summary>
        private readonly HashSetIndex<KT, VT> index;
        /// <summary>
        /// 匹配数据关键字集合
        /// </summary>
        private readonly VT[] valueArray;
        /// <summary>
        /// 匹配数据关键字集合
        /// </summary>
        private readonly HashSet<VT> values;
        /// <summary>
        /// 索引关键字
        /// </summary>
        private readonly KT key;
        /// <summary>
        /// 关键字索引重新计算匹配数据关键字数据回调
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="valueArray"></param>
        internal IndexNodeCallback(HashSetIndex<KT, VT> index, IndexNode<KT, VT> node, KT key, VT[] valueArray)
        {
            this.node = node;
            this.index = index;
            this.valueArray = valueArray;
            values = new HashSet<VT>(valueArray);
            this.key = key;
        }
        /// <summary>
        /// 重新计算关键字数据
        /// </summary>
        public override void RunTask()
        {
            index.Readed(node, key, values, valueArray);
        }
    }
}
