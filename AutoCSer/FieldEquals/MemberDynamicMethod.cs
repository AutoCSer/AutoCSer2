using AutoCSer.Extensions;
using AutoCSer.FieldEquals.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberDynamicMethod
    {
        /// <summary>
        /// 引用比较函数信息
        /// </summary>
        private static readonly MethodInfo objectReferenceEqualsMethod = ((Func<object, object, bool>)object.ReferenceEquals).Method;
        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 数据类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 
        /// </summary>
        private bool isMemberMap;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isMemberMap"></param>
        public MemberDynamicMethod(Type type, bool isMemberMap)
        {
            this.type = type;
            if (this.isMemberMap = isMemberMap) dynamicMethod = new DynamicMethod("MemberMapEquals", typeof(bool), new Type[] { type, type, typeof(AutoCSer.Metadata.MemberMap) }, type, true);
            else dynamicMethod = new DynamicMethod("FieldEquals", typeof(bool), new Type[] { type, type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            if (isValueType = type.IsValueType) return;

            Label next = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.call(objectReferenceEqualsMethod);
            generator.Emit(OpCodes.Brfalse_S, next);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);
            generator.MarkLabel(next);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldInfo field)
        {
            Label next = generator.DefineLabel();
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 0);
            else generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
            else generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, field);
            generator.call(GenericType.Get(field.FieldType).CallEqualsDelegate.Method);
            generator.Emit(OpCodes.Brtrue_S, next);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);
            generator.MarkLabel(next);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="memberIndex">字段信息</param>
        public void Push(FieldInfo field, int memberIndex)
        {
            Label next = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_2, memberIndex);
            generator.Emit(OpCodes.Brfalse_S, next);
            Push(field);
            generator.MarkLabel(next);
        }
        /// <summary>
        /// 基类调用
        /// </summary>
        public void Base()
        {
            if (!isValueType && (type = type.BaseType) != typeof(object))
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.call(GenericType.Get(type).CallEqualsDelegate.Method);
            }
            else generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 创建委托
        /// </summary>
        /// <returns>委托</returns>
        public Delegate Create<delegateType>()
        {
            if (isMemberMap)
            {
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Ret);
            }
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
