using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 索引中间结果
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ResultIndexs
    {
        /// <summary>
        /// 最小文本匹配索引位置
        /// </summary>
        private int index;
        /// <summary>
        /// 分词类型
        /// </summary>
        internal WordTypeEnum WordType;
        /// <summary>
        /// 其它文本匹配索引位置集合
        /// </summary>
        private LeftArray<int> indexs;
        /// <summary>
        /// 索引数量
        /// </summary>
        internal int IndexCount
        {
            get { return indexs.Length + 1; }
        }
        /// <summary>
        /// 文本匹配索引位置集合
        /// </summary>
        internal HeadLeftArray<int> Indexs
        {
            get { return new HeadLeftArray<int>(index, ref indexs); }
        }
        /// <summary>
        /// 索引中间结果
        /// </summary>
        /// <param name="wordType"></param>
        /// <param name="index"></param>
        internal ResultIndexs(WordTypeEnum wordType, int index)
        {
            WordType = wordType;
            this.index = index;
            indexs = new LeftArray<int>(0);
        }
        /// <summary>
        /// 设置文本长度与分词类型
        /// </summary>
        /// <param name="wordType">分词类型</param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(WordTypeEnum wordType, int index)
        {
            WordType = wordType;
            this.index = index;
            indexs = new LeftArray<int>(0);
        }
        /// <summary>
        /// 添加文本匹配索引位置
        /// </summary>
        /// <param name="index"></param>
        internal void Add(int index)
        {
            if (index < this.index)
            {
                indexs.Add(this.index);
                this.index = index;
            }
            else indexs.Add(index);
        }
        /// <summary>
        /// 文本匹配索引位置排序
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SortIndexs()
        {
            indexs.Sort();
        }
    }
}
