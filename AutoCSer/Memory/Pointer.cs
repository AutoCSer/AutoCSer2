using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe partial struct Pointer : IEquatable<Pointer>
    {
        /// <summary>
        /// 指针
        /// </summary>
        internal void* Data;
        /// <summary>
        /// 总字节长度
        /// </summary>
        internal int ByteSize;
        /// <summary>
        /// 总字节长度
        /// </summary>
        public int Size { get { return ByteSize; } }
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        internal int CurrentIndex;
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        public int Index { get { return CurrentIndex; } }
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        public byte* Current
        {
            get { return (byte*)Data + CurrentIndex; }
        }
        /// <summary>
        /// 数据结束位置
        /// </summary>
        public byte* End
        {
            get { return (byte*)Data + ByteSize; }
        }
        /// <summary>
        /// 空闲字节数量
        /// </summary>
        internal int FreeSize
        {
            get { return ByteSize - CurrentIndex; }
        }
        /// <summary>
        /// 不为 0 表示存在空闲字节
        /// </summary>
        internal int IsFreeSize { get { return ByteSize ^ CurrentIndex; } }
        /// <summary>
        /// 指针
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        internal Pointer(void* data, int size)
        {
#if DEBUG
            if (size < 0) throw new Exception(size.toString() + " < 0");
#endif
            Data = data;
            ByteSize = size;
            CurrentIndex = 0;
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public byte* Byte
        {
            get { return (byte*)Data; }
        }
        /// <summary>
        /// 字节指针
        /// </summary>
        public sbyte* SByte
        {
            get { return (sbyte*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public short* Short
        {
            get { return (short*)Data; }
        }
        /// <summary>
        /// 双字节指针
        /// </summary>
        public ushort* UShort
        {
            get { return (ushort*)Data; }
        }
        /// <summary>
        /// 字符指针
        /// </summary>
        public char* Char
        {
            get { return (char*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public int* Int
        {
            get { return (int*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public uint* UInt
        {
            get { return (uint*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public long* Long
        {
            get { return (long*)Data; }
        }
        /// <summary>
        /// 整形指针
        /// </summary>
        public ulong* ULong
        {
            get { return (ulong*)Data; }
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            return (int)((uint)((ulong)Data >> 3) ^ (uint)((ulong)Data >> 35));
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="obj">待比较指针</param>
        /// <returns>指针是否相等</returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<Pointer>());
        }
        /// <summary>
        /// 获取指定字节
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte Get(int index)
        {
            return ((byte*)Data)[index];
        }
        /// <summary>
        /// 指针比较
        /// </summary>
        /// <param name="other">待比较指针</param>
        /// <returns>指针是否相等</returns>
        public bool Equals(Pointer other)
        {
            return Data == other.Data;
        }
        /// <summary>
        /// Clear the data
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNull()
        {
            Data = null;
            ByteSize = CurrentIndex = 0;
        }
        /// <summary>
        /// 数据全部设置为 0
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (Data != null) AutoCSer.Common.Clear(Data, ByteSize);
        }
        /// <summary>
        /// 获取指针并清除
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void* GetDataClearOnly()
        {
            void* data = Data;
            Data = null;
            return data;
        }
        /// <summary>
        /// 设置指针
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(void* data, int size)
        {
#if DEBUG
            if (size < 0) throw new Exception(size.toString() + " < 0");
#endif
            Data = data;
            ByteSize = size;
            CurrentIndex = 0;
        }
        /// <summary>
        /// 当指针匹配时清空数据
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNull(void* data)
        {
            if (Data == data) SetNull();
        }
#if NetStandard21
        /// <summary>
        /// 移动临时指针位置并返回移动后有的当前数据操作位置
        /// </summary>
        /// <param name="count"></param>
        /// <returns>移动后有的当前数据操作位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int MoveData(int count)
        {
            Data = (byte*)Data + count;
            return CurrentIndex -= count;
        }
        /// <summary>
        /// 移动临时指针位置并返回移动后的指针
        /// </summary>
        /// <param name="count"></param>
        /// <param name="index">移动前的当前数据操作位置</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte* GetMoveData(int count, out int index)
        {
            index = CurrentIndex;
            Data = (byte*)Data + count;
            CurrentIndex -= count;
            return (byte*)Data;
        }
        /// <summary>
        /// 临时指针复制并移动位置
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CopyMoveData(ref Pointer pointer, int count)
        {
            Data = (byte*)pointer.Data + count;
            CurrentIndex = pointer.CurrentIndex - count;
        }
        /// <summary>
        /// 获取 ReadOnlySpan
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ReadOnlySpan<byte> GetReadOnlySpan()
        {
            return new ReadOnlySpan<byte>(Data, CurrentIndex);
        }
        /// <summary>
        /// 获取 ReadOnlySpan
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ReadOnlySpan<byte> GetReadOnlySpan(int start)
        {
            int size = CurrentIndex - start;
            if (size > 0) return new ReadOnlySpan<byte>((byte*)Data + start, size);
            return default(ReadOnlySpan<byte>);
        }
#endif
        /// <summary>
        /// 获取子段
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="byteSize">长度</param>
        /// <returns>子段</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Pointer Slice(int startIndex, int byteSize)
        {
#if DEBUG
            if (startIndex < 0) throw new Exception(startIndex.toString() + " < 0");
            debugCheckCurrentIndex(startIndex + byteSize);
#endif
            return new Pointer((byte*)Data + startIndex, byteSize);
        }
        /// <summary>
        /// 复制数据到另外一个指针
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CopyTo(ref Pointer data)
        {
            if (CurrentIndex > 0)
            {
#if DEBUG
                if (CurrentIndex > data.ByteSize) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] > ByteSize[" + data.ByteSize.toString() + "]");
#endif
                AutoCSer.Common.CopyTo(Data, data.Data, data.CurrentIndex = CurrentIndex);
            }
            else data.CurrentIndex = 0;
        }
        /// <summary>
        /// 移动当前数据操作位置并返回移动之前的位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte* GetBeforeMove(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            debugCheckWriteSize(size);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            CurrentIndex += size;
            return data;
        }
        /// <summary>
        /// 设置当前数据操作位置
        /// </summary>
        /// <param name="current"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCurrent(void* current)
        {
            int newIndex = (int)((byte*)current - (byte*)Data);
#if DEBUG
            debugCheckCurrentIndex(newIndex);
#endif
            CurrentIndex = newIndex;
        }
        /// <summary>
        /// 设置位图
        /// </summary>
        /// <param name="bit"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckSetBit(int bit)
        {
            if (Data != null) SetBit(bit);
        }
        /// <summary>
        /// 设置位图
        /// </summary>
        /// <param name="bit"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBit(int bit)
        {
            int index = bit >> 3;
#if DEBUG
            if (index >= ByteSize) throw new Exception(index.toString() + " >= ByteSize[" + ByteSize.toString() + "]");
            if (index < 0) throw new Exception(index.toString() + " < 0");
#endif
            *((byte*)Data + index) |= (byte)(1 << (bit & 7));
        }
        /// <summary>
        /// 获取位图数据
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetBit(int bit)
        {
            int index = bit >> 3;
#if DEBUG
            if (index >= ByteSize) throw new Exception(index.toString() + " >= ByteSize[" + ByteSize.toString() + "]");
            if (index < 0) throw new Exception(index.toString() + " < 0");
#endif
            return *((byte*)Data + index) & (1 << (bit & 7));
        }
        /// <summary>
        /// 移动当前位置到最后
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveToEnd()
        {
            CurrentIndex = ByteSize;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        public void CheckMoveSize(int size)
        {
            int newIndex = CurrentIndex + size;
            if (newIndex > ByteSize) throw new Exception(newIndex.toString() + " > ByteSize[" + ByteSize.toString() + "]");
            if (newIndex < 0) throw new Exception(newIndex.toString() + " < 0");
            CurrentIndex = newIndex;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveSize(int size)
        {
            int newIndex = CurrentIndex + size;
#if DEBUG
            debugCheckCurrentIndex(newIndex);
#endif
            CurrentIndex = newIndex;
        }
        /// <summary>
        /// 移动当前位置并返回当前位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetMoveSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            debugCheckWriteSize(size);
#endif
            return CurrentIndex += size;
        }
        /// <summary>
        /// 移动当前数据操作位置并返回移动之前的位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetIndexBeforeMove(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            debugCheckWriteSize(size);
#endif
            int index = CurrentIndex;
            CurrentIndex += size;
            return index;
        }
        //
        /// <summary>
        /// 写入缓冲区字节数
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="size"></param>
        internal void SerializeMoveSize(int startIndex, int size)
        {
#if DEBUG
            debugCheckCurrentIndex(startIndex);
            debugCheckCurrentIndex(startIndex - sizeof(int));
#endif
            *(int*)((byte*)Data + (startIndex - sizeof(int))) = size;
            switch (size & 3)
            {
                case 1: SerializeFillByteSize3(); return;
                case 2: SerializeFillByteSize2(); return;
                case 3: SerializeFillByteSize1(); return;
            }
        }
        /// <summary>
        /// 扩展数据总字节数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte* newByteSize(int size)
        {
            byte* data = (byte*)Unmanaged.Get(size, false);
            AutoCSer.Common.CopyTo(Data, data, ByteSize);
            AutoCSer.Common.Clear(data + ByteSize, size - ByteSize);
            return data;
        }
        /// <summary>
        /// 扩展数据总字节数
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void NewByteSize(int size)
        {
#if DEBUG
            if (size <= ByteSize) throw new Exception(size.toString() + " <= " + ByteSize.toString());
#endif
            byte* data = newByteSize(size);
            Unmanaged.Free(Data, ByteSize);
            Data = data;
            ByteSize = size;
        }
        /// <summary>
        /// 扩展数据总字节数
        /// </summary>
        /// <param name="size"></param>
        /// <param name="freePool"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void NewByteSize(int size, UnmanagedPool freePool)
        {
#if DEBUG
            if (size <= ByteSize) throw new Exception(size.toString() + " <= " + ByteSize.toString());
#endif
            byte* data = newByteSize(size);
            freePool.PushOnly(ref this);
            Data = data;
            ByteSize = size;
        }
        /// <summary>
        /// 弹出一个整数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int PopInt()
        {
#if DEBUG
            if (CurrentIndex < sizeof(int)) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < "+ sizeof(int).toString());
#endif
            CurrentIndex -= sizeof(int);
            return *(int*)((byte*)Data + CurrentIndex);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ref Guid value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Guid));
#endif
            *(Guid*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Guid);
        }
        /// <summary>
        /// 写入会话回调标识
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(AutoCSer.Net.CommandServer.CallbackIdentity value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(AutoCSer.Net.CommandServer.CallbackIdentity));
#endif
            *(AutoCSer.Net.CommandServer.CallbackIdentity*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(AutoCSer.Net.CommandServer.CallbackIdentity);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(int value1, uint value2)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(uint));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = value1;
            *(uint*)(data + sizeof(int)) = value2;
            CurrentIndex += sizeof(int) + sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(int value1, int value2, int value3)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) * 3);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = value1;
            *(int*)(data + sizeof(int)) = value2;
            *(int*)(data + sizeof(int) * 2) = value3;
            CurrentIndex += sizeof(int) * 3;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(long value1, long value2)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(long) * 2);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(long*)data = value1;
            *(long*)(data + sizeof(long)) = value2;
            CurrentIndex += sizeof(long) * 2;
        }
        /// <summary>
        /// 写入 64 字节数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ulong value0, ulong value1, ulong value2, ulong value3)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(ulong) * 4);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
            *(ulong*)(data + sizeof(ulong) * 3) = value3;
            CurrentIndex += sizeof(ulong) * 4;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteSize(ulong value, int size)
        {
#if DEBUG
            debugCheckWriteSize(size);
#endif
            *(ulong*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += size;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteSize(ulong value0, ulong value1, int size)
        {
#if DEBUG
            debugCheckWriteSize(size);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            CurrentIndex += size;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteSize(ulong value0, ulong value1, ulong value2, int size)
        {
#if DEBUG
            debugCheckWriteSize(size);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
            CurrentIndex += size;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteSize(ulong value0, ulong value1, ulong value2, ulong value3, int size)
        {
#if DEBUG
            debugCheckWriteSize(size);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
            *(ulong*)(data + sizeof(ulong) * 3) = value3;
            CurrentIndex += size;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(byte* data, int size)
        {
#if DEBUG
            debugCheckWriteSize(size);
#endif
            AutoCSer.Common.CopyTo(data, (byte*)Data + CurrentIndex, size);
            CurrentIndex += size;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串，长度必须大于0</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(string value)
        {
#if DEBUG
            debugCheckWriteSize(value.Length << 1);
#endif
            AutoCSer.Common.CopyTo(value, (byte*)Data + CurrentIndex);
            CurrentIndex += value.Length << 1;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串，长度必须大于0</param>
        /// <param name="index">起始位置</param>
        /// <param name="size">长度必须大于0</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(string value, int index, int size)
        {
#if DEBUG
            debugCheckWriteSize(size << 1);
#endif
            AutoCSer.Common.CopyTo(value, index, (byte*)Data + CurrentIndex, size);
            CurrentIndex += size << 1;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quoteChar"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(string value, char quoteChar)
        {
            Write(quoteChar);
            Write(value);
            Write(quoteChar);
        }
        /// <summary>
        /// 写 JSON 名称
        /// </summary>
        /// <param name="name"></param>
        internal void WriteJsonCustomNameFirst(string name)
        {
            Write('{' + ('"' << 16));
            SimpleWrite(name);
            Write('"' + (':' << 16));
        }
        /// <summary>
        /// 写 JSON 名称
        /// </summary>
        /// <param name="name"></param>
        internal void WriteJsonCustomNameNext(string name)
        {
            Write(',' + ('"' << 16));
            SimpleWrite(name);
            Write('"' + (':' << 16));
        }
        ///// <summary>
        ///// 写入 JSON 字符串
        ///// </summary>
        ///// <param name="value"></param>
        //internal void WriteJsonString(string value)
        //{
        //    Write('"');
        //    SimpleWrite(value);
        //    Write('"');
        //}
        /// <summary>
        /// Guid转换成字符串
        /// </summary>
        /// <param name="value">Guid</param>
        /// <param name="quoteChar"></param>
        public void WriteJson(ref System.Guid value, char quoteChar)
        {
#if DEBUG
            debugCheckWriteSize(38 * sizeof(char));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(char*)data = quoteChar;
            new GuidCreator { Value = value }.ToString((char*)(data + sizeof(char)));
            *(char*)(data + sizeof(char) * 37) = quoteChar;
            CurrentIndex += 38 * sizeof(char);
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        internal void JavaScriptUnescape(byte* start, byte* end)
        {
#if DEBUG
            if (start > end) throw new Exception("start > end");
#endif
            while (start != end && *start != '%')
            {
                Write(*start == 0 ? ' ' : (char)*start);
                ++start;
            }
            if (start != end) javaScriptUnescape(start, end);
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void javaScriptUnescape(byte* start, byte* end)
        {
        NEXT:
            if (*++start == 'u')
            {
                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code <<= 12;
                code += (number << 8);
                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code += (number << 4);
                number = (uint)(*++start - '0');
                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                Write(code == 0 ? ' ' : (char)code);
            }
            else
            {
                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                Write(code == 0 ? ' ' : (char)code);
            }
            while (++start < end)
            {
                if (*start == '%') goto NEXT;
                Write(*start == 0 ? ' ' : (char)*start);
            }
        }
        /// <summary>
        /// 复制字符串，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="value"></param>
        internal void SimpleWrite(string value)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value) SimpleWrite((byte*)valueFixed, value.Length << 1);
            }
        }
        /// <summary>
        /// 复制字节数组，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size">必须大于0</param>
        internal void SimpleWrite(byte* source, int size)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
#endif
            byte* end = source + ((size + (sizeof(ulong) - 1)) & (int.MaxValue - (sizeof(ulong) - 1))), write = (byte*)Data + CurrentIndex;
#if DEBUG
            debugCheckWriteSize((int)(end - source));
#endif
            do
            {
                *(ulong*)write = *(ulong*)source;
                source += sizeof(ulong);
                write += sizeof(ulong);
            }
            while (source != end);
            CurrentIndex += size;
        }
        /// <summary>
        /// 增加 1 个空白字节
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void SerializeFillByteSize1()
        {
#if DEBUG
            debugCheckWriteSize(1);
#endif
            *((byte*)Data + CurrentIndex++) = 0;
        }
        /// <summary>
        /// 增加 2 个空白字节
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void SerializeFillByteSize2()
        {
#if DEBUG
            debugCheckWriteSize(2);
#endif
            *(ushort*)((byte*)Data + CurrentIndex) = 0;
            CurrentIndex += 2;
        }
        /// <summary>
        /// 增加 3 个空白字节
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void SerializeFillByteSize3()
        {
#if DEBUG
            debugCheckWriteSize(3);
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *data = 0;
            *(ushort*)(data + 1) = 0;
            CurrentIndex += 3;
        }
        /// <summary>
        /// 填充空白字节
        /// </summary>
        /// <param name="fillSize">字节数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeFill(int fillSize)
        {
            switch (fillSize)
            {
                case 1: SerializeFillByteSize1(); return;
                case 2: SerializeFillByteSize2(); return;
                case 3: SerializeFillByteSize3(); return;
            }
        }
        /// <summary>
        /// 补白对齐 4 字节
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeFillWithStartIndex(int startIndex)
        {
            switch ((CurrentIndex - startIndex) & 3)
            {
                case 1: SerializeFillByteSize3(); return;
                case 2: SerializeFillByteSize2(); return;
                case 3: SerializeFillByteSize1(); return;
            }
        }
        /// <summary>
        /// 左侧补白对齐 4 字节
        /// </summary>
        /// <param name="size">增加数据长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeFillLeftByteSize(int size)
        {
            switch (size & 3)
            {
                case 1:
#if DEBUG
                    if (CurrentIndex < 3) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < 3");
#endif
                    byte* data = (byte*)Data + (CurrentIndex - 3);
                    *data = 0;
                    *(ushort*)(data + 1) = 0;
                    return;
                case 2:
#if DEBUG
                    if (CurrentIndex < 2) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < 2");
#endif
                    *(ushort*)((byte*)Data + (CurrentIndex - 2)) = 0;
                    return;
                case 3:
#if DEBUG
                    if (CurrentIndex < 1) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < 1");
#endif
                    *((byte*)Data + (CurrentIndex - 1)) = 0;
                    return;
            }
        }
        /// <summary>
        /// 左侧补白对齐 4 字节
        /// </summary>
        /// <param name="size">增加数据长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeFillLeftByteSize2(int size)
        {
#if DEBUG
            if (CurrentIndex < 2) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < 2");
#endif
            if ((size & 2) != 0) *(ushort*)((byte*)Data + (CurrentIndex - 2)) = 0;
        }
        /// <summary>
        /// 增加当前数据长度并且补白对齐 4 字节
        /// </summary>
        /// <param name="current">新的当前位置</param>
        internal void SerializeFillByteSize(void* current)
        {
            int currentIndex = (int)((byte*)current - (byte*)Data);
            switch ((currentIndex - CurrentIndex) & 3)
            {
                case 1:
#if DEBUG
                    debugCheckCurrentIndex(currentIndex + 3);
#endif
                    *(byte*)current = 0;
                    *(ushort*)((byte*)current + 1) = 0;
                    CurrentIndex = currentIndex + 3;
                    return;
                case 2:
#if DEBUG
                    debugCheckCurrentIndex(currentIndex + 2);
#endif
                    *(ushort*)current = 0;
                    CurrentIndex = currentIndex + 2;
                    return;
                case 3:
#if DEBUG
                    debugCheckCurrentIndex(currentIndex + 1);
#endif
                    *(byte*)current = 0;
                    CurrentIndex = currentIndex + 1;
                    return;
                default:
#if DEBUG
                    debugCheckCurrentIndex(currentIndex);
#endif
                    CurrentIndex = currentIndex;
                    return;
            }
        }
        /// <summary>
        /// 增加当前数据长度并且补白对齐 4 字节
        /// </summary>
        /// <param name="current">新的当前位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeFillByteSize2(void* current)
        {
            int currentIndex = (int)((byte*)current - (byte*)Data);
            if (((currentIndex - CurrentIndex) & 2) == 0)
            {
#if DEBUG
                debugCheckCurrentIndex(currentIndex);
#endif
                CurrentIndex = currentIndex;
            }
            else
            {
#if DEBUG
                debugCheckCurrentIndex(currentIndex + 2);
#endif
                *(ushort*)current = 0;
                CurrentIndex = currentIndex + 2;
            }
        }
        /// <summary>
        /// 二进制序列化 JSON 成员序列化补白对齐 4 字节并写入 JSON 字节长度
        /// </summary>
        /// <param name="startIndex"></param>
        internal void JsonSerializeFill(int startIndex)
        {
            if (((CurrentIndex ^ startIndex) & 2) != 0) Write(' ');
            *(int*)((byte*)Data + (startIndex - sizeof(int))) = CurrentIndex - startIndex;
            //int length = CurrentIndex - startIndex;
            //if ((length & 2) != 0) Write(' ');
            //*(int*)((byte*)Data + (startIndex - sizeof(int))) = length;
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] GetArray()
        {
            if (CurrentIndex == 0) return EmptyArray<byte>.Array;
            return AutoCSer.Common.GetArray(Data, CurrentIndex);
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index">复制起始位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void GetBuffer(ref ByteArrayBuffer buffer, int index)
        {
#if DEBUG
            if (index < 0) throw new Exception(index.toString() + " < 0");
            if (index >= CurrentIndex) throw new Exception(index.toString() + " >= "+ CurrentIndex.toString());
            DebugCheck();
#endif
            int size = CurrentIndex - index;
            ByteArrayPool.GetBuffer(ref buffer, CurrentIndex);
            AutoCSer.Common.CopyTo((byte*)Data + index, buffer.Buffer.notNull().Buffer, buffer.StartIndex + index, size);
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override unsafe string ToString()
        {
            if (CurrentIndex == 0) return string.Empty;
            return new string((char*)Data, 0, CurrentIndex >> 1);
        }

#if DEBUG
        /// <summary>
        /// 检查数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void DebugCheck()
        {
            if (CurrentIndex < 0) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] < 0");
            if (CurrentIndex > ByteSize) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] > ByteSize[" + ByteSize.toString() + "]");
        }
        /// <summary>
        /// 检测写入数据长度
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void debugCheckWriteSize(int size)
        {
            if (CurrentIndex + size < 0) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] + " + size.toString() + " < 0");
            if (CurrentIndex + size > ByteSize) throw new Exception("CurrentIndex[" + CurrentIndex.toString() + "] + "+ size.toString() + " > ByteSize[" + ByteSize.toString() + "]");
        }
        /// <summary>
        /// 检测设置当前写入位置
        /// </summary>
        /// <param name="newIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void debugCheckCurrentIndex(int newIndex)
        {
            if (newIndex < 0) throw new Exception(newIndex.toString() + " < 0");
            if (newIndex > ByteSize) throw new Exception(newIndex.toString() + " > ByteSize[" + ByteSize.toString() + "]");
        }
        /// <summary>
        /// 检查数据写入结束位置是否超出范围
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void DebugCheckEnd(byte* data)
        {
            if (data > End) throw new Exception("data[" + ((IntPtr)data).ToString() + "] > End[" + ((IntPtr)End).ToString() + "]");
        }
#endif
    }
}
