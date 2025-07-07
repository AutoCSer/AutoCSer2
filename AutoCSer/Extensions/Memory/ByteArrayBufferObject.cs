using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 字节数组缓冲区
    /// </summary>
    public sealed class ByteArrayBufferObject
    {
        /// <summary>
        /// 字节数组缓冲区
        /// </summary>
        internal ByteArrayBuffer Buffer;
        ///// <summary>
        ///// 字节数组缓冲区
        ///// </summary>
        //internal ByteArrayBufferObject() { }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="buffer"></param>
        internal ByteArrayBufferObject(ref SubArray<byte> buffer)
        {
            Buffer = new ByteArrayBuffer(ref buffer);
        }
        /// <summary>
        /// 意外释放（比如网络反序列成功，后续操作失败会导致对象丢失）
        /// </summary>
        ~ByteArrayBufferObject()
        {
            Buffer.Free();
        }
        /// <summary>
        /// 释放缓存区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            Buffer.Free();
        }
        /// <summary>
        /// Get the array substring
        /// 获取数组子串
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubArray<byte> GetSubArray()
        {
            return Buffer.GetSubArray(0, Buffer.CurrentIndex);
        }
    }
}
