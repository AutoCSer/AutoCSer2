using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchDataSource;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class DataSourceCommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 用户信息客户端接口
        /// </summary>
        public IUserServiceClientController UserClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IUserService), typeof(IUserServiceClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public DataSourceCommandClientSocketEvent(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<DataSourceCommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<DataSourceCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchDataSource),
            GetSocketEventDelegate = (client) => new DataSourceCommandClientSocketEvent(client)
        });
    }
}
