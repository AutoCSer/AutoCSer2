using AutoCSer.CommandService;
using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用统计信息服务客户端套接字事件
    /// </summary>
    internal sealed class ExceptionStatisticsCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<ExceptionStatisticsCommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public ExceptionStatisticsCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }

        /// <summary>
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IExceptionStatisticsServiceNodeClientNode, ExceptionStatisticsCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IExceptionStatisticsServiceNodeClientNode, ExceptionStatisticsCommandClientSocketEvent>(new AutoCSer.Net.CommandClientCompressConfig
        {
            Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(AutoCSer.TestCase.Common.CommandServerPortEnum.ExceptionStatistics),
            GetSocketEventDelegate = (client) => new ExceptionStatisticsCommandClientSocketEvent(client)
        });
    }
}
