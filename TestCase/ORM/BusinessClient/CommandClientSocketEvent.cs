using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    public sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// Sample interface of service authentication client based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public AutoCSer.CommandService.ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 自增ID与其它混合测试模型业务数据服务客户端接口
        /// </summary>
        public IAutoIdentityModelClient AutoIdentityModelClient { get; private set; }
        /// <summary>
        /// 字段测试模型业务数据服务客户端接口
        /// </summary>
        public IFieldModelClient FieldModelClient { get; private set; }
        /// <summary>
        /// 属性测试模型业务数据服务客户端接口
        /// </summary>
        public IPropertyModelClient PropertyModelClient { get; private set; }
        /// <summary>
        /// 自定义字段列测试模型业务数据服务客户端接口
        /// </summary>
        public ICustomColumnFieldModelClient CustomColumnFieldModelClient { get; private set; }
        /// <summary>
        /// 自定义属性列测试模型业务数据服务客户端接口
        /// </summary>
        public ICustomColumnPropertyModelClient CustomColumnPropertyModelClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.ITimestampVerifyService), typeof(AutoCSer.CommandService.ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(IAutoIdentityModelClient));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(IFieldModelClient));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(IPropertyModelClient));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(ICustomColumnFieldModelClient));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(ICustomColumnPropertyModelClient));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(AutoCSer.CommandService.TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }
    }
}
