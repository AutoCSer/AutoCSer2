using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 反向命令服务客户端监听配置（监听连接端）
    /// </summary>
    public class CommandReverseListenerConfig : CommandClientConfig
    {
        /// <summary>
        /// 套接字异步事件对象缓存数量，默认为 8
        /// </summary>
        public int SocketAsyncEventArgsMaxCount = 8;
        /// <summary>
        /// 获取服务注册组件，默认返回 new AutoCSer.Net.CommandServiceRegistrar(server)，服务初始化时一次性调用
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public virtual Task<CommandServiceRegistrar> GetRegistrar(CommandReverseListener server)
        {
            return Task.FromResult(new CommandServiceRegistrar(server));
        }
    }
}
