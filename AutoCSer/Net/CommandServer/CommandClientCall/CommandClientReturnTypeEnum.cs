using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 返回值类型，0x80 或者以上空间为自定义返回值
    /// </summary>
    public enum CommandClientReturnTypeEnum : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 版本过期
        /// </summary>
        VersionExpired,
        /// <summary>
        /// 服务器端反序列化错误
        /// </summary>
        ServerDeserializeError,
        /// <summary>
        /// 服务器端异常
        /// </summary>
        ServerException,
        /// <summary>
        /// 服务接口已下线
        /// </summary>
        ServerOffline,
        /// <summary>
        /// 服务端已释放资源
        /// </summary>
        ServerDisposed,

        /// <summary>
        /// 套接字已经关闭
        /// </summary>
        SocketClosed,
        ///// <summary>
        ///// 客户端已关闭
        ///// </summary>
        //ClientDisposed,
        /// <summary>
        /// 客户端异常
        /// </summary>
        ClientException,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout,
        /// <summary>
        /// 客户端命令超出控制器范围
        /// </summary>
        ControllerMethodIndexError,
        /// <summary>
        /// 创建输出错误取消命令调用
        /// </summary>
        ClientBuildError,
        /// <summary>
        /// 客户端反序列化错误
        /// </summary>
        ClientDeserializeError,
        /// <summary>
        /// 取消保持回调通知
        /// </summary>
        CancelKeepCallback,
        /// <summary>
        /// 不支持的队列关键字
        /// </summary>
        NotSupportTaskQueueKey,
        /// <summary>
        /// 已释放回调
        /// </summary>
        KeepCallbackDisposed,
        /// <summary>
        /// 客户端未知错误，可能是没有连接上服务器或者验证未通过
        /// </summary>
        ClientUnknown,
    }
}
