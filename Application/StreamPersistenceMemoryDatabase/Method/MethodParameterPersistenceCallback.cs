using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 非持久化回调
    /// </summary>
    internal sealed class MethodParameterPersistenceCallback : ReadWriteQueueNode
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        private readonly MethodParameter methodParameter;
        /// <summary>
        /// 非持久化回调
        /// </summary>
        /// <param name="methodParameter">调用方法与参数信息</param>
        internal MethodParameterPersistenceCallback(MethodParameter methodParameter)
        {
            this.methodParameter = methodParameter;
        }
        /// <summary>
        /// 非持久化回调
        /// </summary>
        public override void RunTask()
        {
            //methodParameter.Node.NodeCreator.Service.CurrentCallIsPersistence = false;
            methodParameter.PersistenceCallback();
        }
    }
}
