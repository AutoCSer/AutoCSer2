using System;

namespace AutoCSer.TestCase.CommandServerPerformance
{
    /// <summary>
    /// AutoCSer RPC test server interface
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 16, IsCodeGeneratorClientInterface = false)]
    public interface ITestService
    {
        /// <summary>
        /// The server I/O thread returns the result of synchronization
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Synchronous(int left, int right);
        /// <summary>
        /// The server I/O thread is called synchronously, and the callback returns the result
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        void Callback(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
        /// <summary>
        /// The server specifies the result of the synchronization queue execution
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Queue(AutoCSer.Net.CommandServerCallQueue queue, int left, int right);
        /// <summary>
        /// The server determines the Task scheduling mode based on comprehensive information such as concurrent load
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> Task(int left, int right);
        /// <summary>
        /// The server I/O thread directly invokes the asycn Task synchronously
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandServerMethod(IsSynchronousCallTask = true)]
        Task<int> SynchronousCallTask(int left, int right);
        /// <summary>
        /// The server Task queue mode invokes the async Task
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> TaskQueue(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);
    }
}
