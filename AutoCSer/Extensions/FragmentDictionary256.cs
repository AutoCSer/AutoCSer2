using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 字典
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    public class FragmentDictionary256<KT, VT> where KT : IEquatable<KT>
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
                return dictionarys[key.GetHashCode() & 0xff][key];
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
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            foreach (Dictionary<KT, VT> dictionary in dictionarys)
            {
                if (dictionary != null) dictionary.Clear();
            }
            Count = 0;
        }
        /// <summary>
        /// 清除数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearArray()
        {
            Array.Clear(dictionarys, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(KT key, VT value)
        {
            GetOrCreateDictionary(key).Add(key, value);
            ++Count;
        }
        /// <summary>
        /// 根据关键字获取字典，不存在时创建字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Dictionary<KT, VT> GetOrCreateDictionary(KT key)
        {
            int index = key.GetHashCode() & 0xff;
            Dictionary<KT, VT> dictionary = dictionarys[index];
            if (dictionary == null) dictionarys[index] = dictionary = DictionaryCreator<KT>.Create<VT>();
            return dictionary;
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(KT key, VT value)
        {
            int index = key.GetHashCode() & 0xff;
            Dictionary<KT, VT> dictionary = dictionarys[index];
            if (dictionary == null)
            {
                dictionarys[index] = dictionary = DictionaryCreator<KT>.Create<VT>();
            }
            else if (dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            ++Count;
            return true;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="removeValue">被删除数据</param>
        /// <returns>是否存在替换的被删除数据</returns>
        public bool Set(KT key, VT value, out VT removeValue)
        {
            int index = key.GetHashCode() & 0xff;
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
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(KT key)
        {
            Dictionary<KT, VT> dictionary = dictionarys[key.GetHashCode() & 0xff];
            return dictionary != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(KT key)
        {
            Dictionary<KT, VT> dictionary = dictionarys[key.GetHashCode() & 0xff];
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
        public bool Remove(KT key, out VT value)
        {
            Dictionary<KT, VT> dictionary = dictionarys[key.GetHashCode() & 0xff];
            if (dictionary != null)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    --Count;
                    return true;
                }
            }
            else value = default(VT);
            return false;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(KT key, out VT value)
        {
            Dictionary<KT, VT> dictionary;
            return TryGetValue(key, out value, out dictionary);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public bool TryGetValue(KT key, out VT value, out Dictionary<KT, VT> dictionary)
        {
            dictionary = dictionarys[key.GetHashCode() & 0xff];
            if (dictionary != null) return dictionary.TryGetValue(key, out value);
            value = default(VT);
            return false;
        }
    }
}
