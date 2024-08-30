using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EnumGenericType : AutoCSer.Metadata.GenericTypeCache<EnumGenericType>
    {
        /// <summary>
        /// 枚举值对比委托
        /// </summary>
        internal abstract Delegate EqualsDelegate { get; }

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
        /// 枚举值对比委托
        /// </summary>
        internal override Delegate EqualsDelegate
        {
            get
            {
                switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return (Func<T, T, bool>)Comparor.EnumInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return (Func<T, T, bool>)Comparor.EnumUInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return (Func<T, T, bool>)Comparor.EnumByte<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return (Func<T, T, bool>)Comparor.EnumULong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return (Func<T, T, bool>)Comparor.EnumUShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return (Func<T, T, bool>)Comparor.EnumLong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return (Func<T, T, bool>)Comparor.EnumShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return (Func<T, T, bool>)Comparor.EnumSByte<T>;
                    default: return (Func<T, T, bool>)Comparor.EnumInt<T>;
                }
            }
        }
    }
}
