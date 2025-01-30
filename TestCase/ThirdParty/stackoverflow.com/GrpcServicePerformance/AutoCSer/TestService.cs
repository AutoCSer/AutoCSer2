using System;

namespace AutoCSer.TestCase.CommandServerPerformance
{
    /// <summary>
    /// AutoCSer RPC test server
    /// </summary>
    internal sealed class TestService : ITestService
    {
        /// <summary>
        /// The server I/O thread returns the result of synchronization
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ITestService.Synchronous(int left, int right) { return left + right; }
        /// <summary>
        /// The server I/O thread is called synchronously, and the callback returns the result
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        void ITestService.Callback(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback) { callback.Callback(left + right); }
        /// <summary>
        /// The server specifies the result of the synchronization queue execution
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ITestService.Queue(AutoCSer.Net.CommandServerCallQueue queue, int left, int right) { return left + right; }
        /// <summary>
        /// The server determines the Task scheduling mode based on comprehensive information such as concurrent load
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> ITestService.Task(int left, int right)
        {
            return Task.FromResult(left + right);
        }
        /// <summary>
        /// The server I/O thread directly invokes the asycn Task synchronously
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> ITestService.SynchronousCallTask(int left, int right)
        {
            return Task.FromResult(left + right);
        }
        /// <summary>
        /// The server Task queue mode invokes the async Task
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> ITestService.TaskQueue(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return Task.FromResult(left + right);
        }
    }
}
