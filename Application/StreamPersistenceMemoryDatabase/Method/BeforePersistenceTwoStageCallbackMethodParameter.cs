using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持久化之前检查参数的调用方法与参数信息
    /// </summary>
    internal sealed class BeforePersistenceTwoStageCallbackMethodParameter : TwoStageCallbackMethodParameter
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
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        internal BeforePersistenceTwoStageCallbackMethodParameter(ServerNode node, TwoStageCallbackMethod method, CommandServerCallback<ResponseParameter> callback, CommandServerKeepCallback<KeepCallbackResponseParameter> keepCallback) : base(node, method, callback, keepCallback)
        {
            customSessionObject = AutoCSer.Common.EmptyObject;
        }
        /// <summary>
        /// 设置自定义状态对象
        /// </summary>
        /// <param name="sessionObject">自定义状态对象</param>
        public override void SetBeforePersistenceCustomSessionObject(object sessionObject) { customSessionObject = sessionObject; }
        /// <summary>
        /// 获取自定义状态对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public override object? GetBeforePersistenceCustomSessionObject()
#else
        public override object GetBeforePersistenceCustomSessionObject()
#endif
        {
            object customSessionObject = this.customSessionObject;
            this.customSessionObject = AutoCSer.Common.EmptyObject;
            return object.ReferenceEquals(customSessionObject, AutoCSer.Common.EmptyObject) ? null : customSessionObject;
        }
    }
}
