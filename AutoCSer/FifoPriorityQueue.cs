using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 先进先出优先队列
    /// </summary>
    /// <typeparam name="KT">键值类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    [RemoteType]
    public sealed class FifoPriorityQueue<KT, VT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        internal sealed class Node
        {
            /// <summary>
            /// 前一个节点
            /// </summary>
#if NetStandard21
            public Node? Previous;
#else
            public Node Previous;
#endif
            /// <summary>
            /// 后一个节点
            /// </summary>
#if NetStandard21
            public Node? Next;
#else
            public Node Next;
#endif
            /// <summary>
            /// 键值
            /// </summary>
            public KT Key;
            /// <summary>
            /// 数据
            /// </summary>
            public VT Value;
            /// <summary>
            /// 数据节点
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <param name="previous"></param>
#if NetStandard21
            internal Node(KT key, VT value, Node? previous)
#else
            internal Node(KT key, VT value, Node previous)
#endif
            {
                Key = key;
                Value = value;
                Previous = previous;
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        private Dictionary<KT, Node> dictionary;
        /// <summary>
        /// 获取所有关键字
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get
            {
                for (var node = header; node != null; node = node.Next) yield return node.Key;
            }
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                for (var node = header; node != null; node = node.Next) yield return node.Value;
            }
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        public IEnumerable<KeyValue<KT, VT>> KeyValues
        {
            get
            {
                for (var node = header; node != null; node = node.Next) yield return new KeyValue<KT, VT>(node.Key, node.Value);
            }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 头节点
        /// </summary>
#if NetStandard21
        private Node? header;
#else
        private Node header;
#endif
        /// <summary>
        /// 尾节点
        /// </summary>
#if NetStandard21
        private Node? end;
#else
        private Node end;
#endif
        /// <summary>
        /// 数据对象
        /// </summary>
        /// <param name="key">查询键值</param>
        /// <returns>数据对象</returns>
        public VT this[KT key]
        {
            get
            {
                var node = GetNode(key);
                if (node != null) return node.Value;
                throw new KeyNotFoundException();
            }
            set { Set(key, value); }
        }
        /// <summary>
        /// 先进先出优先队列
        /// </summary>
        /// <param name="dictionaryCapacity">字典初始化容器尺寸</param>
        public FifoPriorityQueue(int dictionaryCapacity = 0)
        {
            dictionary = DictionaryCreator<KT>.Create<Node>(dictionaryCapacity);
        }
        ///// <summary>
        ///// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        ///// </summary>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public void Empty()
        //{
        //    dictionary.Clear();
        //    header = end = null;
        //}
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            dictionary.Clear();
            header = end = null;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="nullValue">失败空值</param>
        /// <returns>数据对象</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT Get(KT key, VT nullValue)
        {
            var node = GetNode(key);
            return node != null ? node.Value : nullValue;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">目标数据对象</param>
        /// <returns>是否获取成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetValue(KT key, out VT value)
#endif
        {
            var node = GetNode(key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>数据对象</returns>
#if NetStandard21
        internal Node? GetNode(KT key)
#else
        internal Node GetNode(KT key)
#endif
        {
            var node = default(Node);
            if (dictionary.TryGetValue(key, out node))
            {
                if (node != end)
                {
                    var previous = node.Previous;
                    if (previous == null) (header = node.Next.notNull()).Previous = null;
                    else (previous.Next = node.Next).notNull().Previous = previous;
                    end.notNull().Next = node;
                    node.Previous = end;
                    node.Next = null;
                    end = node;
                }
            }
            return node;
        }
        /// <summary>
        /// 获取数据(不调整位置)
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value"></param>
        /// <returns>数据对象</returns>
#if NetStandard21
        public bool TryGetOnly(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetOnly(KT key, out VT value)
#endif
        {
            var node = default(Node);
            if (dictionary.TryGetValue(key, out node))
            {
                value = node.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据对象</param>
        /// <returns>被替换的数据对象,没有返回default(VT)</returns>
#if NetStandard21
        public VT? Set(KT key, VT value)
#else
        public VT Set(KT key, VT value)
#endif
        {
            var node = GetNode(key);
            if (node != null)
            {
                VT oldValue = node.Value;
                node.Value = value;
                return oldValue;
            }
            else
            {
                UnsafeAdd(key, value);
                return default(VT);
            }
        }
        /// <summary>
        /// 设置数据(不调整位置)
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetOnly(KT key, VT value)
        {
            var node = default(Node);
            if (dictionary.TryGetValue(key, out node)) node.Value = value;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeAdd(KT key, VT value)
        {
            Node node = new Node(key, value, end);
            dictionary.Add(key, node);
            if (end == null) header = end = node;
            else
            {
                end.Next = node;
                end = node;
            }
        }
        /// <summary>
        /// 弹出一个节点
        /// </summary>
        /// <returns></returns>
        internal Node UnsafePopNode()
        {
            var node = header.notNull();
            if ((header = node.Next) == null) end = null;
            else header.Previous = null;
            dictionary.Remove(node.Key);
            return node;
        }
        /// <summary>
        /// 弹出一个值
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Pop()
        {
            if (header != null) UnsafePopNode();
        }
        /// <summary>
        /// 弹出一个值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool TryPopValue([MaybeNullWhen(false)] out VT value)
#else
        public bool TryPopValue(out VT value)
#endif
        {
            if (header != null)
            {
                value = UnsafePopNode().Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 弹出一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool TryPopNode([MaybeNullWhen(false)] out Node node)
#else
        internal bool TryPopNode(out Node node)
#endif
        {
            if (header != null)
            {
                node = UnsafePopNode();
                return true;
            }
            node = null;
            return false;
        }
        /// <summary>
        /// 弹出一个值
        /// </summary>
        /// <returns>值</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal VT UnsafePopValue()
        {
            return UnsafePopNode().Value;
        }
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">被删除数据对象</param>
        /// <returns>是否删除了数据对象</returns>
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            var node = default(Node);
            if (dictionary.Remove(key, out node))
            {
                if (node.Previous == null)
                {
                    header = node.Next;
                    if (header == null) end = null;
                    else header.Previous = null;
                }
                else if (node.Next == null) (end = node.Previous).Next = null;
                else
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                }
                value = node.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        /// <summary>
        /// 尝试获取第一个节点数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public bool TryGetHeader([MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetHeader(out VT value)
#endif
        {
            var node = header;
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            value = default(VT);
            return false;
        }
        ///// <summary>
        ///// 尝试获取第一个节点数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public bool TryGetHeader(out keyType key, out valueType value)
        //{
        //    Node node = header;
        //    if (node != null)
        //    {
        //        key = node.Key;
        //        value = node.Value;
        //        return true;
        //    }
        //    key = default(keyType);
        //    value = default(valueType);
        //    return false;
        //}
    }
}
