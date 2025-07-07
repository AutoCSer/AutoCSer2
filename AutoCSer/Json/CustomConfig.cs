using AutoCSer.CodeGenerator;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public class CustomConfig : AutoCSer.TextSerialize.CustomConfig
    {
#if AOT
        /// <summary>
        /// 根据类型获取 JSON 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonSerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AutoCSer.TextSerialize.SerializeDelegate GetCustomSerializeDelegate<T>()
        {
            Type type = typeof(T);
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type))
            {
                return new AutoCSer.TextSerialize.SerializeDelegate(JsonSerializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)), getCustomSerializeReferenceTypes<JsonSerializeAttribute>(type));
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    return new AutoCSer.TextSerialize.SerializeDelegate(JsonSerializer.LeftArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes); ;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    return new AutoCSer.TextSerialize.SerializeDelegate(JsonSerializer.ListArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes); ;
                }
            }
            return default(AutoCSer.TextSerialize.SerializeDelegate);
        }
        /// <summary>
        /// 根据类型获取 JSON 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Delegate? GeteCustomDeserializeDelegate<T>()
        {
            Type type = typeof(T);
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type)) return JsonDeserializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>)) return JsonDeserializer.LeftArrayMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                if (genericTypeDefinition == typeof(ListArray<>)) return JsonDeserializer.ListArrayMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
            }
            return null;
        }
#else
        /// <summary>
        /// 根据类型获取 JSON 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonSerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual AutoCSer.TextSerialize.SerializeDelegate GetCustomSerializeDelegate(Type type)
        {
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate;
            if (getCustomSerializeDelegate(type, out serializeDelegate)) return serializeDelegate;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return new AutoCSer.TextSerialize.SerializeDelegate(CustomSerializeGenericType.Get(type).SerializeDelegate, getCustomSerializeReferenceTypes<JsonSerializeAttribute>(type));
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>)) return GenericType.Get(type.GetGenericArguments()[0]).JsonSerializeLeftArrayDelegate;
                if (genericTypeDefinition == typeof(ListArray<>)) return GenericType.Get(type.GetGenericArguments()[0]).JsonSerializeListArrayDelegate;
            }
            return default(AutoCSer.TextSerialize.SerializeDelegate);
        }
        /// <summary>
        /// 根据类型获取 JSON 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual Delegate? GeteCustomDeserializeDelegate(Type type)
#else
        public virtual Delegate GeteCustomDeserializeDelegate(Type type)
#endif
        {
            var deserializeDelegate = default(Delegate);
            if (geteCustomDeserializeDelegate(type, out deserializeDelegate)) return deserializeDelegate;

            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return CustomSerializeGenericType.Get(type).DeserializeDelegate;
            }

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>)) return GenericType.Get(type.GetGenericArguments()[0]).JsonDeserializeLeftArrayDelegate;
                if (genericTypeDefinition == typeof(ListArray<>)) return GenericType.Get(type.GetGenericArguments()[0]).JsonDeserializeListArrayDelegate;
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
        public virtual int Write(JsonSerializer serializer, Int128 value)
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
        public virtual int Write(JsonSerializer serializer, UInt128 value)
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
        public virtual int Write(JsonSerializer serializer, Half value)
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
        public virtual int Write(JsonSerializer serializer, DateTime value)
        {
            serializer.SerializeDateTime(value);
            return 0;
        }
        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(JsonSerializer serializer, TimeSpan value)
        {
            serializer.SerializeTimeSpan(value);
            return 0;
        }
        /// <summary>
        /// Custom serialization不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int NotSupport<T>(JsonSerializer serializer, T value)
        {
            if (value != null)
            {
                Type type = typeof(T);
                if (type.IsInterface) serializer.JsonSerializeObject(value);
                else
                {
                    if (!type.IsArray) serializer.CharStream.WriteJsonObject();
                    else serializer.CharStream.WriteJsonArray();
                }
            }
            else serializer.CharStream.WriteJsonNull();
            return 0;
        }

        /// <summary>
        /// 自定义反序列化整数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <param name="isObject"></param>
        /// <returns></returns>
        public virtual bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref Int128 value, bool isObject)
        {
#if NET8
            if (!isObject)
            {
                var stringBuffer = deserializer.GetStringBuffer(ref buffer);
                return stringBuffer.Length != 0 && Int128.TryParse(stringBuffer, out value);
            }
#else
            if (isObject)
            {
                SerializeInt128 serializeInt = default(SerializeInt128);
                TypeDeserializer<SerializeInt128>.DefaultDeserializer(deserializer, ref serializeInt);
                value = new Int128Union { SerializeValue = serializeInt }.Int128;
                return true;
            }
#endif
            return false;
        }
        /// <summary>
        /// 自定义反序列化整数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <param name="isObject"></param>
        /// <returns></returns>
        public virtual bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref UInt128 value, bool isObject)
        {
#if NET8
            if (!isObject)
            {
                var stringBuffer = deserializer.GetStringBuffer(ref buffer);
                return stringBuffer.Length != 0 && UInt128.TryParse(stringBuffer, out value);
            }
#else
            if (isObject)
            {
                SerializeInt128 serializeInt = default(SerializeInt128);
                TypeDeserializer<SerializeInt128>.DefaultDeserializer(deserializer, ref serializeInt);
                value = new Int128Union { SerializeValue = serializeInt }.UInt128;
                return true;
            }
#endif
            return false;
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual unsafe bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref Half value)
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
        public virtual bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref float value)
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
        public virtual bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref double value)
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
        public virtual bool Deserialize(JsonDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref decimal value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && decimal.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(JsonDeserializer deserializer, ref DateTime value)
        {
            string stringBuffer = deserializer.GetQuoteStringBuffer();
            return stringBuffer.Length != 0 && DateTime.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(JsonDeserializer deserializer, ref TimeSpan value)
        {
            string stringBuffer = deserializer.GetQuoteStringBuffer();
            return stringBuffer.Length != 0 && TimeSpan.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual bool NotSupport<T>(JsonDeserializer deserializer, ref T? value)
#else
        public virtual bool NotSupport<T>(JsonDeserializer deserializer, ref T value)
#endif
        {
            value = default(T);
            deserializer.Ignore();
            return true;
        }
    }
}
