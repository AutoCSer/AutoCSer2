using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    public unsafe static class SubStringExtension
    {
        /// <summary>
        /// 字符子串转小写字母是否匹配右侧小写字母字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lowerString">小写字母字符串，长度不为 0</param>
        /// <returns></returns>
        internal unsafe static bool LowerEquals(this SubString value, string lowerString)
        {
            int length = value.Length;
            if (length == lowerString.Length)
            {
                fixed (char* leftFixed = value.GetFixedBuffer(), rightFixed = lowerString)
                {
                    byte* start = (byte*)(leftFixed + value.Start), right = (byte*)rightFixed;
                    for (byte* end = start + ((length <<= 1) & (int.MaxValue - 7)); start != end; start += sizeof(ulong), right += sizeof(ulong))
                    {
                        if ((*(ulong*)start | 0x20002000200020UL) != *(ulong*)right) return false;
                    }
                    if ((length & 4) != 0)
                    {
                        if ((*(uint*)start | 0x200020U) != *(uint*)right) return false;
                        start += sizeof(uint);
                        right += sizeof(uint);
                    }
                    if ((length & 2) != 0) return (*(ushort*)start | 0x20) == *(ushort*)right;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 字符子串转小写字母查找第一个匹配小写字母位置
        /// </summary>
        /// <param name="value">长度不为 0</param>
        /// <param name="lowerChar">小写字母</param>
        /// <returns>相对于原字符串的位置，失败返回 -1</returns>
        internal unsafe static int IndexLower(this SubString value, char lowerChar)
        {
            fixed (char* leftFixed = value.GetFixedBuffer())
            {
                char* start = leftFixed + value.Start, end = start + value.Length;
                do
                {
                    if ((*start | 0x20) == lowerChar) return (int)(start - leftFixed);
                }
                while (++start != end);
            }
            return -1;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">待分割字符串</param>
        /// <param name="splitChar">分割符</param>
        /// <returns>字符子串集合</returns>
        public static LeftArray<SubString> split(this string text, char splitChar)
        {
            return ((SubString)text).Split(splitChar);
        }
        /// <summary>
        /// 比较字符串大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal unsafe static int CompareTo(this SubString left, ref SubString right)
        {
            int size = Math.Min(left.Length, right.Length);
            if (size != 0)
            {
                fixed (char* leftFixed = left.GetFixedBuffer(), rightFixed = right.GetFixedBuffer())
                {
                    char* leftStart = leftFixed + left.Start, end = leftStart + size, rightStart = rightFixed + right.Start;
                    do
                    {
                        if (*leftStart != *rightStart) return *leftStart - *rightStart;
                        ++rightStart;
                    }
                    while (++leftStart != end);
                }
            }
            return left.Length - right.Length;
        }
    }
}
