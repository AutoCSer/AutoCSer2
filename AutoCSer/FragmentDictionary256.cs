using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 字典
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    [RemoteType]
    public sealed class FragmentDictionary256<KT, VT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly Dictionary<KT, VT>[] dictionarys = new Dictionary<KT, VT>[256];
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
                return dictionarys[getIndex(key)][key];
            }
            set
            {
                Dictionary<KT, VT> dictionary = GetOrCreateDictionary(key);
                int count = dictionary.Count;
                dictionary[key] = value;
                Count += dictionary.Count - count;
            }
        }
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<KeyValuePair<KT, VT>> KeyValues
        {
            get
            {
                foreach (Dictionary<KT, VT> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (KeyValuePair<KT, VT> value in dictionary) yield return value;
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
                foreach (Dictionary<KT, VT> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (KT key in dictionary.Keys) yield return key;
                    }
                }
            }
        }
        /// <summary>
        /// The data collection
        /// 数据集合
        /// </summary>
        public IEnumerable<VT> Values
        {
            get
            {
                foreach (Dictionary<KT, VT> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (VT value in dictionary.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// Clear the data (retain the fragmented array)
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            foreach (Dictionary<KT, VT> dictionary in dictionarys) dictionary?.Clear();
            Count = 0;
        }
        /// <summary>
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearArray()
        {
            Array.Clear(dictionarys, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(KT key, VT value)
        {
            GetOrCreateDictionary(key).Add(key, value);
            ++Count;
        }
        /// <summary>
        /// 获取分片索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int getIndex(KT key)
        {
            return key.GetHashCode() & 0xff;
        }
        /// <summary>
        /// 根据关键字获取字典，不存在时创建字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Dictionary<KT, VT> GetOrCreateDictionary(KT key)
        {
            int index = getIndex(key);
            Dictionary<KT, VT> dictionary = dictionarys[index];
            if (dictionary == null) dictionarys[index] = dictionary = DictionaryCreator<KT>.Create<VT>();
            return dictionary;
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(KT key, VT value)
        {
            int index = getIndex(key);
            Dictionary<KT, VT> dictionary = dictionarys[index];
            if (dictionary == null)
            {
                dictionarys[index] = dictionary = DictionaryCreator<KT>.Create<VT>();
                dictionary.Add(key, value);
                ++Count;
                return true;
            }
            if (dictionary.TryAdd(key, value))
            {
                ++Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set the data
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
            Dictionary<KT, VT> dictionary = dictionarys[index];
            if (dictionary == null)
            {
                dictionarys[index] = dictionary = DictionaryCreator<KT>.Create<VT>();
                removeValue = default(VT);
            }
            else if (dictionary.TryGetValue(key, out removeValue))
            {
                dictionary[key] = value;
                return true;
            }
            dictionary.Add(key, value);
            ++Count;
            return false;
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            Dictionary<KT, VT> dictionary = dictionarys[getIndex(key)];
            return dictionary != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            Dictionary<KT, VT> dictionary = dictionarys[getIndex(key)];
            if (dictionary != null && dictionary.Remove(key))
            {
                --Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
#if NetStandard21
        public bool Remove(KT key, [MaybeNullWhen(false)] out VT value)
#else
        public bool Remove(KT key, out VT value)
#endif
        {
            Dictionary<KT, VT> dictionary = dictionarys[getIndex(key)];
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
        /// Delete the matching data based on the keyword collection
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
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
        /// Get data based on keywords
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
            var dictionary = default(Dictionary<KT, VT>);
            return TryGetValue(key, out value, out dictionary);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool TryGetValue(KT key, [MaybeNullWhen(false)] out VT value, [MaybeNullWhen(false)] out Dictionary<KT, VT> dictionary)
#else
        internal bool TryGetValue(KT key, out VT value, out Dictionary<KT, VT> dictionary)
#endif
        {
            dictionary = dictionarys[getIndex(key)];
            if (dictionary != null) return dictionary.TryGetValue(key, out value);
            value = default(VT);
            return false;
        }
        /// <summary>
        /// Get the matching data array based on the keyword collection
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
        ///// <summary>
        ///// 获取数据集合
        ///// </summary>
        ///// <returns>数据集合</returns>
        //public LeftArray<KeyValue<KT, VT>> GetKeyValueArray()
        //{
        //    if (Count == 0) return new LeftArray<KeyValue<KT, VT>>(0);
        //    LeftArray<KeyValue<KT, VT>> array = new LeftArray<KeyValue<KT, VT>>(Count);
        //    foreach (Dictionary<KT, VT> dictionary in dictionarys)
        //    {
        //        if (dictionary != null)
        //        {
        //            foreach (KeyValuePair<KT, VT> value in dictionary) array.Array[array.Length++].Set(value.Key, value.Value);
        //        }
        //    }
        //    return array;
        //}
    }
}
