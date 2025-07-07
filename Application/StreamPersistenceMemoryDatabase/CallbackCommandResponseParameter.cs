using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调委托返回参数
    /// </summary>
    public sealed class CallbackCommandResponseParameter : ResponseParameter
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
        private readonly ClientNode node;
        /// <summary>
        /// Request the index information of the node for passing parameters
        /// 请求传参的节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action<ResponseResult> callback;
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="callback">回调委托</param>
        internal CallbackCommandResponseParameter(ClientNode node, Action<ResponseResult> callback)
        {
            this.node = node;
            nodeIndex = node.Index;
            this.callback = callback;
        }
        /// <summary>
        /// 返回结果回调
        /// </summary>
        /// <param name="result"></param>
        internal void Callback(CommandClientReturnValue<CallStateEnum> result)
        {
            if (result.IsSuccess)
            {
                switch (result.Value)
                {
                    case CallStateEnum.PersistenceCallbackException:
                        try
                        {
                            node.Renew(nodeIndex).NotWait();
                        }
                        finally { callback(result.Value); }
                        return;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        try
                        {
                            node.Reindex(nodeIndex).NotWait();
                        }
                        finally { callback(result.Value); }
                        return;
                }
                callback(result.Value);
            }
            else callback(new ResponseResult(result.ReturnType, result.ErrorMessage));
        }
    }
    /// <summary>
    /// 回调委托返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallbackCommandResponseParameter<T> : ResponseParameter
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
        private readonly ClientNode node;
        /// <summary>
        /// Request the index information of the node for passing parameters
        /// 请求传参的节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action<ResponseResult<T>> callback;
        /// <summary>
        /// Return value
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="callback">回调委托</param>
        protected CallbackCommandResponseParameter(ClientNode node, Action<ResponseResult<T>> callback)
        {
            this.node = node;
            nodeIndex = node.Index;
            this.callback = callback;
        }
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="value">返回值</param>
        /// <param name="callback">回调委托</param>
        internal CallbackCommandResponseParameter(ClientNode node, T value, Action<ResponseResult<T>> callback)
        {
            this.node = node;
            nodeIndex = node.Index;
            this.callback = callback;
            Value.ReturnValue = value;
        }
        /// <summary>
        /// 返回结果回调
        /// </summary>
        /// <param name="result"></param>
        internal void Callback(CommandClientReturnValue<ResponseParameter> result)
        {
            if (result.IsSuccess)
            {
                switch (result.Value.State)
                {
                    case CallStateEnum.Success: callback(Value.ReturnValue); return;
                    case CallStateEnum.PersistenceCallbackException:
                        try
                        {
                            node.Renew(nodeIndex).NotWait();
                        }
                        finally { callback(result.Value.State); }
                        return;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        try
                        {
                            node.Reindex(nodeIndex).NotWait();
                        }
                        finally { callback(result.Value.State); }
                        return;
                }
                callback(result.Value.State);
            }
            else callback(new ResponseResult<T>(result.ReturnType, result.ErrorMessage));
        }
    }
}
