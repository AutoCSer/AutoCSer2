using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public abstract class CustomConfig
    {
#if !AOT
        /// <summary>
        /// Custom serialization委托集合
        /// </summary>
        protected readonly Dictionary<HashObject<System.Type>, KeyValue<AutoCSer.TextSerialize.SerializeDelegate, Delegate>> customSerializeDelegate = DictionaryCreator.CreateHashObject<System.Type, KeyValue<AutoCSer.TextSerialize.SerializeDelegate, Delegate>>();
        /// <summary>
        /// Custom serialization委托集合访问锁
        /// </summary>
        protected readonly object customSerializeDelegateLock = new object();
        /// <summary>
        /// 添加自定义序列化委托（应该在该类型的静态构造函数中调用，否则可能无法生效）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegate"></param>
        /// <param name="deserializeDelegate">必须是静态方法，第一个参数类型为 AutoCSer.JsonDeserializer / AutoCSer.XmlDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void</param>
        /// <returns>如果已经存在该类型的自定义序列化委托则添加失败并返回 false</returns>
        public virtual bool AppendCustomSerializeDelegate(Type type, AutoCSer.TextSerialize.SerializeDelegate serializeDelegate, Delegate deserializeDelegate)
        {
            HashObject<System.Type> hashType = type;
            Monitor.Enter(customSerializeDelegateLock);
            try
            {
                if (!customSerializeDelegate.ContainsKey(hashType))
                {
                    customSerializeDelegate.Add(hashType, new KeyValue<AutoCSer.TextSerialize.SerializeDelegate, Delegate>(serializeDelegate, deserializeDelegate));
                    return true;
                }
            }
            finally { Monitor.Exit(customSerializeDelegateLock); }
            return false;
        }
        /// <summary>
        /// 根据类型获取自定义序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegate"></param>
        /// <returns></returns>
        protected bool getCustomSerializeDelegate(Type type, out AutoCSer.TextSerialize.SerializeDelegate serializeDelegate)
        {
            HashObject<System.Type> hashType = type;
            Monitor.Enter(customSerializeDelegateLock);
            try
            {
                KeyValue<AutoCSer.TextSerialize.SerializeDelegate, Delegate> value;
                if (customSerializeDelegate.TryGetValue(hashType, out value))
                {
                    serializeDelegate = value.Key;
                    return true;
                }
            }
            finally { Monitor.Exit(customSerializeDelegateLock); }
            serializeDelegate = default(SerializeDelegate);
            return false;
        }
        /// <summary>
        /// 根据类型获取自定义反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deserializeDelegate"></param>
        /// <returns></returns>
#if NetStandard21
        protected bool geteCustomDeserializeDelegate(Type type, [MaybeNullWhen(false)] out Delegate deserializeDelegate)
#else
        protected bool geteCustomDeserializeDelegate(Type type, out Delegate deserializeDelegate)
#endif
        {
            HashObject<Type> hashType = type;
            Monitor.Enter(customSerializeDelegateLock);
            try
            {
                KeyValue<AutoCSer.TextSerialize.SerializeDelegate, Delegate> serializeDelegate;
                if (customSerializeDelegate.TryGetValue(hashType, out serializeDelegate))
                {
                    deserializeDelegate = serializeDelegate.Value;
                    return true;
                }
            }
            finally { Monitor.Exit(customSerializeDelegateLock); }
            deserializeDelegate = null;
            return false;
        }

        /// <summary>
        /// 获取自定义类型序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonSerializer / AutoCSer.XmlDeserializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        public virtual IEnumerable<AutoCSer.TextSerialize.SerializeDelegate> PrimitiveSerializeDelegates { get { return EmptyArray<AutoCSer.TextSerialize.SerializeDelegate>.Array; } }
        /// <summary>
        /// 获取自定义类型反序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.JsonDeserializer / AutoCSer.XmlDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        public virtual IEnumerable<Delegate> PrimitiveDeserializeDelegates { get { return EmptyArray<Delegate>.Array; } }
#endif
        /// <summary>
        /// 根据类型获取自定义序列化需要循环引用检查的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        protected Type?[]? getCustomSerializeReferenceTypes<T>(Type type) where T : SerializeAttribute
#else
        protected Type[] getCustomSerializeReferenceTypes<T>(Type type) where T : SerializeAttribute
#endif
        {
            var attribute = type.GetCustomAttribute(typeof(T), false);
            if (attribute == null) return null;
            var referenceTypes = ((T)attribute).CustomReferenceTypes;
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
            return referenceTypes;
        }

        /// <summary>
        /// 写入浮点数
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, float value)
        {
            charStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return 0;
        }
        /// <summary>
        /// 写入浮点数
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, double value)
        {
            charStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return 0;
        }
        /// <summary>
        /// 写入小数
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, decimal value)
        {
            charStream.SimpleWrite(value.ToString());
            return 0;
        }

        /// <summary>
        /// 找不到构造函数的自定义创建对象处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual bool CallCustomConstructor<T>([MaybeNullWhen(false)] out T value)
#else
        public virtual bool CallCustomConstructor<T>(out T value)
#endif
        {
            value = default(T);
            return false;
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public unsafe virtual bool Deserialize(ref SubString buffer, out double value)
        {
#if NetStandard21
            if (buffer.Length != 0)
            {
                fixed (char* bufferFixed = buffer.GetFixedBuffer())
                {
                    return double.TryParse(new ReadOnlySpan<char>(bufferFixed + buffer.Start, buffer.Length), out value);
                }
            }
            value = 0;
            return false;
#else
            return double.TryParse(buffer, out value);
#endif
        }
    }
}
