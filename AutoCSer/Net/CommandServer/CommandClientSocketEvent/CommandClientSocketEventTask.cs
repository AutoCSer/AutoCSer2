using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public abstract class CommandClientSocketEventTask<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
    T> : CommandClientSocketEvent
        where T : CommandClientSocketEventTask<T>
    {
        /// <summary>
        /// Command client socket event task caching
        /// 命令客户端套接字事件任务缓存
        /// </summary>
        private readonly Task<T> socketEventTask;
        /// <summary>
        /// The current command client socket event task
        /// 当前命令客户端套接字事件任务
        /// </summary>
#if NetStandard21
        private Task<T>? currentSocketEventTask;
#else
        private Task<T> currentSocketEventTask;
#endif
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        protected CommandClientSocketEventTask(ICommandClient client) : base(client)
        {
            socketEventTask = Task.FromResult((T)this);
        }
        /// <summary>
        /// Close the command to notify the client of the current socket. The default operation is to notify the caller waiting for the current connection. This call is located in the client lock operation. The initialization operation should be completed as soon as possible. It is prohibited to call the internal nested lock operation to avoid deadlock
        /// 关闭命令客户端当前套接字通知，默认操作为通知等待当前连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override void OnClosed(CommandClientSocket socket)
        {
            currentSocketEventTask = null;
            base.OnClosed(socket);
        }
        /// <summary>
        /// After the command client socket passes the authentication API, the client initialization operation is carried out. The default operation is to reset the current socket and automatically bind the client controller operation and notify the caller waiting to connect. This call is located in the client lock operation. The initialization operation should be completed as soon as possible. It is prohibited to call the internal nested lock operation to avoid deadlock
        /// 命令客户端套接字通过认证 API 以后的客户端初始化操作，默认操作为重置当前套接字与自动绑定客户端控制器操作并通知等待连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override Task OnMethodVerified(CommandClientSocket socket)
        {
            currentSocketEventTask = socketEventTask;
            return base.OnMethodVerified(socket);
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<T?> Wait()
#else
        public Task<T> Wait()
#endif
        {
#pragma warning disable CS8619
            return currentSocketEventTask ?? wait();
#pragma warning restore CS8619
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private Task<T?> wait()
#else
        private Task<T> wait()
#endif
        {
            return Client.GetSocketEvent<T>();
        }
    }
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    /// <typeparam name="CT">The interface type of the client's main controller
    /// 客户端主控制器接口类型</typeparam>
    public abstract class CommandClientSocketEventTask<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
    T, CT> : CommandClientSocketEvent<CT>
        where T : CommandClientSocketEventTask<T, CT>
        where CT : class
    {
        /// <summary>
        /// Command client socket event task caching
        /// 命令客户端套接字事件任务缓存
        /// </summary>
        private readonly Task<T> socketEventTask;
        /// <summary>
        /// The current command client socket event task
        /// 当前命令客户端套接字事件任务
        /// </summary>
#if NetStandard21
        private Task<T>? currentSocketEventTask;
#else
        private Task<T> currentSocketEventTask;
#endif
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        protected CommandClientSocketEventTask(ICommandClient client) : base(client)
        {
            socketEventTask = Task.FromResult((T)this);
        }
        /// <summary>
        /// Close the command to notify the client of the current socket. The default operation is to notify the caller waiting for the current connection. This call is located in the client lock operation. The initialization operation should be completed as soon as possible. It is prohibited to call the internal nested lock operation to avoid deadlock
        /// 关闭命令客户端当前套接字通知，默认操作为通知等待当前连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override void OnClosed(CommandClientSocket socket)
        {
            currentSocketEventTask = null;
            base.OnClosed(socket);
        }
        /// <summary>
        /// After the command client socket passes the authentication API, the client initialization operation is carried out. The default operation is to reset the current socket and automatically bind the client controller operation and notify the caller waiting to connect. This call is located in the client lock operation. The initialization operation should be completed as soon as possible. It is prohibited to call the internal nested lock operation to avoid deadlock
        /// 命令客户端套接字通过认证 API 以后的客户端初始化操作，默认操作为重置当前套接字与自动绑定客户端控制器操作并通知等待连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override Task OnMethodVerified(CommandClientSocket socket)
        {
            currentSocketEventTask = socketEventTask;
            return base.OnMethodVerified(socket);
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<T?> Wait()
#else
        public Task<T> Wait()
#endif
        {
#pragma warning disable CS8619
            return currentSocketEventTask ?? wait();
#pragma warning restore CS8619
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private Task<T?> wait()
#else
        private Task<T> wait()
#endif
        {
            return Client.GetSocketEvent<T>();
        }
    }
}
