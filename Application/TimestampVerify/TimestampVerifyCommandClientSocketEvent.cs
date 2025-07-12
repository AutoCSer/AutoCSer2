using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Service authentication client socket events based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的服务认证客户端套接字事件
    /// </summary>
    public class TimestampVerifyCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// Server authentication verification string
        /// 服务认证验证字符串
        /// </summary>
        private readonly string verifyString;
        /// <summary>
        /// Sample interface of service authentication client based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
            }
        }
        /// <summary>
        /// Service authentication client socket events based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        public TimestampVerifyCommandClientSocketEvent(CommandClient client, string verifyString) : base(client)
        {
            this.verifyString = verifyString;
        }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum>> CallVerifyMethod(AutoCSer.Net.CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, verifyString));
        }
    }
    /// <summary>
    /// Service authentication client socket events based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的服务认证客户端套接字事件
    /// </summary>
    /// <typeparam name="T">Command client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public abstract class TimestampVerifyCommandClientSocketEvent<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
        T> : AutoCSer.Net.CommandClientSocketEventTask<T>
        where T : TimestampVerifyCommandClientSocketEvent<T>
    {
        /// <summary>
        /// Server authentication verification string
        /// 服务认证验证字符串
        /// </summary>
        private readonly string verifyString;
        /// <summary>
        /// Sample interface of service authentication client based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
            }
        }
        /// <summary>
        /// Service authentication client socket events based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        public TimestampVerifyCommandClientSocketEvent(CommandClient client, string verifyString) : base(client)
        {
            this.verifyString = verifyString;
        }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum>> CallVerifyMethod(AutoCSer.Net.CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, verifyString));
        }
    }
}
