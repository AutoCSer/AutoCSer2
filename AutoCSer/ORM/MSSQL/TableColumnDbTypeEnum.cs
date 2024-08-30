using System;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 数据库表格列类型
    /// </summary>
    internal enum TableColumnDbTypeEnum : short
    {
        ///// <summary>
        ///// 图片 byte[]
        ///// </summary>
        //Image = 34,
        /// <summary>
        /// ASCII 文本 string，尽量使用 varchar(max)
        /// </summary>
        Text = 35,
        /// <summary>
        /// 唯一标识符 Guid
        /// </summary>
        UniqueIdentifier = 36,
        /// <summary>
        /// 日期 yyyy/MM/dd
        /// </summary>
        Date = 40,
        /// <summary>
        /// 时间 HH:mm:ss
        /// </summary>
        Time = 41,
        /// <summary>
        /// 高精度日期时间 DateTime yyyy/MM/dd HH:mm:ss.fffffff
        /// </summary>
        DateTime2 = 42,
        /// <summary>
        /// UTC 时间点 DateTimeOffset yyyy/MM/dd HH:mm:ss.fffffff
        /// </summary>
        DateTimeOffset = 43,
        /// <summary>
        /// 整数 byte
        /// </summary>
        TinyInt = 48,
        /// <summary>
        /// 整数 short
        /// </summary>
        SmallInt = 52,
        /// <summary>
        /// 整数 int
        /// </summary>
        Int = 56,
        /// <summary>
        /// 日期时间 DateTime 1900/1/1~2079/6/6
        /// </summary>
        SmallDateTime = 58,
        /// <summary>
        /// 浮点数 float
        /// </summary>
        Real = 59,
        /// <summary>
        /// 货币 decimal
        /// </summary>
        Money = 60,
        /// <summary>
        /// 日期时间 DateTime 1753/1/1~9999/12/31 DateTime yyyy/MM/dd HH:mm:ss.fff
        /// </summary>
        DateTime = 61,
        /// <summary>
        /// 浮点数 double
        /// </summary>
        Float = 62,
        //Variant = 98,
        /// <summary>
        /// 文本 string，尽量使用 nvarchar(max)
        /// </summary>
        NText = 99,
        /// <summary>
        /// 逻辑值 bool
        /// </summary>
        Bit = 104,
        /// <summary>
        /// 小数 decimal
        /// </summary>
        Decimal = 106,
        //Numeric = 108,
        /// <summary>
        /// 货币 decimal
        /// </summary>
        SmallMoney = 122,
        /// <summary>
        /// 整数 long
        /// </summary>
        BigInt = 127,
        //Hierarchyid = 128,
        //Geometry = 129,
        //Geography = 130,
        ///// <summary>
        ///// 二进制数据 byte[]
        ///// </summary>
        //VarBinary = 165,
        /// <summary>
        /// ASCII 字符串 string，最大长度 8000，varchar(max) 则不受限制
        /// </summary>
        VarChar = 167,
        ///// <summary>
        ///// 二进制数据 byte[]
        ///// </summary>
        //Binary = 173,
        /// <summary>
        /// ASCII 字符串 string，最大长度 8000
        /// </summary>
        Char = 175,
        ///// <summary>
        ///// 数据库记录更新版本时间戳 byte[8]
        ///// </summary>
        //Timestamp = 189,
        /// <summary>
        /// 字符串 string，最大长度 4000，nvarchar(max) 则不受限制
        /// </summary>
        NVarChar = 231,
        /// <summary>
        /// 字符串 string，最大长度 4000
        /// </summary>
        NChar = 239,
        ///// <summary>
        ///// XML 字符串 string
        ///// </summary>
        //Xml = 241,
        //SysName = 256
    }
}
