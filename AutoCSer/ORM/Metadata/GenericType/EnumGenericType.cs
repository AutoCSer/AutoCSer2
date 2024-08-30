using AutoCSer.Memory;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EnumGenericType : AutoCSer.Metadata.GenericTypeCache<EnumGenericType>
    {
        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <returns></returns>
        internal abstract Action<CharStream, object> GetConstantConverter(ConnectionCreator connectionCreator);
        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <returns></returns>
        internal abstract Action<CharStream, object> GetNullableConstantConverter(ConnectionCreator connectionCreator);
        /// <summary>
        /// 获取常量转换处理方法
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo GetConstantConvertMethod();
        /// <summary>
        /// 获取常量转换处理委托方法
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo GetNullableConstantConvertMethod();

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static EnumGenericType create<T, UT>()
            where T : struct, IConvertible
            where UT : struct, IConvertible
        {
            return new EnumGenericType<T, UT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static EnumGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EnumGenericType Get(Type type)
        {
            EnumGenericType value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = getEnum(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="UT"></typeparam>
    internal sealed class EnumGenericType<T, UT> : EnumGenericType
        where T : struct, IConvertible
        where UT : struct, IConvertible
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <returns></returns>
        internal override Action<CharStream, object> GetConstantConverter(ConnectionCreator connectionCreator)
        {
            switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
            {
                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return connectionCreator.ConvertIntEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return connectionCreator.ConvertUIntEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return connectionCreator.ConvertByteEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return connectionCreator.ConvertULongEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return connectionCreator.ConvertUShortEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return connectionCreator.ConvertLongEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return connectionCreator.ConvertShortEnum<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return connectionCreator.ConvertSByteEnum<T>;
                default: return connectionCreator.ConvertIntEnum<T>;
            }
        }
        /// <summary>
        /// 获取常量转换处理委托
        /// </summary>
        /// <param name="connectionCreator"></param>
        /// <returns></returns>
        internal override Action<CharStream, object> GetNullableConstantConverter(ConnectionCreator connectionCreator)
        {
            switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
            {
                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return connectionCreator.ConvertIntEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return connectionCreator.ConvertUIntEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return connectionCreator.ConvertByteEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return connectionCreator.ConvertULongEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return connectionCreator.ConvertUShortEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return connectionCreator.ConvertLongEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return connectionCreator.ConvertShortEnumNullable<T>;
                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return connectionCreator.ConvertSByteEnumNullable<T>;
                default: return connectionCreator.ConvertIntEnumNullable<T>;
            }
        }
        /// <summary>
        /// 获取常量转换处理方法
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo GetConstantConvertMethod()
        {
            switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
            {
                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return ((Action<CharStream, T>)ConnectionCreator.ConvertIntEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return ((Action<CharStream, T>)ConnectionCreator.ConvertUIntEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return ((Action<CharStream, T>)ConnectionCreator.ConvertByteEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return ((Action<CharStream, T>)ConnectionCreator.ConvertULongEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return ((Action<CharStream, T>)ConnectionCreator.ConvertUShortEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return ((Action<CharStream, T>)ConnectionCreator.ConvertLongEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return ((Action<CharStream, T>)ConnectionCreator.ConvertShortEnum<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return ((Action<CharStream, T>)ConnectionCreator.ConvertSByteEnum<T>).Method;
                default: return ((Action<CharStream, T>)ConnectionCreator.ConvertIntEnum<T>).Method;
            }
        }
        /// <summary>
        /// 获取常量转换处理委托方法
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo GetNullableConstantConvertMethod()
        {
            switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
            {
                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return ((Action<CharStream, T?>)ConnectionCreator.ConvertIntEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return ((Action<CharStream, T?>)ConnectionCreator.ConvertUIntEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return ((Action<CharStream, T?>)ConnectionCreator.ConvertByteEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return ((Action<CharStream, T?>)ConnectionCreator.ConvertULongEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return ((Action<CharStream, T?>)ConnectionCreator.ConvertUShortEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return ((Action<CharStream, T?>)ConnectionCreator.ConvertLongEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return ((Action<CharStream, T?>)ConnectionCreator.ConvertShortEnumNullable<T>).Method;
                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return ((Action<CharStream, T?>)ConnectionCreator.ConvertSByteEnumNullable<T>).Method;
                default: return ((Action<CharStream, T?>)ConnectionCreator.ConvertIntEnumNullable<T>).Method;
            }
        }
    }
}
