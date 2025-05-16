using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Task 执行完以后返回固定值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TaskReturnAwaiter<T> : TaskCastAwaiter<T>
    {
        /// <summary>
        /// 执行 Task
        /// </summary>
        private readonly Task task;
        /// <summary>
        /// 执行异常信息
        /// </summary>
        public override Exception Exception
        {
            get { return task.Exception.notNull(); }
        }
        /// <summary>
        /// Task 执行完以后返回固定值
        /// </summary>
        /// <param name="task"></param>
        /// <param name="result">返回值</param>
        public TaskReturnAwaiter(Task task, T result)
        {
            this.result = result;
            this.task = task;
            if (task.IsCompleted)
            {
                if (task.Exception == null) IsResult = true;
                setCompleted();
            }
            else task.GetAwaiter().UnsafeOnCompleted(onCompleted);
        }
        /// <summary>
        /// Task 执行完成
        /// </summary>
        private new void onCompleted()
        {
            if (task.Exception == null) IsResult = true;
            base.onCompleted();
        }
    }
}
