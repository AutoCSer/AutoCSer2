using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchQueryService;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class SearchQueryCommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 搜索聚合查询服务客户端接口
        /// </summary>
        public IQueryServiceClientController SearchUserClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IQueryService), typeof(IQueryServiceClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public SearchQueryCommandClientSocketEvent(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<SearchQueryCommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<SearchQueryCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchQueryService),
            GetSocketEventDelegate = (client) => new SearchQueryCommandClientSocketEvent(client)
        });
    }
}
