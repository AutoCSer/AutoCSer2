using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证客户端套接字事件
    /// </summary>
    public class TimestampVerifyCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// 服务认证验证字符串
        /// </summary>
        private readonly string verifyString;
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
            }
        }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        public TimestampVerifyCommandClientSocketEvent(CommandClient client, string verifyString) : base(client)
        {
            this.verifyString = verifyString;
        }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, verifyString));
        }
    }
}
