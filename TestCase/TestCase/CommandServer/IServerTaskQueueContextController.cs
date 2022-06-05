using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// Task 队列上下文服务端测试接口
    /// </summary>
    public interface IServerTaskQueueContextController
    {
        Task<string> TaskQueueReturn();
        Task<string> TaskQueueReturn(int Value, int Ref);
        Task TaskQueue();
        Task TaskQueue( int Value, int Ref);

        [AutoCSer.Net.CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task<string> TaskQueueLowPriorityReturn();
        [AutoCSer.Net.CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task<string> TaskQueueLowPriorityReturn(int Value, int Ref);
        [AutoCSer.Net.CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task TaskQueueLowPriority();
        [AutoCSer.Net.CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task TaskQueueLowPriority(int Value, int Ref);
    }
    /// <summary>
    /// Task 队列上下文服务端测试接口
    /// </summary>
    internal sealed class ServerTaskQueueContextController : AutoCSer.Net.CommandServerTaskQueue<int>, IServerTaskQueueContextController
    {
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public ServerTaskQueueContextController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }

        async Task<string> IServerTaskQueueContextController.TaskQueueReturn()
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        async Task<string> IServerTaskQueueContextController.TaskQueueReturn(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString();
        }
        async Task IServerTaskQueueContextController.TaskQueue()
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
        }
        async Task IServerTaskQueueContextController.TaskQueue(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
        }

        async Task<string> IServerTaskQueueContextController.TaskQueueLowPriorityReturn()
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        async Task<string> IServerTaskQueueContextController.TaskQueueLowPriorityReturn(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString();
        }
        async Task IServerTaskQueueContextController.TaskQueueLowPriority()
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
        }
        async Task IServerTaskQueueContextController.TaskQueueLowPriority(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
        }
    }
}
