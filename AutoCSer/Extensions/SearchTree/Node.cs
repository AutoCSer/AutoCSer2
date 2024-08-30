using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉字典树节点
    /// </summary>
    /// <typeparam name="NT">二叉树节点类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    internal class Node<NT, KT>
        where NT : Node<NT, KT>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 左节点
        /// </summary>
        internal NT Left;
        /// <summary>
        /// 右节点
        /// </summary>
        internal NT Right;
        /// <summary>
        /// 关键字
        /// </summary>
        internal KT Key;
        /// <summary>
        /// 节点数量
        /// </summary>
        internal int Count;
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
            get { return Left.Count - Right.Count; }
        }
        /// <summary>
        /// 设置节点信息
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void set(NT left, NT right, int count)
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
        protected void set(NT left, int count)
        {
            Left = left;
            Count = count;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        internal int IndexOf(ref KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Left != null ? Left.Count : 0;
            if (cmp < 0)
            {
                if (Left != null) return Left.IndexOf(ref key);
            }
            else if (Right != null)
            {
                int index = Right.IndexOf(ref key);
                if (++index != 0) return Left != null ? Left.Count + index : index;
            }
            return -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        internal int CountLess(ref KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Left != null ? Left.Count : 0;
            if (cmp < 0) return Left != null ? Left.CountLess(ref key) : 0;
            if (Right != null)
            {
                return Left != null ? Left.Count + 1 + Right.CountLess(ref key) : (Right.CountLess(ref key) + 1);
            }
            return Left != null ? Left.Count + 1 : 1;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        internal int CountThan(ref KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Right != null ? Right.Count : 0;
            if (cmp > 0) return Right != null ? Right.CountThan(ref key) : 0;
            if (Left != null)
            {
                return Right != null ? Right.Count + 1 + Left.CountThan(ref key) : (Left.CountThan(ref key) + 1);
            }
            return Right != null ? Right.Count + 1 : 1;
        }

        /// <summary>
        /// 删除节点计数
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkRemoveCount1(NT node)
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
        protected NT clearLeft()
        {
            NT left = Left;
            Count = 1;
            Left = null;
            return left;
        }
        /// <summary>
        /// 删除左节点计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT removeLeftCount()
        {
            if (Left != null) Count -= Left.Count;
            return Left;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT rightToLeft()
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
            NT left = Left;
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
            if (Left.Right != null)
            {
                Right = Left.Right;
                Left.Right = Right.removeLeftCount();
                Left.checkRemoveCount1(Right.rightToLeft());
            }
            else
            {
                Right = Left;
                Left = Right.clearLeft();
            }
        }

        /// <summary>
        /// 清除左节点并重置节点数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT clearRight()
        {
            NT right = Right;
            Count = 1;
            Right = null;
            return right;
        }
        /// <summary>
        /// 删除左节点计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT removeRightCount()
        {
            if (Right != null) Count -= Right.Count;
            return Right;
        }
        /// <summary>
        /// 右节点移动到左节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected NT leftToRight()
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
            NT right = Right;
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
            if (Right.Left != null)
            {
                Left = Right.Left;
                Right.Left = Left.removeRightCount();
                Right.checkRemoveCount1(Left.leftToRight());
            }
            else
            {
                Left = Right;
                Right = Left.clearRight();
            }
        }
    }
}
