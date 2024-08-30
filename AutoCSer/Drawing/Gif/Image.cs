using AutoCSer.Memory;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图像数据块
    /// </summary>
    public sealed class Image : DataBlock
    {
        /// <summary>
        /// LZW 压缩解码字符串缓冲区
        /// </summary>
        private static readonly ByteArrayPool stringBufferPool = ByteArrayPool.GetPool(BufferSizeBitsEnum.Kilobyte32);//4097 * 8

        /// <summary>
        /// 数据块类型
        /// </summary>
        public override DataTypeEnum Type { get { return DataTypeEnum.Image; } }
        /// <summary>
        /// X方向偏移量
        /// </summary>
        public readonly short LeftOffset;
        /// <summary>
        /// Y方向偏移量
        /// </summary>
        public readonly short TopOffset;
        /// <summary>
        /// 图象宽度
        /// </summary>
        public readonly short Width;
        /// <summary>
        /// 图象高度
        /// </summary>
        public readonly short Height;
        /// <summary>
        /// 图象数据是否连续方式排列，否则使用顺序排列
        /// </summary>
        public readonly byte InterlaceFlag;
        /// <summary>
        /// 颜色列表是否分类排列
        /// </summary>
        public readonly byte SortFlag;
        /// <summary>
        /// LZW 编码初始码表大小的位数
        /// </summary>
        public readonly byte LzwSize;
        /// <summary>
        /// 颜色列表
        /// </summary>
        public readonly LockBitmapColor[] Colors;
        /// <summary>
        /// 压缩数据集合
        /// </summary>
        private LeftArray<SubArray<byte>> lzwDatas;
        /// <summary>
        /// 压缩数据集合
        /// </summary>
        public LeftArray<SubArray<byte>> LzwDatas { get { return lzwDatas; } }
        /// <summary>
        /// 图像数据块
        /// </summary>
        /// <param name="decoder"></param>
        unsafe internal Image(ref Decoder decoder)
        {
            byte* data = decoder.Data;
            long length = decoder.End - data - 12;
            if (length > 0)
            {
                LeftOffset = *(short*)(data + 1);
                TopOffset = *(short*)(data + 3);
                Width = *(short*)(data + 5);
                Height = *(short*)(data + 7);
                byte localFlag = *(data + 9);
                InterlaceFlag = (byte)(localFlag & 0x40);
                SortFlag = (byte)(localFlag & 0x20);
                data += 10;
                if ((localFlag & 0x80) != 0)
                {
                    Colors = AutoCSer.Common.Config.GetArray<LockBitmapColor>(1 << ((localFlag & 7) + 1));
                    int colorCount = Colors.Length;
                    if ((length -= (colorCount << 1) + colorCount) <= 0)
                    {
                        decoder.IsError = true;
                        return;
                    }
                    data = Decoder.FillColor(Colors, data);
                }
                LzwSize = *data++;
                lzwDatas = new LeftArray<SubArray<byte>>(0);
                decoder.Data = data;
                decoder.GetBlockList(ref lzwDatas);
                return;
            }
            decoder.IsError = true;
        }
        /// <summary>
        /// 创建位图
        /// </summary>
        /// <param name="globalColors">全局颜色列表</param>
        /// <returns>位图,失败返回null</returns>
        public unsafe Bitmap CreateBitmap(LockBitmapColor[] globalColors)
        {
            if (Width != 0 && Height != 0 && LzwSize != 0 && LzwSize <= 8)
            {
                Bitmap bitmap = null;
                int colorSize = Width * Height;
                AutoCSer.Memory.UnmanagedPoolPointer colorIndexs = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(Math.Max(colorSize, UnmanagedPool.LzwEncodeTableBuffer.Size));
                try
                {
                    int length = lzwDecode(Decoder.BlocksToByte(ref lzwDatas), colorIndexs.Pointer.Byte, LzwSize);
                    if (length == colorSize)
                    {
                        bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                        byte* bitmapFixed = (byte*)bitmapData.Scan0;
                        int bitMapSpace = bitmapData.Stride - (Width << 1) - Width;
                        if (globalColors == null) globalColors = Colors;
                        if (globalColors != null)
                        {
                            fixed (LockBitmapColor* colorFixed = globalColors)
                            {
                                FillBitmap fillBitmap = new FillBitmap(Width, colorIndexs.Pointer.Byte, colorFixed);
                                if (InterlaceFlag == 0) fillBitmap.FillColor(Height, bitmapFixed, bitMapSpace);
                                else
                                {
                                    int bitmapStride = bitMapSpace + (bitmapData.Stride << 3) - bitmapData.Stride;
                                    fillBitmap.FillColor((Height + 7) >> 3, bitmapFixed, bitmapStride);
                                    fillBitmap.FillColor((Height + 3) >> 3, bitmapFixed + (bitmapData.Stride << 2), bitmapStride);
                                    fillBitmap.FillColor((Height + 1) >> 2, bitmapFixed + (bitmapData.Stride << 1), bitmapStride -= bitmapData.Stride << 2);
                                    fillBitmap.FillColor(Height >> 1, bitmapFixed + bitmapData.Stride, bitmapStride - (bitmapData.Stride << 1));
                                }
                            }
                        }
                        else
                        {
                            FillBitmap fillBitmap = new FillBitmap(Width, colorIndexs.Pointer.Byte);
                            if (InterlaceFlag == 0) fillBitmap.FillIndex(Height, bitmapFixed, bitMapSpace);
                            else
                            {
                                int bitmapStride = bitMapSpace + (bitmapData.Stride << 3) - bitmapData.Stride;
                                fillBitmap.FillIndex((Height + 7) >> 3, bitmapFixed, bitmapStride);
                                fillBitmap.FillIndex((Height + 3) >> 3, bitmapFixed + (bitmapData.Stride << 2), bitmapStride);
                                fillBitmap.FillIndex((Height + 1) >> 2, bitmapFixed + (bitmapData.Stride << 1), bitmapStride -= bitmapData.Stride << 2);
                                fillBitmap.FillIndex(Height >> 1, bitmapFixed + bitmapData.Stride, bitmapStride - (bitmapData.Stride << 1));
                            }
                        }
                        bitmap.UnlockBits(bitmapData);

                        Bitmap returnBitmap = bitmap;
                        bitmap = null;
                        return returnBitmap;
                    }
                }
                catch (Exception error)
                {
                    bitmap?.Dispose();
                    AutoCSer.LogHelper.ExceptionIgnoreException(error, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally { colorIndexs.PushOnly(); }
            }
            return null;
        }
        /// <summary>
        /// LZW 压缩解码
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <param name="output">输出数据缓冲</param>
        /// <param name="size">编码长度</param>
        /// <returns>解码数据长度,失败返回-1</returns>
        private unsafe static int lzwDecode(byte[] input, byte* output, byte size)
        {
            int tableSize = (int)size + 1;
            short clearIndex = (short)(1 << size), nextIndex = clearIndex;
            ByteArrayBuffer stringBuffer = default(ByteArrayBuffer);
            stringBufferPool.Get(ref stringBuffer);
            try
            {
                fixed (byte* inputFixed = input, stringFixed = stringBuffer.GetFixedBuffer())
                {
                    byte* nextStrings = null, stringStart = stringFixed + stringBuffer.StartIndex;
                    byte* currentInput = inputFixed, inputEnd = inputFixed + input.Length;
                    byte* currentOutput = output, outputEnd = output + UnmanagedPool.LzwEncodeTableBuffer.Size;
                    int valueBits = 0, inputSize = 0, inputOffset = (int)inputEnd & (sizeof(ulong) - 1), startSize = tableSize;
                    ulong inputValue = 0, inputMark = ushort.MaxValue, startMark = ((ulong)1UL << startSize) - 1;
                    short endIndex = (short)(clearIndex + 1), prefixIndex, currentIndex = 0;
                    if (inputOffset == 0)
                    {
                        inputEnd -= sizeof(ulong);
                        inputOffset = sizeof(ulong);
                    }
                    else inputEnd -= inputOffset;
                    if (size == 1) ++startSize;
                    while (currentIndex != endIndex)
                    {
                        if (valueBits >= startSize)
                        {
                            prefixIndex = (short)(inputValue & startMark);
                            valueBits -= startSize;
                            inputValue >>= startSize;
                        }
                        else
                        {
                            if (currentInput > inputEnd) return -1;
                            ulong nextValue = *(ulong*)currentInput;
                            prefixIndex = (short)((inputValue | (nextValue << valueBits)) & startMark);
                            inputValue = nextValue >> -(valueBits -= startSize);
                            valueBits += sizeof(ulong) << 3;
                            if (currentInput == inputEnd && (valueBits -= (sizeof(ulong) - inputOffset) << 3) < 0) return -1;
                            currentInput += sizeof(ulong);
                        }
                        if (prefixIndex == clearIndex) continue;
                        if (prefixIndex == endIndex) break;
                        if (currentOutput == outputEnd) return -1;

                        AutoCSer.Memory.Common.Clear((ulong*)stringStart, 4097);
                        inputSize = startSize;
                        inputMark = startMark;
                        nextIndex = (short)(endIndex + 1);
                        *(short*)(nextStrings = stringStart + (nextIndex << 3)) = prefixIndex;
                        *(short*)(nextStrings + 2) = prefixIndex;
                        *(int*)(nextStrings + 4) = 2;
                        *currentOutput++ = (byte)prefixIndex;
                        do
                        {
                            if (valueBits >= inputSize)
                            {
                                currentIndex = (short)(inputValue & inputMark);
                                valueBits -= inputSize;
                                inputValue >>= inputSize;
                            }
                            else
                            {
                                if (currentInput > inputEnd) return -1;
                                ulong nextValue = *(ulong*)currentInput;
                                currentIndex = (short)((inputValue | (nextValue << valueBits)) & inputMark);
                                inputValue = nextValue >> -(valueBits -= inputSize);
                                valueBits += sizeof(ulong) << 3;
                                if (currentInput == inputEnd && (valueBits -= (sizeof(ulong) - inputOffset) << 3) < 0) return -1;
                                currentInput += sizeof(ulong);
                            }
                            *(short*)(nextStrings += 8) = currentIndex;
                            if (currentIndex < clearIndex)
                            {
                                if (currentOutput == outputEnd) return -1;
                                *(short*)(nextStrings + 2) = currentIndex;
                                *(int*)(nextStrings + 4) = 2;
                                *currentOutput++ = (byte)currentIndex;
                            }
                            else if (currentIndex > endIndex)
                            {
                                byte* currentString = stringStart + (currentIndex << 3);
                                int outputCount = *(int*)(currentString + 4);
                                if (outputCount == 0) return -1;
                                *(short*)(nextStrings + 2) = *(short*)(currentString + 2);
                                *(int*)(nextStrings + 4) = outputCount + 1;
                                if ((currentOutput += outputCount) > outputEnd) return -1;
                                do
                                {
                                    *--currentOutput = *(currentString + 2 + 8);
                                    prefixIndex = *(short*)currentString;
                                    if (prefixIndex < clearIndex) break;
                                    currentString = stringStart + (prefixIndex << 3);
                                }
                                while (true);
                                *--currentOutput = (byte)prefixIndex;
                                currentOutput += outputCount;
                            }
                            else break;
                            prefixIndex = currentIndex;
                            if (nextIndex++ == (short)inputMark)
                            {
                                if (inputSize == 12) return -1;
                                inputMark <<= 1;
                                ++inputSize;
                                ++inputMark;
                            }
                        }
                        while (true);
                    }
                    return (int)(currentOutput - output);
                }
            }
            finally { stringBuffer.Free(); }
        }
    }
}
