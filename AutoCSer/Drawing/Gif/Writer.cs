﻿ using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF 文件数据写入器
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
    public class Writer : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// GIF文件标识与版本信息
        /// </summary>
        private const ulong fileVersion = 'G' + ('I' << 8) + ('F' << 16) + ('8' << 24) + ((ulong)'9' << 32) + ((ulong)'a' << 40);

        /// <summary>
        /// 输出数据流
        /// </summary>
        private readonly Stream stream;
        /// <summary>
        /// 文件缓冲区
        /// </summary>
        private readonly byte[] fileBuffer;
        /// <summary>
        /// 当前图像色彩缓存
        /// </summary>
        private readonly LockBitmapColor[] colors;
        /// <summary>
        /// 当前图像色彩数量缓存
        /// </summary>
        private readonly int[] colorCounts;
        /// <summary>
        /// 当前图像色彩数量
        /// </summary>
        private readonly ReusableHashCodeKeyDictionary<int> colorIndexs;
        /// <summary>
        /// 全局颜色数量
        /// </summary>
        private readonly int globalColorCount;
        /// <summary>
        /// 素数宽度
        /// </summary>
        public readonly short Width;
        /// <summary>
        /// 素数高度
        /// </summary>
        public readonly short Height;
        /// <summary>
        /// 是否自动释放输出数据流
        /// </summary>
        private readonly bool isLeaveDisposeStream;
        /// <summary>
        /// 当前文件缓存位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        protected int isDisposed;
        /// <summary>
        /// GIF文件写入器
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">素数宽度</param>
        /// <param name="height">素数高度</param>
        /// <param name="globalColors">全局颜色列表</param>
        /// <param name="backgroundColorIndex">背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义</param>
        /// <param name="isLeaveDisposeStream">是否自动释放输出数据流</param>
#if NetStandard21
        public unsafe Writer(Stream stream, short width, short height, LockBitmapColor[]? globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false)
#else
        public unsafe Writer(Stream stream, short width, short height, LockBitmapColor[] globalColors = null, byte backgroundColorIndex = 0, bool isLeaveDisposeStream = false)
#endif
        {
            if (stream == null) throw new ArgumentNullException();
            if (width <= 0) throw new IndexOutOfRangeException("width[" + width.toString() + "] <= 0");
            if (height <= 0) throw new IndexOutOfRangeException("height[" + height.toString() + "] <= 0");
            this.stream = stream;
            this.isLeaveDisposeStream = isLeaveDisposeStream;
            Width = width;
            Height = height;
            globalColorCount = globalColors != null ? globalColors.Length : 0;
            int pixel = 0;
            if (globalColorCount != 0)
            {
                if (globalColorCount < 256)
                {
                    pixel = ((uint)globalColorCount).bits() - 1;
                    if (globalColorCount != (1 << pixel)) ++pixel;
                }
                else
                {
                    globalColorCount = 256;
                    pixel = 7;
                }
                pixel |= 0x80;
            }
            fileBuffer = AutoCSer.Common.GetUninitializedArray<byte>(UnmanagedPool.DefaultSize + (256 * 3) + 8);
            fixed (byte* bufferFixed = fileBuffer)
            {
                *(ulong*)bufferFixed = fileVersion | ((ulong)width << 48);
                *(uint*)(bufferFixed + 8) = (uint)(int)height | (globalColorCount == 0 ? 0 : ((uint)pixel << 16)) | (7 << (16 + 4))
                    | (backgroundColorIndex >= globalColorCount ? 0 : ((uint)backgroundColorIndex << 24));
                bufferIndex = 13;
                if (globalColorCount != 0)
                {
                    byte* currentBuffer = bufferFixed + 13;
                    fixed (LockBitmapColor* colorFixed = globalColors)
                    {
                        for (LockBitmapColor* currentColor = colorFixed, colorEnd = colorFixed + globalColorCount; currentColor != colorEnd; ++currentColor) currentBuffer = write(currentBuffer, *currentColor);
                    }
                    bufferIndex += 3 << (pixel ^ 0x80);
                }
            }
            colors = AutoCSer.Common.GetUninitializedArray<LockBitmapColor>((int)Width * Height);
            colorCounts = AutoCSer.Common.GetUninitializedArray<int>(colors.Length);
            colorIndexs = new ReusableHashCodeKeyDictionary<int>();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        protected virtual void dispose()
        {
            fileBuffer[bufferIndex++] = 0x3b;
            if (isLeaveDisposeStream)
            {
                using (stream) stream.Write(fileBuffer, 0, bufferIndex);
            }
            else stream.Write(fileBuffer, 0, bufferIndex);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public async ValueTask DisposeAsync()
#else
        public async Task DisposeAsync()
#endif
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) await disposeAsync();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
        protected virtual async Task disposeAsync()
        {
            fileBuffer[bufferIndex++] = 0x3b;
            if (isLeaveDisposeStream)
            {
#if NetStandard21
                await using (stream) await stream.WriteAsync(fileBuffer, 0, bufferIndex);
#else
                using (stream) await stream.WriteAsync(fileBuffer, 0, bufferIndex);
#endif
            }
            else await stream.WriteAsync(fileBuffer, 0, bufferIndex);
        }
        /// <summary>
        /// 检测文件缓存
        /// </summary>
        /// <param name="bufferFixed">文件缓存起始位置</param>
        private unsafe void checkBuffer(byte* bufferFixed)
        {
            int count = bufferIndex - UnmanagedPool.DefaultSize;
            if (count >= 0)
            {
                bool isWrite = false;
                try
                {
                    stream.Write(fileBuffer, 0, UnmanagedPool.DefaultSize);
                    AutoCSer.Common.CopyTo(bufferFixed + UnmanagedPool.DefaultSize, bufferFixed, bufferIndex = count);
                    isWrite = true;
                }
                finally
                {
                    if (!isWrite && Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0 && isLeaveDisposeStream) stream.Dispose();
                }
            }
        }
        /// <summary>
        /// 检测文件缓存
        /// </summary>
        /// <param name="bufferFixed">文件缓存起始位置</param>
        /// <param name="length">新增长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe void checkBuffer(byte* bufferFixed, int length)
        {
            bufferIndex += length;
            checkBuffer(bufferFixed);
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="leftOffset">X方向偏移量</param>
        /// <param name="topOffset">Y方向偏移量</param>
        /// <param name="width">图象宽度</param>
        /// <param name="height">图象高度</param>
        /// <param name="bitmapLeftOffset">位图剪切X方向偏移量</param>
        /// <param name="bitmapTopOffset">位图剪切Y方向偏移量</param>
        /// <param name="isInterlace">图象数据是否连续方式排列，否则使用顺序排列</param>
        /// <param name="maxPixel">最大色彩深度</param>
        /// <returns>图片是否添加成功</returns>
        public unsafe bool AddImage(Bitmap bitmap, int bitmapLeftOffset = 0, int bitmapTopOffset = 0, int leftOffset = 0, int topOffset = 0, int width = 0, int height = 0, bool isInterlace = false, byte maxPixel = 8)
        {
            if (bitmap != null && isDisposed == 0)
            {
                if (width == 0) width = Width;
                if (height == 0) height = Height;
                if (leftOffset < 0)
                {
                    bitmapLeftOffset -= leftOffset;
                    width += leftOffset;
                    leftOffset = 0;
                }
                if (topOffset < 0)
                {
                    bitmapTopOffset -= topOffset;
                    height += topOffset;
                    topOffset = 0;
                }
                if (bitmapLeftOffset < 0)
                {
                    leftOffset -= bitmapLeftOffset;
                    width += bitmapLeftOffset;
                    bitmapLeftOffset = 0;
                }
                if (bitmapTopOffset < 0)
                {
                    topOffset -= bitmapTopOffset;
                    height += bitmapTopOffset;
                    bitmapTopOffset = 0;
                }
                int minWidth = bitmap.Width - bitmapLeftOffset, minHeight = bitmap.Height - bitmapTopOffset;
                if (minWidth < width) width = minWidth;
                if (minHeight < height) height = minHeight;
                if ((minWidth = width - leftOffset) < width) width = minWidth;
                if ((minHeight = height - topOffset) < height) height = minHeight;
                if (width > 0 && height > 0)
                {
                    if ((byte)(maxPixel - 2) >= 8) maxPixel = 8;
                    BitmapData bitmapData;
                    try
                    {
                        bitmapData = bitmap.LockBits(new Rectangle(bitmapLeftOffset, bitmapTopOffset, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                        return false;
                    }
                    try
                    {
                        addImage(bitmapData, 0, 0, leftOffset, topOffset, width, height, isInterlace, maxPixel);
                    }
                    finally { bitmap.UnlockBits(bitmapData); }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="leftOffset">X方向偏移量</param>
        /// <param name="topOffset">Y方向偏移量</param>
        /// <param name="width">图象宽度</param>
        /// <param name="height">图象高度</param>
        /// <param name="bitmapLeftOffset">位图剪切X方向偏移量</param>
        /// <param name="bitmapTopOffset">位图剪切Y方向偏移量</param>
        /// <param name="isInterlace">图象数据是否连续方式排列，否则使用顺序排列</param>
        /// <param name="maxPixel">最大色彩深度</param>
        internal unsafe void addImage(BitmapData bitmap, int bitmapLeftOffset, int bitmapTopOffset, int leftOffset, int topOffset, int width, int height, bool isInterlace, byte maxPixel)
        {
            fixed (LockBitmapColor* colorFixed = colors)
            fixed (int* colorCountFixed = colorCounts)
            {
                byte* bitmapFixed = (byte*)bitmap.Scan0, currentBitmap = bitmapFixed + bitmap.Stride * (bitmapTopOffset - 1) + (bitmapLeftOffset + width) * 3, endColor = bitmapFixed + (bitmap.Height * bitmap.Stride - 3);
                LockBitmapColor* currentColor = colorFixed;
                int bitMapSpace = bitmap.Stride - (width << 1) - width;
                colorIndexs.ClearCount();
                for (int colorIndex, row = height; row != 0; --row)
                {
                    currentBitmap += bitMapSpace;
                    for (int col = width; col != 0; --col)
                    {
                        LockBitmapColor color = LockBitmap.GetColor(currentBitmap, endColor);
                        currentBitmap += 3;
                        if (colorIndexs.TryGetValue(color.Value, out colorIndex)) ++colorCountFixed[colorIndex];
                        else
                        {
                            colorIndexs.Set(color.Value, colorIndex = (int)(currentColor - colorFixed));
                            colorCountFixed[colorIndex] = 1;
                        }
                        *currentColor++ = color;
                    }
                }
                int pixel = ((uint)colorIndexs.Count).bits() - 1;
                if ((1 << pixel) != colorIndexs.Count) ++pixel;
                if (pixel > maxPixel) pixel = maxPixel;
                else if (pixel < 2) pixel = 2;
                int maxColorCount = 1 << pixel;
                fixed (byte* bufferFixed = fileBuffer)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *currentBuffer = 0x2c;
                    *(short*)(currentBuffer + 1) = (short)leftOffset;
                    *(short*)(currentBuffer + 3) = (short)topOffset;
                    *(short*)(currentBuffer + 5) = (short)width;
                    *(short*)(currentBuffer + 7) = (short)height;
                    *(currentBuffer + 9) = (byte)(0x80 + (isInterlace ? 0x40 : 0) + (pixel - 1));
                    checkBuffer(bufferFixed, 10);
                }
                if (colorIndexs.Count <= maxColorCount)
                {
                    fixed (byte* bufferFixed = fileBuffer)
                    {
                        int* currentColorCount = colorCountFixed;
                        foreach (uint colorKey in colorIndexs.Keys) *currentColorCount++ = (int)colorKey;
                        LockBitmapColor color = new LockBitmapColor();
                        int currentColorIndex = 0;
                        byte* currentBuffer = bufferFixed + bufferIndex;
                        while (currentColorCount != colorCountFixed)
                        {
                            color.Value = *--currentColorCount;
                            currentBuffer = write(currentBuffer, color);
                            colorIndexs.Set(color.Value, currentColorIndex++);
                        }
                        *(bufferFixed + bufferIndex + (maxColorCount << 1) + maxColorCount) = (byte)pixel;
                        checkBuffer(bufferFixed, (maxColorCount << 1) + maxColorCount + 1);
                    }
                }
                else
                {
                    int indexCount = colorIndexs.Count;
                    UnmanagedPoolPointer sizeBuffer = UnmanagedPool.GetPoolPointer(indexCount * (sizeof(IntSortIndex) + sizeof(int)));
                    try
                    {
                        int* buffer = sizeBuffer.Pointer.Int;
                        IntSortIndex* indexFixed = (IntSortIndex*)(buffer + indexCount), currentSortIndex = indexFixed;
                        foreach (KeyValue<uint, int> colorIndex in colorIndexs.KeyValues)
                        {
                            int color0 = (int)colorIndex.Key;
                            int color3 = ((color0 >> 3) & 0x111111) * 0x1020400;
                            int color2 = ((color0 >> 2) & 0x111111) * 0x1020400;
                            int color1 = ((color0 >> 1) & 0x111111) * 0x1020400;
                            color0 = (color0 & 0x111111) * 0x1020400;
                            (*currentSortIndex++).Set((color3 & 0x70000000) | ((color2 >> 4) & 0x7000000)
                                | ((color1 >> 8) & 0x700000) | ((color0 >> 12) & 0x70000) | ((color3 >> 12) & 0x7000)
                                | ((color2 >> 16) & 0x700) | ((color1 >> 20) & 0x70) | ((color0 >> 24) & 7), colorIndex.Value);
                        }
                        IntSortIndex.Sort(indexFixed, indexFixed + indexCount - 1);
                        int* currentSortArray;
                        if (maxColorCount != 2)
                        {
                            currentSortArray = buffer;
                            for (int currentColorCode, lastColorCode = (*--currentSortIndex).Value; currentSortIndex != indexFixed; lastColorCode = currentColorCode)
                            {
                                currentColorCode = (*--currentSortIndex).Value;
                                *currentSortArray++ = lastColorCode - currentColorCode;
                            }
                            currentSortArray = buffer + (maxColorCount >> 1) - 2;
                            new IntQuickRangeSortDesc(currentSortArray, currentSortArray).Sort(buffer, buffer + indexCount - 2);
                            int minColorDifference = *currentSortArray, minColorDifferenceCount = 1;
                            while (currentSortArray != buffer)
                            {
                                if (*--currentSortArray == minColorDifference) ++minColorDifferenceCount;
                            }
                            currentSortIndex = indexFixed + indexCount;
                            int maxCountIndex = (*--currentSortIndex).Index, maxCount = *(colorCountFixed + maxCountIndex);
                            for (int currentColorCode, lastColorCode = (*currentSortIndex).Value; currentSortIndex != indexFixed; lastColorCode = currentColorCode)
                            {
                                currentColorCode = (*--currentSortIndex).Value;
                                int colorDifference = lastColorCode - currentColorCode;
                                if (colorDifference >= minColorDifference)
                                {
                                    if (colorDifference == minColorDifference && --minColorDifferenceCount == 0) ++minColorDifference;
                                    *(colorCountFixed + maxCountIndex) = int.MaxValue;
                                    maxCount = *(colorCountFixed + (maxCountIndex = (*currentSortIndex).Index));
                                }
                                else
                                {
                                    int countIndex = (*currentSortIndex).Index, count = *(colorCountFixed + countIndex);
                                    if (count > maxCount)
                                    {
                                        maxCountIndex = countIndex;
                                        maxCount = count;
                                    }
                                }
                            }
                            *(colorCountFixed + maxCountIndex) = int.MaxValue;
                        }
                        for (currentSortArray = buffer + indexCount, currentSortIndex = indexFixed; currentSortArray != buffer; *(--currentSortArray) = *(colorCountFixed + (*currentSortIndex++).Index)) ;
                        currentSortArray = buffer + maxColorCount - 1;
                        new IntQuickRangeSortDesc(currentSortArray, currentSortArray).Sort(buffer, buffer + indexCount - 1);
                        int minColorCount = *currentSortArray, minColorCounts = 1;
                        while (currentSortArray != buffer)
                        {
                            if (*--currentSortArray == minColorCount) ++minColorCounts;
                        }
                        fixed (byte* fileBufferFixed = fileBuffer)
                        {
                            byte* currentBuffer = fileBufferFixed + bufferIndex;
                            IntSortIndex* lastSortIndex = indexFixed, endSortIndex = indexFixed + indexCount;
                            while (*(colorCountFixed + (*lastSortIndex).Index) < minColorCount) colorIndexs.Set(*(int*)(colorFixed + (*lastSortIndex++).Index), 0);
                            if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                            LockBitmapColor outputColor = *(colorFixed + (*lastSortIndex).Index);
                            currentBuffer = write(currentBuffer, outputColor);
                            colorIndexs.Set(outputColor.Value, 0);
                            for (--maxColorCount; *(colorCountFixed + (*--endSortIndex).Index) < minColorCount; colorIndexs.Set(*(int*)(colorFixed + (*endSortIndex).Index), maxColorCount)) ;
                            if (*(colorCountFixed + (*endSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                            colorIndexs.Set(*(int*)(colorFixed + (*endSortIndex).Index), maxColorCount++);
                            int currentColorIndex = 0;
                            for (int* lastColorCount = colorCountFixed + (*endSortIndex).Index; lastSortIndex != endSortIndex;)
                            {
                                for (*lastColorCount = 0; *(colorCountFixed + (*++lastSortIndex).Index) >= minColorCount; colorIndexs.Set(outputColor.Value, ++currentColorIndex))
                                {
                                    if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                                    outputColor = *(colorFixed + (*lastSortIndex).Index);
                                    currentBuffer = write(currentBuffer, outputColor);
                                }
                                if (lastSortIndex == endSortIndex) break;
                                *lastColorCount = int.MaxValue;
                                IntSortIndex* nextSortIndex = lastSortIndex;
                                while (*(colorCountFixed + (*++nextSortIndex).Index) < minColorCount) ;
                                for (int lastColorCode = (*(lastSortIndex - 1)).Value, nextColorCode = (*nextSortIndex).Value; lastSortIndex != nextSortIndex; ++lastSortIndex)
                                {
                                    colorIndexs.Set(*(int*)(colorFixed + (*lastSortIndex).Index), (*lastSortIndex).Value - lastColorCode <= nextColorCode - (*lastSortIndex).Value ? currentColorIndex : (currentColorIndex + 1));
                                }
                                if (lastSortIndex != endSortIndex)
                                {
                                    if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                                    outputColor = *(colorFixed + (*lastSortIndex).Index);
                                    currentBuffer = write(currentBuffer, outputColor);
                                    colorIndexs.Set(outputColor.Value, ++currentColorIndex);
                                }
                            }
                            outputColor = *(colorFixed + (*lastSortIndex).Index);
                            *write(currentBuffer, outputColor) = (byte)pixel;
                            checkBuffer(fileBufferFixed, (maxColorCount << 1) + maxColorCount + 1);
                        }
                    }
                    finally { sizeBuffer.PushOnly(); }
                }
                byte* colorIndexFixed = (byte*)colorCountFixed;
                if (isInterlace)
                {
                    LockBitmapColor* colorEnd = colorFixed + width * height;
                    int inputSpace = (width << 3) - width;
                    for (LockBitmapColor* inputColor = colorFixed; inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (LockBitmapColor* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[(*inputColor++).Value]) ;
                    }
                    for (LockBitmapColor* inputColor = colorFixed + (width << 2); inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (LockBitmapColor* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[(*inputColor++).Value]) ;
                    }
                    inputSpace -= width << 2;
                    for (LockBitmapColor* inputColor = colorFixed + (width << 1); inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (LockBitmapColor* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[(*inputColor++).Value]) ;
                    }
                    for (LockBitmapColor* inputColor = colorFixed + width; inputColor < colorEnd; inputColor += width)
                    {
                        for (LockBitmapColor* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[(*inputColor++).Value]) ;
                    }
                }
                else
                {
                    for (LockBitmapColor* inputColor = colorFixed, inputEnd = colorFixed + width * height; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[(*inputColor++).Value]) ;
                }
                lzwEncode((byte*)colorCountFixed, colorIndexFixed, pixel);
            }
        }
        /// <summary>
        /// LZW 压缩编码
        /// </summary>
        /// <param name="inputFixed">输入数据</param>
        /// <param name="outputFixed">输出数据缓冲</param>
        /// <param name="size">编码长度</param>
        private unsafe void lzwEncode(byte* inputFixed, byte* outputFixed, int size)
        {
            UnmanagedPoolPointer lzwEncodeTableBuffer = UnmanagedPool.LzwEncodeTableBuffer.GetPoolPointer();
            try
            {
                byte* lzwEncodeTable = lzwEncodeTableBuffer.Pointer.Byte, currentOutput = outputFixed;
                ulong tableClearIndex = (ulong)1 << size, outputValue = tableClearIndex;
                int tableSize = (int)size + 1;
                short clearIndex = (short)tableClearIndex, nextIndex = clearIndex;
                tableClearIndex |= tableClearIndex << 16;
                tableClearIndex |= tableClearIndex << 32;
                AutoCSer.Common.Fill((ulong*)lzwEncodeTable, ((4096 * 2) / sizeof(ulong)) << size, tableClearIndex);
                int outputSize = tableSize;
                if (size == 1) ++outputSize;
                int outputStart = outputSize, nextClearIndex = 1 << outputSize;
                nextIndex += 2;
                short prefixIndex = *inputFixed;
                for (byte* currentInput = inputFixed; ++currentInput != outputFixed;)
                {
                    byte* currentTable = lzwEncodeTable + (prefixIndex << tableSize) + (*currentInput << 1);
                    if (*(short*)currentTable == clearIndex)
                    {
                        outputValue |= (ulong)(uint)(int)prefixIndex << outputStart;
                        if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                        {
                            *(ulong*)currentOutput = outputValue;
                            outputStart -= sizeof(ulong) << 3;
                            currentOutput += sizeof(ulong);
                            outputValue = (uint)(int)prefixIndex >> (outputSize - outputStart);
                        }
                        if (nextIndex == nextClearIndex)
                        {
                            *(short*)currentTable = nextIndex++;
                            ++outputSize;
                            nextClearIndex <<= 1;
                        }
                        else if (nextIndex == 4095)
                        {
                            outputValue |= (ulong)(uint)(int)clearIndex << outputStart;
                            if ((outputStart += 12) >= sizeof(ulong) << 3)
                            {
                                *(ulong*)currentOutput = outputValue;
                                outputStart -= sizeof(ulong) << 3;
                                currentOutput += sizeof(ulong);
                                outputValue = (uint)(int)clearIndex >> (12 - outputStart);
                            }
                            AutoCSer.Common.Fill((ulong*)lzwEncodeTable, ((4096 * 2) / sizeof(ulong)) << size, tableClearIndex);
                            outputSize = tableSize;
                            if (size == 1) ++outputSize;
                            nextClearIndex = 1 << outputSize;
                            nextIndex = clearIndex;
                            nextIndex += 2;
                        }
                        else *(short*)currentTable = nextIndex++;
                        prefixIndex = *currentInput;
                    }
                    else prefixIndex = *(short*)currentTable;
                }
                outputValue |= (ulong)(uint)(int)prefixIndex << outputStart;
                if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                {
                    *(ulong*)currentOutput = outputValue;
                    outputStart -= sizeof(ulong) << 3;
                    currentOutput += sizeof(ulong);
                    outputValue = (uint)(int)prefixIndex >> (outputSize - outputStart);
                }
                outputValue |= (ulong)(uint)(int)++clearIndex << outputStart;
                if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                {
                    *(ulong*)currentOutput = outputValue;
                    outputStart -= sizeof(ulong) << 3;
                    currentOutput += sizeof(ulong);
                    outputValue = (uint)(int)clearIndex >> (outputSize - outputStart);
                }
                if (outputStart != 0)
                {
                    *(ulong*)currentOutput = outputValue;
                    currentOutput += (outputStart + 7) >> 3;
                }
                fixed (byte* bufferFixed = fileBuffer) addBlocks(bufferFixed, outputFixed, currentOutput);
            }
            finally { lzwEncodeTableBuffer.PushOnly(); }
        }
        /// <summary>
        /// 添加数据块
        /// </summary>
        /// <param name="bufferFixed">文件缓存</param>
        /// <param name="outputFixed">输出数据起始位置</param>
        /// <param name="outputEnd">输出数据结束位置</param>
        private unsafe void addBlocks(byte* bufferFixed, byte* outputFixed, byte* outputEnd)
        {
            for (outputEnd -= 255 * 3; outputFixed <= outputEnd; outputFixed += 255 * 3)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = 255;
                AutoCSer.Common.CopyTo(outputFixed, currentBuffer + 1, 255);
                *(currentBuffer + 256) = 255;
                AutoCSer.Common.CopyTo(outputFixed + 255, currentBuffer + 257, 255);
                *(currentBuffer + 512) = 255;
                AutoCSer.Common.CopyTo(outputFixed + 255 * 2, currentBuffer + 513, 255);
                checkBuffer(bufferFixed, 256 * 3);
            }
            for (outputEnd += 255 * 2; outputFixed <= outputEnd; outputFixed += 255)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = 255;
                AutoCSer.Common.CopyTo(outputFixed, currentBuffer + 1, 255);
                bufferIndex += 256;
            }
            int outputLength = (int)(outputEnd + 255 - outputFixed);
            if (outputLength != 0)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = (byte)outputLength;
                AutoCSer.Common.CopyTo(outputFixed, currentBuffer + 1, outputLength);
                bufferIndex += outputLength + 1;
            }
            *(bufferFixed + bufferIndex++) = 0;
            checkBuffer(bufferFixed);
        }
        /// <summary>
        /// 添加数据块
        /// </summary>
        /// <param name="bufferFixed">文件缓存</param>
        /// <param name="text">文本数据</param>
        private unsafe void addBlocks(byte* bufferFixed, string text)
        {
            fixed (char* textFixed = text)
            {
                char* outputFixed = textFixed, outputEnd = outputFixed + text.Length - 255;
                while (outputFixed <= outputEnd)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *currentBuffer = 255;
                    for (char* nextOutput = outputFixed + 255; outputFixed != nextOutput; ++outputFixed)
                    {
                        *++currentBuffer = *(byte*)outputFixed;
                    }
                    checkBuffer(bufferFixed, 256);
                }
                int outputLength = (int)((outputEnd += 255) - outputFixed);
                if (outputLength != 0)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    for (*currentBuffer = (byte)outputLength; outputFixed != outputEnd; ++outputFixed)
                    {
                        *++currentBuffer = *(byte*)outputFixed;
                    }
                    bufferIndex += outputLength + 1;
                }
                *(bufferFixed + bufferIndex++) = 0;
                checkBuffer(bufferFixed);
            }
        }
        /// <summary>
        /// 添加图形控制扩展
        /// </summary>
        /// <param name="delayTime">延迟时间，单位1/100秒</param>
        /// <param name="method">图形处置方法</param>
        /// <param name="isUseInput">用户输入标志，指出是否期待用户有输入之后才继续进行下去，置位表示期待，值否表示不期待。</param>
        /// <returns>图形控制扩展是否添加成功</returns>
        public unsafe bool AddGraphicControl(short delayTime, GraphicControlMethodTypeEnum method = GraphicControlMethodTypeEnum.None, bool isUseInput = false)
        {
            if (isDisposed == 0)
            {
                if (delayTime <= 0) delayTime = 1;
                fixed (byte* bufferFixed = fileBuffer)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *(int*)currentBuffer = 0x4f921 | ((int)method << 26) | (isUseInput ? (0x2000000) : 0);
                    *(int*)(currentBuffer + 4) = delayTime <= 0 ? 1 : (int)delayTime;
                    checkBuffer(bufferFixed, 8);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加图形文本扩展
        /// </summary>
        /// <param name="text">文本数据</param>
        /// <param name="left">文本框离逻辑屏幕的左边界距离</param>
        /// <param name="top">文本框离逻辑屏幕的上边界距离</param>
        /// <param name="width">文本框像素宽度</param>
        /// <param name="height">文本框像素高度</param>
        /// <param name="colorIndex">前景色在全局颜色列表中的索引</param>
        /// <param name="blackgroundColorIndex">背景色在全局颜色列表中的索引</param>
        /// <param name="characterWidth">字符宽度</param>
        /// <param name="characterHeight">字符高度</param>
        /// <returns>图形文本扩展是否添加成功</returns>
        public unsafe bool AddPlainText(string text, short left, short top, short width, short height, byte colorIndex, byte blackgroundColorIndex, byte characterWidth, byte characterHeight)
        {
            if (!string.IsNullOrEmpty(text) && left + width > Width && left < Width && top < Height && top + height > Height
                && colorIndex < globalColorCount && blackgroundColorIndex < globalColorCount && characterWidth != 0 && characterHeight != 0 && isDisposed == 0)
            {
                fixed (byte* bufferFixed = fileBuffer)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *(short*)currentBuffer = 0x121;
                    *(currentBuffer + 2) = 12;
                    *(short*)(currentBuffer + 3) = left;
                    *(short*)(currentBuffer + 5) = top;
                    *(short*)(currentBuffer + 7) = width;
                    *(short*)(currentBuffer + 9) = height;
                    *(currentBuffer + 11) = characterWidth;
                    *(currentBuffer + 12) = characterHeight;
                    *(currentBuffer + 13) = colorIndex;
                    *(currentBuffer + 14) = blackgroundColorIndex;
                    checkBuffer(bufferFixed, 15);
                    addBlocks(bufferFixed, text);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加注释扩展
        /// </summary>
        /// <param name="comment">注释内容</param>
        /// <returns>注释扩展是否添加成功</returns>
        public unsafe bool AddComment(byte[] comment)
        {
            if (comment != null && comment.Length != 0 && isDisposed == 0)
            {
                fixed (byte* bufferFixed = fileBuffer)
                {
                    *(ushort*)(bufferFixed + bufferIndex) = 0xfe21;
                    checkBuffer(bufferFixed, 2);
                    fixed (byte* commentFixed = comment) addBlocks(bufferFixed, commentFixed, commentFixed + comment.Length);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加注释扩展
        /// </summary>
        /// <param name="comment">注释内容</param>
        /// <returns>注释扩展是否添加成功</returns>
        public unsafe bool AddComment(string comment)
        {
            if (!string.IsNullOrEmpty(comment) && isDisposed == 0)
            {
                fixed (byte* bufferFixed = fileBuffer)
                {
                    *(ushort*)(bufferFixed + bufferIndex) = 0xfe21;
                    checkBuffer(bufferFixed, 2);
                    addBlocks(bufferFixed, comment);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加应用程序扩展
        /// </summary>
        /// <param name="identifier">用来鉴别应用程序自身的标识(8个连续ASCII字符)</param>
        /// <param name="authenticationCode">应用程序定义的特殊标识码(3个连续ASCII字符)</param>
        /// <param name="customData">应用程序自定义数据块集合</param>
        /// <returns>应用程序扩展是否添加成功</returns>
#if NetStandard21
        public unsafe bool AddApplication(byte[] identifier, byte[] authenticationCode, byte[]? customData)
#else
        public unsafe bool AddApplication(byte[] identifier, byte[] authenticationCode, byte[] customData)
#endif
        {
            if (identifier != null && authenticationCode != null && ((identifier.Length ^ 8) | (authenticationCode.Length ^ 3) | isDisposed) == 0)
            {
                fixed (byte* bufferFixed = fileBuffer)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *(ushort*)currentBuffer = 0xff21;
                    *(currentBuffer + 2) = 11;
                    fixed (byte* identifierFixed = identifier) *(ulong*)(currentBuffer + 3) = *(ulong*)identifierFixed;
                    fixed (byte* authenticationCodeFixed = authenticationCode) *(int*)(currentBuffer + 11) = *(int*)authenticationCodeFixed;
                    if (customData == null || customData.Length == 0)
                    {
                        *(currentBuffer + 14) = 0;
                        checkBuffer(bufferFixed, 15);
                        return true;
                    }
                    checkBuffer(bufferFixed, 14);
                    fixed (byte* customDataFixed = customData) addBlocks(bufferFixed, customDataFixed, customDataFixed + customData.Length);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 颜色写入缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static unsafe byte* write(byte* data, LockBitmapColor color)
        {
            *data = color.Red;
            *(data + 1) = color.Green;
            *(data + 2) = color.Blue;
            return data + 3;
        }
    }
}
