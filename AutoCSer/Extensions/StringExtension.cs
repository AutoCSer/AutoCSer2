using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static class StringExtension
    {
#if DEBUG
        /// <summary>
        /// 检查字符串长度
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public static void DebugCheckSize(this string source, int index, int count)
        {
            if (index < 0) throw new Exception(index.toString() + " < 0");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
            if (index + count > source.Length) throw new Exception(index.toString() + " + " + count.toString() + " < " + source.Length.toString());
        }
#endif
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="value">大写字符串</param>
        /// <returns>小写字符串(原引用)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string toLower(this string value)
        {
            return !string.IsNullOrEmpty(value) ? toLowerNotEmpty(value) : value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="value">大写字符串</param>
        /// <returns>小写字符串(原引用)</returns>
        internal static string toLowerNotEmpty(this string value)
        {
            fixed (char* valueFixed = value)
            {
                ToLower(valueFixed, valueFixed + value.Length);
            }
            return value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end">长度必须大于0</param>
        internal static void ToLower(char* start, char* end)
        {
            do
            {
                if ((uint)(*start - 'A') < 26) *start |= (char)0x20;
            }
            while (++start != end);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null,长度必须大于0</param>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* FindNotNull(char* start, char* end, char value)
        {
            if (*--end == value)
            {
                while (*start != value) ++start;
                return start;
            }
            while (start != end)
            {
                if (*start == value) return start;
                ++start;
            }
            return null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* TrimStartNotEmpty(char* start, char* end)
        {
            do
            {
                switch (*start & 3)
                {
                    case 0:
                        if (*start != 0x20) return start;
                        break;
                    case 1:
                        if (*start != 0x0d) return start;
                        break;
                    case 2:
                        if (*start != 0x0a) return start;
                        break;
                    case 3:
                        if (*start != 7) return start;
                        break;
                }
            }
            while (++start != end);
            return null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* TrimEndNotEmpty(char* start, char* end)
        {
            do
            {
                switch (*--end & 3)
                {
                    case 0:
                        if (*end != 0x20) return end + 1;
                        break;
                    case 1:
                        if (*end != 0x0d) return end + 1;
                        break;
                    case 2:
                        if (*end != 0x0a) return end + 1;
                        break;
                    case 3:
                        if (*end != 7) return end + 1;
                        break;
                }
            }
            while (start != end);
            return null;
        }
        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="oldChar">原字符</param>
        /// <param name="newChar">目标字符</param>
        /// <returns>字符串</returns>
        internal static string replaceNotNull(this string value, char oldChar, char newChar)
        {
            fixed (char* valueFixed = value)
            {
                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                {
                    if (*start == oldChar) *start = newChar;
                }
            }
            return value;
        }
    }
}
