using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Json
{
    /// <summary>
    /// 反序列化动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeDynamicMethod
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Type pointerRefType = typeof(AutoCSer.Memory.Pointer).MakeByRefType();
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
        /// 
        /// </summary>
        private Label returnLabel;
        /// <summary>
        /// 
        /// </summary>
        private int index;
        /// <summary>
        /// 
        /// </summary>
        private bool isMemberMap;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="isMemberMap"></param>
        public DeserializeDynamicMethod(GenericType genericType, bool isMemberMap)
        {
            this.genericType = genericType;
            Type type = genericType.CurrentType;
            dynamicMethod = new DynamicMethod((this.isMemberMap = isMemberMap) ? (AutoCSer.Common.NamePrefix + "JsonMemberMapDeserializer") : (AutoCSer.Common.NamePrefix + "JsonDeserializer"), null, isMemberMap ? new Type[] { typeof(JsonDeserializer), type.MakeByRefType(), pointerRefType, genericType.GetMemberMapType } : new Type[] { typeof(JsonDeserializer), type.MakeByRefType(), pointerRefType }, type, true);
            generator = dynamicMethod.GetILGenerator();
            returnLabel = generator.DefineLabel();

            index = 0;
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        private static unsafe readonly MethodInfo isNameMethod = ((JsonDeserializer.NameDelegate)JsonDeserializer.IsName).Method;
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        private unsafe void isName()
        {
            #region if (!JsonDeserializer.IsName(jsonDeserializer, ref names)) return;
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.call(isNameMethod);
            generator.Emit(OpCodes.Brfalse, returnLabel);
            #endregion
        }
        /// <summary>
        /// 移动到下一个名称
        /// </summary>
        private static unsafe readonly MethodInfo nextNameIndexMethod = ((JsonDeserializer.NameDelegate)JsonDeserializer.NextNameIndex).Method;
        /// <summary>
        /// 
        /// </summary>
        private void nextIndex()
        {
            #region if (!JsonDeserializer.NextNameIndex(jsonDeserializer, ref names)) return;
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.call(nextNameIndexMethod);
            generator.Emit(OpCodes.Brfalse, returnLabel);
            #endregion
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldIndex field)
        {
            isName();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldflda, field.Member);
            generator.call(Common.GetMemberDeserializeDelegate(field.Member.FieldType).Method);

            nextIndex();
            if (isMemberMap) generator.memberMapObjectSetMember(OpCodes.Ldarg_3, field.MemberIndex, genericType);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod)
        {
            isName();

            Type memberType = property.Member.PropertyType;
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.initobj(memberType, loadMember);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloca, loadMember);
            generator.call(Common.GetMemberDeserializeDelegate(memberType).Method);

            generator.Emit(OpCodes.Ldarg_1);
            if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc, loadMember);
            generator.call(propertyMethod);

            nextIndex();
            if (isMemberMap) generator.memberMapObjectSetMember(OpCodes.Ldarg_3, property.MemberIndex, genericType);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>成员转换委托</returns>
        public Delegate Create(Type type)
        {
            #region return;
            generator.MarkLabel(returnLabel);
            generator.Emit(OpCodes.Ret);
            #endregion
            return dynamicMethod.CreateDelegate(type);
        }

        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns>解析委托函数</returns>
        public static DynamicMethod CreateDynamicMethod(Type type, FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "JsonDeserializer" + field.Name, null, new Type[] { typeof(JsonDeserializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldflda, field);
            generator.call(Common.GetMemberDeserializeDelegate(field.FieldType).Method);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod;
        }
        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <param name="propertyMethod"></param>
        /// <returns>解析委托函数</returns>
        public static DynamicMethod CreateDynamicMethod(Type type, PropertyInfo property, MethodInfo propertyMethod)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "JsonDeserializer" + property.Name, null, new Type[] { typeof(JsonDeserializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            Type memberType = property.PropertyType;
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.initobjShort(memberType, loadMember);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloca_S, loadMember);
            generator.call(Common.GetMemberDeserializeDelegate(memberType).Method);

            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc_0);
            generator.call(propertyMethod);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod;
        }
    }
}
