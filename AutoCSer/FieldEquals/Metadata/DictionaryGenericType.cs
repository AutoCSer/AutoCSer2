using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class DictionaryGenericType : AutoCSer.Metadata.GenericTypeCache<DictionaryGenericType>
    {
        /// <summary>
        /// 字典比较委托
        /// </summary>
        internal abstract Delegate EqualsDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DictionaryGenericType create<T, KT, VT>()
            where T : IDictionary<KT, VT>
        {
            return new DictionaryGenericType<T, KT, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static DictionaryGenericType? lastGenericType;
#else
        protected static DictionaryGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">IDictionary 类型</param>
        /// <returns></returns>
        public static DictionaryGenericType Get(Type type, Type interfaceType)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = getDictionary(type, interfaceType);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class DictionaryGenericType<T, KT, VT> : DictionaryGenericType
        where T : IDictionary<KT, VT>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 字典比较委托
        /// </summary>
        internal override Delegate EqualsDelegate { get { return (Func<T, T, bool>)Comparor.DictionaryEquals<T, KT, VT>; } }
    }
}
