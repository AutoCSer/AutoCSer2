using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.CustomColumn
{
    /// <summary>
    /// 自定义数据列委托
    /// </summary>
    /// <typeparam name="T">自定义数据列类型</typeparam>
    internal static class TableModel<T> where T : struct
    {
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        /// <param name="reader">字段读取器物理存储</param>
        /// <param name="value">Target data</param>
        /// <param name="index">当前读取位置</param>
        internal delegate void ReaderDelegate(DbDataReader reader, ref T value, ref int index);
        /// <summary>
        /// 默认数据列设置
        /// </summary>
        private static readonly ReaderDelegate read;
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Read(DbDataReader reader, ref T value, ref int index)
        {
            TableModel<T>.read(reader, ref value, ref index);
        }
        /// <summary>
        /// 写入表格模型数据委托
        /// </summary>
        private static readonly Func<CharStream, T, TableWriter, int, int> insert;
        /// <summary>
        /// 写入表格模型数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int Insert(CharStream charStream, T value, TableWriter tableWriter, int columnIndex)
        {
            return insert(charStream, value, tableWriter, columnIndex);
        }
        /// <summary>
        /// 写入更新数据
        /// </summary>
        private static readonly Func<CharStream, T, TableWriter, int, bool, int> update;
        /// <summary>
        /// 写入更新数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int Update(CharStream charStream, T value, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            return update(charStream, value, tableWriter, columnIndex, isFirst);
        }
        /// <summary>
        /// 写入条件数据
        /// </summary>
        private static readonly Func<CharStream, T, TableWriter, int, bool, int> concatCondition;
        /// <summary>
        /// 写入条件数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <param name="tableWriter"></param>
        /// <param name="columnIndex"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int ConcatCondition(CharStream charStream, T value, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            return concatCondition(charStream, value, tableWriter, columnIndex, isFirst);
        }

        /// <summary>
        /// 写入列数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="member"></param>
        /// <param name="connectionCreatorLocal"></param>
        private static void write(ILGenerator generator, Member member, LocalBuilder connectionCreatorLocal)
        {
            #region ConnectionCreator.Convert(connectionCreator, charStream, value.NullField);
            bool isObjectToString;
            KeyValue<MethodInfo, bool> method = member.GetConstantConvertMethod(out isObjectToString);
            if (method.Value) generator.Emit(OpCodes.Ldloc_S, connectionCreatorLocal);
            generator.Emit(OpCodes.Ldarg_0);
            var verifyMethod = member.VerifyMethod;
            if (verifyMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_3);
            }
            insertVerifyInterface(generator, member);
            if (member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
            else if (isObjectToString && member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
            if (verifyMethod != null) generator.call(verifyMethod);
            generator.call(method.Key);
            #endregion
            #region ++columnIndex;
            generator.Emit(OpCodes.Ldarg_3);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Add);
            generator.Emit(OpCodes.Starg_S, 3);
            #endregion
        }
        /// <summary>
        /// 写入表格模型数据验证接口调用
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="member"></param>
        private static void insertVerifyInterface(ILGenerator generator, Member member)
        {
            var verifyInterfaceMethod = member.VerifyInterfaceMethod;
            if (member.MemberIndex.IsField)
            {
                if (verifyInterfaceMethod == null || !member.MemberIndex.MemberSystemType.IsValueType)
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                }
                else
                {
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
                }
            }
            else
            {
                generator.Emit(OpCodes.Ldarga_S, 1);
                generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true).notNull());
                if (verifyInterfaceMethod != null && member.MemberIndex.MemberSystemType.IsValueType)
                {
                    LocalBuilder verifyLocal = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Stloc, verifyLocal);
                    generator.Emit(OpCodes.Ldloca, verifyLocal);
                }
            }
            if (verifyInterfaceMethod != null) generator.call(verifyInterfaceMethod);
        }
        static TableModel()
        {
            Type type = typeof(T);
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLTableCustomColumnRead", null, new Type[] { typeof(DbDataReader), type.MakeByRefType(), AutoCSer.ORM.Member.RefIntType }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region value.Field = AutoCSer.ORM.Member.Read(reader, index);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.call(member.GetReadMethod());
                    if (member.MemberIndex.IsField) generator.Emit(OpCodes.Stfld, (FieldInfo)member.MemberIndex.Member);
                    else generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                    #region ++index;
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldc_I4_1);
                    generator.Emit(OpCodes.Add);
                    generator.Emit(OpCodes.Stind_I4);
                    #endregion
                }
                else if (member.MemberIndex.IsField)
                {
                    #region AutoCSer.ORM.CustomColumn.Reader<PropertyType>.Read(reader, ref value.CustomField, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.call(member.StructGenericType.CustomColumnTableReadDelegate.Method);
                    #endregion
                }
                else
                {
                    #region PropertyType property = default(PropertyType);
                    LocalBuilder propertyLocalBuilder = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Initobj, member.MemberIndex.MemberSystemType);
                    #endregion
                    #region AutoCSer.ORM.CustomColumn.Reader<PropertyType>.Read(reader, ref property, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.call(member.StructGenericType.CustomColumnTableReadDelegate.Method);
                    #endregion
                    #region value.CustomProperty = property;
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldloc, propertyLocalBuilder);
                    generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                }
            }
            generator.ret();
            read = (ReaderDelegate)dynamicMethod.CreateDelegate(typeof(ReaderDelegate));

            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLTableCustomColumnInsert", typeof(int), new Type[] { typeof(CharStream), type, typeof(TableWriter), typeof(int) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
            LocalBuilder connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
            generator.Emit(OpCodes.Ldarg_2);
            generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
            generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
            #endregion
            bool isFirst = true;
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    if (isFirst)
                    {
                        #region if (columnIndex != 0) ConnectionCreator.WriteConcat(charStream);
                        Label noFirstLabel = generator.DefineLabel();
                        generator.Emit(OpCodes.Ldarg_3);
                        generator.Emit(OpCodes.Brfalse_S, noFirstLabel);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.call(((Action<CharStream>)ConnectionCreator.WriteConcat).Method);
                        generator.MarkLabel(noFirstLabel);
                        #endregion
                    }
                    else
                    {
                        #region ConnectionCreator.WriteConcat(charStream);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.call(((Action<CharStream>)ConnectionCreator.WriteConcat).Method);
                        #endregion
                    }
                    write(generator, member, connectionCreatorLocal);
                }
                else
                {
                    #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<AutoCSer.ORM.CustomColumn.Date>.Insert(charStream, value.CustomField, tableWriter, columnIndex);
                    generator.Emit(OpCodes.Ldarg_0);
                    insertVerifyInterface(generator, member);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.call(member.StructGenericType.CustomColumnTableInsertDelegate.Method);
                    generator.Emit(OpCodes.Starg_S, 3);
                    #endregion
                }
                isFirst = false;
            }
            generator.Emit(OpCodes.Ldarg_3);
            generator.ret();
            insert = (Func<CharStream, T, TableWriter, int, int>)dynamicMethod.CreateDelegate(typeof(Func<CharStream, T, TableWriter, int, int>));

            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLTableCustomColumnUpdate", typeof(int), new Type[] { typeof(CharStream), type, typeof(TableWriter), typeof(int), typeof(bool) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
            connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
            generator.Emit(OpCodes.Ldarg_2);
            generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
            generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
            #endregion
            isFirst = true;
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region ConnectionCreator.WriteConcat(charStream, tableWriter, columnIndex, isFirst);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    if (isFirst)
                    {
                        generator.ldarg(4);
                        generator.call(((Action<CharStream, TableWriter, int, bool>)ConnectionCreator.WriteConcat).Method);
                    }
                    else generator.call(((Action<CharStream, TableWriter, int>)ConnectionCreator.WriteConcat).Method);
                    #endregion
                    write(generator, member, connectionCreatorLocal);
                }
                else
                {
                    #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<AutoCSer.ORM.CustomColumn.Date>.Update(charStream, value.CustomField, tableWriter, columnIndex, isFirst);
                    generator.Emit(OpCodes.Ldarg_0);
                    insertVerifyInterface(generator, member);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    if (isFirst) generator.ldarg(4);
                    else generator.Emit(OpCodes.Ldc_I4_0);
                    generator.call(member.StructGenericType.CustomColumnTableUpdateDelegate.Method);
                    generator.Emit(OpCodes.Starg_S, 3);
                    #endregion
                }
                isFirst = false;
            }
            generator.Emit(OpCodes.Ldarg_3);
            generator.ret();
            update = (Func<CharStream, T, TableWriter, int, bool, int>)dynamicMethod.CreateDelegate(typeof(Func<CharStream, T, TableWriter, int, bool, int>));

            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLTableCustomColumnConcatCondition", typeof(int), new Type[] { typeof(CharStream), type, typeof(TableWriter), typeof(int), typeof(bool) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
            connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
            generator.Emit(OpCodes.Ldarg_2);
            generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
            generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
            #endregion
            bool isObjectToString;
            isFirst = true;
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region ConnectionCreator.WriteConcatCondition(charStream, tableWriter, columnIndex, isFirst);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    if (isFirst)
                    {
                        generator.ldarg(4);
                        generator.call(((Action<CharStream, TableWriter, int, bool>)ConnectionCreator.WriteConcatCondition).Method);
                    }
                    else generator.call(((Action<CharStream, TableWriter, int>)ConnectionCreator.WriteConcatCondition).Method);
                    #endregion
                    #region ConnectionCreator.Convert(connectionCreator, charStream, value.NullField);
                    KeyValue<MethodInfo, bool> method = member.GetConstantConvertMethod(out isObjectToString);
                    if (method.Value) generator.Emit(OpCodes.Ldloc_S, connectionCreatorLocal);
                    generator.Emit(OpCodes.Ldarg_0);
                    if (member.MemberIndex.IsField)
                    {
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarga_S, 1);
                        generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true).notNull());
                    }
                    if (member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
                    else if (isObjectToString && member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
                    generator.call(method.Key);
                    #endregion
                    #region ++columnIndex;
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.Emit(OpCodes.Ldc_I4_1);
                    generator.Emit(OpCodes.Add);
                    generator.Emit(OpCodes.Starg_S, 3);
                    #endregion
                }
                else
                {
                    #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<AutoCSer.ORM.CustomColumn.Date>.ConcatCondition(charStream, value.CustomField, tableWriter, columnIndex, isFirst);
                    generator.Emit(OpCodes.Ldarg_0);
                    if (member.MemberIndex.IsField)
                    {
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarga_S, 1);
                        generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true).notNull());
                    }
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    if (isFirst) generator.ldarg(4);
                    else generator.Emit(OpCodes.Ldc_I4_0);
                    generator.call(member.StructGenericType.CustomColumnTableConcatConditionDelegate.Method);
                    generator.Emit(OpCodes.Starg_S, 3);
                    #endregion
                }
                isFirst = false;
            }
            generator.Emit(OpCodes.Ldarg_3);
            generator.ret();
            concatCondition = (Func<CharStream, T, TableWriter, int, bool, int>)dynamicMethod.CreateDelegate(typeof(Func<CharStream, T, TableWriter, int, bool, int>));
        }
    }
#if DEBUG && NetStandard21
#pragma warning disable
    internal struct TableReaderIL
    {
        private readonly decimal Field;
        private decimal Property { get; set; }

        private readonly decimal? NullField;
        private decimal? NullProperty { get; set; }

        private VerifyData VerifyDataField;
        private VerifyData VerifyDataProperty { get; set; }

        private readonly VerifyValue VerifyValueField;
        private VerifyValue VerifyValueProperty { get; set; }

        private VerifyData CustomVerifyDataField;
        private VerifyData CustomVerifyDataProperty { get; set; }

        private struct VerifyData
        {
            public VerifyData Verify() { return this; }
        }
        private sealed class VerifyValue
        {
            public VerifyValue Verify() { return this; }
        }

        public static int ConcatCondition(CharStream charStream, TableReaderIL value, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            ConnectionCreator.WriteConcatCondition(charStream, tableWriter, columnIndex, isFirst);
            ConnectionCreator.Convert(charStream, value.Field);
            ++columnIndex;

            ConnectionCreator.WriteConcatCondition(charStream, tableWriter, columnIndex);
            ConnectionCreator.Convert(charStream, value.Property);
            ++columnIndex;


            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.ConcatCondition(charStream, value.CustomVerifyDataField, tableWriter, columnIndex, isFirst);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.ConcatCondition(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex, false);

            return columnIndex;
        }
        public static int Update(CharStream charStream, TableReaderIL value, TableWriter tableWriter, int columnIndex, bool isFirst)
        {
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            ConnectionCreator.WriteConcat(charStream, tableWriter, columnIndex, isFirst);
            ConnectionCreator.Convert(charStream, value.Field);
            ++columnIndex;

            ConnectionCreator.WriteConcat(charStream, tableWriter, columnIndex);
            ConnectionCreator.Convert(charStream, value.Property);
            ++columnIndex;

            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Field));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Property));

            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullField));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty.Verify()));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty.Verify()));

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataField, tableWriter, columnIndex, isFirst);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex, false);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataField.Verify(), tableWriter, columnIndex, false);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataProperty.Verify(), tableWriter, columnIndex, false);

            return columnIndex;
        }
        public static int Insert(CharStream charStream, TableReaderIL value, TableWriter tableWriter, int columnIndex)
        {
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            if (columnIndex != 0) ConnectionCreator.WriteConcat(charStream);
            ConnectionCreator.Convert(charStream, value.Field);
            ConnectionCreator.Convert(charStream, value.Property);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Field));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Property));
            ++columnIndex;

            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullField));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty.Verify()));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty.Verify()));

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataField, tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataField.Verify(), tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataProperty.Verify(), tableWriter, columnIndex);

            return columnIndex;
        }
        public static void Reader(DbDataReader reader, ref TableReaderIL value, ref int index)
        {
            //value.NullField = AutoCSer.ORM.Member.ReadString(reader, index);
            //++index;

            //value.NullProperty = AutoCSer.ORM.Member.ReadString(reader, index);
            //++index;

            //value.JsonStringField = AutoCSer.ORM.Member.ReadJson<VerifyData>(reader, index);
            //++index;

            //value.JsonStringProperty = AutoCSer.ORM.Member.ReadJson<VerifyValue>(reader, index);
            //++index;

            AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Read(reader, ref value.CustomVerifyDataField, ref index);

            VerifyData property = default(VerifyData);
            AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Read(reader, ref property, ref index);
            value.CustomVerifyDataProperty = property;
        }
    }
#endif
}
