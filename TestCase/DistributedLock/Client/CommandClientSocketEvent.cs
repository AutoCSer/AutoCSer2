using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DistributedLockClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent, IDistributedLockClientSocketEvent<int>
    {
        /// <summary>
        /// 锁请求队列管理
        /// </summary>
        private readonly DistributedLockRequestManager<int> distributedLockRequestManager;
        /// <summary>
        /// 分布式锁客户端接口
        /// </summary>
        public IDistributedLockClient<int> DistributedLockClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                //IO 线程同步回调接口避免线程调度开销，必须保证客户端调用 await 后续操作无阻塞，否则可能严重影响性能甚至死锁，如果不能保证无阻塞环境请替换为 IDistributedLockTaskClient 接口避免死锁
                yield return new CommandClientControllerCreatorParameter(typeof(IDistributedLockService<int>), typeof(IDistributedLockClient<int>));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        {
            distributedLockRequestManager = new DistributedLockRequestManager<int>(this);
        }

        /// <summary>
        /// 添加新的锁请求对象
        /// </summary>
        /// <param name="request"></param>
        void IDistributedLockClientSocketEvent.AppendRequest(IDistributedLockRequest request)
        {
            distributedLockRequestManager.Append(request);
        }
        /// <summary>
        /// 删除锁请求对象
        /// </summary>
        /// <param name="request"></param>
        void IDistributedLockClientSocketEvent.RemoveRequest(IDistributedLockRequest request)
        {
            distributedLockRequestManager.Remove(request);
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected override Task onMethodVerified(CommandClientSocket socket)
        {
            distributedLockRequestManager.EnterAgain();
            return AutoCSer.Common.CompletedTask;
        }
    }
}
