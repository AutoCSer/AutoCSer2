using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.ORM.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 默认数据列自定义配置
        /// </summary>
        private static readonly MemberAttribute DefaultAttribute = new MemberAttribute();

        /// <summary>
        /// 成员索引
        /// </summary>
        internal readonly MemberIndexInfo MemberIndex;
        /// <summary>
        /// 数据库成员信息
        /// </summary>
        internal readonly MemberAttribute Attribute;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
#if NetStandard21
        private object? genericType;
#else
        private object genericType;
#endif
        /// <summary>
        /// 自定义数据列配置
        /// </summary>
#if NetStandard21
        internal readonly CustomColumnAttribute? CustomColumnAttribute;
#else
        internal readonly CustomColumnAttribute CustomColumnAttribute;
#endif
        /// <summary>
        /// 可空成员类型的基础类型，否则为成员类型
        /// </summary>
#if NetStandard21
        internal Type? NullableElementType;
#else
        internal Type NullableElementType;
#endif
        /// <summary>
        /// 是否可空成员类型
        /// </summary>
        internal bool IsNullable { get { return !object.ReferenceEquals(MemberIndex.MemberSystemType, NullableElementType);  } }
        /// <summary>
        /// 数据库读取数据类型
        /// </summary>
        internal readonly ReaderDataTypeEnum ReaderDataType;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        internal AutoCSer.ORM.Metadata.GenericType GenericType { get { return (AutoCSer.ORM.Metadata.GenericType)genericType.notNull(); } }
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        internal AutoCSer.ORM.Metadata.StructGenericType StructGenericType { get { return (AutoCSer.ORM.Metadata.StructGenericType)genericType.notNull(); } }
        /// <summary>
        /// 自定义数据列名称集合
        /// </summary>
        internal readonly CustomColumnName[] CustomColumnNames;
        /// <summary>
        /// 字段成员
        /// </summary>
        /// <param name="member">字段成员</param>
        /// <param name="attribute">数据库成员信息</param>
        /// <param name="isModel"></param>
#if NetStandard21
        private Member(MemberIndexInfo member, MemberAttribute? attribute, bool isModel)
#else
        private Member(MemberIndexInfo member, MemberAttribute attribute, bool isModel)
#endif
        {
            Type type = member.MemberSystemType;
            MemberIndex = member;
            Attribute = attribute ?? DefaultAttribute;
            ReaderDataType = ReaderDataTypeEnum.String;
            Type nullableType = type.getNullableType() ?? type;
            if (type.IsValueType && !nullableType.IsEnum)
            {
                genericType = AutoCSer.ORM.Metadata.StructGenericType.Get(nullableType);
                if (object.ReferenceEquals(nullableType, type)) CustomColumnAttribute = StructGenericType.CustomColumnAttribute;
            }
            var customColumnNames = default(CustomColumnName[]);
            if (CustomColumnAttribute == null)
            {
                NullableElementType = nullableType;
                if (NullableElementType.IsEnum)
                {
                    Type enumType = System.Enum.GetUnderlyingType(NullableElementType);
                    if (enumType == typeof(int)) ReaderDataType = ReaderDataTypeEnum.Int;
                    else if (enumType == typeof(uint)) ReaderDataType = ReaderDataTypeEnum.Int;
                    else if (enumType == typeof(byte)) ReaderDataType = ReaderDataTypeEnum.Byte;
                    else if (enumType == typeof(ulong)) ReaderDataType = ReaderDataTypeEnum.Long;
                    else if (enumType == typeof(ushort)) ReaderDataType = ReaderDataTypeEnum.Short;
                    else if (enumType == typeof(long)) ReaderDataType = ReaderDataTypeEnum.Long;
                    else if (enumType == typeof(short)) ReaderDataType = ReaderDataTypeEnum.Short;
                    else if (enumType == typeof(sbyte)) ReaderDataType = ReaderDataTypeEnum.Byte;
                }
                else if (NullableElementType.IsValueType)
                {
                    if (NullableElementType == typeof(int)) ReaderDataType = ReaderDataTypeEnum.Int;
                    else if (NullableElementType == typeof(uint)) ReaderDataType = ReaderDataTypeEnum.Int;
                    else if (NullableElementType == typeof(byte)) ReaderDataType = ReaderDataTypeEnum.Byte;
                    else if (NullableElementType == typeof(ulong)) ReaderDataType = ReaderDataTypeEnum.Long;
                    else if (NullableElementType == typeof(ushort)) ReaderDataType = ReaderDataTypeEnum.Short;
                    else if (NullableElementType == typeof(long)) ReaderDataType = ReaderDataTypeEnum.Long;
                    else if (NullableElementType == typeof(short)) ReaderDataType = ReaderDataTypeEnum.Short;
                    else if (NullableElementType == typeof(sbyte)) ReaderDataType = ReaderDataTypeEnum.Byte;
                    else if (NullableElementType == typeof(bool)) ReaderDataType = ReaderDataTypeEnum.Bool;
                    else if (NullableElementType == typeof(DateTime)) ReaderDataType = ReaderDataTypeEnum.DateTime;
                    else if (NullableElementType == typeof(DateTimeOffset)) ReaderDataType = ReaderDataTypeEnum.DateTimeOffset;
                    else if (NullableElementType == typeof(TimeSpan)) ReaderDataType = ReaderDataTypeEnum.TimeSpan;
                    else if (NullableElementType == typeof(decimal)) ReaderDataType = ReaderDataTypeEnum.Decimal;
                    else if (NullableElementType == typeof(Guid)) ReaderDataType = ReaderDataTypeEnum.Guid;
                    else if (NullableElementType == typeof(double)) ReaderDataType = ReaderDataTypeEnum.Double;
                    else if (NullableElementType == typeof(float)) ReaderDataType = ReaderDataTypeEnum.Float;
                }
                if (ReaderDataType == ReaderDataTypeEnum.String && type != typeof(string))
                {
                    ReaderDataType = ReaderDataTypeEnum.Json;
                    genericType = AutoCSer.ORM.Metadata.GenericType.Get(type);
                }
            }
            else
            {
                ReaderDataType = ReaderDataTypeEnum.CustomColumn;

                if (isModel)
                {
                    var parentName = default(string);
                    switch (CustomColumnAttribute.NameConcatType)
                    {
                        case CustomColumnNameConcatTypeEnum.Parent:
                            if (StructGenericType.CustomColumnMemberCount != 1) throw new IndexOutOfRangeException($"{type.fullName()} 字段数量为 {StructGenericType.CustomColumnMemberCount} 不支持仅父节点名称模式");
                            foreach (CustomColumnName name in StructGenericType.GetCustomColumnMemberNames(member.Member.Name, null))
                            {
                                customColumnNames = new CustomColumnName[] { name };
                                break;
                            }
                            break;
                        case CustomColumnNameConcatTypeEnum.Node: parentName = null; break;
                        default: parentName = member.Member.Name; break;
                    }
                    if (customColumnNames == null)
                    {
                        customColumnNames = new CustomColumnName[StructGenericType.CustomColumnMemberCount];
                        int index = 0;
                        foreach(CustomColumnName name in StructGenericType.GetCustomColumnMemberNames(parentName, CustomColumnAttribute.NameConcatSplit ?? string.Empty)) customColumnNames[index++] = name;
                        if (index != customColumnNames.Length) throw new IndexOutOfRangeException($"{type.fullName()} 字段数量 {StructGenericType.CustomColumnMemberCount} 不匹配 {index}");
                    }
                }
            }
            CustomColumnNames = customColumnNames ?? EmptyArray<CustomColumnName>.Array;
        }
        /// <summary>
        /// 递归获取自定义数据列所有表格列名称
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal IEnumerable<CustomColumnName> GetCustomColumnMemberNames(string? parentName, string? nameConcatSplit)
#else
        internal IEnumerable<CustomColumnName> GetCustomColumnMemberNames(string parentName, string nameConcatSplit)
#endif
        {
            var attribute = CustomColumnAttribute.notNull();
            switch (attribute.NameConcatType)
            {
                case CustomColumnNameConcatTypeEnum.Parent:
                    if (StructGenericType.CustomColumnMemberCount != 1) throw new IndexOutOfRangeException($"{MemberIndex.MemberSystemType.fullName()} 字段数量为 {StructGenericType.CustomColumnMemberCount} 不支持仅父节点名称模式");
                    foreach (CustomColumnName name in StructGenericType.GetCustomColumnMemberNames(ConcatMemberName(parentName, nameConcatSplit), null))
                    {
                        yield return name;
                        break;
                    }
                    break;
                case CustomColumnNameConcatTypeEnum.Node:
                    foreach (CustomColumnName name in StructGenericType.GetCustomColumnMemberNames(parentName, attribute.NameConcatSplit ?? string.Empty)) yield return name;
                    break;
                default:
                    foreach (CustomColumnName name in StructGenericType.GetCustomColumnMemberNames(ConcatMemberName(parentName, nameConcatSplit), attribute.NameConcatSplit ?? string.Empty)) yield return name;
                    break;
            }
        }
        /// <summary>
        /// 连接列名称
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal string ConcatMemberName(string? parentName, string? nameConcatSplit)
#else
        internal string ConcatMemberName(string parentName, string nameConcatSplit)
#endif
        {
            if (string.IsNullOrEmpty(parentName)) return MemberIndex.Member.Name;
            if (nameConcatSplit == null) return parentName;
            return parentName + nameConcatSplit + MemberIndex.Member.Name;
        }
        /// <summary>
        /// 获取自定义列名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <returns></returns>
#if NetStandard21
        internal CustomColumnName GetCustomColumnMemberName(MemberExpression? memberExpression, ref LeftArray<MemberExpression> memberExpressions)
#else
        internal CustomColumnName GetCustomColumnMemberName(MemberExpression memberExpression, ref LeftArray<MemberExpression> memberExpressions)
#endif
        {
            var attribute = CustomColumnAttribute.notNull();
            switch (attribute.NameConcatType)
            {
                case CustomColumnNameConcatTypeEnum.Parent: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, MemberIndex.Member.Name, null);
                case CustomColumnNameConcatTypeEnum.Node: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, null, attribute.NameConcatSplit ?? string.Empty);
                default: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, MemberIndex.Member.Name, attribute.NameConcatSplit ?? string.Empty);
            }
        }
        /// <summary>
        /// 获取自定义列名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal CustomColumnName GetCustomColumnMemberName(MemberExpression? memberExpression, ref LeftArray<MemberExpression> memberExpressions, string? parentName, string? nameConcatSplit)
#else
        internal CustomColumnName GetCustomColumnMemberName(MemberExpression memberExpression, ref LeftArray<MemberExpression> memberExpressions, string parentName, string nameConcatSplit)
#endif
        {
            var attribute = CustomColumnAttribute.notNull();
            switch (attribute.NameConcatType)
            {
                case CustomColumnNameConcatTypeEnum.Parent: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, ConcatMemberName(parentName, nameConcatSplit), null);
                case CustomColumnNameConcatTypeEnum.Node: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, parentName, attribute.NameConcatSplit ?? string.Empty);
                default: return StructGenericType.GetCustomColumnMemberName(memberExpression, memberExpressions, ConcatMemberName(parentName, nameConcatSplit), attribute.NameConcatSplit ?? string.Empty);
            }
        }
        /// <summary>
        /// 获取自定义列信息与数值
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal IEnumerable<KeyValue<CustomColumnName, object?>> GetCustomColumnMemberNameValues(MemberExpression? memberExpression, ref LeftArray<MemberExpression> memberExpressions, object value)
#else
        internal IEnumerable<KeyValue<CustomColumnName, object>> GetCustomColumnMemberNameValues(MemberExpression memberExpression, ref LeftArray<MemberExpression> memberExpressions, object value)
#endif
        {
            var attribute = CustomColumnAttribute.notNull();
            switch (attribute.NameConcatType)
            {
                case CustomColumnNameConcatTypeEnum.Parent: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, MemberIndex.Member.Name, null);
                case CustomColumnNameConcatTypeEnum.Node: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, null, attribute.NameConcatSplit ?? string.Empty);
                default: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, MemberIndex.Member.Name, attribute.NameConcatSplit ?? string.Empty);
            }
        }
        /// <summary>
        /// 获取自定义列信息与数值
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <param name="value"></param>
        /// <param name="parentName"></param>
        /// <param name="nameConcatSplit"></param>
        /// <returns></returns>
#if NetStandard21
        internal IEnumerable<KeyValue<CustomColumnName, object?>> GetCustomColumnMemberNameValues(MemberExpression? memberExpression, ref LeftArray<MemberExpression> memberExpressions, object value, string? parentName, string? nameConcatSplit)
#else
        internal IEnumerable<KeyValue<CustomColumnName, object>> GetCustomColumnMemberNameValues(MemberExpression memberExpression, ref LeftArray<MemberExpression> memberExpressions, object value, string parentName, string nameConcatSplit)
#endif
        {
            var attribute = CustomColumnAttribute.notNull();
            switch (attribute.NameConcatType)
            {
                case CustomColumnNameConcatTypeEnum.Parent: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, ConcatMemberName(parentName, nameConcatSplit), null);
                case CustomColumnNameConcatTypeEnum.Node: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, parentName, attribute.NameConcatSplit ?? string.Empty);
                default: return StructGenericType.GetCustomColumnMemberNameValues(memberExpression, memberExpressions, value, ConcatMemberName(parentName, nameConcatSplit), attribute.NameConcatSplit ?? string.Empty);
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTimeOffset readDateTimeOffset(System.Data.Common.DbDataReader reader, int index)
        {
            return (DateTimeOffset)reader.GetValue(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTimeOffset? readDateTimeOffsetNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return (DateTimeOffset)reader.GetValue(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TimeSpan readTimeSpan(System.Data.Common.DbDataReader reader, int index)
        {
            return (TimeSpan)reader.GetValue(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TimeSpan? readTimeSpanNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return (TimeSpan)reader.GetValue(index);
        }
        /// <summary>
        /// 获取读取数据访问信息
        /// </summary>
        /// <returns></returns>
        internal MethodInfo GetReadMethod()
        {
            switch (ReaderDataType)
            {
                case ReaderDataTypeEnum.Int:
                    if (IsNullable) return ((Func<DbDataReader, int, int?>)AutoCSer.ORM.Member.readIntNullable).Method;
                    return ((Func<DbDataReader, int, int>)AutoCSer.ORM.Member.readInt).Method;
                case ReaderDataTypeEnum.Long:
                    if (IsNullable) return ((Func<DbDataReader, int, long?>)readLongNullable).Method;
                    return ((Func<DbDataReader, int, long>)readLong).Method;
                case ReaderDataTypeEnum.Short:
                    if (IsNullable) return ((Func<DbDataReader, int, short?>)AutoCSer.ORM.Member.readShortNullable).Method;
                    return ((Func<DbDataReader, int, short>)AutoCSer.ORM.Member.readShort).Method;
                case ReaderDataTypeEnum.Byte:
                    if (IsNullable) return ((Func<DbDataReader, int, byte?>)AutoCSer.ORM.Member.readByteNullable).Method;
                    return ((Func<DbDataReader, int, byte>)AutoCSer.ORM.Member.readByte).Method;
                case ReaderDataTypeEnum.Bool:
                    if (IsNullable) return ((Func<DbDataReader, int, bool?>)AutoCSer.ORM.Member.readBoolNullable).Method;
                    return ((Func<DbDataReader, int, bool>)AutoCSer.ORM.Member.readBool).Method;
                case ReaderDataTypeEnum.DateTime:
                    if (IsNullable) return ((Func<DbDataReader, int, DateTime?>)AutoCSer.ORM.Member.readDateTimeNullable).Method;
                    return ((Func<DbDataReader, int, DateTime>)AutoCSer.ORM.Member.readDateTime).Method;
                case ReaderDataTypeEnum.DateTimeOffset:
                    if (IsNullable) return ((Func<DbDataReader, int, DateTimeOffset?>)AutoCSer.ORM.Member.readDateTimeOffsetNullable).Method;
                    return ((Func<DbDataReader, int, DateTimeOffset>)AutoCSer.ORM.Member.readDateTimeOffset).Method;
                case ReaderDataTypeEnum.TimeSpan:
                    if (IsNullable) return ((Func<DbDataReader, int, TimeSpan?>)AutoCSer.ORM.Member.readTimeSpanNullable).Method;
                    return ((Func<DbDataReader, int, TimeSpan>)AutoCSer.ORM.Member.readTimeSpan).Method;
                case ReaderDataTypeEnum.Decimal:
                    if (IsNullable) return ((Func<DbDataReader, int, decimal?>)AutoCSer.ORM.Member.readDecimalNullable).Method;
                    return ((Func<DbDataReader, int, decimal>)AutoCSer.ORM.Member.readDecimal).Method;
                case ReaderDataTypeEnum.Guid:
                    if (IsNullable) return ((Func<DbDataReader, int, Guid?>)AutoCSer.ORM.Member.readGuidNullable).Method;
                    return ((Func<DbDataReader, int, Guid>)AutoCSer.ORM.Member.readGuid).Method;
                case ReaderDataTypeEnum.Double:
                    if (IsNullable) return ((Func<DbDataReader, int, double?>)AutoCSer.ORM.Member.readDoubleNullable).Method;
                    return ((Func<DbDataReader, int, double>)AutoCSer.ORM.Member.readDouble).Method;
                case ReaderDataTypeEnum.Float:
                    if (IsNullable) return ((Func<DbDataReader, int, float?>)AutoCSer.ORM.Member.readFloatNullable).Method;
                    return ((Func<DbDataReader, int, float>)AutoCSer.ORM.Member.readFloat).Method;
                case ReaderDataTypeEnum.Json: return GenericType.ReadJsonDelegate.Method;
#if NetStandard21
                default: return ((Func<DbDataReader, int, string?>)ReadString).Method;
#else
                default: return ((Func<DbDataReader, int, string>)ReadString).Method;
#endif
            }
        }

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="properties"></param>
        /// <param name="isModel"></param>
        /// <returns>字段成员集合</returns>
        internal static LeftArray<Member> Get(LeftArray<FieldIndex> fields, LeftArray<PropertyIndex> properties, bool isModel)
        {
            return get(fields, properties, isModel, fields.Length + properties.Length);
        }
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="properties"></param>
        /// <param name="isModel"></param>
        /// <param name="memberCount"></param>
        /// <returns>字段成员集合</returns>
        private static LeftArray<Member> get(IEnumerable<MemberIndexInfo> fields, IEnumerable<MemberIndexInfo> properties, bool isModel, int memberCount)
        {
            LeftArray<Member> values = new LeftArray<Member>(memberCount);
            foreach (MemberIndexInfo member in fields.Concat(properties))
            {
                if (member.CanGet && member.CanSet)
                {
                    Type type = member.MemberSystemType;
                    if (!type.isIgnoreSerialize() && !member.IsIgnore)
                    {
                        var attribute = member.GetAttribute<MemberAttribute>(false);
                        if (attribute == null || !attribute.GetIsIgnoreCurrent) values.Add(new Member(member, attribute, isModel));
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 数据类型排序
        /// </summary>
        private int dataTypeSort
        {
            get
            {
                switch (ReaderDataType)
                {
                    case ReaderDataTypeEnum.Int: return 0;
                    case ReaderDataTypeEnum.Long: return 1;
                    case ReaderDataTypeEnum.DateTime: return 2;
                    case ReaderDataTypeEnum.Byte: return 3;
                    case ReaderDataTypeEnum.Bool: return 4;
                    case ReaderDataTypeEnum.DateTimeOffset: return 5;
                    case ReaderDataTypeEnum.TimeSpan: return 6;
                    case ReaderDataTypeEnum.Guid: return 7;
                    case ReaderDataTypeEnum.Decimal: return 8;
                    case ReaderDataTypeEnum.Short: return 9;
                    case ReaderDataTypeEnum.Float: return 10;
                    case ReaderDataTypeEnum.Double: return 11;
                    case ReaderDataTypeEnum.CustomColumn: return object.ReferenceEquals(CustomColumnNames, EmptyArray<CustomColumnName>.Array) ? 0xfe : CustomColumnNames.Max(p => p.Member.dataTypeSort);
                    default: return 0xff;
                }
            }
        }
        /// <summary>
        /// 字符串长度排序
        /// </summary>
        private int stringSizeSort
        {
            get
            {
                if (ReaderDataType == ReaderDataTypeEnum.CustomColumn) return CustomColumnNames.Where(p => p.Member.dataTypeSort == 0xff).Max(p => p.Member.stringSizeSort);
                var stringAttribute = Attribute as StringAttribute;
                if (stringAttribute == null || stringAttribute.Size == 0) return int.MaxValue;
                if (stringAttribute.IsAscii) return stringAttribute.Size;
                return (int)Math.Min((long)stringAttribute.Size << 1, int.MaxValue);
            }
        }
        /// <summary>
        /// 成员排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Sort(Member left, Member right)
        {
            int value = left.Attribute.MemberSort - right.Attribute.MemberSort;
            if (value != 0) return value;
            value = (byte)right.Attribute.PrimaryKeyType - (byte)left.Attribute.PrimaryKeyType;
            if (value != 0) return value;
            int dataTypeSort = left.dataTypeSort;
            value = dataTypeSort - right.dataTypeSort;
            if (value != 0) return value;
            if (dataTypeSort == 0xff) value = left.stringSizeSort - right.stringSizeSort;
            return value;
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? ReadString(DbDataReader reader, int index)
#else
        internal static string ReadString(DbDataReader reader, int index)
#endif
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetString(index);
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        internal static T? ReadJson<T>(DbDataReader reader, int index)
#else
        internal static T ReadJson<T>(DbDataReader reader, int index)
#endif
        {
            if (reader.IsDBNull(index)) return default(T);
            string jsonString = reader.GetString(index);
            if (string.IsNullOrEmpty(jsonString)) return default(T);
            return AutoCSer.JsonDeserializer.Deserialize<T>(jsonString);
        }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? JsonSerialize<T>(T value)
#else
        internal static string JsonSerialize<T>(T value)
#endif
        {
            return value == null ? null : AutoCSer.JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTimeOffset readDateTimeOffsetObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(DateTimeOffset)) return (DateTimeOffset)reader.GetValue(index);
            if (reader.IsDBNull(index)) return default(DateTimeOffset);
            return DateTimeOffset.Parse(reader[index].ToString().notNull());
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTimeOffset? readDateTimeOffsetNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(DateTimeOffset)) return (DateTimeOffset)reader.GetValue(index);
            return DateTimeOffset.Parse(reader[index].ToString().notNull());
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static TimeSpan readTimeSpanObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(TimeSpan)) return (TimeSpan)reader.GetValue(index);
            if (reader.IsDBNull(index)) return default(TimeSpan);
            return TimeSpan.Parse(reader[index].ToString().notNull());
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static TimeSpan? readTimeSpanNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(TimeSpan)) return (TimeSpan)reader.GetValue(index);
            return TimeSpan.Parse(reader[index].ToString().notNull());
        }
        /// <summary>
        /// 获取读取数据访问信息
        /// </summary>
        /// <returns></returns>
        internal MethodInfo GetReadObjectMethod()
        {
            switch (ReaderDataType)
            {
                case ReaderDataTypeEnum.Int:
                    if (IsNullable) return ((Func<DbDataReader, int, int?>)AutoCSer.ORM.Member.readIntNullableObject).Method;
                    return ((Func<DbDataReader, int, int>)AutoCSer.ORM.Member.readIntObject).Method;
                case ReaderDataTypeEnum.Long:
                    if (IsNullable) return ((Func<DbDataReader, int, long?>)readLongNullableObject).Method;
                    return ((Func<DbDataReader, int, long>)readLongObject).Method;
                case ReaderDataTypeEnum.Short:
                    if (IsNullable) return ((Func<DbDataReader, int, short?>)AutoCSer.ORM.Member.readShortNullableObject).Method;
                    return ((Func<DbDataReader, int, short>)AutoCSer.ORM.Member.readShortObject).Method;
                case ReaderDataTypeEnum.Byte:
                    if (IsNullable) return ((Func<DbDataReader, int, byte?>)AutoCSer.ORM.Member.readByteNullableObject).Method;
                    return ((Func<DbDataReader, int, byte>)AutoCSer.ORM.Member.readByteObject).Method;
                case ReaderDataTypeEnum.Bool:
                    if (IsNullable) return ((Func<DbDataReader, int, bool?>)AutoCSer.ORM.Member.readBoolNullableObject).Method;
                    return ((Func<DbDataReader, int, bool>)AutoCSer.ORM.Member.readBoolObject).Method;
                case ReaderDataTypeEnum.DateTime:
                    if (IsNullable) return ((Func<DbDataReader, int, DateTime?>)AutoCSer.ORM.Member.readDateTimeNullableObject).Method;
                    return ((Func<DbDataReader, int, DateTime>)AutoCSer.ORM.Member.readDateTimeObject).Method;
                case ReaderDataTypeEnum.DateTimeOffset:
                    if (IsNullable) return ((Func<DbDataReader, int, DateTimeOffset?>)AutoCSer.ORM.Member.readDateTimeOffsetNullableObject).Method;
                    return ((Func<DbDataReader, int, DateTimeOffset>)AutoCSer.ORM.Member.readDateTimeOffsetObject).Method;
                case ReaderDataTypeEnum.TimeSpan:
                    if (IsNullable) return ((Func<DbDataReader, int, TimeSpan?>)AutoCSer.ORM.Member.readTimeSpanNullableObject).Method;
                    return ((Func<DbDataReader, int, TimeSpan>)AutoCSer.ORM.Member.readTimeSpanObject).Method;
                case ReaderDataTypeEnum.Decimal:
                    if (IsNullable) return ((Func<DbDataReader, int, decimal?>)AutoCSer.ORM.Member.readDecimalNullableObject).Method;
                    return ((Func<DbDataReader, int, decimal>)AutoCSer.ORM.Member.readDecimalObject).Method;
                case ReaderDataTypeEnum.Guid:
                    if (IsNullable) return ((Func<DbDataReader, int, Guid?>)AutoCSer.ORM.Member.readGuidNullableObject).Method;
                    return ((Func<DbDataReader, int, Guid>)AutoCSer.ORM.Member.readGuidObject).Method;
                case ReaderDataTypeEnum.Double:
                    if (IsNullable) return ((Func<DbDataReader, int, double?>)AutoCSer.ORM.Member.readDoubleNullableObject).Method;
                    return ((Func<DbDataReader, int, double>)AutoCSer.ORM.Member.readDoubleObject).Method;
                case ReaderDataTypeEnum.Float:
                    if (IsNullable) return ((Func<DbDataReader, int, float?>)AutoCSer.ORM.Member.readFloatNullableObject).Method;
                    return ((Func<DbDataReader, int, float>)AutoCSer.ORM.Member.readFloatObject).Method;
                case ReaderDataTypeEnum.Json: return GenericType.ReadJsonDelegate.Method;
#if NetStandard21
                default: return ((Func<DbDataReader, int, string?>)ReadStringObject).Method;
#else
                default: return ((Func<DbDataReader, int, string>)ReadStringObject).Method;
#endif
            }
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? ReadStringObject(DbDataReader reader, int index)
#else
        internal static string ReadStringObject(DbDataReader reader, int index)
#endif
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(string)) return reader.GetString(index);
            return reader[index].ToString();
        }
        /// <summary>
        /// 获取数据库数据类型
        /// </summary>
        /// <returns></returns>
        internal Type GetObjectType()
        {
            switch (ReaderDataType)
            {
                case ReaderDataTypeEnum.Int: return typeof(int);
                case ReaderDataTypeEnum.Long: return typeof(long);
                case ReaderDataTypeEnum.Short: return typeof(short);
                case ReaderDataTypeEnum.Byte: return typeof(byte);
                case ReaderDataTypeEnum.Bool: return typeof(bool);
                case ReaderDataTypeEnum.DateTime: return typeof(DateTime);
                case ReaderDataTypeEnum.DateTimeOffset: return typeof(DateTimeOffset);
                case ReaderDataTypeEnum.TimeSpan: return typeof(TimeSpan);
                case ReaderDataTypeEnum.Decimal: return typeof(decimal);
                case ReaderDataTypeEnum.Guid: return typeof(Guid);
                case ReaderDataTypeEnum.Double: return typeof(double);
                case ReaderDataTypeEnum.Float: return typeof(float);
                default: return typeof(string);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTimeOffset readDateTimeOffset(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDateTimeOffset();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTimeOffset? readDateTimeOffsetNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDateTimeOffsetNullable();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TimeSpan readTimeSpan(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadTimeSpan();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TimeSpan? readTimeSpanNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadTimeSpanNullable();
        }
        /// <summary>
        /// 获取读取数据访问信息
        /// </summary>
        /// <returns></returns>
        internal MethodInfo GetRemoteProxyReadMethod()
        {
            switch (ReaderDataType)
            {
                case ReaderDataTypeEnum.Int:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, int?>)AutoCSer.ORM.Member.readIntNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, int>)AutoCSer.ORM.Member.readInt).Method;
                case ReaderDataTypeEnum.Long:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, long?>)readLongNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, long>)readLong).Method;
                case ReaderDataTypeEnum.Short:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, short?>)AutoCSer.ORM.Member.readShortNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, short>)AutoCSer.ORM.Member.readShort).Method;
                case ReaderDataTypeEnum.Byte:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, byte?>)AutoCSer.ORM.Member.readByteNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, byte>)AutoCSer.ORM.Member.readByte).Method;
                case ReaderDataTypeEnum.Bool:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, bool?>)AutoCSer.ORM.Member.readBoolNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, bool>)AutoCSer.ORM.Member.readBool).Method;
                case ReaderDataTypeEnum.DateTime:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, DateTime?>)AutoCSer.ORM.Member.readDateTimeNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, DateTime>)AutoCSer.ORM.Member.readDateTime).Method;
                case ReaderDataTypeEnum.DateTimeOffset:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, DateTimeOffset?>)AutoCSer.ORM.Member.readDateTimeOffsetNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, DateTimeOffset>)AutoCSer.ORM.Member.readDateTimeOffset).Method;
                case ReaderDataTypeEnum.TimeSpan:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, TimeSpan?>)AutoCSer.ORM.Member.readTimeSpanNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, TimeSpan>)AutoCSer.ORM.Member.readTimeSpan).Method;
                case ReaderDataTypeEnum.Decimal:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, decimal?>)AutoCSer.ORM.Member.readDecimalNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, decimal>)AutoCSer.ORM.Member.readDecimal).Method;
                case ReaderDataTypeEnum.Guid:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, Guid?>)AutoCSer.ORM.Member.readGuidNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, Guid>)AutoCSer.ORM.Member.readGuid).Method;
                case ReaderDataTypeEnum.Double:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, double?>)AutoCSer.ORM.Member.readDoubleNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, double>)AutoCSer.ORM.Member.readDouble).Method;
                case ReaderDataTypeEnum.Float:
                    if (IsNullable) return ((Func<RemoteProxy.DataValue[], int, float?>)AutoCSer.ORM.Member.readFloatNullable).Method;
                    return ((Func<RemoteProxy.DataValue[], int, float>)AutoCSer.ORM.Member.readFloat).Method;
                case ReaderDataTypeEnum.Json: return GenericType.ReadRemoteProxyJsonDelegate.Method;
#if NetStandard21
                default: return ((Func<RemoteProxy.DataValue[], int, string?>)ReadRemoteProxyString).Method;
#else
                default: return ((Func<RemoteProxy.DataValue[], int, string>)ReadRemoteProxyString).Method;
#endif
            }
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static string? ReadRemoteProxyString(RemoteProxy.DataValue[] row, int index)
#else
        internal static string ReadRemoteProxyString(RemoteProxy.DataValue[] row, int index)
#endif
        {
            return row[index].ReadString();
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        internal static T? ReadRemoteProxyJson<T>(RemoteProxy.DataValue[] row, int index)
#else
        internal static T ReadRemoteProxyJson<T>(RemoteProxy.DataValue[] row, int index)
#endif
        {
            var jsonString = row[index].ReadString();
            if (string.IsNullOrEmpty(jsonString)) return default(T);
            return AutoCSer.JsonDeserializer.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="isObjectToString"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal KeyValue<MethodInfo, bool> GetConstantConvertMethod(out bool isObjectToString)
        {
            return ConnectionCreator.GetConstantConvertMethod(MemberIndex.MemberSystemType, ReaderDataType, Attribute, out isObjectToString);
        }

        /// <summary>
        /// 获取数据验证方法
        /// </summary>
#if NetStandard21
        internal MethodInfo? VerifyMethod
#else
        internal MethodInfo VerifyMethod
#endif
        {
            get
            {
                switch (ReaderDataType)
                {
                    case ReaderDataTypeEnum.String:
                    case ReaderDataTypeEnum.Json:
                        return ((Func<TableWriter, int, string, string>)Verify).Method;
                    case ReaderDataTypeEnum.DateTime:
                        if (IsNullable) return ((Func<TableWriter, int, DateTime?, DateTime?>)Verify).Method;
                        return ((Func<TableWriter, int, DateTime, DateTime>)Verify).Method;
                    case ReaderDataTypeEnum.Decimal:
                        if (IsNullable) return ((Func<TableWriter, int, decimal?, decimal?>)Verify).Method;
                        return ((Func<TableWriter, int, decimal, decimal>)Verify).Method;
                }
                return null;
            }
        }
        /// <summary>
        /// 字符串数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string verify(string value)
        {
            StringAttribute stringAttribute = Attribute as StringAttribute ?? StringAttribute.Default;
            if (!stringAttribute.IsNullable && value == null) throw new InvalidCastException($"{Attribute.Remark} {MemberIndex.Member.Name} 不允许为 null");
            if (!stringAttribute.IsEmpty && string.IsNullOrEmpty(value)) throw new InvalidCastException($"{Attribute.Remark} {MemberIndex.Member.Name} 不允许为空字符串");
            value = stringAttribute.Verify(value);
            if (string.IsNullOrEmpty(value) || stringAttribute.Size == 0) return value;
            ushort size = stringAttribute.Size;
            if (stringAttribute.IsAscii)
            {
                if (size > 8000) size = 8000;
                if ((value.Length << 1) <= size) return value;
                int length = value.Length;
                foreach(char code in value)
                {
                    if (code > sbyte.MaxValue) ++length;
                }
                if (length <= size) return value;
            }
            else
            {
                if (value.Length <= Math.Min((int)size, 4000)) return value;
            }
            throw new IndexOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} 长度 {value.Length} 超出 {size} 限制");
        }
        /// <summary>
        /// 字符串数据验证
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static string Verify(TableWriter tableWriter, int columnIndex, string value)
        {
            return tableWriter.Columns[columnIndex].Member.verify(value);
        }
        /// <summary>
        /// 时间数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private DateTime verify(DateTime value)
        {
            DateTimeAttribute dateTimeAttribute = Attribute as DateTimeAttribute ?? DateTimeAttribute.Default;
            value = dateTimeAttribute.Verify(value);
            switch (dateTimeAttribute.Type)
            {
                case DateTimeTypeEnum.DateTime:
                    if (value < DateTimeAttribute.MinSmallDateTime) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value.toString()} 超出 {DateTimeAttribute.MinDateTime.toString()} 限制");
                    break;
                case DateTimeTypeEnum.SmallDateTime:
                    if (value < DateTimeAttribute.MinSmallDateTime) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value.toString()} 超出 {DateTimeAttribute.MinSmallDateTime.toString()} 限制");
                    if (value >= DateTimeAttribute.MaxSmallDateTime) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value.toString()} 超出 {DateTimeAttribute.MaxSmallDateTime.toString()} 限制");
                    break;
            }
            return value;
        }
        /// <summary>
        /// 时间数据验证
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DateTime Verify(TableWriter tableWriter, int columnIndex, DateTime value)
        {
            return tableWriter.Columns[columnIndex].Member.verify(value);
        }
        /// <summary>
        /// 时间数据验证
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DateTime? Verify(TableWriter tableWriter, int columnIndex, DateTime? value)
        {
            if (value.HasValue) return tableWriter.Columns[columnIndex].Member.verify(value.Value);
            return value;
        }
        /// <summary>
        /// 小数数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal verify(decimal value)
        {
            var moneyAttribute = Attribute as MoneyAttribute;
            if (moneyAttribute != null)
            {
                value = moneyAttribute.Verify(value);
                if (moneyAttribute.IsSmall)
                {
                    if (value < MoneyAttribute.MinSmallMoney) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value} 超出 -214748.3648 限制");
                    if (value > MoneyAttribute.MaxSmallMoney) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value} 超出 214748.3647 限制");
                }
                else
                {
                    if (value < -922337203685477.5808M) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value} 超出 -922337203685477.5808 限制");
                    if (value > 922337203685477.5807M) throw new ArgumentOutOfRangeException($"{Attribute.Remark} {MemberIndex.Member.Name} {value} 超出 922337203685477.5807 限制");
                }
                return value;
            }
            return (Attribute as DecimalAttribute ?? DecimalAttribute.Default).Verify(value);
        }
        /// <summary>
        /// 时间数据验证
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static decimal Verify(TableWriter tableWriter, int columnIndex, decimal value)
        {
            return tableWriter.Columns[columnIndex].Member.verify(value);
        }
        /// <summary>
        /// 时间数据验证
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static decimal? Verify(TableWriter tableWriter, int columnIndex, decimal? value)
        {
            if (value.HasValue) return tableWriter.Columns[columnIndex].Member.verify(value.Value);
            return value;
        }
        /// <summary>
        /// 获取自定义接口数据验证方法
        /// </summary>
#if NetStandard21
        internal MethodInfo? VerifyInterfaceMethod
#else
        internal MethodInfo VerifyInterfaceMethod
#endif
        {
            get
            {
                switch (ReaderDataType)
                {
                    case ReaderDataTypeEnum.Json:
                    case ReaderDataTypeEnum.CustomColumn:
                        if (typeof(IVerify).IsAssignableFrom(MemberIndex.MemberSystemType))
                        {
                            MethodInfo method = MemberIndex.MemberSystemType.GetMethod(nameof(Verify), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, EmptyArray<Type>.Array, null).notNull();
                            if (method.ReturnType == MemberIndex.MemberSystemType) return method;
                        }
                        break;
                }
                return null;
            }
        }

        /// <summary>
        /// int引用参数类型
        /// </summary>
        internal static readonly Type RefIntType = typeof(int).MakeByRefType();
    }
}
