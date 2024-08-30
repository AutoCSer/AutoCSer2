using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Extensions;
using System;
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
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<KT, VT>> GetSnapshotArray()
        {
            return ServerNode.GetSearchTreeSnapshotArray(dictionary.Count, dictionary.KeyValues);
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
        /// 设置数据 持久化参数检查
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBeforePersistence(KT key, VT value)
        {
            if (key != null) return default(ValueResult<bool>);
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        public bool Set(KT key, VT value)
        {
            return dictionary.Set(key, value);
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
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        public bool TryAdd(KT key, VT value)
        {
            return dictionary.TryAdd(key, value);
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
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
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
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>被删除数据</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            VT value;
            if (dictionary.Remove(ref key, out value)) return value;
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
            VT value;
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
            VT value;
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
