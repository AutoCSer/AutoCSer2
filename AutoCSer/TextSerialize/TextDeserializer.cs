using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 文本反序列化
    /// </summary>
    /// <typeparam name="T">文本反序列化类型</typeparam>
    public abstract unsafe class TextDeserializer<T> : AutoCSer.Threading.Link<T>
        where T : TextDeserializer<T>
    {
        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        protected readonly byte* bits;
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public AutoCSer.Metadata.MemberMap? MemberMap { get; internal set; }
#else
        public AutoCSer.Metadata.MemberMap MemberMap { get; internal set; }
#endif
        /// <summary>
        /// 解析字符串
        /// </summary>
        protected string text;
        /// <summary>
        /// 解析字符串起始位置
        /// </summary>
        protected char* textFixed;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        internal char* Current;
        /// <summary>
        /// 解析结束位置
        /// </summary>
        protected char* end;
        /// <summary>
        /// 临时字符串
        /// </summary>
        protected string stringBuffer4;
        /// <summary>
        /// 临时字符串
        /// </summary>
        protected string stringBuffer8;
        /// <summary>
        /// 临时字符串
        /// </summary>
        protected string stringBuffer16;
        /// <summary>
        /// 临时字符串
        /// </summary>
        protected string stringBuffer40;
        /// <summary>
        /// 自定义错误
        /// </summary>
#if NetStandard21
        protected internal string? customError;
#else
        protected internal string customError;
#endif
        /// <summary>
        /// 文本反序列化
        /// </summary>
        /// <param name="bits">字符状态位查询表格</param>
        protected TextDeserializer(byte* bits)
        {
            this.bits = bits;
            text = stringBuffer4 = stringBuffer8 = stringBuffer16 = stringBuffer40 = string.Empty;
        }
        /// <summary>
        /// 释放 XML 解析器（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void free()
        {
            customError = text = string.Empty;
            MemberMap = null;
        }
        /// <summary>
        /// 查找字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool isFindChar(ulong value)
        {
        START:
            ulong code = *(ulong*)Current ^ value;
            if (((code + 0xfffefffefffeffffUL) & ~code & 0x8000800080008000UL) == 0)
            {
                if ((Current += 4) < end) goto START;
                Current = end;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 查找字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在下一个字符</returns>
        protected bool isFindChar(char value)
        {
            if (*Current == value) return true;
            if (*(Current + 1) == value) return ++Current != end;
            //if ((Current += (*(Current + 2) == value ? 2 : 3)) < end) return true;
            if ((Current += (3 - (*(Current + 2) ^ value).logicalInversion())) < end) return true;
            Current = end;
            return false;
        }
        /// <summary>
        /// 查找字符
        /// </summary>
        /// <param name="value"></param>
        protected void findChar(ulong value)
        {
            do
            {
                ulong code = *(ulong*)Current ^ value;
                if (((code + 0xfffefffefffeffffUL) & ~code & 0x8000800080008000UL) == 0) Current += 4;
                else return;
            }
            while (true);
        }
        ///// <summary>
        ///// 临时字符串填充空格
        ///// </summary>
        ///// <param name="bufferFixed"></param>
        ///// <param name="length"></param>
        //protected static void fillStringBuffer(char* bufferFixed, int length)
        //{
        //    if (length <= AutoCSer.TextSerialize.Common.StringBufferSize - 4)
        //    {
        //        if ((length & 3) != 0)
        //        {
        //            //*(long*)(bufferFixed + length) = 0x20002000200020;
        //            *(long*)(bufferFixed + length) = 0;
        //            length = (length + 3) & (60);
        //        }
        //        while (length != 32)
        //        {
        //            //*(long*)(bufferFixed + length) = 0x20002000200020;
        //            *(long*)(bufferFixed + length) = 0;
        //            length += 4;
        //        }
        //    }
        //    else
        //    {
        //        //while (length != 32) *(bufferFixed + length++) = ' ';
        //        while (length != 32) *(bufferFixed + length++) = (char)0;
        //    }
        //}
        /// <summary>
        /// 获取临时字符串
        /// </summary>
        /// <param name="byteSize"></param>
        /// <returns></returns>
        protected string getStringBuffer(int byteSize)
        {
            if (byteSize <= 32)
            {
                if (byteSize <= 8)
                {
                    if (stringBuffer4.Length != 0) return stringBuffer4;
                    return stringBuffer4 = new string((char)0, 4);
                }
                if (byteSize <= 16)
                {
                    if (stringBuffer8.Length != 0) return stringBuffer8;
                    return stringBuffer8 = new string((char)0, 8);
                }
                if (stringBuffer16.Length != 0) return stringBuffer16;
                return stringBuffer16 = new string((char)0, 16);
            }
            if (byteSize <= 80)
            {
                if (stringBuffer40.Length != 0) return stringBuffer40;
                return stringBuffer40 = new string((char)0, 40);
            }
            return string.Empty;
        }
#if NetStandard21
        /// <summary>
        /// 获取临时字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ReadOnlySpan<char> GetStringBuffer(ref AutoCSer.Memory.Pointer buffer)
        {
            return new ReadOnlySpan<char>(buffer.Data, buffer.ByteSize >> 1);
        }
#else
        /// <summary>
        /// 获取临时字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal string GetStringBuffer(ref AutoCSer.Memory.Pointer buffer)
        {
            string stringBuffer = getStringBuffer(buffer.ByteSize);
            if (stringBuffer.Length != 0)
            {
                fixed (char* bufferFixed = stringBuffer)
                {
                    AutoCSer.Common.CopyTo(buffer.Data, bufferFixed, buffer.ByteSize);
                    AutoCSer.Common.Clear((byte*)bufferFixed + buffer.ByteSize, (stringBuffer.Length << 1) - buffer.ByteSize);
                }
            }
            return stringBuffer;
        }
#endif
    }
}
