using System;
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
        /// <summary>
        /// Command client socket event controller property binding identification, Defaults to the current type only define attributes BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly
        /// 命令客户端套接字事件控制器属性绑定标识，默认为仅当前类型定义属性 BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly 
        /// </summary>
        public BindingFlags ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        /// <summary>
        /// Gets the command client socket event delegate
        /// 获取命令客户端套接字事件委托
        /// </summary>
#if NetStandard21
        public Func<ICommandClient, CommandClientSocketEvent>? GetSocketEventDelegate;
#else
        public Func<ICommandClient, CommandClientSocketEvent> GetSocketEventDelegate;
#endif
        /// <summary>
        /// Gets the command client socket event, which defaults to new CommandClientSocketEvent(commandClient) and is called once upon client initialization
        /// 获取命令客户端套接字事件，默认为 new CommandClientSocketEvent(commandClient)，客户端初始化时一次性调用
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public virtual CommandClientSocketEvent GetSocketEvent(CommandReverseClient commandClient)
        {
            if (GetSocketEventDelegate != null) return GetSocketEventDelegate(commandClient);
            return new CommandClientSocketEvent(commandClient);
        }
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
    }
}
