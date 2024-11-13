using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{

    /// <summary>
    /// 自增ID 数据库表格持久化写入
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    internal abstract class AutoIdentityTableWriter<T, KT> : TableWriter<T, KT>
        where T : class
        where KT : IEquatable<KT>, IConvertible
    {
        /// <summary>
        /// 主键是否自增ID
        /// </summary>
        internal override bool AutoIdentity { get { return true; } }
        /// <summary>
        /// 当前已使用的自增ID
        /// </summary>
        protected long currentIdentity;
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        /// <param name="tableEvent">表格操作事件处理</param>
#if NetStandard21
        internal AutoIdentityTableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T>? tableEvent)
#else
        internal AutoIdentityTableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T> tableEvent)
#endif
            : base(connectionPool, attribute, members, primaryKey, tableEvent)
        {
        }
        /// <summary>
        /// 关键字转自增ID
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        protected abstract long primaryKeyToIdentity(KT primaryKey);
        /// <summary>
        /// 初始化获取当前已使用的自增ID
        /// </summary>
        /// <returns></returns>
        internal async Task GetCurrentIdentity()
        {
            QueryBuilder<T> query = CreateQuery(null, false).OrderBy(PrimaryKey.MemberIndex.Member.Name, false, false);
            query.MemberMap = PrimaryKeyMemberMap;
            var value = await query.SingleOrDefault();
            if (value != null) currentIdentity = primaryKeyToIdentity(GetPrimaryKey(value));

            var autoIdentity = await ConnectionPool.CheckAutoIdentity(TableName);
            if (autoIdentity != null && autoIdentity.Identity > currentIdentity) currentIdentity = (int)autoIdentity.Identity;
        }
    }
    /// <summary>
    /// 自增ID 数据库表格持久化写入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class AutoIdentityTableWriter<T> : AutoIdentityTableWriter<T, int>
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        /// <param name="tableEvent">表格操作事件处理</param>
#if NetStandard21
        internal AutoIdentityTableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T>? tableEvent)
#else
        internal AutoIdentityTableWriter(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T> tableEvent)
#endif
            : base(connectionPool, attribute, members, primaryKey, tableEvent)
        {
        }
        /// <summary>
        /// 设置新增对象自增ID
        /// </summary>
        /// <param name="value"></param>
        internal override void SetInsertAutoIdentity(T value)
        {
            SetPrimaryKey(value, (int)++currentIdentity);
        }
        /// <summary>
        /// 关键字转自增ID
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        protected override long primaryKeyToIdentity(int primaryKey) { return primaryKey; }
        /// <summary>
        /// 更新自增ID记录
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        internal override async Task CheckUpdateAutoIdentity(int primaryKey)
        {
            if (primaryKey == (int)currentIdentity) await ConnectionPool.UpdateAutoIdentity(TableName, currentIdentity);
        }
    }
    /// <summary>
    /// 自增ID 数据库表格持久化写入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class AutoIdentityTableWriter64<T> : AutoIdentityTableWriter<T, long>
        where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <param name="members">数据表格模型字段成员集合</param>
        /// <param name="primaryKey">关键字字段成员</param>
        /// <param name="tableEvent">表格操作事件处理</param>
#if NetStandard21
        internal AutoIdentityTableWriter64(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T>? tableEvent)
#else
        internal AutoIdentityTableWriter64(ConnectionPool connectionPool, ModelAttribute attribute, Member[] members, Member primaryKey, ITableEvent<T> tableEvent)
#endif
            : base(connectionPool, attribute, members, primaryKey, tableEvent)
        {
        }
        /// <summary>
        /// 设置新增对象自增ID
        /// </summary>
        /// <param name="value"></param>
        internal override void SetInsertAutoIdentity(T value)
        {
            SetPrimaryKey(value, (int)++currentIdentity);
        }
        /// <summary>
        /// 关键字转自增ID
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        protected override long primaryKeyToIdentity(long primaryKey) { return primaryKey; }
        /// <summary>
        /// 更新自增ID记录
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        internal override async Task CheckUpdateAutoIdentity(long primaryKey)
        {
            if (primaryKey == currentIdentity) await ConnectionPool.UpdateAutoIdentity(TableName, currentIdentity);
        }
    }
}
