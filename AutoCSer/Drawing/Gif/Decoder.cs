using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF 文件数据解码器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct Decoder
    {
        /// <summary>
        /// GIF 文件数据内容
        /// </summary>
        internal readonly byte[] FileData;
        /// <summary>
        /// GIF 文件数据内容起始位置
        /// </summary>
        private readonly byte* fileStart;
        /// <summary>
        /// GIF文件数据结束位置
        /// </summary>
        internal readonly byte* End;
        /// <summary>
        /// 当前解析数据位置
        /// </summary>
        internal byte* Data;
        /// <summary>
        /// 当前解析数据位置
        /// </summary>
        internal int CurrentIndex { get { return (int)(Data - fileStart); } }
        /// <summary>
        /// 是否文件结束
        /// </summary>
        internal bool IsEnd
        {
            get { return *Data == 0x3b; }
        }
        /// <summary>
        /// 是否解析错误
        /// </summary>
        internal bool IsError;
        /// <summary>
        /// GIF 文件数据解码器
        /// </summary>
        /// <param name="fileData">GIF 文件数据内容</param>
        /// <param name="fileStart">GIF 文件数据内容起始位置</param>
        /// <param name="data">当前解析数据位置</param>
        internal Decoder(byte[] fileData, byte* fileStart, byte* data)
        {
            this.FileData = fileData;
            this.fileStart = fileStart;
            Data = data;
            End = fileStart + fileData.Length - 1;
            IsError = false;
        }
        /// <summary>
        /// 解析下一个数据块
        /// </summary>
        /// <returns></returns>
        internal DataBlock Next()
        {
            if (*Data == 0x2c) return new Image(ref this);
            if (*Data == 0x21)
            {
                if (*++Data == 1) return new PlainText(ref this);
                switch (*Data & 3)
                {
                    case 0xf9 & 3: return new GraphicControl(ref this);
                    case 0xfe & 3: return ++Data < End ? new Comment(ref this) : null;
                    case 0xff & 3: return new Application(ref this);
                }
            }
            return null;
        }
        /// <summary>
        /// 填充数据块
        /// </summary>
        /// <param name="blockData"></param>
        internal void GetBlocks(ref SubArray<byte> blockData)
        {
            byte* dataStart = Data;
            for (byte count = *Data; count != 0; count = *Data)
            {
                Data += count;
                if (++Data >= End)
                {
                    IsError = true;
                    return;
                }
            }
            blockData.Set(FileData, (int)(dataStart - dataStart), (int)(Data - dataStart));
            ++Data;
        }
        /// <summary>
        /// 填充数据块集合
        /// </summary>
        /// <param name="datas">填充数据块集合</param>
        internal void GetBlockList(ref LeftArray<SubArray<byte>> datas)
        {
            int startIndex = (int)(Data - fileStart);
            for (byte count = *Data; count != 0; count = *Data)
            {
                Data += count;
                if (++Data >= End)
                {
                    IsError = true;
                    return;
                }
                datas.PrepLength(1);
                datas.Array[datas.Length++].Set(FileData, ++startIndex, count);
                startIndex += count;
            }
            ++Data;
        }

        /// <summary>
        /// 颜色列表数据填充
        /// </summary>
        /// <param name="colors">颜色列表数组</param>
        /// <param name="data">颜色列表数据</param>
        /// <returns>数据结束位置</returns>
        internal static byte* FillColor(LockBitmapColor[] colors, byte* data)
        {
            fixed (LockBitmapColor* globalColorsFixed = colors)
            {
                for (LockBitmapColor* currentColor = globalColorsFixed, endColor = currentColor + colors.Length; currentColor != endColor; data += 3) *currentColor++ = LockBitmap.GetColor(data);
            }
            return data;
        }
        /// <summary>
        /// 合并数据块集合
        /// </summary>
        /// <param name="datas">数据块集合</param>
        /// <returns>合并后的数据块</returns>
        internal static byte[] BlocksToByte(ref LeftArray<SubArray<byte>> datas)
        {
            if (datas.Length != 0)
            {
                int length = 0, count = datas.Length;
                SubArray<byte>[] array = datas.Array;
                for (int index = 0; index != count; ++index) length += array[index].Length;
                byte[] data = AutoCSer.Common.Config.GetArray(length);
                fixed (byte* dataFixed = data)
                {
                    byte* write = dataFixed;
                    for (int index = 0; index != count; ++index)
                    {
                        SubArray<byte> copyData = array[index];
                        if (copyData.Length != 0)
                        {
                            AutoCSer.Common.Config.CopyTo(copyData.Array, copyData.Start, write, copyData.Length);
                            write += copyData.Length;
                        }
                    }
                }
                return data;
            }
            return null;
        }
    }
}
