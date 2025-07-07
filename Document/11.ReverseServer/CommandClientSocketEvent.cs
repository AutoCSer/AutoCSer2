using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ReverseServer
{
    /// <summary>
    /// RPC client instance
    /// RPC 客户端实例
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyReverseServiceCommandClientSocketEvent<CommandClientSocketEvent>
    {
        /// <summary>
        /// Reverse service authentication client interface based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的反向服务认证客户端接口
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.ITimestampVerifyReverseServiceClientController<string> TimestampVerifyReverseClient { get; private set; }
        /// <summary>
        /// The client defines an asymmetric test interface (socket context binding the server)
        /// 客户端定义非对称测试接口（套接字上下文绑定服务端）
        /// </summary>
        [AllowNull]
        public ISymmetryService SymmetryClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.ITimestampVerifyReverseService<string>), typeof(AutoCSer.CommandService.ITimestampVerifyReverseServiceClientController<string>));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(ISymmetryService));
            }
        }
        /// <summary>
        /// RPC client instance
        /// RPC 客户端实例
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
        /// <summary>
        /// The reverse command server client listens for the authentication socket
        /// 反向命令服务客户端监听验证套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override Task<bool> CallVerify(AutoCSer.Net.CommandClientSocket socket)
        {
            return callVerify(socket, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
        }

        /// <summary>
        /// Client listener singleton
        /// 客户端监听单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>.Create(new AutoCSer.Net.CommandReverseListenerConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
        /// The client test
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        public static async Task Test()
        {
            var client = await CommandClientSocketEvent.CommandClient.SocketEvent.Wait();
            if (client != null)
            {
                Console.WriteLine($"2 + 3 = {await client.SymmetryClient.AddAsync(2, 3)}");
                Console.WriteLine($"1 + 2 = {client.SymmetryClient.Add(1, 2)}");
            }
        }
    }
}
