using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EnumGenericType : AutoCSer.Metadata.GenericTypeCache<EnumGenericType>
    {
        /// <summary>
        /// 获取 XML 序列化枚举委托
        /// </summary>
        internal abstract Delegate XmlSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal abstract Delegate XmlDeserializeEnumDelegate { get; }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal abstract Delegate XmlDeserializeEnumFlagsDelegate { get; }

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
        /// 获取 XML 序列化枚举委托
        /// </summary>
        internal override Delegate XmlSerializeEnumDelegate
        {
            get
            {
                switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return (Action<XmlSerializer, T>)XmlSerializer.EnumInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return (Action<XmlSerializer, T>)XmlSerializer.EnumUInt<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return (Action<XmlSerializer, T>)XmlSerializer.EnumByte<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return (Action<XmlSerializer, T>)XmlSerializer.EnumULong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return (Action<XmlSerializer, T>)XmlSerializer.EnumUShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return (Action<XmlSerializer, T>)XmlSerializer.EnumLong<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return (Action<XmlSerializer, T>)XmlSerializer.EnumShort<T>;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return (Action<XmlSerializer, T>)XmlSerializer.EnumSByte<T>;
                    default: return (Action<XmlSerializer, T>)XmlSerializer.EnumInt<T>;
                }
            }
        }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal override Delegate XmlDeserializeEnumDelegate
        {
            get
            {
                switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumIntDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumUIntDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumByteDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumULongDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumUShortDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumLongDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumShortDeserialize<T>.Deserialize;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumSByteDeserialize<T>.Deserialize;
                    default: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumIntDeserialize<T>.Deserialize;
                }
            }
        }
        /// <summary>
        /// 获取 XML 反序列化枚举委托
        /// </summary>
        internal override Delegate XmlDeserializeEnumFlagsDelegate
        {
            get
            {
                switch (AutoCSer.Metadata.EnumGenericType<T, UT>.UnderlyingType)
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumIntDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumUIntDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumByteDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumULongDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumUShortDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumLongDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumShortDeserialize<T>.DeserializeFlags;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumSByteDeserialize<T>.DeserializeFlags;
                    default: return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.Xml.EnumIntDeserialize<T>.DeserializeFlags;
                }
            }
        }
    }
}
