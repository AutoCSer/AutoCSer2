﻿using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举状态查找器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct StateSearcher
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        internal readonly byte* State;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private readonly byte* charsAscii;
        /// <summary>
        /// 特殊字符串查找表
        /// </summary>
        private readonly byte* charStart;
        /// <summary>
        /// 特殊字符串查找表结束位置
        /// </summary>
        private readonly byte* charEnd;
        /// <summary>
        /// 特殊字符起始值
        /// </summary>
        private readonly int charIndex;
        /// <summary>
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private readonly byte tableType;
        /// <summary>
        /// 名称查找器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        internal StateSearcher(ref AutoCSer.Memory.Pointer data)
        {
            if (data.Data == null)
            {
                State = charsAscii = charStart = charEnd = null;
                charIndex = 0;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                State = data.Byte + sizeof(int);
                charsAscii = State + stateCount * 3 * sizeof(int);
                charStart = charsAscii + 128 * sizeof(ushort);
                charIndex = *(ushort*)charStart;
                charStart += sizeof(ushort) * 2;
                charEnd = charStart + *(ushort*)(charStart - sizeof(ushort)) * sizeof(ushort);
                if (stateCount < 256) tableType = 0;
                else if (stateCount < 65536) tableType = 1;
                else tableType = 2;
            }
        }
        /// <summary>
        /// 获取特殊字符索引值
        /// </summary>
        /// <param name="value">特殊字符</param>
        /// <returns>索引值,匹配失败返回0</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int getCharIndex(char value)
        {
            if (charStart == charEnd) return 0;
            char* current = AutoCSer.StateSearcher.CharSearcher.GetCharIndex((char*)charStart, (char*)charEnd, value);
            return current == null ? 0 : (charIndex + (int)(current - (char*)charStart));
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        internal int SearchEnum(XmlDeserializer deserializer)
        {
            if (State != null)
            {
                if (deserializer.IsCData == 0)
                {
                    int index = searchEnumOnly(deserializer);
                    if (deserializer.State == DeserializeStateEnum.Success)
                    {
                        deserializer.SearchValueEnd();
                        return index;
                    }
                }
                else
                {
                    deserializer.SearchCDataValue();
                    if (deserializer.State == DeserializeStateEnum.Success) return searchCDataEnum(deserializer);
                }
            }
            return -1;
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        internal int searchEnumOnly(XmlDeserializer deserializer)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (deserializer.NextEnumChar() != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (deserializer.NextEnumChar() != *prefix) return -1;
                    }
                }
                char value = deserializer.NextEnumChar();
                if (value == 0) return deserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = State + index;
                }
            }
            while (true);
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        internal int searchCDataEnum(XmlDeserializer deserializer)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (deserializer.NextCDataEnumChar() != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (deserializer.NextCDataEnumChar() != *prefix) return -1;
                    }
                }
                char value = deserializer.NextCDataEnumChar();
                if (value == 0) return deserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = State + index;
                }
            }
            while (true);
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SearchFlagEnum(XmlDeserializer deserializer)
        {
            if (deserializer.IsCData == 0) return searchFlagEnum(deserializer);
            deserializer.SearchCDataValue();
            return deserializer.State == DeserializeStateEnum.Success ? searchCDataFlagEnum(deserializer) : -1;
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        private int searchFlagEnum(XmlDeserializer deserializer)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (deserializer.NextEnumChar() != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (deserializer.NextEnumChar() != *prefix) return -1;
                    }
                }
                char value = deserializer.NextEnumChar();
                if (value == 0 || value == ',') return deserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = State + index;
                }
            }
            while (true);
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        private int searchCDataFlagEnum(XmlDeserializer deserializer)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (deserializer.NextCDataEnumChar() != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (deserializer.NextCDataEnumChar() != *prefix) return -1;
                    }
                }
                char value = deserializer.NextCDataEnumChar();
                if (value == 0 || value == ',') return deserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = State + index;
                }
            }
            while (true);
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int NextFlagEnum(XmlDeserializer deserializer)
        {
            return deserializer.IsCData == 0 ? searchFlagEnum(deserializer) : searchCDataFlagEnum(deserializer);
        }
    }
}