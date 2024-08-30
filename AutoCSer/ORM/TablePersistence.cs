using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库表格持久化
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TablePersistence<T, KT>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 数据库表格持久化查询
        /// </summary>
        public readonly TableQuery<T, KT> Query;
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        public readonly TableWriter<T, KT> Writer;
        /// <summary>
        /// 数据库表格持久化
        /// </summary>
        /// <param name="query"></param>
        internal TablePersistence(TableQuery<T, KT> query)
        {
            Query = query;
            Writer = query.Writer;
        }
    }
}
