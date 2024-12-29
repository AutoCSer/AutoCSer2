using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// 无身份认证（字符串匹配认证）命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>
    {
        /// <summary>
        /// 验证用户标识
        /// </summary>
        private readonly string userName;
        /// <summary>
        /// 验证用户密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        private readonly string verifyKey;
        /// <summary>
        /// 自定义服务认证接口
        /// </summary>
        [AllowNull]
        public ICustomVerifyServiceClientController CustomVerifyService { get; private set; }
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
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(ICustomVerifyService), typeof(ICustomVerifyServiceClientController));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(ITestService), typeof(ITestServiceClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="userName">验证用户标识</param>
        /// <param name="verifyKey">验证用户密钥，用户密码应该增加前缀并哈希处理</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client, string userName, string verifyKey) : base(client)
        {
            this.userName = userName;
            this.verifyKey = verifyKey;
        }

        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override async Task<AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum>> CallVerifyMethod(AutoCSer.Net.CommandClientController controller)
        {
            ICustomVerifyServiceClientController client = (ICustomVerifyServiceClientController)controller;
            using (MD5 md5 = MD5.Create())
            {
                do
                {
                    CustomVerifyData verifyData = new CustomVerifyData(userName, verifyKey);
                    byte[] hashData = verifyData.GetMd5Data();
                    AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum> verifyState = await client.Verify(verifyData, hashData);
                    if (verifyState.Value != AutoCSer.Net.CommandServerVerifyStateEnum.Retry || !verifyState.IsSuccess) return verifyState;
                }
                while (true);
            }
        }

        /// <summary>
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, nameof(CustomVerifyData.UserName), AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        });
        /// <summary>
        /// 自定义服务认证客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            CommandClientSocketEvent? client = (CommandClientSocketEvent?)await CommandClient.SocketEvent.Wait();
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
