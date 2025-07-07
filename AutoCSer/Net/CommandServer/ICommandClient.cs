using AutoCSer.Memory;
using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client interface
    /// 命令客户端接口
    /// </summary>
    public interface ICommandClient : IDisposable
    {
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// The server listens to host and port information
        /// 服务监听主机与端口信息
        /// </summary>
        HostEndPoint Host { get; }
        /// <summary>
        /// The service name is a unique identifier of the server registration. If the server registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        string? ServerName { get; }
#else
        string ServerName { get; }
#endif
        /// <summary>
        /// Log processing instance
        /// 日志处理实例
        /// </summary>
        ILog Log { get; }
        /// <summary>
        /// Command client socket event controller property binding identifier
        /// 命令客户端套接字事件控制器属性绑定标识
        /// </summary>
        BindingFlags ControllerCreatorBindingFlags { get; }
        /// <summary>
        /// Wait for the server listen address
        /// 等待服务监听地址
        /// </summary>
        /// <returns>Whether to cancel a scheduled task
        /// 是否需要取消定时任务</returns>
#if NetStandard21
        ValueTask<bool> WaitServerEndPoint();
#else
        Task<bool> WaitServerEndPoint();
#endif
        /// <summary>
        /// The server listens for address update notifications
        /// 服务端监听地址更新通知
        /// </summary>
        /// <param name="endPoint"></param>
        void ServerEndPointChanged(IPEndPoint endPoint);
#if !AOT
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure</returns>
#if NetStandard21
        Task<CommandClientSocketEvent?> GetSocketEvent();
#else
        Task<CommandClientSocketEvent> GetSocketEvent();
#endif
#endif
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Return null on failure</returns>
#if NetStandard21
        Task<T?> GetSocketEvent<T>() where T : CommandClientSocketEvent;
#else
        Task<T> GetSocketEvent<T>() where T : CommandClientSocketEvent;
#endif
        /// <summary>
        /// Command client socket event
        /// 命令客户端套接字事件
        /// </summary>
        CommandClientSocketEvent SocketEvent { get; }
        /// <summary>
        /// Determines whether the current client socket is closed
        /// 判断当前客户端套接字是否已经关闭
        /// </summary>
        bool IsSocketClosed { get; }
        /// <summary>
        /// Gets the send data cache pool
        /// 获取发送数据缓存区池
        /// </summary>
        /// <returns>Send data cache pool
        /// 发送数据缓存区池</returns>
        ByteArrayPool GetSendBufferPool();
    }
}
