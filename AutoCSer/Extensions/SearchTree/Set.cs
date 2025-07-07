using AutoCSer.Configuration;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// Binary search tree set
    /// 二叉搜索树集合
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    [RemoteType]
    public sealed class Set<T> where T : IComparable<T>
    {
        /// <summary>
        /// Binary search tree set node
        /// 二叉搜索树集合节点
        /// </summary>
        internal sealed class Node : Node<Node, T>
        {
            /// <summary>
            /// Binary search tree set node
            /// 二叉搜索树集合节点
            /// </summary>
            /// <param name="key"></param>
            internal Node(T key) : base(key) { }
            /// <summary>
            /// The data collection
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
                    yield return key;
                    if (Right != null)
                    {
                        foreach (T value in Right.Values) yield return value;
                    }
                }
            }
            /// <summary>
            /// Get the first data
            /// 获取第一个数据
            /// </summary>
            internal T Frist
            {
                get
                {
                    return Left != null ? Left.Frist : key;
                }
            }
            /// <summary>
            /// Get the last data
            /// 获取最后一个数据
            /// </summary>
            internal T Last
            {
                get
                {
                    return Right != null ? Right.Last : key;
                }
            }

            /// <summary>
            /// Add data
            /// </summary>
            /// <param name="key"></param>
            /// <returns>Whether new data has been added
            /// 是否添加了新数据</returns>
            internal bool Add(T key)
            {
                int cmp = this.key.CompareTo(key);
                if (cmp < 0)
                {
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
                if (cmp != 0)
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
                }
                return false;
            }
            /// <summary>
            /// Exchange node data
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private void changeKey(ref T key)
            {
                T tempKey = key;
                key = base.key;
                base.key = tempKey;
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
                                leftRight.changeKey(ref key);
                                Right = leftRight;
                            }
                        }
                        else if (left.Right != null)
                        {
                            Left = left.rightToLeft(Right);
                            left.changeKey(ref key);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.notNull().changeKey(ref key);
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
                                rightLeft.changeKey(ref key);
                                Left = rightLeft;
                            }
                        }
                        else if (right.Left != null)
                        {
                            Right = right.leftToRight(Left);
                            right.changeKey(ref key);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.notNull().changeKey(ref key);
                }
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
        /// The data collection
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                return Boot != null && Boot.Count != 0 ? Boot.Values : EmptyArray<T>.Array;
            }
        }
        /// <summary>
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        public T Frist
        {
            get
            {
                if (Boot != null) return Boot.Frist;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        public T Last
        {
            get
            {
                if (Boot != null) return Boot.Last;
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Binary search tree set
        /// 二叉搜索树集合
        /// </summary>
        public Set() { }
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
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Whether new data has been added
        /// 是否添加了新数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add(T key)
        {
            if (Boot == null)
            {
                Boot = new Node(key);
                return true;
            }
            return Boot.Add(key);
        }
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(T key)
        {
            if (Boot != null)
            {
                var node = Boot.Remove(key);
                if (node != null)
                {
                    if (node == Boot) Boot = node.Remove();
                    return true;
                }
            }
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
        public bool Contains(T key)
        {
            return Boot != null && Boot.Get(key) != null;
        }
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T key)
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
        public int CountLess(T key)
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
        public int CountThan(T key)
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
        public T At(int index)
        {
            if (Boot != null && (uint)index < (uint)Boot.Count) return Boot.At(index).Key;
            throw new IndexOutOfRangeException();
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
        internal T[] GetRange(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetKeyArray(skipCount, getCount) : EmptyArray<T>.Array;
        }
        /// <summary>
        /// Get a collection of data based on the range
        /// 根据范围获取数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns>The data collection
        /// 数据集合</returns>
        internal AT[] GetRange<AT>(int skipCount, int getCount, Func<T, AT> getValue)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                PageArray<T, AT> array = new PageArray<T, AT> { Array = new AT[Math.Min(Boot.Count - skipCount, getCount)], SkipCount = skipCount, GetValue = getValue };
                Boot.GetArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<AT>.Array;
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
        internal T[] GetRangeDesc(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetDescKeyArray(skipCount, getCount) : EmptyArray<T>.Array;
        }
        /// <summary>
        /// Get the data collection of the reverse range
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns>The data collection
        /// 数据集合</returns>
        internal AT[] GetRangeDesc<AT>(int skipCount, int getCount, Func<T, AT> getValue)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                getCount = Math.Min(Boot.Count - skipCount, getCount);
                PageArray<T, AT> array = new PageArray<T, AT> { Array = new AT[getCount], SkipCount = Boot.Count - (skipCount + getCount), Index = getCount, GetValue = getValue };
                Boot.GetDescArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<AT>.Array;
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
