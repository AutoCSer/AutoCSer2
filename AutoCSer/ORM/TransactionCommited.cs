using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 事务提交事件
    /// </summary>
    internal abstract class TransactionCommited
    {
        /// <summary>
        /// 表格操作事件类型
        /// </summary>
        protected readonly TableEventTypeEnum eventType;
        /// <summary>
        /// 事务提交事件
        /// </summary>
        /// <param name="eventType"></param>
        protected TransactionCommited(TableEventTypeEnum eventType)
        {
            this.eventType = eventType;
        }
        /// <summary>
        /// 事务提交后的处理
        /// </summary>
        /// <returns></returns>
        internal abstract Task OnCommited();
    }
    /// <summary>
    /// 事务提交事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class TransactionCommited<T> : TransactionCommited where T : class
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        private readonly TableWriter<T> tableWriter;
        /// <summary>
        /// 操作数据对象
        /// </summary>
        private readonly T value;
        /// <summary>
        /// 更新操作成员
        /// </summary>
#if NetStandard21
        private readonly MemberMap<T>? memberMap;
#else
        private readonly MemberMap<T> memberMap;
#endif
        /// <summary>
        /// 事务提交事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
#if NetStandard21
        private TransactionCommited(TableEventTypeEnum eventType, TableWriter<T> tableWriter, T value, MemberMap<T>? memberMap = null) : base(eventType)
#else
        private TransactionCommited(TableEventTypeEnum eventType, TableWriter<T> tableWriter, T value, MemberMap<T> memberMap = null) : base(eventType)
#endif
        {
            this.tableWriter = tableWriter;
            this.value = value;
            this.memberMap = memberMap;
        }
        /// <summary>
        /// 事务提交后的处理
        /// </summary>
        /// <returns></returns>
        internal override async Task OnCommited()
        {
            foreach (ITableEvent<T> tableEvent in tableWriter.Events)
            {
                try
                {
                    switch (eventType)
                    {
                        case TableEventTypeEnum.Insert: await tableEvent.OnInsertedCommited(value); break;
                        case TableEventTypeEnum.Update: await tableEvent.OnUpdatedCommited(value, memberMap.notNull()); break;
                        case TableEventTypeEnum.Delete: await tableEvent.OnDeletedCommited(value); break;
                    }
                }
                catch (Exception exception)
                {
                    await LogHelper.Exception(exception);
                }
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal static async Task<bool> OnInserted(TableWriter<T> tableWriter, T value, Transaction? transaction)
#else
        internal static async Task<bool> OnInserted(TableWriter<T> tableWriter, T value, Transaction transaction)
#endif
        {
            if (transaction == null)
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    try
                    {
                        await tableEvent.OnInserted(value);
                    }
                    catch (Exception exception)
                    {
                        await LogHelper.Exception(exception);
                    }
                }
            }
            else
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    if (!await tableEvent.OnInserted(value, transaction))
                    {
                        await transaction.DisposeAsync();
                        return false;
                    }
                }
                transaction.Commiteds.Add(new TransactionCommited<T>(TableEventTypeEnum.Insert, tableWriter, value));
            }
            return true;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal static async Task<bool> OnUpdated(TableWriter<T> tableWriter, T value, MemberMap<T> memberMap, Transaction? transaction)
#else
        internal static async Task<bool> OnUpdated(TableWriter<T> tableWriter, T value, MemberMap<T> memberMap, Transaction transaction)
#endif
        {
            if (transaction == null)
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    try
                    {
                        await tableEvent.OnUpdated(value, memberMap);
                    }
                    catch (Exception exception)
                    {
                        await LogHelper.Exception(exception);
                    }
                }
            }
            else
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    if (!await tableEvent.OnUpdated(value, memberMap, transaction))
                    {
                        await transaction.DisposeAsync();
                        return false;
                    }
                }
                transaction.Commiteds.Add(new TransactionCommited<T>(TableEventTypeEnum.Update, tableWriter, value, memberMap));
            }
            return true;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
#if NetStandard21
        internal static async Task<bool> OnDeleted(TableWriter<T> tableWriter, T? value, Transaction? transaction)
#else
        internal static async Task<bool> OnDeleted(TableWriter<T> tableWriter, T value, Transaction transaction)
#endif
        {
            if (transaction == null)
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    try
                    {
                        await tableEvent.OnDeleted(value.notNull());
                    }
                    catch (Exception exception)
                    {
                        await LogHelper.Exception(exception);
                    }
                }
            }
            else if (tableWriter.Events.Length != 0)
            {
                foreach (ITableEvent<T> tableEvent in tableWriter.Events)
                {
                    if (!await tableEvent.OnDeleted(value.notNull(), transaction))
                    {
                        await transaction.DisposeAsync();
                        return false;
                    }
                }
                transaction.Commiteds.Add(new TransactionCommited<T>(TableEventTypeEnum.Delete, tableWriter, value.notNull()));
            }
            return true;
        }
    }
}
