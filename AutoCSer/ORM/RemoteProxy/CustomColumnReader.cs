using AutoCSer.Extensions;
using AutoCSer.ORM.CustomColumn;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 读取自定义数据列
    /// </summary>
    /// <typeparam name="T">自定义数据列类型</typeparam>
    internal static class CustomColumnReader<T> where T : struct
    {
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="value">目标数据</param>
        /// <param name="columnIndexs">数据列索引集合</param>
        /// <param name="index">当前读取位置</param>
        internal delegate void ReaderDelegate(DataValue[] row, ref T value, int[] columnIndexs, ref int index);
        /// <summary>
        /// 默认数据列设置
        /// </summary>
        private static readonly ReaderDelegate reader;
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <param name="columnIndexs"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Read(DataValue[] row, ref T value, int[] columnIndexs, ref int index)
        {
            reader(row, ref value, columnIndexs, ref index);
        }
        static CustomColumnReader()
        {
            Type type = typeof(T);
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelCustomColumnRemoteProxyReader", null, new Type[] { typeof(DataValue[]), type.MakeByRefType(), typeof(int[]), AutoCSer.ORM.Member.RefIntType }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            LocalBuilder columnIndexLocalBuilder = generator.DeclareLocal(typeof(int));
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region columnIndex = columnIndexs[index];
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldelem_I4);
                    generator.Emit(OpCodes.Stloc_S, columnIndexLocalBuilder);
                    #endregion
                    #region if (columnIndex >= 0)
                    Label nextLabel = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldloc_S, columnIndexLocalBuilder);
                    generator.Emit(OpCodes.Ldc_I4_0);
                    generator.Emit(OpCodes.Blt_S, nextLabel);
                    #endregion
                    #region value.Field = AutoCSer.ORM.Member.Read(reader, index);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloc_S, columnIndexLocalBuilder);
                    generator.call(member.GetRemoteProxyReadMethod());
                    if (member.MemberIndex.IsField) generator.Emit(OpCodes.Stfld, (FieldInfo)member.MemberIndex.Member);
                    else generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                    #region ++index
                    generator.MarkLabel(nextLabel);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldc_I4_1);
                    generator.Emit(OpCodes.Add);
                    generator.Emit(OpCodes.Stind_I4);
                    #endregion
                }
                else if (member.MemberIndex.IsField)
                {
                    #region AutoCSer.ORM.CustomColumn.Reader<PropertyType>.Read(reader, ref value.CustomField, columnIndexs, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.call(member.StructGenericType.CustomColumnModelRemoteProxyReaderDelegate.Method);
                    #endregion
                }
                else
                {
                    #region PropertyType property = default(PropertyType);
                    LocalBuilder propertyLocalBuilder = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Initobj, member.MemberIndex.MemberSystemType);
                    #endregion
                    #region AutoCSer.ORM.CustomColumn.Reader<PropertyType>.Read(reader, ref property, columnIndexs, ref index);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldarg_3);
                    generator.call(member.StructGenericType.CustomColumnModelRemoteProxyReaderDelegate.Method);
                    #endregion
                    #region value.CustomProperty = property;
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldloc, propertyLocalBuilder);
                    generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true).notNull());
                    #endregion
                }
            }
            generator.Emit(OpCodes.Ret);
            reader = (ReaderDelegate)dynamicMethod.CreateDelegate(typeof(ReaderDelegate));
        }
    }
#if DEBUG && NetStandard21
    internal struct CustomColumnReaderIL
    {
        private string? NullField;
        private string? NullProperty { get; set; }

        private AutoCSer.ORM.CustomColumn.Date JsonStringField;
        private AutoCSer.ORM.CustomColumn.Date JsonStringProperty { get; set; }

        private AutoCSer.ORM.CustomColumn.Date CustomField;
        private AutoCSer.ORM.CustomColumn.Date CustomProperty { get; set; }

        public static void Reader(DataValue[] row, ref CustomColumnReaderIL value, int[] columnIndexs, ref int index)
        {
            int columnIndex = columnIndexs[index];
            if (columnIndex >= 0) value.NullField = AutoCSer.ORM.Member.ReadRemoteProxyString(row, columnIndex);
            ++index;

            columnIndex = columnIndexs[index];
            if (columnIndex >= 0) value.NullProperty = AutoCSer.ORM.Member.ReadRemoteProxyString(row, columnIndex);
            ++index;

            columnIndex = columnIndexs[index];
            if (columnIndex >= 0) value.JsonStringField = AutoCSer.ORM.Member.ReadRemoteProxyJson<AutoCSer.ORM.CustomColumn.Date>(row, columnIndex);
            ++index;

            columnIndex = columnIndexs[index];
            if (columnIndex >= 0) value.JsonStringProperty = AutoCSer.ORM.Member.ReadRemoteProxyJson<AutoCSer.ORM.CustomColumn.Date>(row, columnIndex);
            ++index;

            AutoCSer.ORM.RemoteProxy.CustomColumnReader<AutoCSer.ORM.CustomColumn.Date>.Read(row, ref value.CustomField, columnIndexs, ref index);

            AutoCSer.ORM.CustomColumn.Date property = default(AutoCSer.ORM.CustomColumn.Date);
            AutoCSer.ORM.RemoteProxy.CustomColumnReader<AutoCSer.ORM.CustomColumn.Date>.Read(row, ref property, columnIndexs, ref index);
            value.CustomProperty = property;
        }
    }
#endif
}
