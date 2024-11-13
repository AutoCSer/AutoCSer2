using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType2 : AutoCSer.Metadata.GenericTypeCache2<GenericType2>
    {
        /// <summary>
        /// XML 键值对序列化委托
        /// </summary>
        internal abstract Delegate XmlDeserializeKeyValuePairDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType2 create<T1, T2>()
        {
            return new GenericType2<T1, T2>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType2? lastGenericType;
#else
        protected static GenericType2 lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static GenericType2 Get(Type type1, Type type2)
        {
            var value = lastGenericType;
            if (value?.CurrentType2 == type2 && value.CurrentType1 == type1) return value;

            value = get(type1, type2);
            lastGenericType = value;
            return value;
        }
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static GenericType2 Get(Type[] types)
        {
            return Get(types[0], types[1]);
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal sealed class GenericType2<T1, T2> : GenericType2
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType1 { get { return typeof(T1); } }
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType2 { get { return typeof(T2); } }

        /// <summary>
        /// XML 键值对序列化委托
        /// </summary>
        internal override Delegate XmlDeserializeKeyValuePairDelegate { get { return (XmlDeserializer.DeserializeDelegate<KeyValuePair<T1, T2>>)XmlDeserializer.KeyValuePair<T1, T2>; } }
    }
}