using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EnumGenericType : AutoCSer.Metadata.GenericTypeCache<EnumGenericType>
    {
        /// <summary>
        /// 创建随机枚举值委托
        /// </summary>
        internal abstract Delegate CreateEnumDelegate { get; }

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
#if NetStandard21
        protected static EnumGenericType? lastGenericType;
#else
        protected static EnumGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EnumGenericType Get(Type type)
        {
            var value = lastGenericType;
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
        /// 创建随机枚举值委托
        /// </summary>
        internal override Delegate CreateEnumDelegate
        {
            get
            {
                switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return (Func<Config, T>)Creator.EnumInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return (Func<Config, T>)Creator.EnumUInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return (Func<Config, T>)Creator.EnumByte<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return (Func<Config, T>)Creator.EnumULong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return (Func<Config, T>)Creator.EnumUShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return (Func<Config, T>)Creator.EnumLong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return (Func<Config, T>)Creator.EnumShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return (Func<Config, T>)Creator.EnumSByte<T>;
                    default: return (Func<Config, T>)Creator.EnumInt<T>;
                }
            }
        }
    }
}
