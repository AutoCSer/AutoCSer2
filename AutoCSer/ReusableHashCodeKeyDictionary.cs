using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [RemoteType]
    public sealed class ReusableHashCodeKeyDictionary<T> : ReusableDictionary<T>
    {
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<KeyValue<uint, T>> KeyValues
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<T> node in Nodes)
                    {
                        yield return new KeyValue<uint, T>(node.HashCode, node.Value);
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<uint> Keys
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<T> node in Nodes)
                    {
                        yield return node.HashCode;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                if (Count != 0)
                {
                    int index = Count;
                    foreach (ReusableHashNode<T> node in Nodes)
                    {
                        yield return node.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public T this[int key]
        {
            get { return this[(uint)key]; }
            set { Set((uint)key, value); }
        }
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public T this[uint key]
        {
            get
            {
                var value = default(T);
                if (TryGetValue(key, out value)) return value;
                throw new IndexOutOfRangeException();
            }
            set { Set(key, value); }
        }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public ReusableHashCodeKeyDictionary(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType) { }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否获取成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out T value, bool isRoll = false)
#else
        public bool TryGetValue(int key, out T value, bool isRoll = false)
#endif
        {
            return TryGetValue((uint)key, out value, isRoll);
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否获取成功</returns>
#if NetStandard21
        public bool TryGetValue(uint key, [MaybeNullWhen(false)] out T value, bool isRoll = false)
#else
        public bool TryGetValue(uint key, out T value, bool isRoll = false)
#endif
        {
            if (Count != 0)
            {
                int hashIndex = (int)CapacityDivision.GetMod(key), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == key && node.Source == hashIndex)
                    {
                        value = node.Value;
                        if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return true;
                        changeIndex(nodeIndex);
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (nodeIndex = node.Next; nodeIndex < Count; nodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nodeIndex];
#else
                            node = Nodes[nodeIndex];
#endif
                            if (node.HashCode == key)
                            {
                                value = node.Value;
                                if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return true;
                                changeIndex(nodeIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(int key, T value, bool isRoll = false)
        {
            return set((uint)key, value, isRoll, false);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Set(uint key, T value, bool isRoll = false)
        {
            return set(key, value, isRoll, false);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(int key, T value, bool isRoll = false)
        {
            return set((uint)key, value, isRoll, true);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(uint key, T value, bool isRoll = false)
        {
            return set(key, value, isRoll, true);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRoll">更新时是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <param name="isAdd"></param>
        /// <returns>是否新增数据</returns>
        private bool set(uint key, T value, bool isRoll, bool isAdd)
        {
            int hashIndex = (int)CapacityDivision.GetMod(key);
            if (Count != 0)
            {
                int nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count && Nodes[nodeIndex].Source == (uint)hashIndex)
                {
                    do
                    {
#if NetStandard21
                        ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                        ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                        if (key == node.HashCode)
                        {
                            if (!isAdd) Nodes[nodeIndex].Value = value;
                            if (!isRoll || groupType != ReusableDictionaryGroupTypeEnum.Roll) return false;
                            changeIndex(nodeIndex);
                            return false;
                        }
                        if (node.Next == int.MaxValue)
                        {
                            if (Count != CapacityDivision.Divisor)
                            {
                                Nodes[Count].Set((uint)nodeIndex | 0x80000000U, key, value);
                                Nodes[nodeIndex].Next = Count++;
                            }
                            else
                            {
                                resize();
                                add(key, value);
                            }
                            return true;
                        }
                        nodeIndex = node.Next;
                    }
                    while (true);
                }
                if (Count != CapacityDivision.Divisor)
                {
                    Nodes[Count].Set((uint)hashIndex, key, value);
                    Nodes[hashIndex].HashIndex = Count++;
                }
                else
                {
                    resize();
                    add(key, value);
                }
            }
            else
            {
                Nodes[0].Set((uint)hashIndex, key, value);
                Nodes[hashIndex].HashIndex = 0;
                Count = 1;
            }
            return true;
        }

        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(int key, bool isRoll = false)
        {
            var value = default(T);
            return TryGetValue((uint)key, out value, isRoll);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isRoll">是否尝试修改索引位置（用于优先级淘汰策略）</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(uint key, bool isRoll = false)
        {
            var value = default(T);
            return TryGetValue(key, out value, isRoll);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(int key)
        {
            var value = default(T);
            return Remove((uint)key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(uint key)
        {
            var value = default(T);
            return Remove(key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool Remove(int key, [MaybeNullWhen(false)] out T value)
#else
        public bool Remove(int key, out T value)
#endif
        {
            return Remove((uint)key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
#if NetStandard21
        public bool Remove(uint key, [MaybeNullWhen(false)] out T value)
#else
        public bool Remove(uint key, out T value)
#endif
        {
            if (Count != 0)
            {
                int hashIndex = (int)CapacityDivision.GetMod(key), nodeIndex = Nodes[hashIndex].HashIndex;
                if (nodeIndex < Count)
                {
#if NetStandard21
                    ref ReusableHashNode<T> node = ref Nodes[nodeIndex];
#else
                    ReusableHashNode<T> node = Nodes[nodeIndex];
#endif
                    if (node.HashCode == key && node.Source == hashIndex)
                    {
                        value = node.Value;
                        if (node.Next == int.MaxValue)
                        {
                            if (nodeIndex != removeCount()) remove(nodeIndex, node.HashIndex);
                        }
                        else
                        {
                            Nodes[node.Next].Source = (uint)hashIndex;
                            Nodes[hashIndex].HashIndex = node.Next;
                            if (nodeIndex != removeCount()) remove(nodeIndex, Nodes[nodeIndex].HashIndex);
                        }
                        return true;
                    }
                    if (node.Source == hashIndex)
                    {
                        for (int nextNodeIndex = node.Next; nextNodeIndex < Count; nodeIndex = nextNodeIndex, nextNodeIndex = node.Next)
                        {
#if NetStandard21
                            node = ref Nodes[nextNodeIndex];
#else
                            node = Nodes[nextNodeIndex];
#endif
                            if (node.HashCode == key)
                            {
                                value = node.Value;
                                if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(nodeIndex);
                                Nodes[nodeIndex].Next = node.Next;
                                if (nextNodeIndex != removeCount()) remove(nextNodeIndex, node.HashIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveRoll()
        {
            KeyValue<uint, T> value;
            RemoveRoll(out value);
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveRoll(out KeyValue<int, T> value)
        {
            KeyValue<uint, T> keyValue;
            if (RemoveRoll(out keyValue))
            {
                value = new KeyValue<int, T>((int)keyValue.Key, keyValue.Value);
                return true;
            }
            value = default(KeyValue<int, T>);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
        public bool RemoveRoll(out KeyValue<uint, T> value)
        {
            if (Count != 0 && groupType == ReusableDictionaryGroupTypeEnum.Roll)
            {
                if (rollIndex >= Count) rollIndex = 0;
#if NetStandard21
                ref ReusableHashNode<T> node = ref Nodes[rollIndex];
#else
                ReusableHashNode<T> node = Nodes[rollIndex];
#endif
                value = new KeyValue<uint, T>(node.HashCode, node.Value);
                int hashIndex = (int)CapacityDivision.GetMod(node.HashCode);
                if (node.SourceHigh == 0)
                {
                    if (node.Next != int.MaxValue)
                    {
                        Nodes[node.Next].Source = (uint)hashIndex;
                        Nodes[hashIndex].HashIndex = node.Next;
                        if (rollIndex != removeCount())
                        {
                            remove(rollIndex, Nodes[rollIndex].HashIndex);
                            if (++rollIndex == Count) rollIndex = 0;
                        }
                        else rollIndex = 0;
                        return true;
                    }
                }
                else
                {
                    Nodes[node.SourceIndex].Next = node.Next;
                    if (node.Next != int.MaxValue) Nodes[node.Next].SetNextSource(node.SourceIndex);
                }
                if (rollIndex != removeCount())
                {
                    remove(rollIndex, node.HashIndex);
                    if (++rollIndex == Count) rollIndex = 0;
                }
                else rollIndex = 0;
                return true;
            }
            value = default(KeyValue<uint, T>);
            return false;
        }
        /// <summary>
        /// 删除滚动索引位置数据
        /// </summary>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在数据，非 Roll 类型也返回 false</returns>
#if NetStandard21
        public bool RemoveRoll([MaybeNullWhen(false)] out T value)
#else
        public bool RemoveRoll(out T value)
#endif
        {
            KeyValue<uint, T> keyValue;
            if (RemoveRoll(out keyValue))
            {
                value = keyValue.Value;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
