using System;
/*long,Long,GetInt64;int,Int,GetInt32;short,Short,GetInt16;byte,Byte,GetByte;bool,Bool,GetBoolean;DateTime,DateTime,GetDateTime;decimal,Decimal,GetDecimal;Guid,Guid,GetGuid;double,Double,GetDouble;float,Float,GetFloat*/

namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static long readLong(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetInt64(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static long? readLongNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt64(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static long readLongObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(long)) return reader.GetInt64(index);
            if (reader.IsDBNull(index)) return default(long);
            return long.Parse(reader[index].ToString());
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static long? readLongNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(long)) return reader.GetInt64(index);
            return long.Parse(reader[index].ToString());
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static long readLong(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadLong();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static long? readLongNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadLongNullable();
        }
    }
}