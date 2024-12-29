using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二叉搜索树字典节点
    /// </summary>
    /// <typeparam name="KT">排序关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public class SearchTreeDictionaryNode<KT, VT> : ISearchTreeDictionaryNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 二叉搜索树字典
        /// </summary>
        private AutoCSer.SearchTree.Dictionary<KT, VT> dictionary;
        /// <summary>
        /// 二叉搜索树字典
        /// </summary>
        public SearchTreeDictionaryNode()
        {
            dictionary = new SearchTree.Dictionary<KT, VT>();
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
            return new SnapshotResult<KeyValue<KT, VT>>(snapshotArray, dictionary.KeyValues, dictionary.Count);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<KeyValue<KT, VT>> array, ref LeftArray<KeyValue<KT, VT>> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary.Set(value.Key, value.Value);
        }
        /// <summary>
        /// 获取节点数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dictionary.Count;
        }
        /// <summary>
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        public int GetHeight()
        {
            return dictionary.Height;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        public bool Set(KT key, VT value)
        {
            return key != null && dictionary.Set(key, value);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key);
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>被删除数据</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (dictionary.Remove(ref key, out value)) return value;
            }
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>目标数据</returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && dictionary.TryGetValue(ref key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        public int IndexOf(KT key)
        {
            return key != null ? dictionary.IndexOf(key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        public int CountLess(KT key)
        {
            return key != null ? dictionary.CountLess(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        public int CountThan(KT key)
        {
            return key != null ? dictionary.CountThan(ref key) : -1;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetKeyValueByIndex(int index)
        {
            if ((uint)index < (uint)dictionary.Count) return dictionary.At(index);
            return default(ValueResult<KeyValue<KT, VT>>);
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        public ValueResult<VT> TryGetValueByIndex(int index)
        {
            var value = default(VT);
            if ((uint)index < (uint)dictionary.Count && dictionary.TryGetValueByIndex(index, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 获取第一组数据
        /// </summary>
        /// <returns>第一组数据</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetFirstKeyValue()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue;
            return default(ValueResult<KeyValue<KT, VT>>);
        }
        /// <summary>
        /// 获取最后一组数据
        /// </summary>
        /// <returns>最后一组数据</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetLastKeyValue()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue;
            return default(ValueResult<KeyValue<KT, VT>>);
        }
        /// <summary>
        /// 获取第一个关键字数据
        /// </summary>
        /// <returns>第一个关键字数据</returns>
        public ValueResult<KT> TryGetFirstKey()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue.Key;
            return default(ValueResult<KT>);
        }
        /// <summary>
        /// 获取最后一个关键字数据
        /// </summary>
        /// <returns>最后一个关键字数据</returns>
        public ValueResult<KT> TryGetLastKey()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue.Key;
            return default(ValueResult<KT>);
        }
        /// <summary>
        /// 获取第一个数据
        /// </summary>
        /// <returns>第一个数据</returns>
        public ValueResult<VT> TryGetFirstValue()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue.Value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 获取最后一个数据
        /// </summary>
        /// <returns>最后一个数据</returns>
        public ValueResult<VT> TryGetLastValue()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue.Value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns></returns>
        public IEnumerable<ValueResult<VT>> GetValues(int skipCount, byte getCount)
        {
            if (skipCount >= 0) return dictionary.GetValues(skipCount, getCount).Cast();
            return ValueResult<VT>.NullEnumerable;
        }
    }
}
