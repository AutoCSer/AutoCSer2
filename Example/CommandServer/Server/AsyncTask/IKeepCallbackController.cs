using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTask
{
    /// <summary>
    /// 服务端 async Task 调用 保持回调委托返回数据 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackCall(int parameter, CommandServerKeepCallback callback);

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        [CommandServerMethod(KeepCallbackOutputCount = 4)]
        Task CallbackCountReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        Task CallbackCountCall(int parameter, CommandServerKeepCallbackCount callback);

        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task[IEnumerable[T]]</returns>
        [CommandServerMethod(KeepCallbackOutputCount = 4)]
        Task<IEnumerable<int>> EnumerableCallbackCount(CommandServerSocket socket, int parameter1, int parameter2);

#if !DotNet45 && !NetStandard2
        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async IAsyncEnumerable[T]</returns>
        [CommandServerMethod(KeepCallbackOutputCount = 4)]
        IAsyncEnumerable<int> AsyncEnumerable(CommandServerSocket socket, int parameter1, int parameter2);
#endif
    }
    /// <summary>
    /// 服务端 async Task 调用 保持回调委托返回数据 示例接口实例
    /// </summary>
    internal sealed class KeepCallbackController : IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallback<int> callback)
        {
            await Task.Yield();
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) callback.Callback(value);
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCall(int parameter, CommandServerKeepCallback callback)
        {
            await Task.Yield();
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) callback.Callback();
        }

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCountReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback)
        {
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) await callback.CallbackAsync(value);
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        /// <returns>必须是 async Task</returns>
        async Task IKeepCallbackController.CallbackCountCall(int parameter, CommandServerKeepCallbackCount callback)
        {
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) await callback.CallbackAsync();
        }

        /// <summary>
        /// 保持回调委托返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task[IEnumerable[T]]</returns>
        async Task<IEnumerable<int>> IKeepCallbackController.EnumerableCallbackCount(CommandServerSocket socket, int parameter1, int parameter2)
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
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async IAsyncEnumerable[T]</returns>
        async IAsyncEnumerable<int> IKeepCallbackController.AsyncEnumerable(CommandServerSocket socket, int parameter1, int parameter2)
        {
            await Task.Yield();
            foreach (int value in enumerableCallbackCount(parameter1, parameter2)) yield return value;
        }
#endif
    }
}
