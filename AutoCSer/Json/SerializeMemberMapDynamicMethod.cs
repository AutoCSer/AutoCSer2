using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Json
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberMapDynamicMethod
    {
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        private readonly GenericType genericType;
        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="genericType"></param>
        public SerializeMemberMapDynamicMethod(GenericType genericType)
        {
            this.genericType = genericType;
            Type type = genericType.CurrentType;
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "JsonMemberMapSerializer", null, new Type[] { genericType.GetMemberMapType, typeof(JsonSerializer), type, typeof(CharStream) }, type, true);
            generator = dynamicMethod.GetILGenerator();

            generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <param name="memberIndex"></param>
        /// <param name="end"></param>
        private void push(string name, int memberIndex, Label end)
        {
            Label next = generator.DefineLabel(), value = generator.DefineLabel();
            generator.memberMapObjectIsMember(OpCodes.Ldarg_0, memberIndex, genericType);
            generator.Emit(OpCodes.Brfalse, end);

            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(name.Length > 40 ? OpCodes.Brtrue : OpCodes.Brtrue_S, next);

            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Stloc_0);
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, false);
            generator.Emit(OpCodes.Br_S, value);

            generator.MarkLabel(next);
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, true);

            generator.MarkLabel(value);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="serializeMethod"></param>
        public void Push(FieldIndex field, MethodInfo serializeMethod)
        {
            Label end = generator.DefineLabel();
            push(field.AnonymousName, field.MemberIndex, end);
            if (field.Member.FieldType.IsValueType)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(serializeMethod);
            }
            else
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.Member);
                checkNull(field.Member.FieldType, end, serializeMethod);
            }
            generator.MarkLabel(end);
        }
        /// <summary>
        /// null 值检查
        /// </summary>
        /// <param name="memberType"></param>
        /// <param name="endLabel"></param>
        /// <param name="serializeMethod"></param>
        private void checkNull(Type memberType, Label endLabel, MethodInfo serializeMethod)
        {
            LocalBuilder memberLocalBuilder = generator.DeclareLocal(memberType);
            generator.Emit(OpCodes.Stloc, memberLocalBuilder);

            Label nullLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.Emit(OpCodes.Brfalse_S, nullLabel);

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.call(serializeMethod);
            generator.Emit(OpCodes.Br_S, endLabel);

            generator.MarkLabel(nullLabel);
            generator.Emit(OpCodes.Ldarg_3);
            generator.call(writeJsonNullMethod);
        }
        /// <summary>
        /// 输出 null 值
        /// </summary>
        private static readonly MethodInfo writeJsonNullMethod = ((Action<CharStream>)CharStream.WriteJsonNull).Method;
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="serializeMethod"></param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MethodInfo serializeMethod)
        {
            Label end = generator.DefineLabel();
            push(property.Member.Name, property.MemberIndex, end);
            if (property.Member.PropertyType.IsValueType)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
                generator.call(serializeMethod);
            }
            else
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
                checkNull(property.Member.PropertyType, end, serializeMethod);
            }
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>成员转换委托</returns>
        public Delegate Create(Type type)
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(type);
        }
    }
}
