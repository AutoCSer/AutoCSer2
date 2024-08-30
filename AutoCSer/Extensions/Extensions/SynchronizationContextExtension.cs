using AutoCSer.Threading;
using System;
using System.Threading;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 同步上下文扩展
    /// </summary>
    public static class SynchronizationContextExtension
    {
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="call"></param>
        /// <param name="state"></param>
        public static void Post(this SynchronizationContext context, Action call, object state = null)
        {
            context.Post(new SynchronizationContextAction(call).Call, state);
        }
        /// <summary>
        /// 同步上下文调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="call"></param>
        /// <param name="parameter"></param>
        /// <param name="state"></param>
        public static void Post<T>(this SynchronizationContext context, Action<T> call, T parameter, object state = null)
        {
            context.Post(new SynchronizationContextAction<T>(call, parameter).Call, state);
        }
    }
}
