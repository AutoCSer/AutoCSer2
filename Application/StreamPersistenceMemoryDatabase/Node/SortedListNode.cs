﻿using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序列表节点
    /// </summary>
    /// <typeparam name="KT">排序关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public class SortedListNode<KT, VT> : ISortedListNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
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
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<KT, VT>> GetSnapshotArray()
        {
            return ServerNode.GetSnapshotArray(list);
        }
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
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryAddBeforePersistence(KT key, VT value)
        {
            if (key == null || list.ContainsKey(key)) return false;
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
            return list.TryAdd(key, value);
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
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(KT key)
        {
            if (key == null || !list.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return list.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<VT>> GetRemoveBeforePersistence(KT key)
        {
            if (key == null || !list.ContainsKey(key)) return default(ValueResult<VT>);
            return default(ValueResult<ValueResult<VT>>);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            VT value;
            if (list.Remove(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            VT value;
            if (key != null && list.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 删除指定排序索引位置数据 持久化参数检查
        /// </summary>
        /// <param name="index"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveAtBeforePersistence(int index)
        {
            if ((uint)index < (uint)list.Count) return default(ValueResult<bool>);
            return false;
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