using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 持久化缓存
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">缓存数据类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public interface ICachePersistence<T, VT, KT>
        where T : class
        where VT : class, T
        where KT : IEquatable<KT>
    {
        ///// <summary>
        ///// 数据库表格持久化写入
        ///// </summary>
        //TableWriter<T> TableWriter { get; }
        /// <summary>
        /// 根据缓存更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="isClone"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        Task<VT?> Update(VT value, MemberMap<T>? memberMap = null, bool isClone = true, Transaction? transaction = null);
#else
        Task<VT> Update(VT value, MemberMap<T> memberMap = null, bool isClone = true, Transaction transaction = null);
#endif
        /// <summary>
        /// 根据缓存更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isClone"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        Task<VT?> Update(MemberMapValue<T, VT> value, bool isClone = true, Transaction? transaction = null);
#else
        Task<VT> Update(MemberMapValue<T, VT> value, bool isClone = true, Transaction transaction = null);
#endif
        /// <summary>
        /// 根据关键字删除缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        Task<VT?> Delete(KT key, Transaction? transaction = null);
#else
        Task<VT> Delete(KT key, Transaction transaction = null);
#endif
    }
}
