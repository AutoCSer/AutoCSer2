using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 查询分词结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct WordSegmentResult
    {
        /// <summary>
        /// 词语编号，0 表示未匹配词语
        /// </summary>
        public int Identity;
        /// <summary>
        /// 词语在原文本中的起始位置
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// 词语长度
        /// </summary>
        public int Length;
        /// <summary>
        /// 设置查询分词结果
        /// </summary>
        /// <param name="result"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ref KeyValue<SubString, int> result)
        {
            Identity = result.Value;
            StartIndex = result.Key.Start;
            Length = result.Key.Length;
        }
    }
}
