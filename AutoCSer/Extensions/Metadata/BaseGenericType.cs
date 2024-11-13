using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class BaseGenericType : AutoCSer.Metadata.GenericTypeCache<BaseGenericType>
    {
        /// <summary>
        /// 获取 XML 基类序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetXmlSerializeBaseDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// XML 基类反序列化委托
        /// </summary>
        internal abstract Delegate XmlDeserializeBaseDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static BaseGenericType create<T, BT>()
            where T : class, BT
        {
            return new BaseGenericType<T, BT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static BaseGenericType? lastGenericType;
#else
        protected static BaseGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static BaseGenericType Get(Type type, Type baseType)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;

            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type)
                {
                    lastGenericType = value;
                    return value;
                }
            }
            catch { }

            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = (BaseGenericType)createMethod.MakeGenericMethod(type, baseType).Invoke(null, null).notNull();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="BT"></typeparam>
    internal sealed class BaseGenericType<T, BT> : BaseGenericType
        where T : class, BT
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取 XML 基类序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetXmlSerializeBaseDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<XmlSerializer, T>)XmlSerializer.Base<T, BT>, AutoCSer.Metadata.BaseGenericType<T, BT>.ReferenceTypes);
        }
        /// <summary>
        /// XML 基类反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate XmlDeserializeBaseDelegate { get { return (XmlDeserializer.DeserializeDelegate<T?>)XmlDeserializer.Base<T, BT>; } }
#else
        internal override Delegate XmlDeserializeBaseDelegate { get { return (XmlDeserializer.DeserializeDelegate<T>)XmlDeserializer.Base<T, BT>; } }
#endif
    }
}
