using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EnumGenericType
#if !AOT
        : GenericTypeCache<EnumGenericType>
#endif
    {
#if !AOT
        /// <summary>
        /// 获取简单序列化枚举委托
        /// </summary>
        internal abstract Delegate SimpleSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取简单反序列化枚举委托
        /// </summary>
        internal abstract Delegate SimpleDeserializeEnumDelegate { get; }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumArrayDelegate { get; }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinarySerializeEnumListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeEnumLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeEnumListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeEnumArrayDelegate { get; }
        /// <summary>
        /// 获取 JSON 序列化枚举委托
        /// </summary>
        internal abstract Delegate JsonSerializeEnumDelegate { get; }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal abstract Delegate JsonDeserializeEnumDelegate { get; }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal abstract Delegate JsonDeserializeEnumFlagsDelegate { get; }
        /// <summary>
        /// 获取获取二进制序列化枚举委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeEnumDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        /// <param name="deserializeDelegate"></param>
        internal abstract void GetBinaryDeserializeEnumDelegate(ref AutoCSer.BinarySerialize.DeserializeDelegate deserializeDelegate);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
#endif
        /// <summary>
        /// 枚举类型映射基本类型
        /// </summary>
        /// <param name="underlyingType"></param>
        /// <returns></returns>
        internal static UnderlyingTypeEnum GetUnderlyingType(Type underlyingType)
        {
            if (underlyingType == typeof(int)) return UnderlyingTypeEnum.Int;
            if (underlyingType == typeof(uint)) return UnderlyingTypeEnum.UInt;
            if (underlyingType == typeof(byte)) return UnderlyingTypeEnum.Byte;
            if (underlyingType == typeof(ulong)) return UnderlyingTypeEnum.ULong;
            if (underlyingType == typeof(ushort)) return UnderlyingTypeEnum.UShort;
            if (underlyingType == typeof(long)) return UnderlyingTypeEnum.Long;
            if (underlyingType == typeof(short)) return UnderlyingTypeEnum.Short;
            if (underlyingType == typeof(sbyte)) return UnderlyingTypeEnum.SByte;
            return UnderlyingTypeEnum.Int;
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
#if !AOT
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取简单序列化枚举委托
        /// </summary>
        internal override Delegate SimpleSerializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumInt<T>;
                    case UnderlyingTypeEnum.UInt: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumUInt<T>;
                    case UnderlyingTypeEnum.Byte: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumByte<T>;
                    case UnderlyingTypeEnum.ULong: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumULong<T>;
                    case UnderlyingTypeEnum.UShort: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumUShort<T>;
                    case UnderlyingTypeEnum.Long: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumLong<T>;
                    case UnderlyingTypeEnum.Short: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumShort<T>;
                    case UnderlyingTypeEnum.SByte: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumSByte<T>;
                    default: return (Action<UnmanagedStream, T>)AutoCSer.SimpleSerialize.Serializer.EnumInt<T>;
                }
            }
        }
        /// <summary>
        /// 获取简单反序列化枚举委托
        /// </summary>
        internal override unsafe Delegate SimpleDeserializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumInt<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumUInt<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumByte<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumULong<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumUShort<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumLong<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumShort<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumSByte<T>;
                    default: return (AutoCSer.SimpleSerialize.Deserializer.DeserializeDelegate<T>)AutoCSer.SimpleSerialize.Deserializer.EnumInt<T>;
                }
            }
        }
        /// <summary>
        /// 获取获取二进制序列化枚举委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeEnumDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            switch (UnderlyingType)
            {
                case UnderlyingTypeEnum.Int: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumInt<T>, (Action<BinarySerializer, int>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.UInt: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumUInt<T>, (Action<BinarySerializer, uint>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.Byte: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumByte<T>, (Action<BinarySerializer, byte>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.ULong: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumULong<T>, (Action<BinarySerializer, ulong>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.UShort: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumUShort<T>, (Action<BinarySerializer, ushort>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.Long: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumLong<T>, (Action<BinarySerializer, long>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.Short: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumShort<T>, (Action<BinarySerializer, short>)BinarySerializer.PrimitiveMemberSerialize); return;
                case UnderlyingTypeEnum.SByte: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumSByte<T>, (Action<BinarySerializer, sbyte>)BinarySerializer.PrimitiveMemberSerialize); return;
                default: serializeDelegateReference.SetPrimitive((Action<BinarySerializer, T>)BinarySerializer.EnumInt<T>, (Action<BinarySerializer, int>)BinarySerializer.PrimitiveMemberSerialize); return;
            }
        }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal override Delegate BinarySerializeEnumArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumIntArray<T>;
                    case UnderlyingTypeEnum.UInt: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumUIntArray<T>;
                    case UnderlyingTypeEnum.Byte: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumByteArray<T>;
                    case UnderlyingTypeEnum.ULong: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumULongArray<T>;
                    case UnderlyingTypeEnum.UShort: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumUShortArray<T>;
                    case UnderlyingTypeEnum.Long: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumLongArray<T>;
                    case UnderlyingTypeEnum.Short: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumShortArray<T>;
                    case UnderlyingTypeEnum.SByte: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumSByteArray<T>;
                    default: return (Action<BinarySerializer, T[]>)BinarySerializer.EnumIntArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal override Delegate BinarySerializeEnumLeftArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumIntLeftArray<T>;
                    case UnderlyingTypeEnum.UInt: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumUIntLeftArray<T>;
                    case UnderlyingTypeEnum.Byte: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumByteLeftArray<T>;
                    case UnderlyingTypeEnum.ULong: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumULongLeftArray<T>;
                    case UnderlyingTypeEnum.UShort: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumUShortLeftArray<T>;
                    case UnderlyingTypeEnum.Long: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumLongLeftArray<T>;
                    case UnderlyingTypeEnum.Short: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumShortLeftArray<T>;
                    case UnderlyingTypeEnum.SByte: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumSByteLeftArray<T>;
                    default: return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.EnumIntLeftArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制序列化枚举数组委托
        /// </summary>
        internal override Delegate BinarySerializeEnumListArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumIntListArray<T>;
                    case UnderlyingTypeEnum.UInt: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumUIntListArray<T>;
                    case UnderlyingTypeEnum.Byte: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumByteListArray<T>;
                    case UnderlyingTypeEnum.ULong: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumULongListArray<T>;
                    case UnderlyingTypeEnum.UShort: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumUShortListArray<T>;
                    case UnderlyingTypeEnum.Long: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumLongListArray<T>;
                    case UnderlyingTypeEnum.Short: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumShortListArray<T>;
                    case UnderlyingTypeEnum.SByte: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumSByteListArray<T>;
                    default: return (Action<BinarySerializer, ListArray<T>>)BinarySerializer.EnumIntListArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal override Delegate BinaryDeserializeEnumLeftArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumIntLeftArray<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumUIntLeftArray<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumByteLeftArray<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumULongLeftArray<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumUShortLeftArray<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumLongLeftArray<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumShortLeftArray<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumSByteLeftArray<T>;
                    default: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.BinaryDeserializer.EnumIntLeftArray<T>;
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal override Delegate BinaryDeserializeEnumListArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
#if NetStandard21
                    case UnderlyingTypeEnum.Int: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumIntListArray<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumUIntListArray<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumByteListArray<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumULongListArray<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumUShortListArray<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumLongListArray<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumShortListArray<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumSByteListArray<T>;
                    default: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)AutoCSer.BinaryDeserializer.EnumIntListArray<T>;
#else
                    case UnderlyingTypeEnum.Int: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumIntListArray<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumUIntListArray<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumByteListArray<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumULongListArray<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumUShortListArray<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumLongListArray<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumShortListArray<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumSByteListArray<T>;
                    default: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.BinaryDeserializer.EnumIntListArray<T>;
#endif
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举数组委托
        /// </summary>
        internal override Delegate BinaryDeserializeEnumArrayDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
#if NetStandard21
                    case UnderlyingTypeEnum.Int: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumIntArray<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumUIntArray<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumByteArray<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumULongArray<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumUShortArray<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumLongArray<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumShortArray<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumSByteArray<T>;
                    default: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)AutoCSer.BinaryDeserializer.EnumIntArray<T>;
#else
                    case UnderlyingTypeEnum.Int: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumIntArray<T>;
                    case UnderlyingTypeEnum.UInt: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumUIntArray<T>;
                    case UnderlyingTypeEnum.Byte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumByteArray<T>;
                    case UnderlyingTypeEnum.ULong: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumULongArray<T>;
                    case UnderlyingTypeEnum.UShort: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumUShortArray<T>;
                    case UnderlyingTypeEnum.Long: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumLongArray<T>;
                    case UnderlyingTypeEnum.Short: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumShortArray<T>;
                    case UnderlyingTypeEnum.SByte: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumSByteArray<T>;
                    default: return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)AutoCSer.BinaryDeserializer.EnumIntArray<T>;
#endif
                }
            }
        }
        /// <summary>
        /// 获取二进制反序列化枚举委托
        /// </summary>
        /// <param name="deserializeDelegate"></param>
        internal override void GetBinaryDeserializeEnumDelegate(ref AutoCSer.BinarySerialize.DeserializeDelegate deserializeDelegate)
        {
            switch (UnderlyingType)
            {
                case UnderlyingTypeEnum.Int: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumIntMember<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumIntMember<T>, true); return;
                case UnderlyingTypeEnum.UInt: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumUIntMember<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumUIntMember<T>, true); return;
                case UnderlyingTypeEnum.Byte: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumByte<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumByteMember<T>, true); return;
                case UnderlyingTypeEnum.ULong: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumULongMember<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumULongMember<T>, true); return;
                case UnderlyingTypeEnum.UShort: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumUShort<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumUShortMember<T>, true); return;
                case UnderlyingTypeEnum.Long: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumLongMember<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumLongMember<T>, true); return;
                case UnderlyingTypeEnum.Short: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumShort<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumShortMember<T>, true); return;
                case UnderlyingTypeEnum.SByte: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumSByte<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumSByteMember<T>, true); return;
                default: deserializeDelegate.Set((AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumIntMember<T>, (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.EnumIntMember<T>, true); return;
            }
        }
        /// <summary>
        /// 获取 JSON 序列化枚举委托
        /// </summary>
        internal override Delegate JsonSerializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (Action<JsonSerializer, T>)JsonSerializer.EnumInt<T>;
                    case UnderlyingTypeEnum.UInt: return (Action<JsonSerializer, T>)JsonSerializer.EnumUInt<T>;
                    case UnderlyingTypeEnum.Byte: return (Action<JsonSerializer, T>)JsonSerializer.EnumByte<T>;
                    case UnderlyingTypeEnum.ULong: return (Action<JsonSerializer, T>)JsonSerializer.EnumULong<T>;
                    case UnderlyingTypeEnum.UShort: return (Action<JsonSerializer, T>)JsonSerializer.EnumUShort<T>;
                    case UnderlyingTypeEnum.Long: return (Action<JsonSerializer, T>)JsonSerializer.EnumLong<T>;
                    case UnderlyingTypeEnum.Short: return (Action<JsonSerializer, T>)JsonSerializer.EnumShort<T>;
                    case UnderlyingTypeEnum.SByte: return (Action<JsonSerializer, T>)JsonSerializer.EnumSByte<T>;
                    default: return (Action<JsonSerializer, T>)JsonSerializer.EnumInt<T>;
                }
            }
        }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal override Delegate JsonDeserializeEnumDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumIntDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.UInt: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumUIntDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.Byte: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumByteDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.ULong: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumULongDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.UShort: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumUShortDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.Long: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumLongDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.Short: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumShortDeserialize<T>.Deserialize;
                    case UnderlyingTypeEnum.SByte: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumSByteDeserialize<T>.Deserialize;
                    default: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumIntDeserialize<T>.Deserialize;
                }
            }
        }
        /// <summary>
        /// 获取 JSON 反序列化枚举委托
        /// </summary>
        internal override Delegate JsonDeserializeEnumFlagsDelegate
        {
            get
            {
                switch (UnderlyingType)
                {
                    case UnderlyingTypeEnum.Int: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumIntDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.UInt: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumUIntDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.Byte: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumByteDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.ULong: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumULongDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.UShort: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumUShortDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.Long: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumLongDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.Short: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumShortDeserialize<T>.DeserializeFlags;
                    case UnderlyingTypeEnum.SByte: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumSByteDeserialize<T>.DeserializeFlags;
                    default: return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.Json.EnumIntDeserialize<T>.DeserializeFlags;
                }
            }
        }
#endif

        /// <summary>
        /// 枚举类型映射基本类型
        /// </summary>
        internal static readonly UnderlyingTypeEnum UnderlyingType;
#if NET8
        /// <summary>
        /// 枚举转数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe static UT ToInt(T value)
        {
#pragma warning disable CS8500
            return *(UT*)&value;
#pragma warning restore CS8500
        }
        /// <summary>
        /// 数字转枚举
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe static T FromInt(UT value)
        {
#pragma warning disable CS8500
            return *(T*)&value;
#pragma warning restore CS8500
        }
        /// <summary>
        /// 数字转枚举
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static T FromInt(void* value)
        {
#pragma warning disable CS8500
            return *(T*)value;
#pragma warning restore CS8500
        }
#else
        /// <summary>
        /// 枚举转数字
        /// </summary>
        internal static readonly Func<T, UT> ToInt;
        /// <summary>
        /// 枚举转数字
        /// </summary>
        internal static readonly Func<UT, T> FromInt;
        /// <summary>
        /// 枚举转数字（不支持）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static UT toInt(T value) { throw new NotSupportedException(); }
        /// <summary>
        /// 枚举转数字（不支持）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T fromInt(UT value) { throw new NotSupportedException(); }
#endif

        static EnumGenericType()
        {
            Type type = System.Enum.GetUnderlyingType(typeof(T));
            if (type == typeof(UT))
            {
                UnderlyingType = GetUnderlyingType(type);
#if !NET8
                DynamicMethod toIntDynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "To" + typeof(UT).FullName, typeof(UT), new Type[] { typeof(T) }, typeof(T), true);
                ILGenerator generator = toIntDynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ret);
                ToInt = (Func<T, UT>)toIntDynamicMethod.CreateDelegate(typeof(Func<T, UT>));

                DynamicMethod fromIntDynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "From" + typeof(UT).FullName, typeof(T), new Type[] { typeof(UT) }, typeof(T), true);
                generator = fromIntDynamicMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ret);
                FromInt = (Func<UT, T>)fromIntDynamicMethod.CreateDelegate(typeof(Func<UT, T>));
#endif
                return;
            }
#if !NET8
            ToInt = toInt;
            FromInt = fromInt;
#endif
        }
    }
}
