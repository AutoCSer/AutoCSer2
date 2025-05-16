using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class DictionaryGenericType2 : GenericTypeCache2<DictionaryGenericType2>
    {
        /// <summary>
        /// JSON 字典序列化委托
        /// </summary>
        internal abstract Delegate JsonSerializeDictionaryDelegate { get; }
        /// <summary>
        /// JSON 字典反序列化委托
        /// </summary>
        internal abstract Delegate JsonDeserializeDictionaryDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DictionaryGenericType2 create<KT, VT>()
#if NetStandard21
            where KT : notnull
#endif
        {
            return new DictionaryGenericType2<KT, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static DictionaryGenericType2? lastGenericType;
#else
        protected static DictionaryGenericType2 lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static DictionaryGenericType2 Get(Type type1, Type type2)
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
        public static DictionaryGenericType2 Get(Type[] types)
        {
            return Get(types[0], types[1]);
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class DictionaryGenericType2<KT, VT> : DictionaryGenericType2
#if NetStandard21
        where KT : notnull
#endif
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType1 { get { return typeof(KT); } }
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType2 { get { return typeof(VT); } }

        /// <summary>
        /// JSON 字典序列化委托
        /// </summary>
        internal override Delegate JsonSerializeDictionaryDelegate
        {
            get
            {
                if (typeof(KT) == typeof(string)) return (Action<JsonSerializer, Dictionary<string, VT>>)JsonSerializer.StringDictionary<VT>;
#if NetStandard21
                return (Action<JsonSerializer, Dictionary<KT, VT?>>)JsonSerializer.Dictionary<KT, VT>;
#else
                return (Action<JsonSerializer, Dictionary<KT, VT>>)JsonSerializer.Dictionary<KT, VT>;
#endif
            }
        }
        /// <summary>
        /// JSON 字典反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeDictionaryDelegate { get { return (JsonDeserializer.DeserializeDelegate<Dictionary<KT, VT?>?>)JsonDeserializer.Dictionary<KT, VT>; } }
#else
        internal override Delegate JsonDeserializeDictionaryDelegate { get { return (JsonDeserializer.DeserializeDelegate<Dictionary<KT, VT>>)JsonDeserializer.Dictionary<KT, VT>; } }
#endif
    }
}