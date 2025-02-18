using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引磁盘块索引信息写入失败回调
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    internal sealed class HashSetIndexCallback<KT, VT> : AutoCSer.Threading.QueueTaskNode
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 关键字索引
        /// </summary>
        private readonly HashSetIndex<KT, VT> index;
        /// <summary>
        /// 关键字索引磁盘块索引信息写入失败回调
        /// </summary>
        /// <param name="index">关键字索引</param>
        internal HashSetIndexCallback(HashSetIndex<KT, VT> index)
        {
            this.index = index;
        }
        /// <summary>
        /// 磁盘块索引信息写入失败回调
        /// </summary>
        public override void RunTask()
        {
            index.WriteError();
        }
    }
}
