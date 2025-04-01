using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 快照字典
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    public sealed class FragmentSnapshotDictionary256<KT, VT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
#if NetStandard21
        internal readonly SnapshotDictionary<KT, VT>?[] Dictionarys = new SnapshotDictionary<KT, VT>?[256];
#else
        internal readonly SnapshotDictionary<KT, VT>[] Dictionarys = new SnapshotDictionary<KT, VT>[256];
#endif
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get; internal set; }
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public VT this[KT key]
        {
            get
            {
                return Dictionarys[getIndex(key)].notNull()[key];
            }
            set
            {
                SnapshotDictionary<KT, VT> dictionary = GetOrCreateDictionary(key);
                int count = dictionary.Count;
                dictionary[key] = value;
                Count += dictionary.Count - count;
            }
        }
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<BinarySerializeKeyValue<KT, VT>> KeyValues
        {
            get
            {
                foreach (var dictionary in Dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (BinarySerializeKeyValue<KT, VT> value in dictionary.KeyValues) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get
            {
                foreach (var dictionary in Dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (KT key in dictionary.Keys) yield return key;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                foreach (var dictionary in Dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (VT value in dictionary.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<KeyValue<KT, VT>> GetKeyValueSnapshot() { return new FragmentSnapshotDictionaryEnumerable256<KT, VT>(this); }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>> GetBinarySerializeKeyValueSnapshot() { return new FragmentSnapshotDictionaryEnumerable256<KT, VT>(this); }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<VT> GetValueSnapshot() { return new FragmentSnapshotDictionaryEnumerable256<KT, VT>(this); }
        /// <summary>
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            foreach (var dictionary in Dictionarys) dictionary?.ClearArray();
            Count = 0;
        }
        /// <summary>
        /// 清除计数位置信息
        /// </summary>
        internal void ClearCount()
        {
            foreach (var dictionary in Dictionarys) dictionary?.ClearCount();
            Count = 0;
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearArray()
        {
            Array.Clear(Dictionarys, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// 获取分片索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int getIndex(KT key)
        {
            return key.GetHashCode() & 0xff;
        }
        /// <summary>
        /// 根据关键字获取字典，不存在时创建字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal SnapshotDictionary<KT, VT> GetOrCreateDictionary(KT key)
        {
            int index = getIndex(key);
            var dictionary = Dictionarys[index];
            if (dictionary == null) Dictionarys[index] = dictionary = new SnapshotDictionary<KT, VT>();
            return dictionary;
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(KT key, VT value)
        {
            if (GetOrCreateDictionary(key).TryAdd(key, value))
            {
                ++Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="removeValue">被删除数据</param>
        /// <returns>是否存在替换的被删除数据</returns>
#if NetStandard21
        public bool Set(KT key, VT value, [MaybeNullWhen(false)] out VT removeValue)
#else
        public bool Set(KT key, VT value, out VT removeValue)
#endif
        {
            int index = getIndex(key);
            var dictionary = Dictionarys[index];
            if (dictionary == null)
            {
                Dictionarys[index] = dictionary = new SnapshotDictionary<KT, VT>();
                removeValue = default(VT);
            }
            else if (dictionary.TryGetValue(key, out removeValue))
            {
                dictionary[key] = value;
                return true;
            }
            dictionary.Set(key, value);
            ++Count;
            return false;
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            var dictionary = Dictionarys[getIndex(key)];
            return dictionary != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(KT key)
        {
            var dictionary = Dictionarys[getIndex(key)];
            if (dictionary != null && dictionary.Remove(key))
            {
                --Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在关键字</returns>
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            var dictionary = Dictionarys[getIndex(key)];
            if (dictionary != null)
            {
                if (dictionary.Remove(key, out value))
                {
                    --Count;
                    return true;
                }
            }
            else value = default(VT);
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
            foreach (KT key in keys)
            {
                if (key != null && Remove(key)) ++count;
            }
            return count;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool TryGetValue(KT key, out VT value)
#endif
        {
            var dictionary = default(SnapshotDictionary<KT, VT>);
            return TryGetValue(key, out value, out dictionary);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value, [MaybeNullWhen(false)] out SnapshotDictionary<KT, VT> dictionary)
#else
        internal bool TryGetValue(KT key, out VT value, out SnapshotDictionary<KT, VT> dictionary)
#endif
        {
            dictionary = Dictionarys[getIndex(key)];
            if (dictionary != null) return dictionary.TryGetValue(key, out value);
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
    }
}
