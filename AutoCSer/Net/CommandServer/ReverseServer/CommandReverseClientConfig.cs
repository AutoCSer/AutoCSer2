using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Reverse command service client configuration (initiating connection end)
    /// 反向命令服务客户端配置（发起连接端）
    /// </summary>
    public class CommandReverseClientConfig : CommandServerConfig
    {
        ///// <summary>
        ///// Command client socket event controller property binding identification, Defaults to the current type only define attributes BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly
        ///// 命令客户端套接字事件控制器属性绑定标识，默认为仅当前类型定义属性 BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly 
        ///// </summary>
        //public BindingFlags ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        ///// <summary>
        ///// A default value of false indicates that the client does not create a default initialization controller instance before verification. Setting it to true creates a default controller instance that returns an error state during client initialization to avoid the issue of null exceptions sent by instance references (at the cost of prolonging the client's initialization time)
        ///// 默认为 false 表示客户端通过验证之前不创建默认的初始化控制器实例，设置为 true 则在客户端初始化的时候创建返回错误状态的默认控制器实例以避免实例引用发送 null 异常问题（代价是会延长客户端初始化的时间）
        ///// </summary>
        //public bool IsDefaultController;
//        /// <summary>
//        /// Gets the command client socket event delegate
//        /// 获取命令客户端套接字事件委托
//        /// </summary>
//#if NetStandard21
//        public Func<ICommandClient, CommandClientSocketEvent>? GetSocketEventDelegate;
//#else
//        public Func<ICommandClient, CommandClientSocketEvent> GetSocketEventDelegate;
//#endif
        ///// <summary>
        ///// Gets the command client socket event, which defaults to new CommandClientSocketEvent(commandClient) and is called once upon client initialization
        ///// 获取命令客户端套接字事件，默认为 new CommandClientSocketEvent(commandClient)，客户端初始化时一次性调用
        ///// </summary>
        ///// <param name="commandClient"></param>
        ///// <returns></returns>
        //public virtual CommandClientSocketEvent GetSocketEvent(CommandReverseClient commandClient)
        //{
        //    if (GetSocketEventDelegate != null) return GetSocketEventDelegate(commandClient);
        //    return new CommandClientSocketEvent(commandClient);
        //}
        /// <summary>
        /// Get the service registration client listener component, which is defaulted to new AutoCSer.Net.CommandClientServiceRegistrar(commandClient) and is called all at once during client initialization
        /// 获取服务注册客户端监听组件，默认为 new AutoCSer.Net.CommandClientServiceRegistrar(commandClient)，客户端初始化时一次性调用
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public virtual Task<CommandClientServiceRegistrar> GetRegistrar(CommandReverseClient commandClient)
        {
            return Task.FromResult(new CommandClientServiceRegistrar(commandClient));
        }
        /// <summary>
        /// Used to override the connection logic after server registration is enabled
        /// 用于启用服务注册以后重写自动启动连接逻辑
        /// </summary>
        /// <param name="client"></param>
        public virtual void CreateSocket(CommandReverseClient client)
        {
            client.CreateSocket();
        }
        /// <summary>
        /// By default, the system hibernates for 10ms after the first failed socket creation, 100ms after the second failed socket creation, and 1s after the third failed socket creation. After each failed socket creation, the system hibernates for 5s
        /// 创建套接字失败重试休眠，默认第 1 次失败以后休眠 10ms，第 2 次失败以后休眠 100ms，第 3 次失败以后休眠 1s，以后每次失败都休眠 5s
        /// </summary>
        /// <param name="createErrorCount">Number of failures
        /// 失败次数</param>
        /// <returns></returns>
        public virtual Task CreateSocketSleep(int createErrorCount)
        {
            switch (createErrorCount)
            {
                case 0: return AutoCSer.Common.CompletedTask;
                case 1: return Task.Delay(10);
                case 2: return Task.Delay(100);
                case 3: return Task.Delay(1000);
                default: return Task.Delay(5000);
            }
        }
        /// <summary>
        /// Socket retry connection successful prompt
        /// 套接字重试连接成功提示
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="serverEndPoint"></param>
        /// <param name="exceptionCount">Number of abnormal errors
        /// 异常错误次数</param>
        /// <returns></returns>
#if NetStandard21
        public virtual Task OnCreateSocketRetrySuccess(CommandClientSocket? socket, IPEndPoint serverEndPoint, int exceptionCount)
#else
        public virtual Task OnCreateSocketRetrySuccess(CommandClientSocket socket, IPEndPoint serverEndPoint, int exceptionCount)
#endif
        {
            if (exceptionCount != 0)
            {
                return Log.Debug(ServerName + " 反向命令服务客户端 TCP 连接成功 " + serverEndPoint.ToString(), LogLevelEnum.Debug | LogLevelEnum.AutoCSer);
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Socket creation exception prompt
        /// 套接字创建异常提示
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="exception"></param>
        /// <param name="serverEndPoint"></param>
        /// <param name="exceptionCount">Number of abnormal errors
        /// 异常错误次数</param>
        /// <returns></returns>
#if NetStandard21
        public virtual Task OnCreateSocketException(CommandClientSocket? socket, Exception exception, IPEndPoint serverEndPoint, int exceptionCount)
#else
        public virtual Task OnCreateSocketException(CommandClientSocket socket, Exception exception, IPEndPoint serverEndPoint, int exceptionCount)
#endif
        {
            if (exceptionCount == 1) return Log.Exception(exception, ServerName + " 反向命令服务客户端 TCP 连接失败 " + serverEndPoint.ToString(), LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
