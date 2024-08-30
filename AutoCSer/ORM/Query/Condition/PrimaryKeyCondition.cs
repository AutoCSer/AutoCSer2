using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 关键字条件
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    internal sealed class PrimaryKeyCondition<T, KT> : ICondition
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        private readonly TableWriter<T, KT> tableWriter;
        /// <summary>
        /// 关键字
        /// </summary>
        private readonly KT primaryKey;
        /// <summary>
        /// 关键字条件
        /// </summary>
        /// <param name="tableWriter">数据库表格持久化写入</param>
        /// <param name="primaryKey">关键字</param>
        internal PrimaryKeyCondition(TableWriter<T, KT> tableWriter, KT primaryKey)
        {
            this.tableWriter = tableWriter;
            this.primaryKey = primaryKey;
        }
        /// <summary>
        /// 写入关键字条件
        /// </summary>
        /// <param name="charStream"></param>
        void ICondition.WriteCondition(CharStream charStream)
        {
            tableWriter.PrimaryKeyCondition(charStream, primaryKey);
        }
    }
}
