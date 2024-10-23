using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 颜色指针
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LockBitmapColorPoint
    {
        /// <summary>
        /// 数据起始位置
        /// </summary>
        private byte* data;
        /// <summary>
        /// 最后一个颜色位置
        /// </summary>
        private readonly byte* endColor;
        /// <summary>
        /// 0 表示和左侧颜色相同
        /// </summary>
        internal uint IsLeft
        {
            get
            {
                return (*(uint*)data ^ *(uint*)(data - 3)) & 0xffffffU;
            }
        }
        /// <summary>
        /// 0 表示和右侧颜色相同
        /// </summary>
        internal uint IsRight
        {
            get
            {
                return (*(uint*)data ^ *(uint*)(data + 3)) & 0xffffffU;
            }
        }
        /// <summary>
        /// 获取色彩
        /// </summary>
        /// <returns>色彩</returns>
        internal LockBitmapColor Color
        {
            get { return LockBitmap.GetColor(data, endColor); }
            set { LockBitmap.Write(data, value); }
        }
        /// <summary>
        /// 颜色指针
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endColor">图片最后一个色彩地址，防止访问到不可访问内存</param>
        internal LockBitmapColorPoint(byte* data, byte* endColor)
        {
            this.data = data;
            this.endColor = endColor;
        }
        /// <summary>
        /// 移动到下一个颜色数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveNext()
        {
            data += 3;
        }
        /// <summary>
        /// 移动到上一个颜色数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MovePrevious()
        {
            data -= 3;
        }
        /// <summary>
        /// 移动颜色数据
        /// </summary>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Move(int count)
        {
            data += (count << 1) + count;
        }
        /// <summary>
        /// 移动颜色数据
        /// </summary>
        /// <param name="offset"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveOffset(int offset)
        {
            data += offset;
        }
        /// <summary>
        /// 获取相对颜色指针
        /// </summary>
        /// <param name="offset"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LockBitmapColorPoint GetOffset(int offset)
        {
            return new LockBitmapColorPoint(data + offset, endColor);
        }
        /// <summary>
        /// 获取相对指针颜色
        /// </summary>
        /// <param name="offset"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LockBitmapColor GetOffsetColor(int offset)
        {
            return LockBitmap.GetColor(data + offset, endColor);
        }
    }
}
