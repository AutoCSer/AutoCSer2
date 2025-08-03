using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CollectionExtensions<T>
    {
        /// <summary>
        /// 集合
        /// </summary>
        private readonly ICollection<T> collection;
        /// <summary>
        /// 集合相关扩展
        /// </summary>
        /// <param name="collection"></param>
        public CollectionExtensions(ICollection<T> collection)
        {
            this.collection = collection;
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns>null为0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int Count()
        {
            return collection.count();
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray()
        {
            return collection.getArray();
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LeftArray<VT> GetLeftArray<VT>(Func<T, VT> getValue)
        {
            return collection.getLeftArray(getValue);
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT[] GetArray<VT>(Func<T, VT> getValue)
        {
            return collection.getArray(getValue);
        }
        /// <summary>
        /// 根据集合内容返回单向动态数组
        /// </summary>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>单向动态数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ListArray<VT> GetListArray<VT>(Func<T, VT> getValue)
        {
            return collection.getListArray(getValue);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>匹配数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public VT[] GetFindArrayNotNull<VT>(Func<T, VT?> getValue)
#else
        public VT[] GetFindArrayNotNull<VT>(Func<T, VT> getValue)
#endif
            where VT : class
        {
            return collection.getFindArrayNotNull(getValue);
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="KT">哈希键值类型</typeparam>
        /// <param name="getKey">键值获取器</param>
        /// <returns>Dictionary</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Dictionary<KT, T> getDictionary<KT>(Func<T, KT> getKey) where KT : IEquatable<KT>
        {
            return collection.getDictionary(getKey);
        }
    }
}
