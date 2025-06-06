﻿using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 读取数据模型对象委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class ModelReader<T> where T : class
    {
        /// <summary>
        /// 读取数据模型对象委托
        /// </summary>
        internal static readonly Action<DbDataReader, T, int[]> Reader;
        /// <summary>
        /// 数据列名称集合
        /// </summary>
        private static readonly Dictionary<string, int> columnIndexs;
        /// <summary>
        /// 数据列索引集合 临时缓存
        /// </summary>
#if NetStandard21
        private static int[]? columnIndexCache;
#else
        private static int[] columnIndexCache;
#endif
        /// <summary>
        /// 获取数据列索引集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static unsafe int[] GetColumnIndexCache(DbDataReader reader)
        {
            if (columnIndexs.Count != 0)
            {
                int columnIndex;
                var indexs = Interlocked.Exchange(ref columnIndexCache, null);
                if (indexs == null) indexs = new int[(columnIndexs.Count + 1) & (int.MaxValue - 1)];
                fixed (int* indexFixed = indexs)
                {
                    bool isColumn = false;
                    for (int index = reader.FieldCount; index != 0;)
                    {
                        if (columnIndexs.TryGetValue(reader.GetName(--index), out columnIndex))
                        {
                            if (!isColumn)
                            {
                                isColumn = true;
                                AutoCSer.Common.Fill((ulong*)indexFixed, indexs.Length >> 1, ulong.MaxValue);
                            }
                            indexFixed[columnIndex] = index;
                        }
                    }
                    if (isColumn) return indexs;
                    Interlocked.Exchange(ref columnIndexCache, indexs);
                }
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 释放数据列索引集合
        /// </summary>
        /// <param name="indexs"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void FreeColumnIndexCache(int[] indexs)
        {
            if(indexs.Length != 0) Interlocked.Exchange(ref columnIndexCache, indexs);
        }

        static ModelReader()
        {
            ModelAttribute attribute = TableWriter.DefaultAttribute;
            LeftArray<Member> members = Member.Get(MemberIndexGroup.GetFields(typeof(T), attribute.MemberFilters), MemberIndexGroup.GetProperties(typeof(T), attribute.MemberFilters), true);
            int columnIndexCount = members.Length;
            foreach (Member member in members)
            {
                if (member.CustomColumnAttribute != null) columnIndexCount += member.CustomColumnNames.Length;
            }
            Dictionary<string, int> columnIndexs = DictionaryCreator<string>.Create<int>(columnIndexCount);
            foreach (Member member in members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn) columnIndexs.Add(member.MemberIndex.Member.Name, columnIndexs.Count);
                else
                {
                    foreach (CustomColumnName name in member.CustomColumnNames) columnIndexs.Add(name.Name, columnIndexs.Count);
                }
            }

            Type type = typeof(T);
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelReader", null, new Type[] { typeof(DbDataReader), type, typeof(int[]) }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            #region int index = 0, columnIndex;
            LocalBuilder indexLocalBuilder = generator.DeclareLocal(typeof(int)), columnIndexLocalBuilder = generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_0);
            #endregion
            foreach (Member member in members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region columnIndex = columnIndexs[index++];
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldloc_S, indexLocalBuilder);
                    generator.Emit(OpCodes.Dup);
                    generator.Emit(OpCodes.Ldc_I4_1);
                    generator.Emit(OpCodes.Add);
                    generator.Emit(OpCodes.Stloc_S, indexLocalBuilder);
                    generator.Emit(OpCodes.Ldelem_I4);
                    generator.Emit(OpCodes.Stloc_S, columnIndexLocalBuilder);
                    #endregion
                    #region if (columnIndex >= 0)
                    Label nextLabel = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldloc_S, columnIndexLocalBuilder);
                    generator.Emit(OpCodes.Ldc_I4_0);
                    generator.Emit(OpCodes.Blt_S, nextLabel);
                    #endregion
                    #region value.Field = AutoCSer.ORM.Member.Read(reader, columnIndex);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloc_S, columnIndexLocalBuilder);
                    generator.call(member.GetReadObjectMethod());
                    if (member.MemberIndex.IsField) generator.Emit(OpCodes.Stfld, (FieldInfo)member.MemberIndex.Member);
                    else generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                    generator.MarkLabel(nextLabel);
                }
                else if (member.MemberIndex.IsField)
                {
                    #region AutoCSer.ORM.CustomColumn.ModelReader<PropertyType>.Read(reader, ref value.CustomField, columnIndexs, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
                    generator.call(member.StructGenericType.CustomColumnModelReaderDelegate.Method);
                    #endregion
                }
                else
                {
                    #region PropertyType property = default(PropertyType);
                    LocalBuilder propertyLocalBuilder = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Initobj, member.MemberIndex.MemberSystemType);
                    #endregion
                    #region AutoCSer.ORM.CustomColumn.ModelReader<PropertyType>.Read(reader, ref property, columnIndexs, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
                    generator.call(member.StructGenericType.CustomColumnModelReaderDelegate.Method);
                    #endregion
                    #region value.CustomProperty = property;
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldloc, propertyLocalBuilder);
                    generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                }
            }
            generator.Emit(OpCodes.Ret);
            Reader = (Action<DbDataReader, T, int[]>)dynamicMethod.CreateDelegate(typeof(Action<DbDataReader, T, int[]>));
            ModelReader<T>.columnIndexs = columnIndexs;
        }
    }
#if DEBUG && NetStandard21
#pragma warning disable
    internal class ModelReaderIL
    {
        private AutoCSer.ORM.CustomColumn.Date Field;
        private AutoCSer.ORM.CustomColumn.Date Property { get; set; }

        private AutoCSer.ORM.CustomColumn.Date? NullableField;
        private AutoCSer.ORM.CustomColumn.Date? NullableProperty { get; set; }

        private string NullField;
        private string NullProperty { get; set; }

        private ModelReaderIL JsonStringField;
        private ModelReaderIL JsonStringProperty { get; set; }

        private AutoCSer.ORM.CustomColumn.Date CustomField;
        private AutoCSer.ORM.CustomColumn.Date CustomProperty { get; set; }

        public static void Reader(DbDataReader reader, ModelReaderIL value, int[] columnIndexs)
        {
            int index = 0, columnIndex;

            columnIndex = columnIndexs[index++];
            if (columnIndex >= 0) value.Field = read(reader, columnIndex);

            columnIndex = columnIndexs[index++];
            if (columnIndex >= 0) value.Property = read(reader, columnIndex);

            AutoCSer.ORM.CustomColumn.ModelReader<AutoCSer.ORM.CustomColumn.Date>.Read(reader, ref value.CustomField, columnIndexs, ref index);

            AutoCSer.ORM.CustomColumn.Date property = default(AutoCSer.ORM.CustomColumn.Date);
            AutoCSer.ORM.CustomColumn.ModelReader<AutoCSer.ORM.CustomColumn.Date>.Read(reader, ref property, columnIndexs, ref index);
            value.CustomProperty = property;
        }
        private static AutoCSer.ORM.CustomColumn.Date read(DbDataReader reader, int index)
        {
            return (AutoCSer.ORM.CustomColumn.Date)reader.GetDateTime(index);
        }
    }
#endif
}
