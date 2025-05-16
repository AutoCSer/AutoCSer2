using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 捕获异常任务
    /// </summary>
    public sealed class CatchTask : DoubleLink<CatchTask>
    {
        /// <summary>
        /// 任务
        /// </summary>
        public readonly Task Task;
        /// <summary>
        /// 调用文件路径
        /// </summary>
        public readonly string CallerFilePath;
        /// <summary>
        /// 调用成员名称
        /// </summary>
        public readonly string CallerMemberName;
        /// <summary>
        /// 所在文件行数
        /// </summary>
        public readonly int CallerLineNumber;
        /// <summary>
        /// 是否加入未完成队列
        /// </summary>
        private readonly bool isQueue;

        /// <summary>
        /// 捕获异常线程
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="isQueue">是否加入未完成队列</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        internal CatchTask(Task task, bool isQueue, string callerFilePath, string callerMemberName, int callerLineNumber)
        {
            this.Task = task;
            this.CallerFilePath = callerFilePath;
            this.CallerMemberName = callerMemberName;
            this.CallerLineNumber = callerLineNumber;
            this.isQueue = isQueue;
            TaskAwaiter awaiter = task.GetAwaiter();
            if (isQueue) queue.PushNotNull(this);
            awaiter.OnCompleted(onCompleted);
        }
        /// <summary>
        /// 任务完成处理
        /// </summary>
        private void onCompleted()
        {
            var exception = Task.Exception;
            if (exception != null) AutoCSer.LogHelper.Default.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception, CallerFilePath, CallerMemberName, CallerLineNumber);
            if (isQueue) queue.PopNotNull(this);
        }

        /// <summary>
        /// 未释放任务集合
        /// </summary>
        private static YieldLink queue;
        /// <summary>
        /// 枚举所有未释放任务
        /// </summary>
        public static IEnumerable<CatchTask> Tasks
        {
            get
            {
                var end = queue.End;
                while (end != null)
                {
                    yield return end;
                    end = end.DoubleLinkPrevious;
                }
            }
        }
    }
}
