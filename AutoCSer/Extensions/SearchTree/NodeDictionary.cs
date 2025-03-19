using AutoCSer.Configuration;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉搜索树字典
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据节点类型</typeparam>
    [RemoteType]
    public sealed class NodeDictionary<KT, VT>
        where KT : IComparable<KT>
        where VT : Node<VT, KT>
    {
        /// <summary>
        /// 根节点
        /// </summary>
#if NetStandard21
        internal VT? Boot;
#else
        internal VT Boot;
#endif
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Count
        {
            get { return Boot != null ? Boot.Count : 0; }
        }
        /// <summary>
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
        /// 获取第一组数据
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
        /// 获取最后一组数据
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
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Boot = null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
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
        /// 设置数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>是否添加了关键字</returns>
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
        /// 设置数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="removeValue">方法返回 false 时表示被移除数据</param>
        /// <returns>是否添加了关键字</returns>
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
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
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
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
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
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>删除关键字数量</returns>
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
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
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
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            return Boot != null && Boot.Get(key) != null;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否成功</returns>
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
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(KT key)
        {
            return Boot != null ? Boot.IndexOf(key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountLess(KT key)
        {
            return Boot != null ? Boot.CountLess(key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int CountThan(KT key)
        {
            return Boot != null ? Boot.CountThan(key) : 0;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT At(int index)
        {
            if (Boot != null && (uint)index < (uint)Boot.Count) return Boot.At(index);
            throw new IndexOutOfRangeException();
        }
        /// <summary>
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
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KT[] GetKeyRange(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetKeyArray(skipCount, getCount) : EmptyArray<KT>.Array;
        }
        /// <summary>
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KT[] GetKeyRangeDesc(int skipCount, int getCount)
        {
            return Boot != null ? Boot.GetDescKeyArray(skipCount, getCount) : EmptyArray<KT>.Array;
        }

#if DEBUG
        /// <summary>
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
