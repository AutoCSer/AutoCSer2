﻿using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 业务表格模型表格操作事件
    /// </summary>
    /// <typeparam name="BT">业务表格模型类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public sealed class BusinessTableEvent<BT, T> : ITableEvent<T>
        where BT : class, T
        where T : class
    {
        /// <summary>
        /// 业务表格模型表格操作事件接口
        /// </summary>
        private readonly IBusinessTableEvent<BT, T> tableEvent;
        /// <summary>
        /// 业务表格模型表格操作事件
        /// </summary>
        /// <param name="tableEvent">业务表格模型表格操作事件接口</param>
        public BusinessTableEvent(IBusinessTableEvent<BT, T> tableEvent)
        {
            this.tableEvent = tableEvent;
        }

        /// <summary>
        /// 添加数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行添加数据操作</returns>
        public async Task<bool> BeforeInsert(T value) { return await tableEvent.BeforeInsert((BT)value); }
        /// <summary>
        /// 非事务模式添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task OnInserted(T value) { await tableEvent.OnInserted((BT)value); }
        /// <summary>
        /// 事务模式执行添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public async Task<bool> OnInserted(T value, Transaction transaction) { return await tableEvent.OnInserted((BT)value, transaction); }
        /// <summary>
        /// 事务模式添加数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task OnInsertedCommited(T value) { await tableEvent.OnInsertedCommited((BT)value); }

        /// <summary>
        /// 更新数据之前检查操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否继续执行更新数据操作</returns>
        public async Task<bool> BeforeUpdate(T value, MemberMap<T> memberMap) { return await tableEvent.BeforeUpdate((BT)value, memberMap); }
        /// <summary>
        /// 非事务模式更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        public async Task OnUpdated(T value, MemberMap<T> memberMap) { await tableEvent.OnUpdated((BT)value, memberMap); }
        /// <summary>
        /// 事务模式执行更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public async Task<bool> OnUpdated(T value, MemberMap<T> memberMap, Transaction transaction) { return await tableEvent.OnUpdated((BT)value, memberMap, transaction); }
        /// <summary>
        /// 事务模式更新数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        public async Task OnUpdatedCommited(T value, MemberMap<T> memberMap) { await tableEvent.OnUpdatedCommited((BT)value, memberMap); }

        /// <summary>
        /// 删除数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行删除数据操作</returns>
        public async Task<bool> BeforeDelete(T value) { return await tableEvent.BeforeDelete((BT)value); }
        /// <summary>
        /// 非事务模式删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task OnDeleted(T value) { await tableEvent.OnDeleted((BT)value); }
        /// <summary>
        /// 事务模式执行删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public async Task<bool> OnDeleted(T value, Transaction transaction) { return await tableEvent.OnDeleted((BT)value, transaction); }
        /// <summary>
        /// 事务模式删除数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task OnDeletedCommited(T value) { await tableEvent.OnDeletedCommited((BT)value); }

        /// <summary>
        /// 创建业务表格持久化
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <param name="connectionPool">数据库连接池</param>
        /// <param name="attribute">数据表格模型配置</param>
        /// <returns></returns>
        public async Task<BusinessPersistence<BT, T, KT>> CreatePersistence<KT>(ConnectionPool connectionPool, ModelAttribute attribute = null)
            where KT : IEquatable<KT>
        {
            return await connectionPool.CreateBusinessPersistence<BT, T, KT>(this, attribute);
        }
    }
    /// <summary>
    /// 业务表格模型表格操作事件接口
    /// </summary>
    /// <typeparam name="BT">业务表格模型类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    public interface IBusinessTableEvent<BT, T>
        where BT : class, T
        where T : class
    {
        /// <summary>
        /// 添加数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行添加数据操作</returns>
        Task<bool> BeforeInsert(BT value);
        /// <summary>
        /// 非事务模式添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnInserted(BT value);
        /// <summary>
        /// 事务模式执行添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnInserted(BT value, Transaction transaction);
        /// <summary>
        /// 事务模式添加数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnInsertedCommited(BT value);

        /// <summary>
        /// 更新数据之前检查操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否继续执行更新数据操作</returns>
        Task<bool> BeforeUpdate(BT value, MemberMap<T> memberMap);
        /// <summary>
        /// 非事务模式更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        Task OnUpdated(BT value, MemberMap<T> memberMap);
        /// <summary>
        /// 事务模式执行更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnUpdated(BT value, MemberMap<T> memberMap, Transaction transaction);
        /// <summary>
        /// 事务模式更新数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        Task OnUpdatedCommited(BT value, MemberMap<T> memberMap);

        /// <summary>
        /// 删除数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行删除数据操作</returns>
        Task<bool> BeforeDelete(BT value);
        /// <summary>
        /// 非事务模式删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnDeleted(BT value);
        /// <summary>
        /// 事务模式执行删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnDeleted(BT value, Transaction transaction);
        /// <summary>
        /// 事务模式删除数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnDeletedCommited(BT value);
    }
}