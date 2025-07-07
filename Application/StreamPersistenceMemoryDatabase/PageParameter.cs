using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public abstract class PageParameter : AutoCSer.Data.PageParameter
    {
        /// <summary>
        /// 获取分页数组
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public PageArray<T> GetPageArray<T>()
        {
            return new PageArray<T>(this);
        }
        /// <summary>
        /// 获取空数据
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public PageResult<T> GetPageResult<T>()
        {
            return new PageResult<T>(EmptyArray<T>.Array, 0, PageIndex, PageSize);
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="KT">分页数据关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="getKey">获取分页数据关键字委托</param>
        /// <returns>已排序关键字数据</returns>
        public PageResult<KT> GetPageResult<T, KT>(LeftArray<T> array, Func<T, T, int> comparer, Func<T, KT> getKey)
        {
            KT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0) keyArray = array.GetQuickRangeSort(comparer, (int)startIndex, Math.Min((int)count, PageSize)).GetArray(getKey);
            else keyArray = EmptyArray<KT>.Array;
            return new PageResult<KT>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<int> GetPageResult(LeftArray<int> array)
        {
            int[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0)
            {
                array.QuickRangeSort((int)startIndex, (int)count);
                keyArray = AutoCSer.Common.GetUninitializedArray<int>((int)count);
                Array.Copy(array.Array, (int)startIndex, keyArray, 0, keyArray.Length);
            }
            else keyArray = EmptyArray<int>.Array;
            return new PageResult<int>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get the reverse page data
        /// 获取逆序分页数据
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<int> GetDescPageResult(LeftArray<int> array)
        {
            int[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0)
            {
                array.QuickRangeSortDesc((int)startIndex, (int)count);
                keyArray = AutoCSer.Common.GetUninitializedArray<int>((int)count);
                Array.Copy(array.Array, (int)startIndex, keyArray, 0, keyArray.Length);
            }
            else keyArray = EmptyArray<int>.Array;
            return new PageResult<int>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取分页数据关键字委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<int> GetPageResult<T>(LeftArray<T> array, Func<T, int> getKey)
        {
            int[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0) keyArray = array.GetQuickRangeSortKay((int)startIndex, Math.Min((int)count, PageSize), getKey);
            else keyArray = EmptyArray<int>.Array;
            return new PageResult<int>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get the reverse page data
        /// 获取逆序分页数据
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取分页数据关键字委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<int> GetDescPageResult<T>(LeftArray<T> array, Func<T, int> getKey)
        {
            int[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0) keyArray = array.GetQuickRangeSortKayDesc((int)startIndex, (int)count, getKey);
            else keyArray = EmptyArray<int>.Array;
            return new PageResult<int>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取分页数据关键字委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<long> GetPageResult<T>(LeftArray<T> array, Func<T, long> getKey)
        {
            long[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0) keyArray = array.GetQuickRangeSortKayDesc((int)startIndex, (int)count, getKey);
            else keyArray = EmptyArray<long>.Array;
            return new PageResult<long>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// Get the reverse page data
        /// 获取逆序分页数据
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取分页数据关键字委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<long> GetDescPageResult<T>(LeftArray<T> array, Func<T, long> getKey)
        {
            long[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(array.Length - startIndex, PageSize);
            if (count > 0) keyArray = array.GetQuickRangeSortKay((int)startIndex, (int)count, getKey);
            else keyArray = EmptyArray<long>.Array;
            return new PageResult<long>(keyArray, array.Length, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字分页数据
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="dictionary">二叉搜索树字典</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<KT> GetKeyPageResult<KT, VT>(AutoCSer.SearchTree.Dictionary<KT, VT> dictionary)
            where KT : IComparable<KT>
        {
            KT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(dictionary.Count - startIndex, PageSize);
            if (count > 0) keyArray = dictionary.GetKeyRange((int)startIndex, (int)count);
            else keyArray = EmptyArray<KT>.Array;
            return new PageResult<KT>(keyArray, dictionary.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字逆序分页数据
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="dictionary">二叉搜索树字典</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<KT> GetDescKeyPageResult<KT, VT>(AutoCSer.SearchTree.Dictionary<KT, VT> dictionary)
            where KT : IComparable<KT>
        {
            KT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(dictionary.Count - startIndex, PageSize);
            if (count > 0) keyArray = dictionary.GetKeyRangeDesc((int)startIndex, (int)count);
            else keyArray = EmptyArray<KT>.Array;
            return new PageResult<KT>(keyArray, dictionary.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字分页数据
        /// </summary>
        /// <typeparam name="T">Keyword type
        /// 关键字类型</typeparam>
        /// <param name="tree">二叉搜索树集合</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<T> GetPageResult<T>(AutoCSer.SearchTree.Set<T> tree)
            where T : IComparable<T>
        {
            T[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(tree.Count - startIndex, PageSize);
            if (count > 0) keyArray = tree.GetRange((int)startIndex, (int)count);
            else keyArray = EmptyArray<T>.Array;
            return new PageResult<T>(keyArray, tree.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字分页数据
        /// </summary>
        /// <typeparam name="T">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">返回值数据类型</typeparam>
        /// <param name="tree">二叉搜索树集合</param>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<VT> GetPageResult<T, VT>(AutoCSer.SearchTree.Set<T> tree, Func<T, VT> getValue)
            where T : IComparable<T>
        {
            VT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(tree.Count - startIndex, PageSize);
            if (count > 0) keyArray = tree.GetRange((int)startIndex, (int)count, getValue);
            else keyArray = EmptyArray<VT>.Array;
            return new PageResult<VT>(keyArray, tree.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字逆序分页数据
        /// </summary>
        /// <typeparam name="T">Keyword type
        /// 关键字类型</typeparam>
        /// <param name="tree">二叉搜索树集合</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<T> GetDescPageResult<T>(AutoCSer.SearchTree.Set<T> tree)
            where T : IComparable<T>
        {
            T[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(tree.Count - startIndex, PageSize);
            if (count > 0) keyArray = tree.GetRangeDesc((int)startIndex, (int)count);
            else keyArray = EmptyArray<T>.Array;
            return new PageResult<T>(keyArray, tree.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字逆序分页数据
        /// </summary>
        /// <typeparam name="T">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">返回值数据类型</typeparam>
        /// <param name="tree">二叉搜索树集合</param>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<VT> GetDescPageResult<T, VT>(AutoCSer.SearchTree.Set<T> tree, Func<T, VT> getValue)
            where T : IComparable<T>
        {
            VT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(tree.Count - startIndex, PageSize);
            if (count > 0) keyArray = tree.GetRangeDesc((int)startIndex, (int)count, getValue);
            else keyArray = EmptyArray<VT>.Array;
            return new PageResult<VT>(keyArray, tree.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字分页数据
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="dictionary">二叉搜索树字典</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<KT> GetKeyPageResult<KT, VT>(AutoCSer.SearchTree.NodeDictionary<KT, VT> dictionary)
            where KT : IComparable<KT>
            where VT : AutoCSer.SearchTree.Node<VT, KT>
        {
            KT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(dictionary.Count - startIndex, PageSize);
            if (count > 0) keyArray = dictionary.GetKeyRange((int)startIndex, (int)count);
            else keyArray = EmptyArray<KT>.Array;
            return new PageResult<KT>(keyArray, dictionary.Count, PageIndex, PageSize);
        }
        /// <summary>
        /// 获取关键字逆序分页数据
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="dictionary">二叉搜索树字典</param>
        /// <returns>排序关键字集合</returns>
        public PageResult<KT> GetDescKeyPageResult<KT, VT>(AutoCSer.SearchTree.NodeDictionary<KT, VT> dictionary)
            where KT : IComparable<KT>
            where VT : AutoCSer.SearchTree.Node<VT, KT>
        {
            KT[] keyArray;
            long startIndex = (long)PageIndex * PageSize, count = Math.Min(dictionary.Count - startIndex, PageSize);
            if (count > 0) keyArray = dictionary.GetKeyRangeDesc((int)startIndex, (int)count);
            else keyArray = EmptyArray<KT>.Array;
            return new PageResult<KT>(keyArray, dictionary.Count, PageIndex, PageSize);
        }
    }
}
