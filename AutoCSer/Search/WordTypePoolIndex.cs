using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 关键字结果池索引+分词类型
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct WordTypePoolIndex
    {
        /// <summary>
        /// 结果池索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 分词类型
        /// </summary>
        private WordTypeEnum wordType;
        /// <summary>
        /// 设置结果池索引
        /// </summary>
        /// <param name="index">结果池索引</param>
        /// <param name="wordType">分词类型</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int index, WordTypeEnum wordType)
        {
            Index = index;
            this.wordType = wordType;
        }
    }
}
