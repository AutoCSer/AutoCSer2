using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DatabaseBackupClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent, AutoCSer.CommandService.IDatabaseBackupClientSocketEvent
    {
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 数据库备份客户端接口
        /// </summary>
        public IDatabaseBackupClient DatabaseBackupClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IDatabaseBackupService), typeof(IDatabaseBackupClient));
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
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, ConfigFile.Default.VerifyString));
        }
    }
}
