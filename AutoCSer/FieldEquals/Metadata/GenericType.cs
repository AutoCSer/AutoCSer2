﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 调用对象对比委托
        /// </summary>
        internal abstract Delegate CallEqualsDelegate { get; }
        /// <summary>
        /// 数组对象对比委托
        /// </summary>
        internal abstract Delegate ArrayDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 调用对象对比委托
        /// </summary>
        internal override Delegate CallEqualsDelegate
        {
            get
            {
#if AOT
                return (Func<object, object, bool>)Comparor.CallEquals<T>;
#else
                return (Func<T, T, bool>)Comparor.CallEquals<T>;
#endif
            }
        }
        /// <summary>
        /// 数组对象对比委托
        /// </summary>
        internal override Delegate ArrayDelegate { get { return (Func<T[], T[], bool>)Comparor.ArrayEquals<T>; } }
    }
}
