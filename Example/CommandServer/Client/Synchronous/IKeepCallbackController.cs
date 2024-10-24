﻿using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Client.Synchronous
{
    /// <summary>
    /// 服务端 IO线程同步调用 保持回调委托返回数据 示例接口（客户端）
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> CallbackReturn(int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.IKeepCallbackController.CallbackReturn))]
        EnumeratorQueueCommand<int> CallbackReturnQueue(int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackReturn(int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        EnumeratorCommand CallbackCall(int parameter);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.IKeepCallbackController.CallbackCall))]
        EnumeratorQueueCommand CallbackCallQueue(int parameter);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int parameter, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int parameter, CommandClientKeepCallback callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int parameter, Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCall(int parameter, CommandClientKeepCallbackQueue callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        EnumeratorCommand<int> CallbackCountReturn(int parameter1, int parameter2);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.IKeepCallbackController.CallbackCountReturn))]
        EnumeratorQueueCommand<int> CallbackCountReturnQueue(int parameter1, int parameter2);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int parameter1, int parameter2, CommandClientKeepCallback<int> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int parameter1, int parameter2, Action<CommandClientReturnValue<int>, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountReturn(int parameter1, int parameter2, CommandClientKeepCallbackQueue<int> callback);

        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        EnumeratorCommand CallbackCountCall(int parameter);
        /// <summary>
        /// await 等待保持回调
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.IKeepCallbackController.CallbackCountCall))]
        EnumeratorQueueCommand CallbackCountCallQueue(int parameter);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int parameter, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int parameter, CommandClientKeepCallback callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int parameter, Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback);
        /// <summary>
        /// 同步队列任务保持回调委托返回数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        KeepCallbackCommand CallbackCountCall(int parameter, CommandClientKeepCallbackQueue callback);
    }
}
