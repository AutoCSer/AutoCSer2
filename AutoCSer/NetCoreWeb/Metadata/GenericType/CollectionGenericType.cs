using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class CollectionGenericType : AutoCSer.Metadata.GenericTypeCache<CollectionGenericType>
    {
        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        internal abstract Delegate ParameterCheckerCheckCollectionDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static CollectionGenericType create<T>()
            where T : System.Collections.ICollection
        {
            return new CollectionGenericType<T>();
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
        /// <returns></returns>
        public static CollectionGenericType Get(Type type)
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
    internal sealed class CollectionGenericType<T> : CollectionGenericType
        where T : System.Collections.ICollection
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        private delegate bool parameterCheckerCheckCollectionDelegate(T value, string name, string summary, ref ParameterChecker checker);
        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        internal override Delegate ParameterCheckerCheckCollectionDelegate { get { return (parameterCheckerCheckCollectionDelegate)ParameterChecker.CheckCollection<T>; } }
    }
}
