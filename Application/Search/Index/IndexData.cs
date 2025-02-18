using AutoCSer.CommandService.DiskBlock;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 索引数据
    /// </summary>
    /// <typeparam name="T">数据关键字类型</typeparam>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct IndexData<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 数据关键字集合的磁盘块索引信息
        /// </summary>
        internal readonly BlockIndex BlockIndex;
        /// <summary>
        /// 磁盘块索引信息中的数据关键字数量
        /// </summary>
        internal readonly int BlockIndexValueCount;
        /// <summary>
        /// 数据关键字集合中新增数据关键字数量
        /// </summary>
        internal readonly int ValueCount;
        /// <summary>
        /// 数据关键字集合
        /// </summary>
        internal readonly T[] Values;
        /// <summary>
        /// 索引数据
        /// </summary>
        /// <param name="index"></param>
        internal IndexData(HashSetIndex<T> index)
        {
            BlockIndex = index.BlockIndex;
            BlockIndexValueCount = index.BlockIndexValueCount;
            ValueCount = index.ValueCount;
            Values = index.GetValueArray();
        }
    }
}
