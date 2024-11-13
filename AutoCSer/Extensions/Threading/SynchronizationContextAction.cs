using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 同步上下文调用
    /// </summary>
    internal sealed class SynchronizationContextAction
    {
        /// <summary>
        /// 调用委托
        /// </summary>
        private readonly Action call;
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <param name="call">调用委托</param>
        internal SynchronizationContextAction(Action call)
        {
            this.call = call;
        }
        /// <summary>
        /// 调用委托
        /// </summary>
        /// <param name="_"></param>
#if NetStandard21
        internal void Call(object? _)
#else
        internal void Call(object _)
#endif
        {
            call();
        }
    }
    /// <summary>
    /// 同步上下文调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SynchronizationContextAction<T>
    {
        /// <summary>
        /// 调用委托
        /// </summary>
        private readonly Action<T> call;
        /// <summary>
        /// 调用参数
        /// </summary>
        private readonly T parameter;
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <param name="call">调用委托</param>
        /// <param name="parameter">调用参数</param>
        internal SynchronizationContextAction(Action<T> call, T parameter)
        {
            this.call = call;
            this.parameter = parameter;
        }
        /// <summary>
        /// 调用委托
        /// </summary>
        /// <param name="_"></param>
#if NetStandard21
        internal void Call(object? _)
#else
        internal void Call(object _)
#endif
        {
            call(parameter);
        }
    }
}
