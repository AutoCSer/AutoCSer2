using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ReverseServer
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyReverseServiceCommandClientSocketEvent<CommandClientSocketEvent>
    {
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证客户端接口
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.ITimestampVerifyReverseServiceClientController<string> TimestampVerifyReverseClient { get; private set; }
        /// <summary>
        /// 客户端定义非对称测试接口（套接字上下文绑定服务端）
        /// </summary>
        [AllowNull]
        public ISymmetryService SymmetryClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
        /// <summary>
        /// 反向命令服务客户端监听验证套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override Task<bool> CallVerify(AutoCSer.Net.CommandClientSocket socket)
        {
            return callVerify(socket, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
        }

        /// <summary>
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>.Create(new AutoCSer.Net.CommandReverseListenerConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });

        /// <summary>
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
