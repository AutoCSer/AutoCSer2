using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持久化之前检查参数的调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class BeforePersistenceMethodParameter<T> : CallInputOutputMethodParameter<T>
        where T : struct
    {
        /// <summary>
        /// 自定义状态对象
        /// </summary>
        private object customSessionObject;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        public BeforePersistenceMethodParameter(ServerNode node, CallInputOutputMethod method, T parameter) : base(node, method, parameter) { }
        /// <summary>
        /// 设置自定义状态对象
        /// </summary>
        /// <param name="sessionObject">自定义状态对象</param>
        public override void SetBeforePersistenceCustomSessionObject(object sessionObject) { customSessionObject = sessionObject; }
        /// <summary>
        /// 获取自定义状态对象
        /// </summary>
        /// <returns></returns>
        public override object GetBeforePersistenceCustomSessionObject()
        {
            object customSessionObject = this.customSessionObject;
            this.customSessionObject = null;
            return customSessionObject;
        }
    }
}
