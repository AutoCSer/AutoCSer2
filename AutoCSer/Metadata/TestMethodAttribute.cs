using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 测试函数
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestMethodAttribute : IgnoreMemberAttribute
    {
    }
}
