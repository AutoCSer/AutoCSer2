using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection.Emit;

namespace AutoCSer.BinarySerialize
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
        /// 开始位置
        /// </summary>
#if NetStandard21
        private LocalBuilder? startIndexLocalBuilder;
#else
        private LocalBuilder startIndexLocalBuilder;
#endif
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
        public SerializeMemberMapDynamicMethod(GenericType genericType, string name, bool isFixedFillSize)
        {
            this.genericType = genericType;
            Type type = genericType.CurrentType;
            dynamicMethod = new DynamicMethod(name, null, new Type[] { genericType.GetMemberMapType, typeof(BinarySerializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
            if (this.isFixedFillSize = isFixedFillSize)
            {
                startIndexLocalBuilder = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldarg_1);
                generator.call(getStreamCurrentIndexDelegate.Method);
                generator.Emit(OpCodes.Stloc_S, startIndexLocalBuilder);
            }
            else startIndexLocalBuilder = null;
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
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
            else generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Ldfld, field.Field);
            generator.call(Common.GetMemberSerializeDelegate(field.Field.FieldType).Method);

            generator.MarkLabel(end);
        }
        /// <summary>
        /// 补白对齐 4 字节
        /// </summary>
        public void SerializeFill()
        {
            if (isFixedFillSize)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldloc_S, startIndexLocalBuilder.notNull());
                generator.call(serializeFillDelegate.Method);
            }
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
        /// 获取当前流位置
        /// </summary>
        private static readonly Func<BinarySerializer, int> getStreamCurrentIndexDelegate = BinarySerializer.GetStreamCurrentIndex;
        /// <summary>
        /// 补白对齐 4 字节
        /// </summary>
        private static readonly Action<BinarySerializer, int> serializeFillDelegate = BinarySerializer.SerializeFill;
    }
}
