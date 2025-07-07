using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符子串
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct SubString : IEquatable<SubString>, IEquatable<string>
    {
        /// <summary>
        /// 原字符串
        /// </summary>
#if NetStandard21
        internal string? String;
#else
        internal string String;
#endif
        /// <summary>
        /// 原字符串中的起始位置
        /// </summary>
        internal int Start;
        /// <summary>
        /// 字符子串长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// 获取字符
        /// </summary>
        /// <param name="index">字符位置</param>
        /// <returns></returns>
        public char this[int index]
        {
            get { return String.notNull()[Start + index]; }
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="value"></param>
        /// <param name="length">长度</param>
        internal SubString(int startIndex, int length, string value)
        {
            String = value;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="value"></param>
        /// <param name="length">长度</param>
        internal SubString(long startIndex, long length, string value) : this((int)startIndex, (int)length, value) { }
        /// <summary>
        /// 字符串隐式转换为子串
        /// </summary>
        /// <param name="value"></param>
        /// <returns>字符子串</returns>
        public static implicit operator SubString(string value)
        {
            return new SubString(0, value.Length, value);
        }
        /// <summary>
        /// 字符子串隐式转换为字符串
        /// </summary>
        /// <param name="value">字符子串</param>
        /// <returns>字符串</returns>
#if NetStandard21
        public static implicit operator string?(SubString value) { return value.ToString(); }
#else
        public static implicit operator string(SubString value) { return value.ToString(); }
#endif
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public unsafe override int GetHashCode()
        {
            if (Length == 0) return 0;
            ulong value = GetHashCode64();
            return (int)((uint)value ^ (uint)(value >> 32));
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        internal unsafe ulong GetHashCode64()
        {
            if (Length == 0) return 0;
            fixed (char* valueFixed = GetFixedBuffer()) return AutoCSer.Memory.Common.GetHashCode64((byte*)(valueFixed + Start), Length << 1);
        }
        /// <summary>
        /// Clear the data
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetEmpty()
        {
            String = string.Empty;
            Start = Length = 0;
        }
        /// <summary>
        /// 设置数据长度
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(string value, int startIndex, int length)
        {
            String = value;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="obj">待比较子串</param>
        /// <returns>子串是否相等</returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<SubString>());
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(SubString other)
        {
            return Equals(ref other);
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
        public bool Equals(ref SubString other)
        {
            if (Length == other.Length)
            {
                if (Length != 0)
                {
                    if (object.ReferenceEquals(String, other.String) && Start == other.Start) return true;
                    fixed (char* valueFixed = GetFixedBuffer(), otherFixed = other.GetFixedBuffer())
                    {
                        return AutoCSer.Common.SequenceEqual(valueFixed + Start, otherFixed + other.Start, Length << 1);
                    }
                }
                return other.Length == 0;
            }
            return false;
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
#if NetStandard21
        public bool Equals(string? other)
#else
        public bool Equals(string other)
#endif
        {
            if (Length == 0) return other == null ? String == null : (other.Length == 0 && String != null);
            if (other != null && Length == other.Length)
            {
                if (Object.ReferenceEquals(String, other) && Start == 0) return true;
                fixed (char* valueFixed = GetFixedBuffer(), otherFixed = other)
                {
                    return AutoCSer.Common.SequenceEqual(valueFixed + Start, otherFixed, Length << 1);
                }
            }
            return false;
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
#if NetStandard21
        public unsafe override string? ToString()
#else
        public unsafe override string ToString()
#endif
        {
            if (String == null || (Start | (Length ^ String.Length)) == 0) return String;
            if (Length != 0)
            {
                fixed (char* valueFixed = GetFixedBuffer()) return new string(valueFixed, Start, Length);
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>子串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubString Slice(int startIndex, int length)
        {
            return new SubString { String = String, Start = Start + startIndex, Length = length };
        }
        /// <summary>
        /// 设置为子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="length">长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Sub(int startIndex, int length)
        {
            Start += startIndex;
            Length = length;
        }
        /// <summary>
        /// 设置为子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Sub(int startIndex)
        {
            Start += startIndex;
            Length -= startIndex;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <returns>子串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubString GetSub(int startIndex)
        {
            return new SubString(Start + startIndex, Length - startIndex, String.notNull());
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>子串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubString GetSub(int startIndex, int length)
        {
            return new SubString(Start + startIndex, length, String.notNull());
        }
        /// <summary>
        /// 修改起始位置
        /// </summary>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveStart(int count)
        {
            Start += count;
            Length -= count;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败返回-1</returns>
        public int IndexOf(char value)
        {
            if (Length != 0)
            {
                fixed (char* valueFixed = GetFixedBuffer())
                {
                    char* start = valueFixed + Start, find = AutoCSer.Extensions.StringExtension.FindNotNull(start, start + Length, value);
                    if (find != null) return (int)(find - start);
                }
            }
            return -1;
        }

        /// <summary>
        /// 删除前后空格，包括 \t\r\n
        /// </summary>
        /// <returns>删除前后空格</returns>
        public SubString Trim()
        {
            if (Length != 0)
            {
                fixed (char* valueFixed = GetFixedBuffer())
                {
                    char* start = valueFixed + Start, end = start + Length;
                    start = AutoCSer.Extensions.StringExtension.TrimStartNotEmpty(start, end);
                    if (start == null) return string.Empty;
                    end = AutoCSer.Extensions.StringExtension.TrimEndNotEmpty(start, end);
                    if (end == null) return string.Empty;
                    return new SubString((int)(start - valueFixed), (int)(end - start), String.notNull());
                }
            }
            return this;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        public LeftArray<SubString> Split(char split)
        {
            LeftArray<SubString> values = new LeftArray<SubString>(0);
            if (Length != 0)
            {
                fixed (char* valueFixed = GetFixedBuffer())
                {
                    char* last = valueFixed + Start, end = last + Length;
                    for (char* start = last; start != end;)
                    {
                        if (*start == split)
                        {
                            values.PrepLength(1);
                            values.Array[values.Length++].Set(String.notNull(), (int)(last - valueFixed), (int)(start - last));
                            last = ++start;
                        }
                        else ++start;
                    }
                    values.PrepLength(1);
                    values.Array[values.Length++].Set(String.notNull(), (int)(last - valueFixed), (int)(end - last));
                }
            }
            else
            {
                values.PrepLength(1);
                values.Array[values.Length++].Set(string.Empty, 0, 0);
            }
            return values;
        }

        /// <summary>
        /// Get the fixed buffer, DEBUG mode to detect the data range
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal string GetFixedBuffer()
        {
#if DEBUG
            if (Start < 0) throw new Exception(Start.toString() + " < 0");
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Start != 0 && Length != 0 && Start + Length > String.notNull().Length) throw new Exception(Start.toString() + " + " + Length.toString() + " > " + String.notNull().Length.toString());
#endif
            return String.notNull();
        }
    }
}
