using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 创建数据库连接
    /// </summary>
    public abstract class ConnectionCreator : IDisposable
    {
        /// <summary>
        /// 常量转换字符串处理
        /// </summary>
        private readonly Action<CharStream, object> constantConvertToString;
        /// <summary>
        /// 常量转换处理集合
        /// </summary>
        protected readonly Dictionary<HashObject<System.Type>, Action<CharStream, object>> constantConverters;
        /// <summary>
        /// 常量转换处理集合 访问锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock constantConverterLock;
        /// <summary>
        /// SQL 字符流 临时缓存
        /// </summary>
        private CharStream charStreamCache;
        /// <summary>
        /// 自增ID 数据库表格持久化写入
        /// </summary>
        internal TableWriter<AutoIdentity, string> AutoIdentityWriter;
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        public ConnectionCreator()
        {
            constantConverters = DictionaryCreator.CreateHashObject<System.Type, Action<CharStream, object>>();
            constantConverters.Add(typeof(bool), convertBool);
            constantConverters.Add(typeof(bool?), convertBoolNullable);
            constantConverters.Add(typeof(byte), convertByte);
            constantConverters.Add(typeof(byte?), convertByteNullable);
            constantConverters.Add(typeof(sbyte), convertSByte);
            constantConverters.Add(typeof(sbyte?), convertSByteNullable);
            constantConverters.Add(typeof(short), convertShort);
            constantConverters.Add(typeof(short?), convertShortNullable);
            constantConverters.Add(typeof(ushort), convertUShort);
            constantConverters.Add(typeof(ushort?), convertUShortNullable);
            constantConverters.Add(typeof(int), convertInt);
            constantConverters.Add(typeof(int?), convertIntNullable);
            constantConverters.Add(typeof(uint), convertUInt);
            constantConverters.Add(typeof(uint?), convertUIntNullable);
            constantConverters.Add(typeof(long), convertLong);
            constantConverters.Add(typeof(long?), convertLongNullable);
            constantConverters.Add(typeof(ulong), convertULong);
            constantConverters.Add(typeof(ulong?), convertULongNullable);
            constantConverters.Add(typeof(float), convertFloat);
            constantConverters.Add(typeof(float?), convertFloatNullable);
            constantConverters.Add(typeof(double), convertDouble);
            constantConverters.Add(typeof(double?), convertDoubleNullable);
            constantConverters.Add(typeof(decimal), convertDecimal);
            constantConverters.Add(typeof(decimal?), convertDecimalNullable);
            constantConverters.Add(typeof(DateTimeOffset), convertDateTimeOffset);
            constantConverters.Add(typeof(DateTimeOffset?), convertDateTimeOffsetNullable);
            constantConverters.Add(typeof(TimeSpan), convertTimeSpan);
            constantConverters.Add(typeof(TimeSpan?), convertTimeSpanNullable);
            constantConverters.Add(typeof(Guid), convertGuid);
            constantConverters.Add(typeof(Guid?), convertGuidNullable);
            constantConverters.Add(typeof(string), convertString);
            constantConvertToString = convertToString;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            charStreamCache?.Dispose();
        }
        /// <summary>
        /// 获取 SQL 字符流
        /// </summary>
        /// <returns></returns>
        internal CharStream GetCharStreamCache()
        {
            CharStream charStream = Interlocked.Exchange(ref this.charStreamCache, null);
            if (charStream == null) charStream = new CharStream(UnmanagedPool.Default);
            else charStream.Clear();
            return charStream;
        }
        /// <summary>
        /// 释放 SQL 字符流
        /// </summary>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeCharStreamCache(CharStream charStream)
        {
            if (this.charStreamCache == null) charStream = Interlocked.Exchange(ref this.charStreamCache, charStream);
            charStream?.Dispose();
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        internal abstract Task<DbConnection> CreateConnection();
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal abstract string FormatName(string name);
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="name"></param>
        internal abstract void FormatName(CharStream charStream, string name);
        /// <summary>
        /// 自动创建数据库表格
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <returns></returns>
        internal abstract Task AutoCreateTable(TableWriter tableWriter);
        /// <summary>
        /// 创建表格索引
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="columns"></param>
        /// <param name="indexNameSuffix"></param>
        /// <param name="isUnique"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        internal abstract Task<bool> CreateIndex(TableWriter tableWriter, CustomColumnName[] columns, string indexNameSuffix, bool isUnique, int timeoutSeconds);
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="skipCount">跳过记录数量</param>
        /// <param name="isSubQuery">如果是子查询则在前后增加小括号</param>
        /// <returns></returns>
        internal abstract string GetQueryStatement<T>(QueryBuilder<T> query, MemberMap<T> memberMap, uint readCount, ulong skipCount, bool isSubQuery) where T : class;
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="memberMap"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="skipCount">跳过记录数量</param>
        /// <param name="charStream"></param>
        internal abstract void GetQueryStatement<T>(QueryBuilder<T> query, MemberMap<T> memberMap, uint readCount, ulong skipCount, CharStream charStream) where T : class;
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="extensionQueryData"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="isSubQuery">如果是子查询则在前后增加小括号</param>
        /// <returns></returns>
        internal abstract string GetQueryStatement<T>(QueryBuilder<T> query, ref ExtensionQueryData extensionQueryData, uint readCount, bool isSubQuery) where T : class;
        /// <summary>
        /// 生成查询 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="extensionQueryData"></param>
        /// <param name="readCount">读取数据数量，0 表示不限制</param>
        /// <param name="charStream"></param>
        internal abstract void GetQueryStatement<T>(QueryBuilder<T> query, ref ExtensionQueryData extensionQueryData, uint readCount, CharStream charStream) where T : class;
        /// <summary>
        /// 写入自定义数据列查询条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        internal void WriteCustomColumnCondition(CharStream charStream, TableWriter tableWriter, Member member, object value)
        {
            int index = 0;
            object[] array = tableWriter.GetColumnValueCache(member.CustomColumnNames.Length);
            try
            {
                member.StructGenericType.CustomColumnToArray(value, array, ref index);

                index = 0;
                foreach (CustomColumnName name in member.CustomColumnNames)
                {
                    value = array[index];
                    if (index++ != 0) charStream.SimpleWrite(" and ");
                    WriteCondition(charStream, name, value);
                }
            }
            finally { tableWriter.FreeColumnValueCache(array); }
        }
        /// <summary>
        /// 写入查询条件
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        internal void WriteCondition(CharStream charStream, CustomColumnName name, object value)
        {
            FormatName(charStream, name.Name);
            if (value == null) charStream.SimpleWrite(" is null");
            else
            {
                charStream.Write('=');
                GetConstantConverter(value.GetType(), name.Member)(charStream, value);
            }
        }

        /// <summary>
        /// 获取添加数据 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableWriter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal abstract string GetInsertStatement<T>(TableWriter<T> tableWriter, T value) where T : class;
        /// <summary>
        /// 获取更新数据 SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal abstract string GetUpdateStatement<T>(QueryBuilder<T> query, T value, MemberMap<T> memberMap) where T : class;
        /// <summary>
        /// 获取删除数据 SQL 语句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal abstract string GetDeleteStatement(QueryBuilder query);

        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        internal Action<CharStream, object> GetConstantConverter(Type type, Member member)
        {
            Action<CharStream, object> value;
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                DateTimeAttribute dateTimeAttribute = member?.Attribute as DateTimeAttribute ?? DateTimeAttribute.Default;
                if (type == typeof(DateTime))
                {
                    switch (dateTimeAttribute.Type)
                    {
                        case DateTimeTypeEnum.DateTime2: return convertDateTime2;
                        case DateTimeTypeEnum.SmallDateTime: return convertSmallDateTime;
                        case DateTimeTypeEnum.Date: return convertDate;
                    }
                    return convertDateTime;
                }
                else
                {
                    switch (dateTimeAttribute.Type)
                    {
                        case DateTimeTypeEnum.DateTime2: return convertDateTime2Nullable;
                        case DateTimeTypeEnum.SmallDateTime: return convertSmallDateTimeNullable;
                        case DateTimeTypeEnum.Date: return convertDateNullable;
                    }
                    return convertDateTimeNullable;
                }
            }
            if (constantConverters.TryGetValue(type, out value)) return value;
            Type nullableType = type.getNullableType() ?? type;
            if (nullableType.IsEnum)
            {
                AutoCSer.ORM.Metadata.EnumGenericType genericType = AutoCSer.ORM.Metadata.EnumGenericType.Get(type);
                constantConverterLock.EnterYield();
                try
                {
                    if (!constantConverters.TryGetValue(type, out value))
                    {
                        constantConverters.Add(type, value = object.ReferenceEquals(nullableType, type) ? genericType.GetConstantConverter(this) : genericType.GetNullableConstantConverter(this));
                    }
                }
                finally { constantConverterLock.Exit(); }
                return value;
            }
            return constantConvertToString;
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertBool(CharStream charStream, object parameter)
        {
            convert(charStream, (bool)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        protected virtual void convert(CharStream charStream, bool value)
        {
            charStream.Write(value ? '1' : '0');
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator">SQL字符流</param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, bool parameter)
        {
            connectionCreator.convert(charStream, parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertBoolNullable(CharStream charStream, object parameter)
        {
            convert(charStream, (bool?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convert(CharStream charStream, bool? value)
        {
            if (value.HasValue) convert(charStream, value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator">SQL字符流</param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, bool? parameter)
        {
            connectionCreator.convert(charStream, parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertByte(CharStream charStream, object parameter)
        {
            charStream.WriteString((byte)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, byte parameter)
        {
            charStream.WriteString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertByteNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (byte?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, byte? value)
        {
            if (value.HasValue) charStream.WriteString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertSByte(CharStream charStream, object parameter)
        {
            charStream.WriteString((byte)(sbyte)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, sbyte parameter)
        {
            charStream.WriteString((byte)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertSByteNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (sbyte?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, sbyte? value)
        {
            if (value.HasValue) charStream.WriteString((byte)value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertShort(CharStream charStream, object parameter)
        {
            charStream.WriteString((short)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, short parameter)
        {
            charStream.WriteString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertShortNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (short?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, short? value)
        {
            if (value.HasValue) charStream.WriteString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertUShort(CharStream charStream, object parameter)
        {
            charStream.WriteString((short)(ushort)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, ushort parameter)
        {
            charStream.WriteString((short)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertUShortNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (ushort?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, ushort? value)
        {
            if (value.HasValue) charStream.WriteString((short)value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertInt(CharStream charStream, object parameter)
        {
            charStream.WriteString((int)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, int parameter)
        {
            charStream.WriteString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertIntNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (int?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, int? value)
        {
            if (value.HasValue) charStream.WriteString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertUInt(CharStream charStream, object parameter)
        {
            charStream.WriteString((int)(uint)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, uint parameter)
        {
            charStream.WriteString((int)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertUIntNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (uint?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, uint? value)
        {
            if (value.HasValue) charStream.WriteString((int)value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertLong(CharStream charStream, object parameter)
        {
            charStream.WriteString((long)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, long parameter)
        {
            charStream.WriteString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertLongNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (long?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, long? value)
        {
            if (value.HasValue) charStream.WriteString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertULong(CharStream charStream, object parameter)
        {
            charStream.WriteString((long)(ulong)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, ulong parameter)
        {
            charStream.WriteString((long)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertULongNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (ulong?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, ulong? value)
        {
            if (value.HasValue) charStream.WriteString((long)value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertFloat(CharStream charStream, object parameter)
        {
            charStream.SimpleWrite(((float)parameter).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, float parameter)
        {
            charStream.SimpleWrite(parameter.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertFloatNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (float?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, float? value)
        {
            if (value.HasValue) charStream.SimpleWrite(value.Value.ToString());
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDouble(CharStream charStream, object parameter)
        {
            charStream.SimpleWrite(((double)parameter).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, double parameter)
        {
            charStream.SimpleWrite(parameter.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDoubleNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (double?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, double? value)
        {
            if (value.HasValue) charStream.SimpleWrite(value.Value.ToString());
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDecimal(CharStream charStream, object parameter)
        {
            charStream.SimpleWrite(((decimal)parameter).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, decimal parameter)
        {
            charStream.SimpleWrite(parameter.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDecimalNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (decimal?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, decimal? value)
        {
            if (value.HasValue) charStream.SimpleWrite(value.Value.ToString());
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDateTime(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime)parameter, DateTimeTypeEnum.DateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDateTime2(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime)parameter, DateTimeTypeEnum.DateTime2);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertSmallDateTime(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime)parameter, DateTimeTypeEnum.SmallDateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDate(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime)parameter, DateTimeTypeEnum.Date);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        /// <param name="type"></param>
        protected virtual void convert(CharStream charStream, DateTime parameter, DateTimeTypeEnum type)
        {
            switch (type)
            {
                case DateTimeTypeEnum.DateTime2: charStream.WriteSqlDateTime2String(parameter); return;
                case DateTimeTypeEnum.SmallDateTime: charStream.WriteSqlSmallDateTimeString(parameter); return;
                case DateTimeTypeEnum.Date: charStream.WriteSqlDateString(parameter); return;
                default: charStream.WriteSqlDateTimeString(parameter); return;
            }
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDateTime(ConnectionCreator connectionCreator, CharStream charStream, DateTime parameter)
        {
            connectionCreator.convert(charStream, parameter, DateTimeTypeEnum.DateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDateTime2(ConnectionCreator connectionCreator, CharStream charStream, DateTime parameter)
        {
            connectionCreator.convert(charStream, parameter, DateTimeTypeEnum.DateTime2);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertSmallDateTime(ConnectionCreator connectionCreator, CharStream charStream, DateTime parameter)
        {
            connectionCreator.convert(charStream, parameter, DateTimeTypeEnum.SmallDateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDate(ConnectionCreator connectionCreator, CharStream charStream, DateTime parameter)
        {
            connectionCreator.convert(charStream, parameter, DateTimeTypeEnum.Date);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convertDateTimeNullable(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime?)parameter, DateTimeTypeEnum.DateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convertDateTime2Nullable(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime?)parameter, DateTimeTypeEnum.DateTime2);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convertSmallDateTimeNullable(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime?)parameter, DateTimeTypeEnum.SmallDateTime);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convertDateNullable(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTime?)parameter, DateTimeTypeEnum.Date);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        /// <param name="type">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convert(CharStream charStream, DateTime? value, DateTimeTypeEnum type)
        {
            if (value.HasValue) convert(charStream, value.Value, type);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDateTime(ConnectionCreator connectionCreator, CharStream charStream, DateTime? parameter)
        {
            if (parameter.HasValue) connectionCreator.convert(charStream, parameter.Value, DateTimeTypeEnum.DateTime);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDateTime2(ConnectionCreator connectionCreator, CharStream charStream, DateTime? parameter)
        {
            if (parameter.HasValue) connectionCreator.convert(charStream, parameter.Value, DateTimeTypeEnum.DateTime2);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertSmallDateTime(ConnectionCreator connectionCreator, CharStream charStream, DateTime? parameter)
        {
            if (parameter.HasValue) connectionCreator.convert(charStream, parameter.Value, DateTimeTypeEnum.SmallDateTime);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void convertDate(ConnectionCreator connectionCreator, CharStream charStream, DateTime? parameter)
        {
            if (parameter.HasValue) connectionCreator.convert(charStream, parameter.Value, DateTimeTypeEnum.Date);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDateTimeOffset(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTimeOffset)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convert(CharStream charStream, DateTimeOffset parameter)
        {
            convert(charStream, parameter.UtcDateTime, DateTimeTypeEnum.DateTime2);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, DateTimeOffset parameter)
        {
            connectionCreator.convert(charStream, parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertDateTimeOffsetNullable(CharStream charStream, object parameter)
        {
            convert(charStream, (DateTimeOffset?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convert(CharStream charStream, DateTimeOffset? value)
        {
            if (value.HasValue) convert(charStream, value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, DateTimeOffset? value)
        {
            connectionCreator.convert(charStream, value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertTimeSpan(CharStream charStream, object parameter)
        {
            charStream.WriteSqlString((TimeSpan)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, TimeSpan parameter)
        {
            charStream.WriteSqlString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertTimeSpanNullable(CharStream charStream, object parameter)
        {
            Convert(charStream, (TimeSpan?)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(CharStream charStream, TimeSpan? value)
        {
            if (value.HasValue) charStream.WriteSqlString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertGuid(CharStream charStream, object parameter)
        {
            convert(charStream, (Guid)parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        protected virtual void convert(CharStream charStream, Guid parameter)
        {
            charStream.WriteString(parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, Guid parameter)
        {
            connectionCreator.convert(charStream, parameter);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertGuidNullable(CharStream charStream, object parameter)
        {
            Guid? value = (Guid?)parameter;
            if (value.HasValue) charStream.WriteString(value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convert(CharStream charStream, Guid? value)
        {
            if (value.HasValue) convert(charStream, value.Value);
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, Guid? value)
        {
            connectionCreator.convert(charStream, value);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="charStream">SQL字符流</param>
        /// <param name="parameter">常量</param>
        private void convertString(CharStream charStream, object parameter)
        {
            Convert(charStream, (string)parameter);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        private void convertToString(CharStream charStream, object parameter)
        {
            Convert(charStream, parameter == null ? null : parameter.ToString());
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        internal unsafe virtual void Convert(CharStream charStream, string value)
        {
            if (value == null) charStream.WriteJsonNull();
            else
            {
                fixed (char* valueFixed = value)
                {
                    int length = 0;
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        if (*start == '\'') ++length;
                    }
                    if (length == 0)
                    {
                        charStream.Write(value, '\'');
                        return;
                    }
                    charStream.PrepCharSize((length += value.Length) + 2);
                    charStream.Data.Pointer.Write('\'');
                    byte* write = (byte*)charStream.CurrentChar;
                    for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                    {
                        if (*start != '\'')
                        {
                            *(char*)write = *start;
                            write += sizeof(char);
                        }
                        else
                        {
                            *(int*)write = ('\'' << 16) + '\'';
                            write += sizeof(int);
                        }
                    }
                    charStream.Data.Pointer.CurrentIndex += length * sizeof(char);
                    charStream.Data.Pointer.Write('\'');
                }
            }
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Convert(ConnectionCreator connectionCreator, CharStream charStream, string value)
        {
            connectionCreator.Convert(charStream, value);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertToString(ConnectionCreator connectionCreator, CharStream charStream, object parameter)
        {
            connectionCreator.Convert(charStream, parameter == null ? null : parameter.ToString());
        }
        /// <summary>
        /// LIKE 字符串转义
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        internal unsafe virtual void ConvertLike(CharStream charStream, string value, bool isStart, bool isEnd)
        {
            charStream.Write('\'');
            if (isStart) charStream.Write('%');
            if (value != null)
            {
                foreach (char code in value)
                {
                    switch (code)
                    {
                        case '[':
                        case '_':
                        case '%':
                            charStream.Write('[');
                            charStream.Write(code);
                            charStream.Write(']');
                            break;
                        default:
                            charStream.Write(code);
                            if (code == '\'') charStream.Write('\'');
                            break;
                    }
                }
            }
            if (isEnd) charStream.Write('%');
            charStream.Write('\'');
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertByteEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, byte>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertByteEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, byte>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertByteEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertByteEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertByteEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString(EnumGenericType<T, byte>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertSByteEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString((byte)EnumGenericType<T, sbyte>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertSByteEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString((byte)EnumGenericType<T, sbyte>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertSByteEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertSByteEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertSByteEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString((byte)EnumGenericType<T, sbyte>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertShortEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, short>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertShortEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, short>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertShortEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertShortEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertShortEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString(EnumGenericType<T, short>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertUShortEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString((short)EnumGenericType<T, ushort>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertUShortEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString((short)EnumGenericType<T, ushort>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertUShortEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertUShortEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertUShortEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString((short)EnumGenericType<T, ushort>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertIntEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, int>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertIntEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, int>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertIntEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertIntEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertIntEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString(EnumGenericType<T, int>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertUIntEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString((int)EnumGenericType<T, uint>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertUIntEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString((int)EnumGenericType<T, uint>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertUIntEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertUIntEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertUIntEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString((int)EnumGenericType<T, uint>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertLongEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, long>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertLongEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString(EnumGenericType<T, long>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertLongEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertLongEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertLongEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString(EnumGenericType<T, long>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertULongEnum<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            charStream.WriteString((long)EnumGenericType<T, ulong>.ToInt((T)parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertULongEnum<T>(CharStream charStream, T parameter) where T : struct, IConvertible
        {
            charStream.WriteString((long)EnumGenericType<T, ulong>.ToInt(parameter));
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="parameter"></param>
        internal void ConvertULongEnumNullable<T>(CharStream charStream, object parameter) where T : struct, IConvertible
        {
            ConvertULongEnumNullable(charStream, (T?)parameter);
        }
        /// <summary>
        /// 枚举常量转换字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ConvertULongEnumNullable<T>(CharStream charStream, T? value) where T : struct, IConvertible
        {
            if (value.HasValue) charStream.WriteString((long)EnumGenericType<T, ulong>.ToInt(value.Value));
            else charStream.WriteJsonNull();
        }
        /// <summary>
        /// 写入逗号连接符
        /// </summary>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteConcat(CharStream charStream)
        {
            charStream.Write(',');
        }
        /// <summary>
        /// 写入更新数据列名称
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteConcat(CharStream charStream, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            if (!isFirst) charStream.Write(',');
            tableWriter.ConnectionPool.Creator.FormatName(charStream, tableWriter.Columns[columnIndex].Name);
            charStream.Write('=');
        }
        /// <summary>
        /// 写入更新数据列名称
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteConcat(CharStream charStream, TableWriter tableWriter, int columnIndex)
        {
            charStream.Write(',');
            tableWriter.ConnectionPool.Creator.FormatName(charStream, tableWriter.Columns[columnIndex].Name);
            charStream.Write('=');
        }
        /// <summary>
        /// 写入条件数据列名称
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteConcatCondition(CharStream charStream, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            if (!isFirst) charStream.SimpleWrite(" and ");
            tableWriter.ConnectionPool.Creator.FormatName(charStream, tableWriter.Columns[columnIndex].Name);
            charStream.Write('=');
        }
        /// <summary>
        /// 写入条件数据列名称
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteConcatCondition(CharStream charStream, TableWriter tableWriter, int columnIndex)
        {
            charStream.SimpleWrite(" and ");
            tableWriter.ConnectionPool.Creator.FormatName(charStream, tableWriter.Columns[columnIndex].Name);
            charStream.Write('=');
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static void CloseConnection(DbConnection connection)
        {
            if (connection != null)
            {
                using (connection) connection.Close();
            }
        }
#if DotNet45 || NetStandard2
        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static Task CloseConnectionAsync(DbConnection connection)
        {
            using (connection) connection.Close();
            return AutoCSer.Common.CompletedTask;
        }
#else
        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static async Task CloseConnectionAsync(DbConnection connection)
        {
            await using (connection) await connection.CloseAsync();
        }
#endif

        /// <summary>
        /// 常量转换处理集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, KeyValue<MethodInfo, bool>> constantConvertMethods;
        /// <summary>
        /// 常量转换处理集合 访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock constantConvertMethodLock;
        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="readerDataType"></param>
        /// <param name="attribute"></param>
        /// <param name="isObjectToString"></param>
        /// <returns></returns>
        internal static KeyValue<MethodInfo, bool> GetConstantConvertMethod(Type type, ReaderDataTypeEnum readerDataType, MemberAttribute attribute, out bool isObjectToString)
        {
            isObjectToString = false;
            switch (readerDataType)
            {
                case ReaderDataTypeEnum.DateTime:
                    DateTimeAttribute dateTimeAttribute = attribute as DateTimeAttribute;
                    if (type == typeof(DateTime))
                    {
                        if (dateTimeAttribute != null)
                        {
                            switch (dateTimeAttribute.Type)
                            {
                                case DateTimeTypeEnum.DateTime2: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime>)convertDateTime2).Method, true);
                                case DateTimeTypeEnum.SmallDateTime: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime>)convertSmallDateTime).Method, true);
                                case DateTimeTypeEnum.Date: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime>)convertDate).Method, true);
                            }
                        }
                        return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime>)convertDateTime).Method, true);
                    }
                    else if (type == typeof(DateTime?))
                    {
                        if (dateTimeAttribute != null)
                        {
                            switch (dateTimeAttribute.Type)
                            {
                                case DateTimeTypeEnum.DateTime2: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime?>)convertDateTime2).Method, true);
                                case DateTimeTypeEnum.SmallDateTime: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime?>)convertSmallDateTime).Method, true);
                                case DateTimeTypeEnum.Date: return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime?>)convertDate).Method, true);
                            }
                        }
                        return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTime?>)convertDateTime).Method, true);
                    }
                    break;
                case ReaderDataTypeEnum.Json: type = typeof(string); break;
            }
            KeyValue<MethodInfo, bool> value;
            if (constantConvertMethods.TryGetValue(type, out value)) return value;
            Type nullableType = type.getNullableType() ?? type;
            if (nullableType.IsEnum)
            {
                AutoCSer.ORM.Metadata.EnumGenericType genericType = AutoCSer.ORM.Metadata.EnumGenericType.Get(nullableType);
                constantConvertMethodLock.EnterYield();
                try
                {
                    if (!constantConvertMethods.TryGetValue(type, out value))
                    {
                        constantConvertMethods.Add(type, value = new KeyValue<MethodInfo, bool>(object.ReferenceEquals(nullableType, type) ? genericType.GetConstantConvertMethod() : genericType.GetNullableConstantConvertMethod(), false));
                    }
                }
                finally { constantConvertMethodLock.Exit(); }
                return value;
            }
            isObjectToString = true;
            return new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, object>)ConvertToString).Method, true);
        }
        static ConnectionCreator()
        {
            constantConvertMethods = DictionaryCreator.CreateHashObject<System.Type, KeyValue<MethodInfo, bool>>();
            constantConvertMethods.Add(typeof(bool), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, bool>)Convert).Method, true));
            constantConvertMethods.Add(typeof(bool?), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, bool?>)Convert).Method, true));
            constantConvertMethods.Add(typeof(byte), new KeyValue<MethodInfo, bool>(((Action<CharStream, byte>)Convert).Method, false));
            constantConvertMethods.Add(typeof(byte?), new KeyValue<MethodInfo, bool>(((Action<CharStream, byte?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(sbyte), new KeyValue<MethodInfo, bool>(((Action<CharStream, sbyte>)Convert).Method, false));
            constantConvertMethods.Add(typeof(sbyte?), new KeyValue<MethodInfo, bool>(((Action<CharStream, sbyte?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(short), new KeyValue<MethodInfo, bool>(((Action<CharStream, short>)Convert).Method, false));
            constantConvertMethods.Add(typeof(short?), new KeyValue<MethodInfo, bool>(((Action<CharStream, short?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(ushort), new KeyValue<MethodInfo, bool>(((Action<CharStream, ushort>)Convert).Method, false));
            constantConvertMethods.Add(typeof(ushort?), new KeyValue<MethodInfo, bool>(((Action<CharStream, ushort?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(int), new KeyValue<MethodInfo, bool>(((Action<CharStream, int>)Convert).Method, false));
            constantConvertMethods.Add(typeof(int?), new KeyValue<MethodInfo, bool>(((Action<CharStream, int?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(uint), new KeyValue<MethodInfo, bool>(((Action<CharStream, uint>)Convert).Method, false));
            constantConvertMethods.Add(typeof(uint?), new KeyValue<MethodInfo, bool>(((Action<CharStream, uint?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(long), new KeyValue<MethodInfo, bool>(((Action<CharStream, long>)Convert).Method, false));
            constantConvertMethods.Add(typeof(long?), new KeyValue<MethodInfo, bool>(((Action<CharStream, long?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(ulong), new KeyValue<MethodInfo, bool>(((Action<CharStream, ulong>)Convert).Method, false));
            constantConvertMethods.Add(typeof(ulong?), new KeyValue<MethodInfo, bool>(((Action<CharStream, ulong?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(float), new KeyValue<MethodInfo, bool>(((Action<CharStream, float>)Convert).Method, false));
            constantConvertMethods.Add(typeof(float?), new KeyValue<MethodInfo, bool>(((Action<CharStream, float?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(double), new KeyValue<MethodInfo, bool>(((Action<CharStream, double>)Convert).Method, false));
            constantConvertMethods.Add(typeof(double?), new KeyValue<MethodInfo, bool>(((Action<CharStream, double?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(decimal), new KeyValue<MethodInfo, bool>(((Action<CharStream, decimal>)Convert).Method, false));
            constantConvertMethods.Add(typeof(decimal?), new KeyValue<MethodInfo, bool>(((Action<CharStream, decimal?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(TimeSpan), new KeyValue<MethodInfo, bool>(((Action<CharStream, TimeSpan>)Convert).Method, false));
            constantConvertMethods.Add(typeof(TimeSpan?), new KeyValue<MethodInfo, bool>(((Action<CharStream, TimeSpan?>)Convert).Method, false));
            constantConvertMethods.Add(typeof(DateTimeOffset), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTimeOffset>)Convert).Method, true));
            constantConvertMethods.Add(typeof(DateTimeOffset?), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, DateTimeOffset?>)Convert).Method, true));
            constantConvertMethods.Add(typeof(Guid), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, Guid>)Convert).Method, true));
            constantConvertMethods.Add(typeof(Guid?), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, Guid?>)Convert).Method, true));
            constantConvertMethods.Add(typeof(string), new KeyValue<MethodInfo, bool>(((Action<ConnectionCreator, CharStream, string>)Convert).Method, true));
        }
    }
}
