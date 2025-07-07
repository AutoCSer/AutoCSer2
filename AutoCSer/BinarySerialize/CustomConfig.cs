using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public class CustomConfig
    {
#if AOT
        /// <summary>
        /// 默认为 true 表示非泛型反射调用输出异常日志（用于测试）
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReflectionLog()
        {
            return true;
        }
#else
        /// <summary>
        /// 根据类型获取自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual SerializeDelegate GetCustomSerializeDelegate(Type type)
        {
            //if (!binarySerializeMethodInfo.IsCodeGenerator)
            {
                KeyValue<SerializeDelegate, DeserializeDelegate> serializeDelegate;
                HashObject<System.Type> hashType = type;
                Monitor.Enter(customSerializeDelegateLock);
                try
                {
                    if (customSerializeDelegate.TryGetValue(hashType, out serializeDelegate)) return serializeDelegate.Key;
                }
                finally { Monitor.Exit(customSerializeDelegateLock); }
            }

            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
#if NetStandard21
                Type?[]? referenceTypes = null;
#else
                Type[] referenceTypes = null;
#endif
                var attribute = type.GetCustomAttribute(typeof(BinarySerializeAttribute), false);
                if (attribute != null)
                {
                    referenceTypes = ((BinarySerializeAttribute)attribute).CustomReferenceTypes;
                    if (referenceTypes != null && type.IsGenericType)
                    {
                        int referenceTypeIndex = 0;
                        foreach (var referenceType in referenceTypes)
                        {
                            if (referenceType == null) referenceTypes[referenceTypeIndex] = type.GetGenericArguments()[0];
                            else if (referenceType.IsGenericTypeDefinition)
                            {
                                referenceTypes[referenceTypeIndex] = referenceType.MakeGenericType(type.GetGenericArguments());
                            }
                            ++referenceTypeIndex;
                        }
                    }
                }
                return new SerializeDelegate(CustomSerializeGenericType.Get(type).SerializeDelegate, referenceTypes);
            }

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum) return new SerializeDelegate(EnumGenericType.Get(elementType).BinarySerializeEnumLeftArrayDelegate, EmptyArray<Type>.Array);
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            return new SerializeDelegate(StructGenericType.Get(referenceTypes[0]).BinarySerializeNullableLeftArrayDelegate, null, referenceTypes, true);
                        }
                        return new SerializeDelegate(GenericType.Get(elementType).BinarySerializeStructLeftArrayDelegate, null, elementTypeArray, true);
                    }
                    return new SerializeDelegate(ClassGenericType.Get(elementType).BinarySerializeLeftArrayDelegate, null, elementTypeArray, true);
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum) return new SerializeDelegate(EnumGenericType.Get(elementType).BinarySerializeEnumListArrayDelegate, elementTypeArray);
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            return new SerializeDelegate(StructGenericType.Get(referenceTypes[0]).BinarySerializeNullableListArrayDelegate, null, referenceTypes, true);
                        }
                        return new SerializeDelegate(GenericType.Get(elementType).BinarySerializeStructListArrayDelegate, null, elementTypeArray, true);
                    }
                    return new SerializeDelegate(ClassGenericType.Get(elementType).BinarySerializeListArrayDelegate, null, elementTypeArray, true);
                }
            }
            return default(SerializeDelegate);
        }
        /// <summary>
        /// Custom serialization委托集合
        /// </summary>
        protected readonly Dictionary<HashObject<System.Type>, KeyValue<SerializeDelegate, DeserializeDelegate>> customSerializeDelegate = DictionaryCreator.CreateHashObject<System.Type, KeyValue<SerializeDelegate, DeserializeDelegate>>();
        /// <summary>
        /// Custom serialization委托集合访问锁
        /// </summary>
        protected readonly object customSerializeDelegateLock = new object();
        /// <summary>
        /// 添加自定义序列化委托（应该在该类型的静态构造函数中调用，否则可能无法生效）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegate"></param>
        /// <param name="deserializeDelegate"></param>
        /// <returns>如果已经存在该类型的自定义序列化委托则添加失败并返回 false</returns>
        public virtual bool AppendCustomSerializeDelegate(Type type, SerializeDelegate serializeDelegate, DeserializeDelegate deserializeDelegate)
        {
            HashObject<System.Type> hashType = type;
            Monitor.Enter(customSerializeDelegateLock);
            try
            {
                if (!customSerializeDelegate.ContainsKey(hashType))
                {
                    customSerializeDelegate.Add(hashType, new KeyValue<SerializeDelegate, DeserializeDelegate>(serializeDelegate, deserializeDelegate));
                    return true;
                }
            }
            finally { Monitor.Exit(customSerializeDelegateLock); }
            return false;
        }
        /// <summary>
        /// 获取自定义类型序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        public virtual IEnumerable<SerializeDelegate> PrimitiveSerializeDelegates { get { return EmptyArray<SerializeDelegate>.Array; } }

        /// <summary>
        /// 获取自定义类型序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.BinaryDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        public virtual IEnumerable<DeserializeDelegate> PrimitiveDeserializeDelegates { get { return EmptyArray<DeserializeDelegate>.Array; } }
        /// <summary>
        /// 判断类型是否采用 JSON 序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsJsonSerialize(Type type)
        {
            return false;
        }

        /// <summary>
        /// 根据类型获取自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.BinaryDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual DeserializeDelegate GetCustomDeserializeDelegate(Type type)
        {
            KeyValue<SerializeDelegate, DeserializeDelegate> serializeDelegate;
            HashObject<System.Type> hashType = type;
            Monitor.Enter(customSerializeDelegateLock);
            try
            {
                if (customSerializeDelegate.TryGetValue(hashType, out serializeDelegate)) return serializeDelegate.Value;
            }
            finally { Monitor.Exit(customSerializeDelegateLock); }

            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return new DeserializeDelegate(CustomSerializeGenericType.Get(type).DeserializeDelegate);
            }

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum) return new DeserializeDelegate(EnumGenericType.Get(elementType).BinaryDeserializeEnumLeftArrayDelegate);
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            return new DeserializeDelegate(StructGenericType.Get(referenceTypes[0]).BinaryDeserializeNullableLeftArrayDelegate);
                        }
                        return new DeserializeDelegate(StructGenericType.Get(elementType).BinaryDeserializeStructLeftArrayDelegate);
                    }
                    return new DeserializeDelegate(ClassGenericType.Get(elementType).BinaryDeserializeLeftArrayDelegate);
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum) return new DeserializeDelegate(EnumGenericType.Get(elementType).BinaryDeserializeEnumListArrayDelegate);
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            return new DeserializeDelegate(StructGenericType.Get(referenceTypes[0]).BinaryDeserializeNullableListArrayDelegate);
                        }
                        return new DeserializeDelegate(StructGenericType.Get(elementType).BinaryDeserializeStructListArrayDelegate);
                    }
                    return new DeserializeDelegate(ClassGenericType.Get(elementType).BinaryDeserializeListArrayDelegate);
                }
            }
            return default(DeserializeDelegate);
        }
#endif
        /// <summary>
        /// Custom serialization不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字节数量</returns>
        public virtual int NotSupport<T>(BinarySerializer binarySerializer, T value)
        {
            if (value != null && binarySerializer.Config.IsRealType)
            {
                Type type = value.GetType();
                if (type != typeof(T))
                {
#if AOT
                    BinarySerializer.RealTypeObjectMethod.MakeGenericMethod(type).Invoke(null, new object[] { binarySerializer, value });
#else
                    GenericType.Get(type).BinarySerializeRealTypeObjectDelegate(binarySerializer, value);
#endif
                    return 0;
                }
            }
            binarySerializer.Stream.Write(BinarySerializer.NullValue);
            return 0;
        }
        /// <summary>
        /// Custom serialization不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
#if NetStandard21
        public virtual void NotSupport<T>(BinaryDeserializer deserializer, ref T? value)
#else
        public virtual void NotSupport<T>(BinaryDeserializer deserializer, ref T value)
#endif
        {
            deserializer.NotSupport(ref value);
        }
    }
}
