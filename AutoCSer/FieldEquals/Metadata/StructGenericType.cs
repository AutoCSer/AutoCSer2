﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType : AutoCSer.Metadata.GenericTypeCache<StructGenericType>
    {
        /// <summary>
        /// 可空类型比较委托
        /// </summary>
        internal abstract Delegate NullableDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static StructGenericType create<T>() where T : struct
        {
            return new StructGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static StructGenericType? lastGenericType;
#else
        protected static StructGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
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
    internal sealed class StructGenericType<T> : StructGenericType where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 可空类型比较委托
        /// </summary>
        internal override Delegate NullableDelegate { get { return (Func<T?, T?, bool>)Comparor.NullableEquals<T>; } }
    }
}
