using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DiskBlockClient
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IDiskBlockClientSocketEvent
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockClient DiskBlockClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IDiskBlockService), typeof(IDiskBlockClient));
            }
        }
        /// <summary>
        /// Command client socket event controller property binding identification
        /// 命令客户端套接字事件控制器属性绑定标识
        /// </summary>
        public override BindingFlags ControllerBindingFlags { get { return BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public; } }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
    }
}
