using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EquatableGenericType : GenericTypeCache<EquatableGenericType>
    {
        /// <summary>
        /// 服务端异步调用队列集合类型
        /// </summary>
        internal abstract Type ServerCallTaskQueueSetType { get; }
        /// <summary>
        /// 获取服务端异步调用队列
        /// </summary>
        internal abstract Delegate CommandListenerGetServerCallTaskQueueSetDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static EquatableGenericType create<T>() where T : IEquatable<T>
        {
            return new EquatableGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static EquatableGenericType? lastGenericType;
#else
        protected static EquatableGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EquatableGenericType Get(Type type)
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
    internal sealed class EquatableGenericType<T> : EquatableGenericType where T : IEquatable<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 服务端异步调用队列集合类型
        /// </summary>
        internal override Type ServerCallTaskQueueSetType { get { return typeof(AutoCSer.Net.CommandServerCallTaskQueueSet<T>); } }
        /// <summary>
        /// 获取服务端异步调用队列
        /// </summary>
#if NetStandard21
        internal override Delegate CommandListenerGetServerCallTaskQueueSetDelegate { get { return (Func<AutoCSer.Net.CommandListener, AutoCSer.Net.CommandServerCallTaskQueueSet<T>?>)AutoCSer.Net.CommandListener.GetServerCallTaskQueueSet<T>; } }
#else
        internal override Delegate CommandListenerGetServerCallTaskQueueSetDelegate { get { return (Func<AutoCSer.Net.CommandListener, AutoCSer.Net.CommandServerCallTaskQueueSet<T>>)AutoCSer.Net.CommandListener.GetServerCallTaskQueueSet<T>; } }
#endif
    }
}
