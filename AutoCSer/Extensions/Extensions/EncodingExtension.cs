using System;
using System.Text;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字符编码相关扩展
    /// </summary>
    internal static class EncodingExtension
    {
        /// <summary>
        /// 根据编码获取字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal unsafe static string GetString(this Encoding encoding, byte* data, int size)
        {
            if (data != null)
            {
                if (size > 0)
                {
                    int charCount = encoding.GetCharCount(data, size);
                    if (charCount != 0)
                    {
                        string value = AutoCSer.Common.Config.AllocateString(charCount);
                        fixed (char* valueFixed = value) encoding.GetChars(data, size, valueFixed, charCount);
                        return value;
                    }
                }
                return string.Empty;
            }
            return null;
        }
    }
}
