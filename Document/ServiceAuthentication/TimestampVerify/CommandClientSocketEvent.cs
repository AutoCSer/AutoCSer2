using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoCSer.Document.ServiceAuthentication.TimestampVerify
{
    /// <summary>
    /// 无身份认证（字符串匹配认证）命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.CommandService.TimestampVerifyCommandClientSocketEvent<CommandClientSocketEvent>
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        [AllowNull]
        public ITestServiceClientController TestService { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                foreach (AutoCSer.Net.CommandClientControllerCreatorParameter creatorParameter in base.ControllerCreatorParameters)
                {
                    yield return creatorParameter;
                }
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(ITestService), typeof(ITestServiceClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client, string verifyString) : base(client, verifyString) { }

        /// <summary>
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        });
        /// <summary>
        /// 无身份认证（字符串匹配认证）客户端测试
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Test()
        {
            CommandClientSocketEvent? client = await CommandClient.SocketEvent.Wait();
            if (client == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            var result = await client.TestService.Add(1, 2);
            if (result.Value != 1 + 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
