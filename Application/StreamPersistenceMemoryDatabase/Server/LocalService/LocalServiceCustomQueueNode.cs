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
        internal LocalServiceCustomQueueNode(LocalService service, Func<T> getResult) : base(service)
        {
            this.getResult = getResult;
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 获取结果数据
        /// </summary>
        public override void RunTask()
        {
            result = getResult();
            completed();
        }
    }
}
