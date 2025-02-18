using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 超时检查回调
    /// </summary>
    internal sealed class TimeoutCallback : AutoCSer.Threading.QueueTaskNode
    {
        /// <summary>
        /// 接口实时调用监视服务
        /// </summary>
        private readonly InterfaceRealTimeCallMonitorService service;
        /// <summary>
        /// 超时检查回调
        /// </summary>
        /// <param name="service">接口实时调用监视服务</param>
        internal TimeoutCallback(InterfaceRealTimeCallMonitorService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 超时检查回调
        /// </summary>
        public override void RunTask()
        {
            service.CheckTimeout();
        }
    }
}
