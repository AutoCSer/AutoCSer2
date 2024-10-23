using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 位图填充
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct FillBitmap
    {
        /// <summary>
        /// 当前颜色索引
        /// </summary>
        private byte* currentIndex;
        /// <summary>
        /// 颜色列表
        /// </summary>
        private readonly LockBitmapColor* colors;
        /// <summary>
        /// 图像宽度
        /// </summary>
        private readonly int width;
        /// <summary>
        /// 位图填充
        /// </summary>
        /// <param name="width"></param>
        /// <param name="currentIndex"></param>
        /// <param name="colors"></param>
        internal FillBitmap(int width, byte* currentIndex, LockBitmapColor* colors = null)
        {
            this.width = width;
            this.currentIndex = currentIndex;
            this.colors = colors;
        }
        /// <summary>
        /// 填充颜色列表
        /// </summary>
        /// <param name="height">填充行数</param>
        /// <param name="bitmap">位图当前填充位置</param>
        /// <param name="bitMapSpace">位图填充留空</param>
        internal void FillColor(int height, byte* bitmap, int bitMapSpace)
        {
            byte* row = currentIndex;
            for (byte* rowEnd = currentIndex + width * height; row != rowEnd; bitmap += bitMapSpace)
            {
                byte* col = row;
                for (row += width; col != row; ++col)
                {
                    LockBitmap.Write(bitmap, colors[*col]);
                    bitmap += 3;
                }
            }
            currentIndex = row;
        }
        /// <summary>
        /// 填充颜色索引
        /// </summary>
        /// <param name="height">填充行数</param>
        /// <param name="bitmap">位图当前填充位置</param>
        /// <param name="bitMapSpace">位图填充留空</param>
        internal void FillIndex(int height, byte* bitmap, int bitMapSpace)
        {
            byte* row = currentIndex;
            for (byte* rowEnd = currentIndex + width * height; row != rowEnd; bitmap += bitMapSpace)
            {
                byte* col = row;
                for (row += width; col != row; ++col)
                {
                    byte color = *col;
                    *bitmap++ = color;
                    *bitmap++ = color;
                    *bitmap++ = color;
                }
            }
            currentIndex = row;
        }
    }
}
