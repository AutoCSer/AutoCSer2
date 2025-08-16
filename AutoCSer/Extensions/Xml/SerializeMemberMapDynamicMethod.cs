using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Xml
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
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="genericType"></param>
        public SerializeMemberMapDynamicMethod(AutoCSer.Metadata.GenericType genericType)
        {
            this.genericType = genericType;
            Type type = genericType.CurrentType;
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "XmlMemberMapSerializer", null, new Type[] { genericType.GetMemberMapType, typeof(XmlSerializer), type, typeof(CharStream) }, type, true);
            generator = dynamicMethod.GetILGenerator();

            //generator.DeclareLocal(typeof(int));
            //generator.Emit(OpCodes.Ldc_I4_0);
            //generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
#if NetStandard21
        private void nameStart(string name, XmlSerializeMemberAttribute? attribute)
#else
        private void nameStart(string name, XmlSerializeMemberAttribute attribute)
#endif
        {
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, false);
            if (!string.IsNullOrEmpty(attribute?.ItemName))
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.ldstr(attribute.ItemName);
                generator.call(SerializeMemberDynamicMethod.SetItemNameMethod);
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="serializeMethod"></param>
        /// <param name="attribute"></param>
#if NetStandard21
        public void Push(FieldIndex field, MethodInfo serializeMethod, XmlSerializeMemberAttribute? attribute)
#else
        public void Push(FieldIndex field, MethodInfo serializeMethod, XmlSerializeMemberAttribute attribute)
#endif
        {
            Label end = generator.DefineLabel();
            generator.memberMapObjectIsMember(OpCodes.Ldarg_0, field.MemberIndex, genericType);
            generator.Emit(OpCodes.Brfalse, end);

            var isOutputMethod = Common.GetIsOutputDelegate(field.Member.FieldType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.Member);
                generator.call(isOutputMethod.Method);
                generator.Emit(OpCodes.Brfalse, end);
            }
            string name = field.AnonymousName;
            nameStart(name, attribute);
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
                checkNull(field.Member.FieldType, serializeMethod);
            }
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, true);
            generator.MarkLabel(end);
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
            generator.Emit(OpCodes.Ldarg_1);
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
#if NetStandard21
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MethodInfo serializeMethod, XmlSerializeMemberAttribute? attribute)
#else
        public void Push(PropertyIndex property, MethodInfo propertyMethod, MethodInfo serializeMethod, XmlSerializeMemberAttribute attribute)
#endif
        {
            Label end = generator.DefineLabel();
            generator.memberMapObjectIsMember(OpCodes.Ldarg_0, property.MemberIndex, genericType);
            generator.Emit(OpCodes.Brfalse, end);

            var isOutputMethod = Common.GetIsOutputDelegate(property.Member.PropertyType);
            if (isOutputMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
                else generator.Emit(OpCodes.Ldarg_2);
                generator.call(propertyMethod);
                generator.call(isOutputMethod.Method);
                generator.Emit(OpCodes.Brfalse, end);
            }
            string name = property.Member.Name;
            nameStart(name, attribute);
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
                checkNull(property.Member.PropertyType, serializeMethod);
            }
            SerializeMemberDynamicMethod.WriteName(generator, OpCodes.Ldarg_3, name, true);
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>成员转换委托</returns>
        public Delegate Create(Type type)
        {
            generator.ret();
            return dynamicMethod.CreateDelegate(type);
        }
    }
}
