﻿using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public class DictionaryNode<KT, VT> : ContextNode<IDictionaryNode<KT, VT>>, IDictionaryNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private Dictionary<KT, VT> dictionary;
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public DictionaryNode(int capacity = 0)
        {
            dictionary = DictionaryCreator<KT>.Create<VT>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<KT, VT>> GetSnapshotArray()
        {
            return ServerNode.GetSnapshotArray(dictionary);
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 清除所有数据并重建容器 持久化参数检查
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool RenewBeforePersistence(int capacity)
        {
            streamPersistenceMemoryDatabaseService.SetBeforePersistenceMethodParameterCustomSessionObject(DictionaryCreator<KT>.Create<VT>(capacity));
            return true;
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            dictionary = ((Dictionary<KT, VT>)streamPersistenceMemoryDatabaseService?.GetBeforePersistenceMethodParameterCustomSessionObject()) ?? DictionaryCreator<KT>.Create<VT>(capacity);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dictionary.Count;
        }
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryAddBeforePersistence(KT key, VT value)
        {
            if (key == null || dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBeforePersistence(KT key, VT value)
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
        public bool Set(KT key, VT value)
        {
            dictionary[key] = value;
            return true;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            VT value;
            if (key != null && dictionary.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(VT value)
        {
            return dictionary.ContainsValue(value);
        }
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(KT key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<VT>> GetRemoveBeforePersistence(KT key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return default(ValueResult<VT>);
            return default(ValueResult<ValueResult<VT>>);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            VT value;
            if (dictionary.Remove(key, out value)) return value;
            return default(ValueResult<VT>);
        }
    }
}
