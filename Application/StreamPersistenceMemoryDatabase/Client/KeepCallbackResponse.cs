using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Keep the callback output
    /// 保持回调输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class KeepCallbackResponse<T> : IDisposable, IEnumeratorCommand<ResponseResult<T>>
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly ClientNode node;
        /// <summary>
        /// Callback command
        /// 回调命令
        /// </summary>
#if NetStandard21
        public readonly AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>? EnumeratorCommand;
#else
        public readonly AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter> EnumeratorCommand;
#endif
        /// <summary>
        /// Command client socket
        /// 命令客户端套接字
        /// </summary>
#if NetStandard21
        public CommandClientSocket? Socket { get { return EnumeratorCommand?.Socket; } }
#else
        public CommandClientSocket Socket { get { return EnumeratorCommand?.Socket; } }
#endif
        /// <summary>
        /// Whether the command was successfully added to the output queue
        /// 命令是否成功添加到输出队列
        /// </summary>
        public bool IsSuccess { get { return EnumeratorCommand != null; } }
        /// <summary>
        /// The return value type of the callback command
        /// 回调命令返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType
        {
            get
            {
                return EnumeratorCommand != null ? EnumeratorCommand.ReturnType : CommandClientReturnTypeEnum.Unknown; 
            }
        }
        /// <summary>
        /// Empty response (Command submission failed)
        /// 空响应（命令提交失败）
        /// </summary>
        private KeepCallbackResponse() { }
        /// <summary>
        /// Keep the callback output
        /// 保持回调输出
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="enumeratorCommand">Callback command
        /// 回调命令</param>
        internal KeepCallbackResponse(ClientNode node, AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter> enumeratorCommand)
        {
            this.EnumeratorCommand = enumeratorCommand;
            this.node = node;
            nodeIndex = node.Index;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (EnumeratorCommand != null) ((IDisposable)EnumeratorCommand).Dispose();
        }
#if NetStandard21
        /// <summary>
        /// Get the IAsyncEnumerable
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ResponseResult<T>> GetAsyncEnumerable()
        {
            if (EnumeratorCommand != null)
            {
                if (!EnumeratorCommand.IsDisposed)
                {
                    try
                    {
                        bool isError = false;
                        while (await EnumeratorCommand.MoveNext())
                        {
                            KeepCallbackResponseParameter response = EnumeratorCommand.Current;
                            if (response.State == CallStateEnum.Success)
                            {
                                if (EnumeratorCommand.ReturnType == AutoCSer.Net.CommandClientReturnTypeEnum.Success)
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
                                    yield return EnumeratorCommand.ReturnType;
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
                        //if (isError) Dispose();
                        //else if (enumeratorCommand.ReturnType != Net.CommandClientReturnTypeEnum.Success) yield return enumeratorCommand.ReturnType;
                        if (!isError && EnumeratorCommand.ReturnType != Net.CommandClientReturnTypeEnum.Success) yield return EnumeratorCommand.ReturnType;
                    }
                    finally { Dispose(); }
                }
                else yield return AutoCSer.Net.CommandClientReturnTypeEnum.KeepCallbackDisposed;
            }
            else yield return AutoCSer.Net.CommandClientReturnTypeEnum.ClientUnknown;
        }
        /// <summary>
        /// Convert the data and get the IAsyncEnumerable
        /// 数据转换并获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="VT">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ResponseResult<VT>> GetAsyncEnumerable<VT>(Func<T?, VT> getValue)
        {
            await foreach (ResponseResult<T> value in GetAsyncEnumerable())
            {
                if (value.IsSuccess) yield return getValue(value.Value);
                else yield return value.Cast<VT>();
            }
        }
#else
#endif
        /// <summary>
        /// Node index information
        /// 节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// await bool, the collection enumeration command returns true when the next data exists
        /// await bool，集合枚举命令存在下一个数据返回 true
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommandMoveNext MoveNext()
        {
            if (EnumeratorCommand != null) return EnumeratorCommand.MoveNext();
            return EnumeratorCommandMoveNext.NextValueFalse;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MoveNextAsync()
        {
            if (EnumeratorCommand != null) return await EnumeratorCommand.MoveNext();
            return false;
        }
        /// <summary>
        /// Get current data
        /// 获取当前数据
        /// </summary>
        public ResponseResult<T> Current
        {
            get
            {
                if (EnumeratorCommand != null)
                {
                    if (!EnumeratorCommand.IsDisposed)
                    {
                        KeepCallbackResponseParameter response = EnumeratorCommand.Current;
                        if (response.State == CallStateEnum.Success)
                        {
                            if (EnumeratorCommand.ReturnType == Net.CommandClientReturnTypeEnum.Success)
                            {
                                if (response.DeserializeValue != null)
                                {
                                    return new ResponseResult<T>(KeepCallbackResponseDeserializeValue<T>.GetValue(response.DeserializeValue));
                                }
                                Dispose();
                                return AutoCSer.Net.CommandClientReturnTypeEnum.ClientDeserializeError;
                            }
                            Dispose();
                            return EnumeratorCommand.ReturnType;
                        }
                        Dispose();
                        //node.CheckState(nodeIndex, response.State);
                        node.CheckStateAsync(nodeIndex, response.State).NotWait();
                        return response.State;
                    }
                    return AutoCSer.Net.CommandClientReturnTypeEnum.KeepCallbackDisposed;
                }
                return AutoCSer.Net.CommandClientReturnTypeEnum.ClientUnknown;
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (EnumeratorCommand != null) await ((IAsyncDisposable)EnumeratorCommand).DisposeAsync();
        }

        /// <summary>
        /// Empty response (Command submission failed)
        /// 空响应（命令提交失败）
        /// </summary>
        internal static readonly KeepCallbackResponse<T> NullResponse = new KeepCallbackResponse<T>();
    }
}
