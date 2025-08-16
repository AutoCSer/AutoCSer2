using AutoCSer.Extensions;
using System;
using System.Reflection.Emit;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberDynamicMethod
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
        public SerializeMemberDynamicMethod(Type type, string name)
        {
            dynamicMethod = new DynamicMethod(name, null, new Type[] { typeof(BinarySerializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 1);
            else generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, field.Field);
            generator.call(Common.GetMemberSerializeDelegate(field.Field.FieldType).Method);
        }
        /// <summary>
        /// 填充空白字节
        /// </summary>
        /// <param name="size"></param>
        public void FixedFillSize(int size)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.int32(size);
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
        /// 填充空白字节
        /// </summary>
        private static readonly Action<BinarySerializer, int> fixedFillSizeDelegate = BinarySerializer.FixedFillSize;
    }
}
