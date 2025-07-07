using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 锁住的位图，用于指针操作
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public sealed unsafe class LockBitmap : IDisposable
    {
        /// <summary>
        /// 位图
        /// </summary>
        internal readonly Bitmap Bitmap;
        /// <summary>
        /// Bitmap data
        /// 位图数据
        /// </summary>
        private readonly BitmapData bitmapData;
        /// <summary>
        /// 位图数据起始位置
        /// </summary>
        internal readonly byte* Data;
        /// <summary>
        /// 每行数据字节数
        /// </summary>
        internal readonly int Stride;
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return Bitmap.Width; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return Bitmap.Height; }
        }
        /// <summary>
        /// 最后一个颜色位置
        /// </summary>
        private byte* endColor
        {
            get { return Data + (Bitmap.Height * Stride - 3); }
        }
        /// <summary>
        /// 获取色彩
        /// </summary>
        /// <param name="top">顶部大小</param>
        /// <param name="left">左部大小</param>
        /// <returns>色彩</returns>
        internal LockBitmapColor this[int top, int left]
        {
            get
            {
                return GetColor(Data + Stride * top + left * 3, endColor);
            }
        }
        /// <summary>
        /// 颜色指针
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        internal LockBitmapColorPoint this[int top]
        {
            get { return new LockBitmapColorPoint(Data + Stride * top, endColor); }
        }
        /// <summary>
        /// 锁住的位图
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="mode"></param>
        public LockBitmap(Bitmap bitmap, ImageLockMode mode = ImageLockMode.ReadWrite)
        {
            this.Bitmap = bitmap;
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), mode, PixelFormat.Format24bppRgb);
            Data = (byte*)bitmapData.Scan0;
            Stride = bitmapData.Stride;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            Bitmap.UnlockBits(bitmapData);
        }
        /// <summary>
        /// 检查左右位置
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private bool checkLeftWidth(ref int left, ref int width)
        {
            if (left < 0)
            {
                width += left;
                left = 0;
            }
            width = Math.Min(width, Bitmap.Width - left);
            return width > 0;
        }
        /// <summary>
        /// 画横线
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void HorizontalLine(int top, int left, int width, LockBitmapColor color)
        {
            if (top >= 0 && top < Bitmap.Height && checkLeftWidth(ref left, ref width)) horizontalLine(top, left, width, color);
        }
        /// <summary>
        /// 画横线
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        private void horizontalLine(int top, int left, int width, LockBitmapColor color)
        {
            byte* data = Data + Stride * top + left * 3;
            for (byte* end = data + (width - 1) * 3; data != end; data += 3) *(int*)data = color.Value;
            Write(data, color);
        }
        /// <summary>
        /// 改变横线中指定的颜色
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void HorizontalLineChangeColor(int top, int left, int width, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            if (top >= 0 && top < Bitmap.Height && checkLeftWidth(ref left, ref width)) horizontalLineChangeColor(top, left, width, sourceColor, setColor);
        }
        /// <summary>
        /// 改变横线中指定的颜色
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        private void horizontalLineChangeColor(int top, int left, int width, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            byte* data = Data + Stride * top + left * 3, end = data + width * 3;
            do
            {
                changeColor(data, sourceColor, setColor);
            }
            while ((data += 3) != end);
        }
        /// <summary>
        /// 改变指定的颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void changeColor(byte* data, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            if (getColor(data, endColor) == sourceColor.Value) Write(data, setColor);
        }
        /// <summary>
        /// 画虚横线
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DotHorizontalLine(int top, int left, int width, LockBitmapColor color)
        {
            if (top >= 0 && top < Bitmap.Height && checkLeftWidth(ref left, ref width)) dotHorizontalLine(top, left, width, color);
        }
        /// <summary>
        /// 画虚横线
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        private void dotHorizontalLine(int top, int left, int width, LockBitmapColor color)
        {
            byte* data = Data + Stride * top + left * 3;
            for (byte* end = data + (width >> 1) * 6; data != end; data += 6) Write(data, color);
            if ((width & 1) == 0) data -= 3;
            Write(data, color);
        }
        /// <summary>
        /// 改变虚横线中指定的颜色
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DotHorizontalLineChangeColor(int top, int left, int width, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            if (top >= 0 && top < Bitmap.Height && checkLeftWidth(ref left, ref width)) dotHorizontalLineChangeColor(top, left, width, sourceColor, setColor);
        }
        /// <summary>
        /// 改变虚横线中指定的颜色
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        private void dotHorizontalLineChangeColor(int top, int left, int width, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            byte* data = Data + Stride * top + left * 3;
            for (byte* end = data + (width >> 1) * 6; data != end; data += 6)
            {
                changeColor(data, sourceColor, setColor);
            }
            if ((width & 1) == 0) data -= 3;
            changeColor(data, sourceColor, setColor);
        }
        /// <summary>
        /// 检查顶底位置
        /// </summary>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private bool checkTopHeight(ref int top, ref int height)
        {
            if (top < 0)
            {
                height += top;
                top = 0;
            }
            height = Math.Min(height, Bitmap.Height - top);
            return height > 0;
        }
        /// <summary>
        /// 画竖线
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void VerticalLine(int left, int top, int height, LockBitmapColor color)
        {
            if (left >= 0 && left < Bitmap.Width && checkTopHeight(ref top, ref height)) verticalLine(left, top, height, color);
        }
        /// <summary>
        /// 画竖线
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void verticalLine(int left, int top, int height, LockBitmapColor color)
        {
            byte* data = Data + Stride * top + left * 3, end = data + Stride * height;
            do
            {
                Write(data, color);
            }
            while ((data += Stride) != end);
        }
        /// <summary>
        /// 改变竖线中指定的颜色
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void VerticalLineChangeColor(int left, int top, int height, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            if (left >= 0 && left < Bitmap.Width && checkTopHeight(ref top, ref height)) verticalLineChangeColor(left, top, height, sourceColor, setColor);
        }
        /// <summary>
        /// 改变竖线中指定的颜色
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        private void verticalLineChangeColor(int left, int top, int height, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            byte* data = Data + Stride * top + left * 3, end = data + Stride * height;
            do
            {
                changeColor(data, sourceColor, setColor);
            }
            while ((data += Stride) != end);
        }
        /// <summary>
        /// 画平行竖线
        /// </summary>
        /// <param name="left1"></param>
        /// <param name="left2"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void VerticalLine2(int left1, int left2, int top, int height, LockBitmapColor color)
        {
            bool isLeft = left1 >= 0 && left1 < Bitmap.Width, isRight = left2 >= 0 && left2 < Bitmap.Width && left1 != left2;
            if ((isLeft || isRight) && checkTopHeight(ref top, ref height))
            {
                if (isLeft)
                {
                    if (isRight) verticalLine2(left1, left2, top, height, color);
                    else verticalLine(left1, top, height, color);
                }
                else verticalLine(left2, top, height, color);
            }
        }
        /// <summary>
        /// 画平行竖线
        /// </summary>
        /// <param name="left1"></param>
        /// <param name="left2"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void verticalLine2(int left1, int left2, int top, int height, LockBitmapColor color)
        {
            byte* data = Data + Stride * top + left1 * 3, end = data + Stride * height;
            int width = (left2 - left1) * 3;
            do
            {
                Write(data, color);
                Write(data + width, color);
            }
            while ((data += Stride) != end);
        }
        /// <summary>
        /// 画空心矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Rectangle(int left, int width, int top, int height, LockBitmapColor color)
        {
            if (height > 0 && width > 0) rectangle(left, width, top, height, color);
        }
        /// <summary>
        /// 画空心矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void rectangle(int left, int width, int top, int height, LockBitmapColor color)
        {
            HorizontalLine(top, left, width, color);
            if (height != 1)
            {
                HorizontalLine(top + height - 1, left, width, color);
                if ((height -= 2) != 0)
                {
                    top += 1;
                    if (width != 1) VerticalLine2(left, left + width - 1, top, height, color);
                    else VerticalLine(left, top, height, color);
                }
            }
        }
        /// <summary>
        /// 设置边框色彩值
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetBorder(LockBitmapColor color)
        {
            rectangle(0, Bitmap.Width, 0, Bitmap.Height, color);
        }
        /// <summary>
        /// 画实心矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SolidRectangle(int left, int width, int top, int height, LockBitmapColor color)
        {
            if (checkLeftWidth(ref left, ref width) && checkTopHeight(ref top, ref height)) solidRectangle(left, width, top, height, color);
        }
        /// <summary>
        /// 画实心矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void solidRectangle(int left, int width, int top, int height, LockBitmapColor color)
        {
            int bottom = top + height;
            do
            {
                horizontalLine(top, left, width, color);
            }
            while (++top != bottom);
        }
        /// <summary>
        /// 改变实心矩形中指定的颜色
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SolidRectangleChangeColor(int left, int width, int top, int height, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            if (checkLeftWidth(ref left, ref width) && checkTopHeight(ref top, ref height)) solidRectangleChangeColor(left, width, top, height, sourceColor, setColor);
        }
        /// <summary>
        /// 改变实心矩形中指定的颜色
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="sourceColor"></param>
        /// <param name="setColor"></param>
        private void solidRectangleChangeColor(int left, int width, int top, int height, LockBitmapColor sourceColor, LockBitmapColor setColor)
        {
            int bottom = top + height;
            do
            {
                horizontalLineChangeColor(top, left, width, sourceColor, setColor);
            }
            while (++top != bottom);
        }
        /// <summary>
        /// 填充颜色
        /// </summary>
        /// <param name="color"></param>
        public void Fill(LockBitmapColor color)
        {
            solidRectangle(0, Bitmap.Width, 0, Bitmap.Height, color);
        }
        /// <summary>
        /// 画直线
        /// </summary>
        /// <param name="left1"></param>
        /// <param name="top1"></param>
        /// <param name="left2"></param>
        /// <param name="top2"></param>
        /// <param name="color"></param>
        public void Line(int left1, int top1, int left2, int top2, LockBitmapColor color)
        {
            int width = left2 - left1, height = top2 - top1;
            if (height != 0)
            {
                if (width != 0)
                {
                    if (Math.Abs(width) > Math.Abs(height))
                    {
                        if (left1 < left2) horizontalLine(left1, top1, left2, top2, color);
                        else horizontalLine(left2, top2, left1, top1, color);
                    }
                    else
                    {
                        if (top1 < top2) verticalLine(left1, top1, left2, top2, color);
                        else verticalLine(left2, top2, left1, top1, color);
                    }
                }
                else VerticalLine(left1, Math.Min(top1, top2), Math.Abs(height) + 1, color);
            }
            else HorizontalLine(top1, Math.Min(left1, left2), Math.Abs(width) + 1, color);
        }
        /// <summary>
        /// 横线方向画直线
        /// </summary>
        /// <param name="left1"></param>
        /// <param name="top1"></param>
        /// <param name="left2"></param>
        /// <param name="top2"></param>
        /// <param name="color"></param>
        private void horizontalLine(int left1, int top1, int left2, int top2, LockBitmapColor color)
        {
            if (left1 >= Bitmap.Width) return;
            int height = Math.Abs(top2 - top1), diff = Math.Abs(left2 - left1) - height, sum = -(++height);
            if (top1 < top2)
            {
                if (top1 >= Bitmap.Height) return;
                left2 = Math.Min(left2 + 1, Bitmap.Width);
                while (top1 < 0)
                {
                    ++left1;
                    for (sum += diff; sum >= 0; sum -= height) ++left1;
                    if (left1 >= left2) return;
                    ++top1;
                }
                top2 = Math.Min(top2 + 1, Bitmap.Height);
            }
            else
            {
                if (top1 < 0) return;
                left2 = Math.Min(left2 + 1, Bitmap.Width);
                while (top1 >= Bitmap.Height)
                {
                    ++left1;
                    for (sum += diff; sum >= 0; sum -= height) ++left1;
                    if (left1 >= left2) return;
                    --top1;
                }
                top2 = Math.Max(top2 - 1, -1);
            }
            int addTop = top1 < top2 ? 1 : -1, stride = top1 < top2 ? Stride + 3 : (3 - Stride);
            byte* data = Data + Stride * top1 + left1 * 3;
            do
            {
                if (++left1 == left2)
                {
                    if (left1 > 0) Write(data, color);
                    return;
                }
                for (sum += diff; sum >= 0; sum -= height)
                {
                    if (left1 > 0) *(int*)data = color.Value;
                    data += 3;
                    if (++left1 == left2)
                    {
                        if (left1 > 0) Write(data, color);
                        return;
                    }
                }
                if (left1 > 0) Write(data, color);
                data += stride;
            }
            while ((top1 += addTop) != top2);
        }
        /// <summary>
        /// 竖线方向画直线
        /// </summary>
        /// <param name="left1"></param>
        /// <param name="top1"></param>
        /// <param name="left2"></param>
        /// <param name="top2"></param>
        /// <param name="color"></param>
        private void verticalLine(int left1, int top1, int left2, int top2, LockBitmapColor color)
        {
            if (top1 >= Bitmap.Height) return;
            int width = Math.Abs(left2 - left1), diff = Math.Abs(top2 - top1) - width, sum = -(++width);
            if (left1 < left2)
            {
                if (left1 >= Bitmap.Width) return;
                top2 = Math.Min(top2 + 1, Bitmap.Height);
                while (left1 < 0)
                {
                    ++top1;
                    for (sum += diff; sum >= 0; sum -= width) ++top1;
                    if (top1 >= top2) return;
                    ++left1;
                }
                left2 = Math.Min(left2 + 1, Bitmap.Width);
            }
            else
            {
                if (left1 < 0) return;
                top2 = Math.Min(top2 + 1, Bitmap.Height);
                while (left1 >= Bitmap.Width)
                {
                    ++top1;
                    for (sum += diff; sum >= 0; sum -= width) ++top1;
                    if (top1 >= top2) return;
                    --left1;
                }
                left2 = Math.Max(left2 - 1, -1);
            }
            int addLeft = left1 < left2 ? 1 : -1, stride = left1 < left2 ? Stride + 3 : (Stride - 3);
            byte* data = Data + Stride * top1 + left1 * 3;
            do
            {
                sum += diff;
                do
                {
                    if (top1 >= 0) Write(data, color);
                    if (++top1 == top2) return;
                    if (sum < 0)
                    {
                        data += stride;
                        break;
                    }
                    data += Stride;
                    sum -= width;
                }
                while (true);
            }
            while ((left1 += addLeft) != left2);
        }
        /// <summary>
        /// 查找颜色所在的第一个行位置，从 0 开始
        /// </summary>
        /// <param name="color"></param>
        /// <returns>失败返回 -1</returns>
        public Point FindColorTop(LockBitmapColor color)
        {
            int top = 0;
            byte* start = Data, endColor = this.endColor;
            do
            {
                byte* data = start, end = data + Bitmap.Width * 3;
                do
                {
                    if (getColor(data, endColor) == color.Value) return new Point((int)(data - start) / 3, top);
                }
                while ((data += 3) != end);
                start += Stride;
            }
            while (++top != Bitmap.Height);
            return new Point(-1, -1);
        }
        /// <summary>
        /// 查找颜色所在的第一个列位置，从 0 开始
        /// </summary>
        /// <param name="top">查找起始行位置</param>
        /// <param name="width">查找宽度</param>
        /// <param name="color"></param>
        /// <returns>失败返回 -1</returns>
        public int FindColorLeft(int top, int width, LockBitmapColor color)
        {
            if (top >= 0 && top < Bitmap.Height && width > 0)
            {
                width = Math.Max(width, Bitmap.Width);
                byte* start = Data + Stride * top, endColor = this.endColor;
                int left = 0, stride = Stride * (Bitmap.Height - top);
                do
                {
                    byte* data = start, end = start + stride;
                    do
                    {
                        if (getColor(data, endColor) == color.Value) return left;
                    }
                    while ((data += Stride) != end);
                    start += 3;
                }
                while (++left != width);
            }
            return -1;
        }
        /// <summary>
        /// 读取颜色位图
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        internal ulong ReadColorMap(int left, int top, LockBitmapColor color, out int width)
        {
            if (top < Bitmap.Height)
            {
                ulong map = 0, bit = 1;
                byte* start = Data + Stride * top + left * 3, data = start, end = data + Math.Min((Bitmap.Width - left) * 3, 64 * 3), endColor = this.endColor, rightData = null;
                do
                {
                    if (getColor(data, endColor) == color.Value)
                    {
                        map |= bit;
                        rightData = data;
                    }
                    bit <<= 1;
                }
                while ((data += 3) != end);
                width = map != 0 ? (int)((rightData - start) / 3) + 1 : 0;
                return map;
            }
            width = 0;
            return 0;
        }
        /// <summary>
        /// 写入颜色位图
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="map"></param>
        /// <param name="color"></param>
        internal void WriteColorMap(int left, int top, ulong map, LockBitmapColor color)
        {
            if (top >= 0 && top < Bitmap.Height)
            {
                byte* data = Data + Stride * top, end = data + Bitmap.Width * 3;
                if (left >= 0) data += left * 3;
                else
                {
                    map >>= -left;
                    left = 0;
                }
                do
                {
                    if ((map & 1) != 0) Write(data, color);
                }
                while ((map >>= 1) != 0 && (data += 3) != end);
            }
        }
        /// <summary>
        /// 根据空白颜色查找图片底部行号
        /// </summary>
        /// <param name="nullColor">空白颜色</param>
        /// <returns>Failure and return 0
        /// 失败返回 0</returns>
        internal int FindBottom(LockBitmapColor nullColor)
        {
            int top = Bitmap.Height;
            byte* start = Data + top * Stride, bitmapEnd = start - 3;
            do
            {
                byte* data = (start -= Stride), end = data + Bitmap.Width * 3;
                do
                {
                    if (getColor(data, bitmapEnd) != nullColor.Value) return top;
                }
                while ((data += 3) != end);
            }
            while (--top != 0);
            return 0;
        }
        /// <summary>
        /// 根据空白颜色查找图片右侧列号
        /// </summary>
        /// <param name="height"></param>
        /// <param name="nullColor">空白颜色</param>
        /// <returns></returns>
        internal int FindRight(int height, LockBitmapColor nullColor)
        {
            if (height != 0)
            {
                int right = 0;
                byte* start = Data, endColor = this.endColor;
                do
                {
                    int checkRight = Bitmap.Width;
                    byte* data = start + checkRight * 3, end = start + right * 3;
                    do
                    {
                        if (getColor(data -= 3, endColor) != nullColor.Value)
                        {
                            right = checkRight;
                            break;
                        }
                    }
                    while (--checkRight != right);
                    start += Stride;
                }
                while (--height != 0 && right != Bitmap.Width);
                return right;
            }
            return 0;
        }
        /// <summary>
        /// 写入 24 位色彩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Write(byte* data, LockBitmapColor color)
        {
            *data = color.Blue;
            *(data + 1) = color.Green;
            *(data + 2) = color.Red;
        }
        /// <summary>
        /// 获取 24 位色彩
        /// </summary>
        /// <param name="color">色彩地址</param>
        /// <param name="endColor">图片最后一个色彩地址，防止访问到不可访问内存</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int getColor(byte* color, byte* endColor)
        {
            return color != endColor ? ((*(int*)color) & 0xffffff) : ((*(int*)(color - 1)) >> 8);
        }
        /// <summary>
        /// 根据指针获取 24 位色彩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static LockBitmapColor GetColor(byte* data)
        {
            LockBitmapColor color = *(LockBitmapColor*)data;
            color.Value &= 0xffffff;
            return color;
        }
        /// <summary>
        /// 根据指针获取 24 位色彩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endColor">图片最后一个色彩地址，防止访问到不可访问内存</param>
        /// <returns></returns>
        internal static LockBitmapColor GetColor(byte* data, byte* endColor)
        {
            if (data != endColor) return GetColor(data);
            LockBitmapColor color = *(LockBitmapColor*)(data - 1);
            color.Value >>= 8;
            return color;
        }
    }
}
