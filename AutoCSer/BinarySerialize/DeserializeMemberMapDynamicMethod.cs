using AutoCSer.Metadata;
using System;
using System.Reflection.Emit;
using AutoCSer.Extensions;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeMemberMapDynamicMethod
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
        /// 是否需要补全字节数
        /// </summary>
        private bool isFixedFillSize;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="name"></param>
        /// <param name="isFixedFillSize"></param>
        public DeserializeMemberMapDynamicMethod(GenericType genericType, string name, bool isFixedFillSize)
        {
            this.genericType = genericType;
            Type type = genericType.CurrentType;
            dynamicMethod = new DynamicMethod(name, null, new Type[] { genericType.GetMemberMapType, typeof(BinaryDeserializer), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
            if (this.isFixedFillSize = isFixedFillSize)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.call(setFixedCurrentDelegate.Method);

            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            Label end = generator.DefineLabel();
            generator.memberMapObjectIsMember(OpCodes.Ldarg_0, field.MemberIndex, genericType);
            generator.Emit(OpCodes.Brfalse_S, end);

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldflda, field.Field);
            generator.call(Common.GetMemberDeserializeDelegate(field.Field.FieldType).Method);

            generator.MarkLabel(end);
        }
        /// <summary>
        /// 设置固定数据结束位置
        /// </summary>
        public void SetFixedCurrentEnd()
        {
            if (isFixedFillSize)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.call(setFixedCurrentEndDelegate.Method);
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
        /// 设置固定数据起始位置
        /// </summary>
        private static readonly Action<BinaryDeserializer> setFixedCurrentDelegate = BinaryDeserializer.SetFixedCurrent;
        /// <summary>
        /// 设置固定数据结束位置
        /// </summary>
        private static readonly Action<BinaryDeserializer> setFixedCurrentEndDelegate = BinaryDeserializer.SetFixedCurrentEnd;
    }
}
