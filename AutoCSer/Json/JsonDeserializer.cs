using AutoCSer.Extensions;
using AutoCSer.Json;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer : TextDeserializer<JsonDeserializer>
    {
        /// <summary>
        /// 字符串 null
        /// </summary>
        internal const long NullStringValue = 'n' | ('u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);
        /// <summary>
        /// 默认解析所有成员
        /// </summary>
        internal static readonly JsonSerializeAttribute AllMemberAttribute = JsonSerializer.ConfigurationAttribute ?? new JsonSerializeAttribute { Filter = Metadata.MemberFiltersEnum.Instance, IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly JsonDeserializeConfig DefaultConfig = AutoCSer.Configuration.Common.Get<JsonDeserializeConfig>()?.Value ?? new JsonDeserializeConfig();

        /// <summary>
        /// 转义字符集合
        /// </summary>
        private readonly char* escapeChars = escapeCharData.Char;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal JsonDeserializeConfig Config;
        /// <summary>
        /// 最后一个字符
        /// </summary>
        private char endChar;
        /// <summary>
        /// 当前字符串引号
        /// </summary>
        internal char Quote;
        /// <summary>
        /// 解析状态
        /// </summary>
        internal DeserializeStateEnum State;
        /// <summary>
        /// 是否二进制混杂模式
        /// </summary>
        internal bool IsBinaryMix;
        /// <summary>
        /// 是否以空格字符结束
        /// </summary>
        private bool isEndSpace;
        /// <summary>
        /// 是否以10进制数字字符结束
        /// </summary>
        private bool isEndDigital;
        /// <summary>
        /// 是否以16进制数字字符结束
        /// </summary>
        private bool isEndHex;
        /// <summary>
        /// 是否以数字字符结束
        /// </summary>
        private bool isEndNumber;
        /// <summary>
        /// Whether it is necessary to call AutoCSer.Common.Config.CheckRemoteType to check the validity of the remote type
        /// 是否需要调用 AutoCSer.Common.Config.CheckRemoteType 检查远程类型的合法性
        /// </summary>
        private bool isCheckRemoteType = true;
        /// <summary>
        /// JSON 解析器
        /// </summary>
        internal JsonDeserializer() : base(DeserializeBits.Byte)
        {
            Config = DefaultConfig;
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(ref SubString json, ref T? value, JsonDeserializeConfig? config)
#else
        private DeserializeResult deserialize<T>(ref SubString json, ref T value, JsonDeserializeConfig config)
#endif
        {
            fixed (char* jsonFixed = (this.text = json.String.notNull()))
            {
                Current = (this.textFixed = jsonFixed) + json.Start;
                this.Config = config ?? DefaultConfig;
                end = Current + json.Length;
                deserialize(ref value);
                if (State == DeserializeStateEnum.Success)
                {
                    DeserializeResult result = new DeserializeResult(MemberMap);
                    MemberMap = null;
                    return result;
                }
                return new DeserializeResult(State, ref json, (int)(Current - jsonFixed) - json.Start, customError);
            }
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(string json, ref T? value, JsonDeserializeConfig? config)
#else
        private DeserializeResult deserialize<T>(string json, ref T value, JsonDeserializeConfig config)
#endif
        {
            fixed (char* jsonFixed = (this.text = json))
            {
                Current = this.textFixed = jsonFixed;
                this.Config = config ?? DefaultConfig;
                end = jsonFixed + json.Length;
                deserialize(ref value);
                if (State == DeserializeStateEnum.Success)
                {
                    DeserializeResult result = new DeserializeResult(MemberMap);
                    MemberMap = null;
                    return result;
                }
                return new DeserializeResult(State, (int)(Current - jsonFixed), json, customError);
            }
        }
        /// <summary>
        /// Json解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="length">Json长度</param>
        /// <param name="value">Target data</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        internal DeserializeResult Deserialize<T>(char* json, int length, ref T? value)
#else
        internal DeserializeResult Deserialize<T>(char* json, int length, ref T value)
#endif
        {
            end = (textFixed = Current = json) + length;
            deserialize(ref value);
            if (State == DeserializeStateEnum.Success)
            {
                DeserializeResult result = new DeserializeResult(MemberMap);
                MemberMap = null;
                return result;
            }
            return new DeserializeResult(State, (int)(Current - textFixed), Config.IsErrorNewString ? new string(json, 0, length) : null, customError);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">Target data</param>
        /// <returns>解析状态</returns>
#if NetStandard21
        private void deserialize<T>(ref T? value)
#else
        private void deserialize<T>(ref T value)
#endif
        {
            if (endChar != *(end - 1))
            {
                if (((endChar = *(end - 1)) & 0xff00) == 0)
                {
                    isEndSpace = (bits[(byte)endChar] & DeserializeSpaceBit) == 0;
                    if ((uint)(endChar - '0') < 10) isEndDigital = isEndHex = isEndNumber = true;
                    else
                    {
                        isEndDigital = false;
                        if ((uint)((endChar | 0x20) - 'a') < 6) isEndHex = isEndNumber = true;
                        else
                        {
                            isEndHex = false;
                            isEndNumber = (bits[(byte)endChar] & DeserializeNumberBit) == 0;
                        }
                    }
                }
                else isEndSpace = isEndDigital = isEndHex = isEndNumber = false;
            }
            State = DeserializeStateEnum.Success;
            TypeDeserializer<T>.DefaultDeserializer(this, ref value);
            if (State == DeserializeStateEnum.Success)
            {
                switch(end - Current)
                {
                    case 0: return;
                    case 2:
                        if (*(char*)Current == ' ') return;
                        break;
                }
                if (!Config.IsEndSpace) return;
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current == end) return;
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
        }
//        /// <summary>
//        /// 命令服务反序列化
//        /// </summary>
//        /// <typeparam name="T">Data type</typeparam>
//        /// <param name="data">Data</param>
//        /// <param name="value">Target object</param>
//        /// <returns>Return false on failure</returns>
//#if NetStandard21
//        internal bool DeserializeCommandServer<T>(ref SubArray<byte> data, ref T? value)
//#else
//        internal bool DeserializeCommandServer<T>(ref SubArray<byte> data, ref T value)
//#endif
//        {
//            if ((data.Length & 1) == 0)
//            {
//                fixed (byte* dataFixed = data.GetFixedBuffer())
//                {
//                    byte* start = dataFixed + data.Start;
//                    textFixed = Current = (char*)start;
//                    end = (char*)(start + data.Length);
//                    MemberMap = null;
//                    deserialize(ref value);
//                    return State == DeserializeStateEnum.Success;
//                }
//            }
//            return false;
//        }
//        /// <summary>
//        /// 命令服务反序列化
//        /// </summary>
//        /// <typeparam name="T">Data type</typeparam>
//        /// <param name="deserializer"></param>
//        /// <param name="value">Target object</param>
//        /// <returns>Return false on failure</returns>
//#if NetStandard21
//        internal bool DeserializeCommandServer<T>(BinaryDeserializer deserializer, ref T? value)
//#else
//        internal bool DeserializeCommandServer<T>(BinaryDeserializer deserializer, ref T value)
//#endif
//        {
//            Current = (char*)deserializer.Current;
//            end = (char*)deserializer.End;
//            if (end > Current)
//            {
//                textFixed = Current;
//                MemberMap = null;
//                deserialize(ref value);
//                if (State == DeserializeStateEnum.Success) return true;
//            }
//            else State = DeserializeStateEnum.CrashEnd;
//            deserializer.SetCustomError(this);
//            return false;
//        }
        /// <summary>
        /// 释放 JSON 解析器（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeThreadStatic()
        {
            free();
            Config = DefaultConfig;
        }
        /// <summary>
        /// 释放 JSON 解析器
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Push(this);
        }
        /// <summary>
        /// 释放 JSON 解析器
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeBinaryMix()
        {
            isCheckRemoteType = true;
            IsBinaryMix = false;
            Free();
        }
        /// <summary>
        /// 设置二进制混杂模式
        /// </summary>
        /// <param name="isCheckRemoteType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBinaryMix(bool isCheckRemoteType)
        {
            this.isCheckRemoteType = isCheckRemoteType;
            IsBinaryMix = true;
            Config = DefaultConfig;
        }
        /// <summary>
        /// 扫描空格字符
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void space()
        {
            if (Current != end)
            {
                if (((bits[*(byte*)Current] & DeserializeSpaceStartBit) | *(((byte*)Current) + 1)) != 0) return;
                spaceNext();
            }
        }
        /// <summary>
        /// 扫描空格字符
        /// </summary>
        private void spaceNext()
        {
        SPACE:
            if (*Current != '/')
            {
                if (++Current != end)
                {
                    if (!isEndSpace)
                    {
                        do
                        {
                            if (((bits[*(byte*)Current] & DeserializeSpaceBit) | *(((byte*)Current) + 1)) != 0)
                            {
                                if (*Current != '/') return;
                                goto NOTE;
                            }
                        }
                        while (++Current != end);
                        return;
                    }
                    while (((bits[*(byte*)Current] & DeserializeSpaceBit) | *(((byte*)Current) + 1)) == 0) ++Current;
                    if (*Current != '/') return;
                }
                else return;
            }
        NOTE:
            if (++Current != end)
            {
                if (*Current == '/')
                {
                    ++Current;
                    if (endChar != '\n')
                    {
                        if (Current == end || !isFindChar(0xa000a000a000aUL) || !isFindChar('\n')) return;
                    }
                    else
                    {
                        findChar(0xa000a000a000aUL);
                        while (*Current != '\n') ++Current;
                    }
                    if (++Current == end) return;
                    if (((bits[*(byte*)Current] & DeserializeSpaceStartBit) | *(((byte*)Current) + 1)) != 0) return;
                    goto SPACE;
                }
                if (*Current == '*')
                {
                    if (++Current != end)
                    {
                        if (endChar == '/')
                        {
                            while (++Current != end)
                            {
                                findChar(0x2f002f002f002fUL);
                                while (*Current != '/') ++Current;
                                if (*(Current - 1) == '*')
                                {
                                    if (++Current == end) return;
                                    if (((bits[*(byte*)Current] & DeserializeSpaceStartBit) | *(((byte*)Current) + 1)) != 0) return;
                                    goto SPACE;
                                }
                                if (++Current == end) break;
                            }
                        }
                        else
                        {
                            while (++Current != end && isFindChar(0x2f002f002f002fUL) && isFindChar('/'))
                            {
                                if (*(Current - 1) == '*')
                                {
                                    if (++Current == end) return;
                                    if (((bits[*(byte*)Current] & DeserializeSpaceStartBit) | *(((byte*)Current) + 1)) != 0) return;
                                    goto SPACE;
                                }
                                if (++Current == end) break;
                            }
                        }
                    }
                    State = DeserializeStateEnum.NoteNotRound;
                    return;
                }
            }
            State = DeserializeStateEnum.UnknownNote;
        }
        /// <summary>
        /// 是否null
        /// </summary>
        /// <returns>是否null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsNull()
        {
            if (*Current == 'n')
            {
                if (*(long*)Current == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    Current += 4;
                    return true;
                }
                State = DeserializeStateEnum.NotNull;
            }
            return false;
        }
        /// <summary>
        /// 是否null
        /// </summary>
        /// <returns>是否null</returns>
        private bool tryNull()
        {
            if (IsNull()) return true;
            if (IsBinaryMix) return false;
            space();
            return IsNull();
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Deserialize<T>(JsonDeserializer jsonDeserializer, ref T? value)
#else
        internal static void Deserialize<T>(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
            TypeDeserializer<T>.DefaultDeserializer(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 自定义反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ICustom<T>(AutoCSer.JsonDeserializer deserializer, ref T? value)
#else
        public static void ICustom<T>(AutoCSer.JsonDeserializer deserializer, ref T value)
#endif
             where T : ICustomSerialize<T>
        {
            if (deserializer.IsNull()) value = default(T);
            else if (deserializer.Constructor(out value)) value.Deserialize(deserializer);
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void NotSupport<T>(JsonDeserializer jsonDeserializer, ref T? value)
#else
        internal static void NotSupport<T>(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
            if (!JsonSerializer.CustomConfig.NotSupport(jsonDeserializer, ref value)) jsonDeserializer.State = DeserializeStateEnum.NotSupport;
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
        /// 基类转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="value">Target data</param>
#if NetStandard21
        private void baseSerialize<T, BT>(ref T? value) where T : class, BT
#else
        private void baseSerialize<T, BT>(ref T value) where T : class, BT
#endif
        {
            if (value == null)
            {
                if (!SearchObject()) return;
                if (AutoCSer.Metadata.DefaultConstructor<T>.Type != Metadata.DefaultConstructorTypeEnum.None)
                {
                    if (!Constructor(out value)) return;
                }
                else if (!AutoCSer.JsonSerializer.CustomConfig.CallCustomConstructor(out value))
                {
                    NoConstructorIgnoreObject();
                    return;
                }
                --Current;
            }
#if NetStandard21
            BT? newValue = value;
#else
            BT newValue = value;
#endif
            TypeDeserializer<BT>.DefaultDeserializer(this, ref newValue);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Base<T, BT>(JsonDeserializer jsonDeserializer, ref T? value) where T : class, BT
#else
        public static void Base<T, BT>(JsonDeserializer jsonDeserializer, ref T value) where T : class, BT
#endif
        {
            jsonDeserializer.baseSerialize<T, BT>(ref value);
        }
        /// <summary>
        /// 二进制混杂模式反序列化数组长度
        /// </summary>
        /// <returns></returns>
        private int binaryMixArrayLength()
        {
            switch ((*(byte*)Current - (byte)BinaryMixTypeEnum.ArrayByte) & 7)
            {
                case (byte)BinaryMixTypeEnum.ArrayByte - (byte)BinaryMixTypeEnum.ArrayByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.ArrayByte)
                    {
                        int length = *((byte*)Current + 1);
                        if (length != 0 && end - ++Current >= length) return length;
                    }
                    break;
                case (byte)BinaryMixTypeEnum.ArrayByte3 - (byte)BinaryMixTypeEnum.ArrayByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.ArrayByte3)
                    {
                        int length = (int)(*(uint*)Current >> 8);
                        if (length != 0 && end - (Current += sizeof(uint) >> 1) >= length) return length;
                    }
                    break;
                case (byte)BinaryMixTypeEnum.Array - (byte)BinaryMixTypeEnum.ArrayByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.Array)
                    {
                        int length = *(int*)((byte*)Current + sizeof(ushort));
                        if (length > 0 && end - (Current += (sizeof(ushort) + sizeof(int)) >> 1) >= length) return length;
                    }
                    break;
                case ('n' - (byte)BinaryMixTypeEnum.ArrayByte) & 7:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        if ((Current += 4) <= end) return -1;
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    break;
                case ('[' - (byte)BinaryMixTypeEnum.ArrayByte) & 7:
                    if (*(int*)Current == '[' + (']' << 16))
                    {
                        if ((Current += 2) <= end) return 0;
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    else if (*Current == '[')
                    {
                        if (++Current < end) return -2;
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    break;
            }
            if (State != DeserializeStateEnum.Success) State = DeserializeStateEnum.NotArrayValue;
            return int.MinValue;
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">目标数据</param>
        /// <returns>数据数量,-1 表示失败</returns>
#if NetStandard21
        private int arrayIndex<T>(ref T?[]? array)
#else
        private int arrayIndex<T>(ref T[] array)
#endif
        {
            int index;
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        index = 0;
                        do
                        {
                            TypeDeserializer<T>.DefaultDeserializer(this, ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return -1;
                        }
                        while (index != array.Length);
                        return index;
                    }
                }
                else return -1;
            }
            else
            {
                switch (SearchArray(ref array))
                {
                    case 0: break;
                    case 1: return 0;
                    default: return -1;
                }
            }
            array = new T[Config.NewArraySize];
            index = 0;
            do
            {
                TypeDeserializer<T>.DefaultDeserializer(this, ref array[index]);
                if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                {
                    ++index;
                    if (IsNextArrayValue())
                    {
                        if (index == array.Length) array = AutoCSer.Common.GetCopyArray(array, index << 1);
                    }
                    else return index;
                }
                else return -1;
            }
            while (true);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize<T>(ref T?[]? array)
#else
        public void JsonDeserialize<T>(ref T[] array)
#endif
        {
            int count = arrayIndex(ref array);
            if (count >= 0 && count != array.notNull().Length)
            {
                if (count != 0) System.Array.Resize(ref array, count);
                else array = EmptyArray<T>.Array;
            }
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Array<T>(JsonDeserializer jsonDeserializer, ref T?[]? array)
#else
        public static void Array<T>(JsonDeserializer jsonDeserializer, ref T[] array)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref array);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void JsonDeserialize<T>(ref LeftArray<T?> array)
#else
        public void JsonDeserialize<T>(ref LeftArray<T> array)
#endif
        {
#if NetStandard21
            T?[]? arrayValue = null;
#else
            T[] arrayValue = null;
#endif
            JsonDeserialize(ref arrayValue);

#if NetStandard21
            array = arrayValue != null ? new LeftArray<T?>(arrayValue) : default(LeftArray<T?>);
#else
            array = arrayValue != null ? new LeftArray<T>(arrayValue) : default(LeftArray<T>);
#endif
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void LeftArray<T>(JsonDeserializer jsonDeserializer, ref LeftArray<T?> array)
#else
        public static void LeftArray<T>(JsonDeserializer jsonDeserializer, ref LeftArray<T> array)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref array);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void JsonDeserialize<T>(ref ListArray<T?>? array)
#else
        public void JsonDeserialize<T>(ref ListArray<T> array)
#endif
        {
#if NetStandard21
            T?[]? arrayValue = null;
#else
            T[] arrayValue = null;
#endif
            JsonDeserialize(ref arrayValue);
#if NetStandard21
            array = arrayValue != null ? new ListArray<T?>(arrayValue) : null;
#else
            array = arrayValue != null ? new ListArray<T>(arrayValue) : null;
#endif
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="array"></param>
#if NetStandard21
        public static void ListArray<T>(JsonDeserializer jsonDeserializer, ref ListArray<T?>? array)
#else
        public static void ListArray<T>(JsonDeserializer jsonDeserializer, ref ListArray<T> array)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref array);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Target data</param>
        public void JsonDeserialize<T>(ref Nullable<T> value) where T : struct
        {
            if (tryNull()) value = null;
            else if (State == DeserializeStateEnum.Success)
            {
                T newValue = value.HasValue ? value.Value : default(T);
                TypeDeserializer<T>.DefaultDeserializer(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Nullable<T>(JsonDeserializer jsonDeserializer, ref Nullable<T> value) where T : struct
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 查找数组起始位置
        /// </summary>
        /// <returns>返回 0 表示未结束，返回 1 表示已结束，返回 -1 表示 null 或者错误</returns>
        private int searchCollection()
        {
            bool isSpace = IsBinaryMix;
        START:
            if (*Current == '[')
            {
                if (*++Current == ']')
                {
                    ++Current;
                    return 1;
                }
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current < end)
                    {
                        if (*Current == ']')
                        {
                            ++Current;
                            return 1;
                        }
                        return 0;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
                return -1;
            }
            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == NullStringValue)
            {
                Current += 4;
                return -1;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.CrashEnd;
            return -1;
        }
        /// <summary>
        /// 集合反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="collection"></param>
#if NetStandard21
        public void JsonDeserialize<T, VT>(ref T? collection) where T : ICollection<VT?>
#else
        public void JsonDeserialize<T, VT>(ref T collection) where T : ICollection<VT>
#endif
        {
            switch (searchCollection())
            {
                case 0:
                    if (!Constructor(out collection)) return;
                    do
                    {
                        var value = default(VT);
                        TypeDeserializer<VT>.DefaultDeserializer(this, ref value);
                        if (State == DeserializeStateEnum.Success) collection.Add(value);
                        else return;
                    }
                    while (IsNextArrayValue());
                    return;
                case 1: Constructor(out collection); return;
                default: collection = default(T); return;
            }
        }
        /// <summary>
        /// 集合反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="collection"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Collection<T, VT>(JsonDeserializer jsonDeserializer, ref T? collection) where T : ICollection<VT?>
#else
        public static void Collection<T, VT>(JsonDeserializer jsonDeserializer, ref T collection) where T : ICollection<VT>
#endif
        {
            jsonDeserializer.JsonDeserialize<T, VT>(ref collection);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="value">Target data</param>
        public void JsonDeserialize<KT, VT>(ref KeyValuePair<KT, VT> value)
        {
            KeyValue<KT, VT> keyValue = new KeyValue<KT, VT>(value.Key, value.Value);
            TypeDeserializer<KeyValue<KT, VT>>.DefaultDeserializer(this, ref keyValue);
            value = new KeyValuePair<KT, VT>(keyValue.Key, keyValue.Value);
        }
        /// <summary>
        /// 键值对解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void KeyValuePair<KT, VT>(JsonDeserializer jsonDeserializer, ref KeyValuePair<KT, VT> value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 字典解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary">目标数据</param>
#if NetStandard21
        public void JsonDeserialize<KT, VT>(ref Dictionary<KT, VT?>? dictionary) where KT : notnull
#else
        public void JsonDeserialize<KT, VT>(ref Dictionary<KT, VT> dictionary)
#endif
        {
            byte type = searchDictionary();
            if (type == 0) dictionary = null;
            else
            {
#if NetStandard21
                dictionary = new Dictionary<KT, VT?>();
#else
                dictionary = new Dictionary<KT, VT>();
#endif
                if (type == 1)
                {
                    if (isDictionaryObjectEnd() == 0)
                    {
                        bool isStringKey = typeof(KT) == typeof(string) && IsBinaryMix;
                        do
                        {
                            var key = default(KT);
                            if (isStringKey)
                            {
                                var stringKey = default(string);
                                primitiveDeserialize(ref stringKey);
                                if (stringKey != null) key = (KT)(object)stringKey;
                            }
                            else TypeDeserializer<KT>.DefaultDeserializer(this, ref key);
                            if (State == DeserializeStateEnum.Success && SearchColon() != 0)
                            {
                                var value = default(VT);
                                TypeDeserializer<VT>.DefaultDeserializer(this, ref value);
                                if (State == DeserializeStateEnum.Success)
                                {
#pragma warning disable CS8604
                                    dictionary.Add(key, value);
#pragma warning restore CS8604
                                }
                                else return;
                            }
                            else return;
                        }
                        while (IsNextObject());
                    }
                }
                else if (IsFirstArrayValue())
                {
                    do
                    {
#if NetStandard21
                        KeyValue<KT, VT?> value = default(KeyValue<KT, VT?>);
                        TypeDeserializer<KeyValue<KT, VT?>>.DefaultDeserializer(this, ref value);
#else
                        KeyValue<KT, VT> value = default(KeyValue<KT, VT>);
                        TypeDeserializer<KeyValue<KT, VT>>.DefaultDeserializer(this, ref value);
#endif
                        if (State == DeserializeStateEnum.Success) dictionary.Add(value.Key, value.Value);
                        else return;
                    }
                    while (IsNextArrayValue());
                }
            }
        }
        /// <summary>
        /// 字典解析
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="dictionary">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Dictionary<KT, VT>(JsonDeserializer jsonDeserializer, ref Dictionary<KT, VT?>? dictionary) where KT : notnull
#else
        public static void Dictionary<KT, VT>(JsonDeserializer jsonDeserializer, ref Dictionary<KT, VT> dictionary)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref dictionary);
        }
        /// <summary>
        /// 字典反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
#if NetStandard21
        public void JsonDeserialize<T, KT, VT>(ref T? dictionary)
#else
        public void JsonDeserialize<T, KT, VT>(ref T dictionary)
#endif
            where T : IDictionary<KT, VT>
        {
            switch (searchCollection())
            {
                case 0:
                    if (!Constructor(out dictionary)) return;
                    do
                    {
                        KeyValue<KT, VT> value = new KeyValue<KT, VT>();
                        TypeDeserializer<KeyValue<KT, VT>>.DefaultDeserializer(this, ref value);
                        if (State == DeserializeStateEnum.Success) dictionary.Add(value.Key, value.Value);
                        else return;
                    }
                    while (IsNextArrayValue());
                    return;
                case 1: Constructor(out dictionary); return;
                default: dictionary = default(T); return;
            }
        }
        /// <summary>
        /// 字典反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void IDictionary<T, KT, VT>(JsonDeserializer jsonDeserializer, ref T? dictionary) where T : IDictionary<KT, VT>
#else
        public static void IDictionary<T, KT, VT>(JsonDeserializer jsonDeserializer, ref T dictionary) where T : IDictionary<KT, VT>
#endif
        {
            jsonDeserializer.JsonDeserialize<T, KT, VT>(ref dictionary);
        }

        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>解析状态</returns>
        public void JsonDeserialize(ref bool value)
        {
            if (IsBinaryMix)
            {
                switch (*Current - (byte)BinaryMixTypeEnum.False)
                {
                    case 0:
                        value = false;
                        ++Current;
                        return;
                    case 1:
                        value = true;
                        ++Current;
                        return;
                    default: State = DeserializeStateEnum.NotBool; return;
                }
            }
            else
            {
                Quote = (char)0;
                deserialize(out value);
            }
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>解析状态</returns>
        public void JsonDeserialize(ref bool? value)
        {
            if (IsBinaryMix)
            {
                switch (*Current & 3)
                {
                    case (byte)BinaryMixTypeEnum.False & 3:
                        if (*Current == (byte)BinaryMixTypeEnum.False)
                        {
                            value = false;
                            ++Current;
                            return;
                        }
                        break;
                    case (byte)BinaryMixTypeEnum.True & 3:
                        if (*Current == (byte)BinaryMixTypeEnum.True)
                        {
                            value = true;
                            ++Current;
                            return;
                        }
                        break;
                    case 'n' & 3:
                        if (*(long*)Current == JsonDeserializer.NullStringValue)
                        {
                            value = null;
                            Current += 4;
                            return;
                        }
                        break;
                }
                State = DeserializeStateEnum.NotBool;
                return;
            }
            Quote = (char)0;
            bool boolValue;
            if (deserialize(out boolValue)) value = null;
            else value = boolValue;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref byte value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    return;
                default: State = DeserializeStateEnum.NotNumber; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref byte value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        private void primitiveDeserialize(ref byte value)
        {
            uint value32 = primitiveDeserializeUInt();
            value = (byte)value32;
            if ((value32 & 0xffffff00U) != 0) State = DeserializeStateEnum.NumberOutOfRange;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private NullableBoolEnum binaryMixNull(out byte value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    break;
                case 'n' - (byte)BinaryMixTypeEnum.Byte:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        value = 0;
                        Current += 4;
                        return NullableBoolEnum.Null;
                    }
                    break;
                default: State = DeserializeStateEnum.NotNumber; break;
            }
            value = 0;
            return NullableBoolEnum.False;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref byte? value)
        {
            if (IsBinaryMix)
            {
                byte binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeUIntNull().GetByteNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(ref sbyte value)
        {
            byte binaryMixValue = 0;
            binaryMix(ref binaryMixValue);
            value = (sbyte)binaryMixValue;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref sbyte value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else value = primitiveDeserializeInt().GetSByte(ref State);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref sbyte? value)
        {
            if (IsBinaryMix)
            {
                byte binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = (sbyte)binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeIntNull().GetSByteNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref ushort value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    return;
                default: State = DeserializeStateEnum.NotNumber; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref ushort value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        private void primitiveDeserialize(ref ushort value)
        {
            uint value32 = primitiveDeserializeUInt();
            value = (ushort)value32;
            if ((value32 & 0xffff0000U) != 0) State = DeserializeStateEnum.NumberOutOfRange;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private NullableBoolEnum binaryMixNull(out ushort value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    break;
                case 'n' - (byte)BinaryMixTypeEnum.Byte:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        value = 0;
                        Current += 4;
                        return NullableBoolEnum.Null;
                    }
                    break;
                default: State = DeserializeStateEnum.NotNumber; break;
            }
            value = 0;
            return NullableBoolEnum.False;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref ushort? value)
        {
            if (IsBinaryMix)
            {
                ushort binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeUIntNull().GetUShortNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(ref short value)
        {
            ushort binaryMixValue = 0;
            binaryMix(ref binaryMixValue);
            value = (short)binaryMixValue;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref short value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else value = primitiveDeserializeInt().GetShort(ref State);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref short? value)
        {
            if (IsBinaryMix)
            {
                ushort binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = (short)binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeIntNull().GetShortNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref uint value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    value = *(uint*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    return;
                default: State = DeserializeStateEnum.NotNumber; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref uint value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else value = primitiveDeserializeUInt();
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private NullableBoolEnum binaryMixNull(out uint value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    value = *(uint*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    break;
                case 'n' - (byte)BinaryMixTypeEnum.Byte:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        value = 0;
                        Current += 4;
                        return NullableBoolEnum.Null;
                    }
                    break;
                default: State = DeserializeStateEnum.NotNumber; break;
            }
            value = 0;
            return NullableBoolEnum.False;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref uint? value)
        {
            if (IsBinaryMix)
            {
                uint binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeUIntNull().GetUIntNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(ref int value)
        {
            uint binaryMixValue = 0;
            binaryMix(ref binaryMixValue);
            value = (int)binaryMixValue;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref int value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else value = primitiveDeserializeInt().GetInt(ref State);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref int? value)
        {
            if (IsBinaryMix)
            {
                uint binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = (int)binaryMixValue; return;
                    default: return;
                }
            }
            value = primitiveDeserializeIntNull().GetIntNull(ref State);
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref ulong value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    value = *(uint*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ulong*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(ulong)) >> 1;
                    return;
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    return;
                default: State = DeserializeStateEnum.NotNumber; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref ulong value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        private void primitiveDeserialize(ref ulong value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if (*Current == '"' || *Current == '\'')
                            {
                                Quote = *Current;
                                if (++Current != end)
                                {
                                    if ((number = (uint)(*Current - '0')) < 10)
                                    {
                                        if (++Current != end)
                                        {
                                            if (number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    value = 0;
                                                    ++Current;
                                                    return;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value = deserializeHex64();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value = deserializeUInt64(number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.NotNumber;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                            }
                            else State = DeserializeStateEnum.NotNumber;
                            return;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else return;
            }
            if (++Current != end)
            {
                if (number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value = deserializeHex64();
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                    else value = 0;
                }
                else value = deserializeUInt64(number);
            }
            else value = number;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private NullableBoolEnum binaryMixNull(out ulong value)
        {
            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
            {
                case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    value = *((byte*)Current + 1);
                    ++Current;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ushort*)((byte*)Current + sizeof(ushort));
                    Current += sizeof(uint) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    value = *(uint*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    value = *(ulong*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(ulong)) >> 1;
                    return NullableBoolEnum.True;
                case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                    State = DeserializeStateEnum.NumberOutOfRange;
                    break;
                case 'n' - (byte)BinaryMixTypeEnum.Byte:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        value = 0;
                        Current += 4;
                        return NullableBoolEnum.Null;
                    }
                    break;
                default: State = DeserializeStateEnum.NotNumber; break;
            }
            value = 0;
            return NullableBoolEnum.False;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref ulong? value)
        {
            if (IsBinaryMix)
            {
                ulong binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = binaryMixValue; return;
                    default: return;
                }
            }
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (IsNull())
                {
                    value = null;
                    return;
                }
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if (IsNull()) value = null;
                            else if (*Current == '"' || *Current == '\'')
                            {
                                Quote = *Current;
                                if (++Current != end)
                                {
                                    if ((number = (uint)(*Current - '0')) < 10)
                                    {
                                        if (++Current != end)
                                        {
                                            if (number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    value = 0;
                                                    ++Current;
                                                    return;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value = deserializeHex64();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value = deserializeUInt64(number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.NotNumber;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                            }
                            else State = DeserializeStateEnum.NotNumber;
                            return;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else return;
            }
            if (++Current != end)
            {
                if (number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value = deserializeHex64();
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                    else value = 0;
                }
                else value = deserializeUInt64(number);
            }
            else value = number;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(ref long value)
        {
            ulong binaryMixValue = 0;
            binaryMix(ref binaryMixValue);
            value = (long)binaryMixValue;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref long value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        private void primitiveDeserialize(ref long value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current != '-')
                {
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current == '"' || *Current == '\'')
                                {
                                    Quote = *Current;
                                    if (++Current != end)
                                    {
                                        if ((number = (uint)(*Current - '0')) > 9)
                                        {
                                            if (*Current == '-')
                                            {
                                                if (++Current != end)
                                                {
                                                    if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                            else State = DeserializeStateEnum.NotNumber;
                                        }
                                        if (++Current != end)
                                        {
                                            if (number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    value = 0;
                                                    ++Current;
                                                    return;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value = getLong(deserializeHex64(), sign);
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value = getLong(deserializeUInt64(number), sign);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.CrashEnd;
                                    return;
                                }
                                if (*Current == '-')
                                {
                                    if (++Current != end)
                                    {
                                        if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                                        else
                                        {
                                            State = DeserializeStateEnum.NotNumber;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return;
                                    }
                                }
                                else
                                {
                                    State = DeserializeStateEnum.NotNumber;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return;
                        }
                    }
                    else return;
                }
                else if (++Current != end)
                {
                    if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                    else
                    {
                        State = DeserializeStateEnum.NotNumber;
                        return;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            if (++Current != end)
            {
                if (number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value = getLong(deserializeHex64(), sign);
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                    else value = 0;
                    return;
                }
                value = getLong(deserializeUInt64(number), sign);
            }
            else value = sign == 0 ? (long)(int)number : -(long)(int)number;
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private long getLong(ulong value, int sign)
        {
            if (sign == 0)
            {
                if (value <= long.MaxValue) return (long)value;
            }
            else
            {
                if (value < (1UL << 63)) return -(long)value;
                if (value == (1UL << 63)) return long.MinValue;
            }
            State = DeserializeStateEnum.NumberOutOfRange;
            return 0;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref long? value)
        {
            if (IsBinaryMix)
            {
                ulong binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = (long)binaryMixValue; return;
                    default: return;
                }
            }
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (IsNull())
                {
                    value = null;
                    return;
                }
                if (*Current != '-')
                {
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (IsNull())
                                {
                                    value = null;
                                    return;
                                }
                                if (*Current == '"' || *Current == '\'')
                                {
                                    Quote = *Current;
                                    if (++Current != end)
                                    {
                                        if ((number = (uint)(*Current - '0')) > 9)
                                        {
                                            if (*Current == '-')
                                            {
                                                if (++Current != end)
                                                {
                                                    if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                                                    else
                                                    {
                                                        State = DeserializeStateEnum.NotNumber;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    State = DeserializeStateEnum.CrashEnd;
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                State = DeserializeStateEnum.NotNumber;
                                                return;
                                            }
                                        }
                                        if (++Current != end)
                                        {
                                            if (number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    value = 0;
                                                    ++Current;
                                                    return;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value = getLong(deserializeHex64(), sign);
                                                    else State = DeserializeStateEnum.CrashEnd;

                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value = getLong(deserializeUInt64(number),sign);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.CrashEnd;
                                    return;
                                }
                                if (*Current == '-')
                                {
                                    if (++Current != end)
                                    {
                                        if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                                        else
                                        {
                                            State = DeserializeStateEnum.NotNumber;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return;
                                    }
                                }
                                else
                                {
                                    State = DeserializeStateEnum.NotNumber;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return;
                        }
                    }
                    else return;
                }
                else if (++Current != end)
                {
                    if ((number = (uint)(*Current - '0')) < 10) sign = 1;
                    else
                    {
                        State = DeserializeStateEnum.NotNumber;
                        return;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            if (++Current != end)
            {
                if (number == 0)
                {
                    if (*Current != 'x') value = 0;
                    else if (++Current != end) value = getLong(deserializeHex64(), sign);
                    else State = DeserializeStateEnum.CrashEnd;
                    return;
                }
                value = getLong(deserializeUInt64(number), sign);
            }
            else value = sign == 0 ? (long)(int)number : -(long)(int)number;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref Int128 value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
#if NET8
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                        value = *((byte*)Current + 1);
                        ++Current;
                        return;
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                        value = *(ushort*)((byte*)Current + sizeof(ushort));
                        Current += sizeof(uint) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                        value = *(uint*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                        value = *(ulong*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(ulong)) >> 1;
                        return;
#endif
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                        value = *(Int128*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Int128)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, false))
                    {
                        Current = end;
                        return;
                    }
                    break;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, false))
                    {
                        Current = end + 1;
                        return;
                    }
                    break;
                case NumberTypeEnum.Object:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, true)) return;
                    break;
            }
            State = DeserializeStateEnum.NotNumber;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref UInt128 value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                        value = new UInt128(0, *((byte*)Current + 1));
                        ++Current;
                        return;
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                        value = new UInt128(0, *(ushort*)((byte*)Current + sizeof(ushort)));
                        Current += sizeof(uint) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                        value = new UInt128(0, *(uint*)((byte*)Current + sizeof(ushort)));
                        Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                        value = new UInt128(0, *(ulong*)((byte*)Current + sizeof(ushort)));
                        Current += (sizeof(ushort) + sizeof(ulong)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                        value = *(UInt128*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(UInt128)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, false))
                    {
                        Current = end;
                        return;
                    }
                    break;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, false))
                    {
                        Current = end + 1;
                        return;
                    }
                    break;
                case NumberTypeEnum.Object:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value, true)) return;
                    break;
            }
            State = DeserializeStateEnum.NotNumber;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref Half value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                        value = *(Half*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Half)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
#if NET8
                case NumberTypeEnum.NaN: value = Half.NaN; return;
                case NumberTypeEnum.PositiveInfinity: value = Half.PositiveInfinity; return;
                case NumberTypeEnum.NegativeInfinity: value = Half.NegativeInfinity; return;
#endif
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref float value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
#if NET8
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                        value = (float)*(Half*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Half)) >> 1;
                        return;
#endif
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                        value = *(float*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(float)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN: value = float.NaN; return;
                case NumberTypeEnum.PositiveInfinity: value = float.PositiveInfinity; return;
                case NumberTypeEnum.NegativeInfinity: value = float.NegativeInfinity; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref float? value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
#if NET8
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                        value = (float)*(Half*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Half)) >> 1;
                        return;
#endif
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                        value = *(float*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(float)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    case 'n' - (byte)BinaryMixTypeEnum.Byte:
                        if (*(long*)Current == JsonDeserializer.NullStringValue)
                        {
                            value = null;
                            Current += 4;
                            return;
                        }
                        break;
                }
                State = DeserializeStateEnum.NotNumber; 
                return;
            }
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberTypeEnum.Number:
                    float valueNumber = default(float);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    float valueString = default(float);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN: value = float.NaN; return;
                case NumberTypeEnum.PositiveInfinity: value = float.PositiveInfinity; return;
                case NumberTypeEnum.NegativeInfinity: value = float.NegativeInfinity; return;
                case NumberTypeEnum.Null: value = null; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref double value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
#if NET8
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                        value = (double)*(Half*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Half)) >> 1;
                        return;
#endif
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                        value = *(float*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(float)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                        value = *(double*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(double)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN: value = double.NaN; return;
                case NumberTypeEnum.PositiveInfinity: value = double.PositiveInfinity; return;
                case NumberTypeEnum.NegativeInfinity: value = double.NegativeInfinity; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref double? value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
#if NET8
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                        value = (double)*(Half*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(Half)) >> 1;
                        return;
#endif
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                        value = *(float*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(float)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                        value = *(double*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(double)) >> 1;
                        return;
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    case 'n' - (byte)BinaryMixTypeEnum.Byte:
                        if (*(long*)Current == JsonDeserializer.NullStringValue)
                        {
                            value = null;
                            Current += 4;
                            return;
                        }
                        break;
                }
                State = DeserializeStateEnum.NotNumber;
                return;
            }
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberTypeEnum.Number:
                    double valueNumber = default(double);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    double valueString = default(double);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN: value = double.NaN; return;
                case NumberTypeEnum.PositiveInfinity: value = double.PositiveInfinity; return;
                case NumberTypeEnum.NegativeInfinity: value = double.NegativeInfinity; return;
                case NumberTypeEnum.Null: value = null; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref decimal value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        value = *(decimal*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(decimal)) >> 1;
                        return;
                    default: State = DeserializeStateEnum.NotNumber; return;
                }
            }
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberTypeEnum.Number:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN:
                case NumberTypeEnum.PositiveInfinity:
                case NumberTypeEnum.NegativeInfinity:
                    State = DeserializeStateEnum.NotNumber;
                    return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref decimal? value)
        {
            if (IsBinaryMix)
            {
                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                {
                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                        State = DeserializeStateEnum.NumberOutOfRange;
                        return;
                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                        value = *(decimal*)((byte*)Current + sizeof(ushort));
                        Current += (sizeof(ushort) + sizeof(decimal)) >> 1;
                        return;
                    case 'n' - (byte)BinaryMixTypeEnum.Byte:
                        if (*(long*)Current == JsonDeserializer.NullStringValue)
                        {
                            value = null;
                            Current += 4;
                            return;
                        }
                        break;
                }
                State = DeserializeStateEnum.NotNumber; 
                return;
            }
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberTypeEnum.Number:
                    decimal valueNumber = default(decimal);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.String:
                    decimal valueString = default(decimal);
                    if (JsonSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(Current, (int)((byte*)end - (byte*)Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.NaN:
                case NumberTypeEnum.PositiveInfinity:
                case NumberTypeEnum.NegativeInfinity:
                    State = DeserializeStateEnum.NotNumber;
                    return;
                case NumberTypeEnum.Null: value = null; return;
            }
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref char value)
        {
            if (IsBinaryMix)
            {
                ushort binaryMixValue = 0;
                binaryMix(ref binaryMixValue);
                value = (char)binaryMixValue;
                return;
            }
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current != end)
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == '\\')
                        {
                            if (++Current != end)
                            {
                                if (*Current == 'u')
                                {
                                    if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) value = (char)deserializeHex4();
                                    else
                                    {
                                        State = DeserializeStateEnum.NotChar;
                                        return;
                                    }
                                }
                                else if (*Current == 'x')
                                {
                                    if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) value = (char)deserializeHex2();
                                    else
                                    {
                                        State = DeserializeStateEnum.NotChar;
                                        return;
                                    }
                                }
                                else value = *Current < EscapeCharSize ? escapeChars[*Current] : *Current;
                            }
                            else
                            {
                                State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                        }
                        else if (*Current != Quote) value = *Current;
                        else
                        {
                            State = DeserializeStateEnum.NotChar;
                            return;
                        }
                    }
                    else value = *Current;
                    if (++Current != end)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            else if (isSpace == 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = 1;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
                return;
            }
            State = DeserializeStateEnum.NotChar;
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref char? value)
        {
            if (IsBinaryMix)
            {
                ushort binaryMixValue;
                switch (binaryMixNull(out binaryMixValue))
                {
                    case NullableBoolEnum.Null: value = null; return;
                    case NullableBoolEnum.True: value = (char)binaryMixValue; return;
                    default: return;
                }
            }
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current != end)
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == '\\')
                        {
                            if (++Current != end)
                            {
                                if (*Current == 'u')
                                {
                                    if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) value = (char)deserializeHex4();
                                    else
                                    {
                                        State = DeserializeStateEnum.NotChar;
                                        return;
                                    }
                                }
                                else if (*Current == 'x')
                                {
                                    if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) value = (char)deserializeHex2();
                                    else
                                    {
                                        State = DeserializeStateEnum.NotChar;
                                        return;
                                    }
                                }
                                else value = *Current < EscapeCharSize ? escapeChars[*Current] : *Current;
                            }
                            else
                            {
                                State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                        }
                        else if (*Current != Quote) value = *Current;
                        else
                        {
                            State = DeserializeStateEnum.NotChar;
                            return;
                        }
                    }
                    else value = *Current;
                    if (++Current != end)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            else
            {
                if (*(long*)(Current) == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    value = null;
                    Current += 4;
                    return;
                }
                if (isSpace == 0)
                {
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            isSpace = 1;
                            goto START;
                        }
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    return;
                }
            }
            State = DeserializeStateEnum.NotChar;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref DateTime value)
        {
            if (*(byte*)Current == (byte)BinaryMixTypeEnum.DateTime)
            {
                value = *(DateTime*)((byte*)Current + sizeof(ushort));
                Current += (sizeof(ushort) + sizeof(DateTime)) >> 1;
            }
            else State = DeserializeStateEnum.NotDateTime;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref DateTime value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else
            {
                Quote = (char)0;
                if (deserializeDateTime(ref value)) value = DateTime.MinValue;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref DateTime? value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)BinaryMixTypeEnum.DateTime)
                {
                    value = *(DateTime*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(DateTime)) >> 1;
                }
                else if (*(long*)Current == JsonDeserializer.NullStringValue)
                {
                    value = null;
                    Current += 4;
                    return;
                }
                else State = DeserializeStateEnum.NotDateTime;
                return;
            }
            Quote = (char)0;
            DateTime dateTime = default(DateTime);
            if (deserializeDateTime(ref dateTime)) value = null;
            else value = dateTime;
        }
        /// <summary>
        /// 二进制混杂反序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ref TimeSpan value)
        {
            if (*(byte*)Current == (byte)BinaryMixTypeEnum.TimeSpan)
            {
                value = *(TimeSpan*)((byte*)Current + sizeof(ushort));
                Current += (sizeof(ushort) + sizeof(TimeSpan)) >> 1;
            }
            else State = DeserializeStateEnum.NotTimeSpan;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonDeserialize(ref TimeSpan value)
        {
            if (IsBinaryMix) binaryMix(ref value);
            else
            {
                Quote = (char)0;
                if (deserializeTimeSpan(ref value)) value = TimeSpan.Zero;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref TimeSpan? value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)BinaryMixTypeEnum.TimeSpan)
                {
                    value = *(TimeSpan*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(TimeSpan)) >> 1;
                }
                else if (*(long*)Current == JsonDeserializer.NullStringValue)
                {
                    value = null;
                    Current += 4;
                    return;
                }
                else State = DeserializeStateEnum.NotTimeSpan;
                return;
            }
            Quote = (char)0;
            TimeSpan time = default(TimeSpan);
            if (deserializeTimeSpan(ref time)) value = null;
            else value = time;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否 null</returns>
        private bool deserializeTimeSpan(ref TimeSpan value)
        {
        START:
            switch (*Current)
            {
                case '"':
                case '\'':
                    if (Quote <= 1)
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 10 * sizeof(char))
                        {
                            char* start = Current;
                            if (deserializeTimeSpanString(ref value))
                            {
                                if (Current < end)
                                {
                                    if (*Current == *start)
                                    {
                                        ++Current;
                                        return false;
                                    }
                                }
                                else
                                {
                                    State = DeserializeStateEnum.CrashEnd;
                                    return false;
                                }
                            }
                            Current = start;
                            if (JsonSerializer.CustomConfig.Deserialize(this, ref value))
                            {
                                State = DeserializeStateEnum.Success;
                                return false;
                            }
                        }
                        Quote = (char)1;
                    }
                    break;
                case 'n':
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == NullStringValue)
                    {
                        Current += 4;
                        return true;
                    }
                    Quote = (char)1;
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            Quote = (char)1;
                            goto START;
                        }
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    return false;
                }
            }
            State = DeserializeStateEnum.NotTimeSpan;
            return false;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool deserializeTimeSpanString(ref TimeSpan value)
        {
            bool sign = false;
            if (*++Current == '-')
            {
                ++Current;
                sign = true;
            }
            uint hour = (uint)(*Current - '0');
            if (hour <= 9)
            {
                ++Current;
                long ticks = (long)deserializeUInt64(hour) * TimeSpan.TicksPerHour;
                if (*Current == ':')
                {
                    uint minute = deserializeDateTime();
                    if (minute < 60)
                    {
                        if (*Current == ':')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return false;
                            uint second = deserializeDateTime();
                            if (second >= 60 || Current >= end) return false;
                            ticks += (long)second * TimeSpan.TicksPerSecond;
                        }
                        if (*Current == '.')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return false;
                            ticks += deserializeDateTimeTicks();
                            if (Current >= end && ticks < 0) return false;
                        }
                        ticks += minute * TimeSpan.TicksPerMinute;
                        value = new TimeSpan(sign ? -ticks : ticks);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref Guid value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)BinaryMixTypeEnum.Guid)
                {
                    value = *(Guid*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Guid)) >> 1;
                }
                else State = DeserializeStateEnum.NotGuid;
                return;
            }
            if (*Current == '\'' || *Current == '"')
            {
                GuidCreator guid = new GuidCreator();
                deserialize(ref guid);
                value = guid.Value;
                return;
            }
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current == '\'' || *Current == '"')
                    {
                        GuidCreator guid = new GuidCreator();
                        deserialize(ref guid);
                        value = guid.Value;
                        return;
                    }
                    State = DeserializeStateEnum.NotGuid;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref Guid? value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)BinaryMixTypeEnum.Guid)
                {
                    value = *(Guid*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Guid)) >> 1;
                }
                else if (*(long*)Current == JsonDeserializer.NullStringValue)
                {
                    value = null;
                    Current += 4;
                    return;
                }
                else State = DeserializeStateEnum.NotGuid;
                return;
            }
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                GuidCreator guid = new GuidCreator();
                deserialize(ref guid);
                value = guid.Value;
                return;
            }
            if (*(long*)(Current) == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = 1;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotGuid;
        }
        /// <summary>
        /// 二进制混杂模式反序列化字符串长度
        /// </summary>
        /// <returns></returns>
        private int binaryMixStringLength()
        {
            switch ((*(byte*)Current - (byte)BinaryMixTypeEnum.StringByte) & 7)
            {
                case (byte)BinaryMixTypeEnum.StringByte - (byte)BinaryMixTypeEnum.StringByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.StringByte)
                    {
                        int length = *((byte*)Current + 1);
                        if (length != 0)
                        {
                            ++Current;
                            return length;
                        }
                    }
                    break;
                case (byte)BinaryMixTypeEnum.StringByte3 - (byte)BinaryMixTypeEnum.StringByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.StringByte3)
                    {
                        int length = (int)(*(uint*)Current >> 8);
                        if (length != 0)
                        {
                            Current += sizeof(uint) >> 1;
                            return length;
                        }
                    }
                    break;
                case (byte)BinaryMixTypeEnum.String - (byte)BinaryMixTypeEnum.StringByte:
                    if (*(byte*)Current == (byte)BinaryMixTypeEnum.String)
                    {
                        int length = *(int*)((byte*)Current + sizeof(ushort));
                        if (length > 0)
                        {
                            Current += (sizeof(ushort) + sizeof(int)) >> 1;
                            return length;
                        }
                    }
                    break;
                case ('n' - (byte)BinaryMixTypeEnum.StringByte) & 7:
                    if (*(long*)Current == JsonDeserializer.NullStringValue)
                    {
                        if ((Current += 4) <= end) return -1;
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    break;
                case ('"' - (byte)BinaryMixTypeEnum.StringByte) & 7:
                    if (*(int*)Current == '"' + ('"' << 16))
                    {
                        if ((Current += 2) <= end) return 0;
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    break;
            }
            if (State != DeserializeStateEnum.Success) State = DeserializeStateEnum.NotString;
            return int.MinValue;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">data</param>
#if NetStandard21
        public void JsonDeserialize(ref string? value)
#else
        public void JsonDeserialize(ref string value)
#endif
        {
            if (IsBinaryMix)
            {
                int length = binaryMixStringLength();
                if (length > 0)
                {
                    char* stringEnd = Current + length;
                    if (stringEnd <= end)
                    {
                        value = new string(Current, 0, length);
                        Current = stringEnd;
                    }
                    else State = DeserializeStateEnum.CrashEnd;
                    return;
                }
                switch (length + 1)
                {
                    case -1 + 1: value = null; return;
                    case 0 + 1: value = string.Empty; return;
                    default: return;
                }
            }
            primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">data</param>
#if NetStandard21
        private void primitiveDeserialize(ref string? value)
#else
        private void primitiveDeserialize(ref string value)
#endif
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                value = deserializeString();
                return;
            }
            if (*(long*)(Current) == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = 1;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotString;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref SubString value)
        {
            if (IsBinaryMix)
            {
                int length = binaryMixStringLength();
                if (length > 0)
                {
                    char* stringEnd = Current + length;
                    if (stringEnd <= end)
                    {
                        value.Set(new string(Current, 0, length), 0, length);
                        Current = stringEnd;
                    }
                    else State = DeserializeStateEnum.CrashEnd;
                }
                else if (length == 0) value.SetEmpty();
                return;
            }
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current != end)
                {
                    char* start = Current;
                    if (searchEscape() == 0)
                    {
                        if (State == DeserializeStateEnum.Success)
                        {
                            int length = (int)(Current++ - start);
                            if (text.Length == 0) value.Set(new string(start, 0, length), 0, length);
                            else value.Set(text, (int)(start - textFixed), length);
                        }
                        return;
                    }
                    if (Config.IsTempString && text.Length != 0)
                    {
                        char* writeEnd = deserializeEscape();
                        if (writeEnd != null) value.Set(text, (int)(start - textFixed), (int)(writeEnd - start));
                    }
                    else
                    {
                        var newValue = deserializeEscape(start);
                        if (newValue != null) value.Set(newValue, 0, newValue.Length);
                    }
                }
                else State = DeserializeStateEnum.CrashEnd;
                return;
            }
            if (*(long*)(Current) == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value.SetEmpty();
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = 1;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotString;
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="value">data</param>
#if NetStandard21
        public void JsonDeserialize(ref object? value)
#else
        public void JsonDeserialize(ref object value)
#endif
        {
            JsonNode node = default(JsonNode);
            JsonDeserialize(ref node);
            if (State == DeserializeStateEnum.Success)
            {
                if (node.Type == JsonNodeTypeEnum.Null) value = null;
                else value = node;
            }
        }
        /// <summary>
        /// 类型解析
        /// </summary>
        /// <param name="type"></param>
#if NetStandard21
        public void JsonDeserialize(ref Type? type)
#else
        public void JsonDeserialize(ref Type type)
#endif
        {
            bool isSpace = IsBinaryMix;
        START:
            if (*Current == '{')
            {
                AutoCSer.Reflection.RemoteType remoteType = default(AutoCSer.Reflection.RemoteType);
                TypeDeserializer<AutoCSer.Reflection.RemoteType>.DeserializeMembers(this, ref remoteType);
                if (State == DeserializeStateEnum.Success && !remoteType.TryGet(out type, isCheckRemoteType)) State = DeserializeStateEnum.ErrorType;
                return;
            }
            if (*(long*)Current == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                type = null;
                Current += 4;
                return;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.ErrorType;
        }
        /// <summary>
        /// JSON节点解析
        /// </summary>
        /// <param name="value">data</param>
        public void JsonDeserialize(ref JsonNode value)
        {
            bool isBinaryMix = IsBinaryMix;
            IsBinaryMix = false;
            try
            {
                primitiveDeserialize(ref value);
            }
            finally { IsBinaryMix = isBinaryMix; }
        }
        /// <summary>
        /// JSON节点解析
        /// </summary>
        /// <param name="value">data</param>
        private void primitiveDeserialize(ref JsonNode value)
        {
            LeftArray<JsonNode> nodeArray = new LeftArray<JsonNode>(0), nameArray = new LeftArray<JsonNode>(0);
        NEXTNODE:
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    switch (*Current & 7)
                    {
                        case '"' & 7:
                        case '\'' & 7:
                            if (*Current == '"' || *Current == '\'')
                            {
                                deserializeStringNode(ref value);
                                if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                return;
                            }
                            goto NUMBER;
                        case '{' & 7:
                            if (*Current == '{')
                            {
                                if (++Current != end)
                                {
                                    value.SetDictionary();
                                    if (IsFirstObject())
                                    {
                                        nodeArray.Add(value);
                                        goto DICTIONARYNAME;
                                    }
                                    if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                            if (*Current == '[')
                            {
                                if (++Current != end)
                                {
                                    value.SetList();
                                    if (IsFirstArrayValue())
                                    {
                                        nodeArray.Add(value);
                                        value = default(JsonNode);
                                        goto NEXTNODE;
                                    }
                                    if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                            goto NUMBER;
                        case 't' & 7:
                            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
                            {
                                Current += 4;
                                value.Int64 = 1;
                                value.Type = JsonNodeTypeEnum.Bool;
                                goto CHECKNODE;
                            }
                            goto NUMBER;
                        case 'f' & 7:
                            if (*Current == 'f')
                            {
                                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                                {
                                    Current += 5;
                                    value.Int64 = 0;
                                    value.Type = JsonNodeTypeEnum.Bool;
                                    goto CHECKNODE;
                                }
                                break;
                            }
                            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                            {
                                if (*(long*)(Current) == NullStringValue)
                                {
                                    value.Type = JsonNodeTypeEnum.Null;
                                    Current += 4;
                                    goto CHECKNODE;
                                }
                                if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                                {
                                    long millisecond = 0;
                                    Current += 9;
                                    JsonDeserialize(ref millisecond);
                                    if (State == DeserializeStateEnum.Success)
                                    {
                                        if (Current != end)
                                        {
                                            if (*Current == ')')
                                            {
                                                value.Int64 = JavaScriptLocalMinTimeTicks + millisecond * TimeSpan.TicksPerMillisecond;
                                                value.Type = JsonNodeTypeEnum.DateTimeTick;
                                                ++Current;
                                                goto CHECKNODE;
                                            }
                                            break;
                                        }
                                        State = DeserializeStateEnum.CrashEnd;
                                    }
                                    return;
                                }
                            }
                            goto NUMBER;
                        default:
                        NUMBER:
                            char* numberEnd = null;
                            switch (searchNumber(ref numberEnd))
                            {
                                case NumberTypeEnum.Number:
                                    int length = (int)(numberEnd - Current);
                                    if (text.Length == 0) value.SubString.Set(new string(Current, 0, length), 0, length);
                                    else value.SubString.Set(text, (int)(Current - textFixed), length);
                                    Current = numberEnd;
                                    value.SetNumberString(Quote);
                                    goto CHECKNODE;
                                case NumberTypeEnum.String:
                                    int stringLength = (int)(numberEnd - Current);
                                    if (text.Length == 0) value.SubString.Set(new string(Current, 0, stringLength), 0, stringLength);
                                    else value.SubString.Set(text, (int)(Current - textFixed), stringLength);
                                    Current = numberEnd + 1;
                                    value.SetNumberString(Quote);
                                    goto CHECKNODE;
                                case NumberTypeEnum.NaN: value.Type = JsonNodeTypeEnum.NaN; goto CHECKNODE;
                                case NumberTypeEnum.PositiveInfinity: value.Type = JsonNodeTypeEnum.PositiveInfinity; goto CHECKNODE;
                                case NumberTypeEnum.NegativeInfinity: value.Type = JsonNodeTypeEnum.NegativeInfinity; goto CHECKNODE;
                            }
                            break;
                    }
                    State = DeserializeStateEnum.UnknownValue;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return;

        DICTIONARYNAME:
            if (*Current == '"' || *Current == '\'') deserializeStringNode(ref value);
            else
            {
                char* nameStart = Current;
                SearchNameEnd();
                if (text.Length == 0)
                {
                    int length = (int)(Current - nameStart);
                    if (length == 0) value.SubString.Set(string.Empty, 0, 0);
                    else value.SubString.Set(new string(nameStart, 0, length), 0, length);
                }
                else value.SubString.Set(text, (int)(nameStart - textFixed), (int)(Current - nameStart));
                value.Type = JsonNodeTypeEnum.String;
            }
            if (State == DeserializeStateEnum.Success && SearchColon() != 0)
            {
                nameArray.Add(value);
                goto NEXTNODE;
            }
            return;

        CHECKNODE:
            if (nodeArray.Length != 0)
            {
                JsonNode parentNode = nodeArray.Array[nodeArray.Length - 1];
                switch (parentNode.Type)
                {
                    case JsonNodeTypeEnum.Dictionary:
                        LeftArray<KeyValue<JsonNode, JsonNode>> dictionary = parentNode.Dictionary;
                        dictionary.Add(new KeyValue<JsonNode, JsonNode>(ref nameArray.Array[--nameArray.Length], ref value));
                        if (IsNextObject())
                        {
                            nodeArray.Array[nodeArray.Length - 1].SetDictionary(ref dictionary);
                            goto DICTIONARYNAME;
                        }
                        value.SetDictionary(ref dictionary);
                        --nodeArray.Length;
                        goto CHECKNODE;
                    case JsonNodeTypeEnum.Array:
                        LeftArray<JsonNode> list = parentNode.Array;
                        list.Add(value);
                        if (IsNextArrayValue())
                        {
                            nodeArray.Array[nodeArray.Length - 1].SetList(ref list);
                            goto NEXTNODE;
                        }
                        value.SetList(ref list);
                        --nodeArray.Length;
                        goto CHECKNODE;
                }
            }
        }
        /// <summary>
        /// 解析字符串节点
        /// </summary>
        /// <param name="value"></param>
        private void deserializeStringNode(ref JsonNode value)
        {
            Quote = *Current;
            if (++Current != end)
            {
                char* start = Current;
                if (searchEscape() == 0)
                {
                    if (State == DeserializeStateEnum.Success)
                    {
                        int length = (int)(Current++ - start);
                        if (length == 0) value.SubString.Set(string.Empty, 0, 0);
                        else if (text.Length == 0) value.SubString = new string(start, 0, length);
                        else value.SubString.Set(text, (int)(start - textFixed), length);
                        value.Type = JsonNodeTypeEnum.String;
                    }
                    return;
                }
                if (text.Length != 0)
                {
                    char* escapeStart = Current;
                    searchEscapeEnd();
                    if (State == DeserializeStateEnum.Success)
                    {
                        value.SubString.Set(text, (int)(start - textFixed), (int)(Current - start));
                        value.SetQuoteString((int)(escapeStart - start), Quote, Config.IsTempString);
                        ++Current;
                    }
                }
                else
                {
                    var newValue = deserializeEscape(start);
                    if (newValue != null)
                    {
                        value.SubString.Set(newValue, 0, newValue.Length);
                        value.Type = JsonNodeTypeEnum.String;
                    }
                }
            }
            else State = DeserializeStateEnum.CrashEnd;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref bool[]? array)
#else
        public void JsonDeserialize(ref bool[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            switch (*(byte*)Current - (byte)BinaryMixTypeEnum.ArrayFalse)
                            {
                                case 0: array[index] = false; break;
                                case 1: array[index] = true; break;
                                default: State = DeserializeStateEnum.NotBool; return;
                            }
                            if (++index != array.Length)
                            {
                                switch (*((byte*)Current + 1) - (byte)BinaryMixTypeEnum.ArrayFalse)
                                {
                                    case 0: array[index] = false; break;
                                    case 1: array[index] = true; break;
                                    default: State = DeserializeStateEnum.NotBool; return;
                                }
                                ++Current;
                            }
                            else
                            {
                                ++Current;
                                return;
                            }
                        }
                        while (++index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref bool[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref bool[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 查找转义字符串结束位置
        /// </summary>
        private void searchEscapeEnd()
        {
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) Current += 5;
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) Current += 3;
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
            }
            else if (Current != end) ++Current;
            else
            {
                State = DeserializeStateEnum.CrashEnd;
                return;
            }
            if (Current != end)
            {
                if (endChar == Quote)
                {
                    do
                    {
                        if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote) return;
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                State = DeserializeStateEnum.StringEnter;
                                return;
                            }
                        }
                        ++Current;
                    }
                    while (true);
                }
                do
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return;
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            State = DeserializeStateEnum.StringEnter;
                            return;
                        }
                    }
                }
                while (++Current != end);
            }
            State = DeserializeStateEnum.CrashEnd;
        }
        /// <summary>
        /// 判断是否存在第一个成员
        /// </summary>
        /// <returns>是否存在第一个成员</returns>
        internal bool IsFirstObject()
        {
            if (((bits[*(byte*)Current] & DeserializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (((bits[*(byte*)Current] & DeserializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                    if (*Current == '}') ++Current;
                    else State = DeserializeStateEnum.NotFoundName;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 是否存在下一个数组数据
        /// </summary>
        /// <returns>是否存在下一个数组数据</returns>
        internal bool IsFirstArrayValue()
        {
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            if (IsBinaryMix) return true;
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current != ']') return true;
                    ++Current;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 查找名称直到结束
        /// </summary>
        internal void SearchNameEnd()
        {
            if (State == DeserializeStateEnum.Success)
            {
                while (((bits[*(byte*)Current] & DeserializeNameBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (++Current == end)
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 查找冒号
        /// </summary>
        /// <returns>是否找到</returns>
        internal byte SearchColon()
        {
            if (*Current == ':')
            {
                ++Current;
                return 1;
            }
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current == ':')
                    {
                        ++Current;
                        return 1;
                    }
                    State = DeserializeStateEnum.NotFoundColon;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return 0;
        }
        /// <summary>
        /// 判断是否存在下一个成员
        /// </summary>
        /// <returns>是否存在下一个成员</returns>
        internal bool IsNextObject()
        {
            bool isSpace = IsBinaryMix;
        START:
            if (*Current == ',')
            {
                if (++Current != end)
                {
                    if (((bits[*(byte*)Current] & DeserializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            if (((bits[*(byte*)Current] & DeserializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                            State = DeserializeStateEnum.NotFoundName;
                        }
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                }
                else State = DeserializeStateEnum.CrashEnd;
                return false;
            }
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotObject;
            return false;
        }
        /// <summary>
        /// 是否存在下一个数组数据
        /// </summary>
        /// <returns>是否存在下一个数组数据</returns>
        /// <summary>
        /// 获取成员名称第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsNextArrayValue()
        {
            if (*Current == ',')
            {
                ++Current;
                return true;
            }
            return isNextArrayValue();
        }
        /// <summary>
        /// 是否存在下一个数组数据
        /// </summary>
        /// <returns>是否存在下一个数组数据</returns>
        private bool isNextArrayValue()
        {
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current == ',')
                    {
                        ++Current;
                        return true;
                    }
                    if (*Current == ']')
                    {
                        ++Current;
                        return false;
                    }
                    State = DeserializeStateEnum.NotArrayValue;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 查找数组起始位置
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">目标数组</param>
        /// <returns>返回 0 表示未结束，返回 1 表示已结束，返回 -1 表示 null 或者错误</returns>
#if NetStandard21
        internal int SearchArray<T>(ref T[]? array)
#else
        internal int SearchArray<T>(ref T[] array)
#endif
        {
            bool isSpace = false;
        START:
            if (*Current == '[')
            {
                if (*++Current == ']')
                {
                    ++Current;
                    array = EmptyArray<T>.Array;
                    return 1;
                }
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current < end)
                    {
                        if (*Current == ']')
                        {
                            ++Current;
                            array = EmptyArray<T>.Array;
                            return 1;
                        }
                        return 0;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
                return -1;
            }
            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == NullStringValue)
            {
                array = null;
                Current += 4;
                return -1;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.CrashEnd;
            return -1;
        }
        /// <summary>
        /// Get the array length
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>返回 true 表示需要继续解析数组, array.Length 为 0 表示未知长度</returns>
#if NetStandard21
        private bool searchBinaryMixArraySize<T>([NotNullWhen(true)] ref T[]? array)
#else
        private bool searchBinaryMixArraySize<T>(ref T[] array)
#endif
        {
            int length = binaryMixArrayLength();
            if (length > 0)
            {
                array = AutoCSer.Common.GetUninitializedArray<T>(length);
                return true;
            }
            switch (length + 2)
            {
                case -2 + 2: array = EmptyArray<T>.Array; return true;
                case -1 + 2: array = null; return false;
                case 0 + 2: array = EmptyArray<T>.Array; return false;
                default: State = DeserializeStateEnum.NotArrayValue; return false;
            }
        }
        /// <summary>
        /// Get the array length
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>返回 true 表示需要继续解析数组</returns>
#if NetStandard21
        private bool searchArraySize<T>([NotNullWhen(true)]ref T[]? array)
#else
        private bool searchArraySize<T>(ref T[] array)
#endif
        {
            if (IsBinaryMix || SearchArray(ref array) == 0)
            {
                int count = 1;
                char* read = Current;
                if (endChar == ']')
                {
                    do
                    {
                        //if (*read == ',') ++count;
                        count += (*read ^ ',').logicalInversion();
                    }
                    while (*++read != ']');
                }
                else
                {
                    do
                    {
                        //if (*read == ',') ++count;
                        count += (*read ^ ',').logicalInversion();
                        if (++read == end)
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return false;
                        }
                    }
                    while (*read != ']');
                }
                array = AutoCSer.Common.GetUninitializedArray<T>(count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumberUnsigned()
        {
            if ((uint)(*Current - '0') < 10) return true;
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end) return (uint)(*Current - '0') < 10;
                State = DeserializeStateEnum.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumberSigned()
        {
            if ((uint)(*Current - '0') < 10 || *Current == '-') return true;
            space();
            if (State != DeserializeStateEnum.Success) return false;
            if (Current == end)
            {
                State = DeserializeStateEnum.CrashEnd;
                return false;
            }
            return (uint)(*Current - '0') < 10 || *Current == '-';
        }
        /// <summary>
        /// 获取成员名称第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal char GetFirstName()
        {
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            Quote = (char)0;
            return *Current;
        }
        /// <summary>
        /// 读取下一个字符
        /// </summary>
        /// <returns>字符,结束或者错误返回0</returns>
        internal char NextStringChar()
        {
            if (++Current != end)
            {
                if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\')
                    {
                        if (*++Current == 'u')
                        {
                            if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) return (char)deserializeHex4();
                        }
                        else if (*Current == 'x')
                        {
                            if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) return (char)deserializeHex2();
                        }
                        else if (Current != end) return *Current < EscapeCharSize ? escapeChars[*Current] : *Current;
                        State = DeserializeStateEnum.CrashEnd;
                        return (char)0;
                    }
                    if (*Current == '\n')
                    {
                        State = DeserializeStateEnum.StringEnter;
                        return (char)0;
                    }
                }
                return *Current;
            }
            else
            {
                State = DeserializeStateEnum.CrashEnd;
                return (char)0;
            }
        }
        /// <summary>
        /// 获取成员名称下一个字符
        /// </summary>
        /// <returns>第一个字符,0表示失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetNextName()
        {
            if (++Current != end)
            {
                int code = *Current;
                return code & (((bits[(byte)code] & DeserializeNameBit) | (code >> 8)).toLogical() - 1);
                //return ((bits[*(byte*)Current] & DeserializeNameBit) | *(((byte*)Current) + 1)) == 0 ? *Current : (char)0;
            }
            State = DeserializeStateEnum.CrashEnd;
            return 0;
        }
        /// <summary>
        /// 查找字符串引号并返回第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示null</returns>
        internal char SearchQuote()
        {
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            if (IsNull()) return Quote = (char)0;
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (++Current != end)
                {
                    if (*Current == '\'' || *Current == '"')
                    {
                        Quote = *Current;
                        return NextStringChar();
                    }
                    if (IsNull()) return Quote = (char)0;
                    State = DeserializeStateEnum.NotString;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return (char)0;
        }
        /// <summary>
        /// 查找枚举引号并返回第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示null</returns>
        internal char SearchEnumQuote()
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                return NextEnumChar();
            }
            if (*(long*)Current == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return Quote = (char)0;
            }
            if (isSpace == 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (++Current != end)
                    {
                        isSpace = 1;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotEnumChar;
            return (char)0;
        }
        /// <summary>
        /// 获取下一个枚举字符
        /// </summary>
        /// <returns>下一个枚举字符,0表示null</returns>
        internal char NextEnumChar()
        {
            if (++Current != end)
            {
                if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\' || *Current == '\n')
                    {
                        State = DeserializeStateEnum.NotEnumChar;
                        return (char)0;
                    }
                }
                return *Current;
            }
            State = DeserializeStateEnum.CrashEnd;
            return (char)0;
        }
        /// <summary>
        /// 查找下一个枚举字符
        /// </summary>
        /// <returns>下一个枚举字符,0表示null</returns>
        internal char SearchNextEnum()
        {
            do
            {
                if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\' || *Current == '\n')
                    {
                        State = DeserializeStateEnum.NotEnumChar;
                        return (char)0;
                    }
                }
                else if (*Current == ',')
                {
                    while (++Current != end)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return Quote = (char)0;
                        }
                        if (*Current != ' ')
                        {
                            if (*Current != '\\' && *Current != '\n') return *Current;
                            State = DeserializeStateEnum.NotEnumChar;
                            return (char)0;
                        }
                    }
                    break;
                }
            }
            while (++Current != end);
            State = DeserializeStateEnum.CrashEnd;
            return (char)0;
        }
        /// <summary>
        /// 查找字符串直到结束
        /// </summary>
        internal void SearchStringEnd()
        {
            if (Quote != 0 && State == DeserializeStateEnum.Success)
            {
                while (Current != end)
                {
                    if (endChar == Quote)
                    {
                        do
                        {
                            if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                            {
                                if (*Current == Quote)
                                {
                                    ++Current;
                                    return;
                                }
                                if (*Current == '\\') goto NEXT;
                                if (*Current == '\n')
                                {
                                    State = DeserializeStateEnum.StringEnter;
                                    return;
                                }
                            }
                            ++Current;
                        }
                        while (true);
                    }
                    do
                    {
                        if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                ++Current;
                                return;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                State = DeserializeStateEnum.StringEnter;
                                return;
                            }
                        }
                    }
                    while (++Current != end);
                    break;

                NEXT:
                    if (*++Current == 'u')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) Current += 5;
                        else break;
                    }
                    else if (*Current == 'x')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) Current += 3;
                        else break;
                    }
                    else if (Current != end) ++Current;
                    else break;
                }
                State = DeserializeStateEnum.CrashEnd;
            }
        }
        /// <summary>
        /// 检查强制匹配枚举值配置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckMatchEnum()
        {
            if (Config.IsMatchEnum) State = DeserializeStateEnum.NoFoundEnumValue;
            else SearchStringEnd();
        }
        /// <summary>
        /// 检查强制匹配枚举值配置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckMatchEnumIgnore()
        {
            if (Config.IsMatchEnum) State = DeserializeStateEnum.NoFoundEnumValue;
            else Ignore();
        }
        /// <summary>
        /// 检查字符串引号
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQuote()
        {
            return Quote != 0 && State == DeserializeStateEnum.Success;
        }
        /// <summary>
        /// 忽略对象
        /// </summary>
        public void Ignore()
        {
            LeftArray<KeyValue<JsonNodeTypeEnum, int>> typeArray = new LeftArray<KeyValue<JsonNodeTypeEnum, int>>(0);
        NEXTNODE:
            if (!IsBinaryMix) space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    switch (*Current & 7)
                    {
                        case '"' & 7:
                        case '\'' & 7:
                            if (*Current == '"' || *Current == '\'')
                            {
                                ignoreString();
                                if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                return;
                            }
                            goto MIX;
                        case '{' & 7:
                            if (*Current == '{')
                            {
                                if (++Current != end)
                                {
                                    if (IsFirstObject())
                                    {
                                        typeArray.Add(new KeyValue<JsonNodeTypeEnum, int>(JsonNodeTypeEnum.Dictionary, 0));
                                        goto DICTIONARYNAME;
                                    }
                                    if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                            if (*Current == '[')
                            {
                                if (++Current != end)
                                {
                                    if (IsFirstArrayValue())
                                    {
                                        typeArray.Add(new KeyValue<JsonNodeTypeEnum, int>(JsonNodeTypeEnum.Array, 0));
                                        goto NEXTNODE;
                                    }
                                    if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                            goto MIX;
                        case 't' & 7:
                            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
                            {
                                Current += 4;
                                goto CHECKNODE;
                            }
                            goto MIX;
                        case 'f' & 7:
                            if (*Current == 'f')
                            {
                                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                                {
                                    Current += 5;
                                    goto CHECKNODE;
                                }
                                //break;
                            }
                            else if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                            {
                                if (*(long*)Current == NullStringValue)
                                {
                                    Current += 4;
                                    goto CHECKNODE;
                                }
                                if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                                {
                                    Current += 9;
                                    ignoreNumber();
                                    if (State == DeserializeStateEnum.Success)
                                    {
                                        if (Current != end)
                                        {
                                            if (*Current == ')')
                                            {
                                                ++Current;
                                                goto CHECKNODE;
                                            }
                                            break;
                                        }
                                        State = DeserializeStateEnum.CrashEnd;
                                    }
                                    return;
                                }
                            }
                            goto MIX;
                        default:
                        MIX:
                            if (IsBinaryMix)
                            {
                                switch (*(byte*)Current - (byte)BinaryMixTypeEnum.Byte)
                                {
                                    case (byte)BinaryMixTypeEnum.Byte - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.False - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.True - (byte)BinaryMixTypeEnum.Byte:
                                        if (Current < end)
                                        {
                                            ++Current;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.UShort - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.Half - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(uint))
                                        {
                                            Current += sizeof(uint) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.UInt - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(uint))
                                        {
                                            Current += (sizeof(ushort) + sizeof(uint)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.ULong - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(ulong))
                                        {
                                            Current += (sizeof(ushort) + sizeof(ulong)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.UInt128 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(UInt128))
                                        {
                                            Current += (sizeof(ushort) + sizeof(UInt128)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Float - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(float))
                                        {
                                            Current += (sizeof(ushort) + sizeof(float)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Double - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(double))
                                        {
                                            Current += (sizeof(ushort) + sizeof(double)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Decimal - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(decimal))
                                        {
                                            Current += (sizeof(ushort) + sizeof(decimal)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.DateTime - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(DateTime))
                                        {
                                            Current += (sizeof(ushort) + sizeof(DateTime)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.TimeSpan - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(TimeSpan))
                                        {
                                            Current += (sizeof(ushort) + sizeof(TimeSpan)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Guid - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(Guid))
                                        {
                                            Current += (sizeof(ushort) + sizeof(Guid)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Complex - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Complex))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Complex)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Vector2 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Vector2))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Vector2)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Vector3 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Vector3))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Vector3)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Vector4 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Vector4))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Vector4)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Plane - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Plane))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Plane)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Quaternion - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Quaternion))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Quaternion)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Matrix3x2 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Matrix3x2))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Matrix3x2)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.Matrix4x4 - (byte)BinaryMixTypeEnum.Byte:
                                        if ((int)((byte*)end - (byte*)Current) >= sizeof(ushort) + sizeof(System.Numerics.Matrix4x4))
                                        {
                                            Current += (sizeof(ushort) + sizeof(System.Numerics.Matrix4x4)) >> 1;
                                            goto CHECKNODE;
                                        }
                                        break;
                                    case (byte)BinaryMixTypeEnum.None - (byte)BinaryMixTypeEnum.Byte: goto NEXTNODE;
                                    case (byte)BinaryMixTypeEnum.StringByte - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.StringByte3 - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.String - (byte)BinaryMixTypeEnum.Byte:
                                        int length = binaryMixStringLength();
                                        if (length > 0)
                                        {
                                            if (end - Current >= length)
                                            {
                                                Current += length;
                                                goto CHECKNODE;
                                            }
                                            State = DeserializeStateEnum.CrashEnd;
                                            break;
                                        }
                                        else
                                        {
                                            switch (length + 1)
                                            {
                                                case -1 + 1:
                                                case 0 + 1: goto CHECKNODE;
                                                default: return;
                                            }
                                        }
                                    case (byte)BinaryMixTypeEnum.ArrayByte - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.ArrayByte3 - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.Array - (byte)BinaryMixTypeEnum.Byte:
                                        length = binaryMixArrayLength();
                                        if (length > 0)
                                        {
                                            if (end - Current >= length)
                                            {
                                                typeArray.Add(new KeyValue<JsonNodeTypeEnum, int>(JsonNodeTypeEnum.Array, length));
                                                goto NEXTNODE;
                                            }
                                            State = DeserializeStateEnum.CrashEnd;
                                            break;
                                        }
                                        else
                                        {
                                            switch (length + 1)
                                            {
                                                case -1 + 1:
                                                case 0 + 1: goto CHECKNODE;
                                                default: State = DeserializeStateEnum.NotArrayValue; return;
                                            }
                                        }
                                    case (byte)BinaryMixTypeEnum.ArrayFalse - (byte)BinaryMixTypeEnum.Byte:
                                    case (byte)BinaryMixTypeEnum.ArrayTrue - (byte)BinaryMixTypeEnum.Byte:
                                        if (Current < end)
                                        {
                                            ++Current;
                                            int index = typeArray.Length - 1;
                                            if (index >= 0 && typeArray.Array[index].Key == JsonNodeTypeEnum.Array)
                                            {
                                                if ((typeArray.Array[index].Value -= 2) > 0) goto NEXTNODE;
                                                typeArray.Length = index;
                                                goto CHECKNODE;
                                            }
                                            State = DeserializeStateEnum.NotArrayValue;
                                            return;
                                        }
                                        break;
                                    default: goto NUMBER;
                                }
                                State = DeserializeStateEnum.CrashEnd;
                                return;
                            }
                        NUMBER:
                            ignoreNumber();
                            if (State == DeserializeStateEnum.Success) goto CHECKNODE;
                            return;
                    }
                    State = DeserializeStateEnum.UnknownValue;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return;

        DICTIONARYNAME:
            if (*Current == '\'' || *Current == '"') ignoreString();
            else ignoreName();
            if (State == DeserializeStateEnum.Success && SearchColon() != 0) goto NEXTNODE;
            return;

        CHECKNODE:
            if (typeArray.Length != 0)
            {
                int index = typeArray.Length - 1;
                switch (typeArray.Array[index].Key)
                {
                    case JsonNodeTypeEnum.Dictionary:
                        if (IsNextObject()) goto DICTIONARYNAME;
                        typeArray.Length = index;
                        goto CHECKNODE;
                    case JsonNodeTypeEnum.Array:
                        if (typeArray.Array[index].Value != 0 ? --typeArray.Array[index].Value != 0 : IsNextArrayValue()) goto NEXTNODE;
                        typeArray.Length = index;
                        goto CHECKNODE;
                }
            }
        }
        /// <summary>
        /// 忽略字符串
        /// </summary>
        private void ignoreString()
        {
            Quote = *Current;
            if (++Current != end)
            {
                if (searchEscape() == 0)
                {
                    if (State == DeserializeStateEnum.Success) ++Current;
                    return;
                }
            NEXT:
                if (*++Current == 'u')
                {
                    if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) Current += 5;
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else if (*Current == 'x')
                {
                    if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) Current += 3;
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return;
                    }
                }
                else if (Current != end) ++Current;
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return;
                }
                if (Current != end)
                {
                    if (endChar == Quote)
                    {
                        do
                        {
                            if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                            {
                                if (*Current == Quote)
                                {
                                    ++Current;
                                    return;
                                }
                                if (*Current == '\\') goto NEXT;
                                if (*Current == '\n')
                                {
                                    State = DeserializeStateEnum.StringEnter;
                                    return;
                                }
                            }
                            ++Current;
                        }
                        while (true);
                    }
                    do
                    {
                        if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                ++Current;
                                return;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                State = DeserializeStateEnum.StringEnter;
                                return;
                            }
                        }
                    }
                    while (++Current != end);
                }
            }
            State = DeserializeStateEnum.CrashEnd;
        }
        /// <summary>
        /// 忽略数字
        /// </summary>
        private void ignoreNumber()
        {
            if (((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) == 0)
            {
                while (++Current != end && ((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) == 0) ;
                return;
            }
            State = DeserializeStateEnum.NotNumber;
        }
        /// <summary>
        /// 忽略成员名称
        /// </summary>
        private void ignoreName()
        {
            while (++Current != end)
            {
                if (((bits[*(byte*)Current] & DeserializeNameBit) | *(((byte*)Current) + 1)) != 0) return;
            }
            State = DeserializeStateEnum.CrashEnd;
        }
        /// <summary>
        /// 查找对象起始位置
        /// </summary>
        /// <returns>是否查找到</returns>
        internal bool SearchObject()
        {
            bool isSpace = IsBinaryMix;
        START:
            if (*Current == '{')
            {
                ++Current;
                return true;
            }
            if (*(long*)Current == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return false;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotObject;
            return false;
        }
        /// <summary>
        /// 查找字典起始位置
        /// </summary>
        /// <returns>是否查找到</returns>
        private byte searchDictionary()
        {
            bool isSpace = IsBinaryMix;
        START:
            if (*Current == '{')
            {
                ++Current;
                return 1;
            }
            if (*Current == '[')
            {
                ++Current;
                return 2;
            }
            if (*(long*)Current == NullStringValue && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return 0;
            }
            if (!isSpace)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        isSpace = true;
                        goto START;
                    }
                    State = DeserializeStateEnum.CrashEnd;
                }
            }
            else State = DeserializeStateEnum.NotObject;
            return 0;
        }
        /// <summary>
        /// 对象是否结束
        /// </summary>
        /// <returns>对象是否结束</returns>
        private byte isDictionaryObjectEnd()
        {
            if (*Current == '}')
            {
                ++Current;
                return 1;
            }
            space();
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current == '}')
                    {
                        ++Current;
                        return 1;
                    }
                    return 0;
                }
                State = DeserializeStateEnum.CrashEnd;
            }
            return 1;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public bool IsName(ref AutoCSer.Memory.Pointer names)
        {
            int length = *(short*)names.Current;
            if (length != 0)
            {
                names.CurrentIndex += sizeof(short);
                if (AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)Current, names.Current, length) && (int)((byte*)end - (byte*)Current) >= length)
                {
                    Current = (char*)((byte*)Current + length);
                    names.CurrentIndex += length;
                    return true;
                }
            }
            else if (*Current == '}')
            {
                names.ByteSize = -1;
                ++Current;
            }
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsName(JsonDeserializer jsonDeserializer, ref AutoCSer.Memory.Pointer names)
        {
            return jsonDeserializer.IsName(ref names);
        }
        /// <summary>
        /// 移动到下一个名称
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool NextNameIndex(JsonDeserializer jsonDeserializer, ref AutoCSer.Memory.Pointer names)
        {
            if (jsonDeserializer.State == DeserializeStateEnum.Success)
            {
                ++names.ByteSize;
                return true;
            }
            names.ByteSize = -1;
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        internal delegate bool NameDelegate(JsonDeserializer jsonDeserializer, ref AutoCSer.Memory.Pointer names);
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        internal void NoConstructorIgnoreObject()
        {
            --Current;
            ignoreObject();
        }
        /// <summary>
        /// 忽略对象
        /// </summary>
        private void ignoreObject()
        {
            if (++Current != end)
            {
                if (IsFirstObject())
                {
                    if (*Current == '\'' || *Current == '"') ignoreString();
                    else ignoreName();
                    if (State == DeserializeStateEnum.Success && SearchColon() != 0)
                    {
                        Ignore();
                        while (State == DeserializeStateEnum.Success && IsNextObject())
                        {
                            if (*Current == '\'' || *Current == '"') ignoreString();
                            else ignoreName();
                            if (State == DeserializeStateEnum.Success && SearchColon() != 0) Ignore();
                            else return;
                        }
                    }
                }
            }
            else State = DeserializeStateEnum.CrashEnd;
        }

        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否 null</returns>
        private bool deserialize(out bool value)
        {
        START:
            switch (*Current & 7)
            {
                case 'f' & 7:
                    if (*Current == 'f')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)
                            && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                        {
                            value = false;
                            Current += 5;
                            return false;
                        }
                    }
                    else if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == NullStringValue)
                    {
                        value = false;
                        Current += 4;
                        return true;
                    }
                    break;
                case 't' & 7:
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
                    {
                        value = true;
                        Current += 4;
                        return false;
                    }
                    break;
                case '0' & 7:
                    if (*Current == '0')
                    {
                        value = false;
                        ++Current;
                        return false;
                    }
                    break;
                case '1' & 7:
                    if (*Current == '1')
                    {
                        value = true;
                        ++Current;
                        return false;
                    }
                    break;
                case '"' & 7:
                case '\'' & 7:
                    if ((*Current == '"' || *Current == '\'') && Quote <= 1)
                    {
                        Quote = *Current;
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            bool isNull = deserialize(out value);
                            if (!isNull)
                            {
                                if (Current < end)
                                {
                                    if (*Current == Quote) ++Current;
                                    else State = DeserializeStateEnum.NotBool;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                                return false;
                            }
                        }
                    }
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            Quote = (char)1;
                            goto START;
                        }
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    return value = false;
                }
            }
            State = DeserializeStateEnum.NotBool;
            return value = false;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <returns></returns>
        private uint deserializeHex32()
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                else
                {
                    State = DeserializeStateEnum.NotHex;
                    return 0;
                }
            }
            uint value = number;
            if (++Current != end)
            {
                int count = 7;
                if (isEndHex)
                {
                    do
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                            else return value;
                        }
                        if (count != 0)
                        {
                            value = (value << 4) + number;
                            --count;
                        }
                        else
                        {
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return value;
                        }
                    }
                    while (++Current != end);
                }
                else
                {
                    do
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                            else return value;
                        }
                        if (count != 0)
                        {
                            value = (value << 4) + number;
                            ++Current;
                            --count;
                        }
                        else
                        {
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return value;
                        }
                    }
                    while (true);
                }
            }
            return value;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private uint deserializeUInt32(uint value)
        {
            uint number;
            int count = 9;
            if (isEndDigital)
            {
                while ((number = (uint)(*Current - '0')) < 10)
                {
                    switch(count)
                    {
                        case 0:
                            ERROR:
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return value;
                        case 1:
                            if (value <= uint.MaxValue / 10)
                            {
                                value = value * 10 + number;
                                if (value >= 10)
                                {
                                    if (++Current == end) return value;
                                    count = 0;
                                    break;
                                }
                            }
                            goto ERROR;
                        default:
                            value = value * 10 + number;
                            if (++Current == end) return value;
                            --count;
                            break;
                    }
                }
                return value;
            }
            else
            {
                while ((number = (uint)(*Current - '0')) < 10)
                {
                    switch (count)
                    {
                        case 0:
                        ERROR:
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return value;
                        case 1:
                            if (value <= uint.MaxValue / 10)
                            {
                                value = value * 10 + number;
                                if (value >= 10)
                                {
                                    ++Current;
                                    count = 0;
                                    break;
                                }
                            }
                            goto ERROR;
                        default:
                            value = value * 10 + number;
                            ++Current;
                            --count;
                            break;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <returns></returns>
        private uint primitiveDeserializeUInt()
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if (*Current == '"' || *Current == '\'')
                            {
                                Quote = *Current;
                                if (++Current != end)
                                {
                                    if ((number = (uint)(*Current - '0')) < 10)
                                    {
                                        if (++Current != end)
                                        {
                                            if (number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    ++Current;
                                                    return 0;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) number = deserializeHex32();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else number = deserializeUInt32(number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote)
                                                    {
                                                        ++Current;
                                                        return number;
                                                    }
                                                    State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.NotNumber;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                            }
                            else State = DeserializeStateEnum.NotNumber;
                            return 0;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return 0;
                    }
                }
                else return 0;
            }
            if (++Current != end)
            {
                if (number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) return deserializeHex32();
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    return 0;
                }
                return deserializeUInt32(number);
            }
            return number;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <returns></returns>
        private DeserializeInt primitiveDeserializeInt()
        {
            DeserializeInt value = new DeserializeInt((uint)(*Current - '0'));
            if (value.Number > 9)
            {
                if (*Current != '-')
                {
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            if ((value.Number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current == '"' || *Current == '\'')
                                {
                                    Quote = *Current;
                                    if (++Current != end)
                                    {
                                        if ((value.Number = (uint)(*Current - '0')) > 9)
                                        {
                                            if (*Current == '-')
                                            {
                                                if (++Current != end)
                                                {
                                                    if ((value.Number = (uint)(*Current - '0')) < 10) value.Sign = 1;
                                                    else
                                                    {
                                                        State = DeserializeStateEnum.NotNumber;
                                                        return value;
                                                    }
                                                }
                                                else
                                                {
                                                    State = DeserializeStateEnum.CrashEnd;
                                                    return value;
                                                }
                                            }
                                            else
                                            {
                                                State = DeserializeStateEnum.NotNumber;
                                                return value;
                                            }
                                        }
                                        if (++Current != end)
                                        {
                                            if (value.Number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    ++Current;
                                                    return value;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value.Number = deserializeHex32();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value.Number = deserializeUInt32(value.Number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.CrashEnd;
                                    return value;
                                }
                                if (*Current == '-')
                                {
                                    if (++Current != end)
                                    {
                                        if ((value.Number = (uint)(*Current - '0')) <= 10) value.Sign = 1;
                                        else
                                        {
                                            State = DeserializeStateEnum.NotNumber;
                                            return value;
                                        }
                                    }
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return value;
                                    }
                                }
                                else
                                {
                                    State = DeserializeStateEnum.NotNumber;
                                    return value;
                                }
                            }
                        }
                        else
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return value;
                        }
                    }
                    else return value;
                }
                else if (++Current != end)
                {
                    if ((value.Number = (uint)(*Current - '0')) < 10) value.Sign = 1;
                    else
                    {
                        State = DeserializeStateEnum.NotNumber;
                        return value;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return value;
                }
            }
            if (++Current != end)
            {
                if (value.Number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value.Number = deserializeHex32();
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                }
                else value.Number = deserializeUInt32(value.Number);
            }
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <returns></returns>
        private DeserializeInt primitiveDeserializeUIntNull()
        {
            DeserializeInt value = new DeserializeInt((uint)(*Current - '0'));
            if (value.Number > 9)
            {
                if (IsNull())
                {
                    value.IsNull = 1;
                    return value;
                }
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if ((value.Number = (uint)(*Current - '0')) > 9)
                        {
                            if (IsNull()) value.IsNull = 1;
                            else if (*Current == '"' || *Current == '\'')
                            {
                                Quote = *Current;
                                if (++Current != end)
                                {
                                    if ((value.Number = (uint)(*Current - '0')) < 10)
                                    {
                                        if (++Current != end)
                                        {
                                            if (value.Number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    ++Current;
                                                    return value;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value.Number = deserializeHex32();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value.Number = deserializeUInt32(value.Number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.NotNumber;
                                }
                                else State = DeserializeStateEnum.CrashEnd;
                            }
                            else State = DeserializeStateEnum.NotNumber;
                            return value;
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return value;
                    }
                }
                else return value;
            }
            if (++Current != end)
            {
                if (value.Number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value.Number = deserializeHex32();
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                }
                else value.Number = deserializeUInt32(value.Number);
            }
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <returns></returns>
        private DeserializeInt primitiveDeserializeIntNull()
        {
            DeserializeInt value = new DeserializeInt((uint)(*Current - '0'));
            if (value.Number > 9)
            {
                if (IsNull())
                {
                    value.IsNull = 1;
                    return value;
                }
                if (*Current != '-')
                {
                    space();
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            if ((value.Number = (uint)(*Current - '0')) > 9)
                            {
                                if (IsNull())
                                {
                                    value.IsNull = 1;
                                    return value;
                                }
                                if (*Current == '"' || *Current == '\'')
                                {
                                    Quote = *Current;
                                    if (++Current != end)
                                    {
                                        if ((value.Number = (uint)(*Current - '0')) > 9)
                                        {
                                            if (*Current == '-')
                                            {
                                                if (++Current != end)
                                                {
                                                    if ((value.Number = (uint)(*Current - '0')) < 10) value.Sign = 1;
                                                    else
                                                    {
                                                        State = DeserializeStateEnum.NotNumber;
                                                        return value;
                                                    }
                                                }
                                                else
                                                {
                                                    State = DeserializeStateEnum.CrashEnd;
                                                    return value;
                                                }
                                            }
                                            else
                                            {
                                                State = DeserializeStateEnum.NotNumber;
                                                return value;
                                            }
                                        }
                                        if (++Current != end)
                                        {
                                            if (value.Number == 0)
                                            {
                                                if (*Current == Quote)
                                                {
                                                    ++Current;
                                                    return value;
                                                }
                                                if (*Current == 'x')
                                                {
                                                    if (++Current != end) value.Number = deserializeHex32();
                                                    else State = DeserializeStateEnum.CrashEnd;
                                                }
                                                else State = DeserializeStateEnum.NotNumber;
                                            }
                                            else value.Number = deserializeUInt32(value.Number);
                                            if (State == DeserializeStateEnum.Success)
                                            {
                                                if (Current != end)
                                                {
                                                    if (*Current == Quote) ++Current;
                                                    else State = DeserializeStateEnum.NotNumber;
                                                }
                                                else State = DeserializeStateEnum.CrashEnd;
                                            }
                                        }
                                        else State = DeserializeStateEnum.CrashEnd;
                                    }
                                    else State = DeserializeStateEnum.CrashEnd;
                                    return value;
                                }
                                if (*Current == '-')
                                {
                                    if (++Current != end)
                                    {
                                        if ((value.Number = (uint)(*Current - '0')) < 10) value.Sign = 1;
                                        else
                                        {
                                            State = DeserializeStateEnum.NotNumber;
                                            return value;
                                        }
                                    }
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return value;
                                    }
                                }
                                else
                                {
                                    State = DeserializeStateEnum.NotNumber;
                                    return value;
                                }
                            }
                        }
                        else
                        {
                            State = DeserializeStateEnum.CrashEnd;
                            return value;
                        }
                    }
                    else return value;
                }
                else if (++Current != end)
                {
                    if ((value.Number = (uint)(*Current - '0')) < 10) value.Sign = 1;
                    else
                    {
                        State = DeserializeStateEnum.NotNumber;
                        return value;
                    }
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return value;
                }
            }
            if (++Current != end)
            {
                if (value.Number == 0)
                {
                    if (*Current == 'x')
                    {
                        if (++Current != end) value.Number = deserializeHex32();
                        else State = DeserializeStateEnum.CrashEnd;
                    }
                }
                else value.Number = deserializeUInt32(value.Number);
            }
            return value;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <returns>数字</returns>
        private ulong deserializeHex64()
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                else
                {
                    State = DeserializeStateEnum.NotHex;
                    return 0;
                }
            }
            if (++Current != end)
            {
                uint high = number;
                char* end32 = Current + 7;
                if (end32 > end) end32 = end;
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                        else return high;
                    }
                    high <<= 4;
                    high += number;
                }
                while (++Current != end32);
                if (Current != end)
                {
                    uint low = 0;
                    int bits = 32;
                    if (isEndHex)
                    {
                        do
                        {
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                                else return low | ((ulong)high << (32 - bits));
                            }
                            if (bits != 0)
                            {
                                low <<= 4;
                                bits -= 4;
                                low += number;
                            }
                            else
                            {
                                State = DeserializeStateEnum.NumberOutOfRange;
                                return low | ((ulong)high << 32);
                            }
                        }
                        while (++Current != end);
                        return low | ((ulong)high << (32 - bits));
                    }
                    do
                    {
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            if ((number = (number - ('A' - '0')) & 0xffdfU) < 6) number += 10;
                            else return low | ((ulong)high << (32 - bits));
                        }
                        if (bits != 0)
                        {
                            low <<= 4;
                            ++Current;
                            bits -= 4;
                            low += number;
                        }
                        else
                        {
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return low | ((ulong)high << 32);
                        }
                    }
                    while (true);
                }
                return high;
            }
            return number;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private ulong deserializeUInt64(uint value)
        {
            char* end32 = Current + 8;
            if (end32 > end) end32 = end;
            uint number;
            do
            {
                if ((number = (uint)(*Current - '0')) < 10) value = value * 10 + number;
                else return value;
            }
            while (++Current != end32);
            if (Current != end)
            {
                ulong value64 = value;
                int count = 11;
                if (isEndDigital)
                {
                    do
                    {
                        if ((number = (uint)(*Current - '0')) < 10)
                        {
                            switch (count)
                            {
                                case 0:
                                ERROR:
                                    State = DeserializeStateEnum.NumberOutOfRange;
                                    return value64;
                                case 1:
                                    if (value64 <= ulong.MaxValue / 10)
                                    {
                                        value64 = value64 * 10 + number;
                                        if (value64 >= 10)
                                        {
                                            if (++Current == end) return value64;
                                            count = 0;
                                            break;
                                        }
                                    }
                                    goto ERROR;
                                default:
                                    value64 = value64 * 10 + number;
                                    if (++Current == end) return value64;
                                    --count;
                                    break;
                            }
                        }
                        else return value64;
                    }
                    while (true);
                }
                while ((number = (uint)(*Current - '0')) < 10)
                {
                    switch (count)
                    {
                        case 0:
                        ERROR:
                            State = DeserializeStateEnum.NumberOutOfRange;
                            return value64;
                        case 1:
                            if (value64 <= ulong.MaxValue / 10)
                            {
                                value64 = value64 * 10 + number;
                                if (value64 >= 10)
                                {
                                    ++Current;
                                    count = 0;
                                    break;
                                }
                            }
                            goto ERROR;
                        default:
                            value64 = value64 * 10 + number;
                            ++Current;
                            --count;
                            break;
                    }
                }
                return value64;
            }
            return value;
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private uint deserializeHex2()
        {
            uint code = (uint)(*++Current - '0'), number = (uint)(*++Current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint deserializeHex4()
        {
            uint code = (uint)(*++Current - '0'), number = (uint)(*++Current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code <<= 12;
            code += (number << 8);
            if ((number = (uint)(*++Current - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code += (number << 4);
            number = (uint)(*++Current - '0');
            return code + (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
        }
        /// <summary>
        /// 是否 -Infinity
        /// </summary>
        /// <returns>是否 -Infinity</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private NumberTypeEnum isNegativeInfinity()
        {
            if (((*(long*)(Current + 1) ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(Current + 5) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0
                && (int)((byte*)end - (byte*)Current) >= 9 * sizeof(char))
            {
                Current += 9;
                return NumberTypeEnum.NegativeInfinity;
            }
            State = DeserializeStateEnum.NotNumber;
            return NumberTypeEnum.Error;
        }
        /// <summary>
        /// 是否非数字 NaN / Infinity
        /// </summary>
        /// <returns>是否非数字NaN</returns>
        private NumberTypeEnum isNaNPositiveInfinity()
        {
            if (*Current == 'N')
            {
                if (*(int*)(Current + 1) == ('a' + ('N' << 16)) && (int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                {
                    Current += 3;
                    return NumberTypeEnum.NaN;
                }
            }
            else if (*Current == 'I')
            {
                if (((*(long*)Current ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(Current + 4) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0
                    && (int)((byte*)end - (byte*)Current) >= 8 * sizeof(char))
                {
                    Current += 8;
                    return NumberTypeEnum.PositiveInfinity;
                }
            }
            State = DeserializeStateEnum.NotNumber;
            return NumberTypeEnum.Error;
        }
        /// <summary>
        /// 查找数字结束位置
        /// </summary>
        /// <param name="numberEnd">数字结束位置</param>
        /// <returns>数字类型</returns>
        private NumberTypeEnum searchNumber(ref char* numberEnd)
        {
            if (((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current != end)
                            {
                                numberEnd = Current;
                                if (endChar == Quote)
                                {
                                    while (*numberEnd != Quote) ++numberEnd;
                                }
                                else
                                {
                                    while (*numberEnd != Quote)
                                    {
                                        if (++numberEnd == end)
                                        {
                                            State = DeserializeStateEnum.CrashEnd;
                                            return NumberTypeEnum.Error;
                                        }
                                    }
                                }
                                return NumberTypeEnum.String;
                            }
                            else
                            {
                                State = DeserializeStateEnum.CrashEnd;
                                return NumberTypeEnum.Error;
                            }
                        }
                        if (*Current == '{') return NumberTypeEnum.Object;
                        if (((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) != 0) return isNaNPositiveInfinity();
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return NumberTypeEnum.Error;
                    }
                }
                else return NumberTypeEnum.Error;
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & DeserializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & DeserializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            return (int)(numberEnd - Current) != 1 || *Current != '-' ? NumberTypeEnum.Number : isNegativeInfinity();
        }
        /// <summary>
        /// 查找数字结束位置
        /// </summary>
        /// <param name="numberEnd">数字结束位置</param>
        /// <returns>数字类型</returns>
        private NumberTypeEnum searchNumberNull(ref char* numberEnd)
        {
            if (((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                if (IsNull()) return NumberTypeEnum.Null;
                space();
                if (State == DeserializeStateEnum.Success)
                {
                    if (Current != end)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current != end)
                            {
                                numberEnd = Current;
                                if (endChar == Quote)
                                {
                                    while (*numberEnd != Quote) ++numberEnd;
                                }
                                else
                                {
                                    while (*numberEnd != Quote)
                                    {
                                        if (++numberEnd == end)
                                        {
                                            State = DeserializeStateEnum.CrashEnd;
                                            return NumberTypeEnum.Error;
                                        }
                                    }
                                }
                                return NumberTypeEnum.String;
                            }
                            else
                            {
                                State = DeserializeStateEnum.CrashEnd;
                                return NumberTypeEnum.Error;
                            }
                        }
                        if (((bits[*(byte*)Current] & DeserializeNumberBit) | *(((byte*)Current) + 1)) != 0)
                        {
                            return IsNull() ? NumberTypeEnum.Null : isNaNPositiveInfinity();
                        }
                    }
                    else
                    {
                        State = DeserializeStateEnum.CrashEnd;
                        return NumberTypeEnum.Error;
                    }
                }
                else return NumberTypeEnum.Error;
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & DeserializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & DeserializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            return (int)(numberEnd - Current) != 1 || *Current != '-' ? NumberTypeEnum.Number : isNegativeInfinity();
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void deserializeInt32Next(ref uint value)
        {
            uint number;
            if (isEndDigital)
            {
                while ((number = (uint)(*Current - '0')) < 10)
                {
                    value = value * 10 + number;
                    if (++Current == end) return;
                }
            }
            else
            {
                while ((number = (uint)(*Current - '0')) < 10)
                {
                    value = value * 10 + number;
                    ++Current;
                }
            }
        }
        /// <summary>
        /// 时间片段值解析
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private uint deserializeDateTime()
        {
            uint high = (uint)(*(Current + 1) - '0');
            if (high < 10)
            {
                uint low = (uint)(*(Current + 2) - '0');
                if (low < 10)
                {
                    Current += 3;
                    return high * 10 + low;
                }
                Current += 2;
                return high;
            }
            return uint.MinValue;
        }
        /// <summary>
        /// 时间时钟周期解析
        /// </summary>
        /// <returns></returns>
        private long deserializeDateTimeTicks()
        {
            uint high = (uint)(*(Current + 1) - '0');
            if (high < 10)
            {
                uint low = (uint)(*(Current + 2) - '0');
                if (low < 10)
                {
                    int size = (int)(end - Current);
                    high = high * 10 + low;
                    if (size > 4 && (low = (uint)(*(Current + 3) - '0')) < 10)
                    {
                        high = high * 10 + low;
                        if (size > 5 && (low = (uint)(*(Current + 4) - '0')) < 10)
                        {
                            high = high * 10 + low;
                            if (size > 6 && (low = (uint)(*(Current + 5) - '0')) < 10)
                            {
                                high = high * 10 + low;
                                if (size > 7 && (low = (uint)(*(Current + 6) - '0')) < 10)
                                {
                                    high = high * 10 + low;
                                    if (size > 8 && (low = (uint)(*(Current + 7) - '0')) < 10)
                                    {
                                        Current += 8;
                                        return high * 10 + low;
                                    }
                                    else Current += 7;
                                    return high * 10;
                                }
                                else Current += 6;
                                return high * 100;
                            }
                            else Current += 5;
                            return high * 1000;
                        }
                        else Current += 4;
                        return high * 10000;
                    }
                    else Current += 3;
                    return high * 100000;
                }
                else Current += 2;
                return high * 1000000;
            }
            return long.MinValue;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool deserializeDateTimeMillisecond(ref DateTime value)
        {
            long millisecond = 0;
            JsonDeserialize(ref millisecond);
            if (State == DeserializeStateEnum.Success)
            {
                if (Current != end)
                {
                    if (*Current == ')')
                    {
                        value = JavaScriptLocalMinTime.AddTicks(millisecond * TimeSpan.TicksPerMillisecond);
                        ++Current;
                        return true;
                    }
                    State = DeserializeStateEnum.NotDateTime;
                }
                else State = DeserializeStateEnum.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int deserializeDateTimeString(ref DateTime value)
        {
            switch (*++Current & 15)
            {
                case '0' & 15:
                case '1' & 15:
                case '2' & 15:
                case '3' & 15:
                case '4' & 15:
                case '5' & 15:
                case '6' & 15:
                case '7' & 15:
                case '8' & 15:
                case '9' & 15:
                    uint year = (uint)(*Current - '0');
                    if (year < 10)
                    {
                        ++Current;
                        deserializeInt32Next(ref year);
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                        {
                            switch (*Current)
                            {
                                case '/':
                                case '-':
                                    Quote = *Current;
                                    uint month = deserializeDateTime();
                                    if ((month - 1U) <= (12U - 1U) && *Current == Quote && (int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                                    {
                                        uint day = deserializeDateTime();
                                        if ((day - 1U) <= (31U - 1U) && Current < end)
                                        {
                                            try
                                            {
                                                switch (*Current)
                                                {
                                                    case ' ':
                                                    case 'T':
                                                        if ((int)((byte*)end - (byte*)Current) >= 7 * sizeof(char))
                                                        {
                                                            uint hour = deserializeDateTime();
                                                            if (hour < 24 && *Current == ':')
                                                            {
                                                                uint minute = deserializeDateTime();
                                                                if (minute < 60)
                                                                {
                                                                    uint second;
                                                                    if (*Current == ':')
                                                                    {
                                                                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return 2;
                                                                        second = deserializeDateTime();
                                                                        if (second >= 60 || Current >= end) return 2;
                                                                    }
                                                                    else second = 0;

                                                                    long ticks;
                                                                    switch (*Current)
                                                                    {
                                                                        case ' ':
                                                                        case '.':
                                                                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return 2;
                                                                            ticks = deserializeDateTimeTicks();
                                                                            if (Current >= end && ticks < 0) return 2;
                                                                            break;
                                                                        default: ticks = 0; break;
                                                                    }

                                                                    bool zone;
                                                                    switch (*Current & 3)
                                                                    {
                                                                        case 'Z' & 3:
                                                                            if (*Current == 'Z')
                                                                            {
                                                                                ++Current;
                                                                                value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, DateTimeKind.Utc).AddTicks(ticks);
                                                                                return 0;
                                                                            }
                                                                            goto TICKS;
                                                                        case '+' & 3:
                                                                            if (*Current == '+')
                                                                            {
                                                                                if ((int)((byte*)end - (byte*)Current) < 7 * sizeof(char)) return 2;
                                                                                zone = true;
                                                                                break;
                                                                            }
                                                                            goto TICKS;
                                                                        case '-' & 3:
                                                                            if (*Current == '-')
                                                                            {
                                                                                if ((int)((byte*)end - (byte*)Current) < 7 * sizeof(char)) return 2;
                                                                                zone = false;
                                                                                break;
                                                                            }
                                                                            goto TICKS;
                                                                        default:
                                                                        TICKS:
                                                                            value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second).AddTicks(ticks);
                                                                            return 0;
                                                                    }
                                                                    uint zoneHour = deserializeDateTime();
                                                                    if (zoneHour < 24 && *Current == ':')
                                                                    {
                                                                        uint zoneMinute = deserializeDateTime();
                                                                        if (zoneMinute < 60)
                                                                        {
                                                                            long zoneTicks = (int)(zoneHour * 60 + zoneMinute) * TimeSpan.TicksPerMinute;
                                                                            if (zone) zoneTicks = -zoneTicks;
                                                                            value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, DateTimeKind.Local)
                                                                               .AddTicks(zoneTicks + ticks + Date.LocalTimeTicks);
                                                                            return 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        value = new DateTime((int)year, (int)month, (int)day);
                                                        return 0;
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case '/' & 15:
                    if (*(long*)Current == '/' + ('D' << 16) + ((long)'a' << 32) + ((long)'t' << 48) && *(int*)(Current + 4) == 'e' + ('(' << 16))
                    {
                        Current += 6;
                        if (deserializeDateTimeMillisecond(ref value) && *Current == '/')
                        {
                            ++Current;
                            return 1;
                        }
                    }
                    break;
            }
            return 2;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否 null</returns>
        private bool deserializeDateTime(ref DateTime value)
        {
        START:
            switch (*Current)
            {
                case '"':
                case '\'':
                    if (Quote <= 1)
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 10 * sizeof(char))
                        {
                            char* start = Current;
                            switch (deserializeDateTimeString(ref value))
                            {
                                case 0:
                                    if (Current < end)
                                    {
                                        if (*Current == *start)
                                        {
                                            ++Current;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        State = DeserializeStateEnum.CrashEnd;
                                        return false;
                                    }
                                    break;
                                case 1:
                                    if (Current < end)
                                    {
                                        if (*Current == *start) ++Current;
                                        else State = DeserializeStateEnum.NotDateTime;
                                    }
                                    else State = DeserializeStateEnum.CrashEnd;
                                    return false;
                            }
                            Current = start;
                            if (JsonSerializer.CustomConfig.Deserialize(this, ref value))
                            {
                                State = DeserializeStateEnum.Success;
                                return false;
                            }
                        }
                        Quote = (char)1;
                    }
                    break;
                case 'n':
                    if ((int)((byte*)end - (byte*)Current) >= 11 * sizeof(char))
                    {
                        if (((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                        {
                            Current += 9;
                            deserializeDateTimeMillisecond(ref value);
                            return false;
                        }
                    }
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == NullStringValue)
                    {
                        Current += 4;
                        return true;
                    }
                    Quote = (char)1;
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (State == DeserializeStateEnum.Success)
                    {
                        if (Current != end)
                        {
                            Quote = (char)1;
                            goto START;
                        }
                        State = DeserializeStateEnum.CrashEnd;
                    }
                    return false;
                }
            }
            State = DeserializeStateEnum.NotDateTime;
            return false;
        }
        /// <summary>
        /// 临时字符串解析（不处理转义）
        /// </summary>
        /// <returns></returns>
        internal string GetQuoteStringBuffer()
        {
            Quote = *Current;
            if (++Current != end)
            {
                char* start = Current;
                if (endChar == Quote)
                {
                    while (*Current != Quote) ++Current;
                }
                else
                {
                    do
                    {
                        if (Current != end)
                        {
                            if (*Current != Quote) ++Current;
                            else break;
                        }
                        else return string.Empty;
                    }
                    while (true);
                }
                int byteSize = (int)((byte*)Current - (byte*)start);
                string stringBuffer = getStringBuffer(byteSize);
                if (stringBuffer.Length != 0)
                {
                    fixed (char* bufferFixed = stringBuffer)
                    {
                        AutoCSer.Common.CopyTo(start, bufferFixed, byteSize);
                        AutoCSer.Common.Clear((byte*)bufferFixed + byteSize, (stringBuffer.Length << 1) - byteSize);
                    }
                    ++Current;
                    return stringBuffer;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">data</param>
        private void deserialize(ref GuidCreator value)
        {
            if ((int)((byte*)end - (byte*)Current) >= 38 * sizeof(char))
            {
                Quote = *Current;
                value.Byte3 = (byte)deserializeHex2();
                value.Byte2 = (byte)deserializeHex2();
                value.Byte1 = (byte)deserializeHex2();
                value.Byte0 = (byte)deserializeHex2();
                if (*++Current == '-')
                {
                    value.Byte45 = (ushort)deserializeHex4();
                    if (*++Current == '-')
                    {
                        value.Byte67 = (ushort)deserializeHex4();
                        if (*++Current == '-')
                        {
                            value.Byte8 = (byte)deserializeHex2();
                            value.Byte9 = (byte)deserializeHex2();
                            if (*++Current == '-')
                            {
                                value.Byte10 = (byte)deserializeHex2();
                                value.Byte11 = (byte)deserializeHex2();
                                value.Byte12 = (byte)deserializeHex2();
                                value.Byte13 = (byte)deserializeHex2();
                                value.Byte14 = (byte)deserializeHex2();
                                value.Byte15 = (byte)deserializeHex2();
                                if (*++Current == Quote)
                                {
                                    ++Current;
                                    return;
                                }
                            }
                        }
                    }
                }
                State = DeserializeStateEnum.NotGuid;
            }
            else State = DeserializeStateEnum.CrashEnd;
        }

        /// <summary>
        /// 查找字符串中的转义符
        /// </summary>
        private byte searchEscape()
        {
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return 0;
                        if (*Current == '\\') return 1;
                        if (*Current == '\n')
                        {
                            State = DeserializeStateEnum.StringEnter;
                            return 0;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote) return 0;
                    if (*Current == '\\') return 1;
                    if (*Current == '\n')
                    {
                        State = DeserializeStateEnum.StringEnter;
                        return 0;
                    }
                }
            }
            while (++Current != end);
            State = DeserializeStateEnum.CrashEnd;
            return 0;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <returns>字符串,失败返回null</returns>
#if NetStandard21
        private string? deserializeString()
#else
        private string deserializeString()
#endif
        {
            Quote = *Current;
            if (++Current != end)
            {
                char* start = Current;
                if (searchEscape() == 0) return State == DeserializeStateEnum.Success ? new string(start, 0, (int)(Current++ - start)) : null;
                if (Config.IsTempString)
                {
                    char* writeEnd = deserializeEscape();
                    return writeEnd != null ? new string(start, 0, (int)(writeEnd - start)) : null;
                }
                return deserializeEscape(start);
            }
            State = DeserializeStateEnum.CrashEnd;
            return null;
        }
        /// <summary>
        /// 获取转义后的字符串长度
        /// </summary>
        /// <returns>String length</returns>
        private int deserializeEscapeSize()
        {
            char* start = Current;
            int length = 0;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                {
                    length += 5;
                    Current += 5;
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return 0;
                }
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                {
                    length += 3;
                    Current += 3;
                }
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return 0;
                }
            }
            else if (Current != end)
            {
                ++length;
                ++Current;
            }
            else
            {
                State = DeserializeStateEnum.CrashEnd;
                return 0;
            }
            if (Current != end)
            {
                if (endChar == Quote)
                {
                    do
                    {
                        if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                length = (int)(Current - start) - length;
                                Current = start;
                                return length;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                State = DeserializeStateEnum.StringEnter;
                                return 0;
                            }
                        }
                        ++Current;
                    }
                    while (true);
                }
                do
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            length = (int)(Current - start) - length;
                            Current = start;
                            return length;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            State = DeserializeStateEnum.StringEnter;
                            return 0;
                        }
                    }
                }
                while (++Current != end);
            }
            State = DeserializeStateEnum.CrashEnd;
            return 0;
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="write">当前写入位置</param>
        private void deserializeEscapeUnsafe(char* write)
        {
        NEXT:
            if (*++Current == 'u') *write++ = (char)deserializeHex4();
            else if (*Current == 'x') *write++ = (char)deserializeHex2();
            else *write++ = *Current < EscapeCharSize ? escapeChars[*Current] : *Current;
            while (*++Current != Quote)
            {
                if (*Current != '\\') *write++ = *Current;
                else goto NEXT;
            }
            ++Current;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="start"></param>
        /// <returns>字符串,失败返回null</returns>
#if NetStandard21
        private string? deserializeEscape(char* start)
#else
        private string deserializeEscape(char* start)
#endif
        {
            int size = deserializeEscapeSize();
            if (size != 0)
            {
                int left = (int)(Current - start);
                string value = AutoCSer.Common.AllocateString(left + size);
                fixed (char* valueFixed = value)
                {
                    AutoCSer.Common.CopyTo(start, valueFixed, left << 1);
                    deserializeEscapeUnsafe(valueFixed + left);
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <returns>写入结束位置</returns>
        private char* deserializeEscape()
        {
            char* write = Current;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)) *write++ = (char)deserializeHex4();
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return null;
                }

            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char)) *write++ = (char)deserializeHex2();
                else
                {
                    State = DeserializeStateEnum.CrashEnd;
                    return null;
                }
            }
            else if (Current != end) *write++ = *Current < EscapeCharSize ? escapeChars[*Current] : *Current;
            else
            {
                State = DeserializeStateEnum.CrashEnd;
                return null;
            }
            if (++Current != end)
            {
                if (endChar == Quote)
                {
                    do
                    {
                        if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                ++Current;
                                return write;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                State = DeserializeStateEnum.StringEnter;
                                return null;
                            }
                        }
                        *write++ = *Current++;
                    }
                    while (true);
                }
                do
                {
                    if (((bits[*(byte*)Current] & DeserializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return write;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            State = DeserializeStateEnum.StringEnter;
                            return null;
                        }
                    }
                    *write++ = *Current++;
                }
                while (Current != end);
            }
            State = DeserializeStateEnum.CrashEnd;
            return null;
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        /// <returns>解析是否成功</returns>
        private bool deserializeQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            fixed (char* jsonFixed = value.GetFixedBuffer())
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;
                if (isTempString == 0)
                {
                    var newValue = deserializeEscape(start);
                    if (newValue != null)
                    {
                        value.Set(newValue, 0, newValue.Length);
                        return true;
                    }
                }
                else
                {
                    char* writeEnd = deserializeEscape();
                    if (writeEnd != null)
                    {
                        value.Set(value.String.notNull(), (int)(start - jsonFixed), (int)(writeEnd - start));
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        /// <returns>解析是否成功</returns>
        internal static bool DeserializeQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            JsonDeserializer jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
            try
            {
                return jsonDeserializer.deserializeQuoteString(ref value, escapeIndex, quote, isTempString);
            }
            finally { jsonDeserializer.Free(); }
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <returns>解析是否成功</returns>
        internal bool DeserializeQuoteString(ref SubString value, CharStream charStream, int escapeIndex, char quote)
        {
            fixed (char* jsonFixed = value.GetFixedBuffer())
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;

                int size = deserializeEscapeSize();
                if (size != 0)
                {
                    int left = (int)(Current - start);
                    char* valueFixed = (char*)charStream.GetBeforeMove((size += left) * sizeof(char));
                    if (valueFixed != null)
                    {
                        AutoCSer.Common.CopyTo(start, valueFixed, left << 1);
                        deserializeEscapeUnsafe(valueFixed + left);
                        return true;
                    }
                }
            }
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
#if AOT
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        /// <param name="memberIndex">成员编号</param>
        internal delegate void MemberDeserializeDelegate<T>(JsonDeserializer deserializer, ref T value, int memberIndex);
        /// <summary>
        /// 自定义 JSON 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ICustom<T>(ref T value) where T : ICustomSerialize<T>
        {
            value.Deserialize(this);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void JsonDeserialize<T>(ref T? value)
#else
        public void JsonDeserialize<T>(ref T value)
#endif
        {
            TypeDeserializer<T>.DefaultDeserializer(this, ref value);
        }
        /// <summary>
        /// 不支持类型反序列化
        /// </summary>
        internal static readonly MethodInfo NotSupportMethod;
        /// <summary>
        /// 基类反序列化
        /// </summary>
        internal static readonly MethodInfo BaseMethod;
        /// <summary>
        /// 自定义反序列化
        /// </summary>
        internal static readonly MethodInfo ICustomMethod;
        /// <summary>
        /// Array deserialization
        /// </summary>
        internal static readonly MethodInfo LeftArrayMethod;
        /// <summary>
        /// Array deserialization
        /// </summary>
        internal static readonly MethodInfo ListArrayMethod;
        /// <summary>
        /// Array deserialization
        /// </summary>
        internal static readonly MethodInfo ArrayMethod;
        /// <summary>
        /// 可空数据反序列化
        /// </summary>
        internal static readonly MethodInfo NullableMethod;
        /// <summary>
        /// 集合反序列化
        /// </summary>
        internal static readonly MethodInfo CollectionMethod;
        /// <summary>
        /// 字典反序列化
        /// </summary>
        internal static readonly MethodInfo DictionaryMethod;
        /// <summary>
        /// 字典反序列化
        /// </summary>
        internal static readonly MethodInfo IDictionaryMethod;
        /// <summary>
        /// 键值对反序列化
        /// </summary>
        internal static readonly MethodInfo KeyValuePairMethod;
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void EnumJsonDeserializeMethodName<T>(ref T value) { }
        /// <summary>
        /// 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void DeserializeMethodName<T>(ref T value) { }
        ///// <summary>
        ///// JSON 反序列化
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        //internal void JsonDeserialize<T>(ref T value) { }
        ///// <summary>
        ///// 反序列化模板
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //internal static void JsonDeserialize<T>() { }
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
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(string json, JsonDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(string json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult Deserialize<T>(string json, ref T? value, JsonDeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(string json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(json)) return new DeserializeResult(DeserializeStateEnum.NullJson);
            JsonDeserializer jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
            try
            {
                return jsonDeserializer.deserialize(json, ref value, config);
            }
            finally { jsonDeserializer.Free(); }
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(SubString json, JsonDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(SubString json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(ref json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static DeserializeResult Deserialize<T>(SubString json, ref T? value, JsonDeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(SubString json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            return Deserialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(ref SubString json, JsonDeserializeConfig? config = null)
#else
        public static T Deserialize<T>(ref SubString json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(ref json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
#pragma warning disable CS8601
        public static DeserializeResult Deserialize<T>(ref SubString json, ref T? value, JsonDeserializeConfig? config = null)
#pragma warning restore CS8601
#else
        public static DeserializeResult Deserialize<T>(ref SubString json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(json)) return new DeserializeResult(DeserializeStateEnum.NullJson);
            JsonDeserializer jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
            try
            {
                return jsonDeserializer.deserialize(ref json, ref value, config);
            }
            finally { jsonDeserializer.Free(); }
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">Json 字符串</param>
        /// <param name="length">Json 长度</param>
        /// <param name="value">Target data</param>
        /// <returns>是否解析成功</returns>
#if NetStandard21
        internal static DeserializeResult UnsafeDeserialize<T>(char* json, int length, ref T? value)
#else
        internal static DeserializeResult UnsafeDeserialize<T>(char* json, int length, ref T value)
#endif
        {
            JsonDeserializer jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
            try
            {
                jsonDeserializer.Config = DefaultConfig;
                return jsonDeserializer.Deserialize(json, length, ref value);
            }
            finally { jsonDeserializer.Free(); }
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">Json 字符串</param>
        /// <param name="length">Json 长度</param>
        /// <param name="value">Target data</param>
        /// <returns>是否解析成功</returns>
#if NetStandard21
        internal static DeserializeResult UnsafeDeserializeBinaryMix<T>(char* json, int length, ref T? value)
#else
        internal static DeserializeResult UnsafeDeserializeBinaryMix<T>(char* json, int length, ref T value)
#endif
        {
            JsonDeserializer jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
            try
            {
                jsonDeserializer.SetBinaryMix(false);
                return jsonDeserializer.Deserialize(json, length, ref value);
            }
            finally { jsonDeserializer.FreeBinaryMix(); }
        }

        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(string json, JsonDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(string json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(string json, ref T? value, JsonDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(string json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(json)) return new DeserializeResult(DeserializeStateEnum.NullJson);
            JsonDeserializer jsonDeserializer = ThreadStaticDeserializer.Get().Deserializer;
            try
            {
                return jsonDeserializer.deserialize(json, ref value, config);
            }
            finally { jsonDeserializer.freeThreadStatic(); }
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(SubString json, JsonDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(SubString json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(ref json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(SubString json, ref T? value, JsonDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(SubString json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            return ThreadStaticDeserialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(ref SubString json, JsonDeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(ref SubString json, JsonDeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(ref json, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">Target data</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>反序列化状态</returns>
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(ref SubString json, ref T? value, JsonDeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(ref SubString json, ref T value, JsonDeserializeConfig config = null)
#endif
        {
            if (string.IsNullOrEmpty(json)) return new DeserializeResult(DeserializeStateEnum.NullJson);
            JsonDeserializer jsonDeserializer = ThreadStaticDeserializer.Get().Deserializer;
            try
            {
                return jsonDeserializer.deserialize(ref json, ref value, config);
            }
            finally { jsonDeserializer.freeThreadStatic(); }
        }

        /// <summary>
        /// 解析委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        internal delegate void DeserializeDelegate<T>(JsonDeserializer deserializer, ref T value);
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
        /// JSON 转换时间差
        /// </summary>
        internal static readonly DateTime JavaScriptLocalMinTime;
        /// <summary>
        ///  JSON 转换时间差
        /// </summary>
        internal static readonly long JavaScriptLocalMinTimeTicks;
        /// <summary>
        /// 转义字符集合尺寸
        /// </summary>
        internal const int EscapeCharSize = 128;
        /// <summary>
        /// 转义字符集合
        /// </summary>
        private static AutoCSer.Memory.Pointer escapeCharData;
        /// <summary>
        /// JSON 解析数字
        /// </summary>
        internal const byte DeserializeNumberBit = 16;
        /// <summary>
        /// JSON 解析键值
        /// </summary>
        internal const byte DeserializeNameBit = 32;
        /// <summary>
        /// JSON 解析键值开始
        /// </summary>
        internal const byte DeserializeNameStartBit = 64;
        /// <summary>
        /// JSON 解析空格[ ,\t,\r,\n,160]
        /// </summary>
        internal const byte DeserializeSpaceBit = 128;
        /// <summary>
        /// JSON 解析转义查找
        /// </summary>
        internal const byte DeserializeEscapeSearchBit = 8;
        /// <summary>
        /// JavaScript 转义位[\0,\b,\t,\f,\r,\n,\\,"]
        /// </summary>
        internal const byte EscapeBit = 4;
        /// <summary>
        /// JSON 解析空格开始[ ,\t,\r,\n,/,160]
        /// </summary>
        internal const byte DeserializeSpaceStartBit = 1;
        /// <summary>
        /// JSON 解析字符状态位
        /// </summary>
        internal static AutoCSer.Memory.Pointer DeserializeBits;

        static JsonDeserializer()
        {
            JavaScriptLocalMinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddTicks(Date.LocalTimeTicks);
            JavaScriptLocalMinTimeTicks = JavaScriptLocalMinTime.Ticks;

            DeserializeBits = Unmanaged.GetJsonDeserializeBits();
            byte* bits = DeserializeBits.Byte;
            AutoCSer.Memory.Common.Fill((ulong*)bits, 256 >> 3, ulong.MaxValue);
            for (char value = '0'; value <= '9'; ++value) bits[value] &= (DeserializeNumberBit | DeserializeNameBit) ^ 255;
            for (char value = 'A'; value <= 'F'; ++value) bits[value] &= (DeserializeNameBit | DeserializeNameStartBit | DeserializeNumberBit) ^ 255;
            for (char value = 'a'; value <= 'f'; ++value) bits[value] &= (DeserializeNameBit | DeserializeNameStartBit | DeserializeNumberBit) ^ 255;
            for (char value = 'G'; value <= 'Z'; ++value) bits[value] &= (DeserializeNameBit | DeserializeNameStartBit) ^ 255;
            for (char value = 'g'; value <= 'z'; ++value) bits[value] &= (DeserializeNameBit | DeserializeNameStartBit) ^ 255;
            bits['\t'] &= (DeserializeSpaceStartBit | DeserializeSpaceBit | EscapeBit) ^ 255;
            bits['\r'] &= (DeserializeSpaceStartBit | DeserializeSpaceBit | EscapeBit) ^ 255;
            bits['\n'] &= (DeserializeSpaceStartBit | DeserializeSpaceBit | DeserializeEscapeSearchBit | EscapeBit) ^ 255;
            bits[' '] &= (DeserializeSpaceStartBit | DeserializeSpaceBit) ^ 255;
            bits[0xA0] &= (DeserializeSpaceStartBit | DeserializeSpaceBit) ^ 255;
            bits['/'] &= DeserializeSpaceStartBit ^ 255;
            bits['x'] &= DeserializeNumberBit ^ 255;
            bits['+'] &= DeserializeNumberBit ^ 255;
            bits['-'] &= DeserializeNumberBit ^ 255;
            bits['.'] &= DeserializeNumberBit ^ 255;
            bits['_'] &= (DeserializeNameBit | DeserializeNameStartBit) ^ 255;
            bits['\''] &= (DeserializeNameStartBit | DeserializeEscapeSearchBit) ^ 255;
            bits['"'] &= (DeserializeNameStartBit | DeserializeEscapeSearchBit | EscapeBit) ^ 255;
            bits['\\'] &= (DeserializeEscapeSearchBit | EscapeBit) ^ 255;
            bits['\f'] &= EscapeBit ^ 255;
            bits['\b'] &= EscapeBit ^ 255;
            bits[0] &= EscapeBit ^ 255;

            escapeCharData = Unmanaged.GetJsonDeserializeEscapeCharData();
            char* escapeCharDataChar = escapeCharData.Char;
            for (int value = 0; value != EscapeCharSize; ++value) escapeCharDataChar[value] = (char)value;
            escapeCharDataChar['0'] = '\0';
            escapeCharDataChar['B'] = escapeCharDataChar['b'] = '\b';
            escapeCharDataChar['F'] = escapeCharDataChar['f'] = '\f';
            escapeCharDataChar['N'] = escapeCharDataChar['n'] = '\n';
            escapeCharDataChar['R'] = escapeCharDataChar['r'] = '\r';
            escapeCharDataChar['T'] = escapeCharDataChar['t'] = '\t';
            escapeCharDataChar['V'] = escapeCharDataChar['v'] = '\v';

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
            deserializeDelegates.Add(typeof(JsonNode), (DeserializeDelegate<JsonNode>)primitiveDeserialize);
#if NetStandard21
            deserializeDelegates.Add(typeof(string), (DeserializeDelegate<string?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(object), (DeserializeDelegate<object?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(Type), (DeserializeDelegate<Type?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(bool[]), (DeserializeDelegate<bool[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(byte[]), (DeserializeDelegate<byte[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(sbyte[]), (DeserializeDelegate<sbyte[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ushort[]), (DeserializeDelegate<ushort[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(short[]), (DeserializeDelegate<short[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(int[]), (DeserializeDelegate<int[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(uint[]), (DeserializeDelegate<uint[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(long[]), (DeserializeDelegate<long[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ulong[]), (DeserializeDelegate<ulong[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(DateTime[]), (DeserializeDelegate<DateTime[]?>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(TimeSpan[]), (DeserializeDelegate<TimeSpan[]?>)primitiveDeserialize);
#else
            deserializeDelegates.Add(typeof(string), (DeserializeDelegate<string>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(object), (DeserializeDelegate<object>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(Type), (DeserializeDelegate<Type>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(bool[]), (DeserializeDelegate<bool[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(byte[]), (DeserializeDelegate<byte[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(sbyte[]), (DeserializeDelegate<sbyte[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ushort[]), (DeserializeDelegate<ushort[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(short[]), (DeserializeDelegate<short[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(int[]), (DeserializeDelegate<int[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(uint[]), (DeserializeDelegate<uint[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(long[]), (DeserializeDelegate<long[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(ulong[]), (DeserializeDelegate<ulong[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(DateTime[]), (DeserializeDelegate<DateTime[]>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(TimeSpan[]), (DeserializeDelegate<TimeSpan[]>)primitiveDeserialize);
#endif
            deserializeDelegates.Add(typeof(Int128), (DeserializeDelegate<Int128>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(UInt128), (DeserializeDelegate<UInt128>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(Half), (DeserializeDelegate<Half>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Complex), (DeserializeDelegate<System.Numerics.Complex>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Plane), (DeserializeDelegate<System.Numerics.Plane>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Quaternion), (DeserializeDelegate<System.Numerics.Quaternion>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Matrix3x2), (DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Matrix4x4), (DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector2), (DeserializeDelegate<System.Numerics.Vector2>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector3), (DeserializeDelegate<System.Numerics.Vector3>)primitiveDeserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector4), (DeserializeDelegate<System.Numerics.Vector4>)primitiveDeserialize);
#if AOT
            foreach (MethodInfo method in typeof(JsonDeserializer).GetMethods(BindingFlags.Static | BindingFlags.Public))
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
                        else if (method.Name == nameof(Dictionary)) DictionaryMethod = method;
                        else if (method.Name == nameof(EnumUShort)) EnumUShortMethod = method;
                        else if (method.Name == nameof(NotSupport)) NotSupportMethod = method;
                        break;
                    case 11:
                        if (method.Name == nameof(IDictionary)) IDictionaryMethod = method;
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
            foreach (Delegate deserializeDelegate in AutoCSer.JsonSerializer.CustomConfig.PrimitiveDeserializeDelegates)
            {
                var type = AutoCSer.Common.CheckDeserializeType(typeof(JsonDeserializer), deserializeDelegate);
                if (type != null) deserializeDelegates[type] = deserializeDelegate;
            }
#endif
        }
    }
}
