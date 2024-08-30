using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class ClassGenericType : AutoCSer.Metadata.GenericTypeCache<ClassGenericType>
    {
        /// <summary>
        /// 检查参数不允许为 null
        /// </summary>
        internal abstract Delegate ParameterCheckerCheckNullDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static ClassGenericType create<T>()
            where T : class
        {
            return new ClassGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static ClassGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ClassGenericType Get(Type type)
        {
            ClassGenericType value = lastGenericType;
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
    internal sealed class ClassGenericType<T> : ClassGenericType
        where T : class
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 检查参数不允许为 null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        private delegate bool parameterCheckerCheckNullDelegate(T value, string name, string summary, ref ParameterChecker checker);
        /// <summary>
        /// 检查参数不允许为 null
        /// </summary>
        internal override Delegate ParameterCheckerCheckNullDelegate { get { return (parameterCheckerCheckNullDelegate)ParameterChecker.CheckNull<T>; } }
    }
}
