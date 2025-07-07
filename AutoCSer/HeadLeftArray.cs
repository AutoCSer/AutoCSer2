using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 带头节点的数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HeadLeftArray<T>
    {
        /// <summary>
        /// Head node
        /// </summary>
        private T head;
        /// <summary>
        /// Head node
        /// </summary>
        public T Head { get { return head; } }
        /// <summary>
        /// 其它节点集合
        /// </summary>
        internal LeftArray<T> Array;
        /// <summary>
        /// 返回所有数据
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                yield return head;
                foreach(T value in Array) yield return value;
            }
        }
        /// <summary>
        /// 节点总数
        /// </summary>
        public int Count { get { return Array.Length + 1; } }
        /// <summary>
        /// 带头节点的数组子串
        /// </summary>
        /// <param name="head">头节点</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public HeadLeftArray(T head, int capacity = 0)
        {
            this.head = head;
            Array = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// 带头节点的数组子串
        /// </summary>
        /// <param name="head">头节点</param>
        /// <param name="array">其它节点集合</param>
        internal HeadLeftArray(T head, ref LeftArray<T> array)
        {
            this.head = head;
            Array = array;
        }
        /// <summary>
        /// 添加其它节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            Array.Add(value);
        }
        /// <summary>
        /// 设置头节点，并将原头节点添加为其它节点
        /// </summary>
        /// <param name="value">新的头节点</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddHead(T value)
        {
            Array.Add(head);
            head = value;
        }
        ///// <summary>
        ///// 数据最后一个元素设置为头节点
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal bool ArrayEndToHead()
        //{
        //    if (Array.Length == 0) return false;
        //    head = Array.Pop();
        //    return true;
        //}
        /// <summary>
        /// 将数组指定位置元素设置为头节点
        /// </summary>
        /// <param name="index"></param>
        internal void ArrayToHead(int index)
        {
            head = Array.Array[index];
            Array.RemoveToEnd(index);
        }
    }
}
