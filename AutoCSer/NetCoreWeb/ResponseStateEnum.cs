using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 返回值状态
    /// </summary>
    public enum ResponseStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异常
        /// </summary>
        Exception,
        /// <summary>
        /// 模板路由参数解析失败
        /// </summary>
        RouteParameterFail,
        /// <summary>
        /// 来源页面检测失败
        /// </summary>
        RefererNotMatch,
        /// <summary>
        /// 请求参数不符合约束
        /// </summary>
        ParameterConstraint,
        /// <summary>
        /// 没有读取到参数数据
        /// </summary>
        EmptyRequestBody,
        /// <summary>
        /// 请求参数 JSON 反序列化失败
        /// </summary>
        JsonDeserializeFail,
        /// <summary>
        /// POST 数据长度超出限制范围
        /// </summary>
        ContentLengthOutOfRange,
        /// <summary>
        /// 读取 POST 数据字节数不足 ContentLength
        /// </summary>
        ReadBodySizeError,
        /// <summary>
        /// XML 反序列化失败
        /// </summary>
        XmlDeserializeFail,
        /// <summary>
        /// HTTP 头部鉴权失败
        /// </summary>
        AccessTokenFail,
        /// <summary>
        /// 鉴权参数没有通过检查
        /// </summary>
        AccessTokenParameterFail,
        /// <summary>
        /// 仅支持 POST 请求
        /// </summary>
        OnlyPost,
        /// <summary>
        /// 不支持的 POST 请求数据类型
        /// </summary>
        NotSupportPostType,

        /// <summary>
        /// 客户端请求失败
        /// </summary>
        ClientFail = 0xff,
    }
}
