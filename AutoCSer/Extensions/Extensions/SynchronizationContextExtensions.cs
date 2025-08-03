using AutoCSer.Threading;
using System;
using System.Threading;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 同步上下文扩展
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SynchronizationContextExtensions
    {
        /// <summary>
        /// 同步上下文
        /// </summary>
        private readonly SynchronizationContext context;
        /// <summary>
        /// 同步上下文扩展
        /// </summary>
        /// <param name="context"></param>
        public SynchronizationContextExtensions(SynchronizationContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <param name="call"></param>
        /// <param name="state"></param>
#if NetStandard21
        public void Post(Action call, object? state = null)
#else
        public void Post(Action call, object state = null)
#endif
        {
            context.Post(new SynchronizationContextAction(call).Call, state);
        }
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="call"></param>
        /// <param name="parameter"></param>
        /// <param name="state"></param>
#if NetStandard21
        public void Post<T>(Action<T> call, T parameter, object? state = null)
#else
        public void Post<T>(Action<T> call, T parameter, object state = null)
#endif
        {
            context.Post(new SynchronizationContextAction<T>(call, parameter).Call, state);
        }
    }
}
