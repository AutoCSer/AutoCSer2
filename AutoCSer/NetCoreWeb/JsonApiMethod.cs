using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 方法信息
    /// </summary>
    internal sealed class JsonApiMethod
    {
        /// <summary>
        /// 方法信息
        /// </summary>
        internal readonly MethodInfo Method;
        /// <summary>
        /// JSON API 自定义配置
        /// </summary>
        internal readonly JsonApiAttribute Attribute;
        /// <summary>
        /// ResponseResult{T} 返回值类型 T
        /// </summary>
        private readonly Type resultType;
        /// <summary>
        /// 参数信息集合
        /// </summary>
        internal readonly ParameterInfo[] Parameters;
        /// <summary>
        /// 控制器方法参数信息
        /// </summary>
        internal HttpMethodParameter[] HttpMethodParameters;
        /// <summary>
        /// JSON API 请求实例父类型
        /// </summary>
        internal Type ParentType
        {
            get
            {
                switch (type)
                {
                    case JsonApiMethodTypeEnum.ResponseState: return typeof(JsonApiResultRequest);
                    case JsonApiMethodTypeEnum.ResponseResult: return typeof(JsonApiResultRequest<>).MakeGenericType(resultType);
                }
                return null;
            }
        }
        /// <summary>
        /// 帮助文档返回值类型
        /// </summary>
        internal Type HelpReturnType
        {
            get
            {
                switch (type)
                {
                    case JsonApiMethodTypeEnum.ResponseState: return typeof(ResponseResult);
                    case JsonApiMethodTypeEnum.ResponseResult: return typeof(ResponseResult<>).MakeGenericType(resultType);
                }
                return null;
            }
        }
        /// <summary>
        /// 请求路由路径
        /// </summary>
        internal string RoutePath;
        /// <summary>
        /// 是否生成 API 调用
        /// </summary>
        internal bool IsApi;
        /// <summary>
        /// JSON API 方法类型
        /// </summary>
        private readonly JsonApiMethodTypeEnum type;
        /// <summary>
        /// JSON API 方法信息
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="attribute">JSON API 自定义配置</param>
        /// <param name="parameters">参数信息集合</param>
        /// <param name="type">JSON API 方法类型</param>
        internal JsonApiMethod(MethodInfo method, JsonApiAttribute attribute, ParameterInfo[] parameters, JsonApiMethodTypeEnum type)
        {
            Method = method;
            Parameters = parameters;
            Attribute = attribute ?? JsonApiAttribute.Default;
            resultType = null;
            this.type = type;
            IsApi = true;
        }
        /// <summary>
        /// JSON API 方法信息
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="attribute">JSON API 自定义配置</param>
        /// <param name="parameters">参数信息集合</param>
        /// <param name="resultType">ResponseResult{T} 返回值类型 T</param>
        internal JsonApiMethod(MethodInfo method, JsonApiAttribute attribute, ParameterInfo[] parameters, Type resultType)
        {
            Method = method;
            Parameters = parameters;
            Attribute = attribute ?? JsonApiAttribute.Default;
            this.resultType = resultType;
            type = JsonApiMethodTypeEnum.ResponseResult;
            IsApi = true;
        }
    }
}
