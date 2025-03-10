using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class DiskBlockCommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IDiskBlockClientSocketEvent
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockClient DiskBlockClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public DiskBlockCommandClientSocketEvent(ICommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// 客户端单例（测试环境公用同一个磁盘块服务，实战环境需要物理隔离）
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<DiskBlockCommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<DiskBlockCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DiskBlock),
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            GetSocketEventDelegate = (client) => new DiskBlockCommandClientSocketEvent(client)
        });
    }
}
