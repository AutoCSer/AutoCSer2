using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 视图数据输出
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ViewResponse
    {
        /// <summary>
        /// 视图数据
        /// </summary>
        private readonly View view;
        /// <summary>
        /// JSON 序列化
        /// </summary>
        internal readonly JsonSerializer JsonSerializer;
        /// <summary>
        /// 是否已经释放输出
        /// </summary>
        private bool isFree;
        /// <summary>
        /// 是否存在回调函数输出
        /// </summary>
        private bool isCallback;
        /// <summary>
        /// 视图数据输出
        /// </summary>
        /// <param name="view">视图数据</param>
        internal ViewResponse(View view)
        {
            this.view = view;
            isCallback = isFree = false;
            JsonSerializer = AutoCSer.JsonSerializer.YieldPool.Default.Pop() ?? new JsonSerializer();
        }
        /// <summary>
        /// 释放输出
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            if (!isFree)
            {
                isFree = true;
                JsonSerializer.Free();
            }
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="result">错误状态</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="encoding">输出编码</param>
        /// <param name="isResponseJavaScript"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe ByteArrayBuffer Error(ref ResponseResult result, string? callback, Encoding encoding, bool isResponseJavaScript)
#else
        internal unsafe ByteArrayBuffer Error(ref ResponseResult result, string callback, Encoding encoding, bool isResponseJavaScript)
#endif
        {
            CharStream stream = JsonSerializer.CharStream;
            stream.Clear();
            if (!string.IsNullOrEmpty(callback))
            {
                stream.SimpleWrite(callback);
                stream.Write('(');
            }
            stream.SimpleWrite(isResponseJavaScript ? @"{State:" : @"{""State"":");
            stream.WriteString((byte)result.State);
            var message = result.Message;
            if (message != null)
            {
                stream.SimpleWrite(isResponseJavaScript ? @",Message:" : @",""Message"":");
                if (message.Length == 0) stream.WriteJsonEmptyString();
                else
                {
                    fixed (char* valueFixed = message) stream.WriteJson(valueFixed, message.Length);
                }
            }
            stream.Write('}');
            if (!string.IsNullOrEmpty(callback)) stream.Write(')');
            return getBuffer(encoding);
        }
        /// <summary>
        /// 输出成功状态
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="encoding"></param>
        /// <param name="isResponseJavaScript"></param>
        /// <returns></returns>
#if NetStandard21
        internal ByteArrayBuffer Success(string? callback, Encoding encoding, bool isResponseJavaScript)
#else
        internal ByteArrayBuffer Success(string callback, Encoding encoding, bool isResponseJavaScript)
#endif
        {
            CharStream stream = JsonSerializer.CharStream;
            stream.Clear();
            if (!string.IsNullOrEmpty(callback))
            {
                stream.SimpleWrite(callback);
                stream.Write('(');
            }
            stream.SimpleWrite(isResponseJavaScript?"{State:1}": @"{""State"":1}");
            if (!string.IsNullOrEmpty(callback)) stream.Write(')');
            return getBuffer(encoding);
        }
        /// <summary>
        /// 获取输出缓冲区
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private unsafe ByteArrayBuffer getBuffer(Encoding encoding)
        {
            Pointer pointer = JsonSerializer.CharStream.Data.Pointer;
            int charCount = pointer.CurrentIndex >> 1, size = encoding.GetByteCount(pointer.Char, charCount);
            ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(size);
            bool isEncoding = false;
            try
            {
                fixed (byte* bufferFixed = buffer.GetFixedBuffer()) encoding.GetBytes(pointer.Char, charCount, bufferFixed + buffer.StartIndex, buffer.CurrentIndex = size);
                isEncoding = true;
                return buffer;
            }
            finally
            {
                if (!isEncoding) buffer.Free();
            }
        }
        /// <summary>
        /// 开始输出
        /// </summary>
        /// <param name="callback">回调函数名称</param>
        /// <param name="isResponseJavaScript"></param>
        /// <returns>输出流</returns>
#if NetStandard21
        internal CharStream Start(string? callback, bool isResponseJavaScript)
#else
        internal CharStream Start(string callback, bool isResponseJavaScript)
#endif
        {
            CharStream stream = JsonSerializer.CharStream;
            stream.Clear();
            if (!string.IsNullOrEmpty(callback))
            {
                isCallback = true;
                stream.SimpleWrite(callback);
                stream.Write('(');
            }
            stream.SimpleWrite(isResponseJavaScript ? @"{State:1,Result:" : @"{""State"":1,""Result"":");
            return stream;
        }
        /// <summary>
        /// 结束输出
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal ByteArrayBuffer End(Encoding encoding)
        {
            CharStream stream = JsonSerializer.CharStream;
            stream.Write('}');
            if (isCallback) stream.Write(')');
            return getBuffer(encoding);
        }
    }
}
