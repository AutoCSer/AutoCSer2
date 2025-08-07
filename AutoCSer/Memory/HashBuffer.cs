using AutoCSer.Extensions;
using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 用于 HASH 的字节数组与数据缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct HashBuffer : IEquatable<HashBuffer>
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        private readonly byte[] data;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Pointer Buffer;
        /// <summary>
        /// 用于 HASH 的数据缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="hashCode"></param>
        internal HashBuffer(byte* data, int size, int hashCode)
        {
            this.data = EmptyArray<byte>.Array;
            Buffer = new Pointer(data, size, hashCode);
        }
        /// <summary>
        /// 用于 HASH 的数据缓冲区
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        internal HashBuffer(byte* start, byte* end)
        {
            int size = *(int*)start + sizeof(int);
            this.data = EmptyArray<byte>.Array;
            byte* hashData = start + size;
            if (size > 0 && end - hashData >= sizeof(int)) Buffer = new Pointer(start, size, *(int*)hashData);
            else Buffer = default(Pointer);
        }
        /// <summary>
        /// 用于 HASH 的字节数组
        /// </summary>
        /// <param name="key"></param>
        internal HashBuffer(ref HashBuffer key)
        {
            Buffer = key.Buffer;
            data = Buffer.GetBufferArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashBuffer other)
        {
            if (data.Length != 0)
            {
                if (other.data.Length != 0) return AutoCSer.Common.SequenceEqual(data, other.data);
                return other.Buffer.BufferSequenceEqual(data);
            }
            return other.data.Length != 0 && other.Equals(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<HashBuffer>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Buffer.CurrentIndex;
        }
    }
}
