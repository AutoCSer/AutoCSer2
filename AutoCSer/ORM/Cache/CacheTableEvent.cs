using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 时间缓存表格操作事件
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    internal sealed class CacheTableEvent<T> : ITableEvent<T>
        where T : class
    {
        /// <summary>
        /// 事件缓存
        /// </summary>
        private readonly EventCache<T> cache;
        /// <summary>
        /// 时间缓存表格操作事件
        /// </summary>
        /// <param name="cache">事件缓存</param>
        internal CacheTableEvent(EventCache<T> cache)
        {
            this.cache = cache;
        }
        /// <summary>
        /// 添加数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行添加数据操作</returns>
        public Task<bool> BeforeInsert(T value) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 非事务模式添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnInserted(T value)
        {
             cache.OnInserted(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnInserted(T value, Transaction transaction) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 事务模式添加数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnInsertedCommited(T value)
        {
            cache.OnInserted(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 更新数据之前检查操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否继续执行更新数据操作</returns>
        public Task<bool> BeforeUpdate(T value, MemberMap<T> memberMap) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 非事务模式更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        public Task OnUpdated(T value, MemberMap<T> memberMap)
        {
            cache.OnUpdated(value, memberMap);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnUpdated(T value, MemberMap<T> memberMap, Transaction transaction) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 事务模式更新数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        public Task OnUpdatedCommited(T value, MemberMap<T> memberMap)
        {
            cache.OnUpdated(value, memberMap);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 删除数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行删除数据操作</returns>
        public Task<bool> BeforeDelete(T value) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 非事务模式删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnDeleted(T value)
        {
            cache.OnDeleted(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnDeleted(T value, Transaction transaction) { return AutoCSer.Common.TrueCompletedTask; }
        /// <summary>
        /// 事务模式删除数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnDeletedCommited(T value)
        {
            cache.OnDeleted(value);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
