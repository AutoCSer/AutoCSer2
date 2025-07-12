using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// Custom user identity authentication service authentication command client socket event
    /// 自定义用户身份鉴权服务认证命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>
    {
        /// <summary>
        /// Verify the user identity
        /// 验证用户标识
        /// </summary>
        private readonly string userName;
        /// <summary>
        /// User verification key. The user password should be prefixed and hashed
        /// 用户验证密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        private readonly string verifyKey;
        /// <summary>
        /// The service authentication client interface for customizing user identity authentication
        /// 自定义用户身份鉴权服务认证客户端接口
        /// </summary>
        [AllowNull]
        public ICustomVerifyServiceClientController CustomVerifyService { get; private set; }
        /// <summary>
        /// Test the client interface
        /// 测试客户端接口
        /// </summary>
        [AllowNull]
        public ITestServiceClientController TestService { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
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
        /// Custom user identity authentication service authentication command client socket event
        /// 自定义用户身份鉴权服务认证命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="userName">User identification to be verified
        /// 待验证用户标识</param>
        /// <param name="verifyKey">User verification key. The user password should be prefixed and hashed
        /// 用户验证密钥，用户密码应该增加前缀并哈希处理</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client, string userName, string verifyKey) : base(client)
        {
            this.userName = userName;
            this.verifyKey = verifyKey;
        }

        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override async System.Threading.Tasks.Task<AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum>> CallVerifyMethod(AutoCSer.Net.CommandClientController controller)
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
        /// Client singleton
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
