using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class ParameterConstraintGenericType : AutoCSer.Metadata.GenericTypeCache<ParameterConstraintGenericType>
    {
        /// <summary>
        /// 自定义约束参数检查
        /// </summary>
        internal abstract Delegate ParameterCheckerCheckConstraintDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static ParameterConstraintGenericType create<T>()
            where T : IParameterConstraint
        {
            return new ParameterConstraintGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static ParameterConstraintGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ParameterConstraintGenericType Get(Type type)
        {
            ParameterConstraintGenericType value = lastGenericType;
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
    internal sealed class ParameterConstraintGenericType<T> : ParameterConstraintGenericType
        where T : IParameterConstraint
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 自定义约束参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        private delegate bool parameterCheckerCheckConstraintDelegate(T value, string name, string summary, ref ParameterChecker checker);
        /// <summary>
        /// 自定义约束参数检查
        /// </summary>
        internal override Delegate ParameterCheckerCheckConstraintDelegate { get { return (parameterCheckerCheckConstraintDelegate)ParameterChecker.CheckConstraint<T>; } }
    }
}
