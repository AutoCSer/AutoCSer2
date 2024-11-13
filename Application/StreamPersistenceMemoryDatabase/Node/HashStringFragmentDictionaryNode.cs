using AutoCSer.Net;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 HashString 字典 节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HashStringFragmentDictionaryNode<T> : IHashStringFragmentDictionaryNode<T>, ISnapshot<KeyValue<string, T>>
    {
        /// <summary>
        /// 256 基分片 HashString 字典
        /// </summary>
        private readonly FragmentHashStringDictionary256<T> dictionary = new FragmentHashStringDictionary256<T>();
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<string, T>> GetSnapshotArray()
        {
            if (dictionary.Count == 0) return new LeftArray<KeyValue<string, T>>(0);
            LeftArray<KeyValue<string, T>> array = new LeftArray<KeyValue<string, T>>(dictionary.Count);
            foreach (KeyValuePair<HashString, T> value in dictionary.KeyValues) array.Array[array.Length++].Set(value.Key.String, value.Value);
            return array;
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<string, T> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count() { return dictionary.Count; }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<T> TryGetValue(string key)
        {
            var value = default(T);
            if (key != null && dictionary.TryGetValue(key, out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            dictionary.ClearArray();
        }
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryAddBeforePersistence(string key, T value)
        {
            if (key == null || dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(string key, T value)
        {
            return dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBeforePersistence(string key, T value)
        {
            if (key == null) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public bool Set(string key, T value)
        {
            dictionary[key] = value;
            return true;
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(string key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(string key)
        {
            return dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> GetRemoveBeforePersistence(string key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return default(ValueResult<T>);
            return default(ValueResult<ValueResult<T>>);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        public ValueResult<T> GetRemove(string key)
        {
            var value = default(T);
            if (dictionary.Remove(key, out value)) return value;
            return default(ValueResult<T>);
        }
    }
}
