using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// The return type of the call. The space 0x80 or above is a custom return value
    /// 调用返回类型，0x80 或者以上空间为自定义返回值
    /// </summary>
    public enum CommandClientReturnTypeEnum : byte
    {
        /// <summary>
        /// Unknown status. Unexpected errors may occur
        /// 未知状态，可能出现意外错误
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// The API version has expired
        /// API 版本过期
        /// </summary>
        VersionExpired,
        /// <summary>
        /// Server-side deserialization error
        /// 服务端反序列化错误
        /// </summary>
        ServerDeserializeError,
        /// <summary>
        /// Server exception
        /// </summary>
        ServerException,
        /// <summary>
        /// The service interface has been taken offline
        /// 服务接口已下线
        /// </summary>
        ServerOffline,
        /// <summary>
        /// The server has released resources
        /// 服务端已释放资源
        /// </summary>
        ServerDisposed,

        /// <summary>
        /// It has been added to the batch command
        /// 已添加到批处理命令
        /// </summary>
        PushBatch,
        /// <summary>
        /// No socket was created
        /// 未创建套接字
        /// </summary>
        NoSocketCreated,
        /// <summary>
        /// The socket is waiting to connect
        /// 套接字等待连接中
        /// </summary>
        WaitConnect,
        /// <summary>
        /// The disconnection is being reconnected
        /// 断线重连中
        /// </summary>
        DisconnectionReconnect,
        /// <summary>
        /// Verification failed
        /// 验证失败
        /// </summary>
        VerifyError,
        /// <summary>
        /// The socket has been closed
        /// 套接字已经关闭
        /// </summary>
        SocketClosed,
        ///// <summary>
        ///// 客户端已关闭
        ///// </summary>
        //ClientDisposed,
        /// <summary>
        /// Client exception
        /// </summary>
        ClientException,
        /// <summary>
        /// Timeout
        /// </summary>
        Timeout,
        /// <summary>
        /// The client command number is beyond the range of the controller
        /// 客户端命令编号超出控制器范围
        /// </summary>
        ControllerMethodIndexError,
        /// <summary>
        /// Generate an output data error and cancel the command call
        /// 生成输出数据错误，取消命令调用
        /// </summary>
        ClientBuildError,
        /// <summary>
        /// Client deserialization error
        /// 客户端反序列化错误
        /// </summary>
        ClientDeserializeError,
        /// <summary>
        /// Cancel the keep callback notification
        /// 取消保持回调通知
        /// </summary>
        CancelKeepCallback,
        /// <summary>
        /// Unsupported queue keywords
        /// 不支持的队列关键字
        /// </summary>
        NotSupportTaskQueueKey,
        /// <summary>
        /// The callback has been released
        /// 已释放回调
        /// </summary>
        KeepCallbackDisposed,
        /// <summary>
        /// There is an unknown error on the client side. It might be that the server was not connected or the verification failed
        /// 客户端未知错误，可能是没有连接上服务器或者验证未通过
        /// </summary>
        ClientUnknown,
    }
}
