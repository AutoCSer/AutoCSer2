using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EquatableGenericType : AutoCSer.Metadata.GenericTypeCache<EquatableGenericType>
    {
        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        internal abstract Delegate ParameterCheckerCheckEquatableDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static EquatableGenericType create<T>()
            where T : IEquatable<T>
        {
            return new EquatableGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static EquatableGenericType? lastGenericType;
#else
        protected static EquatableGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EquatableGenericType Get(Type type)
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
    internal sealed class EquatableGenericType<T> : EquatableGenericType
        where T : IEquatable<T>
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
        private delegate bool parameterCheckerCheckEquatableDelegate(T value, string name, string summary, ref ParameterChecker checker);
        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        internal override Delegate ParameterCheckerCheckEquatableDelegate { get { return (parameterCheckerCheckEquatableDelegate)ParameterChecker.CheckEquatable<T>; } }
    }
}
