using AutoCSer.CommandService;
using AutoCSer.CommandService.DeployTask;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.FileSynchronousClient
{
    /// <summary>
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
        /// 客户端控制器创建器参数集合
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }
    }
}
