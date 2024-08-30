using System;
using System.Runtime.InteropServices;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据 联合体
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    internal struct Bit128
    {
        /// <summary>
        /// int
        /// </summary>
        [FieldOffset(0)]
        internal int Int;
        /// <summary>
        /// 逻辑值
        /// </summary>
        [FieldOffset(0)]
        internal bool Bool;
        /// <summary>
        /// 时间值
        /// </summary>
        [FieldOffset(0)]
        internal DateTime DateTime;
        /// <summary>
        /// 时间值
        /// </summary>
        [FieldOffset(0)]
        internal TimeSpan TimeSpan;
        /// <summary>
        /// long
        /// </summary>
        [FieldOffset(0)]
        internal long Long;
        /// <summary>
        /// 小数
        /// </summary>
        [FieldOffset(0)]
        internal decimal Decimal;
        /// <summary>
        /// Guid
        /// </summary>
        [FieldOffset(0)]
        internal Guid Guid;
        /// <summary>
        /// byte
        /// </summary>
        [FieldOffset(0)]
        internal byte Byte;
        /// <summary>
        /// short
        /// </summary>
        [FieldOffset(0)]
        internal short Short;
        /// <summary>
        /// 浮点数
        /// </summary>
        [FieldOffset(0)]
        internal float Float;
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        [FieldOffset(0)]
        internal double Double;
    }
}
