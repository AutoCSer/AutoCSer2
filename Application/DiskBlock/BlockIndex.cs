using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块索引信息
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(long) + sizeof(uint) + sizeof(int))]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct BlockIndex
    {
        /// <summary>
        /// 错误
        /// </summary>
        internal const sbyte ErrorSize = sbyte.MaxValue;
        /// <summary>
        /// 直接使用索引记录数据的最大字节数
        /// </summary>
        internal const int MaxIndexSize = 15;

        /// <summary>
        /// 起始字节位置
        /// </summary>
        [FieldOffset(0)]
        internal long Index;
        /// <summary>
        /// 磁盘块服务唯一编号
        /// </summary>
        [FieldOffset(sizeof(long))]
        internal uint Identity;
        /// <summary>
        /// 字节数
        /// </summary>
        [FieldOffset(sizeof(long) + sizeof(uint))]
        internal int Size;
        /// <summary>
        /// 错误磁盘块索引信息
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        internal BlockIndex(uint identity)
        {
            Index = 0;
            Identity = identity;
            Size = new BlockSize().SetSize(ErrorSize);
        }
        /// <summary>
        /// 错误磁盘块索引信息
        /// </summary>
        /// <param name="index"></param>
        internal BlockIndex(long index)
        {
            Index = index;
            Identity = 0;
            Size = 0;
        }
        /// <summary>
        /// 直接使用索引信息记录
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="identity"></param>
        internal BlockIndex(long index, sbyte size, uint identity = 0)
        {
            Index = index;
            Identity = identity;
            Size = new BlockSize().SetSize(size);
        }
        /// <summary>
        /// 设置字节数
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void SetSize(sbyte size)
        {
            Size = new BlockSize { Value = Size }.SetSize(size);
        }
        /// <summary>
        /// 字节数小于 16 直接使用索引信息记录
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isIndex"></param>
        /// <returns></returns>
        internal unsafe static BlockIndex GetIndexSize(ref SubArray<byte> buffer, out bool isIndex)
        {
            int size = buffer.Length;
            if (size <= MaxIndexSize)
            {
                if (size == 0)
                {
                    isIndex = true;
                    return buffer.Array == null ? new BlockIndex((long)BinarySerializer.NullValue) : default(BlockIndex);
                }
                fixed (byte* dataFixed = buffer.GetFixedBuffer())
                {
                    BlockIndex index = *(BlockIndex*)(dataFixed + buffer.Start);
                    index.SetSize((sbyte)-size);
                    isIndex = true;
                    return index;
                }
            }
            isIndex = false;
            return default(BlockIndex);
        }
        /// <summary>
        /// 字节数小于 16 直接使用索引信息记录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isIndex"></param>
        /// <returns></returns>
        internal unsafe static BlockIndex GetIndexSize(string data, out bool isIndex)
        {
            if (data == null)
            {
                isIndex = true;
                return new BlockIndex(BinarySerializer.NullValue, -4);
            }
            int size = data.Length;
            if (size < 6)
            {
                if (size == 0)
                {
                    isIndex = true;
                    return new BlockIndex(0, -4);
                }
                byte* buffer = stackalloc byte[sizeof(long) + sizeof(BlockIndex)];
                *(int*)buffer = size <<= 1;
                fixed (char* dataFixed = data) *(BlockIndex*)(buffer + sizeof(int)) = *(BlockIndex*)dataFixed;
                BlockIndex index = *(BlockIndex*)buffer;
                if (size == 2) size = 4;
                else if (size == 6) size = 8;
                index.SetSize((sbyte)-(size + sizeof(int)));
                isIndex = true;
                return index;
            }
            isIndex = false;
            return default(BlockIndex);
        }
        /// <summary>
        /// 设置起始字节位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(long index, int size)
        {
            Index = index;
            Size = size;
        }
        /// <summary>
        /// 获取读取数据状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReadBufferStateEnum GetReadState()
        {
            if (Size > MaxIndexSize) return ReadBufferStateEnum.Unknown;
            if (Size <= 0 && -new BlockSize().GetSize(Size) <= MaxIndexSize) return ReadBufferStateEnum.IndexSize;
            return ReadBufferStateEnum.UnidentifiableSize;
        }
        ///// <summary>
        ///// 获取数据字节数
        ///// </summary>
        ///// <returns>负数表示失败</returns>
        //public int GetSize()
        //{
        //    if (Size > MaxIndexSize) return Size;
        //    if (Size <= 0)
        //    {
        //        int size = -new BlockSize().GetSize(Size);
        //        if (size <= MaxIndexSize) return size;
        //    }
        //    return int.MinValue;
        //}
        /// <summary>
        /// 获取读取数据结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe bool GetResult(out ReadResult<byte[]?> result)
#else
        internal unsafe bool GetResult(out ReadResult<byte[]> result)
#endif
        {
#if NetStandard21
            result = new ReadResult<byte[]?>(CommandClientReturnTypeEnum.ClientException);
#else
            result = new ReadResult<byte[]>(CommandClientReturnTypeEnum.ClientException);
#endif
            switch (result.BufferState = GetReadState())
            {
                case ReadBufferStateEnum.Unknown: return false;
                case ReadBufferStateEnum.IndexSize:
                    int size = -new BlockSize().GetSize(Size);
                    if (size == 0) result.Set(Index != BinarySerializer.NullValue ? EmptyArray<byte>.Array : null);
                    else
                    {
                        BlockIndex index = this;
                        byte[] buffer = AutoCSer.Common.GetUninitializedArray<byte>(size);
                        AutoCSer.Common.CopyTo((byte*)&index, buffer, 0, size);
                        result.Set(buffer);
                    }
                    return true;
                default: result.ReturnType = CommandClientReturnTypeEnum.Success; return true;
            }
        }
        /// <summary>
        /// 获取读取数据结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe bool GetResult(out ReadResult<string?> result)
#else
        internal unsafe bool GetResult(out ReadResult<string> result)
#endif
        {
#if NetStandard21
            result = new ReadResult<string?>(CommandClientReturnTypeEnum.ClientException);
#else
            result = new ReadResult<string>(CommandClientReturnTypeEnum.ClientException);
#endif
            switch (result.BufferState = GetReadState())
            {
                case ReadBufferStateEnum.Unknown: return false;
                case ReadBufferStateEnum.IndexSize:
                    int size = -new BlockSize().GetSize(Size);
                    if (size < sizeof(int)) result.Set(ReadBufferStateEnum.UnidentifiableSize);
                    else
                    {
                        var value = default(string);
                        BlockIndex index = this;
                        if (deserializeString((byte*)&index, size + (-size & 3), out value)) result.Set(value);
                        else result.ReturnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                    }
                    return true;
                default: result.ReturnType = CommandClientReturnTypeEnum.Success; return true;
            }
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static bool deserializeString(byte* start, int size, out string? value)
#else
        private unsafe static bool deserializeString(byte* start, int size, out string value)
#endif
        {
            if (*(int*)start == BinarySerializer.NullValue)
            {
                value = null;
                return size == sizeof(int);
            }
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (length == 0)
                {
                    value = string.Empty;
                    return size == sizeof(int);
                }
                if (length == 10 && size == 14) size = 16;
                if (size == (((long)length + (3 + sizeof(int))) & (long.MaxValue - 3)))
                {
                    value = new string((char*)(start + sizeof(int)), 0, length >> 1);
                    return true;
                }
            }
            else
            {
                length >>= 1;
                int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= size)
                {
                    value = AutoCSer.Common.AllocateString(length);
                    fixed (char* valueFixed = value)
                    {
                        byte* end = start + size;
                        return BinaryDeserializer.Deserialize(start, end, valueFixed, length, lengthSize) == end;
                    }
                }
            }
            value = null;
            return false;
        }
        /// <summary>
        /// 获取读取数据结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe bool GetJsonResult<T>(out ReadResult<T?> result)
#else
        internal unsafe bool GetJsonResult<T>(out ReadResult<T> result)
#endif
        {
#if NetStandard21
            result = new ReadResult<T?>(CommandClientReturnTypeEnum.ClientException);
#else
            result = new ReadResult<T>(CommandClientReturnTypeEnum.ClientException);
#endif
            switch (result.BufferState = GetReadState())
            {
                case ReadBufferStateEnum.Unknown: return false;
                case ReadBufferStateEnum.IndexSize:
                    int size = -new BlockSize().GetSize(Size);
                    if (size < sizeof(int)) result.Set(ReadBufferStateEnum.UnidentifiableSize);
                    //else if (size == 12 && Index == JsonNull.Index && Identity == JsonNull.Identity) result.Set(default(T));
                    else
                    {
                        BlockIndex index = this;
                        var value = default(T);
                        if (deserializeJson((byte*)&index, size + (-size & 3), out value)) result.Set(value);
                        else result.ReturnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                    }
                    return true;
                default: result.ReturnType = CommandClientReturnTypeEnum.Success; return true;
            }
        }
        /// <summary>
        /// JSON 对象反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static bool deserializeJson<T>(byte* start, int size, out T? value)
#else
        private unsafe static bool deserializeJson<T>(byte* start, int size, out T value)
#endif
        {
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (size == (((long)length + (3 + sizeof(int))) & (long.MaxValue - 3)))
                {
                    if (length != 0) return deserializeJson((char*)(start + sizeof(int)), length >> 1, out value);
                    value = default(T);
                    return true;
                }
            }
            else
            {
                length >>= 1;
                int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= size)
                {
                    ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(length << 1);
                    try
                    {
                        fixed (byte* bufferFixed = buffer.GetFixedBuffer())
                        {
                            byte* bufferStart = bufferFixed + buffer.StartIndex, end = start + size;
                            if (BinaryDeserializer.Deserialize(start, end, (char*)bufferStart, length, lengthSize) == end) return deserializeJson((char*)bufferStart, length, out value);
                        }
                    }
                    finally { buffer.Free(); }
                }
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// JSON 对象反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static bool deserializeJson<T>(char* start, int length, out T? value)
#else
        private unsafe static bool deserializeJson<T>(char* start, int length, out T value)
#endif
        {
            value = default(T);
            if (length == 4 && *(long*)start == JsonDeserializer.NullStringValue) return true;
            return JsonDeserializer.UnsafeDeserialize(start, length, ref value).State == Json.DeserializeStateEnum.Success;
        }
        /// <summary>
        /// 获取读取数据结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe bool GetBinaryResult<T>(out ReadResult<T?> result)
#else
        internal unsafe bool GetBinaryResult<T>(out ReadResult<T> result)
#endif
        {
#if NetStandard21
            result = new ReadResult<T?>(CommandClientReturnTypeEnum.ClientException);
#else
            result = new ReadResult<T>(CommandClientReturnTypeEnum.ClientException);
#endif
            switch (result.BufferState = GetReadState())
            {
                case ReadBufferStateEnum.Unknown: return false;
                case ReadBufferStateEnum.IndexSize:
                    int size = -new BlockSize().GetSize(Size);
                    if (size == 0 || (size & 3) != 0)
                    {
                        result.Set(ReadBufferStateEnum.UnidentifiableSize);
                        return true;
                    }
                    if (size == 4 && Index == BinarySerializer.NullValue)
                    {
                        result.Set(default(T));
                        return true;
                    }
                    var value = default(T);
                    BlockIndex index = this;
                    if (BinaryDeserializer.UnsafeDeserialize((byte*)&index, size, ref value).State == BinarySerialize.DeserializeStateEnum.Success)
                    {
                        result.Set(value);
                        return true;
                    }
                    result.ReturnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                    return false;
                default: result.ReturnType = CommandClientReturnTypeEnum.Success; return true;
            }
        }

        /// <summary>
        /// JSON null
        /// </summary>
        internal static readonly BlockIndex JsonNull;
        unsafe static BlockIndex()
        {
            byte* buffer = stackalloc byte[sizeof(BlockIndex)];
            *(int*)buffer = sizeof(long);
            *(long*)(buffer + sizeof(int)) = JsonDeserializer.NullStringValue;
            BlockIndex index = *(BlockIndex*)buffer;
            index.SetSize(-12);
            JsonNull = index;
        }
    }
}
