using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.CustomColumn
{
    /// <summary>
    /// 自定义数据列值转数组
    /// </summary>
    /// <typeparam name="T">自定义数据列类型</typeparam>
    internal static class ToArray<T> where T : struct
    {
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        /// <param name="value">Target data</param>
        /// <param name="array">数据列储存数组</param>
        /// <param name="index">当前读取位置</param>
        internal delegate void Writer(T value, object[] array,  ref int index);
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        private static readonly Writer writer;
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Write(T value, object[] array, ref int index)
        {
            writer(value, array, ref index);
        }
        static ToArray()
        {
            Type type = typeof(T);
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLCustomColumnToArray", null, new Type[] { type, typeof(object[]), AutoCSer.ORM.Member.RefIntType }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            foreach (Member member in ModelMetadata<T>.Members)
            {
                if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                {
                    #region array[index] = value.NullField;
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    if (member.MemberIndex.IsField)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarga_S, 0);
                        generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true).notNull());
                    }
                    if(member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
                    else if (member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Stelem_Ref);
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
                else 
                {
                    #region AutoCSer.ORM.CustomColumn.ToArray<AutoCSer.ORM.CustomColumn.Date>.Write(array, value.CustomField, ref index);
                    generator.Emit(OpCodes.Ldarg_1);
                    if (member.MemberIndex.IsField)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarga_S, 0);
                        generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true).notNull());
                    }
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.call(member.StructGenericType.CustomColumnToArrayDelegate.Method);
                    #endregion
                }
            }
            generator.ret();
            writer = (Writer)dynamicMethod.CreateDelegate(typeof(Writer));
        }
    }
#if DEBUG && NetStandard21
#pragma warning disable
    internal struct ToArrayIL
    {
        private int NullField;
        private string NullProperty { get; set; }

        private AutoCSer.ORM.CustomColumn.Date JsonStringField;
        private AutoCSer.ORM.CustomColumn.Date JsonStringProperty { get; set; }

        private AutoCSer.ORM.CustomColumn.Date CustomField;
        private AutoCSer.ORM.CustomColumn.Date CustomProperty { get; set; }

        public static void ToArray(ToArrayIL value, object[] array, ref int index)
        {
            array[index] = value.NullField;
            ++index;

            array[index] = value.NullProperty;
            ++index;

            array[index] = AutoCSer.ORM.Member.JsonSerialize(value.JsonStringField);
            ++index;

            array[index] = AutoCSer.ORM.Member.JsonSerialize(value.JsonStringProperty);
            ++index;

            AutoCSer.ORM.CustomColumn.ToArray<AutoCSer.ORM.CustomColumn.Date>.Write(value.CustomField, array, ref index);

            AutoCSer.ORM.CustomColumn.ToArray<AutoCSer.ORM.CustomColumn.Date>.Write(value.CustomProperty, array, ref index);
        }
    }
#endif
}
