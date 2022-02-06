using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉搜索树字典
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public sealed partial class Dictionary<KT, VT> where KT : IComparable<KT>
    {
        /// <summary>
        /// 二叉搜索树字典节点
        /// </summary>
        internal sealed class Node : Node<Node, KT>
        {
            /// <summary>
            /// 节点数据
            /// </summary>
            internal VT Value;
            /// <summary>
            /// 数据
            /// </summary>
            internal KeyValue<KT, VT> KeyValue
            {
                get { return new KeyValue<KT, VT>(Key, Value); }
            }
            /// <summary>
            /// 数据集合
            /// </summary>
            internal IEnumerable<KeyValue<KT, VT>> KeyValues
            {
                get
                {
                    if (Left != null)
                    {
                        foreach (KeyValue<KT, VT> value in Left.KeyValues) yield return value;
                    }
                    yield return new KeyValue<KT, VT>(Key, Value);
                    if (Right != null)
                    {
                        foreach (KeyValue<KT, VT> value in Right.KeyValues) yield return value;
                    }
                }
            }

            /// <summary>
            /// 根据关键字获取二叉树节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>匹配节点</returns>
            internal Node Get(ref KT key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                return cmp < 0 ? (Left != null ? Left.Get(ref key) : null) : (Right != null ? Right.Get(ref key) : null);
            }
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal Node At(int index)
            {
                if ((uint)index < Count) return at(index);
                throw new IndexOutOfRangeException();
            }
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            internal Node at(int index)
            {
                if (Left != null)
                {
                    if (index < Left.Count) return Left.at(index);
                    if ((index -= Left.Count) == 0) return this;
                }
                else if (index == 0) return this;
                return Right.at(index - 1);
            }

            /// <summary>
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private void changeKeyValue(ref KT key, ref VT value)
            {
                KT tempKey = key;
                VT tempValue = value;
                key = Key;
                value = Value;
                Key = tempKey;
                Value = tempValue;
            }
            /// <summary>
            /// 检测左节点数量
            /// </summary>
            private void checkLeft()
            {
                ++Count;
                if (Right != null)
                {
                    if ((Left.Count >> 1) > Right.Count && Left.isLeftAndRight)
                    {
                        if (Left.leftRightDifferenceCount <= 0)
                        {
                            Node leftRight = Left.Right;
                            if (leftRight.isLeftAndRight)
                            {
                                Left.Right = leftRight.rightToLeft(Right);
                                Left.removeCount1(leftRight.Left);
                                leftRight.changeKeyValue(ref Key, ref Value);
                                Right = leftRight;
                            }
                        }
                        else if (Left.Right != null)
                        {
                            Node left = Left;
                            Left = left.rightToLeft(Right);
                            left.changeKeyValue(ref Key, ref Value);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.changeKeyValue(ref Key, ref Value);
                }
            }
            /// <summary>
            /// 检测右节点数量
            /// </summary>
            private void checkRight()
            {
                ++Count;
                if (Left != null)
                {
                    if ((Right.Count >> 1) > Left.Count && Right.isLeftAndRight)
                    {
                        if (Right.leftRightDifferenceCount >= 0)
                        {
                            Node rightLeft = Right.Left;
                            if (rightLeft.isLeftAndRight)
                            {
                                Right.Left = rightLeft.leftToRight(Left);
                                Right.removeCount1(rightLeft.Right);
                                rightLeft.changeKeyValue(ref Key, ref Value);
                                Left = rightLeft;
                            }
                        }
                        else if (Right.Left != null)
                        {
                            Node right = Right;
                            Right = right.leftToRight(Left);
                            right.changeKeyValue(ref Key, ref Value);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.changeKeyValue(ref Key, ref Value);
                }
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="keyValue"></param>
            /// <returns>是否添加了数据</returns>
            internal bool TryAdd(ref KeyValue<KT, VT> keyValue)
            {
                int cmp = keyValue.Key.CompareTo(Key);
                if (cmp == 0) return false;
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                        ++Count;
                        return true;
                    }
                    if (Left.TryAdd(ref keyValue))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
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
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="keyValue">数据</param>
            /// <returns>是否添加了数据</returns>
            internal bool Set(ref KeyValue<KT, VT> keyValue)
            {
                int cmp = keyValue.Key.CompareTo(Key);
                if (cmp == 0)
                {
                    Value = keyValue.Value;
                    return false;
                }
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                        ++Count;
                        return true;
                    }
                    if (Left.Set(ref keyValue))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                    ++Count;
                    return true;
                }
                if (Right.Set(ref keyValue))
                {
                    checkRight();
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除节点</returns>
            internal Node Remove(ref KT key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                if (cmp < 0)
                {
                    if (Left != null)
                    {
                        Node node = Left.Remove(ref key);
                        if (node != null)
                        {
                            --Count;
                            if (node == Left) Left = node.Remove();
                            return node;
                        }
                    }
                }
                else if (Right != null)
                {
                    Node node = Right.Remove(ref key);
                    if (node != null)
                    {
                        --Count;
                        if (node == Right) Right = node.Remove();
                        return node;
                    }
                }
                return null;
            }
            /// <summary>
            /// 删除当前节点
            /// </summary>
            /// <returns>用户替换当前节点的节点</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal Node Remove()
            {
                if (Right != null)
                {
                    Node node = Right.removeMin();
                    if (node == Right) node.set(Left, Count - 1);
                    else node.set(Left, Right, Count - 1);
                    return node;
                }
                if (Left != null)
                {
                    Left.Count = Count - 1;
                    return Left;
                }
                return null;
            }
            /// <summary>
            /// 删除最小节点
            /// </summary>
            /// <returns></returns>
            private Node removeMin()
            {
                if (Left != null)
                {
                    --Count;
                    Node node = Left.removeMin();
                    if (node == Left) Left = node.Right;
                    return node;
                }
                return this;
            }

            /// <summary>
            /// 获取数组
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
                        if (!array.IsArray && !array.Add(Value)) Right.getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            private void getArray(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    Left.getArray(ref array);
                    if (array.IsArray) return;
                }
                if (!array.Add(Value)) Right.getArray(ref array);
            }

            /// <summary>
            /// 获取数组
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
                        if (!array.IsArray && !array.Add(Value)) Right.getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
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
                if (!array.Add(Value)) Right.getArray(ref array);
            }

            /// <summary>
            /// 获取数组
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
                        if (array.Index != 0 && array.AddDesc(Value) != 0) Right.getDescArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (array.AddDesc(Value) != 0) Right.getDescArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetDescArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            private void getDescArray(ref PageArray<VT> array)
            {
                if (Left != null)
                {
                    Left.getDescArray(ref array);
                    if (array.Index == 0) return;
                }
                if (array.AddDesc(Value) != 0) Right.getDescArray(ref array);
            }
            /// <summary>
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
        /// 根节点
        /// </summary>
        internal Node Boot;
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Count
        {
            get { return Boot != null ? Boot.Count : 0; }
        }
        /// <summary>
        /// 获取树高度，需要 O(n)
        /// </summary>
        public int Height
        {
            get
            {
                return Boot == null ? 0 : Boot.Height;
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        internal IEnumerable<KeyValue<KT, VT>> KeyValues
        {
            get
            {
                return Count != 0 ? Boot.KeyValues : EmptyArray<KeyValue<KT, VT>>.Array;
            }
        }
        /// <summary>
        /// 二叉树更新版本
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// 更新版本重置事件
        /// </summary>
        internal event Action OnResetVersion;
        /// <summary>
        /// 根据关键字获取或者设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据,获取失败KeyNotFoundException</returns>
        public VT this[KT key]
        {
            get
            {
                if (Boot != null)
                {
                    Node node = Boot.Get(ref key);
                    if (node != null) return node.Value;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set { Set(ref key, value); }
        }
        /// <summary>
        /// 二叉树字典
        /// </summary>
        public Dictionary() { Version = 1; }
        /// <summary>
        /// 更新二叉树版本
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void nextVersion()
        {
            if (++Version == 0)
            {
                Version = 1;
                if (OnResetVersion != null) OnResetVersion();
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            Boot = null;
            nextVersion();
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(KT key, VT value)
        {
            return Set(ref key, value);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        public bool Set(ref KT key, VT value)
        {
            if (Boot == null)
            {
                Boot = new Node { Key = key, Value = value, Count = 1 };
                nextVersion();
                return true;
            }
            KeyValue<KT, VT> keyValue = new KeyValue<KT, VT>(ref key, value);
            if (Boot.Set(ref keyValue))
            {
                nextVersion();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryAdd(KT key, VT value)
        {
            return TryAdd(ref key, value);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        public bool TryAdd(ref KT key, VT value)
        {
            if (Boot == null)
            {
                Boot = new Node { Key = key, Value = value, Count = 1 };
                nextVersion();
                return true;
            }
            KeyValue<KT, VT> keyValue = new KeyValue<KT, VT>(ref key, value);
            if (Boot.TryAdd(ref keyValue))
            {
                nextVersion();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(KT key)
        {
            return Remove(ref key);
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ref KT key)
        {
            if (Boot != null)
            {
                Node node = Boot.Remove(ref key);
                if (node != null)
                {
                    if (node == Boot) Boot = node.Remove();
                    nextVersion();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ref KT key, out VT value)
        {
            if (Boot != null)
            {
                Node node = Boot.Remove(ref key);
                if (node != null)
                {
                    if (node == Boot) Boot = node.Remove();
                    value = node.Value;
                    nextVersion();
                    return true;
                }
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            return Boot != null && Boot.Get(ref key) != null;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(ref KT key)
        {
            return Boot != null && Boot.Get(ref key) != null;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue(KT key, out VT value)
        {
            return TryGetValue(ref key, out value);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否成功</returns>
        public bool TryGetValue(ref KT key, out VT value)
        {
            if (Boot != null)
            {
                Node node = Boot.Get(ref key);
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
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(KT key)
        {
            return Boot != null ? Boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(ref KT key)
        {
            return Boot != null ? Boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountLess(ref KT key)
        {
            return Boot != null ? Boot.CountLess(ref key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountThan(ref KT key)
        {
            return Boot != null ? Boot.CountThan(ref key) : 0;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public KeyValue<KT, VT> At(int index)
        {
            if (Boot != null) return Boot.At(index).KeyValue;
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValueByIndex(int index, out VT value)
        {
            if (Boot != null && (uint)index < Count)
            {
                value = Boot.at(index).Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <returns>分页数据</returns>
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
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <param name="getValue">获取数据委托</param>
        /// <returns>分页数据</returns>
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
        /// 获取逆序分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <returns>分页数据</returns>
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
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
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
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
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
        /// 查找数据
        /// </summary>
        /// <param name="isValue">数据匹配委托</param>
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
    }
}
