using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EquatableGenericType : AutoCSer.Metadata.GenericTypeCache<EquatableGenericType>
    {
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal abstract Delegate EquatableEqualsDelegate { get; }
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal abstract Delegate ReferenceEqualsDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static EquatableGenericType create<T>() where T : IEquatable<T>
        {
            return new EquatableGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static EquatableGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EquatableGenericType Get(Type type)
        {
            EquatableGenericType value = lastGenericType;
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
        /// 对象对比委托
        /// </summary>
        internal override Delegate EquatableEqualsDelegate { get { return (Func<T, T, bool>)Comparor.EquatableEquals<T>; } }
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal override Delegate ReferenceEqualsDelegate { get { return (Func<T, T, bool>)Comparor.ReferenceEquals<T>; } }
    }
}
