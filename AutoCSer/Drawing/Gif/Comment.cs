using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 注释扩展
    /// </summary>
    public sealed class Comment : DataBlock
    {
        /// <summary>
        /// 数据块类型
        /// </summary>
        public override DataTypeEnum Type { get { return DataTypeEnum.Comment; } }
        /// <summary>
        /// 注释数据块
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 注释扩展
        /// </summary>
        /// <param name="decoder"></param>
        internal Comment(ref Decoder decoder)
        {
            decoder.GetBlocks(ref data);
        }
    }
}
