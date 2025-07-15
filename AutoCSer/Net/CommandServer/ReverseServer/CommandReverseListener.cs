using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Threading;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 反向命令服务客户端监听（监听连接端）
    /// </summary>
    public class CommandReverseListener : CommandListenerBase
    {
        /// <summary>
        /// 反向命令服务客户端监听配置
        /// </summary>
        internal readonly CommandReverseListenerConfig Config;
        /// <summary>
        /// 命令客户端
        /// </summary>
        public readonly CommandClient CommandClient;
        /// <summary>
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public override string? ServerName { get { return Config.ServerName; } }
#else
        public override string ServerName { get { return Config.ServerName; } }
#endif
        /// <summary>
        /// 服务监听主机与端口信息
        /// </summary>
        public override HostEndPoint Host { get { return Config.Host; } }
        /// <summary>
        /// 启动服务监听访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock startLock;
        ///// <summary>
        ///// 接受数据缓存区池
        ///// </summary>
        //internal readonly ByteArrayPool ReceiveBufferPool;
        ///// <summary>
        ///// 发送数据缓存区池
        ///// </summary>
        //internal readonly ByteArrayPool SendBufferPool;
        ///// <summary>
        ///// 二进制反序列化配置参数
        ///// </summary>
        //internal readonly AutoCSer.BinarySerialize.DeserializeConfig BinaryDeserializeConfig;
        ///// <summary>
        ///// 客户端控制器创建器集合
        ///// </summary>
        //internal LeftArray<CommandClientInterfaceControllerCreator> ControllerCreators;
//        /// <summary>
//        /// 客户端操作锁
//        /// </summary>
//#if DEBUG && NetStandard21
//        [AllowNull]
//#endif
//        private readonly SemaphoreSlimLock clientLock;
//        /// <summary>
//        /// 等待客户端锁集合
//        /// </summary>
//        private LeftArray<System.Threading.SemaphoreSlim> clientWaitLocks;
        /// <summary>
        /// 反向命令服务客户端监听（监听连接端）
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        /// <param name="creators">客户端控制器创建器集合</param>
        public CommandReverseListener(CommandReverseListenerConfig config, params CommandClientInterfaceControllerCreator[] creators)
        {
            this.Config = config;
            //ReceiveBufferPool = ByteArrayPool.GetPool(config.ReceiveBufferSizeBits);
            //SendBufferPool = ByteArrayPool.GetPool(config.SendBufferSizeBits);
            //BinaryDeserializeConfig = config.GetBinaryDeserializeConfig();
            //ControllerCreators.SetEmpty();
            //clientLock = new SemaphoreSlimLock(1, 1);
            //clientWaitLocks = new LeftArray<SemaphoreSlim>(0);
            //if (creators?.Length > 0) CommandClient.AppendCreators(creators, ref ControllerCreators);
            CommandClient = new CommandClient(this, creators);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        protected override void dispose()
        {
            base.dispose();
            CommandClient.Dispose();

            //clientLock.Enter();
            //try
            //{
            //    releaseClientWaitLock();
            //}
            //finally
            //{
            //    clientLock.Exit();
            //    Interlocked.Exchange(ref currentClient, null)?.Dispose();
            //}
        }
        /// <summary>
        /// 启动服务监听
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Start()
        {
            startLock.Enter();
            try
            {
                if (!IsStart && !IsDisposed)
                {
                    startLock.SleepFlag = 1;

                    serviceRegistrar = await Config.GetRegistrar(this);
                    AutoCSer.Net.CommandServer.HostEndPoint endPoint = await serviceRegistrar.GetEndPoint();
                    socket = new Socket(endPoint.IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //if (Config.IsTcpFastOpen) socket.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)15, true);
                    socket.Bind(endPoint.IPEndPoint);
                    listenAcceptEvent = new SocketAsyncEventArgs();
                    listenAcceptEvent.Completed += listenAcceptCompleted;
                    socket.Listen(int.MaxValue);
                    IsStart = true;
                    if (!socket.AcceptAsync(listenAcceptEvent)) listenAcceptCompleted(null, listenAcceptEvent);
                    //AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(acceptSocket);
                    await serviceRegistrar.OnListened(endPoint.RegisterHost);
                    return true;
                }
            }
            catch(Exception exception)
            {
                await Config.Log.Exception(exception);
            }
            finally { startLock.ExitSleepFlag(); }
            return false;
        }
        /// <summary>
        /// 获取客户端请求套接字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listenAcceptEvent"></param>
#if NetStandard21
        private void listenAcceptCompleted(object? sender, SocketAsyncEventArgs listenAcceptEvent)
#else
        private void listenAcceptCompleted(object sender, SocketAsyncEventArgs listenAcceptEvent)
#endif
        {
            Socket listenSocket = socket;
            var serverSocket = default(Socket);
            do
            {
                try
                {
                    while (listenAcceptEvent.SocketError == SocketError.Success)
                    {
                        serverSocket = listenAcceptEvent.AcceptSocket.notNull();
                        onSocket(serverSocket);
                        serverSocket = null;
                        if (isSocketDisposed) return;
                        listenAcceptEvent.AcceptSocket = null;
                        if (listenSocket.AcceptAsync(listenAcceptEvent)) return;
                    }
                    if (isSocketDisposed) return;
                    AutoCSer.LogHelper.ErrorIgnoreException(listenAcceptEvent.SocketError.ToString(), LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                }
                catch (Exception exception)
                {
                    if (isSocketDisposed) return;
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    if (serverSocket != null) serverSocket.Dispose();
                }
                System.Threading.Thread.Sleep(1);
            }
            while (!isSocketDisposed);
        }
        /// <summary>
        /// 新的套接字连接处理
        /// </summary>
        /// <param name="socket"></param>
        private void onSocket(Socket socket)
        {
            bool isClient = false;
            try
            {
                if (CommandClient.IsSocketClosed && verify(socket))//只支持一个连接
                {
                    CommandClientSocket commandClientSocket = new CommandClientSocket(CommandClient, socket);
                    isClient = true;
                    start(commandClientSocket).Catch();
                }
            }
            catch(Exception exception)
            {
                Config.Log.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isClient) socket.Dispose();
            }
        }
        /// <summary>
        /// 套接字验证，一般用于验证 IP 地址，默认返回 true
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected virtual bool verify(Socket socket)
        {
            return true;
        }
        /// <summary>
        /// 反向客户端启动操作
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task start(CommandClientSocket socket)
        {
            if (await socket.Start())
            {
                bool isVerify = false;
                try
                {
                    isVerify = !isSocketDisposed && await CommandClient.SocketEvent.CallVerify(socket) && await socket.OnVerify(0);
                }
                finally
                {
                    if (!isVerify) socket.Close();
                }
                if (isVerify) await onVerified();
            }
        }
        /// <summary>
        /// 客户端验证完成处理
        /// </summary>
        /// <returns></returns>
        protected virtual Task onVerified()
        {
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Gets the command client socket event
        /// 获取命令客户端套接字事件
        /// </summary>
        /// <returns>Return null on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<CommandClientSocketEvent?> GetSocketEvent()
#else
        public Task<CommandClientSocketEvent> GetSocketEvent()
#endif
        {
            return CommandClient.GetSocketEvent();
        }
        //        /// <summary>
        //        /// 当前客户端
        //        /// </summary>
        //#if NetStandard21
        //        private CommandClient? currentClient;
        //#else
        //        private CommandClient currentClient;
        //#endif
        ///// <summary>
        ///// 反向客户端启动操作
        ///// </summary>
        ///// <param name="client"></param>
        ///// <returns></returns>
        //private async Task start(CommandClient client)
        //{
        //    if (await client.Start())
        //    {
        //        bool isVerify = false;
        //        try
        //        {
        //            isVerify = !isSocketDisposed && await verify(client);
        //        }
        //        finally
        //        {
        //            if (!isVerify) client.Dispose();
        //        }
        //        if (isVerify) await setCommandClient(client);
        //    }
        //}
        ///// <summary>
        ///// 反向客户端验证，用于验证客户端的合法性，默认返回 true
        ///// </summary>
        ///// <param name="client"></param>
        ///// <returns></returns>
        //protected virtual Task<bool> verify(CommandClient client)
        //{
        //    return AutoCSer.Common.GetCompletedTask(true);
        //}
        //        /// <summary>
        //        /// 设置当前客户端
        //        /// </summary>
        //        /// <param name="client"></param>
        //        /// <returns></returns>
        //        protected virtual async ValueTask setCommandClient(CommandClient client)
        //        {
        //            var closeClient = Interlocked.Exchange(ref currentClient, client);
        //            await clientLock.EnterAsync();
        //            try
        //            {
        //                releaseClientWaitLock();
        //            }
        //            finally
        //            {
        //                clientLock.Exit();
        //                closeClient?.Dispose();
        //            }
        //        }
        //        /// <summary>
        //        /// 释放等待客户端锁，必须在客户端锁操作中调用
        //        /// </summary>
        //        private void releaseClientWaitLock()
        //        {
        //            foreach (System.Threading.SemaphoreSlim socketWaitLock in clientWaitLocks) socketWaitLock.Release();
        //            clientWaitLocks.SetEmpty();
        //        }
        //        /// <summary>
        //        /// 获取客户端
        //        /// </summary>
        //        /// <returns></returns>
        //#if NetStandard21
        //        public async Task<CommandClient?> GetCommandClient()
        //#else
        //        public async Task<CommandClient> GetCommandClient()
        //#endif
        //        {
        //            if (currentClient == null)
        //            {
        //                if (IsStart && !IsDisposed)
        //                {
        //                    var waitLock = default(System.Threading.SemaphoreSlim);
        //                    await clientLock.EnterAsync();
        //                    if (!IsDisposed)
        //                    {
        //                        try
        //                        {
        //                            waitLock = new System.Threading.SemaphoreSlim(0, 1);
        //                            clientWaitLocks.Add(waitLock);
        //                        }
        //                        finally { clientLock.Exit(); }
        //                        await waitLock.WaitAsync();
        //                    }
        //                    else clientLock.Exit();
        //                }
        //            }
        //            return !IsDisposed ? currentClient : null;
        //        }
    }
}
