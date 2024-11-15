using AutoCSer.Memory;
using System;

namespace AutoCSer.Html
{
    /// <summary>
    /// 原始 HTML 节点，用于简单操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct RawNode
    {
        /// <summary>
        /// 字符串位置
        /// </summary>
        private readonly Range range;
        /// <summary>
        /// HTML 节点类型
        /// </summary>
        internal readonly RawNodeTypeEnum NodeType;
        /// <summary>
        /// 原始 HTML 节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        internal RawNode(RawNodeTypeEnum nodeType, long startIndex, int endIndex)
        {
            this.NodeType = nodeType;
            range = new Range(startIndex, endIndex);
        }
        /// <summary>
        /// 原始 HTML 节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        internal RawNode(RawNodeTypeEnum nodeType, long startIndex, long endIndex) : this(nodeType, startIndex, (int)endIndex) { }
        /// <summary>
        /// 获取标签名称位置
        /// </summary>
        /// <param name="htmlFixed"></param>
        /// <returns></returns>
        internal unsafe Range GetTag(char* htmlFixed)
        {
            switch (NodeType)
            {
                case RawNodeTypeEnum.StartTag:
                case RawNodeTypeEnum.Tag:
                    byte* bits = Bits.Byte;
                    for (char* current = htmlFixed + (range.StartIndex + 1), end = htmlFixed + (range.EndIndex - 1); current != end; ++current)
                    {
                        if (((bits[*(byte*)current] & TagNameSplitBit) | *(((byte*)current) + 1)) == 0) return new Range(range.StartIndex + 1, current - htmlFixed);
                    }
                    break;
            }
            return default(Range);
        }
        /// <summary>
        /// 获取标签属性名称
        /// </summary>
        /// <param name="htmlFixed"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal unsafe Range GetAttributeName(char* htmlFixed, int startIndex)
        {
            if (startIndex > range.StartIndex && startIndex < range.EndIndex)
            {
                switch (NodeType)
                {
                    case RawNodeTypeEnum.StartTag:
                    case RawNodeTypeEnum.Tag:
                        char* current = htmlFixed + startIndex;
                        if (*current != '>')
                        {
                            byte* bits = Bits.Byte;
                            while (((bits[*(byte*)current] & AttributeSplitBit) | *(((byte*)current) + 1)) == 0) ++current;
                            if (*current != '>')
                            {
                                startIndex = (int)(current - htmlFixed);
                                while (((bits[*(byte*)current] & TagNameSplitBit) | *(((byte*)current) + 1)) != 0) ++current;
                                return new Range(startIndex, (int)(current - htmlFixed));
                            }
                        }
                        break;
                }
            }
            return default(Range);
        }
        /// <summary>
        /// 获取标签属性值
        /// </summary>
        /// <param name="htmlFixed"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal unsafe Range GetAttributeValue(char* htmlFixed, int startIndex)
        {
            if (startIndex > range.StartIndex && startIndex < range.EndIndex)
            {
                switch (NodeType)
                {
                    case RawNodeTypeEnum.StartTag:
                    case RawNodeTypeEnum.Tag:
                        char* current = htmlFixed + startIndex;
                        byte* bits = Bits.Byte;
                        if (((bits[*(byte*)current] & AttributeNameSplitBit) | *(((byte*)current) + 1)) == 0) return new Range(startIndex, startIndex);
                        if (*current != '=')
                        {
                            while (((bits[*(byte*)current] & SpaceBit) | *(((byte*)current) + 1)) == 0) ++current;
                        }
                        if (*current != '=')
                        {
                            startIndex = (int)(current - htmlFixed);
                            return new Range(startIndex, startIndex);
                        }
                        do
                        {
                            ++current;
                        }
                        while (((bits[*(byte*)current] & SpaceBit) | *(((byte*)current) + 1)) == 0);
                        if (*current != '>')
                        {
                            char* end = htmlFixed + range.EndIndex - 1;
                            if (*current == '"')
                            {
                                char* start = ++current;
                                while (current < end && *current != '"') ++current;
                                return new Range(start - htmlFixed, current - htmlFixed);
                            }
                            if (*current == '\'')
                            {
                                char* start = ++current;
                                while (current < end && *current != '\'') ++current;
                                return new Range(start - htmlFixed, current - htmlFixed);
                            }
                            else
                            {
                                char* start = current;
                                do
                                {
                                    ++current;
                                }
                                while (((bits[*(byte*)current] & SpaceSplitBit) | *(((byte*)current) + 1)) != 0);
                                return new Range(start - htmlFixed, current - htmlFixed);
                            }
                        }
                        break;
                }
            }
            return default(Range);
        }
        /// <summary>
        /// 获取回合标签名称位置
        /// </summary>
        /// <param name="htmlFixed"></param>
        /// <returns></returns>
        internal unsafe Range GetRoundTag(char* htmlFixed)
        {
            if (NodeType == RawNodeTypeEnum.RoundTag)
            {
                byte* bits = Bits.Byte;
                for (char* current = htmlFixed + (range.StartIndex + 2), end = htmlFixed + (range.EndIndex - 1); current != end; ++current)
                {
                    if (((bits[*(byte*)current] & SpaceBit) | *(((byte*)current) + 1)) != 0)
                    {
                        if ((uint)((*current | 0x20) - 'a') <= 26)
                        {
                            char* start = current;
                            while (++current != end && ((bits[*(byte*)current] & TagNameSplitBit) | *(((byte*)current) + 1)) != 0) ;
                            return new Range(start - htmlFixed, current - htmlFixed);
                        }
                        return default(Range);
                    }
                }
            }
            return default(Range);
        }

        /// <summary>
        /// 解析 HTML 获取原始 HTML 节点集合
        /// </summary>
        /// <param name="html">原始 HTML 代码</param>
        /// <returns>原始 HTML 节点集合</returns>
        internal unsafe static LeftArray<RawNode> GetNodes(string html)
        {
            LeftArray<RawNode> nodes = new LeftArray<RawNode>(0);
            fixed (char* htmlFixed = html)
            {
                char* start = htmlFixed, end = htmlFixed + html.Length;
                byte* bits = Bits.Byte;
                for (char* current = htmlFixed; current < end;)
                {
                    while (*current != '<' && ++current != end) ;
                    if (current == end) break;
                    if (((bits[*(byte*)++current] & tagNameBit) | *(((byte*)current) + 1)) == 0)
                    {
                        int size = (int)(current - start);
                        if (size > 1) nodes.Add(new RawNode(RawNodeTypeEnum.Html, start - htmlFixed, (int)(current - htmlFixed) - 1));
                        start = current - 1;
                        RawNodeTypeEnum nodeType;
                        if (*current == '/')
                        {
                            nodeType = RawNodeTypeEnum.RoundTag;
                            while (++current < end && *current != '>') ;
                        }
                        else if (*current != '!')
                        {
                            if (*current != '>')
                            {
                                nodeType = RawNodeTypeEnum.StartTag;
                                while (++current < end)
                                {
                                    if (*current == '>')
                                    {
                                        if (*(current - 1) == '/') nodeType = RawNodeTypeEnum.Tag;
                                        break;
                                    }
                                }
                            }
                            else nodeType = RawNodeTypeEnum.NullTag;
                        }
                        else
                        {
                            if (*(int*)++current == '-' + ('-' << 16))
                            {
                                nodeType = RawNodeTypeEnum.Note;
                                for (current += 2; current < end; ++current)
                                {
                                    if (*current == '>' && *(int*)(current - 2) == '-' + ('-' << 16)) break;
                                }
                            }
                            else if ((*(long*)current | 0x20002000200000) == '[' + ('c' << 16) + ((long)'d' << 32) + ((long)'a' << 48)
                                && (*(ulong*)(current + 4) | 0xffff000000200020U) == ('t' | ('a' << 16) | ((ulong)'[' << 32) | 0xffff000000000000U))
                            {
                                nodeType = RawNodeTypeEnum.DataNote;
                                for (current += 9; current < end; ++current)
                                {
                                    if (*current == '>' && *(int*)(current - 2) == ']' + (']' << 16)) break;
                                }
                            }
                            else
                            {
                                nodeType = RawNodeTypeEnum.NoteTag;
                                while (current < end && *current != '>') ++current;
                            }
                        }
                        if (current < end)
                        {
                            nodes.Add(new RawNode(nodeType, start - htmlFixed, ++current - htmlFixed));
                            start = current;
                        }
                    }
                    else ++current;
                }
                if (start != end) nodes.Add(new RawNode(RawNodeTypeEnum.Html, start - htmlFixed, html.Length));
            }
            return nodes;
        }

        /// <summary>
        /// 空隔字符
        /// </summary>
        internal const int SpaceBit = 1;
        /// <summary>
        /// 空隔+结束字符
        /// </summary>
        internal const int SpaceSplitBit = 2;
        /// <summary>
        /// 标签名称结束字符
        /// </summary>
        private const int tagNameBit = 4;
        /// <summary>
        /// 标签名称开始字符
        /// </summary>
        internal const int TagNameSplitBit = 8;
        /// <summary>
        /// 标签属性分隔结束字符
        /// </summary>
        internal const int AttributeSplitBit = 0x10;
        /// <summary>
        /// 标签属性名称结束字符
        /// </summary>
        internal const int AttributeNameSplitBit = 0x20;
        /// <summary>
        /// 字符状态位
        /// </summary>
        internal static Pointer Bits;
        unsafe static RawNode()
        {
            Bits = AutoCSer.Extensions.Memory.Unmanaged.GetHtmlBits();
            byte* bits = Bits.Byte;
            AutoCSer.Common.Fill(Bits.ULong, 256 >> 3, ulong.MaxValue);
            bits['/'] &= (TagNameSplitBit | AttributeSplitBit | AttributeNameSplitBit | tagNameBit) ^ 255;
            bits['\t'] &= (SpaceBit | SpaceSplitBit | TagNameSplitBit | AttributeSplitBit) ^ 255;
            bits['\r'] &= (SpaceBit | SpaceSplitBit | TagNameSplitBit | AttributeSplitBit) ^ 255;
            bits['\n'] &= (SpaceBit | SpaceSplitBit | TagNameSplitBit | AttributeSplitBit) ^ 255;
            bits[' '] &= (SpaceBit | SpaceSplitBit | TagNameSplitBit | AttributeSplitBit) ^ 255;
            bits['>'] &= (SpaceSplitBit | TagNameSplitBit | AttributeNameSplitBit) ^ 255;
            bits['"'] &= (TagNameSplitBit | AttributeSplitBit | AttributeNameSplitBit) ^ 255;
            bits['\''] &= (TagNameSplitBit | AttributeSplitBit | AttributeNameSplitBit) ^ 255;
            bits['='] &= (TagNameSplitBit | AttributeSplitBit) ^ 255;
            bits['!'] &= tagNameBit ^ 255;
            for (int value = 'A'; value <= 'Z'; ++value)
            {
                bits[value] &= tagNameBit ^ 255;
                bits[value | 0x20] &= tagNameBit ^ 255;
            }
        }
    }
}
