using AutoCSer.Extensions;
using AutoCSer.FieldEquals.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

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
        /// 泛型类型元数据
        /// </summary>
        private readonly AutoCSer.Metadata.GenericType genericType;
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
        /// <param name="genericType"></param>
        /// <param name="isMemberMap"></param>
        public MemberDynamicMethod(AutoCSer.Metadata.GenericType genericType, bool isMemberMap)
        {
            this.genericType = genericType;
            this.type = genericType.CurrentType;
            if (this.isMemberMap = isMemberMap) dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "MemberMapEquals", typeof(bool), new Type[] { type, type, genericType.GetMemberMapType }, type, true);
            else dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "FieldEquals", typeof(bool), new Type[] { type, type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            if (isValueType = type.IsValueType) return;

            Label next = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.call(objectReferenceEqualsMethod);
            generator.Emit(OpCodes.Brfalse_S, next);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.ret();
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
            generator.ret();
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
            generator.memberMapObjectIsMember(OpCodes.Ldarg_2, memberIndex, genericType);
            generator.Emit(OpCodes.Brfalse_S, next);
            Push(field);
            generator.MarkLabel(next);
        }
        /// <summary>
        /// 基类调用
        /// </summary>
        public void Base()
        {
            if (!isValueType)
            {
                Type baseType = type.BaseType.notNull();
                if (baseType != typeof(object))
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.call(GenericType.Get(baseType).CallEqualsDelegate.Method);
                    generator.ret();
                    return;
                }
            }
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.ret();
        }
        /// <summary>
        /// 创建委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>委托</returns>
        public Delegate Create(Type type)
        {
            if (isMemberMap)
            {
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.ret();
            }
            return dynamicMethod.CreateDelegate(type);
        }
    }
}
