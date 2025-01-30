using System;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// AutoCSer RPC test client interface
    /// </summary>
    public interface ITestClient
    {
        /// <summary>
        /// The server I/O thread returns the result of synchronization
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> Synchronous(int left, int right);
        /// <summary>
        /// The server I/O thread is called synchronously, and the callback returns the result
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> Callback(int left, int right);
        /// <summary>
        /// The server specifies the result of the synchronization queue execution
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> Queue(int left, int right);
        /// <summary>
        /// The server determines the Task scheduling mode based on comprehensive information such as concurrent load
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> Task(int left, int right);
        /// <summary>
        /// The server I/O thread directly invokes the asycn Task synchronously
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> SynchronousCallTask(int left, int right);
        /// <summary>
        /// The server Task queue mode invokes the async Task
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        AutoCSer.Net.ReturnCommand<int> TaskQueue(int left, int right);
    }
}
