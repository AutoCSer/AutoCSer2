using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchDataSource;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.Search
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class DataSourceCommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 用户信息客户端接口
        /// </summary>
        public IUserServiceClientController UserClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
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
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public DataSourceCommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<DataSourceCommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<DataSourceCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchDataSource),
            GetSocketEventDelegate = (client) => new DataSourceCommandClientSocketEvent(client)
        });
    }
}
