//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
        /// <summary>
        /// 服务端控制器 Task 异步读写队列 API 示例接口 客户端接口
        /// </summary>
        public partial interface ITaskQueueControllerClientController
        {
            /// <summary>
            /// 一次性响应 API 示例
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
            AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
            /// <summary>
            /// 服务端异步流 API 示例
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns>返回值类型必须为 IAsyncEnumerable{T}</returns>
            AutoCSer.Net.EnumeratorCommand<int> AsyncEnumerable(int left, int right);
            /// <summary>
            /// 仅执行 API 示例
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.Net.SendOnlyCommand Call(int value);
            /// <summary>
            /// 回调委托 API 示例
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns>回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</returns>
            AutoCSer.Net.EnumeratorCommand<int> Callback(int left, int right);
            /// <summary>
            /// 回调计数委托 API 示例
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns>回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</returns>
            AutoCSer.Net.EnumeratorCommand<int> CallbackCount(int left, int right);
            /// <summary>
            /// 集合封装 API 示例
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns>返回值类型必须为 Task{IEnumerable{T}}</returns>
            AutoCSer.Net.EnumeratorCommand<int> Enumerable(int left, int right);
        }
}
#endif