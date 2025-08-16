using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeDynamicMethod
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
        /// 结束位置
        /// </summary>
        private Label returnLabel;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fixedSize"></param>
        public SerializeDynamicMethod(Type type, int fixedSize)
        {
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SimpleSerializer", null, new Type[] { typeof(UnmanagedStream), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.int32(fixedSize);
            generator.call(AutoCSer.Reflection.Emit.StringWriter.UnmanagedStreamBasePrepSizeMethod);
            generator.Emit(OpCodes.Brfalse, returnLabel = generator.DefineLabel());
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(BinarySerialize.FieldSize field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, field.Field);
            Type type = field.Field.FieldType;
            generator.call(type.IsEnum ? EnumGenericType.Get(type).SimpleSerializeEnumDelegate.Method : Serializer.GetSerializeDelegate(type).notNull().Method);
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
                generator.call(unmanagedStreamBaseMoveSizeMethod);
            }
        }
        /// <summary>
        /// 内存字符流移动当前位置方法信息
        /// </summary>
        internal static readonly MethodInfo unmanagedStreamBaseMoveSizeMethod = ((Action<UnmanagedStreamBase, int>)UnmanagedStreamBase.MoveSize).Method;

        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>成员转换委托</returns>
        public Delegate Create(Type type)
        {
            generator.MarkLabel(returnLabel);
            generator.ret();
            return dynamicMethod.CreateDelegate(type);
        }
    }
}
