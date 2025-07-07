using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 定时获取图片生成 GIF 文件数据
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public abstract class TimerWriter : Writer
    {
        /// <summary>
        /// 获取图片定时器
        /// </summary>
#if NetStandard21
        private System.Threading.Timer? timer;
#else
        private System.Threading.Timer timer;
#endif
        /// <summary>
        /// 截屏定时毫秒数
        /// </summary>
        private readonly int interval;
        /// <summary>
        /// 图片间隔毫秒数
        /// </summary>
        private int keepInterval;
        /// <summary>
        /// 最后一次获取的图片
        /// </summary>
#if NetStandard21
        private Bitmap? lastBitmap;
#else
        private Bitmap lastBitmap;
#endif
        /// <summary>
        /// 最后一次获取的图片数据
        /// </summary>
#if NetStandard21
        private BitmapData? lastBitmapData;
#else
        private BitmapData lastBitmapData;
#endif
        /// <summary>
        /// 跳图数量
        /// </summary>
        private int keepCount;
        /// <summary>
        /// 是否正在获取图片
        /// </summary>
        private int isTimer;
        /// <summary>
        /// 最大色彩深度
        /// </summary>
        private readonly byte maxPixel;
        /// <summary>
        /// 定时获取图片生成 GIF 文件
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">素数宽度</param>
        /// <param name="height">素数高度</param>
        /// <param name="globalColors">全局颜色列表</param>
        /// <param name="backgroundColorIndex">背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义</param>
        /// <param name="isLeaveDisposeStream">是否自动释放输出数据流</param>
        /// <param name="interval">获取图片定时毫秒数默认最小值为 40</param>
        /// <param name="maxPixel">最大色彩深度默认为最大值 8，最小值为 2</param>
        /// <param name="isStart">默认为 true 表示开始</param>
#if NetStandard21
        protected TimerWriter(Stream stream, short width, short height, LockBitmapColor[]? globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false, int interval = 40, byte maxPixel = 8, bool isStart = true)
#else
        protected TimerWriter(Stream stream, short width, short height, LockBitmapColor[] globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false, int interval = 40, byte maxPixel = 8, bool isStart = true)
#endif
            : base(stream, width, height, globalColors, backgroundColorIndex, isLeaveDisposeStream)
        {
            this.maxPixel = (byte)(maxPixel - 2) < 8 ? maxPixel : (byte)8;
            this.interval = Math.Max(interval, 40);
            keepInterval = -interval;
            if (isStart) start();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        protected override void dispose()
        {
            if (timer != null) timer.Dispose();
            while (Interlocked.CompareExchange(ref isTimer, 1, 0) != 0) Thread.Sleep(1);
            disposeBitmap();
            base.dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
        protected override async Task disposeAsync()
        {
            if (timer != null) await timer.DisposeAsync();
            while (Interlocked.CompareExchange(ref isTimer, 1, 0) != 0) await Task.Delay(1);
            disposeBitmap();
            await base.disposeAsync();
        }
        /// <summary>
        /// 释放图片资源
        /// </summary>
        protected virtual void disposeBitmap()
        {
            disposeBitmap(lastBitmap, lastBitmapData);
        }
        /// <summary>
        /// 释放图片资源
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="bitmapData"></param>
#if NetStandard21
        protected virtual void disposeBitmap(Bitmap? bitmap, BitmapData? bitmapData)
#else
        protected virtual void disposeBitmap(Bitmap bitmap, BitmapData bitmapData)
#endif
        {
            if (bitmap != null)
            {
                if (bitmapData != null) bitmap.UnlockBits(bitmapData);
                bitmap.Dispose();
            }
        }
        /// <summary>
        /// 设置获取图片定时器并开始获取图片
        /// </summary>
        protected void start()
        {
            timer = new System.Threading.Timer(onTimer, this, interval, interval);
            onTimer(null);
        }
        /// <summary>
        /// 定时获取图片
        /// </summary>
        /// <param name="state"></param>
#if NetStandard21
        private unsafe void onTimer(object? state)
#else
        private unsafe void onTimer(object state)
#endif
        {
            if (Interlocked.Exchange(ref isTimer, 1) == 0 && isDisposed == 0)
            {
                var bitmap = default(Bitmap);
                var bitmapData = default(BitmapData);
                try
                {
                    int keepCount = Interlocked.Exchange(ref this.keepCount, 0);
                    keepInterval += keepCount == 0 ? interval : ((keepCount + 1) * interval);
                    bitmap = getBitmap();
                    int left = 0, top = 0, right = Math.Min(Width, bitmap.Width), bottom = Math.Min(Height, bitmap.Height);
                    bitmapData = bitmap.LockBits(new Rectangle(left, top, right, bottom), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    if (lastBitmapData != null)
                    {
                        byte* lastBitmapFixed = (byte*)lastBitmapData.Scan0, bitmapFixed = (byte*)bitmapData.Scan0;
                        var lastBitmap = this.lastBitmap.notNull();
                        int minHeight = bitmap.Height <= lastBitmap.Height ? bitmap.Height : lastBitmap.Height;
                        int minWidth = lastBitmap.Width, width3;
                        if (bitmap.Width <= lastBitmap.Width)
                        {
                            minWidth = bitmap.Width;
                            width3 = (minWidth << 1) + minWidth;
                            for (byte* lastRow = lastBitmapFixed, currentRow = bitmapFixed; top != minHeight; ++top)
                            {
                                if (!AutoCSer.Memory.Common.SequenceEqual(lastRow, currentRow, width3)) break;
                                lastRow += lastBitmapData.Stride;
                                currentRow += bitmapData.Stride;
                            }
                            if (bitmap.Height <= lastBitmap.Height && top != minHeight)
                            {
                                ++top;
                                for (byte* lastRow = lastBitmapFixed + lastBitmapData.Stride * minHeight, currentRow = bitmapFixed + bitmapData.Stride * minHeight; top != bottom; --bottom)
                                {
                                    if (!AutoCSer.Memory.Common.SequenceEqual(lastRow -= lastBitmapData.Stride, currentRow -= bitmapData.Stride, width3)) break;
                                }
                                --top;
                            }
                        }
                        if (bitmap.Height <= lastBitmap.Height && top != minHeight)
                        {
                            width3 = (minWidth << 1) + minWidth;
                            int endRowStride = lastBitmapData.Stride * (bottom - top);
                            byte* lastTopRow = lastBitmapFixed + lastBitmapData.Stride * top, currentTopRow = bitmapFixed + bitmapData.Stride * top;
                            if ((((int)lastBitmapFixed & (sizeof(ulong) - 1)) | (lastBitmapData.Stride & (sizeof(ulong) - 1)) | ((int)bitmapFixed & (sizeof(ulong) - 1)) | (bitmapData.Stride & (sizeof(ulong) - 1))) == 0)
                            {
                                byte* lastTopCol = lastTopRow, topColEnd = lastTopRow + width3 - 1;
                                ulong color = 0;
                                for (byte* currentTopCol = currentTopRow; lastTopCol <= topColEnd; lastTopCol += sizeof(ulong), currentTopCol += sizeof(ulong))
                                {
                                    color = 0;
                                    for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += bitmapData.Stride)
                                    {
                                        color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                    }
                                    if (color != 0) break;
                                }
                                int length = (int)(lastTopCol - lastTopRow);
                                if (lastTopCol <= topColEnd) length += color.endBits() >> 3;
                                left += (length /= 3) < minWidth ? length : minWidth;
                                if (bitmap.Width <= lastBitmap.Width && left != minWidth)
                                {
                                    int offset = width3 & (sizeof(ulong) - 1);
                                    byte* currentTopCol = currentTopRow + width3;
                                    lastTopCol = lastTopRow + width3;
                                    length = 0;
                                    if (offset != 0)
                                    {
                                        currentTopCol -= offset;
                                        lastTopCol -= offset;
                                        color = 0;
                                        for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += bitmapData.Stride)
                                        {
                                            color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                        }
                                        if ((color <<= ((sizeof(ulong) - offset) << 3)) == 0) length = offset;
                                        else length = ((sizeof(ulong) << 3) - color.bits()) >> 3;
                                    }
                                    if (length == offset)
                                    {
                                        topColEnd = lastTopCol;
                                        do
                                        {
                                            color = 0;
                                            for (byte* lastRow = (lastTopCol -= sizeof(ulong)), currentRow = (currentTopCol -= sizeof(ulong)), endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += bitmapData.Stride)
                                            {
                                                color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                            }
                                        }
                                        while (color == 0);
                                        length += (int)(topColEnd - lastTopCol) - sizeof(ulong) + ((sizeof(ulong) << 3) - color.bits()) >> 3;
                                    }
                                    right -= length / 3;
                                }
                            }
                            else
                            {
                                for (byte* lastTopCol = lastTopRow, topColEnd = lastTopRow + width3, currentTopCol = currentTopRow; lastTopCol != topColEnd; lastTopCol += 3, currentTopCol += 3, ++left)
                                {
                                    int color = 0;
                                    for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += bitmapData.Stride)
                                    {
                                        color |= *(int*)lastRow ^ *(int*)currentRow;
                                    }
                                    if ((color & 0xffffff) != 0) break;
                                }
                                if (bitmap.Width <= lastBitmap.Width && left != minWidth)
                                {
                                    byte* lastTopCol = lastTopRow + width3, currentTopCol = currentTopRow + width3;
                                    do
                                    {
                                        int color = 0;
                                        for (byte* lastRow = (lastTopCol -= 3), currentRow = (currentTopCol -= 3), endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += bitmapData.Stride)
                                        {
                                            color |= *(int*)lastRow ^ *(int*)currentRow;
                                        }
                                        if ((color & 0xffffff) != 0) break;
                                        --right;
                                    }
                                    while (true);
                                }
                            }
                        }
                    }
                    if (top != bottom)
                    {
                        int delayTime = keepInterval / 10;
                        for (keepInterval -= delayTime * 10; delayTime > short.MaxValue; delayTime -= short.MaxValue)
                        {
                            if (!AddGraphicControl(short.MaxValue, GraphicControlMethodTypeEnum.None, true)) return;
                            //addImage(lastBitmapData, 0, 0, 0, 0, 1, 1, false, maxPixel);
                        }
                        if (delayTime != 0 && !AddGraphicControl((short)delayTime, GraphicControlMethodTypeEnum.None, true)) return;
                        addImage(bitmapData, left, top, left, top, right - left, bottom - top, false, maxPixel);
                        disposeBitmap(lastBitmap, lastBitmapData);
                        lastBitmapData = bitmapData;
                        lastBitmap = bitmap;
                        bitmapData = null;
                        bitmap = null;
                    }
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    disposeBitmap(bitmap, bitmapData);
                    Interlocked.Exchange(ref isTimer, 0);
                }
            }
            else Interlocked.Increment(ref keepCount);
        }
        /// <summary>
        /// 定时获取图片
        /// </summary>
        /// <returns></returns>
        protected abstract Bitmap getBitmap();
    }
}
