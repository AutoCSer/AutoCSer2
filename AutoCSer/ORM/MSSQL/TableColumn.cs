using AutoCSer.Threading;
using System;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// 数据库表格列信息
    /// </summary>
    internal sealed class TableColumn
    {
#pragma warning disable
        /// <summary>
        /// 列编号
        /// </summary>
        public short colid;
        /// <summary>
        /// 数据列名称
        /// </summary>
        public string name;
        /// <summary>
        /// 数据库表格列类型
        /// </summary>
        public TableColumnDbTypeEnum xusertype;
        /// <summary>
        /// 数据字节长度，NChar 与 NVarChar 要 /2
        /// </summary>
        public short length;
        /// <summary>
        /// decimal 小数位数
        /// </summary>
        public DecimalDigits DecimalDigits;
        /// <summary>
        /// 是否允许空值，1 表示允许空值
        /// </summary>
        public int isnullable;
        /// <summary>
        /// 是否自增字段，1 表示自增
        /// </summary>
        public int isidentity;
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaultvalue;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark;
#pragma warning restore

        /// <summary>
        /// 成员信息匹配
        /// </summary>
        /// <param name="member"></param>
        /// <param name="tableWriter"></param>
        internal void Match(Member member, TableWriter tableWriter)
        {
            switch (member.ReaderDataType)
            {
                case ReaderDataTypeEnum.Int:
                    if (xusertype == TableColumnDbTypeEnum.Int) return;
                    break;
                case ReaderDataTypeEnum.Long:
                    if (xusertype == TableColumnDbTypeEnum.BigInt) return;
                    break;
                case ReaderDataTypeEnum.Short:
                    if (xusertype == TableColumnDbTypeEnum.SmallInt) return;
                    break;
                case ReaderDataTypeEnum.Byte:
                    if (xusertype == TableColumnDbTypeEnum.TinyInt) return;
                    break;
                case ReaderDataTypeEnum.Bool:
                    if (xusertype == TableColumnDbTypeEnum.Bit) return;
                    break;
                case ReaderDataTypeEnum.DateTime:
                    DateTimeAttribute dateTimeAttribute = member.Attribute as DateTimeAttribute ?? DateTimeAttribute.Default;
                    switch (dateTimeAttribute.Type)
                    {
                        case DateTimeTypeEnum.DateTime:
                            switch (xusertype)
                            {
                                case TableColumnDbTypeEnum.DateTime: return;
                                case TableColumnDbTypeEnum.DateTime2:
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {dateTimeAttribute.Type}");
                                    return;
                            }
                            break;
                        case DateTimeTypeEnum.DateTime2:
                            if (xusertype == TableColumnDbTypeEnum.DateTime2) return;
                            break;
                        case DateTimeTypeEnum.SmallDateTime:
                            switch (xusertype)
                            {
                                case TableColumnDbTypeEnum.SmallDateTime: return;
                                case TableColumnDbTypeEnum.DateTime:
                                case TableColumnDbTypeEnum.DateTime2:
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {dateTimeAttribute.Type}");
                                    return;
                            }
                            break;
                        case DateTimeTypeEnum.Date:
                            switch (xusertype)
                            {
                                case TableColumnDbTypeEnum.Date: return;
                                case TableColumnDbTypeEnum.DateTime:
                                case TableColumnDbTypeEnum.DateTime2:
                                case TableColumnDbTypeEnum.SmallDateTime:
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {dateTimeAttribute.Type}");
                                    return;
                            }
                            break;
                            //case DateTimeTypeEnum.Time:
                            //    switch (xusertype)
                            //    {
                            //        case TableColumnDbTypeEnum.Time: return;
                            //        case TableColumnDbTypeEnum.Date:
                            //        case TableColumnDbTypeEnum.DateTime:
                            //        case TableColumnDbTypeEnum.DateTime2:
                            //        case TableColumnDbTypeEnum.SmallDateTime:
                            //            CatchTask.AddIgnoreException(LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {dateTimeAttribute.Type}"));
                            //            return;
                            //    }
                            //    break;
                    }
                    break;
                case ReaderDataTypeEnum.DateTimeOffset:
                    if (xusertype == TableColumnDbTypeEnum.DateTimeOffset) return;
                    break;
                case ReaderDataTypeEnum.TimeSpan:
                    if (xusertype == TableColumnDbTypeEnum.Time) return;
                    break;
                case ReaderDataTypeEnum.Decimal:
                    var decimalAttribute = member.Attribute as DecimalAttribute;
                    if (decimalAttribute == null)
                    {
                        var moneyAttribute = member.Attribute as MoneyAttribute;
                        if (moneyAttribute != null)
                        {
                            if (moneyAttribute.IsSmall)
                            {
                                switch (xusertype)
                                {
                                    case TableColumnDbTypeEnum.SmallMoney: return;
                                    case TableColumnDbTypeEnum.Money:
                                        LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {nameof(TableColumnDbTypeEnum.SmallMoney)}");
                                        return;
                                    case TableColumnDbTypeEnum.Decimal:
                                        if (DecimalDigits.Integer >= 6 && DecimalDigits.xscale >= 4)
                                        {
                                            LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({DecimalDigits.xprec},{DecimalDigits.xscale}) 精度高于模型定义 {nameof(TableColumnDbTypeEnum.SmallMoney)}");
                                            return;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                switch (xusertype)
                                {
                                    case TableColumnDbTypeEnum.Money: return;
                                    case TableColumnDbTypeEnum.Decimal:
                                        if (DecimalDigits.Integer >= 15 && DecimalDigits.xscale >= 4)
                                        {
                                            LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({DecimalDigits.xprec},{DecimalDigits.xscale}) 精度高于模型定义 {nameof(TableColumnDbTypeEnum.Money)}");
                                            return;
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    }
                    if (decimalAttribute == null) decimalAttribute = DecimalAttribute.Default;
                    switch (xusertype)
                    {
                        case TableColumnDbTypeEnum.Decimal:
                            if (DecimalDigits.Integer >= decimalAttribute.Integer && DecimalDigits.xscale >= decimalAttribute.Scale)
                            {
                                if (DecimalDigits.xprec != decimalAttribute.Precision && DecimalDigits.xscale != decimalAttribute.Scale)
                                {
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({DecimalDigits.xprec},{DecimalDigits.xscale}) 精度高于模型定义 {nameof(TableColumnDbTypeEnum.Decimal)}({decimalAttribute.Precision},{decimalAttribute.Scale})");
                                }
                                return;
                            }
                            break;
                        case TableColumnDbTypeEnum.Money:
                            if (decimalAttribute.Integer < 15 && decimalAttribute.Scale <= 4)
                            {
                                LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {nameof(TableColumnDbTypeEnum.Decimal)}({decimalAttribute.Precision},{decimalAttribute.Scale})");
                                return;
                            }
                            break;
                        case TableColumnDbTypeEnum.SmallMoney:
                            if (decimalAttribute.Integer < 6 && decimalAttribute.Scale <= 4)
                            {
                                LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {nameof(TableColumnDbTypeEnum.Decimal)}({decimalAttribute.Precision},{decimalAttribute.Scale})");
                                return;
                            }
                            break;
                    }
                    break;
                case ReaderDataTypeEnum.Guid:
                    if (xusertype == TableColumnDbTypeEnum.UniqueIdentifier) return;
                    break;
                case ReaderDataTypeEnum.Double:
                    if (xusertype == TableColumnDbTypeEnum.Float) return;
                    break;
                case ReaderDataTypeEnum.Float:
                    if (xusertype == TableColumnDbTypeEnum.Real) return;
                    break;
                default:
                    StringAttribute stringAttribute = member.Attribute as StringAttribute ?? StringAttribute.Default;
                    switch (xusertype)
                    {
                        case TableColumnDbTypeEnum.NText:
                            if (stringAttribute.Size == 0)
                            {
                                if (stringAttribute.IsAscii) LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype} 精度高于模型定义 {member.ReaderDataType}");
                                return;
                            }
                            break;
                        case TableColumnDbTypeEnum.Text:
                            if (stringAttribute.Size == 0 && stringAttribute.IsAscii) return;
                            break;
                        case TableColumnDbTypeEnum.NVarChar:
                        case TableColumnDbTypeEnum.NChar:
                            if (length < 0)
                            {
                                if (stringAttribute.Size == 0)
                                {
                                    if (stringAttribute.IsAscii) LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({length}) 精度高于模型定义 {member.ReaderDataType}({stringAttribute.Size})");
                                    return;
                                }
                            }
                            else if (length >= (stringAttribute.Size << 1))
                            {
                                if (stringAttribute.IsAscii || (stringAttribute.Size << 1) != length || ((xusertype == TableColumnDbTypeEnum.NChar) ^ stringAttribute.IsFixed))
                                {
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({length >> 1}) 精度高于模型定义 {member.ReaderDataType}({stringAttribute.Size})");
                                }
                                return;
                            }
                            break;
                        case TableColumnDbTypeEnum.VarChar:
                        case TableColumnDbTypeEnum.Char:
                            if (length < 0)
                            {
                                if (stringAttribute.Size == 0 && stringAttribute.IsAscii) return;
                            }
                            else if (length >= stringAttribute.Size && stringAttribute.IsAscii)
                            {
                                if (stringAttribute.Size != length || ((xusertype == TableColumnDbTypeEnum.Char) ^ stringAttribute.IsFixed))
                                {
                                    LogHelper.DebugIgnoreException($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({length}) 精度高于模型定义 {member.ReaderDataType}({stringAttribute.Size})");
                                }
                                return;
                            }
                            break;
                    }
                    break;
            }
            throw new Exception($"{tableWriter.TableName} 数据列 {name} 类型 {xusertype}({length}) 不兼容匹配模型定义 {member.ReaderDataType}");
        }
    }
}
