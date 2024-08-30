using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 是否检查 API 方法调用，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权
    /// </summary>
    public enum JsonApiCheckRequestEnum : byte
    {
        /// <summary>
        /// 由控制器配置 AutoCSer.NetCoreWeb.JsonApiControllerAttribute.IsCheckRequest 决定
        /// </summary>
        Controller,
        /// <summary>
        /// 不检查 API 方法调用
        /// </summary>
        NotCheck,
        /// <summary>
        /// 检查 API 方法调用，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权
        /// </summary>
        Check,
    }
}
