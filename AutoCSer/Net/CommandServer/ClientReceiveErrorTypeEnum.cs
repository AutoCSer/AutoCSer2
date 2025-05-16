using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端接收数据错误类型
    /// </summary>
    public enum ClientReceiveErrorTypeEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异常
        /// </summary>
        Exception,
        /// <summary>
        /// 缺少验证函数逻辑，需要重载实现 AutoCSer.Net.CommandServerAttribute.ClientVerifyMethod
        /// </summary>
        LessVerifyMethod,
        /// <summary>
        /// 命令控制器查询回调数据解析失败
        /// </summary>
        ControllerDataError,
        /// <summary>
        /// 回调标识接收不足
        /// </summary>
        CallbackIdentityLess,
        /// <summary>
        /// 回调标识解析失败
        /// </summary>
        CallbackIdentityError,
        /// <summary>
        /// 数据长度解析错误
        /// </summary>
        DataSizeError,
        /// <summary>
        /// 接收数据不足
        /// </summary>
        DataSizeLess,
        /// <summary>
        /// 临时接收数据不足
        /// </summary>
        BigDataSizeLess,
        /// <summary>
        /// 数据解码失败
        /// </summary>
        DataDecodeError,
        /// <summary>
        /// 临时数据解码失败
        /// </summary>
        BigDataDecodeError,
        /// <summary>
        /// 不允许的接收数据回调操作
        /// </summary>
        OnReceiveInvalidOperation,
        /// <summary>
        /// 自定义命令回调数据解析失败
        /// </summary>
        CustomDataError,
        /// <summary>
        /// 自定义命令处理错误
        /// </summary>
        CustomCommandError,
    }
}
