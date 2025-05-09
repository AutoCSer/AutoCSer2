﻿using AutoCSer.Extensions;
using AutoCSer.SearchTree;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序列表节点
    /// </summary>
    /// <typeparam name="KT">排序关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
#if AOT
    public abstract class SortedListNode<KT, VT> : ISnapshot<KeyValue<KT, VT>>
#else
    public sealed class SortedListNode<KT, VT> : ISortedListNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
#endif
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 排序列表
        /// </summary>
        private SortedList<KT, VT> list;
        /// <summary>
        /// 排序列表
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public SortedListNode(int capacity)
        {
            list = new SortedList<KT, VT>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return list.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult(KeyValue<KT, VT>[] snapshotArray, object customObject)
        {
            return ServerNode.GetSnapshotResult(list, snapshotArray);
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
            list[value.Key] = value.Value;
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return list.Count;
        }
        /// <summary>
        /// 获取容器大小
        /// </summary>
        /// <returns></returns>
        public int GetCapacity()
        {
            return list.Capacity;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && list.TryAdd(key, value);
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && list.ContainsKey(key);
        }
        /// <summary>
        /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(VT value)
        {
            return list.ContainsValue(value);
        }
        /// <summary>
        /// 获取关键字排序位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns>负数表示没有找到关键字</returns>
        public int IndexOfKey(KT key)
        {
            return key != null ? list.IndexOfKey(key) : -1;
        }
        /// <summary>
        /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>负数表示没有找到匹配数据</returns>
        public int IndexOfValue(VT value)
        {
            return list.IndexOfValue(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return key != null && list.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (list.Remove(key, out value)) return value;
            }
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && list.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 删除指定排序索引位置数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>索引超出范围返回 false</returns>
        public bool RemoveAt(int index)
        {
            if ((uint)index < (uint)list.Count)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}
