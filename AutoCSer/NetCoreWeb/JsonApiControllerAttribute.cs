using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 代理控制器自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonApiControllerAttribute : Attribute
    {
        /// <summary>
        /// 控制器路由，默认为 null 表示 命名空间+类型名称，设置为空字符串则表示无控制器路由
        /// </summary>
#if NetStandard21
        public string? Route;
#else
        public string Route;
#endif
        /// <summary>
        /// 默认为 true 表示当参数没有配置 AutoCSer.NetCoreWeb.ParameterConstraintAttribute 时按照默认约束处理（IsParameterConstraint / IsEmpty / IsDefault），设置为 false 则仅检查 AutoCSer.NetCoreWeb.IParameterConstraint 约束
        /// </summary>
        public bool IsDefaultParameterConstraint = true;
        /// <summary>
        /// 默认为 false 表示每一个请求使用不同实例，设置为 true 则同一个 API 的所有请求使用同一个单例实例
        /// </summary>
        public bool IsSingleton;
        /// <summary>
        /// 是否检查代理控制器 API 方法调用，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权
        /// </summary>
        public bool IsCheckRequest;
        /// <summary>
        /// 默认为 false 表示 API 不需要指定方法配置，设置为 true 则需要配置 [AutoCSer.NetCoreWeb.JsonApiAttribute]
        /// </summary>
        public bool IsMethodAttribute;
        /// <summary>
        /// 默认为 true 表示生成帮助文档视图数据信息
        /// </summary>
        public bool IsHelp = true;
        /// <summary>
        /// JSON API 代理控制器自定义配置
        /// </summary>
        /// <param name="route">控制器路由，默认为 null 表示 命名空间+类型名称，设置为空字符串则表示无控制器路由</param>
        /// <param name="isDefaultParameterConstraint">默认为 true 表示当参数没有配置 AutoCSer.NetCoreWeb.ParameterConstraintAttribute 时按照默认约束处理（IsParameterConstraint / IsEmpty / IsDefault），设置为 false 则仅检查 AutoCSer.NetCoreWeb.IParameterConstraint 约束</param>
        /// <param name="isSingleton">默认为 false 表示每一个请求使用不同实例，设置为 true 则同一个 API 的所有请求使用同一个单例实例</param>
        /// <param name="isCheckRequest">是否检查代理控制器 API 方法调用，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权</param>
        /// <param name="isMethodAttribute">默认为 false 表示 API 不需要指定方法配置，设置为 true 则需要配置 [AutoCSer.NetCoreWeb.JsonApiAttribute]</param>
        /// <param name="isHelp">默认为 true 表示生成帮助文档视图数据信息</param>
#if NetStandard21
        public JsonApiControllerAttribute(string? route = null, bool isDefaultParameterConstraint = true, bool isSingleton = false, bool isCheckRequest = false, bool isMethodAttribute = false, bool isHelp = true)
#else
        public JsonApiControllerAttribute(string route = null, bool isDefaultParameterConstraint = true, bool isSingleton = false, bool isCheckRequest = false, bool isMethodAttribute = false, bool isHelp = true)
#endif
        {
            Route = route;
            IsDefaultParameterConstraint = isDefaultParameterConstraint;
            IsSingleton = isSingleton;
            IsCheckRequest = isCheckRequest;
            IsMethodAttribute = isMethodAttribute;
            IsHelp = isHelp;
        }

        /// <summary>
        /// JSON API 代理控制器自定义配置
        /// </summary>
        internal static readonly JsonApiControllerAttribute Default = new JsonApiControllerAttribute();
    }
}
