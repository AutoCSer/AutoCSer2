using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉字典树节点
    /// </summary>
    /// <typeparam name="NT">二叉树节点类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public class Node<NT, KT>
        where NT : Node<NT, KT>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 左节点
        /// </summary>
#if NetStandard21
        internal NT? Left;
#else
        internal NT Left;
#endif
        /// <summary>
        /// 右节点
        /// </summary>
#if NetStandard21
        internal NT? Right;
#else
        internal NT Right;
#endif
        /// <summary>
        /// 关键字
        /// </summary>
        protected KT key;
        /// <summary>
        /// 关键字
        /// </summary>
        public KT Key { get { return key; } }
        /// <summary>
        /// Number of nodes
        /// 节点数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// The data collection
        /// 数据集合
        /// </summary>
        internal IEnumerable<NT> Nodes
        {
            get
            {
                if (Left != null)
                {
                    foreach (NT value in Left.Nodes) yield return value;
                }
                yield return (NT)this;
                if (Right != null)
                {
                    foreach (NT value in Right.Nodes) yield return value;
                }
            }
        }
        /// <summary>
        /// 获取第一个节点
        /// </summary>
        internal NT FristNode
        {
            get
            {
                return Left != null ? Left.FristNode : (NT)this;
            }
        }
        /// <summary>
        /// 获取最后一个节点
        /// </summary>
        internal NT LastNode
        {
            get
            {
                return Right != null ? Right.LastNode : (NT)this;
            }
        }
        ///// <summary>
        ///// 获取第一组数据
        ///// </summary>
        //internal KeyValue<KT, VT> FristKeyValue
        //{
        //    get
        //    {
        //        return Left != null ? Left.FristKeyValue : new KeyValue<KT, VT>(Key, Value);
        //    }
        //}
        ///// <summary>
        ///// 获取最后一组数据
        ///// </summary>
        //internal KeyValue<KT, VT> LastKeyValue
        //{
        //    get
        //    {
        //        return Right != null ? Right.LastKeyValue : new KeyValue<KT, VT>(Key, Value);
        //    }
        //}
        /// <summary>
        /// 二叉字典树节点
        /// </summary>
        /// <param name="key"></param>
        public Node(KT key)
        {
            this.key = key;
            Count = 1;
        }
        /// <summary>
        /// 节点高度
        /// </summary>
        internal int Height
        {
            get
            {
                if (Left != null)
                {
                    return Right != null ? Math.Max(Left.Height, Right.Height) + 1 : (Left.Height + 1);
                }
                return Right == null ? 1 : (Right.Height + 1);
            }
        }
        /// <summary>
        /// 是否同时存在左右节点
        /// </summary>
        protected bool isLeftAndRight
        {
            get { return Left != null && Right != null; }
        }
        /// <summary>
        /// 左右节点数据量差
        /// </summary>
        protected int leftRightDifferenceCount
        {
            get { return Left.notNull().Count - Right.notNull().Count; }
        }
        /// <summary>
        /// 根据关键字获取二叉树节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>匹配节点</returns>
#if NetStandard21
        internal NT? Get(KT key)
#else
        internal NT Get(KT key)
#endif
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0) return Right?.Get(key);
            if (cmp != 0) return Left?.Get(key);
            return (NT)this;
        }
        /// <summary>
        /// 设置替换节点
        /// </summary>
        /// <param name="node">原节点数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemove(NT node)
        {
            Left = node.Left;
            Right = node.Right;
            Count = node.Count;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="node">数据</param>
        /// <returns>替换节点</returns>
#if NetStandard21
        internal NT? Set(ref SetRemoveNode<KT, NT> node)
#else
        internal NT Set(ref SetRemoveNode<KT, NT> node)
#endif
        {
            int cmp = key.CompareTo(node.Value.Key);
            if (cmp < 0)
            {
                if (Right == null)
                {
                    Right = node.Value;
                    ++Count;
                    node.IsNewValue = true;
                    return null;
                }
                var right = Right.Set(ref node);
                if (right == null)
                {
                    if (node.IsRemove) Right = node.SetRemove(Right);
                }
                else Right = right;
                return node.IsNewValue ? checkRight() : null;
            }
            if (cmp != 0)
            {
                if (Left == null)
                {
                    Left = node.Value;
                    ++Count;
                    node.IsNewValue = true;
                    return null;
                }
                var left = Left.Set(ref node);
                if (left == null)
                {
                    if (node.IsRemove) Left = node.SetRemove(Left);
                }
                else Left = left;
                return node.IsNewValue ? checkLeft() : null;
            }
            node.IsRemove = true;
            return null;
        }
        /// <summary>
        /// Check the number of left nodes
        /// 检测左节点数量
        /// </summary>
        /// <returns>替换节点</returns>
#if NetStandard21
        private NT? checkLeft()
#else
        private NT checkLeft()
#endif
        {
            ++Count;
            NT left = Left.notNull();
            if (Right != null)
            {
                if ((left.Count >> 1) > Right.Count)
                {
                    var leftRight = left.Right;
                    if (leftRight != null)
                    {
                        left.setCheckRightCheckLeftCount(leftRight.Left);
                        setCheckLeftRightCount(leftRight.Right);
                        leftRight.set(left, (NT)this);
                        return leftRight;
                    }
                    var leftLeft = left.Left.notNull();
                    left.setCheckLeft(leftLeft.Right);
                    Count = left.Count + Right.Count + 1;
                    leftLeft.setRightCheckLeftCount((NT)this);
                    return leftLeft;
                }
                return null;
            }
            else
            {
                var leftRight = left.Right;
                if (leftRight != null)
                {
                    var leftLeft = left.Left;
                    setCheckLeft(leftRight.Right);
                    if (leftLeft != null)
                    {
                        if (leftRight.Count <= leftLeft.Count)
                        {
                            leftRight.addRight((NT)this);
                            ++left.Count;
                            return left;
                        }
                        left.setCheckRightLeftCount(leftRight.Left);
                    }
                    else left.setCheckRight(leftRight.Left);
                    leftRight.set(left, (NT)this);
                    return leftRight;
                }
                left.addRight((NT)this);
                Left = null;
                Count = 1;
                return left;
            }
        }
        /// <summary>
        /// Check the number of right nodes
        /// 检测右节点数量
        /// </summary>
        /// <returns>替换节点</returns>
#if NetStandard21
        private NT? checkRight()
#else
        private NT checkRight()
#endif
        {
            ++Count;
            NT right = Right.notNull();
            if (Left != null)
            {
                if ((right.Count >> 1) > Left.Count)
                {
                    var rightLeft = right.Left;
                    if (rightLeft != null)
                    {
                        right.setCheckLeftCheckRightCount(rightLeft.Right);
                        setCheckRightLeftCount(rightLeft.Left);
                        rightLeft.set((NT)this, right);
                        return rightLeft;
                    }
                    var rightRight = right.Right.notNull();
                    right.setCheckRight(rightRight.Left);
                    Count = right.Count + Left.Count + 1;
                    rightRight.setLeftCheckRightCount((NT)this);
                    return rightRight;
                }
                return null;
            }
            else
            {
                var rightLeft = right.Left;
                if (rightLeft != null)
                {
                    var rightRight = right.Right;
                    setCheckRight(rightLeft.Left);
                    if (rightRight != null)
                    {
                        if (rightLeft.Count <= rightRight.Count)
                        {
                            rightLeft.addLeft((NT)this);
                            ++right.Count;
                            return right;
                        }
                        right.setCheckLeftRightCount(rightLeft.Right);
                    }
                    else right.setCheckLeft(rightLeft.Right);
                    rightLeft.set((NT)this, right);
                    return rightLeft;
                }
                right.addLeft((NT)this);
                Right = null;
                Count = 1;
                return right;
            }
        }
        /// <summary>
        /// 设置左右节点并重新计算节点数量
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void set(NT left, NT right)
        {
            Left = left;
            Right = right;
            Count = left.Count + right.Count + 1;
        }
        /// <summary>
        /// 设置左节点（确定右节点为 null）
        /// </summary>
        /// <param name="left"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckLeft(NT? left)
#else
        private void setCheckLeft(NT left)
#endif
        {
            Left = left;
            Count = left != null ? left.Count + 1 : 1;
        }
        /// <summary>
        /// 设置右节点（确定左节点为 null）
        /// </summary>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckRight(NT? right)
#else
        private void setCheckRight(NT right)
#endif
        {
            Right = right;
            Count = right != null ? right.Count + 1 : 1;
        }
        /// <summary>
        /// 设置左节点（确定右节点不为 null）
        /// </summary>
        /// <param name="left"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckLeftRightCount(NT? left)
#else
        private void setCheckLeftRightCount(NT left)
#endif
        {
            Left = left;
            Count = Right.notNull().Count + (left != null ? left.Count + 1 : 1);
        }
        /// <summary>
        /// 设置右节点（确定左节点不为 null）
        /// </summary>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckRightLeftCount(NT? right)
#else
        private void setCheckRightLeftCount(NT right)
#endif
        {
            Right = right;
            Count = Left.notNull().Count + (right != null ? right.Count + 1 : 1);
        }
        /// <summary>
        /// 设置左节点并重新计算节点数量
        /// </summary>
        /// <param name="left"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckLeftCheckRightCount(NT? left)
#else
        private void setCheckLeftCheckRightCount(NT left)
#endif
        {
            Left = left;
            Count = left != null ? left.Count + 1 : 1;
            if (Right != null) Count += Right.Count;
        }
        /// <summary>
        /// 设置右节点并重新计算节点数量
        /// </summary>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void setCheckRightCheckLeftCount(NT? right)
#else
        private void setCheckRightCheckLeftCount(NT right)
#endif
        {
            Right = right;
            Count = right != null ? right.Count + 1 : 1;
            if (Left != null) Count += Left.Count;
        }
        /// <summary>
        /// 设置左节点并重新计算节点数量
        /// </summary>
        /// <param name="left"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setLeftCheckRightCount(NT left)
        {
            Left = left;
            Count = left.Count + (Right != null ? Right.Count + 1 : 1);
        }
        /// <summary>
        /// 设置右节点并重新计算节点数量
        /// </summary>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setRightCheckLeftCount(NT right)
        {
            Right = right;
            Count = right.Count + (Left != null ? Left.Count + 1 : 1);
        }
        /// <summary>
        /// 添加左节点（确定左节点数量为 1）
        /// </summary>
        /// <param name="left"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void addLeft(NT left)
        {
            Left = left;
            ++Count;
        }
        /// <summary>
        /// 添加右节点（确定右节点数量为 1）
        /// </summary>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void addRight(NT right)
        {
            Right = right;
            ++Count;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="node">数据</param>
        /// <returns>替换节点</returns>
#if NetStandard21
        internal NT? TryAdd(ref SetRemoveNode<KT, NT> node)
#else
        internal NT TryAdd(ref SetRemoveNode<KT, NT> node)
#endif
        {
            int cmp = key.CompareTo(node.Value.Key);
            if (cmp < 0)
            {
                if (Right == null)
                {
                    Right = node.Value;
                    ++Count;
                    node.IsNewValue = true;
                    return null;
                }
                var right = Right.TryAdd(ref node);
                if (right != null) Right = right;
                return node.IsNewValue ? checkRight() : null;
            }
            if (cmp != 0)
            {
                if (Left == null)
                {
                    Left = node.Value;
                    ++Count;
                    node.IsNewValue = true;
                    return null;
                }
                var left = Left.TryAdd(ref node);
                if (left != null) Left = left;
                return node.IsNewValue ? checkLeft() : null;
            }
            return null;
        }
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>被删除节点</returns>
#if NetStandard21
        internal NT? Remove(KT key)
#else
        internal NT Remove(KT key)
#endif
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0)
            {
                if (Right != null)
                {
                    var node = Right.Remove(key);
                    if (node != null)
                    {
                        --Count;
                        if (object.ReferenceEquals(Right, node)) Right = node.Remove();
                        return node;
                    }
                }
                return null;
            }
            if (cmp != 0)
            {
                if (Left != null)
                {
                    var node = Left.Remove(key);
                    if (node != null)
                    {
                        --Count;
                        if (object.ReferenceEquals(Left, node)) Left = node.Remove();
                        return node;
                    }
                }
                return null;
            }
            return (NT)this;
        }
        /// <summary>
        /// 删除当前节点
        /// </summary>
        /// <returns>替换当前节点的节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal NT? Remove()
#else
        internal NT Remove()
#endif
        {
            if (Right != null)
            {
                NT node = Right.removeMin();
                if (object.ReferenceEquals(Right, node)) node.set(Left, Count - 1);
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
        private NT removeMin()
        {
            if (Left != null)
            {
                --Count;
                NT node = Left.removeMin();
                if (object.ReferenceEquals(Left, node)) Left = node.Right;
                return node;
            }
            return (NT)this;
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        internal NT At(int index)
        {
            if (Left != null)
            {
                if (index < Left.Count) return Left.At(index);
                if ((index -= Left.Count) == 0) return (NT)this;
            }
            else if (index == 0) return (NT)this;
            return Right.notNull().At(index - 1);
        }

        /// <summary>
        /// 设置节点信息
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected void set(NT? left, NT right, int count)
#else
        protected void set(NT left, NT right, int count)
#endif
        {
            Left = left;
            Right = right;
            Count = count;
        }
        /// <summary>
        /// 设置节点信息
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected void set(NT? left, int count)
#else
        protected void set(NT left, int count)
#endif
        {
            Left = left;
            Count = count;
        }
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        internal int IndexOf(KT key)
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0)
            {
                if (Right != null)
                {
                    int index = Right.IndexOf(key);
                    if (++index != 0) return Left != null ? Left.Count + index : index;
                }
                return -1;
            }
            if (cmp != 0) return Left != null ? Left.IndexOf(key) : -1;
            return Left != null ? Left.Count : 0;
        }
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Number of nodes
        /// 节点数量</returns>
        internal int CountLess(KT key)
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0)
            {
                if (Right != null)
                {
                    int count = Right.CountLess(key) + 1;
                    return Left != null ? Left.Count + count : count;
                }
                return Left != null ? Left.Count + 1 : 1;
            }
            if (Left != null) return cmp != 0 ? Left.CountLess(key) : Left.Count;
            return 0;
        }
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Number of nodes
        /// 节点数量</returns>
        internal int CountThan(KT key)
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0) return Right != null ? Right.CountThan(key) : 0;
            if (cmp != 0)
            {
                if (Left != null)
                {
                    int count = Left.CountThan(key) + 1;
                    return Right != null ? Right.Count + count : count;
                }
                return Right != null ? Right.Count + 1 : 1;
            }
            return Right != null ? Right.Count : 0;
        }
        /// <summary>
        /// 获取第一个大于关键字的节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        internal NT? GetThanNode(KT key)
#else
        internal NT GetThanNode(KT key)
#endif
        {
            int cmp = this.key.CompareTo(key);
            if (cmp < 0) return Right?.GetThanNode(key);
#if NetStandard21
            return Left?.GetThanNode(key) ?? (cmp != 0 ? (NT?)this : null);
#else
            return Left?.GetThanNode(key) ?? (cmp != 0 ? (NT)this : null);
#endif
        }

        /// <summary>
        /// 删除节点计数
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected void checkRemoveCount1(NT? node)
#else
        protected void checkRemoveCount1(NT node)
#endif
        {
            if (node != null) Count -= node.Count;
            --Count;
        }
        /// <summary>
        /// 删除节点计数
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void removeCount1(NT node)
        {
            Count -= node.Count + 1;
        }

        /// <summary>
        /// 清除左节点并重置节点数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? clearLeft()
#else
        protected NT clearLeft()
#endif
        {
            var left = Left;
            Count = 1;
            Left = null;
            return left;
        }
        /// <summary>
        /// 删除左节点计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? removeLeftCount()
#else
        protected NT removeLeftCount()
#endif
        {
            if (Left != null) Count -= Left.Count;
            return Left;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? rightToLeft()
#else
        protected NT rightToLeft()
#endif
        {
            Left = Right;
            Right = null;
            return Left;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT rightToLeft(NT right)
        {
            NT left = Left.notNull();
            Count += right.Count;
            Left = Right;
            Count -= left.Count;
            Right = right;
            return left;
        }
        /// <summary>
        /// 检测左节点的右节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkLeftRight()
        {
            NT left = Left.notNull();
            if (left.Right != null)
            {
                Right = left.Right;
                left.Right = Right.notNull().removeLeftCount();
                left.checkRemoveCount1(Right.notNull().rightToLeft());
            }
            else
            {
                Right = left;
                Left = left.clearLeft();
            }
        }

        /// <summary>
        /// 清除左节点并重置节点数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? clearRight()
#else
        protected NT clearRight()
#endif
        {
            var right = Right;
            Count = 1;
            Right = null;
            return right;
        }
        /// <summary>
        /// 删除左节点计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? removeRightCount()
#else
        protected NT removeRightCount()
#endif
        {
            if (Right != null) Count -= Right.Count;
            return Right;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected NT? leftToRight()
#else
        protected NT leftToRight()
#endif
        {
            Right = Left;
            Left = null;
            return Right;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT leftToRight(NT left)
        {
            NT right = Right.notNull();
            Count += left.Count;
            Right = Left;
            Count -= right.Count;
            Left = left;
            return right;
        }
        /// <summary>
        /// 检测右节点的左节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkRightLeft()
        {
            NT right = Right.notNull();
            if (right.Left != null)
            {
                Left = right.Left;
                right.Left = Left.notNull().removeRightCount();
                right.checkRemoveCount1(Left.notNull().leftToRight());
            }
            else
            {
                Left = right;
                Right = right.clearRight();
            }
        }

        /// <summary>
        /// 获取关键字数组
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="getCount"></param>
        /// <returns></returns>
        internal KT[] GetKeyArray(int skipCount, int getCount)
        {
            if (skipCount < Count)
            {
                PageArray<KT> array = new PageArray<KT> { Array = new KT[Math.Min(Count - skipCount, getCount)], SkipCount = skipCount };
                getArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<KT>.Array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getArraySkip(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.getArraySkip(ref array);
                    if (!array.IsArray && !array.Add(key)) Right.notNull().getArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (!array.Add(key)) Right?.getArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.getArraySkip(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getArray(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                Left.getArray(ref array);
                if (array.IsArray) return;
            }
            if (!array.Add(key)) Right?.getArray(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        internal void GetArraySkip<T>(ref PageArray<KT, T> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.GetArraySkip(ref array);
                    if (!array.IsArray && !array.Add(key)) Right.notNull().getArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (!array.Add(key)) Right?.getArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.GetArraySkip(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getArray<T>(ref PageArray<KT, T> array)
        {
            if (Left != null)
            {
                Left.getArray(ref array);
                if (array.IsArray) return;
            }
            if (!array.Add(key)) Right?.getArray(ref array);
        }
        /// <summary>
        /// 获取获取关键字数组
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="getCount"></param>
        /// <returns></returns>
        internal KT[] GetDescKeyArray(int skipCount, int getCount)
        {
            if (skipCount < Count)
            {
                getCount = Math.Min(Count - skipCount, getCount);
                PageArray<KT> array = new PageArray<KT> { Array = new KT[getCount], SkipCount = Count - (skipCount + getCount), Index = getCount };
                getDescArraySkip(ref array);
                return array.Array;
            }
            return EmptyArray<KT>.Array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getDescArraySkip(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.getDescArraySkip(ref array);
                    if (array.Index != 0 && array.AddDesc(key) != 0) Right.notNull().getDescArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (array.AddDesc(key) != 0) Right?.getDescArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.getDescArraySkip(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getDescArray(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                Left.getDescArray(ref array);
                if (array.Index == 0) return;
            }
            if (array.AddDesc(key) != 0) Right?.getDescArray(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        internal void GetDescArraySkip<T>(ref PageArray<KT, T> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.GetDescArraySkip(ref array);
                    if (array.Index != 0 && array.AddDesc(key) != 0) Right.notNull().getDescArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (array.AddDesc(key) != 0) Right?.getDescArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.GetDescArraySkip(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        private void getDescArray<T>(ref PageArray<KT, T> array)
        {
            if (Left != null)
            {
                Left.getDescArray(ref array);
                if (array.Index == 0) return;
            }
            if (array.AddDesc(key) != 0) Right?.getDescArray(ref array);
        }

#if DEBUG
        /// <summary>
        /// 检查数据正确性
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check(int count)
        {
            return Count == count && check();
        }
        /// <summary>
        /// 检查数据正确性
        /// </summary>
        /// <returns></returns>
        private bool check()
        {
            if (Left != null)
            {
                if (Right != null)
                {
                    return Count == Left.Count + Right.Count + 1 && key.CompareTo(Left.key) > 0 && Left.check() && key.CompareTo(Right.Key) < 0 && Right.check();
                }
                return Count == Left.Count + 1 && key.CompareTo(Left.key) > 0 && Left.check();
            }
            if (Right != null) return Count == Right.Count + 1 && key.CompareTo(Right.Key) < 0 && Right.check();
            return Count == 1;
        }
#endif
    }
}
