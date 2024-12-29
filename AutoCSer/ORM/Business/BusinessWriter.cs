using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 业务表格模型持久化写入
    /// </summary>
    /// <typeparam name="BT">业务表格模型类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public sealed class BusinessWriter<BT, T, KT>
        where BT : class, T
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        private readonly TableWriter<T, KT> writer;
        /// <summary>
        /// 业务表格模型持久化查询
        /// </summary>
        /// <param name="writer"></param>
        internal BusinessWriter(TableWriter<T, KT> writer)
        {
            this.writer = writer;
        }
        /// <summary>
        /// 添加表格操作事件处理对象（表格增删改操作必须在队列中调用）
        /// </summary>
        /// <param name="tableEvent"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AppendEvent(BusinessTableEvent<BT, T> tableEvent)
        {
            writer.AppendEvent(tableEvent);
        }
        /// <summary>
        /// 移除表格操作事件处理对象
        /// </summary>
        /// <param name="tableEvent"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveEvent(BusinessTableEvent<BT, T> tableEvent)
        {
            return writer.RemoveEvent(tableEvent);
        }
        /// <summary>
        /// 创建表格索引
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="indexNameSuffix">索引名称后缀</param>
        /// <param name="isUnique">是否唯一索引</param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <returns>指定的索引名称已经存在则返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> CreateIndex(string[] columnNames, string? indexNameSuffix = null, bool isUnique = false, int timeoutSeconds = 0)
#else
        public Task<bool> CreateIndex(string[] columnNames, string indexNameSuffix = null, bool isUnique = false, int timeoutSeconds = 0)
#endif
        {
            return writer.CreateIndex(columnNames, indexNameSuffix, isUnique, timeoutSeconds);
        }
        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Insert<VT>(VT value, Transaction? transaction = null) where VT : class, BT
#else
        public Task<bool> Insert<VT>(VT value, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Insert<VT>(value, transaction);
        }
        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Insert<VT>(IEnumerable<VT> values, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Insert<VT>(IEnumerable<VT> values, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Insert<VT>(values, transaction);
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Update<VT>(VT value, MemberMap<T>? memberMap = null, Transaction? transaction = null) where VT : class, BT
#else
        public Task<bool> Update<VT>(VT value, MemberMap<T> memberMap = null, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value, memberMap, transaction);
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Update<VT>(MemberMapValue<T, VT> value, Transaction? transaction = null) where VT : class, BT
#else
        public Task<bool> Update<VT>(MemberMapValue<T, VT> value, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value.Value.notNull(), value.MemberMap, transaction);
        }
        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(IEnumerable<VT> values, MemberMap<T>? memberMap = null, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Update<VT>(IEnumerable<VT> values, MemberMap<T> memberMap = null, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(values, memberMap, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value, memberMap, condition, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value.Value.notNull(), value.MemberMap, condition, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap">查询成员位图，默认为所有成员</param>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Update<VT>(VT value, MemberMap<T> memberMap, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value, memberMap, query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件更新数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>更新数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Update<VT>(MemberMapValue<T, VT> value, QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Update<VT>(value.Value.notNull(), value.MemberMap, query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据缓存更新数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CKT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Update<VT, CKT>(ICachePersistence<T, VT, CKT> cache, VT value, MemberMap<T>? memberMap = null, bool isClone = true, Transaction? transaction = null)
#else
        public Task<VT> Update<VT, CKT>(ICachePersistence<T, VT, CKT> cache, VT value, MemberMap<T> memberMap = null, bool isClone = true, Transaction transaction = null)
#endif
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return cache.Update(value, memberMap, isClone, transaction);
        }
        /// <summary>
        /// 根据缓存更新数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CKT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="value"></param>
        /// <param name="isClone">默认为 true 表示浅复制缓存数据对象，避免缓存数据对象数据被意外修改</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Update<VT, CKT>(ICachePersistence<T, VT, CKT> cache, MemberMapValue<T, VT> value, bool isClone = true, Transaction? transaction = null)
#else
        public Task<VT> Update<VT, CKT>(ICachePersistence<T, VT, CKT> cache, MemberMapValue<T, VT> value, bool isClone = true, Transaction transaction = null)
#endif
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return cache.Update(value.Value.notNull(), value.MemberMap, isClone, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete(KT primaryKey, Transaction? transaction = null)
#else
        public Task<bool> Delete(KT primaryKey, Transaction transaction = null)
#endif
        {
            return writer.Delete<BT>(primaryKey,transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete<VT>(KT primaryKey, Transaction? transaction = null) where VT : class, BT
#else
        public Task<bool> Delete<VT>(KT primaryKey, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(primaryKey, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<bool> Delete<VT>(VT value, Transaction? transaction = null) where VT : class, BT
#else
        public Task<bool> Delete<VT>(VT value, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(value, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            return writer.Delete<BT>(primaryKeys, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="primaryKeys"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Delete<VT>(IEnumerable<KT> primaryKeys, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(primaryKeys, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据关键字删除表格数据
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="values"></param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量，失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(IEnumerable<VT> values, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Delete<VT>(IEnumerable<VT> values, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(values, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            return writer.Delete<BT>(condition, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Delete<VT>(Expression<Func<T, bool>> condition, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(condition, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null)
#else
        public Task<int> Delete(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null)
#endif
        {
            return writer.Delete<BT>(query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据查询条件删除数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeoutSeconds">查询命令超时秒数，0 表示不设置为默认值</param>
        /// <param name="ignoreFail">默认表示忽略失败继续执行，否则任意数据删除失败则回滚事务处理</param>
        /// <param name="transaction"></param>
        /// <returns>删除数据数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<int> Delete<VT>(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction? transaction = null) where VT : class, BT
#else
        public Task<int> Delete<VT>(QueryBuilder<T> query, int timeoutSeconds = 0, bool ignoreFail = false, Transaction transaction = null) where VT : class, BT
#endif
        {
            return writer.Delete<VT>(query, timeoutSeconds, ignoreFail, transaction);
        }
        /// <summary>
        /// 根据缓存关键字删除数据（缓存操作必须在队列中调用）
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <typeparam name="CKT"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<VT?> Delete<VT, CKT>(ICachePersistence<T, VT, CKT> cache, CKT key, Transaction? transaction = null)
#else
        public Task<VT> Delete<VT, CKT>(ICachePersistence<T, VT, CKT> cache, CKT key, Transaction transaction = null)
#endif
            where VT : class, BT
            where CKT : IEquatable<CKT>
        {
            return cache.Delete(key, transaction);
        }
    }
}
