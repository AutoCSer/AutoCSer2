using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 base fragment dictionary node interface
    /// 256 基分片字典 节点接口
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    [ServerNode(IsReturnValueNode = ServerNodeAttribute.DefaultIsReturnValueNode)]
    public partial interface IByteArrayFragmentDictionaryNode<KT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(BinarySerializeKeyValue<KT, byte[]> value);
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryGetValue(KT key);
#else
        ValueResult<byte[]> TryGetValue(KT key);
#endif
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        byte[]?[] GetValueArray(KT[] keys);
#else
        byte[][] GetValueArray(KT[] keys);
#endif
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryGetResponseParameter(KT key);
        /// <summary>
        /// Clear the data (retain the fragmented array)
        /// 清除数据（保留分片数组）
        /// </summary>
        void Clear();
        /// <summary>
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        void ClearArray();
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(KT key, ServerByteArray value);
        /// <summary>
        /// Force the data to be set and overwrite if the keyword already exists
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(KT key, ServerByteArray value);
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(KT key);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(KT key);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        int RemoveKeys(KT[] keys);
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
#if NetStandard21
        ValueResult<byte[]?> GetRemove(KT key);
#else
        ValueResult<byte[]> GetRemove(KT key);
#endif
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ResponseParameter GetRemoveResponseParameter(KT key);
    }
}
