using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 代理控制器方法参数约束
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterConstraintAttribute : Attribute
    {
        /// <summary>
        /// 默认为 false 表示不允许默认值，接口实现 IEquatable{T} 优先于引用类型的 null 值判断
        /// </summary>
        public bool IsDefault;
        /// <summary>
        /// 默认为 false 表示不允许空集合，仅支持 string / ICollection
        /// </summary>
        public bool IsEmpty;
        /// <summary>
        /// 默认为 true 表示检查接口约束 AutoCSer.NetCoreWeb.IParameterConstraint
        /// </summary>
        public bool IsParameterConstraint = true;
    }
}
