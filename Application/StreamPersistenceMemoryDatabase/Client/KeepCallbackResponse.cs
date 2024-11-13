using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 保持回调输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KeepCallbackResponse<T> : IDisposable
#if !NetStandard21
, IEnumeratorTask<ResponseResult<T>>
#endif
    {
        /// <summary>
        /// 客户端节点
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly ClientNode node;
        /// <summary>
        /// 回调命令
        /// </summary>
#if NetStandard21
        private readonly AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>? enumeratorCommand;
#else
        private readonly AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter> enumeratorCommand;
#endif
        /// <summary>
        /// 命令客户端套接字
        /// </summary>
#if NetStandard21
        public CommandClientSocket? Socket { get { return enumeratorCommand?.Socket; } }
#else
        public CommandClientSocket Socket { get { return enumeratorCommand?.Socket; } }
#endif
        /// <summary>
        /// 命令是否提交成功
        /// </summary>
        public bool IsSuccess { get { return enumeratorCommand != null; } }
        /// <summary>
        /// 回调命令返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType
        {
            get
            {
                return enumeratorCommand != null ? enumeratorCommand.ReturnType : CommandClientReturnTypeEnum.Unknown; 
            }
        }
        /// <summary>
        /// 空响应（命令提交失败）
        /// </summary>
        protected KeepCallbackResponse() { }
        /// <summary>
        /// 保持回调输出
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <param name="enumeratorCommand">回调命令</param>
        internal KeepCallbackResponse(ClientNode node, AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter> enumeratorCommand)
        {
            this.enumeratorCommand = enumeratorCommand;
            this.node = node;
#if !NetStandard21
            nodeIndex = node.Index;
#endif
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
        }
#if NetStandard21
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <returns></returns>
        public virtual async IAsyncEnumerable<ResponseResult<T>> GetAsyncEnumerable()
        {
            if (enumeratorCommand != null)
            {
                if (!enumeratorCommand.IsDisposed)
                {
                    bool isError = false;
                    NodeIndex nodeIndex = node.Index;
                    while (await enumeratorCommand.MoveNext())
                    {
                        KeepCallbackResponseParameter response = enumeratorCommand.Current;
                        if (response.State == CallStateEnum.Success)
                        {
                            if (enumeratorCommand.ReturnType == AutoCSer.Net.CommandClientReturnTypeEnum.Success)
                            {
                                if (response.DeserializeValue != null)
                                {
                                    yield return new ResponseResult<T>(KeepCallbackResponseDeserializeValue<T>.GetValue(response.DeserializeValue));
                                }
                                else
                                {
                                    isError = true;
                                    yield return AutoCSer.Net.CommandClientReturnTypeEnum.ClientDeserializeError;
                                    break;
                                }
                            }
                            else
                            {
                                isError = true;
                                yield return enumeratorCommand.ReturnType;
                                break;
                            }
                        }
                        else
                        {
                            isError = true;
                            yield return response.State;
                            await node.CheckStateAsync(nodeIndex, response.State);
                            break;
                        }
                    }
                    if (isError) Dispose();
                    else if (enumeratorCommand.ReturnType != Net.CommandClientReturnTypeEnum.Success) yield return enumeratorCommand.ReturnType;
                }
                else yield return AutoCSer.Net.CommandClientReturnTypeEnum.KeepCallbackDisposed;
            }
            else yield return AutoCSer.Net.CommandClientReturnTypeEnum.ClientUnknown;
        }
        /// <summary>
        /// 数据转换获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="VT">目标数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <returns></returns>
        public virtual async IAsyncEnumerable<ResponseResult<VT>> GetAsyncEnumerable<VT>(Func<T?, VT> getValue)
        {
            await foreach (ResponseResult<T> value in GetAsyncEnumerable())
            {
                if (value.IsSuccess) yield return getValue(value.Value);
                else yield return value.Cast<VT>();
            }
        }
#else
        /// <summary>
        /// 节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MoveNextAsync()
        {
            if (enumeratorCommand != null) return await enumeratorCommand.MoveNext();
            return false;
        }
        /// <summary>
        /// 获取当前数据
        /// </summary>
        public ResponseResult<T> Current
        {
            get
            {
                if (enumeratorCommand != null)
                {
                    if (!enumeratorCommand.IsDisposed)
                    {
                        KeepCallbackResponseParameter response = enumeratorCommand.Current;
                        if (response.State == CallStateEnum.Success)
                        {
                            if (enumeratorCommand.ReturnType == Net.CommandClientReturnTypeEnum.Success) return new ResponseResult<T>(KeepCallbackResponseDeserializeValue<T>.GetValue(response.DeserializeValue));
                            Dispose();
                            return enumeratorCommand.ReturnType;
                        }
                        Dispose();
                        node.CheckState(nodeIndex, response.State);
                        return response.State;
                    }
                    return AutoCSer.Net.CommandClientReturnTypeEnum.KeepCallbackDisposed;
                }
                return AutoCSer.Net.CommandClientReturnTypeEnum.ClientUnknown;
            }
        }
        /// <summary>
        /// 异步释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (enumeratorCommand != null) await ((IAsyncDisposable)enumeratorCommand).DisposeAsync();
        }
#endif
        /// <summary>
        /// 空响应（命令提交失败）
        /// </summary>
        internal static readonly KeepCallbackResponse<T> NullResponse = new KeepCallbackResponse<T>();
    }
}
