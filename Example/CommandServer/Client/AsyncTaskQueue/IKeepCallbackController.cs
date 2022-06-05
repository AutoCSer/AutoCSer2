using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Client.AsyncTaskQueue
{
    /// <summary>
    /// 服务端 async Task 读写队列调用 保持回调委托返回数据 示例接口（客户端）
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> CallbackReturn(int queueKey, int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.CallbackReturn))]
        EnumeratorQueueCommand<int> CallbackReturnQueue(int queueKey, int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int queueKey, int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int queueKey, int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int queueKey, int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int queueKey, int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        EnumeratorCommand CallbackCall(int queueKey, int parameter);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.CallbackCall))]
        EnumeratorQueueCommand CallbackCallQueue(int queueKey, int parameter);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int queueKey, int parameter, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int queueKey, int parameter, CommandClientKeepCallback callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int queueKey, int parameter, Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int queueKey, int parameter, CommandClientKeepCallbackQueue callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> CallbackCountReturn(int queueKey, int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.CallbackCountReturn))]
        EnumeratorQueueCommand<int> CallbackCountReturnQueue(int queueKey, int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int queueKey, int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int queueKey, int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int queueKey, int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int queueKey, int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        EnumeratorCommand CallbackCountCall(int queueKey, int parameter);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.CallbackCountCall))]
        EnumeratorQueueCommand CallbackCountCallQueue(int queueKey, int parameter);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int queueKey, int parameter, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int queueKey, int parameter, CommandClientKeepCallback callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int queueKey, int parameter, Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int queueKey, int parameter, CommandClientKeepCallbackQueue callback);


        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> EnumerableCallbackCount(int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.EnumerableCallbackCount))]
        EnumeratorQueueCommand<int> EnumerableCallbackCountQueue(int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand EnumerableCallbackCount(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand EnumerableCallbackCount(int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand EnumerableCallbackCount(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand EnumerableCallbackCount(int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);

#if !DotNet45 && !NetStandard2
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> AsyncEnumerable(int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.AsyncTaskQueue.IKeepCallbackController.AsyncEnumerable))]
        EnumeratorQueueCommand<int> AsyncEnumerableQueue(int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand AsyncEnumerable(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand AsyncEnumerable(int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand AsyncEnumerable(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand AsyncEnumerable(int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);
#endif
    }
}
