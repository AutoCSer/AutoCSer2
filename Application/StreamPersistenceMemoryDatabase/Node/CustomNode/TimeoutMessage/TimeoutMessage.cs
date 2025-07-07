using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// 超时任务消息
    /// </summary>
    /// <typeparam name="T">Task message data type
    /// 任务消息数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class TimeoutMessage<T> : AutoCSer.Threading.Link<TimeoutMessage<T>>
    {
        /// <summary>
        /// 序列化数据
        /// </summary>
        internal TimeoutMessageData<T> Data;
        /// <summary>
        /// 任务消息数据
        /// </summary>
        public T TaskData
        {
            get { return Data.Task; }
        }
        /// <summary>
        /// 超时任务消息
        /// </summary>
        private TimeoutMessage() { }
        /// <summary>
        /// 超时任务消息
        /// </summary>
        /// <param name="data"></param>
        internal TimeoutMessage(ref TimeoutMessageData<T> data)
        {
            Data = data;
        }
        /// <summary>
        /// 超时任务消息
        /// </summary>
        /// <param name="task">任务消息</param>
        public TimeoutMessage(T task)
        {
            Data.Task = task;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="task"></param>
        public static implicit operator TimeoutMessage<T>(T task) { return new TimeoutMessage<T>(task); }
        /// <summary>
        /// 超时启动任务
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task Timeout(TimeoutMessageNode<T> node)
        {
            Data.IsRunTask = true;
            return RunTask(node, TimeoutMessageRunTaskTypeEnum.Timeout);
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal async Task RunTask(TimeoutMessageNode<T> node, TimeoutMessageRunTaskTypeEnum type)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            bool isSuccess = false;
            try
            {
                isSuccess = await node.RunTask(this, type);
            }
            catch (Exception exception)
            {
                await node.OnTaskException(this, exception);
            }
            finally { node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(Data.Identity, isSuccess); }
        }
    }
}
