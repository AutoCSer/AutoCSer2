using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF 文件数据块
    /// </summary>
    public abstract class DataBlock
    {
        /// <summary>
        /// 数据块类型
        /// </summary>
        public abstract DataTypeEnum Type { get; }
    }
}
