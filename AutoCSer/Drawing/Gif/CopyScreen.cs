﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 定时截屏获取图片生成 GIF 文件数据
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public sealed class CopyScreen : TimerWriter
    {
        /// <summary>
        /// 截屏开始横坐标位置
        /// </summary>
        private readonly int screenLeft;
        /// <summary>
        /// 截屏开始纵坐标位置
        /// </summary>
        private readonly int screenTop;
        /// <summary>
        /// 获取的图片
        /// </summary>
#if NetStandard21
        private Bitmap? bitmap;
#else
        private Bitmap bitmap;
#endif
        /// <summary>
        /// 定时截屏获取图片生成 GIF 文件数据
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">素数宽度</param>
        /// <param name="height">素数高度</param>
        /// <param name="globalColors">全局颜色列表</param>
        /// <param name="backgroundColorIndex">背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义</param>
        /// <param name="isLeaveDisposeStream">是否自动释放输出数据流</param>
        /// <param name="interval">获取图片定时毫秒数，最小值为 40</param>
        /// <param name="maxPixel">最大色彩深度，2-8</param>
        /// <param name="screenLeft">截屏开始横坐标位置默认为最小值 0</param>
        /// <param name="screenTop">截屏开始纵坐标位置默认为最小值 0</param>
#if NetStandard21
        public CopyScreen(Stream stream, short width, short height, LockBitmapColor[]? globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false, int interval = 40, byte maxPixel = 8, int screenLeft = 0, int screenTop = 0)
#else
        public CopyScreen(Stream stream, short width, short height, LockBitmapColor[] globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false, int interval = 40, byte maxPixel = 8, int screenLeft = 0, int screenTop = 0)
#endif
            : base(stream, width, height, globalColors, backgroundColorIndex, isLeaveDisposeStream, interval, maxPixel, false)
        {
            this.screenLeft = Math.Max(screenLeft, 0);
            this.screenTop = Math.Max(screenTop, 0);
            start();
        }
        /// <summary>
        /// 释放图片资源
        /// </summary>
        protected override void disposeBitmap()
        {
            base.disposeBitmap();
            bitmap?.Dispose();
        }
        /// <summary>
        /// 释放图片资源
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="bitmapData"></param>
#if NetStandard21
        protected override void disposeBitmap(Bitmap? bitmap, BitmapData? bitmapData)
#else
        protected override void disposeBitmap(Bitmap bitmap, BitmapData bitmapData)
#endif
        {
            if (bitmap != null)
            {
                if (bitmapData != null) bitmap.UnlockBits(bitmapData);
                this.bitmap?.Dispose();
                this.bitmap = bitmap;
            }
        }
        /// <summary>
        /// 定时获取图片
        /// </summary>
        /// <returns></returns>
        protected override Bitmap getBitmap()
        {
            Bitmap bitmap = this.bitmap ?? new Bitmap(Width, Height);
            this.bitmap = null;
            bool isCopy = false;
            try
            {
                using (Graphics graphics = Graphics.FromImage(bitmap)) graphics.CopyFromScreen(screenLeft, screenTop, 0, 0, new Size(Width, Height));
                isCopy = true;
            }
            finally
            {
                if (!isCopy) bitmap.Dispose();
            }
            return bitmap;
        }

        /// <summary>
        /// 检查屏幕分辨率
        /// </summary>
        /// <param name="maxWidth">最大检查宽度为 10240 像素</param>
        /// <param name="maxHeight">最大检查高度为 5400 像素</param>
        /// <returns></returns>
        public static unsafe Size CheckSize(int maxWidth = 10240, int maxHeight = 5400)
        {
            int width = Math.Max(maxWidth, 1), height = Math.Max(maxHeight, 1);
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap)) graphics.CopyFromScreen(0, 0, 0, 0, new Size(bitmap.Width, bitmap.Height));
                using (LockBitmap lockBitmap = new LockBitmap(bitmap, ImageLockMode.ReadOnly))
                {
                    height = lockBitmap.FindBottom(default(LockBitmapColor));
                    width = lockBitmap.FindRight(height, default(LockBitmapColor));
                }
            }
            return new Size(width, height);
        }
    }
}
