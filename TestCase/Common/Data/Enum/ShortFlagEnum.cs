using System;

namespace AutoCSer.TestCase.Common.Data
{
    /// <summary>
    /// 枚举定义
    /// </summary>
    [Flags]
    public enum ShortFlagEnum : short
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 0x10,
        F = 0x20,
        G = 0x40,
        H = 0x80,
        I = 0x100,
        J = 0x200,
        K = 0x400,
        L = 0x800,
        M = 0x1000,
        N = 0x2000,
        O = 0x4000
    }
}
