using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据库读取数据类型
    /// </summary>
    internal enum ReaderDataTypeEnum : byte
    {
        String,
        Int,
        Long,
        Short,
        Byte,
        Bool,
        DateTime,
        DateTimeOffset,
        TimeSpan,
        Decimal,
        Guid,
        Double,
        Float,
        CustomColumn, 
        Json
    }
}
