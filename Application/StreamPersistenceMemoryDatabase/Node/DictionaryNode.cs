using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public sealed class DictionaryNode<KT, VT> : IDictionaryNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
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
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return dictionary.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult(KeyValue<KT, VT>[] snapshotArray, object customObject)
        {
            return ServerNode.GetSnapshotResult(dictionary, snapshotArray);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<KeyValue<KT, VT>> array, ref LeftArray<KeyValue<KT, VT>> newArray) { }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            dictionary = DictionaryCreator<KT>.Create<VT>(capacity);
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
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public bool Set(KT key, VT value)
        {
            if (key != null)
            {
                dictionary[key] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
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
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (dictionary.Remove(key, out value)) return value;
            }
            return default(ValueResult<VT>);
        }
    }
}
