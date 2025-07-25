﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.DeployTask;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.FileSynchronousClient
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IPullFileClientSocketEvent, IUploadFileClientSocketEvent
    {
        /// <summary>
        /// 拉取文件客户端接口
        /// </summary>
        public IPullFileServiceClientController PullFileClient { get; private set; }
        /// <summary>
        /// 文件上传客户端接口
        /// </summary>
        public IUploadFileServiceClientController UploadFileClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IPullFileService), typeof(IPullFileServiceClientController));
                yield return new CommandClientControllerCreatorParameter(typeof(IUploadFileService), typeof(IUploadFileServiceClientController));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
    }
}
