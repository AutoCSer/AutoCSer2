using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    public sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
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
        /// 客户端控制器创建器参数集合
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
        /// <summary>
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
