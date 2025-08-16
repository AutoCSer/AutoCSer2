using AutoCSer.Extensions;
using System;
using System.Reflection.Emit;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeMemberDynamicMethod
    {
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
        /// <param name="name"></param>
        public DeserializeMemberDynamicMethod(Type type, string name)
        {
            dynamicMethod = new DynamicMethod(name, null, new Type[] { typeof(BinaryDeserializer), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            Type fieldType = field.Field.FieldType;
            generator.Emit(OpCodes.Ldarg_0);
            if (field.Field.IsStatic)
            {
                LocalBuilder staticMember = generator.DeclareLocal(fieldType);
                generator.initobj(fieldType, staticMember);
                generator.Emit(OpCodes.Ldloca, staticMember);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field.Field);
            }
            generator.call(Common.GetMemberDeserializeDelegate(fieldType).Method);
        }
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        /// <param name="fixedFillSize"></param>
        public void FixedFillSize(int fixedFillSize)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.int32(fixedFillSize);
            generator.call(fixedFillSizeDelegate.Method);
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

        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private static readonly Action<BinaryDeserializer, int> fixedFillSizeDelegate = BinaryDeserializer.FixedFillSize;
    }
}
