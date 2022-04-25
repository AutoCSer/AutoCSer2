using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTaskQueue
{
    /// <summary>
    /// 服务端 async Task 读写队列调用 保持回调委托返回数据 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int queueKey, int parameter1, int parameter2, CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackCall(CommandServerCallTaskLowPriorityQueue queue, int queueKey, int parameter, CommandServerKeepCallback callback);

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackCountReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int queueKey, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackCountCall(CommandServerCallTaskLowPriorityQueue queue, int queueKey, int parameter, CommandServerKeepCallbackCount callback);

        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task[IEnumerable[T]]</returns>
        [CommandServerMethod(KeepCallbackOutputCount = 4)]
        Task<IEnumerable<int>> EnumerableCallbackCount(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2);

#if !DotNet45 && !NetStandard2
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async IAsyncEnumerable[T]</returns>
        [CommandServerMethod(KeepCallbackOutputCount = 4)]
        IAsyncEnumerable<int> AsyncEnumerable(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2);
#endif
    }
    /// <summary>
    /// 服务端 async Task 读写队列调用 保持回调委托返回数据 示例接口实例
    /// </summary>
    internal sealed class KeepCallbackController : IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int queueKey, int parameter1, int parameter2, CommandServerKeepCallback<int> callback)
        {
            await Task.Yield();
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) callback.Callback(value);
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCall(CommandServerCallTaskLowPriorityQueue queue, int queueKey, int parameter, CommandServerKeepCallback callback)
        {
            await Task.Yield();
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) callback.Callback();
        }

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCountReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int queueKey, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback)
        {
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) await callback.CallbackAsync(value);
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="queueKey">默认第一个数据参数为队列关键字</param>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCountCall(CommandServerCallTaskLowPriorityQueue queue, int queueKey, int parameter, CommandServerKeepCallbackCount callback)
        {
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) await callback.CallbackAsync();
        }

        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task[IEnumerable[T]]</returns>
        async Task<IEnumerable<int>> IKeepCallbackController.EnumerableCallbackCount(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2)
        {
            await Task.Yield();
            return enumerableCallbackCount(parameter1, parameter2);
        }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        private static IEnumerable<int> enumerableCallbackCount(int parameter1, int parameter2)
        {
            for (int value = parameter1 + parameter2, endValue = value + 4; value != endValue;) yield return value++;
        }

#if !DotNet45 && !NetStandard2
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async IAsyncEnumerable[T]</returns>
        async IAsyncEnumerable<int> IKeepCallbackController.AsyncEnumerable(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2)
        {
            await Task.Yield();
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) yield return value;
        }
#endif
    }
}
