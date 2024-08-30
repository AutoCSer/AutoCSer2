using System;

namespace AutoCSer.TestCase.Common.Data
{
    /// <summary>
    /// 枚举定义
    /// </summary>
    [Flags]
    public enum SByteFlagEnum : sbyte
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 0x10,
        F = 0x20,
        G = 0x40
    }
}
