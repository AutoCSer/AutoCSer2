using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// Server-side Task asynchronous two-stage response API sample interface
    /// 服务端 Task 异步 二阶段响应 API 示例接口
    /// </summary>
    public interface ITwoStage‌CallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task Callback(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback);
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task CallbackCount(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallbackCount<int> keepCallback);
    }
    /// <summary>
    /// Server Task Asynchronous two-stage response API sample controller
    /// 服务端 Task 异步 二阶段响应 API 示例控制器
    /// </summary>
    internal sealed class TwoStage‌CallbackController : ITwoStage‌CallbackController
    {
        /// <summary>
        /// Example of callback delegate
        /// 回调委托示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        internal static void Callback(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback)
        {
            if (callback.Callback(new TwoStageCallbackParameter { Start = left, Count = count }))
            {
                int right = left + count;
                do
                {
                    if (!keepCallback.Callback(left)) return;
                }
                while (++left != right);
                keepCallback.CancelKeep();
            }
        }
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task ITwoStage‌CallbackController.Callback(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback)
        {
            Callback(left, count, callback, keepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Callback count delegate example
        /// 回调计数委托示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        internal static async System.Threading.Tasks.Task CallbackCount(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallbackCount<int> keepCallback)
        {
            if (callback.Callback(new TwoStageCallbackParameter { Start = left, Count = count }))
            {
                int right = left + count;
                do
                {
                    if (!await keepCallback.CallbackAsync(left)) return;
                }
                while (++left != right);
                keepCallback.CancelKeep();
            }
        }
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task ITwoStage‌CallbackController.CallbackCount(int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallbackCount<int> keepCallback)
        {
            return CallbackCount(left, count, callback, keepCallback);
        }
    }
}
