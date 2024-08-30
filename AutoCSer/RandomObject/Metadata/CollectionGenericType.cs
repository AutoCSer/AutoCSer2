using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class CollectionGenericType : AutoCSer.Metadata.GenericTypeCache<CollectionGenericType>
    {
        /// <summary>
        /// 创建随机集合委托
        /// </summary>
        internal abstract Delegate CreateCollectionDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static CollectionGenericType create<T, VT>()
            where T : ICollection<VT>
        {
            return new CollectionGenericType<T, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static CollectionGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">ICollection 类型</param>
        /// <returns></returns>
        public static CollectionGenericType Get(Type type, Type interfaceType)
        {
            CollectionGenericType value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = getCollection(type, interfaceType);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class CollectionGenericType<T, VT> : CollectionGenericType
        where T : ICollection<VT>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 创建随机集合委托
        /// </summary>
        internal override Delegate CreateCollectionDelegate { get { return (Func<Config, T>)Creator.CreateCollection<T, VT>; } }
    }
}
