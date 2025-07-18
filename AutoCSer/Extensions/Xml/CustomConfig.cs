﻿using AutoCSer.CodeGenerator;
using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public class CustomConfig : AutoCSer.TextSerialize.CustomConfig
    {
#if AOT
        /// <summary>
        /// 根据类型获取 XML 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.XmlSerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AutoCSer.TextSerialize.SerializeDelegate GetCustomSerializeDelegate<T>()
        {
            Type type = typeof(T);
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type))
            {
                return new AutoCSer.TextSerialize.SerializeDelegate(XmlSerializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)), getCustomSerializeReferenceTypes<XmlSerializeAttribute>(type));
            }
            return default(AutoCSer.TextSerialize.SerializeDelegate);
        }
#else
        /// <summary>
        /// 根据类型获取 XML 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.XmlSerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual AutoCSer.TextSerialize.SerializeDelegate GetCustomSerializeDelegate(Type type)
        {
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate;
            if (getCustomSerializeDelegate(type, out serializeDelegate)) return serializeDelegate;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return new AutoCSer.TextSerialize.SerializeDelegate(CustomSerializeGenericType.Get(type).SerializeDelegate, getCustomSerializeReferenceTypes<XmlSerializeAttribute>(type));
            }
            return default(AutoCSer.TextSerialize.SerializeDelegate);
        }
        /// <summary>
        /// 根据类型获取 XML 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.XmlDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual Delegate? GeteCustomDeserializDelegate(Type type)
#else
        public virtual Delegate GeteCustomDeserializDelegate(Type type)
#endif
        {
            var deserializDelegate = default(Delegate);
            if (geteCustomDeserializeDelegate(type, out deserializDelegate)) return deserializDelegate;

            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return CustomSerializeGenericType.Get(type).DeserializeDelegate;
            }
            return null;
        }
#endif

        /// <summary>
        /// 写入整数
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, Int128 value)
        {
#if NET8
            serializer.CharStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
#else
            TypeSerializer<SerializeInt128>.Serialize(serializer, new Int128Union { Int128 = value }.SerializeValue);
#endif
            return 0;
        }
        /// <summary>
        /// 写入整数
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, UInt128 value)
        {
#if NET8
            serializer.CharStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
#else
            TypeSerializer<SerializeInt128>.Serialize(serializer, new Int128Union { UInt128 = value }.SerializeValue);
#endif
            return 0;
        }
        /// <summary>
        /// 写入浮点数
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, Half value)
        {
#if NET8
            serializer.CharStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
#else
            serializer.CharStream.WriteString(new HalfUnion { Half = value }.UShort);
#endif
            return 0;
        }
        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, DateTime value)
        {
            serializer.PrimitiveSerialize(value);
            return 0;
        }
        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, TimeSpan value)
        {
            serializer.PrimitiveSerialize(value);
            return 0;
        }
        /// <summary>
        /// Custom serialization不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int NotSupport<T>(XmlSerializer serializer, T value)
        {
            return 0;
        }

        /// <summary>
        /// 自定义反序列化整数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, ref Int128 value)
        {
#if NET8
            AutoCSer.Memory.Pointer buffer = deserializer.GetValue();
            if (buffer.ByteSize != 0)
            {
                var stringBuffer = deserializer.GetStringBuffer(ref buffer);
                if (stringBuffer.Length != 0 && Int128.TryParse(stringBuffer, out value))
                {
                    deserializer.GetValueEnd();
                    return true;
                }
            }
            return false;
#else
            SerializeInt128 serializeInt = default(SerializeInt128);
            TypeDeserializer<SerializeInt128>.DefaultDeserializer(deserializer, ref serializeInt);
            value = new Int128Union { SerializeValue = serializeInt }.Int128;
            return true;
#endif
        }
        /// <summary>
        /// 自定义反序列化整数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, ref UInt128 value)
        {
#if NET8
            AutoCSer.Memory.Pointer buffer = deserializer.GetValue();
            if (buffer.ByteSize != 0)
            {
                var stringBuffer = deserializer.GetStringBuffer(ref buffer);
                if (stringBuffer.Length != 0 && UInt128.TryParse(stringBuffer, out value))
                {
                    deserializer.GetValueEnd();
                    return true;
                }
            }
            return false;
#else
            SerializeInt128 serializeInt = default(SerializeInt128);
            TypeDeserializer<SerializeInt128>.DefaultDeserializer(deserializer, ref serializeInt);
            value = new Int128Union { SerializeValue = serializeInt }.UInt128;
            return true;
#endif
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual unsafe bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref Half value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            if (stringBuffer.Length != 0)
            {
#if NET8
                return Half.TryParse(stringBuffer, out value);
#else
                ushort ushortValue;
                if (ushort.TryParse(stringBuffer, out ushortValue))
                {
                    value = *(Half*)&ushortValue;
                    return true;
                }
#endif
            }
            return false;
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref float value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && float.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref double value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && double.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化小数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref decimal value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && decimal.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref DateTime value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && DateTime.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual unsafe bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref TimeSpan value)
        {
#if NetStandard21
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && TimeSpan.TryParse(stringBuffer, out value);
#else
            return buffer.ByteSize != 0 && TimeSpan.TryParse(new string(buffer.Char, 0, buffer.ByteSize >> 1), out value);
#endif
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual bool NotSupport<T>(XmlDeserializer deserializer, ref T? value)
#else
        public virtual bool NotSupport<T>(XmlDeserializer deserializer, ref T value)
#endif
        {
            value = default(T);
            deserializer.IgnoreValue();
            return true;
        }
    }
}
