using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 参数类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class MethodParameterTypeAttribute : Attribute
    {
        /// <summary>
        /// 重定向类型
        /// </summary>
        public Type RedirectType;
        /// <summary>
        /// 参数类型配置
        /// </summary>
        /// <param name="redirectType">重定向类型</param>
        public MethodParameterTypeAttribute(Type redirectType)
        {
            RedirectType = redirectType;
        }
    }
}
