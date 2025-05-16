using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection.Emit;

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeDynamicMethod
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
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public DeserializeDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SimpleDeserializer", typeof(byte*), new Type[] { typeof(byte*), type.MakeByRefType(), typeof(byte*) }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(BinarySerialize.FieldSize field)
        {
            Type type = field.Field.FieldType;
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldflda, field.Field);
            if (type == typeof(string) || type == typeof(byte[])) generator.Emit(OpCodes.Ldarg_2);
            generator.call(type.IsEnum ? EnumGenericType.Get(type).SimpleDeserializeEnumDelegate.Method : Deserializer.GetDeserializeDelegate(type).notNull().Method);
            generator.Emit(OpCodes.Starg_S, 0);
        }
        /// <summary>
        /// 填充对齐数据
        /// </summary>
        /// <param name="fixedFillSize"></param>
        public void FixedFill(int fixedFillSize)
        {
            if (fixedFillSize != 0)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.int32(fixedFillSize);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Starg_S, 0);
            }
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>成员转换委托</returns>
        public Delegate Create(Type type)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(type);
        }
    }
}
