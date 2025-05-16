using System;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志忽略异常，不输出日志，用于测试
    /// </summary>
    public sealed class IgnoreException : Exception
    {
        /// <summary>
        /// 日志忽略异常，不输出日志，用于测试
        /// </summary>
        public IgnoreException() { }
        /// <summary>
        /// 日志忽略异常，不输出日志，用于测试
        /// </summary>
        /// <param name="message"></param>
        public IgnoreException(string message) : base(message) { }
    }
}
