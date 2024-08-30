using AutoCSer.Metadata;
using AutoCSer.ORM;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommonModel
{
    /// <summary>
    /// 控制台输出表格操作记录
    /// </summary>
    /// <typeparam name="BT"></typeparam>
    /// <typeparam name="T"></typeparam>
    public sealed class BusinessTableEventConsoleOutput<BT, T> : IBusinessTableEvent<BT, T>
        where BT : class, T
        where T : class
    {
        /// <summary>
        /// 业务表格模型表格操作事件
        /// </summary>
        public BusinessTableEvent<BT, T> BusinessTableEvent
        {
            get { return new BusinessTableEvent<BT, T>(this); }
        }

        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        private bool output(BT value, [CallerMemberName] string callerMemberName = null)
        {
            Console.WriteLine($"{callerMemberName} {AutoCSer.JsonSerializer.Serialize(value)}");
            Console.WriteLine();
            return true;
        }

        /// <summary>
        /// 添加数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行添加数据操作</returns>
        public Task<bool> BeforeInsert(BT value) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 非事务模式添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnInserted(BT value)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行添加数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnInserted(BT value, Transaction transaction) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 事务模式添加数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnInsertedCommited(BT value)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// 更新数据之前检查操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否继续执行更新数据操作</returns>
        public Task<bool> BeforeUpdate(BT value, MemberMap<T> memberMap) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 非事务模式更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        public Task OnUpdated(BT value, MemberMap<T> memberMap)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行更新数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnUpdated(BT value, MemberMap<T> memberMap, Transaction transaction) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 事务模式更新数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        public Task OnUpdatedCommited(BT value, MemberMap<T> memberMap)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// 删除数据之前检查数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否继续执行删除数据操作</returns>
        public Task<bool> BeforeDelete(BT value) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 非事务模式删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnDeleted(BT value)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 事务模式执行删除数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns>是否继续执行后续事务，否则中止事务</returns>
        public Task<bool> OnDeleted(BT value, Transaction transaction) { return AutoCSer.Common.GetCompletedTask(output(value)); }
        /// <summary>
        /// 事务模式删除数据事务提交以后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task OnDeletedCommited(BT value)
        {
            output(value);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
