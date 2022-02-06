using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Client.Synchronous
{
    /// <summary>
    /// 服务端 IO线程同步调用 同步返回数据 示例接口（客户端）
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 客户端同步等待
        /// </summary>
        /// <param name="parameter">普通参数</param>
        /// <param name="refParameter">ref 参数</param>
        /// <param name="outParameter">out 参数</param>
        /// <returns></returns>
        CommandClientReturnValue<int> SynchronousReturn(int parameter, ref int refParameter, out long outParameter);

        /// <summary>
        /// 客户端 await 等待
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.ISynchronousController.SynchronousCall))]
        ReturnCommand SynchronousCallAsync(int parameter1, int parameter2);
        /// <summary>
        /// 客户端同步队列任务触发 await 等待返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Server.Synchronous.ISynchronousController.SynchronousCall))]
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
    /// <summary>
    /// 服务端 IO线程同步调用 同步返回数据 示例接口（客户端调用）
    /// </summary>
    internal static class SynchronousController
    {
        /// <summary>
        /// 客户端调用
        /// </summary>
        /// <param name="socketEvent"></param>
        /// <returns></returns>
        internal static async Task Call(CommandClientSocketEvent socketEvent)
        {
            int refParameter = 3;
            long outParameter = 5;
            CommandClientReturnValue<int> returnValue = socketEvent.Synchronous_SynchronousController.SynchronousReturn(7, ref refParameter, out outParameter);
            if (returnValue.IsSuccess)
            {
                Console.WriteLine(refParameter);
                Console.WriteLine(outParameter);
                Console.WriteLine(returnValue.Value);
            }
            else
            {
                Console.WriteLine(returnValue.ReturnType);
                Program.Breakpoint();
            }

            CommandClientReturnValue returnType = await socketEvent.Synchronous_SynchronousController.SynchronousCallAsync(3, 5);
            if (!returnType.IsSuccess) Program.Breakpoint();

            returnType = await socketEvent.Synchronous_SynchronousController.SynchronousCallQueueAsync(3, 5);
            if (!returnType.IsSuccess) Program.Breakpoint();

            returnType = socketEvent.Synchronous_SynchronousController.SynchronousCall(3, 5);
            if (!returnType.IsSuccess) Program.Breakpoint();
        }
    }
}
