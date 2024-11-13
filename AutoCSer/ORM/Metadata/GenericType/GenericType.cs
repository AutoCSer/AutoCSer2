using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        internal abstract Delegate ReadJsonDelegate { get; }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        internal abstract Delegate JsonSerializeDelegate { get; }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        internal abstract Delegate ReadRemoteProxyJsonDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType? lastGenericType;
#else
        protected static GenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
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
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
#if NetStandard21
        internal override Delegate ReadJsonDelegate { get { return (Func<DbDataReader, int, T?>)AutoCSer.ORM.Member.ReadJson<T>; } }
#else
        internal override Delegate ReadJsonDelegate { get { return (Func<DbDataReader, int, T>)AutoCSer.ORM.Member.ReadJson<T>; } }
#endif
        /// <summary>
        /// JSON 序列化
        /// </summary>
#if NetStandard21
        internal override Delegate JsonSerializeDelegate { get { return (Func<T, string?>)AutoCSer.ORM.Member.JsonSerialize<T>; } }
#else
        internal override Delegate JsonSerializeDelegate { get { return (Func<T, string>)AutoCSer.ORM.Member.JsonSerialize<T>; } }
#endif
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
#if NetStandard21
        internal override Delegate ReadRemoteProxyJsonDelegate { get { return (Func<RemoteProxy.DataValue[], int, T?>)AutoCSer.ORM.Member.ReadRemoteProxyJson<T>; } }
#else
        internal override Delegate ReadRemoteProxyJsonDelegate { get { return (Func<RemoteProxy.DataValue[], int, T>)AutoCSer.ORM.Member.ReadRemoteProxyJson<T>; } }
#endif
    }
}
