using AutoCSer.Configuration;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// Binary search tree dictionary
    /// 二叉搜索树字典
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">数据节点类型</typeparam>
    [RemoteType]
    public sealed class NodeDictionary<KT, VT>
        where KT : IComparable<KT>
        where VT : Node<VT, KT>
    {
        /// <summary>
        /// Root node
        /// 根节点
        /// </summary>
#if NetStandard21
        internal VT? Boot;
#else
        internal VT Boot;
#endif
        /// <summary>
        /// Number of nodes
        /// 节点数量
        /// </summary>
        public int Count
        {
            get { return Boot != null ? Boot.Count : 0; }
        }
        /// <summary>
        /// Get the tree height has a time complexity of O(n)
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        public int Height
        {
            get
            {
                return Boot == null ? 0 : Boot.Height;
            }
        }
        /// <summary>
        /// The data collection
        /// 数据集合
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                return Boot != null && Boot.Count != 0 ? Boot.Nodes : EmptyArray<VT>.Array;
            }
        }
        /// <summary>
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        public VT FristValue
        {
            get
            {
                if (Boot != null) return Boot.FristNode;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        public VT LastValue
        {
            get
            {
                if (Boot != null) return Boot.LastNode;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Get or set data based on keyword
        /// 根据关键字获取或者设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>数据,获取失败KeyNotFoundException</returns>
        public VT this[KT key]
        {
            get
            {
                if (Boot != null)
                {
                    var node = Boot.Get(key);
                    if (node != null) return node;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set { Set(value); }
        }
        /// <summary>
        /// 二叉树字典
        /// </summary>
        public NodeDictionary() { }
        /// <summary>
        /// Clear the data
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Boot = null;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
        public bool Set(VT value)
        {
            if (value.Count == 1)
            {
                if (Boot != null)
                {
                    SetRemoveNode<KT, VT> node = new SetRemoveNode<KT, VT>(value);
                    return set(ref node);
                }
                Boot = value;
                return true;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
        private bool set(ref SetRemoveNode<KT, VT> node)
        {
            VT boot = Boot.notNull();
            var newBoot = boot.Set(ref node);
            if (newBoot == null)
            {
                if (node.IsRemove) Boot = node.SetRemove(boot);
            }
            else Boot = newBoot;
            return node.IsNewValue;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="value">data</param>
        /// <param name="removeValue">When a method returns false, it indicates that data has been removed
        /// 方法返回 false 时表示被移除数据</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
#if NetStandard21
        public bool Set(VT value, [MaybeNullWhen(true)] out VT removeValue)
#else
        public bool Set(VT value, out VT removeValue)
#endif
        {
            if (value.Count == 1)
            {
                if (Boot != null)
                {
                    SetRemoveNode<KT, VT> node = new SetRemoveNode<KT, VT>(value);
                    if (set(ref node))
                    {
                        removeValue = node.RemoveValue;
                        return false;
                    }
                }
                else Boot = value;
                removeValue = default(VT);
                return true;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Whether new data has been added
        /// 是否添加了新数据</returns>
        public bool TryAdd(VT value)
        {
            if (value.Count == 1)
            {
                if (Boot == null)
                {
                    Boot = value;
                    return true;
                }
                SetRemoveNode<KT, VT> node = new SetRemoveNode<KT, VT>(value);
                var boot = Boot.TryAdd(ref node);
                if (boot != null) Boot = boot;
                return node.IsNewValue;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            if (Boot != null)
            {
                var node = Boot.Remove(key);
                if (node != null)
                {
                    if (object.ReferenceEquals(Boot, node)) Boot = node.Remove();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Delete the matching data based on the keyword collection
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        public int RemoveKeys(KT[] keys)
        {
            int count = 0;
            if (Boot != null)
            {
                foreach (KT key in keys)
                {
                    if (key != null && Remove(key)) ++count;
                }
            }
            return count;
        }
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">Deleted data
        /// 被删除数据</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            if (Boot != null)
            {
                var node = Boot.Remove(key);
                if (node != null)
                {
                    if (object.ReferenceEquals(Boot, node)) Boot = node.Remove();
                    value = node;
                    return true;
                }
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Determines if the keyword exists
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Whether the keyword exists
        /// 是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            return Boot != null && Boot.Get(key) != null;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">Target data</param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetValue(KT key, out VT value)
#endif
        {
            if (Boot != null)
            {
                var node = Boot.Get(key);
                if (node != null)
                {
                    value = node;
                    return true;
                }
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Get the matching data array based on the keyword collection
        /// 根据关键字集合获取匹配数据数组
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public VT[] GetValueArray(KT[] keys)
        {
            if (keys != null && keys.Length != 0)
            {
                VT[] values = new VT[keys.Length];
                var value = default(VT);
                int index = 0;
                foreach (KT key in keys)
                {
                    if (key != null && TryGetValue(key, out value)) values[index] = value;
                    ++index;
                }
                return values;
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(KT key)
        {
            return Boot != null ? Boot.IndexOf(key) : -1;
        }
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Number of nodes
        /// 节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountLess(KT key)
        {
            return Boot != null ? Boot.CountLess(key) : 0;
        }
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Number of nodes
        /// 节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountThan(KT key)
        {
            return Boot != null ? Boot.CountThan(key) : 0;
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT At(int index)
        {
            if (Boot != null && (uint)index < (uint)Boot.Count) return Boot.At(index);
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public bool TryGetValueByIndex(int index, [MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetValueByIndex(int index, out VT value)
#endif
        {
            if (Boot != null && (uint)index < (uint)Boot.Count)
            {
                value = Boot.At(index);
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Get a collection of data based on the range
        /// 根据范围获取数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <returns>The data collection
        /// 数据集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KT[] GetKeyRange(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetKeyArray(skipCount, getCount) : EmptyArray<KT>.Array;
        }
        /// <summary>
        /// Get the data collection of the reverse range
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <returns>The data collection
        /// 数据集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KT[] GetKeyRangeDesc(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetDescKeyArray(skipCount, getCount) : EmptyArray<KT>.Array;
        }

#if DEBUG
        /// <summary>
        /// Check the correctness of the data (for testing)
        /// 检查数据正确性（用于测试）
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool Check(int count)
        {
            return Boot != null ? Boot.Check(count) : (count == 0);
        }
#endif
    }
}
