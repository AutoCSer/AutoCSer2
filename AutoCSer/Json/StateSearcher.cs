using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 名称状态查找器
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
        internal StateSearcher(Pointer data)
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
        /// 获取名称索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="isQuote">名称是否带引号</param>
        /// <returns>名称索引,失败返回-1</returns>
        internal int SearchName(JsonDeserializer jsonDeserializer, out bool isQuote)
        {
            char value = jsonDeserializer.GetFirstName();
            if (State != null)
            {
                if (jsonDeserializer.Quote != 0)
                {
                    isQuote = true;
                    return searchString(jsonDeserializer, value);
                }
                isQuote = false;
                if (jsonDeserializer.State == DeserializeStateEnum.Success)
                {
                    byte* currentState = State;
                    do
                    {
                        char* prefix = (char*)(currentState + *(int*)currentState);
                        if (*prefix != 0)
                        {
                            if (value == *prefix)
                            {
                                while (*++prefix != 0)
                                {
                                    if (jsonDeserializer.GetNextName() != *prefix) return -1;
                                }
                                value = (char)jsonDeserializer.GetNextName();
                            }
                            else break;
                        }
                        if (value == 0) return jsonDeserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                        if (*(int*)(currentState + sizeof(int)) != 0)
                        {
                            int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
                            byte* table = currentState + *(int*)(currentState + sizeof(int));
                            if (tableType == 0)
                            {
                                if ((index = *(table + index)) != 0) currentState = State + index * 3 * sizeof(int);
                                else break;
                            }
                            else if (tableType == 1)
                            {
                                if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) != 0) currentState = State + index * 3 * sizeof(int);
                                else break;
                            }
                            else
                            {
                                if ((index = *(int*)(table + index * sizeof(int))) != 0) currentState = State + index;
                                else break;
                            }
                            value = (char)jsonDeserializer.GetNextName();
                        }
                        else break;
                    }
                    while (true);
                }
            }
            else isQuote = jsonDeserializer.Quote != 0;
            return -1;
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SearchString(JsonDeserializer jsonDeserializer)
        {
            char value = jsonDeserializer.SearchQuote();
            return jsonDeserializer.State == DeserializeStateEnum.Success && State != null ? searchString(jsonDeserializer, value) : -1;
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">第一个字符</param>
        /// <returns>目标索引,null返回-1</returns>
        internal int searchString(JsonDeserializer jsonDeserializer, char value)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (jsonDeserializer.NextStringChar() != *prefix) return -1;
                    }
                    value = jsonDeserializer.NextStringChar();
                }
                if (value == 0) return jsonDeserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
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
                value = jsonDeserializer.NextStringChar();
            }
            while (true);
        }
        /// <summary>
        /// 获取特殊字符索引值
        /// </summary>
        /// <param name="value">特殊字符</param>
        /// <returns>索引值,匹配失败返回0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int getCharIndex(char value)
        {
            if (charStart != charEnd)
            {
                char* current = AutoCSer.StateSearcher.CharSearcher.GetCharIndex((char*)charStart, (char*)charEnd, value);
                if (current != null) return charIndex + (int)(current - (char*)charStart);
            }
            return 0;
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SearchFlagEnum(JsonDeserializer jsonDeserializer)
        {
            return flagEnum(jsonDeserializer, jsonDeserializer.SearchEnumQuote());
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">当前字符</param>
        /// <returns>目标索引,null返回-1</returns>
        private int flagEnum(JsonDeserializer jsonDeserializer, char value)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (jsonDeserializer.NextEnumChar() != *prefix) return -1;
                    }
                    value = jsonDeserializer.NextEnumChar();
                }
                if (value == 0 || value == ',') return jsonDeserializer.State == DeserializeStateEnum.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
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
                value = jsonDeserializer.NextEnumChar();
            }
            while (true);
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int NextFlagEnum(JsonDeserializer jsonDeserializer)
        {
            return flagEnum(jsonDeserializer, jsonDeserializer.SearchNextEnum());
        }
    }
}
