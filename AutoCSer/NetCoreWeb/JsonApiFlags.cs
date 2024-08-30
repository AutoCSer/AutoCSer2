using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 方法信息标记
    /// </summary>
    [Flags]
    internal enum JsonApiFlags : byte
    {
        /// <summary>
        /// 是否检查 WEB API 调用，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权
        /// </summary>
        IsCheckRequest = 1,
        /// <summary>
        /// 是否需要检查参数
        /// </summary>
        IsCheckParameter = 2,
        /// <summary>
        /// 是否参数鉴权
        /// </summary>
        IsAccessTokenParameter = 4,
        /// <summary>
        /// 是否存在 POST 参数
        /// </summary>
        IsPostParameter = 8,
    }
}
