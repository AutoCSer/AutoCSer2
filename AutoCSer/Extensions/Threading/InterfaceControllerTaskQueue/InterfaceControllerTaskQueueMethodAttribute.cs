using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列节点方法配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InterfaceControllerTaskQueueMethodAttribute : Attribute
    {
        /// <summary>
        /// 数据回调线程模式，默认为 Task.Run
        /// </summary>
        public ClientCallbackTypeEnum CallbackType;

        /// <summary>
        /// 默认接口任务队列节点方法配置
        /// </summary>
        internal static readonly InterfaceControllerTaskQueueMethodAttribute Default = new InterfaceControllerTaskQueueMethodAttribute();
    }
}
