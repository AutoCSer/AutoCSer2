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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static DictionaryGenericType create<T, KT, VT>()
            where T : IDictionary<KT, VT>
        {
            return new DictionaryGenericType<T, KT, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static DictionaryGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">IDictionary 类型</param>
        /// <returns></returns>
        public static DictionaryGenericType Get(Type type, Type interfaceType)
        {
            DictionaryGenericType value = lastGenericType;
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
        /// 创建随机字典委托
        /// </summary>
        internal override Delegate CreateDictionaryDelegate { get { return (Func<Config, T>)Creator.CreateDictionary<T, KT, VT>; } }
    }
}
