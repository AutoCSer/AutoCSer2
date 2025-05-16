using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 异步任务队列关键字类型自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Enum)]
    public sealed class CommandServerCallTaskQueueTypeAttribute : Attribute
    {
        /// <summary>
        /// 异步队列驻留超时秒数，等待指定时间以后没有新任务再删除，负数表示永久驻留内存
        /// </summary>
        public int TimeoutSeconds;
    }
}
