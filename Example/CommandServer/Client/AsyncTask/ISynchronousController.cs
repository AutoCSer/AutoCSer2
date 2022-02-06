using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Client.AsyncTask
{
    /// <summary>
    /// 服务端 async Task 调用 同步返回数据 示例接口（客户端）
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 客户端 await 等待
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTask.ISynchronousController.SynchronousReturn))]
        ReturnCommand<int> SynchronousReturnAsync(int parameter1, int parameter2);
        /// <summary>
        /// 客户端同步队列任务触发 await 等待返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTask.ISynchronousController.SynchronousReturn))]
        ReturnQueueCommand<int> SynchronousReturnQueueAsync(int parameter1, int parameter2);
        /// <summary>
        /// 客户端同步等待
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        CommandClientReturnValue<int> SynchronousReturn(int parameter1, int parameter2);
        /// <summary>
        /// 客户端回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 客户端回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousReturn(int parameter1, int parameter2, CommandClientCallback<int> callback);
        /// <summary>
        /// 客户端同步队列任务执行回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue> callback);
        /// <summary>
        /// 客户端同步队列任务执行回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousReturn(int parameter1, int parameter2, CommandClientCallbackQueueNode<int> callback);

        /// <summary>
        /// 客户端 await 等待
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTask.ISynchronousController.SynchronousCall))]
        ReturnCommand SynchronousCallAsync(int parameter1, int parameter2);
        /// <summary>
        /// 客户端同步队列任务触发 await 等待返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTask.ISynchronousController.SynchronousCall))]
        ReturnQueueCommand SynchronousCallQueueAsync(int parameter1, int parameter2);
        /// <summary>
        /// 客户端同步等待
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        CommandClientReturnValue SynchronousCall(int parameter1, int parameter2);
        /// <summary>
        /// 客户端回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousCall(int parameter1, int parameter2, Action<CommandClientReturnValue> callback);
        /// <summary>
        /// 客户端回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousCall(int parameter1, int parameter2, CommandClientCallback callback);
        /// <summary>
        /// 客户端同步队列任务执行回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousCall(int parameter1, int parameter2, Action<CommandClientReturnValue, CommandClientCallQueue> callback);
        /// <summary>
        /// 客户端同步队列任务执行回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托返回数据</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        CallbackCommand SynchronousCall(int parameter1, int parameter2, CommandClientCallbackQueueNode callback);
    }
}
