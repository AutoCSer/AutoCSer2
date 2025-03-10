using AutoCSer.Configuration;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉搜索树集合
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [RemoteType]
    public sealed class Set<T> where T : IComparable<T>
    {
        /// <summary>
        /// 二叉搜索树集合节点
        /// </summary>
        private sealed class Node : Node<Node, T>
        {
            /// <summary>
            /// 二叉搜索树集合
            /// </summary>
            /// <param name="key"></param>
            internal Node(T key) : base(key) { }
            /// <summary>
            /// 数据集合
            /// </summary>
            internal IEnumerable<T> Values
            {
                get
                {
                    if (Left != null)
                    {
                        foreach (T value in Left.Values) yield return value;
                    }
                    yield return Key;
                    if (Right != null)
                    {
                        foreach (T value in Right.Values) yield return value;
                    }
                }
            }
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            internal T Frist
            {
                get
                {
                    return Left != null ? Left.Frist : Key;
                }
            }
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            internal T Last
            {
                get
                {
                    return Right != null ? Right.Last : Key;
                }
            }
            /// <summary>
            /// 根据关键字获取二叉树节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>匹配节点</returns>
#if NetStandard21
            internal Node? Get(T key)
#else
            internal Node Get(T key)
#endif
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                return cmp < 0 ? (Left != null ? Left.Get(key) : null) : (Right != null ? Right.Get(key) : null);
            }
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
                return Right.notNull().at(index - 1);
            }

            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否添加了数据</returns>
            internal bool Add(T key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return false;
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node(key);
                        ++Count;
                        return true;
                    }
                    if (Left.Add(key))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node(key);
                    ++Count;
                    return true;
                }
                if (Right.Add(key))
                {
                    checkRight();
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private void changeKey(ref T key)
            {
                T tempKey = key;
                key = Key;
                Key = tempKey;
            }
            /// <summary>
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
                                leftRight.changeKey(ref Key);
                                Right = leftRight;
                            }
                        }
                        else if (left.Right != null)
                        {
                            Left = left.rightToLeft(Right);
                            left.changeKey(ref Key);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.notNull().changeKey(ref Key);
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
                                rightLeft.changeKey(ref Key);
                                Left = rightLeft;
                            }
                        }
                        else if (right.Left != null)
                        {
                            Right = right.leftToRight(Left);
                            right.changeKey(ref Key);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.notNull().changeKey(ref Key);
                }
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除节点</returns>
#if NetStandard21
            internal Node? Remove(T key)
#else
            internal Node Remove(T key)
#endif
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                if (cmp < 0)
                {
                    if (Left != null)
                    {
                        var node = Left.Remove(key);
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
                    var node = Right.Remove(key);
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
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
            internal Node? Remove()
#else
            internal Node Remove()
#endif
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
        }
        /// <summary>
        /// 根节点
        /// </summary>
#if NetStandard21
        private Node? boot;
#else
        private Node boot;
#endif
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Count
        {
            get { return boot != null ? boot.Count : 0; }
        }
        /// <summary>
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        public int Height
        {
            get
            {
                return boot == null ? 0 : boot.Height;
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                return boot != null && boot.Count != 0 ? boot.Values : EmptyArray<T>.Array;
            }
        }
        /// <summary>
        /// 获取第一组数据
        /// </summary>
        public T Frist
        {
            get
            {
                if (boot != null) return boot.Frist;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// 获取最后一组数据
        /// </summary>
        public T Last
        {
            get
            {
                if (boot != null) return boot.Last;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// 二叉树集合
        /// </summary>
        public Set() { }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            boot = null;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否添加了数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add(T key)
        {
            if (boot == null)
            {
                boot = new Node(key);
                return true;
            }
            return boot.Add(key);
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(T key)
        {
            if (boot != null)
            {
                var node = boot.Remove(key);
                if (node != null)
                {
                    if (node == boot) boot = node.Remove();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T key)
        {
            return boot != null && boot.Get(key) != null;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T key)
        {
            return boot != null ? boot.IndexOf(key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountLess(T key)
        {
            return boot != null ? boot.CountLess(key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountThan(T key)
        {
            return boot != null ? boot.CountThan(key) : 0;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T At(int index)
        {
            if (boot != null) return boot.At(index).Key;
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        internal T[] GetRange(int skipCount, int getCount)
        {
            if (boot != null && skipCount < boot.Count)
            {
                PageArray<T> array = new PageArray<T> { Array = new T[Math.Min(boot.Count - skipCount, getCount)], SkipCount = skipCount };
                boot.GetArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <param name="getValue">数据转换委托</param>
        /// <returns>数据集合</returns>
        internal AT[] GetRange<AT>(int skipCount, int getCount, Func<T, AT> getValue)
        {
            if (boot != null && skipCount < boot.Count)
            {
                PageArray<T, AT> array = new PageArray<T, AT> { Array = new AT[Math.Min(boot.Count - skipCount, getCount)], SkipCount = skipCount, GetValue = getValue };
                boot.GetArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<AT>.Array;
        }
        /// <summary>
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        internal T[] GetRangeDesc(int skipCount, int getCount)
        {
            if (boot != null && skipCount < boot.Count)
            {
                getCount = Math.Min(boot.Count - skipCount, getCount);
                PageArray<T> array = new PageArray<T> { Array = new T[getCount], SkipCount = boot.Count - (skipCount + getCount), Index = getCount };
                boot.GetDescArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <param name="getValue">数据转换委托</param>
        /// <returns>数据集合</returns>
        internal AT[] GetRangeDesc<AT>(int skipCount, int getCount, Func<T, AT> getValue)
        {
            if (boot != null && skipCount < boot.Count)
            {
                getCount = Math.Min(boot.Count - skipCount, getCount);
                PageArray<T, AT> array = new PageArray<T, AT> { Array = new AT[getCount], SkipCount = boot.Count - (skipCount + getCount), Index = getCount, GetValue = getValue };
                boot.GetDescArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<AT>.Array;
        }
    }
}
