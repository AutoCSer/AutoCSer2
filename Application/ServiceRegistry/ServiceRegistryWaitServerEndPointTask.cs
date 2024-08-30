using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册客户端等待服务监听地址定时任务
    /// </summary>
    public sealed class ServiceRegistryWaitServerEndPointTask : AutoCSer.Threading.SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 服务注册客户端组件
        /// </summary>
        private readonly ServiceRegistryCommandClientServiceRegistrar serviceRegistrar;
        /// <summary>
        /// 是否已经取消定时任务
        /// </summary>
        private bool isCanceled;
        /// <summary>
        /// 服务注册客户端等待服务监听地址定时任务
        /// </summary>
        /// <param name="serviceRegistrar">服务注册客户端组件</param>
        /// <param name="waitServerEndPointSeconds">等待服务监听地址定时间隔秒数</param>
        internal ServiceRegistryWaitServerEndPointTask(ServiceRegistryCommandClientServiceRegistrar serviceRegistrar, byte waitServerEndPointSeconds)
            : base(AutoCSer.Threading.SecondTimer.TaskArray, waitServerEndPointSeconds, Threading.SecondTimerTaskThreadModeEnum.WaitTask, Threading.SecondTimerKeepModeEnum.After, waitServerEndPointSeconds)
        {
            this.serviceRegistrar = serviceRegistrar;
        }
        /// <summary>
        /// 获取下一个超时秒计数
        /// </summary>
        /// <returns></returns>
        protected override long getNextTimeoutSeconds()
        {
            return isCanceled ? 0 : base.getNextTimeoutSeconds();
        }
        /// <summary>
        /// 定时任务
        /// </summary>
        /// <returns></returns>
        protected internal override async Task OnTimerAsync()
        {
            if (serviceRegistrar.Assembler.MainLog != null || await serviceRegistrar.WaitServerEndPoint()) isCanceled = true;
        }
    }
}
