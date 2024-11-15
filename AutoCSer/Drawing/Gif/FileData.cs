using System;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF 文件解析数据
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public sealed class FileData
    {
        /// <summary>
        /// 素数宽度
        /// </summary>
        public readonly short Width;
        /// <summary>
        /// 素数高度
        /// </summary>
        public readonly short Height;
        /// <summary>
        /// 颜色深度 1-8
        /// </summary>
        public readonly byte ColorResoluTion;
        /// <summary>
        /// 全局颜色列表是否分类排列
        /// </summary>
        public readonly byte SortFlag;
        /// <summary>
        /// 背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义
        /// </summary>
        public readonly byte BackgroundColorIndex;
        /// <summary>
        /// 像素宽高比
        /// </summary>
        public readonly byte PixelAspectRadio;
        /// <summary>
        /// 全局颜色列表
        /// </summary>
        public readonly LockBitmapColor[] GlobalColors;
        /// <summary>
        /// 数据块集合
        /// </summary>
        private LeftArray<DataBlock> blocks = new LeftArray<DataBlock>(0);
        /// <summary>
        /// 数据块集合
        /// </summary>
        public LeftArray<DataBlock> Blocks { get { return blocks; } }
        /// <summary>
        /// GIF 文件数据是否解析成功
        /// </summary>
        private readonly bool isSucceed;
        /// <summary>
        /// GIF 文件解析数据
        /// </summary>
        /// <param name="fileData">GIF 文件内容数据</param>
        private unsafe FileData(byte[] fileData)
        {
#if NetStandard21
            GlobalColors = EmptyArray<LockBitmapColor>.Array;
#endif
            fixed (byte* dataFixed = fileData)
            {
                if ((*(int*)dataFixed & 0xffffff) == ('G' | ('I' << 8) | ('F' << 16)))
                {
                    Width = *(short*)(dataFixed + 6);
                    Height = *(short*)(dataFixed + 8);
                    byte globalFlag = *(dataFixed + 10);
                    BackgroundColorIndex = *(dataFixed + 11);
                    PixelAspectRadio = *(dataFixed + 12);
                    ColorResoluTion = (byte)(((globalFlag >> 4) & 7) + 1);
                    SortFlag = (byte)(globalFlag & 8);
                    byte* data = dataFixed + 6 + 7;
                    if ((globalFlag & 0x80) != 0)
                    {
                        int colorCount = 1 << ((globalFlag & 7) + 1);
                        if (fileData.Length < 14 + (colorCount << 1) + colorCount) return;
                        data = Decoder.FillColor(GlobalColors = AutoCSer.Common.GetUninitializedArray<LockBitmapColor>(colorCount), data);
                    }
                    Decoder decoder = new Decoder(fileData, dataFixed, data);
                    while (!decoder.IsEnd)
                    {
                        var block = decoder.Next();
                        if (block != null) blocks.Add(block);
                        if (decoder.IsError) return;
                    }
                    isSucceed = true;
                }
            }
        }

        /// <summary>
        /// 创建 GIF 文件解析数据
        /// </summary>
        /// <param name="fileData">GIF 文件内容数据</param>
        /// <returns>GIF 文件解析数据，失败返回 null</returns>
#if NetStandard21
        public static FileData? Create(byte[] fileData)
#else
        public static FileData Create(byte[] fileData)
#endif
        {
            if (fileData != null && fileData.Length > 3 + 3 + 7 + 1)
            {
                FileData data = new FileData(fileData);
                if (data.isSucceed) return data;
            }
            return null;
        }
        /// <summary>
        /// 创建 GIF 文件解析数据
        /// </summary>
        /// <param name="filename">GIF 文件名</param>
        /// <returns>GIF 文件解析数据，失败返回 null</returns>
#if NetStandard21
        public static async Task<FileData?> Create(string filename)
#else
        public static async Task<FileData> Create(string filename)
#endif
        {
            if (await AutoCSer.Common.FileExists(filename))
            {
                return Create(await AutoCSer.Common.ReadFileAllBytes(filename));
            }
            return null;
        }
    }
}
