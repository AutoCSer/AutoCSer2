using System;
using System.Runtime.Versioning;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 注释扩展
    /// </summary>
#if NET8
    [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
#endif
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
