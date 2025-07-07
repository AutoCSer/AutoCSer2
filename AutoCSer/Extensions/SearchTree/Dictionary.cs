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
    /// <typeparam name="VT">Data type</typeparam>
    [RemoteType]
    public sealed class Dictionary<KT, VT> where KT : IComparable<KT>
    {
        /// <summary>
        /// Binary search tree dictionary node
        /// 二叉搜索树字典节点
        /// </summary>
        internal sealed class Node : Node<Node, KT>
        {
            /// <summary>
            /// Node data
            /// 节点数据
            /// </summary>
            internal VT Value;
            /// <summary>
            /// Binary search tree dictionary node
            /// 二叉搜索树字典节点
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            internal Node(KT key, VT value) : base(key)
            {
                Value = value;
            }
            /// <summary>
            /// Key-value pair data
            /// 键值对数据
            /// </summary>
            internal KeyValue<KT, VT> KeyValue
            {
                get { return new KeyValue<KT, VT>(key, Value); }
            }
            /// <summary>
            /// The collection of key-value pair data
            /// 键值对数据集合
            /// </summary>
            internal IEnumerable<KeyValue<KT, VT>> KeyValues
            {
                get
                {
                    if (Left != null)
                    {
                        foreach (KeyValue<KT, VT> value in Left.KeyValues) yield return value;
                    }
                    yield return new KeyValue<KT, VT>(key, Value);
                    if (Right != null)
                    {
                        foreach (KeyValue<KT, VT> value in Right.KeyValues) yield return value;
                    }
                }
            }
            /// <summary>
            /// The data collection
            /// 数据集合
            /// </summary>
            internal IEnumerable<VT> Values
            {
                get
                {
                    if (Left != null)
                    {
                        foreach (VT value in Left.Values) yield return value;
                    }
                    yield return Value;
                    if (Right != null)
                    {
                        foreach (VT value in Right.Values) yield return value;
                    }
                }
            }
            /// <summary>
            /// Get the first pair of data
            /// 获取第一对数据
            /// </summary>
            internal KeyValue<KT, VT> FristKeyValue
            {
                get
                {
                    return Left != null ? Left.FristKeyValue : new KeyValue<KT, VT>(key, Value);
                }
            }
            /// <summary>
            /// Get the last pair of data
            /// 获取最后一对数据
            /// </summary>
            internal KeyValue<KT, VT> LastKeyValue
            {
                get
                {
                    return Right != null ? Right.LastKeyValue : new KeyValue<KT, VT>(key, Value);
                }
            }

            /// <summary>
            /// Exchange node data
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private void changeKeyValue(ref KT key, ref VT value)
            {
                KT tempKey = key;
                VT tempValue = value;
                key = base.key;
                value = Value;
                base.key = tempKey;
                Value = tempValue;
            }
            /// <summary>
            /// Check the number of left nodes
            /// 检测左节点数量
            /// </summary>
            private void checkLeft()
            {
                ++Count;
                if (Right != null)
                {
                    Node left = Left.notNull();
                    if ((left.Count >> 1) > Right.Count && left.isLeftAndRight)
                    {
                        if (left.leftRightDifferenceCount <= 0)
                        {
                            Node leftRight = left.Right.notNull();
                            if (leftRight.isLeftAndRight)
                            {
                                left.Right = leftRight.rightToLeft(Right);
                                left.removeCount1(leftRight.Left.notNull());
                                leftRight.changeKeyValue(ref key, ref Value);
                                Right = leftRight;
                            }
                        }
                        else if (left.Right != null)
                        {
                            Left = left.rightToLeft(Right);
                            left.changeKeyValue(ref key, ref Value);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.notNull().changeKeyValue(ref key, ref Value);
                }
            }
            /// <summary>
            /// Check the number of right nodes
            /// 检测右节点数量
            /// </summary>
            private void checkRight()
            {
                ++Count;
                if (Left != null)
                {
                    Node right = Right.notNull();
                    if ((right.Count >> 1) > Left.Count && right.isLeftAndRight)
                    {
                        if (right.leftRightDifferenceCount >= 0)
                        {
                            Node rightLeft = right.Left.notNull();
                            if (rightLeft.isLeftAndRight)
                            {
                                right.Left = rightLeft.leftToRight(Left);
                                right.removeCount1(rightLeft.Right.notNull());
                                rightLeft.changeKeyValue(ref key, ref Value);
                                Left = rightLeft;
                            }
                        }
                        else if (right.Left != null)
                        {
                            Right = right.leftToRight(Left);
                            right.changeKeyValue(ref key, ref Value);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.notNull().changeKeyValue(ref key, ref Value);
                }
            }
            /// <summary>
            /// Add data
            /// </summary>
            /// <param name="keyValue"></param>
            /// <returns>Whether new data has been added
            /// 是否添加了新数据</returns>
            internal bool TryAdd(ref KeyValue<KT, VT> keyValue)
            {
                int cmp = key.CompareTo(keyValue.Key);
                if (cmp < 0)
                {
                    if (Right == null)
                    {
                        Right = new Node(keyValue.Key, keyValue.Value);
                        ++Count;
                        return true;
                    }
                    if (Right.TryAdd(ref keyValue))
                    {
                        checkRight();
                        return true;
                    }
                    return false;
                }
                if (cmp != 0)
                {
                    if (Left == null)
                    {
                        Left = new Node(keyValue.Key, keyValue.Value);
                        ++Count;
                        return true;
                    }
                    if (Left.TryAdd(ref keyValue))
                    {
                        checkLeft();
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// Set the data
            /// 设置数据
            /// </summary>
            /// <param name="node">Node data
            /// 节点数据</param>
            /// <returns>Whether new data has been added
            /// 是否添加了新数据</returns>
            internal bool Set(ref SetRemoveValue<KT, VT> node)
            {
                int cmp = key.CompareTo(node.Key);
                if (cmp < 0)
                {
                    if (Right == null)
                    {
                        Right = node.Node;
                        ++Count;
                        return true;
                    }
                    if (Right.Set(ref node))
                    {
                        checkRight();
                        return true;
                    }
                    return false;
                }
                if(cmp != 0)
                {
                    if (Left == null)
                    {
                        Left = node.Node;
                        ++Count;
                        return true;
                    }
                    if (Left.Set(ref node))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                Value = node.SetRemove(Value);
                return false;
            }


            /// <summary>
            /// Get the data collection
            /// 获取数据集合
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            internal IEnumerable<VT> Enumerable(EnumerableCount count)
            {
                do
                {
                    if (Left != null)
                    {
                        int leftCount = Left.Count;
                        if (leftCount > count.SkipCount)
                        {
                            foreach (VT value in Left.Enumerable(count)) yield return value;
                            if (count.GetCount == 0) break;
                            yield return Value;
                            if (--count.GetCount == 0) break;
                            foreach (VT value in Right.notNull().enumerable(count)) yield return value;
                            break;
                        }
                        count.SkipCount -= leftCount;
                    }
                    if (count.SkipCount == 0)
                    {
                        yield return Value;
                        if (--count.GetCount == 0 || Right == null) break;
                        foreach (VT value in Right.enumerable(count)) yield return value;
                        break;
                    }
                    --count.SkipCount;
                    if (Right != null)
                    {
                        foreach (VT value in Right.Enumerable(count)) yield return value;
                    }
                }
                while (false);
            }
            /// <summary>
            /// Get the data collection
            /// 获取数据集合
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            private IEnumerable<VT> enumerable(EnumerableCount count)
            {
                do
                {
                    if (Left != null)
                    {
                        foreach (VT value in Left.enumerable(count)) yield return value;
                        if (count.GetCount == 0) break;
                    }
                    yield return Value;
                    if (--count.GetCount != 0 && Right != null)
                    {
                        foreach (VT value in Right.enumerable(count)) yield return value;
                    }
                }
                while (false);
            }

            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <param name="array"></param>
            internal void GetArraySkip(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetArraySkip(ref array);
                        if (!array.IsArray && !array.Add(Value)) Right.notNull().getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right?.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right?.GetArraySkip(ref array);
            }
            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <param name="array"></param>
            private void getArray(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    Left.getArray(ref array);
                    if (array.IsArray) return;
                }
                if (!array.Add(Value)) Right?.getArray(ref array);
            }

            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <typeparam name="AT"></typeparam>
            /// <param name="array"></param>
            internal void GetArraySkip<AT>(ref PageArray<VT, AT> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetArraySkip(ref array);
                        if (!array.IsArray && !array.Add(Value)) Right.notNull().getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right?.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right?.GetArraySkip(ref array);
            }
            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <typeparam name="arrayType"></typeparam>
            /// <param name="array"></param>
            private void getArray<arrayType>(ref PageArray<VT, arrayType> array)
            {
                if (Left != null)
                {
                    Left.getArray(ref array);
                    if (array.IsArray) return;
                }
                if (!array.Add(Value)) Right?.getArray(ref array);
            }

            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <param name="array"></param>
            internal void GetDescArraySkip(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetDescArraySkip(ref array);
                        if (array.Index != 0 && array.AddDesc(Value) != 0) Right.notNull().getDescArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (array.AddDesc(Value) != 0) Right?.getDescArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right?.GetDescArraySkip(ref array);
            }
            /// <summary>
            /// Get get array data
            /// 获取数组数据
            /// </summary>
            /// <param name="array"></param>
            private void getDescArray(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    Left.getDescArray(ref array);
                    if (array.Index == 0) return;
                }
                if (array.AddDesc(Value) != 0) Right?.getDescArray(ref array);
            }
            /// <summary>
            /// Search for data
            /// 查找数据
            /// </summary>
            /// <param name="array"></param>
            internal void GetFind(ref FindArray<VT> array)
            {
                if (Left != null) Left.GetFind(ref array);
                array.Add(Value);
                if (Right != null) Right.GetFind(ref array);
            }
        }
        /// <summary>
        /// Root node
        /// 根节点
        /// </summary>
#if NetStandard21
        internal Node? Boot;
#else
        internal Node Boot;
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
        /// The collection of key-value pair data
        /// 键值对数据集合
        /// </summary>
        public IEnumerable<KeyValue<KT, VT>> KeyValues
        {
            get
            {
                return Boot != null && Boot.Count != 0 ? Boot.KeyValues : EmptyArray<KeyValue<KT, VT>>.Array;
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
                return Boot != null && Boot.Count != 0 ? Boot.Values : EmptyArray<VT>.Array;
            }
        }
        /// <summary>
        /// Get the first pair of data
        /// 获取第一对数据
        /// </summary>
        public KeyValue<KT, VT> FristKeyValue
        {
            get
            {
                if (Boot != null) return Boot.FristKeyValue;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Get the last pair of data
        /// 获取最后一对数据
        /// </summary>
        public KeyValue<KT, VT> LastKeyValue
        {
            get
            {
                if (Boot != null) return Boot.LastKeyValue;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Get or set data based on keyword
        /// 根据关键字获取或者设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>The acquisition failed and a KeyNotFoundException exception was thrown
        /// 获取失败抛出 KeyNotFoundException 异常</returns>
        public VT this[KT key]
        {
            get
            {
                if (Boot != null)
                {
                    var node = Boot.Get(key);
                    if (node != null) return node.Value;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set { Set(key, value); }
        }
        /// <summary>
        /// Binary search tree dictionary
        /// 二叉搜索树字典
        /// </summary>
        public Dictionary() { }
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
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
        public bool Set(KT key, VT value)
        {
            if (Boot != null)
            {
                SetRemoveValue<KT, VT> node = new SetRemoveValue<KT, VT>(key, value);
                return Boot.Set(ref node);
            }
            Boot = new Node(key, value);
            return true;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <param name="removeValue">When a method returns false, it indicates that data has been removed
        /// 方法返回 false 时表示被移除数据</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
#if NetStandard21
        public bool Set(KT key, VT value, [MaybeNullWhen(true)] out VT removeValue)
#else
        public bool Set(KT key, VT value, out VT removeValue)
#endif
        {
            if (Boot != null)
            {
                SetRemoveValue<KT, VT> node = new SetRemoveValue<KT, VT>(key, value);
                if (!Boot.Set(ref node))
                {
                    removeValue = node.RemoveValue;
                    return false;
                }
            }
            else Boot = new Node(key, value);
            removeValue = default(VT);
            return true;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Whether new data has been added
        /// 是否添加了新数据</returns>
        public bool TryAdd(KT key, VT value)
        {
            if (Boot == null)
            {
                Boot = new Node(key, value);
                return true;
            }
            KeyValue<KT, VT> keyValue = new KeyValue<KT, VT>(key, value);
            return Boot.TryAdd(ref keyValue);
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
                    value = node.Value;
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
                    value = node.Value;
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
        /// Get the data of the first node that is larger than the key
        /// 获取第一个大于关键字的节点数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KeyValue<KT, VT> GetThanNodeKeyValue(KT key)
        {
            var node = Boot?.GetThanNode(key);
            return node != null ? node.KeyValue : default(KeyValue<KT, VT>);
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeyValue<KT, VT> At(int index)
        {
            if (Boot != null && (uint)index < (uint)Boot.Count) return Boot.At(index).KeyValue;
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
                value = Boot.At(index).Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">The number of paginated records
        /// 分页记录数量</param>
        /// <param name="page">Page number, starting from 1
        /// 分页号，从 1 开始</param>
        /// <returns>Page data
        /// 分页数据</returns>
        internal VT[] GetPage(int pageSize, int page)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    PageArray<VT> array = new PageArray<VT> { Array = new VT[Math.Min(count - skipCount, pageSize)], SkipCount = skipCount };
                    Boot.GetArraySkip(ref array);
                    return array.Array;
                }
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">The number of paginated records
        /// 分页记录数量</param>
        /// <param name="page">Page number, starting from 1
        /// 分页号，从 1 开始</param>
        /// <param name="getValue">The delegate to get the data
        /// 获取数据委托</param>
        /// <returns>Page data
        /// 分页数据</returns>
        internal arrayType[] GetPage<arrayType>(int pageSize, int page, Func<VT, arrayType> getValue)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    PageArray<VT, arrayType> array = new PageArray<VT, arrayType> { Array = new arrayType[Math.Min(count - skipCount, pageSize)], SkipCount = skipCount, GetValue = getValue };
                    Boot.GetArraySkip(ref array);
                    return array.Array;
                }
            }
            return EmptyArray<arrayType>.Array;
        }
        /// <summary>
        /// Get the reverse page data
        /// 获取逆序分页数据
        /// </summary>
        /// <param name="pageSize">The number of paginated records
        /// 分页记录数量</param>
        /// <param name="page">Page number, starting from 1
        /// 分页号，从 1 开始</param>
        /// <returns>Page data
        /// 分页数据</returns>
        internal VT[] GetPageDesc(int pageSize, int page)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    pageSize = Math.Min(count - skipCount, pageSize);
                    PageArray<VT> array = new PageArray<VT> { Array = new VT[pageSize], SkipCount = count - (skipCount + pageSize), Index = pageSize };
                    Boot.GetDescArraySkip(ref array);
                    return array.Array;
                }
            }
            return EmptyArray<VT>.Array;
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
        internal VT[] GetRange(int skipCount, int getCount)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                PageArray<VT> array = new PageArray<VT> { Array = new VT[Math.Min(Boot.Count - skipCount, getCount)], SkipCount = skipCount };
                Boot.GetArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<VT>.Array;
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
        internal VT[] GetRangeDesc(int skipCount, int getCount)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                getCount = Math.Min(Boot.Count - skipCount, getCount);
                PageArray<VT> array = new PageArray<VT> { Array = new VT[getCount], SkipCount = Boot.Count - (skipCount + getCount), Index = getCount };
                Boot.GetDescArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<VT>.Array;
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
        /// <summary>
        /// Search for data
        /// 查找数据
        /// </summary>
        /// <param name="isValue">Delegate for data matching
        /// 数据匹配委托</param>
        /// <returns></returns>
        internal LeftArray<VT> GetFind(Func<VT, bool> isValue)
        {
            if (Boot != null)
            {
                FindArray<VT> array = new FindArray<VT> { IsValue = isValue, Array = new LeftArray<VT>(0) };
                Boot.GetFind(ref array);
                return array.Array;
            }
            return new LeftArray<VT>(0);
        }
        /// <summary>
        /// Get a collection of data based on the range
        /// 根据范围获取数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <returns></returns>
        internal IEnumerable<VT> GetValues(int skipCount, int getCount)
        {
            if (Boot != null)
            {
                if (skipCount < 0)
                {
                    getCount += skipCount;
                    skipCount = 0;
                }
                if (skipCount < Boot.Count && skipCount >= 0 && getCount > 0) return Boot.Enumerable(new EnumerableCount(skipCount, getCount));
            }
            return EmptyArray<VT>.Array;
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
            return Boot != null? Boot.Check(count) : (count == 0);
        }
#endif
    }
}
