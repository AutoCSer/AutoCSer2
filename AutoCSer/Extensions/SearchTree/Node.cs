using AutoCSer.Extensions;
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
        internal KT Key;
        /// <summary>
        /// 节点数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 二叉字典树节点
        /// </summary>
        /// <param name="key"></param>
        internal Node(KT key)
        {
            Key = key;
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
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        internal int IndexOf(KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Left != null ? Left.Count : 0;
            if (cmp < 0)
            {
                if (Left != null) return Left.IndexOf(key);
            }
            else if (Right != null)
            {
                int index = Right.IndexOf(key);
                if (++index != 0) return Left != null ? Left.Count + index : index;
            }
            return -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        internal int CountLess(KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Left != null ? Left.Count : 0;
            if (cmp < 0) return Left != null ? Left.CountLess(key) : 0;
            if (Right != null)
            {
                return Left != null ? Left.Count + 1 + Right.CountLess(key) : (Right.CountLess(key) + 1);
            }
            return Left != null ? Left.Count + 1 : 1;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        internal int CountThan(KT key)
        {
            int cmp = key.CompareTo(Key);
            if (cmp == 0) return Right != null ? Right.Count : 0;
            if (cmp > 0) return Right != null ? Right.CountThan(key) : 0;
            if (Left != null)
            {
                return Right != null ? Right.Count + 1 + Left.CountThan(key) : (Left.CountThan(key) + 1);
            }
            return Right != null ? Right.Count + 1 : 1;
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
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        internal void GetArraySkip(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.GetArraySkip(ref array);
                    if (!array.IsArray && !array.Add(Key)) Right.notNull().getArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (!array.Add(Key)) Right?.getArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.GetArraySkip(ref array);
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
            if (!array.Add(Key)) Right?.getArray(ref array);
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
                    if (!array.IsArray && !array.Add(Key)) Right.notNull().getArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (!array.Add(Key)) Right?.getArray(ref array);
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
            if (!array.Add(Key)) Right?.getArray(ref array);
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="array"></param>
        internal void GetDescArraySkip(ref PageArray<KT> array)
        {
            if (Left != null)
            {
                int count = Left.Count;
                if (count > array.SkipCount)
                {
                    Left.GetDescArraySkip(ref array);
                    if (array.Index != 0 && array.AddDesc(Key) != 0) Right.notNull().getDescArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (array.AddDesc(Key) != 0) Right?.getDescArray(ref array);
                return;
            }
            --array.SkipCount;
            Right?.GetDescArraySkip(ref array);
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
            if (array.AddDesc(Key) != 0) Right?.getDescArray(ref array);
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
                    if (array.Index != 0 && array.AddDesc(Key) != 0) Right.notNull().getDescArray(ref array);
                    return;
                }
                array.SkipCount -= count;
            }
            if (array.SkipCount == 0)
            {
                if (array.AddDesc(Key) != 0) Right?.getDescArray(ref array);
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
            if (array.AddDesc(Key) != 0) Right?.getDescArray(ref array);
        }
    }
}
