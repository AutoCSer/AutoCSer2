using System;
using System.Drawing;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 字符位图生成工具
    /// </summary>
    public sealed class CharMapBuilder : IDisposable
    {
        /// <summary>
        /// 位图
        /// </summary>
        private readonly Bitmap bitmap;
        /// <summary>
        /// 画笔
        /// </summary>
        private readonly SolidBrush brush;
        /// <summary>
        /// 字体
        /// </summary>
        private readonly Font font;
        /// <summary>
        /// 字符位图生成工具
        /// </summary>
        /// <param name="fontSize">默认为 10</param>
        /// <param name="bitmapSize">默认为最大值 16</param>
        public CharMapBuilder(int fontSize = 10, int bitmapSize = 16)
        {
            bitmapSize = Math.Min(bitmapSize, 16);
            bitmap = new Bitmap(bitmapSize, bitmapSize);
            brush = new SolidBrush(Color.Black);
            font = new Font(SystemFonts.DefaultFont.Name, fontSize);
        }
        /// <summary>
        /// 字符位图生成工具
        /// </summary>
        /// <param name="font"></param>
        /// <param name="bitmapSize">默认为最大值 16</param>
        public CharMapBuilder(Font font, int bitmapSize = 16)
        {
            bitmapSize = Math.Min(bitmapSize, 16);
            bitmap = new Bitmap(bitmapSize, bitmapSize);
            brush = new SolidBrush(Color.Black);
            this.font = font;
        }
        /// <summary>
        /// 创建字符位图
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CharMap Create(char value)
        {
            using (LockBitmap lockBitmap = new LockBitmap(bitmap)) lockBitmap.Fill(Color.White);
            using (Graphics graphic = Graphics.FromImage(bitmap)) graphic.DrawString(value.ToString(), font, brush, 0, 0);
            using (LockBitmap lockBitmap = new LockBitmap(bitmap, System.Drawing.Imaging.ImageLockMode.ReadOnly))
            {
                Point point = lockBitmap.FindColorTop(Color.Black);
                int top = point.Y, left = point.X;
                if (left < 0) return CharMap.Empty;
                int nextLeft = lockBitmap.FindColorLeft(top + 1, left, Color.Black);
                if (nextLeft >= 0) left = nextLeft;
                return new CharMap(lockBitmap, (short)left, (short)top, Color.Black);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            brush.Dispose();
            bitmap.Dispose();
        }
    }
}
