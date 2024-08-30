using System;
using System.Threading.Tasks;

#pragma warning disable
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
    internal sealed class ServerTaskQueueContextController : AutoCSer.Net.CommandServerTaskQueueService<int>, IServerTaskQueueContextController
    {
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public ServerTaskQueueContextController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }

        Task<string> IServerTaskQueueContextController.TaskQueueReturn()
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task<string> IServerTaskQueueContextController.TaskQueueReturn(int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueContextController.TaskQueue()
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskQueueContextController.TaskQueue(int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskQueueContextController.TaskQueueLowPriorityReturn()
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task<string> IServerTaskQueueContextController.TaskQueueLowPriorityReturn(int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueContextController.TaskQueueLowPriority()
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskQueueContextController.TaskQueueLowPriority(int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
