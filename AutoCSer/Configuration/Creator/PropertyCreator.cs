using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 属性配置创建
    /// </summary>
    internal sealed class PropertyCreator : Creator
    {
        /// <summary>
        /// 目标属性
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 属性配置创建
        /// </summary>
        /// <param name="method">目标属性</param>
        internal PropertyCreator(MethodInfo method)
        {
            this.method = method;
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override ConfigObject? Create()
#else
        internal override ConfigObject Create()
#endif
        {
            return method.Invoke(null, null).castClass<ConfigObject>();
        }
    }
}
