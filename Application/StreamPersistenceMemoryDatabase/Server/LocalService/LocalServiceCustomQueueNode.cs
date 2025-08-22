using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCustomQueueNode<T> : LocalServiceQueueNode<T>
    {
        /// <summary>
        /// 获取结果数据委托
        /// </summary>
        private readonly Func<T> getResult;
        /// <summary>
        /// 日志流持久化内存数据库本地服务队列节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="getResult">获取结果数据委托</param>
        /// <param name="queueNodeType"></param>
        internal LocalServiceCustomQueueNode(LocalService service, Func<T> getResult, ReadWriteNodeTypeEnum queueNodeType) : base(service)
        {
            this.getResult = getResult;
            switch (queueNodeType)
            {
                case ReadWriteNodeTypeEnum.Read: service.CommandServerCallQueue.AppendReadOnly(this); return;
                case ReadWriteNodeTypeEnum.ConcurrencyRead: service.CommandServerCallQueue.ConcurrencyRead(this); return;
                default: service.CommandServerCallQueue.AppendWriteOnly(this); return;
            }
        }
        /// <summary>
        /// 获取结果数据
        /// </summary>
        public override void RunTask()
        {
            Result = getResult();
            completed();
        }
    }
}
