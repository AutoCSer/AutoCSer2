using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 属性配置创建
    /// </summary>
    internal sealed class PropertyObjectCreator : Creator
    {
        /// <summary>
        /// 目标属性
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 属性配置创建
        /// </summary>
        /// <param name="method">目标属性</param>
        internal PropertyObjectCreator(MethodInfo method)
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
            var value = method.Invoke(null, null);
#if AOT
            return value != null ? ConfigObject.CreateMethod.MakeGenericMethod(method.ReturnType).Invoke(null, new object[] { value }).notNullCastType<ConfigObject>() : null;
#else
            return value != null ? AutoCSer.Metadata.ClassGenericType.Get(method.ReturnType).CreateConfigObject(value) : null;
#endif
        }
    }
}
