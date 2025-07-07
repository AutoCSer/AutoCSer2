using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Empty callback
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class EmptyCommandServerCallback<T> : CommandServerCallback<T>
    {
        /// <summary>
        /// Empty callback
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool Callback(T returnValue)
        {
            return true;
        }
        /// <summary>
        /// 同步回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal override bool SynchronousCallback(T returnValue)
        {
            return true;
        }

        /// <summary>
        /// Empty callback
        /// </summary>
        internal static readonly EmptyCommandServerCallback<T> Default = new EmptyCommandServerCallback<T>();
    }
}
