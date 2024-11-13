using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class DictionaryGenericType : AutoCSer.Metadata.GenericTypeCache<DictionaryGenericType>
    {
        /// <summary>
        /// 创建随机字典委托
        /// </summary>
        internal abstract Delegate CreateDictionaryDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static DictionaryGenericType create<T, KT, VT>()
#if NetStandard21
            where T : IDictionary<KT, VT?>
#else
            where T : IDictionary<KT, VT>
#endif
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
#if NetStandard21
        where T : IDictionary<KT, VT?>
#else
        where T : IDictionary<KT, VT>
#endif
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 创建随机字典委托
        /// </summary>
#if NetStandard21
        internal override Delegate CreateDictionaryDelegate { get { return (Func<Config, T?>)Creator.CreateDictionary<T, KT, VT>; } }
#else
        internal override Delegate CreateDictionaryDelegate { get { return (Func<Config, T>)Creator.CreateDictionary<T, KT, VT>; } }
#endif
    }
}
