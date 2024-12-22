using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二叉搜索树节点
    /// </summary>
    /// <typeparam name="KT">排序关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    [ServerNode(MethodIndexEnumType = typeof(SearchTreeDictionaryNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface ISearchTreeDictionaryNode<KT, VT>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotAdd(KeyValue<KT, VT> value);
        /// <summary>
        /// 获取节点数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        [ServerMethod(IsPersistence = false)]
        int GetHeight();
        /// <summary>
        /// 清除数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(KT key, VT value);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(KT key, VT value);
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        bool Remove(KT key);
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>被删除数据</returns>
        ValueResult<VT> GetRemove(KT key);
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(KT key);
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>目标数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetValue(KT key);
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(KT key);
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        [ServerMethod(IsPersistence = false)]
        int CountLess(KT key);
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        [ServerMethod(IsPersistence = false)]
        int CountThan(KT key);
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KeyValue<KT, VT>> TryGetKeyValueByIndex(int index);
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetValueByIndex(int index);
        /// <summary>
        /// 获取第一组数据
        /// </summary>
        /// <returns>第一组数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KeyValue<KT, VT>> TryGetFirstKeyValue();
        /// <summary>
        /// 获取最后一组数据
        /// </summary>
        /// <returns>最后一组数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KeyValue<KT, VT>> TryGetLastKeyValue();
        /// <summary>
        /// 获取第一个关键字数据
        /// </summary>
        /// <returns>第一个关键字数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KT> TryGetFirstKey();
        /// <summary>
        /// 获取最后一个关键字数据
        /// </summary>
        /// <returns>最后一个关键字数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KT> TryGetLastKey();
        /// <summary>
        /// 获取第一个数据
        /// </summary>
        /// <returns>第一个数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetFirstValue();
        /// <summary>
        /// 获取最后一个数据
        /// </summary>
        /// <returns>最后一个数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetLastValue();
        /// <summary>
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        IEnumerable<ValueResult<VT>> GetValues(int skipCount, byte getCount);
    }
}
