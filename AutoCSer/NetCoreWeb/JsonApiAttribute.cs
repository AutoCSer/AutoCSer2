using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonApiAttribute : Attribute
    {
        /// <summary>
        /// 路由模板字符串，默认为 null 表示方法名称（一般用于 POST 请求），设置为空字符串则表示 GET 请求所有参数，/ 开始表示忽略代理控制器路由，非模板参数路由不允许包含 {} 这两个符号，模板参数必须放在最后，可以表示为 /{参数名称} 或者 {参数名称}
        /// </summary>
        public string Template;
        /// <summary>
        /// 单例实例配置，默认由控制器配置 JsonApiControllerAttribute.IsSingleton 决定
        /// </summary>
        public JsonApiSingletonEnum SingletonEnum;
        /// <summary>
        /// 调用请求检查配置，默认由控制器配置 AutoCSer.NetCoreWeb.JsonApiControllerAttribute.IsCheckRequest 决定
        /// </summary>
        public JsonApiCheckRequestEnum CheckRequestEnum;
        /// <summary>
        /// 默认为 false 表示支持 GET 请求，否则仅支持 POST 请求
        /// </summary>
        public bool OnlyPost;
        /// <summary>
        /// 默认为 false 表示输出 JSON，设置为 true 表示数据视图客户端请求返回 JavaScript 代码可以降低序列化开销
        /// </summary>
        public bool IsResponseJavaScript;
        /// <summary>
        /// 默认为 true 表示检查来源页面，用于跨域验证
        /// </summary>
        public bool CheckReferer = true;
        /// <summary>
        /// 默认为 false 表示不记录请求信息日志，设置为 true 则记录请求信息日志（比如写操作接口可能需要记录日志）
        /// </summary>
        public bool IsLog;
        /// <summary>
        /// 默认为 true 表示生成帮助文档视图数据信息，控制器生成帮助文档视图数据信息时有效
        /// </summary>
        public bool IsHelp = true;
        /// <summary>
        /// 默认为 false 表示不缓存 GET 返回数据，设置为 true 则根据静态版本信息检查结果缓存 GET 返回数据
        /// </summary>
        public bool IsStaticVersion;
        /// <summary>
        /// 最大 POST 字节数，默认为 1MB
        /// </summary>
        public int MaxContentLength = 1 << 20;
        /// <summary>
        /// JSON API 自定义配置
        /// </summary>
        /// <param name="template">路由模板字符串，默认为 null 表示方法名称（一般用于 POST 请求），设置为空字符串则表示 GET 请求所有参数，/ 开始表示忽略代理控制器路由，非模板参数路由不允许包含 {} 这两个符号，模板参数必须放在最后，可以表示为 /{参数名称} 或者 {参数名称}</param>
        /// <param name="singletonEnum">单例实例配置，默认由控制器配置 JsonApiControllerAttribute.IsSingleton 决定</param>
        /// <param name="checkRequestEnum">调用请求检查配置，默认由控制器配置 AutoCSer.NetCoreWeb.JsonApiControllerAttribute.IsCheckRequest 决定</param>
        /// <param name="onlyPost">默认为 false 表示支持 GET 请求，否则仅支持 POST 请求</param>
        /// <param name="isResponseJavaScript">默认为 false 表示输出 JSON，设置为 true 表示数据视图客户端请求返回 JavaScript 代码可以降低序列化开销</param>
        /// <param name="checkReferer">默认为 true 表示检查来源页面，用于跨域验证</param>
        /// <param name="isLog">默认为 false 表示不记录请求信息日志，设置为 true 则记录请求信息日志（比如写操作接口可能需要记录日志）</param>
        /// <param name="isStaticVersion">默认为 false 表示不缓存 GET 返回数据，设置为 true 则根据静态版本信息检查结果缓存 GET 返回数据</param>
        /// <param name="isHelp">默认为 true 表示生成帮助文档视图数据信息，控制器生成帮助文档视图数据信息时有效</param>
        /// <param name="maxContentLength">最大 POST 字节数，默认为 1MB</param>
        public JsonApiAttribute(string template = null, JsonApiSingletonEnum singletonEnum = JsonApiSingletonEnum.Controller, JsonApiCheckRequestEnum checkRequestEnum = JsonApiCheckRequestEnum.Controller
            , bool onlyPost = false, bool isResponseJavaScript = false, bool checkReferer = true, bool isLog = false, bool isStaticVersion = false, bool isHelp = true, int maxContentLength = 1 << 20)
        {
            Template = template;
            SingletonEnum = singletonEnum;
            CheckRequestEnum = checkRequestEnum;
            OnlyPost = onlyPost;
            IsResponseJavaScript = isResponseJavaScript;
            CheckReferer = checkReferer;
            IsLog = isLog;
            IsStaticVersion = isStaticVersion;
            IsHelp = isHelp;
            MaxContentLength = maxContentLength;
        }

        /// <summary>
        /// 默认 JSON API 自定义配置
        /// </summary>
        internal static readonly JsonApiAttribute Default = new JsonApiAttribute();
    }
}
