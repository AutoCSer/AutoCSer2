using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令服务会话对象操作接口
    /// </summary>
    public interface ICommandListenerSession { }
    /// <summary>
    /// 命令服务会话对象操作接口
    /// </summary>
    /// <typeparam name="T">指定会话对象类型</typeparam>
    public interface ICommandListenerGetSession<T> : ICommandListenerSession
    {
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        T? TryGetSessionObject(CommandServerSocket socket);
#else
        T TryGetSessionObject(CommandServerSocket socket);
#endif
    }
    /// <summary>
    /// 命令服务会话对象操作接口
    /// </summary>
    /// <typeparam name="T">指定会话对象类型</typeparam>
    public interface ICommandListenerSession<T> : ICommandListenerGetSession<T>
    {
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        T CreateSessionObject(CommandServerSocket socket);
    }
    /// <summary>
    /// 命令服务会话对象操作接口
    /// </summary>
    /// <typeparam name="T">指定会话对象类型</typeparam>
    /// <typeparam name="ST">服务实例类型</typeparam>
    public interface ICommandListenerSession<T, ST> : ICommandListenerGetSession<T>
        where ST : class
    {
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="service">服务控制器对象</param>
        /// <param name="socket"></param>
        /// <returns></returns>
        T CreateSessionObject(ST service, CommandServerSocket socket);
    }
    /// <summary>
    /// 命令服务套接字会话对象
    /// </summary>
    public class CommandServerSocketSessionObject
    {
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        public readonly CommandServerSocket CommandServerSocket;
        /// <summary>
        /// 命令服务套接字会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        protected CommandServerSocketSessionObject(CommandServerSocket socket)
        {
            CommandServerSocket = socket;
        }

        /// <summary>
        /// 默认空命令服务套接字会话对象
        /// </summary>
        internal static readonly CommandServerSocketSessionObject Null = new CommandServerSocketSessionObject(CommandServerSocket.CommandServerSocketContext);
    }
//    /// <summary>
//    /// 仅用于标识命令服务套接字自定义会话对象操作接口
//    /// </summary>
//    public interface ICommandServerSocketSessionObject { }
//    /// <summary>
//    /// 命令服务套接字自定义会话对象操作接口
//    /// </summary>
//    /// <typeparam name="T">指定会话对象类型</typeparam>
//    public interface ICommandServerSocketSessionObject<T> : ICommandServerSocketSessionObject
//    {
//        /// <summary>
//        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
//        /// </summary>
//        /// <param name="socket"></param>
//        /// <returns>失败返回 null</returns>
//#if NetStandard21
//        T? TryGetSessionObject(CommandServerSocket socket);
//#else
//        T TryGetSessionObject(CommandServerSocket socket);
//#endif
//        /// <summary>
//        /// 创建会话对象
//        /// </summary>
//        /// <param name="socket"></param>
//        /// <returns></returns>
//        T CreateSessionObject(CommandServerSocket socket);
//    }
//    /// <summary>
//    /// 命令服务套接字自定义会话对象操作接口
//    /// </summary>
//    /// <typeparam name="ST">服务控制器对象</typeparam>
//    /// <typeparam name="T">指定会话对象类型</typeparam>
//    public interface ICommandServerSocketSessionObject<ST, T> : ICommandServerSocketSessionObject
//    {
//        /// <summary>
//        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
//        /// </summary>
//        /// <param name="socket"></param>
//        /// <returns>失败返回 null</returns>
//#if NetStandard21
//        T? TryGetSessionObject(CommandServerSocket socket);
//#else
//        T TryGetSessionObject(CommandServerSocket socket);
//#endif
//        /// <summary>
//        /// 创建会话对象
//        /// </summary>
//        /// <param name="service">服务控制器对象</param>
//        /// <param name="socket"></param>
//        /// <returns></returns>
//        T CreateSessionObject(ST service, CommandServerSocket socket);
//    }
}
