using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandServerPerformance
{
    /// <summary>
    /// 命令服务性能测试服务端接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 16, IsCodeGeneratorClientInterface = false)]
    public interface IService
    {
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Synchronous(int left, int right);
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        void Callback(int left, int right, CommandServerCallback<int> callback);
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Queue(CommandServerCallQueue queue, int left, int right);
        /// <summary>
        /// 服务端配置支持并发读队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, int left, int right);
        /// <summary>
        /// 服务端配置读写队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ReadWriteQueue(CommandServerCallReadQueue queue, int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> Task(int left, int right);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<int> SynchronousCallTask(int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> TaskQueue(CommandServerCallTaskQueue queue, int left, int right);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="queueKey">队列关键字</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandServerMethod(IsControllerTaskQueue = false)]
        Task<int> TaskQueueKey(CommandServerCallTaskQueue<int> queue, int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnly 应答处理
        /// </summary>
        /// <param name="callback"></param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void KeepCallback(CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 服务端配合 KeepCallback 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandServerSendOnly SendOnly(int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnlyTask 应答处理
        /// </summary>
        /// <param name="callback"></param>
        [CommandServerMethod(KeepCallbackOutputCount = 1 << 13, AutoCancelKeep = false)]
        void KeepCallbackCount(CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 服务端配合 KeepCallbackCount 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<CommandServerSendOnly> SendOnlyTask(int left, int right);
    }
    /// <summary>
    /// 命令服务性能测试服务端实例
    /// </summary>
    internal sealed class Service : IService
    {
        //private static CommandServerSocket socket;
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int IService.Synchronous(int left, int right) { return left + right; }
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        void IService.Callback(int left, int right, CommandServerCallback<int> callback) { callback.Callback(left + right); }
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int IService.Queue(CommandServerCallQueue queue, int left, int right) { return left + right; }
        /// <summary>
        /// 服务端配置支持并发读队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int IService.ConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, int left, int right) { return left + right; }
        /// <summary>
        /// 服务端配置读写队列执行返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int IService.ReadWriteQueue(CommandServerCallReadQueue queue, int left, int right) { return left + right; }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> IService.Task(int left, int right)
        {
            return Task.FromResult(left + right);
        }
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> IService.SynchronousCallTask(int left, int right)
        {
            return Task.FromResult(left + right);
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> IService.TaskQueue(CommandServerCallTaskQueue queue, int left, int right)
        {
            return Task.FromResult(left + right); 
        }
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> IService.TaskQueueKey(CommandServerCallTaskQueue<int> queue, int left, int right)
        {
            return Task.FromResult(left + right);
        }

        /// <summary>
        /// 当前保持回调
        /// </summary>
        private CommandServerKeepCallback<int> keepCallback;
        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnly 应答处理
        /// </summary>
        /// <param name="callback"></param>
        void IService.KeepCallback(CommandServerKeepCallback<int> callback)
        {
            keepCallback?.CancelKeep();
            keepCallback = callback;
        }
        /// <summary>
        /// 服务端配合 KeepCallback 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandServerSendOnly IService.SendOnly(int left, int right)
        {
            keepCallback.Callback(left + right);
            return CommandServerSendOnly.Null;
        }

        /// <summary>
        /// 当前保持回调
        /// </summary>
        private CommandServerKeepCallbackCount<int> keepCallbackCount;
        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnlyTask 应答处理
        /// </summary>
        /// <param name="callback"></param>
        void IService.KeepCallbackCount(CommandServerKeepCallbackCount<int> callback)
        {
             keepCallbackCount?.CancelKeep();
             keepCallbackCount = callback;
        }
        //private static int c;
        /// <summary>
        /// 服务端配合 KeepCallbackCount 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        async Task<CommandServerSendOnly> IService.SendOnlyTask(int left, int right)
        {
            //if ((++c & ((1 << 20) - 1)) == 0) Console.Write('.');
            await keepCallbackCount.CallbackAsync(left + right);
            return CommandServerSendOnly.Null;
        }
    }
}
