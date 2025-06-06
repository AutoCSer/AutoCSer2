﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class CollectionGenericType : AutoCSer.Metadata.GenericTypeCache<CollectionGenericType>
    {
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal abstract Delegate EqualsDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static CollectionGenericType create<T, VT>()
            where T : ICollection<VT>
        {
            return new CollectionGenericType<T, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static CollectionGenericType? lastGenericType;
#else
        protected static CollectionGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">ICollection 类型</param>
        /// <returns></returns>
        public static CollectionGenericType Get(Type type, Type interfaceType)
        {
            var value = lastGenericType;
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
        /// 对象对比委托
        /// </summary>
        internal override Delegate EqualsDelegate { get { return (Func<T, T, bool>)Comparor.CollectionEquals<T, VT>; } }
    }
}
