using System;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 已完成 Awaiter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CompletedTaskCastAwaiter<T> : TaskCastAwaiter<T>
    {
        /// <summary>
        /// 不支持，直接抛出异常
        /// </summary>
        public override Exception Exception { get { throw new InvalidOperationException(); } }
        /// <summary>
        /// 已完成默认值 Awaiter
        /// </summary>
        private CompletedTaskCastAwaiter()
        {
            IsResult = true;
            setCompleted();
        }
        /// <summary>
        /// 已完成 Awaiter
        /// </summary>
        /// <param name="result">任务执行结果</param>
        public CompletedTaskCastAwaiter(T result)
        {
            setResult(result);
        }

        ///// <summary>
        ///// 隐式转换
        ///// </summary>
        ///// <param name="result">任务执行结果</param>
        //public static implicit operator CompletedTaskCastAwaiter<T>(T result) { return new CompletedTaskCastAwaiter<T>(result); }

        /// <summary>
        /// 默认值已完成 Awaiter
        /// </summary>
        public static readonly CompletedTaskCastAwaiter<T> Default = new CompletedTaskCastAwaiter<T>();
    }
}
