using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 表格操作事件接口
    /// </summary>
    /// <typeparam name="T">表格模型类型</typeparam>
    public interface ITableEvent<T> where T : class
    {
        /// <summary>
        /// 添加数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行添加数据操作</returns>
        Task<bool> BeforeInsert(T value);
        /// <summary>
        /// 非事务模式添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnInserted(T value);
        /// <summary>
        /// 事务模式执行添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnInserted(T value, Transaction transaction);
        /// <summary>
        /// 事务模式添加数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnInsertedCommited(T value);

        /// <summary>
        /// 更新数据之前检查操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否继续执行更新数据操作</returns>
        Task<bool> BeforeUpdate(T value, MemberMap<T> memberMap);
        /// <summary>
        /// 非事务模式更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        Task OnUpdated(T value, MemberMap<T> memberMap);
        /// <summary>
        /// 事务模式执行更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnUpdated(T value, MemberMap<T> memberMap, Transaction transaction);
        /// <summary>
        /// 事务模式更新数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        Task OnUpdatedCommited(T value, MemberMap<T> memberMap);

        /// <summary>
        /// 删除数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行删除数据操作</returns>
        Task<bool> BeforeDelete(T value);
        /// <summary>
        /// 非事务模式删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnDeleted(T value);
        /// <summary>
        /// 事务模式执行删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        Task<bool> OnDeleted(T value, Transaction transaction);
        /// <summary>
        /// 事务模式删除数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task OnDeletedCommited(T value);
    }
}
