using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Xml
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
        private static readonly MethodInfo getCharStreamMethod = ((Func<XmlSerializer, CharStream>)XmlSerializer.GetCharStream).Method;
        /// <summary>
        /// 设置集合子节点名称函数信息
        /// </summary>
        internal static readonly MethodInfo SetItemNameMethod = ((Action<XmlSerializer, string>)XmlSerializer.SetItemName).Method;

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
        /// <param name="type"></param>
        public SerializeMemberDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "XmlSerializer", null, new Type[] { typeof(XmlSerializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(CharStream));

            generator.Emit(OpCodes.Ldarg_0);
            generator.call(getCharStreamMethod);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        private void nameStart(string name, XmlSerializeMemberAttribute attribute)
        {
            WriteName(generator, OpCodes.Ldloc_0, name, false);
            if (!string.IsNullOrEmpty(attribute?.ItemName))
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.ldstr(attribute.ItemName);
                generator.call(SetItemNameMethod);
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="serializeMethod"></param>
        /// <param name="attribute"></param>
        public void Push(FieldIndex field, MethodInfo serializeMethod, XmlSerializeMemberAttribute attribute)
        {
            Label end = default(Label);
            Delegate isOutputMethod = Common.GetIsOutputDelegate(field.Member.FieldType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(isOutputMethod.Method);
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
            }
            string name = field.AnonymousName;
            nameStart(name, attribute);
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
            WriteName(generator, OpCodes.Ldloc_0, name, true);
            if (isOutputMethod != null) generator.MarkLabel(end);
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

            Label endLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.Emit(OpCodes.Brfalse_S, endLabel);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloc, memberLocalBuilder);
            generator.call(serializeMethod);
            generator.MarkLabel(endLabel);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        /// <param name="serializeMethod"></param>
        /// <param name="attribute"></param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MethodInfo serializeMethod, XmlSerializeMemberAttribute attribute)
        {
            Label end = default(Label);
            Delegate isOutputMethod = Common.GetIsOutputDelegate(property.Member.PropertyType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
                else generator.Emit(OpCodes.Ldarg_1);
                generator.call(propertyMethod);
                generator.call(isOutputMethod.Method);
                generator.Emit(OpCodes.Brfalse, end = generator.DefineLabel());
            }
            string name = property.Member.Name;
            nameStart(name, attribute);
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
            WriteName(generator, OpCodes.Ldloc_0, name, true);
            if (isOutputMethod != null) generator.MarkLabel(end);
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
        /// <param name="isEnd"></param>
        internal static void WriteName(ILGenerator generator, OpCode target, string name, bool isEnd)
        {
            AutoCSer.Reflection.Emit.StringWriter stringWriter = new AutoCSer.Reflection.Emit.StringWriter(generator, target, (name.Length << 1) + (isEnd ? 6 : 4));
            stringWriter.Write('<');
            if (isEnd) stringWriter.Write('/');
            stringWriter.Write(name);
            stringWriter.Write('>');
            stringWriter.WriteEnd();
        }
    }
}
