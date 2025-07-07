using AutoCSer.Extensions;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 24 位色彩
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(int))]
    public unsafe struct LockBitmapColor : IEquatable<LockBitmapColor>
    {
        /// <summary>
        /// 整数值
        /// </summary>
        [FieldOffset(0)]
        internal int Value;
        /// <summary>
        /// 蓝色
        /// </summary>
        [FieldOffset(0)]
        public readonly byte Blue;
        /// <summary>
        /// 绿色
        /// </summary>
        [FieldOffset(1)]
        public readonly byte Green;
        /// <summary>
        /// 红色
        /// </summary>
        [FieldOffset(2)]
        public readonly byte Red;
        /// <summary>
        /// 24位色彩
        /// </summary>
        /// <param name="blue"></param>
        /// <param name="green"></param>
        /// <param name="red"></param>
        public LockBitmapColor(byte blue, byte green, byte red)
        {
            Value = 0;
            Blue = blue;
            Green = green;
            Red = red;
        }
        /// <summary>
        /// 24位色彩
        /// </summary>
        /// <param name="color"></param>
        public LockBitmapColor(Color color)
        {
            Value = 0;
            Blue = color.B;
            Green = color.G;
            Red = color.R;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static implicit operator LockBitmapColor(Color color) { return new LockBitmapColor(color); }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            return Value;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Is it equal
        /// 是否相等</returns>
        public bool Equals(LockBitmapColor other)
        {
            return Value == other.Value;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Is it equal
        /// 是否相等</returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<LockBitmapColor>());
        }
        /// <summary>
        /// == 操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LockBitmapColor left, LockBitmapColor right)
        {
            return left.Value == right.Value;
        }
        /// <summary>
        /// != 操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LockBitmapColor left, LockBitmapColor right)
        {
            return left.Value != right.Value;
        }
    }
}
