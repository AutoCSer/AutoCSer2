using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Reflection.Emit;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Json
{
    /// <summary>
    /// 序列化动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberDynamicMethod
    {
        /// <summary>
        /// 获取字符串输出缓冲区属性方法信息
        /// </summary>
        private static readonly MethodInfo getCharStreamMethod = ((Func<JsonSerializer, CharStream>)JsonSerializer.GetCharStream).Method;

        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 是否第一个字段
        /// </summary>
        private byte isFirstMember;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public SerializeMemberDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "JsonSerializer", null, new Type[] { typeof(JsonSerializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_0);
            generator.call(getCharStreamMethod);
            generator.Emit(OpCodes.Stloc_0);

            isFirstMember = 1;
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name">成员名称</param>
        private void push(string name)
        {
            if (isFirstMember == 0) WriteName(generator, OpCodes.Ldloc_0, name, true);
            else
            {
                WriteName(generator, OpCodes.Ldloc_0, name, false);
                isFirstMember = 0;
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="serializeMethod"></param>
        public void Push(FieldIndex field, MethodInfo serializeMethod)
        {
            push(field.AnonymousName);
            if (field.Member.FieldType.IsValueType)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(serializeMethod);
            }
            else
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
                checkNull(field.Member.FieldType, serializeMethod);
            }
        }
        /// <summary>
        /// null 值检查
        /// </summary>
        /// <param name="memberType"></param>
        /// <param name="serializeMethod"></param>
        private void checkNull(Type memberType, MethodInfo serializeMethod)
        {
            LocalBuilder memberLocalBuilder = generator.DeclareLocal(memberType);
            generator.Emit(OpCodes.Stloc, memberLocalBuilder);

            Label nullLabel = generator.DefineLabel(), endLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.Emit(OpCodes.Brfalse_S, nullLabel);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.call(serializeMethod);
            generator.Emit(OpCodes.Br_S, endLabel);

            generator.MarkLabel(nullLabel);
            generator.Emit(OpCodes.Ldarg_0);
            generator.call(writeJsonNullMethod);
            generator.MarkLabel(endLabel);
        }
        /// <summary>
        /// 输出 null 值
        /// </summary>
        private static readonly MethodInfo writeJsonNullMethod = ((Action<JsonSerializer>)JsonSerializer.WriteJsonNull).Method;
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="serializeMethod"></param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MethodInfo serializeMethod)
        {
            push(property.Member.Name);
            if (property.Member.PropertyType.IsValueType)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.call(serializeMethod);
            }
            else
            {
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                checkNull(property.Member.PropertyType, serializeMethod);
            }
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

        /// <summary>
        /// 写入名称
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="isNext"></param>
        internal static void WriteName(ILGenerator generator, OpCode target, string name, bool isNext)
        {
            StringWriter stringWriter = new StringWriter(generator, target, (name.Length << 1) + (isNext ? 8 : 6));
            if (isNext) stringWriter.Write(',');
            stringWriter.Write('"');
            stringWriter.Write(name);
            stringWriter.Write('"');
            stringWriter.Write(':');
            stringWriter.WriteEnd();
        }
    }
}
