using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// await Task 队列关键字配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CommandServerQueueKeyAttribute : Attribute
    {
    }
}
