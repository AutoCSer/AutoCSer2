using System;
using System.Collections.Generic;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 数据库表格索引列
    /// </summary>
    internal sealed class TableIndexColumn
    {
#pragma warning disable
        /// <summary>
        /// 索引编号
        /// </summary>
        public short indid;
        /// <summary>
        /// 列编号
        /// </summary>
        public short colid;
        /// <summary>
        /// 索引类型
        /// </summary>
        public TableIndexStateEnum status;
#pragma warning restore

        /// <summary>
        /// 匹配关键字
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="indexs"></param>
        /// <param name="columnIDs"></param>
        /// <returns></returns>
        internal static bool MatchPrimaryKey(Member primaryKey, ref LeftArray<TableIndexColumn> indexs, Dictionary<short, TableColumn> columnIDs)
        {
            if (primaryKey.CustomColumnAttribute == null)
            {
                return indexs.Count == 1 && columnIDs[indexs.Array[0].colid].name == primaryKey.MemberIndex.Member.Name;
            }
            if (primaryKey.CustomColumnNames.Length != indexs.Length) return false;
            int columnIndex = 0;
            foreach (TableIndexColumn index in indexs)
            {
                if (columnIDs[index.colid].name != primaryKey.CustomColumnNames[columnIndex++].Name) return false;
            }
            return true;
        }
    }
}
