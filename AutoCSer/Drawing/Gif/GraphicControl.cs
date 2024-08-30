using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图形控制扩展
    /// </summary>
    public sealed class GraphicControl : DataBlock
    {
        /// <summary>
        /// 数据块类型
        /// </summary>
        public override DataTypeEnum Type { get { return DataTypeEnum.GraphicControl; } }
        /// <summary>
        /// 延迟时间，单位1/100秒
        /// </summary>
        public readonly short DelayTime;
        /// <summary>
        /// 透明色索引值
        /// </summary>
        public readonly byte TransparentColorIndex;
        /// <summary>
        /// 是否使用使用透明颜色
        /// </summary>
        public readonly byte IsTransparentColor;
        /// <summary>
        /// 用户输入标志，指出是否期待用户有输入之后才继续进行下去，置位表示期待，值否表示不期待。
        /// </summary>
        public readonly byte IsUseInput;
        /// <summary>
        /// 图形处置方法 0-7
        /// </summary>
        public readonly GraphicControlMethodTypeEnum MethodType;
        /// <summary>
        /// 图形控制扩展
        /// </summary>
        /// <param name="decoder"></param>
        unsafe internal GraphicControl(ref Decoder decoder)
        {
            byte* data = decoder.Data;
            if (decoder.End - data > 7)
            {
                ++data;
                if (((*data ^ 4) | *(data + 5)) == 0)
                {
                    byte flag = *(data + 1);
                    MethodType = (GraphicControlMethodTypeEnum)((flag >> 2) & 7);
                    IsUseInput = (byte)(flag & 2);
                    IsTransparentColor = (byte)(flag & 1);
                    DelayTime = *(short*)(data + 2);
                    TransparentColorIndex = *(data + 4);
                    decoder.Data = data + 6;
                    return;
                }
            }
            decoder.IsError = true;
        }
    }
}
