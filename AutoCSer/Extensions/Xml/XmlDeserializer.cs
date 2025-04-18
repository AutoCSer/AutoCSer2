using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// XML 解析器
    /// </summary>
    public unsafe sealed partial class XmlDeserializer : TextDeserializer<XmlDeserializer>
    {
        /// <summary>
        /// XML 反序列化方法名称
        /// </summary>
        internal const string XmlDeserializeMethodName = "XmlDeserialize";
        /// <summary>
        /// 获取 XML 反序列化成员名称
        /// </summary>
        internal const string XmlDeserializeMemberNameMethodName = "XmlDeserializeMemberNames";
        /// <summary>
        /// 默认解析所有成员
        /// </summary>
        internal static readonly XmlSerializeAttribute AllMemberAttribute = XmlSerializer.ConfigurationAttribute ?? new XmlSerializeAttribute { Filter = Metadata.MemberFiltersEnum.Instance, IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly XmlDeserializeConfig DefaultConfig = AutoCSer.Configuration.Common.Get<XmlDeserializeConfig>()?.Value ?? new XmlDeserializeConfig();

        /// <summary>
        /// 配置参数
        /// </summary>
        internal XmlDeserializeConfig Config;
        /// <summary>
        /// 集合子节点名称
        /// </summary>
#if NetStandard21
        internal string? ItemName;
#else
        internal string ItemName;
#endif
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        internal string ArrayItemName
        {
            get
            {
                if (ItemName == null) return Config.ItemName ?? XmlSerializeConfig.DefaultItemName;
                string value = ItemName;
                ItemName = null;
                return value;
            }
        }
        /// <summary>
        /// 属性
        /// </summary>
        private LeftArray<KeyValue<Range, Range>> attributes = new LeftArray<KeyValue<Range, Range>>(0);
        /// <summary>
        /// 当前数据起始位置
        /// </summary>
        private char* valueStart;
        /// <summary>
        /// 当前数据长度
        /// </summary>
        private int valueSize;
        /// <summary>
        /// 属性名称起始位置
        /// </summary>
        private int attributeNameStartIndex;
        /// <summary>
        /// 属性名称结束位置
        /// </summary>
        private int attributeNameEndIndex;
        /// <summary>
        /// 数字符号
        /// </summary>
        private char sign;
        /// <summary>
        /// 解析状态
        /// </summary>
        internal DeserializeStateEnum State;
        /// <summary>
        /// 当前数据是否CDATA
        /// </summary>
        internal byte IsCData;
        /// <summary>
        /// 名称解析节点是否结束
        /// </summary>
        private byte isTagEnd;
        /// <summary>
        /// XML 解析器
        /// </summary>
        internal XmlDeserializer() : base(DeserializeBits.Byte)
        {
            Config = DefaultConfig;
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(ref SubString xml, ref T? value, XmlDeserializeConfig? config)
#else
        private DeserializeResult deserialize<T>(ref SubString xml, ref T value, XmlDeserializeConfig config)
#endif
        {
            fixed (char* xmlFixed = (this.text = xml.String.notNull()))
            {
                Current = (this.textFixed = xmlFixed) + xml.Start;
                this.Config = config ?? DefaultConfig;
                end = Current + xml.Length;
                deserialize(ref value);
                if (State == DeserializeStateEnum.Success)
                {
                    DeserializeResult result = new DeserializeResult(MemberMap);
                    MemberMap = null;
                    return result;
                }
                return new DeserializeResult(State, ref xml, (int)(Current - xmlFixed) - xml.Start, customError);
            }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(string xml, ref T? value, XmlDeserializeConfig? config)
#else
        private DeserializeResult deserialize<T>(string xml, ref T value, XmlDeserializeConfig config)
#endif
        {
            fixed (char* xmlFixed = (this.text = xml))
            {
                Current = this.textFixed = xmlFixed;
                this.Config = config ?? DefaultConfig;
                end = xmlFixed + xml.Length;
                deserialize(ref value);
                if (State == DeserializeStateEnum.Success)
                {
                    DeserializeResult result = new DeserializeResult(MemberMap);
                    MemberMap = null;
                    return result;
                }
                return new DeserializeResult(State, (int)(Current - xmlFixed), xml, customError);
            }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="length"></param>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(char* xml, int length, ref T? value)
#else
        private DeserializeResult deserialize<T>(char* xml, int length, ref T value)
#endif
        {
            Config = DefaultConfig;
            end = (textFixed = Current = xml) + length;
            deserialize(ref value);
            if (State == DeserializeStateEnum.Success)
            {
                DeserializeResult result = new DeserializeResult(MemberMap);
                MemberMap = null;
                return result;
            }
            return new DeserializeResult(State, (int)(Current - textFixed), Config.IsErrorNewString ? new string(xml, 0, length) : null, customError);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private void deserialize<T>(ref T? value)
#else
        private void deserialize<T>(ref T value)
#endif
        {
            string bootName = Config.BootNodeName;
            fixed (char* bootNameFixed = bootName)
            {
            NEXTEND:
                while (end != Current)
                {
                    if (((bits[*(byte*)--end] & spaceBit) | *(((byte*)end) + 1)) != 0)
                    {
                        if (*end == '>')
                        {
                            if (*(end - 1) == '-')
                            {
                                if (*(end - 2) == '-' && (end -= 2 + 3) > Current)
                                {
                                    do
                                    {
                                    NOTE:
                                        if (*--end == '<')
                                        {
                                            if (*(end + 1) == '!' && *(int*)(end + 2) == '-' + ('-' << 16)) goto NEXTEND;
                                            if ((end -= 3) <= Current) break;
                                            goto NOTE;
                                        }
                                    }
                                    while (end != Current);
                                }
                                State = DeserializeStateEnum.NoteError;
                            }
                            else if ((end -= (2 + bootName.Length)) > Current && *(int*)end == ('<' + ('/' << 16))
                                && AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)bootNameFixed, (byte*)(end + 2), bootName.Length << 1))
                            {
                                State = DeserializeStateEnum.Success;
                                space();
                                if (State == DeserializeStateEnum.Success)
                                {
                                    if (*(int*)Current == ('<' + ('?' << 16)))
                                    {
                                        Current += 3;
                                        do
                                        {
                                            if (*Current == '>')
                                            {
                                                if (Current <= end)
                                                {
                                                    if (*(Current - 1) == '?')
                                                    {
                                                        ++Current;
                                                        break;
                                                    }
                                                    else State = DeserializeStateEnum.HeaderError;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                                return;
                                            }
                                            ++Current;
                                        }
                                        while (true);
                                        space();
                                        if (State != DeserializeStateEnum.Success) return;
                                    }
                                    if (*Current == '<' && AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)bootNameFixed, (byte*)(++Current), bootName.Length << 1))
                                    {
                                        if (((bits[*(byte*)(Current += bootName.Length)] & spaceBit) | *(((byte*)Current) + 1)) != 0)
                                        {
                                            if (*Current == '>')
                                            {
                                                attributes.Length = 0;
                                                ++Current;
                                                deserializeValue(ref value);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            ++Current;
                                            attribute();
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                deserializeValue(ref value);
                                                return;
                                            }
                                        }
                                    }
                                }
                                else State = DeserializeStateEnum.NotFoundBootNodeStart;
                                return;
                            }
                        }
                        State = DeserializeStateEnum.NotFoundBootNodeEnd;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        private void deserializeValue<T>(ref T? value)
#else
        private void deserializeValue<T>(ref T value)
#endif
        {
            TypeDeserializer<T>.DefaultDeserializer(this, ref value);
            if (State == DeserializeStateEnum.Success)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current == end) return;
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
        }
        /// <summary>
        /// 释放 XML 解析器（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeThreadStatic()
        {
            free();
            ItemName = null;
            Config = DefaultConfig;
        }
        /// <summary>
        /// 释放 XML 解析器
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            AutoCSer.Threading.LinkPool<XmlDeserializer>.Default.Push(this);
        }
        /// <summary>
        /// 空格过滤
        /// </summary>
        private void space()
        {
        START:
            while (((bits[*(byte*)Current] & spaceBit) | *(((byte*)Current) + 1)) == 0) ++Current;
            if (*(long*)Current == '<' + ('!' << 16) + ((long)'-' << 32) + ((long)'-' << 48))
            {
                Current += 6;
                do
                {
                    if (*Current == '>')
                    {
                        if (Current > end)
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return;
                        }
                        if (*(int*)(Current - 2) == '-' + ('-' << 16))
                        {
                            ++Current;
                            goto START;
                        }
                        Current += 3;
                    }
                    else ++Current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 属性解析
        /// </summary>
        private void attribute()
        {
            attributes.Length = 0;
            while (((bits[*(byte*)Current] & spaceBit) | *(((byte*)Current) + 1)) == 0) ++Current;
            if (*Current == '>')
            {
                if (Current++ <= end) isTagEnd = 0;
                else State = DeserializeStateEnum.CrashEnd;
                return;
            }
            if (*Current == '/')
            {
                if (*(Current + 1) == '>')
                {
                    isTagEnd = 1;
                    Current += 2;
                }
                else State = DeserializeStateEnum.NotFoundTagStart;
                return;
            }
            attributeName();
        }
        /// <summary>
        /// 属性名称解析
        /// </summary>
        private void attributeName()
        {
        NAME:
            attributeNameStartIndex = (int)(Current - textFixed);
            do
            {
                if (((bits[*(byte*)++Current] & attributeNameSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    switch (*Current & 7)
                    {
                        case '\t' & 7:
                        case ' ' & 7:
                        case '\n' & 7:
                        SPACE:
                            attributeNameEndIndex = (int)(Current - textFixed);
                            while (((bits[*(byte*)++Current] & spaceBit) | *(((byte*)Current) + 1)) == 0) ;
                            if (*Current == '=')
                            {
                                if (attributeValue() == 0) return;
                                goto NAME;
                            }
                            break;
                        case '=' & 7:
                            if (*Current == '=')
                            {
                                attributeNameEndIndex = (int)(Current - textFixed);
                                if (attributeValue() == 0) return;
                                goto NAME;
                            }
                            goto SPACE;
                    }
                    State = DeserializeStateEnum.NotFoundAttributeName;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 属性值解析
        /// </summary>
        /// <returns></returns>
        private int attributeValue()
        {
            while (((bits[*(byte*)++Current] & spaceBit) | *(((byte*)Current) + 1)) == 0) ;
            if (*Current == '"')
            {
                int valueStartIndex = (int)(++Current - textFixed);
                do
                {
                    if (*Current == '"')
                    {
                        if (Config.IsAttribute)
                        {
                            attributes.Add(new KeyValue<Range, Range>(new Range(attributeNameStartIndex, attributeNameEndIndex), new Range(valueStartIndex, (int)(Current - textFixed))));
                        }
                        while (((bits[*(byte*)++Current] & spaceBit) | *(((byte*)Current) + 1)) == 0) ;
                        if (*Current == '>')
                        {
                            if (Current++ <= end) isTagEnd = 0;
                            else State = DeserializeStateEnum.CrashEnd;
                            return 0;
                        }
                        if (*Current == '/')
                        {
                            if (*(Current + 1) == '>')
                            {
                                isTagEnd = 1;
                                Current += 2;
                            }
                            else State = DeserializeStateEnum.NotFoundTagStart;
                            return 0;
                        }
                        return 1;
                    }
                    if (*Current == '<' && Current >= end)
                    {
                        State = DeserializeStateEnum.NotFoundAttributeValue;
                        return 0;
                    }
                    ++Current;
                }
                while (true);
            }
            State = DeserializeStateEnum.NotFoundAttributeValue;
            return 0;
        }
        /// <summary>
        /// 判断否存存在数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int IsValue()
        {
            space();
            return State == DeserializeStateEnum.Success ? *(int*)Current ^ ('<' + ('/' << 16)) : 0;
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        /// <returns>是否成功</returns>
        internal int IgnoreValue()
        {
            LeftArray<AutoCSer.Memory.Pointer> pointerArray = new LeftArray<AutoCSer.Memory.Pointer>(0);
        START:
            space();
            if (State != DeserializeStateEnum.Success) return 0;
            if (*Current == '<')
            {
                char code = *(Current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/') goto CHECK;
                    if (code == '!')
                    {
                        if (((*(int*)(Current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(Current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(Current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(Current + 8) ^ '[')) == 0)
                        {
                            Current += 11;
                            do
                            {
                                if (*Current == '>')
                                {
                                    if (*(int*)(Current - 2) == (']' + (']' << 16)))
                                    {
                                        ++Current;
                                        goto CHECK;
                                    }
                                    else if (Current < end) Current += 3;
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return 0;
                                    }
                                }
                                else ++Current;
                            }
                            while (true);
                        }
                    }
                    State = DeserializeStateEnum.NotFoundTagStart;
                    return 0;
                }
                int nameSize = 0;
                char* nameStart = getNameOnly(ref nameSize);
                if (nameStart == null) return 0;
                if (isTagEnd == 0) pointerArray.Add(new AutoCSer.Memory.Pointer { Data = nameStart, ByteSize = nameSize });
                goto START;
            }
            while (*++Current != '<') ;
            CHECK:
            if (pointerArray.Length != 0)
            {
                AutoCSer.Memory.Pointer name = pointerArray.Pop();
                if (CheckNameEnd(name.Char, name.ByteSize) == 0) return 0;
                goto START;
            }
            return 1;
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="nameSize">节点名称长度</param>
        /// <returns></returns>
        private char* getName(ref int nameSize)
        {
            space();
            if (State != DeserializeStateEnum.Success) return null;
            if (*Current == '<')
            {
                char code = *(Current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/') return null;
                }
                else return getNameOnly(ref nameSize);
            }
            State = DeserializeStateEnum.NotFoundTagStart;
            return null;
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="nameSize">节点名称长度</param>
        /// <returns></returns>
        private char* getNameOnly(ref int nameSize)
        {
            char* nameStart = ++Current;
            do
            {
                if (((bits[*(byte*)++Current] & spaceBit) | *(((byte*)Current) + 1)) != 0)
                {
                    if (*Current == '>')
                    {
                        if (Current < end)
                        {
                            isTagEnd = 0;
                            attributes.Length = 0;
                            nameSize = (int)(Current++ - nameStart);
                            return nameStart;
                        }
                        State = DeserializeStateEnum.CrashEnd;
                        return null;
                    }
                    if (*Current == '/')
                    {
                        if (*(Current + 1) == '>')
                        {
                            nameSize = (int)(Current - nameStart);
                            isTagEnd = 1;
                            attributes.Length = 0;
                            Current += 2;
                            return nameStart;
                        }
                        State = DeserializeStateEnum.NotFoundTagStart;
                        return null;
                    }
                }
                else
                {
                    nameSize = (int)(Current++ - nameStart);
                    attribute();
                    return State == DeserializeStateEnum.Success ? nameStart : null;
                }
            }
            while (true);
        }
        /// <summary>
        /// 节点名称结束检测
        /// </summary>
        /// <param name="nameStart"></param>
        /// <param name="nameSize"></param>
        /// <returns></returns>
        internal int CheckNameEnd(char* nameStart, int nameSize)
        {
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (*(int*)Current == '<' + ('/' << 16) && *(Current + (2 + nameSize)) == '>' && AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)(Current + 2), (byte*)nameStart, nameSize << 1) && Current != end)
                {
                    Current += nameSize + 3;
                    return 1;
                }
                State = DeserializeStateEnum.NotFoundTagEnd;
            }
            return 0;
        }
        /// <summary>
        /// 节点名称结束检测
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int CheckNameEnd(ref Pointer name)
        {
            return CheckNameEnd(name.Char, name.ByteSize);
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool IsName(byte* names, ref int index)
        {
            int length = *(short*)names;
            if (length == 0)
            {
                index = -1;
                return true;
            }
            if (AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)Current, names += sizeof(short), length))
            {
                Current = (char*)((byte*)Current + length);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        internal byte IsNameEnd(byte* names)
        {
            if (*(int*)Current == '<' + ('/' << 16))
            {
                int length = *(short*)names - sizeof(char);
                if (AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)Current + sizeof(int), names + (sizeof(short) + sizeof(char)), length) && Current != end)
                {
                    Current = (char*)((byte*)Current + (length + sizeof(int)));
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="isTagEnd">名称解析节点是否结束</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetName(ref Pointer name, ref byte isTagEnd)
        {
            name.Data = getName(ref name.ByteSize);
            isTagEnd = this.isTagEnd;
            return name.Data == null;
        }
        /// <summary>
        /// 是否存在数组数据
        /// </summary>
        /// <param name="nameStart"></param>
        /// <param name="nameSize"></param>
        /// <returns></returns>
        internal int IsArrayItem(char* nameStart, int nameSize)
        {
            if (*(int*)Current == '<' + ('/' << 16) && *(Current + (2 + nameSize)) == '>' && AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)(Current + 2), (byte*)nameStart, nameSize << 1) && Current != end)
            {
                Current += nameSize + 3;
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// 是否存在数组数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int IsArrayItem(ref Pointer name)
        {
            return IsArrayItem(name.Char, name.ByteSize);
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsEnumNumberUnsigned()
        {
            searchValue();
            return (uint)(*Current - '0') < 10;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsEnumNumberSigned()
        {
            searchValue();
            uint number = (uint)(*Current - '0');
            return number < 10 || (int)number == '-' - '0';
        }
        /// <summary>
        /// 查找数据起始位置
        /// </summary>
        private void searchValue()
        {
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (*Current == '<')
                {
                    switch (*(Current + 1))
                    {
                        case '/':
                            IsCData = 0;
                            return;
                        case '!':
                            if (((*(int*)(Current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(Current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(Current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(Current + 8) ^ '[')) == 0)
                            {
                                Current += 9;
                                IsCData = 1;
                                return;
                            }
                            break;
                    }
                    State = DeserializeStateEnum.NotFoundValue;
                    IsCData = 2;
                }
                else IsCData = 0;
            }
            else IsCData = 2;
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        internal void IgnoreSearchValue()
        {
            if (IsCData == 0)
            {
                while (*Current != '<') ++Current;
            }
            else
            {
                Current += 2;
                do
                {
                    if (*Current == '>')
                    {
                        if (*(int*)(Current - 2) == (']' + (']' << 16)))
                        {
                            ++Current;
                            space();
                            if (*Current != '<') State = DeserializeStateEnum.NotFoundValueEnd;
                            return;
                        }
                        else if (Current < end) Current += 3;
                        else
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return;
                        }
                    }
                    else ++Current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 枚举值是否结束
        /// </summary>
        /// <returns></returns>
        internal int IsNextFlagEnum()
        {
            if (State == DeserializeStateEnum.Success)
            {
                if (IsCData == 0)
                {
                START:
                    switch ((*Current >> 3) & 3)
                    {
                        case (' ' >> 3) & 3:
                        case (',' >> 3) & 3:
                            if (*Current == ',' || *Current == ' ')
                            {
                                ++Current;
                                goto START;
                            }
                            return 1;
                        case ('<' >> 3) & 3:
                            return *Current - '<';
                        default:
                            return 1;
                    }
                }
                else
                {
                    while (valueSize != 0)
                    {
                        if (*valueStart == ',' || *valueStart == ' ')
                        {
                            --valueSize;
                            ++valueStart;
                            continue;
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 数据结束处理
        /// </summary>
        internal void SearchValueEnd()
        {
            switch (IsCData)
            {
                case 0:
                SPACE:
                    space();
                    if (*Current == '<' || State != DeserializeStateEnum.Success) return;
                    break;
                case 1:
                    if (((*(int*)Current ^ (']' + (']' << 16))) | (*(short*)(Current + 2) ^ '>')) == 0)
                    {
                        Current += 3;
                        goto SPACE;
                    }
                    break;
            }
            State = DeserializeStateEnum.NotFoundValueEnd;
        }
        /// <summary>
        /// 读取下一个枚举字符
        /// </summary>
        /// <returns>枚举字符,结束或者错误返回0</returns>
        internal char NextEnumChar()
        {
            if (((bits[*(byte*)Current] & spaceBit) | *(((byte*)Current) + 1)) != 0)
            {
                if (*Current == '<') return (char)0;
                if (*Current == '&')
                {
                    char value = (char)0;
                    decodeChar(ref value);
                    return value;
                }
                return *Current++;
            }
            State = DeserializeStateEnum.NotEnumChar;
            return (char)0;
        }
        /// <summary>
        /// 读取下一个枚举字符
        /// </summary>
        /// <returns>枚举字符,结束或者错误返回0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal char NextCDataEnumChar()
        {
            if (valueSize == 0) return (char)0;
            --valueSize;
            return *valueStart++;
        }
        /// <summary>
        /// 字符解码
        /// </summary>
        /// <param name="value"></param>
        private void decodeChar(ref char value)
        {
            if (*++valueStart == '#')
            {
                uint code = (uint)(*++valueStart - '0');
                if (code < 10)
                {
                    do
                    {
                        uint number = (uint)(*++valueStart - '0');
                        if (number < 10) code = code * 10 + number;
                        else
                        {
                            if (number == ';' - '0')
                            {
                                ++valueStart;
                                value = (char)code;
                            }
                            else State = DeserializeStateEnum.DecodeError;
                            return;
                        }
                    }
                    while (true);
                }
            }
            else
            {
                int code = decodeSearcher.UnsafeSearch(ref valueStart);
                if (code > 0)
                {
                    value = (char)code;
                    return;
                }
            }
            State = DeserializeStateEnum.DecodeError;
        }
        /// <summary>
        /// 查找CDATA数据结束位置
        /// </summary>
        internal void SearchCDataValue()
        {
            valueStart = Current;
            Current += 2;
            do
            {
                if (*Current == '>')
                {
                    char* valueEnd = Current - 2;
                    if (*(int*)valueEnd == (']' + (']' << 16)))
                    {
                        ++Current;
                        valueSize = (int)(valueEnd - valueStart);
                        return;
                    }
                    else if (Current < end) Current += 3;
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else ++Current;
            }
            while (true);
        }
        /// <summary>
        /// 获取文本数据
        /// </summary>
        private void getValue()
        {
            space();
            if (State != DeserializeStateEnum.Success) return;
            if (*Current == '<')
            {
                switch (*(Current + 1))
                {
                    case '/':
                        valueStart = Current;
                        IsCData = 1;
                        valueSize = 0;
                        return;
                    case '!':
                        if (((*(int*)(Current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(Current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(Current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(Current + 8) ^ '[')) == 0)
                        {
                            valueStart = Current + 9;
                            Current += 11;
                            do
                            {
                                if (*Current == '>')
                                {
                                    char* cDataValueEnd = Current - 2;
                                    if (*(int*)cDataValueEnd == (']' + (']' << 16)))
                                    {
                                        ++Current;
                                        valueSize = (int)(cDataValueEnd - valueStart);
                                        IsCData = 1;
                                        return;
                                    }
                                    else if (Current < end) Current += 3;
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return;
                                    }
                                }
                                else ++Current;
                            }
                            while (true);
                        }
                        break;
                }
                State = DeserializeStateEnum.NotFoundValue;
                return;
            }
            valueStart = Current;
            while (*++Current != '<') ;
            valueSize = (int)(endSpace() - valueStart);
            IsCData = 0;
            return;
        }
        /// <summary>
        /// 数据结束处理
        /// </summary>
        private void getValueEnd()
        {
            if (IsCData != 0)
            {
                space();
                if (*Current != '<') State = DeserializeStateEnum.NotFoundValueEnd;
            }
        }
        /// <summary>
        /// 空格过滤
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private char* endSpace()
        {
            char* end = Current;
            while (((bits[*(byte*)--end] & spaceBit) | *(((byte*)end) + 1)) == 0) ;
            return end + 1;
        }

        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        private void deserializeHex32(ref uint value)
        {
            uint isValue = 0;
            do
            {
                uint number = (uint)(*Current - '0');
                if (number > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        if (isValue == 0) State = DeserializeStateEnum.NotHex;
                        return;
                    }
                    number += 10;
                }
                ++Current;
                value <<= 4;
                isValue = 1;
                value += number;
            }
            while (true);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private uint deserializeUInt32(uint value)
        {
            uint number;
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value *= 10;
                ++Current;
                value += (byte)number;
            }
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref byte value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = (byte)number;
                        }
                        else value = 0;
                    }
                    else value = (byte)deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref sbyte value)
        {
            if (IsCData != 2)
            {
                if ((sign = *Current) == '-') ++Current;
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = sign == '-' ? (sbyte)-(int)number : (sbyte)(byte)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (sbyte)-(int)deserializeUInt32(number) : (sbyte)(byte)deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref ushort value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = (ushort)number;
                        }
                        else value = 0;
                    }
                    else value = (ushort)deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref short value)
        {
            if (IsCData != 2)
            {
                if ((sign = *Current) == '-') ++Current;
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = sign == '-' ? (short)-(int)number : (short)(ushort)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (short)-(int)deserializeUInt32(number) : (short)(ushort)deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref uint value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = number;
                        }
                        else value = 0;
                    }
                    else value = deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref int value)
        {
            if (IsCData != 2)
            {
                if ((sign = *Current) == '-') ++Current;
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            deserializeHex32(ref number);
                            if (State != DeserializeStateEnum.Success) return;
                            value = sign == '-' ? -(int)number : (int)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(int)deserializeUInt32(number) : (int)deserializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        private ulong deserializeHex64()
        {
            ulong number = 0;
            uint isValue = 0, value;
            do
            {
                if ((value = (uint)(*Current - '0')) > 9)
                {
                    if ((value = (value - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        if (isValue == 0) State = DeserializeStateEnum.NotHex;
                        return 0;
                    }
                    value += 10;
                }
                ++Current;
                number <<= 4;
                isValue = 1;
                number += value;
            }
            while (true);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private ulong deserializeUInt64(uint value)
        {
            ulong number = value;
            while ((value = (uint)(*Current - '0')) < 10)
            {
                number *= 10;
                ++Current;
                number += value;
            }
            return number;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref ulong value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            value = deserializeHex64();
                            if (State != DeserializeStateEnum.Success) return;
                        }
                        else value = 0;
                    }
                    else value = deserializeUInt64(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void DeserializeNumber(ref long value)
        {
            if (IsCData != 2)
            {
                if ((sign = *Current) == '-') ++Current;
                uint number = (uint)(*Current - '0');
                if (number < 10)
                {
                    ++Current;
                    if (number == 0)
                    {
                        if (*Current == 'x')
                        {
                            ++Current;
                            value = sign == '-' ? -(long)deserializeHex64() : (long)deserializeHex64();
                            if (State != DeserializeStateEnum.Success) return;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(long)deserializeUInt64(number) : (long)deserializeUInt64(number);
                    SearchValueEnd();
                    return;
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }

        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Deserialize<T>(XmlDeserializer deserializer, ref T? value)
#else
        internal static void Deserialize<T>(XmlDeserializer deserializer, ref T value)
#endif
        {
            TypeDeserializer<T>.DefaultDeserializer(deserializer, ref value);
        }
        /// <summary>
        /// 自定义反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ICustom<T>(AutoCSer.XmlDeserializer deserializer, ref T value)
             where T : ICustomSerialize<T>
        {
            value.Deserialize(deserializer);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Array<T>(XmlDeserializer deserializer, ref T?[]? array)
#else
        public static void Array<T>(XmlDeserializer deserializer, ref T[] array)
#endif
        {
            TypeDeserializer<T>.Array(deserializer, ref array);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void XmlDeserialize<T>(ref LeftArray<T?> values)
#else
        public void XmlDeserialize<T>(ref LeftArray<T> values)
#endif
        {
#if NetStandard21
            var array = default(T?[]);
#else
            var array = default(T[]);
#endif
            int count = TypeDeserializer<T>.ArrayIndex(this, ref array);
#if NetStandard21
            if (count != -1) values = new LeftArray<T?>(count, array.notNull());
#else
            if (count != -1) values = new LeftArray<T>(count, array);
#endif
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="values"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void LeftArray<T>(XmlDeserializer deserializer, ref LeftArray<T?> values)
#else
        public static void LeftArray<T>(XmlDeserializer deserializer, ref LeftArray<T> values)
#endif
        {
            deserializer.XmlDeserialize(ref values);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void XmlDeserialize<T>(ref ListArray<T?>? values)
#else
        public void XmlDeserialize<T>(ref ListArray<T> values)
#endif
        {
#if NetStandard21
            var array = default(T?[]);
#else
            var array = default(T[]);
#endif
            int count = TypeDeserializer<T>.ArrayIndex(this, ref array);
#if NetStandard21
            if (count != -1) values = new ListArray<T?>(count, array.notNull());
#else
            if (count != -1) values = new ListArray<T>(count, array);
#endif
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="values"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ListArray<T>(XmlDeserializer deserializer, ref ListArray<T?>? values)
#else
        public static void ListArray<T>(XmlDeserializer deserializer, ref ListArray<T> values)
#endif
        {
            deserializer.XmlDeserialize(ref values);
        }
        /// <summary>
        /// 集合解析
        /// </summary>
        /// <returns>目标数据</returns>
#if NetStandard21
        private System.Collections.Generic.IEnumerable<T?> enumerable<T>()
#else
        private System.Collections.Generic.IEnumerable<T> enumerable<T>()
#endif
        {
            string arrayItemName = ArrayItemName;
            fixed (char* itemFixed = arrayItemName) return TypeDeserializer<T>.Enumerable(this, new AutoCSer.Memory.Pointer(itemFixed, arrayItemName.Length));
        }
        /// <summary>
        /// 集合反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="collection"></param>
#if NetStandard21
        public void XmlDeserialize<T, VT>(ref T? collection) where T : ICollection<VT?>
#else
        public void XmlDeserialize<T, VT>(ref T collection) where T : ICollection<VT>
#endif
        {
            if (Constructor(out collection))
            {
                string arrayItemName = ArrayItemName;
                fixed (char* itemFixed = arrayItemName)
                {
                    foreach (var value in TypeDeserializer<VT>.Enumerable(this, new AutoCSer.Memory.Pointer(itemFixed, arrayItemName.Length)))
                    {
                        collection.Add(value);
                    }
                }
            }
        }
        /// <summary>
        /// 集合反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="collection"></param>
#if NetStandard21
        public static void Collection<T, VT>(XmlDeserializer deserializer, ref T? collection) where T : ICollection<VT?>
#else
        public static void Collection<T, VT>(XmlDeserializer deserializer, ref T collection) where T : ICollection<VT>
#endif
        {
            deserializer.XmlDeserialize<T, VT>(ref collection);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void XmlDeserialize<T>(ref T? value) where T : struct
        {
            T newValue = value.HasValue ? value.Value : default(T);
            TypeDeserializer<T>.DefaultDeserializer(this, ref newValue);
            value = newValue;
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Nullable<T>(XmlDeserializer deserializer, ref T? value) where T : struct
        {
            deserializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="value">目标数据</param>
#if NetStandard21
        private void baseDeserialize<T, BT>(ref T? value) where T : class, BT
#else
        private void baseDeserialize<T, BT>(ref T value) where T : class, BT
#endif
        {
            if (value == null)
            {
                if (IsValue() == 0) return;
                if (AutoCSer.Metadata.DefaultConstructor<T>.Type != Metadata.DefaultConstructorTypeEnum.None)
                {
                    if (!Constructor(out value)) return;
                }
                else if (!AutoCSer.XmlSerializer.CustomConfig.CallCustomConstructor(out value))
                {
                    IgnoreValue();
                    return;
                }
            }
            var newValue = (BT)value.notNull();
            TypeDeserializer<BT>.DefaultDeserializer(this, ref newValue);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Base<T, BT>(XmlDeserializer deserializer, ref T? value) where T : class, BT
#else
        public static void Base<T, BT>(XmlDeserializer deserializer, ref T value) where T : class, BT
#endif
        {
            deserializer.baseDeserialize<T, BT>(ref value);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value">目标数据</param>
        public void XmlDeserialize<KT, VT>(ref KeyValuePair<KT, VT> value)
        {
            BinarySerializeKeyValue<KT, VT> keyValue = new BinarySerializeKeyValue<KT, VT>(value.Key, value.Value);
            TypeDeserializer<BinarySerializeKeyValue<KT, VT>>.DefaultDeserializer(this, ref keyValue);
            value = new KeyValuePair<KT, VT>(keyValue.Key, keyValue.Value);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void KeyValuePair<KT, VT>(XmlDeserializer deserializer, ref KeyValuePair<KT, VT> value)
        {
            deserializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void NotSupport<T>(XmlDeserializer deserializer, ref T? value)
#else
        public static void NotSupport<T>(XmlDeserializer deserializer, ref T value)
#endif
        {
            if (!XmlSerializer.CustomConfig.NotSupport(deserializer, ref value)) deserializer.State = DeserializeStateEnum.NotSupport;
        }
        /// <summary>
        /// 构造函数调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Constructor<T>([NotNullWhen(true)] out T? value)
#else
        internal bool Constructor<T>(out T value)
#endif
        {
            value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            if (value != null) return true;
            State = DeserializeStateEnum.ConstructorNull;
            customError = $"{typeof(T).fullName()} 默认构造缺少返回值";
            return false;
        }

        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool CustomDeserialize<T>(ref T? value)
#else
        public bool CustomDeserialize<T>(ref T value)
#endif
        {
            TypeDeserializer<T>.DefaultDeserializer(this, ref value);
            return State == DeserializeStateEnum.Success;
        }
        /// <summary>
        /// 设置反序列化自定义错误状态
        /// </summary>
        /// <param name="customError">自定义错误</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetCustomError(string customError)
        {
            if (State == DeserializeStateEnum.Success)
            {
                State = DeserializeStateEnum.CustomError;
                this.customError = customError;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref bool value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((*Current | 0x20) == 'f')
                {
                    if ((*(long*)(Current + 1) | 0x20002000200020L) == ('a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48)))
                    {
                        Current += 5;
                        value = false;
                        SearchValueEnd();
                        return;
                    }
                }
                else
                {
                    if ((*(long*)Current | 0x20002000200020L) == ('t' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48)))
                    {
                        Current += 4;
                        value = true;
                        SearchValueEnd();
                        return;
                    }
                    if ((uint)(*Current - '0') < 2)
                    {
                        value = *Current++ != '0';
                        SearchValueEnd();
                        return;
                    }
                }
                State = DeserializeStateEnum.NotBool;
            }
        }
        /// <summary>
        /// 是否非数字 NaN
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool isNaN()
        {
            return *valueStart == 'N' && *(int*)(valueStart + 1) == 'a' + ('N' << 16);
        }
        /// <summary>
        /// 是否 Infinity
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool isPositiveInfinity()
        {
            return *valueStart == 'I' && ((*(long*)valueStart ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(valueStart + 4) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0;
        }
        /// <summary>
        /// 是否 -Infinity
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool isNegativeInfinity()
        {
            return *valueStart == '-' && ((*(long*)(valueStart + 1) ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(valueStart + 5) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0;
        }
        ///// <summary>
        ///// 数字解析
        ///// </summary>
        ///// <param name="value">数据</param>
        //public void PrimitiveDeserialize(ref Int128 value)
        //{
        //    getValue();
        //    if (State == DeserializeStateEnum.Success)
        //    {
        //        if (valueSize != 0 && XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
        //        {
        //            getValueEnd();
        //            return;
        //        }
        //        State = DeserializeStateEnum.NotNumber;
        //    }
        //}
        ///// <summary>
        ///// 数字解析
        ///// </summary>
        ///// <param name="value">数据</param>
        //public void PrimitiveDeserialize(ref UInt128 value)
        //{
        //    getValue();
        //    if (State == DeserializeStateEnum.Success)
        //    {
        //        if (valueSize != 0 && XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
        //        {
        //            getValueEnd();
        //            return;
        //        }
        //        State = DeserializeStateEnum.NotNumber;
        //    }
        //}
        ///// <summary>
        ///// 数字解析
        ///// </summary>
        ///// <param name="value">数据</param>
        //public void PrimitiveDeserialize(ref Half value)
        //{
        //    getValue();
        //    if (State == DeserializeStateEnum.Success)
        //    {
        //        if (valueSize != 0)
        //        {
        //            switch (valueSize)
        //            {
        //                case 3:
        //                    if (isNaN())
        //                    {
        //                        value = Half.NaN;
        //                        return;
        //                    }
        //                    break;
        //                case 8:
        //                    if (isPositiveInfinity())
        //                    {
        //                        value = Half.PositiveInfinity;
        //                        return;
        //                    }
        //                    break;
        //                case 9:
        //                    if (isNegativeInfinity())
        //                    {
        //                        value = Half.NegativeInfinity;
        //                        return;
        //                    }
        //                    break;
        //            }
        //            if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
        //            {
        //                getValueEnd();
        //                return;
        //            }
        //        }
        //        State = DeserializeStateEnum.NotNumber;
        //    }
        //}
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref float value)
        {
            getValue();
            if (State == DeserializeStateEnum.Success)
            {
                if (valueSize != 0)
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = float.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = float.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = float.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref double value)
        {
            getValue();
            if (State == DeserializeStateEnum.Success)
            {
                if (valueSize != 0)
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = double.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = double.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = double.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = DeserializeStateEnum.NotNumber;
            }
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref char value)
        {
            getValue();
            if (valueSize == 1)
            {
                value = *valueStart;
                getValueEnd();
                return;
            }
            if ((IsCData | (*valueStart ^ '&')) == 0)
            {
                decodeChar(ref value);
                if (State == DeserializeStateEnum.Success) getValueEnd();
                return;
            }
            State = DeserializeStateEnum.NotChar;
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private uint deserializeHex2()
        {
            uint code = (uint)(*valueStart++ - '0'), number = (uint)(*valueStart++ - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint deserializeHex4()
        {
            uint code = (uint)(*valueStart++ - '0'), number = (uint)(*valueStart++ - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code <<= 12;
            code += (number << 8);
            if ((number = (uint)(*valueStart++ - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code += (number << 4);
            number = (uint)(*valueStart++ - '0');
            return code + (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        private void deSerialize(ref GuidCreator value)
        {
            value.Byte3 = (byte)deserializeHex2();
            value.Byte2 = (byte)deserializeHex2();
            value.Byte1 = (byte)deserializeHex2();
            value.Byte0 = (byte)deserializeHex2();
            if (*valueStart++ != '-')
            {
                State = DeserializeStateEnum.NotGuid;
                return;
            }
            value.Byte45 = (ushort)deserializeHex4();
            if (*valueStart++ != '-')
            {
                State = DeserializeStateEnum.NotGuid;
                return;
            }
            value.Byte67 = (ushort)deserializeHex4();
            if (*valueStart++ != '-')
            {
                State = DeserializeStateEnum.NotGuid;
                return;
            }
            value.Byte8 = (byte)deserializeHex2();
            value.Byte9 = (byte)deserializeHex2();
            if (*valueStart++ != '-')
            {
                State = DeserializeStateEnum.NotGuid;
                return;
            }
            value.Byte10 = (byte)deserializeHex2();
            value.Byte11 = (byte)deserializeHex2();
            value.Byte12 = (byte)deserializeHex2();
            value.Byte13 = (byte)deserializeHex2();
            value.Byte14 = (byte)deserializeHex2();
            value.Byte15 = (byte)deserializeHex2();
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref Guid value)
        {
            getValue();
            if (State == DeserializeStateEnum.Success)
            {
                if (valueSize == 36)
                {
                    GuidCreator guid = new GuidCreator();
                    deSerialize(ref guid);
                    value = guid.Value;
                    getValueEnd();
                }
                else State = DeserializeStateEnum.NotGuid;
            }
        }
        /// <summary>
        /// 查找CDATA数据结束位置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void searchCData2()
        {
            if (((*(int*)(Current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(Current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(Current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(Current + 8) ^ '[')) == 0)
            {
                Current += 9;
                SearchCDataValue();
            }
            else State = DeserializeStateEnum.NotFoundCDATAStart;
        }
        /// <summary>
        /// 字符串解码
        /// </summary>
        /// <param name="write"></param>
        /// <param name="writeEnd"></param>
        private void decodeString(char* write, char* writeEnd)
        {
            char decodeValue = (char)0;
            do
            {
                if (*valueStart == '&')
                {
                    decodeChar(ref decodeValue);
                    if (State != DeserializeStateEnum.Success) return;
                    *write = decodeValue;
                }
                else *write = *valueStart++;
            }
            while (++write != writeEnd);
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
#if NetStandard21
        public void XmlDeserialize(ref string? value)
#else
        public void XmlDeserialize(ref string value)
#endif
        {
            space();
            if (State != DeserializeStateEnum.Success) return;
            if (*Current == '<')
            {
                if (*(Current + 1) == '!')
                {
                    searchCData2();
                    if (State == DeserializeStateEnum.Success) value = valueSize == 0 ? string.Empty : new string(valueStart, 0, valueSize);
                }
                else value = null;
            }
            else
            {
                valueStart = Current;
                valueSize = 0;
                do
                {
                    if (*Current == '<')
                    {
                        int length = (int)(endSpace() - valueStart);
                        if (valueSize == 0) value = new string(valueStart, 0, length);
                        else
                        {
                            fixed (char* valueFixed = value = AutoCSer.Common.AllocateString(length - valueSize))
                            {
                                decodeString(valueFixed, valueFixed + value.Length);
                            }
                        }
                        return;
                    }
                    if (*Current == '&')
                    {
                        do
                        {
                            ++valueSize;
                            if (*++Current == ';') break;
                            if (*Current == '<')
                            {
                                State = DeserializeStateEnum.DecodeError;
                                return;
                            }
                        }
                        while (true);
                    }
                    ++Current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(XmlDeserializer serializer, ref string? value)
#else
        private static void primitiveDeserialize(XmlDeserializer serializer, ref string value)
#endif
        {
            serializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref SubString value)
        {
            space();
            if (State != DeserializeStateEnum.Success) return;
            if (*Current == '<')
            {
                if (*(Current + 1) == '!')
                {
                    searchCData2();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (valueSize == 0) value.Set(string.Empty, 0, 0);
                        else value.Set(text, (int)(valueStart - textFixed), valueSize);
                    }
                }
                else value = default(SubString);
            }
            else
            {
                valueStart = Current;
                valueSize = 0;
                do
                {
                    if (*Current == '<')
                    {
                        int length = (int)(endSpace() - valueStart);
                        if (valueSize == 0) value.Set(text, (int)(valueStart - textFixed), length);
                        else if (Config.IsTempString)
                        {
                            value.Set(text, (int)(valueStart - textFixed), length - valueSize);
                            while (*valueStart != '&') ++valueStart;
                            decodeString(valueStart, textFixed + value.Start + value.Length);
                        }
                        else
                        {
                            string decodeValue = AutoCSer.Common.AllocateString(length - valueSize);
                            fixed (char* valueFixed = decodeValue) decodeString(valueFixed, valueFixed + decodeValue.Length);
                            value.Set(decodeValue, 0, decodeValue.Length);
                        }
                        return;
                    }
                    if (*Current == '&')
                    {
                        do
                        {
                            ++valueSize;
                            if (*++Current == ';') break;
                            if (*Current == '<')
                            {
                                State = DeserializeStateEnum.DecodeError;
                                return;
                            }
                        }
                        while (true);
                    }
                    ++Current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref SubString value)
        {
            serializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="value">数据</param>
#if NetStandard21
        public void XmlDeserialize(ref object? value)
#else
        public void XmlDeserialize(ref object value)
#endif
        {
            XmlNode node = default(XmlNode);
            XmlDeserialize(ref node);
            if (State == DeserializeStateEnum.Success) value = node;
            //IgnoreValue();
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(XmlDeserializer serializer, ref object? value)
#else
        private static void primitiveDeserialize(XmlDeserializer serializer, ref object value)
#endif
        {
            serializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// XML节点解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref XmlNode value)
        {
            space();
            if (State != DeserializeStateEnum.Success) return;
            if (*Current == '<')
            {
                char code = *(Current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/')
                    {
                        value.SetString(string.Empty);
                        return;
                    }
                    if (code == '!')
                    {
                        searchCData2();
                        if (State == DeserializeStateEnum.Success) value.SetString(text, (int)(valueStart - textFixed), valueSize);
                        return;
                    }
                    State = DeserializeStateEnum.NotFoundTagStart;
                    return;
                }
                char* nameStart;
                LeftArray<KeyValue<SubString, XmlNode>> nodes = new LeftArray<KeyValue<SubString, XmlNode>>(0);
                var attributes = default(KeyValue<Range, Range>[]);
                int nameSize = 0;
                do
                {
                    nameStart = getName(ref nameSize);
                    if (State != DeserializeStateEnum.Success) return;
                    if (nameStart == null)
                    {
                        value.SetNode(ref nodes);
                        return;
                    }
                    nodes.PrepLength(1);
                    nodes.Array[nodes.Length].Key.Set(text, (int)(nameStart - textFixed), nameSize);
                    attributes = Config.IsAttribute && this.attributes.Length != 0 ? this.attributes.GetArray() : null;
                    if (isTagEnd == 0)
                    {
                        XmlDeserialize(ref nodes.Array[nodes.Length].Value);
                        if (State != DeserializeStateEnum.Success || CheckNameEnd(nameStart, nameSize) == 0) return;
                    }
                    if (attributes != null) nodes.Array[nodes.Length].Value.SetAttribute(text, attributes);
                    ++nodes.Length;
                }
                while (true);
            }
            else
            {
                valueStart = Current;
                value.Type = XmlNodeTypeEnum.String;
                do
                {
                    if (*Current == '<')
                    {
                        value.String.Set(text, (int)(valueStart - textFixed), (int)(endSpace() - valueStart));
                        if (Config.IsTempString && value.Type == XmlNodeTypeEnum.EncodeString) value.Type = XmlNodeTypeEnum.TempString;
                        return;
                    }
                    if (*Current == '&')
                    {
                        value.Type = XmlNodeTypeEnum.EncodeString;
                        while (*++Current != ';')
                        {
                            if (*Current == '<')
                            {
                                State = DeserializeStateEnum.DecodeError;
                                return;
                            }
                        }
                    }
                    ++Current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref XmlNode value)
        {
            serializer.XmlDeserialize(ref value);
        }
#if AOT
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        /// <param name="memberIndex"></param>
        internal delegate void MemberDeserializeDelegate<T>(XmlDeserializer deserializer, ref T value, int memberIndex);
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void XmlDeserialize<T>(ref T?[]? array)
        {
            TypeDeserializer<T>.Array(this, ref array);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value">目标数据</param>
        public void XmlDeserialize<KT, VT>(ref KeyValue<KT, VT> value)
        {
            BinarySerializeKeyValue<KT, VT> keyValue = new BinarySerializeKeyValue<KT, VT>(value.Key, value.Value);
            TypeDeserializer<BinarySerializeKeyValue<KT, VT>>.DefaultDeserializer(this, ref keyValue);
            value = new KeyValue<KT, VT>(keyValue.Key, keyValue.Value);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void KeyValue<KT, VT>(XmlDeserializer deserializer, ref KeyValue<KT, VT> value)
        {
            deserializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void XmlDeserialize<T>(ref T? value)
        {
            TypeDeserializer<T>.DefaultDeserializer(this, ref value);
        }
        /// <summary>
        /// 代码生成调用激活 AOT 反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void XmlDeserialize<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] T>()
        {
        }
        /// <summary>
        /// 不支持类型反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NotSupportMethod;
        /// <summary>
        /// 基类反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo BaseMethod;
        /// <summary>
        /// 自定义反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ICustomMethod;
        /// <summary>
        /// 数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo LeftArrayMethod;
        /// <summary>
        /// 数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ListArrayMethod;
        /// <summary>
        /// 数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ArrayMethod;
        /// <summary>
        /// 可空数据反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableMethod;
        /// <summary>
        /// 集合反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CollectionMethod;
        /// <summary>
        /// 键值对反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo KeyValueMethod;
        /// <summary>
        /// 键值对反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo KeyValuePairMethod;
        /// <summary>
        /// 枚举序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void DeserializeMethodName<T>(ref T value) { }
        /// <summary>
        /// 枚举序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void EnumXmlDeserializeMethodName<T>(ref T value) { }
        ///// <summary>
        ///// 反序列化模板
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //internal static void XmlDeserialize<T>() { }
        /// <summary>
        /// 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        internal static void ReflectionMethodName<T, VT>(JsonDeserializer deserializer, ref VT value) { }
#endif

        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(string xml, XmlDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(string xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult Deserialize<T>(string xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(string xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(xml)) return new DeserializeResult(DeserializeStateEnum.NullXml);
            XmlDeserializer xmlDeserializer = AutoCSer.Threading.LinkPool<XmlDeserializer>.Default.Pop() ?? new XmlDeserializer();
            try
            {
                return xmlDeserializer.deserialize(xml, ref value, config);
            }
            finally { xmlDeserializer.Free(); }
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(SubString xml, XmlDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(SubString xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(ref xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static DeserializeResult Deserialize<T>(SubString xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(SubString xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            return Deserialize(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(ref SubString xml, XmlDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(ref SubString xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(ref xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult Deserialize<T>(ref SubString xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(ref SubString xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(xml)) return new DeserializeResult(DeserializeStateEnum.NullXml);
            XmlDeserializer xmlDeserializer = AutoCSer.Threading.LinkPool<XmlDeserializer>.Default.Pop() ?? new XmlDeserializer();
            try
            {
                return xmlDeserializer.deserialize(ref xml, ref value, config);
            }
            finally { xmlDeserializer.Free(); }
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="length">XML 长度</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
#if NetStandard21
        internal static DeserializeResult UnsafeDeserialize<T>(char* xml, int length, ref T? value)
#else
        internal static DeserializeResult UnsafeDeserialize<T>(char* xml, int length, ref T value)
#endif
        {
            XmlDeserializer xmlDeserializer = AutoCSer.Threading.LinkPool<XmlDeserializer>.Default.Pop() ?? new XmlDeserializer();
            try
            {
                return xmlDeserializer.deserialize(xml, length, ref value);
            }
            finally { xmlDeserializer.Free(); }
        }

        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(string xml, XmlDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(string xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(string xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(string xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(xml)) return new DeserializeResult(DeserializeStateEnum.NullXml);
            XmlDeserializer xmlDeserializer = ThreadStaticDeserializer.Get().Deserializer;
            try
            {
                return xmlDeserializer.deserialize(xml, ref value, config);
            }
            finally { xmlDeserializer.freeThreadStatic(); }
        }
        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(SubString xml, XmlDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(SubString xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(ref xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(SubString xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(SubString xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            return ThreadStaticDeserialize(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(ref SubString xml, XmlDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(ref SubString xml, XmlDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(ref xml, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// XML 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(ref SubString xml, ref T? value, XmlDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(ref SubString xml, ref T value, XmlDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(xml)) return new DeserializeResult(DeserializeStateEnum.NullXml);
            XmlDeserializer xmlDeserializer = ThreadStaticDeserializer.Get().Deserializer;
            try
            {
                return xmlDeserializer.deserialize(ref xml, ref value, config);
            }
            finally { xmlDeserializer.freeThreadStatic(); }
        }

        /// <summary>
        /// 解析委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal delegate void DeserializeDelegate<T>(XmlDeserializer deserializer, ref T value);
        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Delegate> deserializeDelegates;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Delegate? GetDeserializeDelegate(Type type)
#else
        internal static Delegate GetDeserializeDelegate(Type type)
#endif
        {
            var deserializeDelegate = default(Delegate);
            return deserializeDelegates.TryGetValue(type, out deserializeDelegate) ? deserializeDelegate : null;
        }

        /// <summary>
        /// XML解析空格[ ,\t,\r,\n]
        /// </summary>
        private const byte spaceBit = 128;
        /// <summary>
        /// XML解析名称检测
        /// </summary>
        private const byte targetStartCheckBit = 64;
        /// <summary>
        /// XML解析属性名称查找
        /// </summary>
        private const byte attributeNameSearchBit = 32;
        /// <summary>
        /// XML序列化转换字符[ ,\t,\r,\n,&amp;,>,&lt;]
        /// </summary>
        internal const byte EncodeSpaceBit = 8;
        /// <summary>
        /// XML序列化转换字符[&amp;,>,&lt;]
        /// </summary>
        internal const byte EncodeBit = 4;
        /// <summary>
        /// 字符状态位
        /// </summary>
        internal static Pointer DeserializeBits;
        /// <summary>
        /// 字符 Decode 转码
        /// </summary>
        private static readonly AutoCSer.StateSearcher.AsciiSearcher decodeSearcher;

        static XmlDeserializer()
        {
            DeserializeBits = AutoCSer.Extensions.Memory.Unmanaged.GetXmlBits();
            byte* bits = DeserializeBits.Byte;
            AutoCSer.Common.Fill(DeserializeBits.ULong, 256 >> 3, ulong.MaxValue);
            bits['\t'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['\r'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['\n'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits[' '] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['/'] &= (targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['!'] &= targetStartCheckBit ^ 255;
            bits['<'] &= targetStartCheckBit ^ 255;
            bits['>'] &= (targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['='] &= attributeNameSearchBit ^ 255;
            #region 序列化
            bits['\t'] &= EncodeSpaceBit ^ 255;
            bits['\r'] &= EncodeSpaceBit ^ 255;
            bits['\n'] &= EncodeSpaceBit ^ 255;
            bits[' '] &= EncodeSpaceBit ^ 255;
            bits['&'] &= (EncodeSpaceBit | EncodeBit) ^ 255;
            bits['<'] &= (EncodeSpaceBit | EncodeBit) ^ 255;
            bits['>'] &= (EncodeSpaceBit | EncodeBit) ^ 255;
            #endregion

            KeyValue<string, int>[] chars = new KeyValue<string, int>[]
            {
                new KeyValue<string, int>("AElig;", 198)
                , new KeyValue<string, int>("Aacute;", 193)
                , new KeyValue<string, int>("Acirc;", 194)
                , new KeyValue<string, int>("Agrave;", 192)
                , new KeyValue<string, int>("Alpha;", 913)
                , new KeyValue<string, int>("Aring;", 197)
                , new KeyValue<string, int>("Atilde;", 195)
                , new KeyValue<string, int>("Auml;", 196)
                , new KeyValue<string, int>("Beta;", 914)
                , new KeyValue<string, int>("Ccedil;", 199)
                , new KeyValue<string, int>("Chi;", 935)
                , new KeyValue<string, int>("Dagger;", 8225)
                , new KeyValue<string, int>("Delta;", 916)
                , new KeyValue<string, int>("ETH;", 208)
                , new KeyValue<string, int>("Eacute;", 201)
                , new KeyValue<string, int>("Ecirc;", 202)
                , new KeyValue<string, int>("Egrave;", 200)
                , new KeyValue<string, int>("Epsilon;", 917)
                , new KeyValue<string, int>("Eta;", 919)
                , new KeyValue<string, int>("Euml;", 203)
                , new KeyValue<string, int>("Gamma;", 915)
                , new KeyValue<string, int>("Iacute;", 205)
                , new KeyValue<string, int>("Icirc;", 206)
                , new KeyValue<string, int>("Igrave;", 204)
                , new KeyValue<string, int>("Iota;", 921)
                , new KeyValue<string, int>("Iuml;", 207)
                , new KeyValue<string, int>("Kappa;", 922)
                , new KeyValue<string, int>("Lambda;", 923)
                , new KeyValue<string, int>("Mu;", 924)
                , new KeyValue<string, int>("Ntilde;", 209)
                , new KeyValue<string, int>("Nu;", 925)
                , new KeyValue<string, int>("OElig;", 338)
                , new KeyValue<string, int>("Oacute;", 211)
                , new KeyValue<string, int>("Ocirc;", 212)
                , new KeyValue<string, int>("Ograve;", 210)
                , new KeyValue<string, int>("Omega;", 937)
                , new KeyValue<string, int>("Omicron;", 927)
                , new KeyValue<string, int>("Oslash;", 216)
                , new KeyValue<string, int>("Otilde;", 213)
                , new KeyValue<string, int>("Ouml;", 214)
                , new KeyValue<string, int>("Phi;", 934)
                , new KeyValue<string, int>("Pi;", 928)
                , new KeyValue<string, int>("Prime;", 8243)
                , new KeyValue<string, int>("Psi;", 936)
                , new KeyValue<string, int>("Rho;", 929)
                , new KeyValue<string, int>("Scaron;", 352)
                , new KeyValue<string, int>("Sigma;", 931)
                , new KeyValue<string, int>("THORN;", 222)
                , new KeyValue<string, int>("Tau;", 932)
                , new KeyValue<string, int>("Theta;", 920)
                , new KeyValue<string, int>("Uacute;", 218)
                , new KeyValue<string, int>("Ucirc;", 219)
                , new KeyValue<string, int>("Ugrave;", 217)
                , new KeyValue<string, int>("Upsilon;", 933)
                , new KeyValue<string, int>("Uuml;", 220)
                , new KeyValue<string, int>("Xi;", 926)
                , new KeyValue<string, int>("Yacute;", 221)
                , new KeyValue<string, int>("Yuml;", 376)
                , new KeyValue<string, int>("Zeta;", 918)
                , new KeyValue<string, int>("aacute;", 225)
                , new KeyValue<string, int>("acirc;", 226)
                , new KeyValue<string, int>("acute;", 180)
                , new KeyValue<string, int>("aelig;", 230)
                , new KeyValue<string, int>("agrave;", 224)
                , new KeyValue<string, int>("alefsym;", 8501)
                , new KeyValue<string, int>("alpha;", 945)
                , new KeyValue<string, int>("amp;", 38)
                , new KeyValue<string, int>("and;", 8743)
                , new KeyValue<string, int>("ang;", 8736)
                , new KeyValue<string, int>("aring;", 229)
                , new KeyValue<string, int>("asymp;", 8776)
                , new KeyValue<string, int>("atilde;", 227)
                , new KeyValue<string, int>("auml;", 228)
                , new KeyValue<string, int>("bdquo;", 8222)
                , new KeyValue<string, int>("beta;", 946)
                , new KeyValue<string, int>("brvbar;", 166)
                , new KeyValue<string, int>("bull;", 8226)
                , new KeyValue<string, int>("cap;", 8745)
                , new KeyValue<string, int>("ccedil;", 231)
                , new KeyValue<string, int>("cedil;", 184)
                , new KeyValue<string, int>("cent;", 162)
                , new KeyValue<string, int>("chi;", 967)
                , new KeyValue<string, int>("circ;", 710)
                , new KeyValue<string, int>("clubs;", 9827)
                , new KeyValue<string, int>("cong;", 8773)
                , new KeyValue<string, int>("copy;", 169)
                , new KeyValue<string, int>("crarr;", 8629)
                , new KeyValue<string, int>("cup;", 8746)
                , new KeyValue<string, int>("curren;", 164)
                , new KeyValue<string, int>("dArr;", 8659)
                , new KeyValue<string, int>("dagger;", 8224)
                , new KeyValue<string, int>("darr;", 8595)
                , new KeyValue<string, int>("deg;", 176)
                , new KeyValue<string, int>("delta;", 948)
                , new KeyValue<string, int>("diams;", 9830)
                , new KeyValue<string, int>("divide;", 247)
                , new KeyValue<string, int>("eacute;", 233)
                , new KeyValue<string, int>("ecirc;", 234)
                , new KeyValue<string, int>("egrave;", 232)
                , new KeyValue<string, int>("empty;", 8709)
                , new KeyValue<string, int>("emsp;", 8195)
                , new KeyValue<string, int>("ensp;", 8194)
                , new KeyValue<string, int>("epsilon;", 949)
                , new KeyValue<string, int>("equiv;", 8801)
                , new KeyValue<string, int>("eta;", 951)
                , new KeyValue<string, int>("eth;", 240)
                , new KeyValue<string, int>("euml;", 235)
                , new KeyValue<string, int>("euro;", 8364)
                , new KeyValue<string, int>("exist;", 8707)
                , new KeyValue<string, int>("fnof;", 402)
                , new KeyValue<string, int>("forall;", 8704)
                , new KeyValue<string, int>("frac12;", 189)
                , new KeyValue<string, int>("frac14;", 188)
                , new KeyValue<string, int>("frac34;", 190)
                , new KeyValue<string, int>("frasl;", 8260)
                , new KeyValue<string, int>("gamma;", 947)
                , new KeyValue<string, int>("ge;", 8805)
                , new KeyValue<string, int>("gt;", 62)
                , new KeyValue<string, int>("hArr;", 8660)
                , new KeyValue<string, int>("harr;", 8596)
                , new KeyValue<string, int>("hearts;", 9829)
                , new KeyValue<string, int>("hellip;", 8230)
                , new KeyValue<string, int>("iacute;", 237)
                , new KeyValue<string, int>("icirc;", 238)
                , new KeyValue<string, int>("iexcl;", 161)
                , new KeyValue<string, int>("igrave;", 236)
                , new KeyValue<string, int>("image;", 8465)
                , new KeyValue<string, int>("infin;", 8734)
                , new KeyValue<string, int>("int;", 8747)
                , new KeyValue<string, int>("iota;", 953)
                , new KeyValue<string, int>("iquest;", 191)
                , new KeyValue<string, int>("isin;", 8712)
                , new KeyValue<string, int>("iuml;", 239)
                , new KeyValue<string, int>("kappa;", 954)
                , new KeyValue<string, int>("lArr;", 8656)
                , new KeyValue<string, int>("lambda;", 955)
                , new KeyValue<string, int>("lang;", 9001)
                , new KeyValue<string, int>("laquo;", 171)
                , new KeyValue<string, int>("larr;", 8592)
                , new KeyValue<string, int>("lceil;", 8968)
                , new KeyValue<string, int>("ldquo;", 8220)
                , new KeyValue<string, int>("le;", 8804)
                , new KeyValue<string, int>("lfloor;", 8970)
                , new KeyValue<string, int>("lowast;", 8727)
                , new KeyValue<string, int>("loz;", 9674)
                , new KeyValue<string, int>("lrm;", 8206)
                , new KeyValue<string, int>("lsaquo;", 8249)
                , new KeyValue<string, int>("lsquo;", 8216)
                , new KeyValue<string, int>("lt;", 60)
                , new KeyValue<string, int>("macr;", 175)
                , new KeyValue<string, int>("mdash;", 8212)
                , new KeyValue<string, int>("micro;", 181)
                , new KeyValue<string, int>("middot;", 183)
                , new KeyValue<string, int>("minus;", 8722)
                , new KeyValue<string, int>("mu;", 956)
                , new KeyValue<string, int>("nabla;", 8711)
                , new KeyValue<string, int>("nbsp;", 160)
                , new KeyValue<string, int>("ndash;", 8211)
                , new KeyValue<string, int>("ne;", 8800)
                , new KeyValue<string, int>("ni;", 8715)
                , new KeyValue<string, int>("not;", 172)
                , new KeyValue<string, int>("notin;", 8713)
                , new KeyValue<string, int>("nsub;", 8836)
                , new KeyValue<string, int>("ntilde;", 241)
                , new KeyValue<string, int>("nu;", 957)
                , new KeyValue<string, int>("oacute;", 243)
                , new KeyValue<string, int>("ocirc;", 244)
                , new KeyValue<string, int>("oelig;", 339)
                , new KeyValue<string, int>("ograve;", 242)
                , new KeyValue<string, int>("oline;", 8254)
                , new KeyValue<string, int>("omega;", 969)
                , new KeyValue<string, int>("omicron;", 959)
                , new KeyValue<string, int>("oplus;", 8853)
                , new KeyValue<string, int>("or;", 8744)
                , new KeyValue<string, int>("ordf;", 170)
                , new KeyValue<string, int>("ordm;", 186)
                , new KeyValue<string, int>("oslash;", 248)
                , new KeyValue<string, int>("otilde;", 245)
                , new KeyValue<string, int>("otimes;", 8855)
                , new KeyValue<string, int>("ouml;", 246)
                , new KeyValue<string, int>("para;", 182)
                , new KeyValue<string, int>("part;", 8706)
                , new KeyValue<string, int>("permil;", 8240)
                , new KeyValue<string, int>("perp;", 8869)
                , new KeyValue<string, int>("phi;", 966)
                , new KeyValue<string, int>("pi;", 960)
                , new KeyValue<string, int>("piv;", 982)
                , new KeyValue<string, int>("plusmn;", 177)
                , new KeyValue<string, int>("pound;", 163)
                , new KeyValue<string, int>("prime;", 8242)
                , new KeyValue<string, int>("prod;", 8719)
                , new KeyValue<string, int>("prop;", 8733)
                , new KeyValue<string, int>("psi;", 968)
                , new KeyValue<string, int>("quot;", 34)
                , new KeyValue<string, int>("rArr;", 8658)
                , new KeyValue<string, int>("radic;", 8730)
                , new KeyValue<string, int>("rang;", 9002)
                , new KeyValue<string, int>("raquo;", 187)
                , new KeyValue<string, int>("rarr;", 8594)
                , new KeyValue<string, int>("rceil;", 8969)
                , new KeyValue<string, int>("rdquo;", 8221)
                , new KeyValue<string, int>("real;", 8476)
                , new KeyValue<string, int>("reg;", 174)
                , new KeyValue<string, int>("rfloor;", 8971)
                , new KeyValue<string, int>("rho;", 961)
                , new KeyValue<string, int>("rlm;", 8207)
                , new KeyValue<string, int>("rsaquo;", 8250)
                , new KeyValue<string, int>("rsquo;", 8217)
                , new KeyValue<string, int>("sbquo;", 8218)
                , new KeyValue<string, int>("scaron;", 353)
                , new KeyValue<string, int>("sdot;", 8901)
                , new KeyValue<string, int>("sect;", 167)
                , new KeyValue<string, int>("shy;", 173)
                , new KeyValue<string, int>("sigma;", 963)
                , new KeyValue<string, int>("sigmaf;", 962)
                , new KeyValue<string, int>("sim;", 8764)
                , new KeyValue<string, int>("spades;", 9824)
                , new KeyValue<string, int>("sub;", 8834)
                , new KeyValue<string, int>("sube;", 8838)
                , new KeyValue<string, int>("sum;", 8721)
                , new KeyValue<string, int>("sup1;", 185)
                , new KeyValue<string, int>("sup2;", 178)
                , new KeyValue<string, int>("sup3;", 179)
                , new KeyValue<string, int>("sup;", 8835)
                , new KeyValue<string, int>("supe;", 8839)
                , new KeyValue<string, int>("szlig;", 223)
                , new KeyValue<string, int>("tau;", 964)
                , new KeyValue<string, int>("there4;", 8756)
                , new KeyValue<string, int>("theta;", 952)
                , new KeyValue<string, int>("thetasym;", 977)
                , new KeyValue<string, int>("thinsp;", 8201)
                , new KeyValue<string, int>("thorn;", 254)
                , new KeyValue<string, int>("tilde;", 732)
                , new KeyValue<string, int>("times;", 215)
                , new KeyValue<string, int>("trade;", 8482)
                , new KeyValue<string, int>("uArr;", 8657)
                , new KeyValue<string, int>("uacute;", 250)
                , new KeyValue<string, int>("uarr;", 8593)
                , new KeyValue<string, int>("ucirc;", 251)
                , new KeyValue<string, int>("ugrave;", 249)
                , new KeyValue<string, int>("uml;", 168)
                , new KeyValue<string, int>("upsih;", 978)
                , new KeyValue<string, int>("upsilon;", 965)
                , new KeyValue<string, int>("uuml;", 252)
                , new KeyValue<string, int>("weierp;", 8472)
                , new KeyValue<string, int>("xi;", 958)
                , new KeyValue<string, int>("yacute;", 253)
                , new KeyValue<string, int>("yen;", 165)
                , new KeyValue<string, int>("yuml;", 255)
                , new KeyValue<string, int>("zeta;", 950)
                , new KeyValue<string, int>("zwj;", 8205)
                , new KeyValue<string, int>("zwnj;", 8204)
            };
            decodeSearcher = new AutoCSer.StateSearcher.AsciiSearcher(new AutoCSer.StateSearcher.AsciiBuilder(chars, true).Data);

            deserializeDelegates = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, Delegate>();
            deserializeDelegates.Add(typeof(bool), (DeserializeDelegate<bool>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(bool?), (DeserializeDelegate<bool?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(byte), (DeserializeDelegate<byte>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(byte?), (DeserializeDelegate<byte?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(sbyte), (DeserializeDelegate<sbyte>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(sbyte?), (DeserializeDelegate<sbyte?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(short), (DeserializeDelegate<short>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(short?), (DeserializeDelegate<short?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ushort), (DeserializeDelegate<ushort>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ushort?), (DeserializeDelegate<ushort?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(int), (DeserializeDelegate<int>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(int?), (DeserializeDelegate<int?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(uint), (DeserializeDelegate<uint>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(uint?), (DeserializeDelegate<uint?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(long), (DeserializeDelegate<long>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(long?), (DeserializeDelegate<long?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ulong), (DeserializeDelegate<ulong>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ulong?), (DeserializeDelegate<ulong?>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(Int128), (DeserializeDelegate<Int128>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(Int128?), (DeserializeDelegate<Int128?>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(UInt128), (DeserializeDelegate<UInt128>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(UInt128?), (DeserializeDelegate<UInt128?>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(Half), (DeserializeDelegate<Half>)primitiveDeserialize);
            //deserializeDelegates.Add(typeof(Half?), (DeserializeDelegate<Half?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(float), (DeserializeDelegate<float>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(float?), (DeserializeDelegate<float?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(double), (DeserializeDelegate<double>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(double?), (DeserializeDelegate<double?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(decimal), (DeserializeDelegate<decimal>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(decimal?), (DeserializeDelegate<decimal?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(char), (DeserializeDelegate<char>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(char?), (DeserializeDelegate<char?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(DateTime), (DeserializeDelegate<DateTime>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(DateTime?), (DeserializeDelegate<DateTime?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(TimeSpan), (DeserializeDelegate<TimeSpan>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(TimeSpan?), (DeserializeDelegate<TimeSpan?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(Guid), (DeserializeDelegate<Guid>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(Guid?), (DeserializeDelegate<Guid?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(SubString), (DeserializeDelegate<SubString>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(XmlNode), (DeserializeDelegate<XmlNode>)primitiveDeserialize);
#if NetStandard21
            deserializeDelegates.Add(typeof(string), (DeserializeDelegate<string?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(object), (DeserializeDelegate<object?>)primitiveDeserialize);
#else
            deserializeDelegates.Add(typeof(string), (DeserializeDelegate<string>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(object), (DeserializeDelegate<object>)primitiveDeserialize);
#endif
#if AOT
            foreach (System.Reflection.MethodInfo method in typeof(XmlDeserializer).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                switch (method.Name.Length)
                {
                    case 4:
                        if (method.Name == nameof(Base)) BaseMethod = method;
                        break;
                    case 5:
                        if (method.Name == nameof(Array)) ArrayMethod = method;
                        break;
                    case 7:
                        if (method.Name == nameof(EnumInt)) EnumIntMethod = method;
                        else if (method.Name == nameof(ICustom)) ICustomMethod = method;
                        break;
                    case 8:
                        if (method.Name == nameof(EnumByte)) EnumByteMethod = method;
                        else if (method.Name == nameof(EnumLong)) EnumLongMethod = method;
                        else if (method.Name == nameof(EnumUInt)) EnumUIntMethod = method;
                        else if (method.Name == nameof(Nullable)) NullableMethod = method;
                        else if (method.Name == nameof(KeyValue)) KeyValueMethod = method;
                        break;
                    case 9:
                        if (method.Name == nameof(EnumULong)) EnumULongMethod = method;
                        else if (method.Name == nameof(LeftArray)) LeftArrayMethod = method;
                        else if (method.Name == nameof(EnumShort)) EnumShortMethod = method;
                        else if (method.Name == nameof(EnumSByte)) EnumSByteMethod = method;
                        else if (method.Name == nameof(ListArray)) ListArrayMethod = method;
                        break;
                    case 10:
                        if (method.Name == nameof(Collection)) CollectionMethod = method;
                        else if (method.Name == nameof(EnumUShort)) EnumUShortMethod = method;
                        else if (method.Name == nameof(NotSupport)) NotSupportMethod = method;
                        break;
                    case 12:
                        if (method.Name == nameof(EnumFlagsInt)) EnumFlagsIntMethod = method;
                        else if (method.Name == nameof(KeyValuePair)) KeyValuePairMethod = method;
                        break;
                    case 13:
                        if (method.Name == nameof(EnumFlagsUInt)) EnumFlagsUIntMethod = method;
                        else if (method.Name == nameof(EnumFlagsLong)) EnumFlagsLongMethod = method;
                        else if (method.Name == nameof(EnumFlagsByte)) EnumFlagsByteMethod = method;
                        break;
                    case 14:
                        if (method.Name == nameof(EnumFlagsULong)) EnumFlagsULongMethod = method;
                        else if (method.Name == nameof(EnumFlagsSByte)) EnumFlagsSByteMethod = method;
                        else if (method.Name == nameof(EnumFlagsShort)) EnumFlagsShortMethod = method;
                        break;
                    case 15:
                         if (method.Name == nameof(EnumFlagsUShort)) EnumFlagsUShortMethod = method;
                        break;
                }
            }
#else
            foreach (Delegate deserializeDelegate in AutoCSer.XmlSerializer.CustomConfig.PrimitiveDeserializeDelegates)
            {
                var type = AutoCSer.Common.CheckDeserializeType(typeof(XmlDeserializer), deserializeDelegate);
                if (type != null) deserializeDelegates[type] = deserializeDelegate;
            }
#endif
        }
    }
}
