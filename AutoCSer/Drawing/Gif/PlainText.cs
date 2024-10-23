using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图形文本扩展
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    public sealed class PlainText : DataBlock
    {
        /// <summary>
        /// 数据块类型
        /// </summary>
        public override DataTypeEnum Type { get { return DataTypeEnum.PlainText; } }
        /// <summary>
        /// 文本框离逻辑屏幕的左边界距离
        /// </summary>
        public readonly short Left;
        /// <summary>
        /// 文本框离逻辑屏幕的上边界距离
        /// </summary>
        public readonly short Top;
        /// <summary>
        /// 文本框像素宽度
        /// </summary>
        public readonly short Width;
        /// <summary>
        /// 文本框像素高度
        /// </summary>
        public readonly short Height;
        /// <summary>
        /// 字符宽度
        /// </summary>
        public readonly short CharacterWidth;
        /// <summary>
        /// 字符高度
        /// </summary>
        public readonly short CharacterHeight;
        /// <summary>
        /// 前景色在全局颜色列表中的索引
        /// </summary>
        public readonly byte ColorIndex;
        /// <summary>
        /// 背景色在全局颜色列表中的索引
        /// </summary>
        public readonly byte BlackgroundColorIndex;
        /// <summary>
        /// 文本数据块集合
        /// </summary>
        private LeftArray<SubArray<byte>> textData;
        /// <summary>
        /// 文本数据
        /// </summary>
        public byte[] Text
        {
            get { return Decoder.BlocksToByte(ref textData); }
        }
        /// <summary>
        /// 图形文本扩展
        /// </summary>
        /// <param name="decoder"></param>
        unsafe internal PlainText(ref Decoder decoder)
        {
            byte* data = decoder.Data;
            if (*(data + 1) == 12)
            {
                Left = *(short*)(data + 2);
                Top = *(short*)(data + 4);
                Width = *(short*)(data + 6);
                Height = *(short*)(data + 8);
                CharacterWidth = *(data + 10);
                CharacterHeight = *(data + 11);
                ColorIndex = *(data + 12);
                BlackgroundColorIndex = *(data + 13);
                if ((data += 14) < decoder.End)
                {
                    textData = new LeftArray<SubArray<byte>>(0);
                    decoder.Data = data;
                    decoder.GetBlockList(ref textData);
                    return;
                }
            }
            decoder.IsError = true;
        }
    }
}
