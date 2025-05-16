using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class ClassGenericType : GenericTypeCache<ClassGenericType>
    {
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal abstract ConfigObject CreateConfigObject(object value);
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract Task<ConfigObject?> CreateConfigObjectTask(object task);
#else
        internal abstract Task<ConfigObject> CreateConfigObjectTask(object task);
#endif
        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract Task<ConfigObject?> GetConfigObjectTask(object task);
#else
        internal abstract Task<ConfigObject> GetConfigObjectTask(object task);
#endif

        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeArrayDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal abstract Delegate BinarySerializeLeftArrayDelegate { get; }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal abstract Delegate BinarySerializeListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeArrayDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static ClassGenericType create<T>() where T : class
        {
            return new ClassGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static ClassGenericType? lastGenericType;
#else
        protected static ClassGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ClassGenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ClassGenericType<T> : ClassGenericType
        where T : class
    {
        /// <summary>
        /// 引用类型数组
        /// </summary>
        private static readonly Type[] referenceTypes = new Type[] { typeof(T) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override ConfigObject CreateConfigObject(object value)
        {
            return ConfigObject.Create<T>(value);
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        internal override Task<ConfigObject?> CreateConfigObjectTask(object task)
#else
        internal override Task<ConfigObject> CreateConfigObjectTask(object task)
#endif
        {
            return ConfigObject.CreateTask<T>(task);
        }
        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        internal override Task<ConfigObject?> GetConfigObjectTask(object task)
#else
        internal override Task<ConfigObject> GetConfigObjectTask(object task)
#endif
        {
            return ConfigObject.GetTask<T>(task);
        }

        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeArrayDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<AutoCSer.BinarySerializer, T[]>)AutoCSer.BinarySerializer.Array<T>, referenceTypes, BinarySerialize.SerializePushTypeEnum.Primitive, true);
        }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal override Delegate BinarySerializeLeftArrayDelegate { get { return (Action<AutoCSer.BinarySerializer, LeftArray<T>>)AutoCSer.BinarySerializer.LeftArray<T>; } }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal override Delegate BinarySerializeListArrayDelegate { get { return (Action<AutoCSer.BinarySerializer, ListArray<T>>)AutoCSer.BinarySerializer.ListArray<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeLeftArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T?>>)BinaryDeserializer.LeftArray<T>; } }
#else
        internal override Delegate BinaryDeserializeLeftArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)BinaryDeserializer.LeftArray<T>; } }
#endif
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T?>?>)BinaryDeserializer.ListArray<T>; } }
#else
        internal override Delegate BinaryDeserializeListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)BinaryDeserializer.ListArray<T>; } }
#endif
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T?[]?>)BinaryDeserializer.Array<T>; } }
#else
        internal override Delegate BinaryDeserializeArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)BinaryDeserializer.Array<T>; } }
#endif
    }
}
