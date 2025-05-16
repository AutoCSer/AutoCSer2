using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase : IDisposable
    {
        /// <summary>
        /// 数据指针
        /// </summary>
        internal UnmanagedPoolPointer Data;
        /// <summary>
        /// 空闲字节数量
        /// </summary>
        public int FreeSize
        {
            get { return Data.Pointer.FreeSize; }
        }
        ///// <summary>
        ///// 当前数据操作位置
        ///// </summary>
        //public byte* Current
        //{
        //    get { return Data.Pointer.Current; }
        //}
        /// <summary>
        /// 是否非托管内存数据
        /// </summary>
        internal bool IsUnmanaged;
        /// <summary>
        /// 是否允许扩展缓存区大小
        /// </summary>
        internal bool CanResize = true;
        /// <summary>
        /// 在不允许扩展缓存区大小的情况下是否产生了扩展操作
        /// </summary>
        internal bool IsResizeError;
        /// <summary>
        /// 默认为 true 表示字符串二进制序列化直接复制内存数据，设置为 false 则对 ASCII 进行编码可以降低空间占用
        /// </summary>
        internal bool IsSerializeCopyString = true;
        /// <summary>
        /// 非托管内存池
        /// </summary>
        /// <param name="unmanagedPool"></param>
        internal UnmanagedStreamBase(UnmanagedPool unmanagedPool)
        {
            Data = unmanagedPool.GetPoolPointer();
            IsUnmanaged = true;
        }
        /// <summary>
        /// 非托管内存池
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isUnmanaged"></param>
        internal UnmanagedStreamBase(ref UnmanagedPoolPointer data, bool isUnmanaged)
        {
#if DEBUG
            data.Pointer.DebugCheck();
#endif
            Data = data;
            IsUnmanaged = isUnmanaged;
        }
        /// <summary>
        /// 析构释放资源
        /// </summary>
        ~UnmanagedStreamBase()
        {
            if (IsUnmanaged) Data.Push();
        }
        /// <summary>
        /// 强制重置数据缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            Data = default(AutoCSer.Memory.UnmanagedPoolPointer);
            IsUnmanaged = false;
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public virtual void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public virtual void Close()
        {
            if (IsUnmanaged) Data.PushSetNull();
            else Data.Pointer.SetNull();
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close(bool isSerializeCopyString)
        {
            IsSerializeCopyString = isSerializeCopyString;
            Close();
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public virtual void Clear()
        {
            Data.Pointer.CurrentIndex = 0;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearCanResize()
        {
            Clear();
            CanResize = true;
        }
        /// <summary>
        /// 尝试申请数据缓冲区并清空数据
        /// </summary>
        /// <param name="unmanagedPool"></param>
        /// <returns>flase 表示已经存在数据缓冲区</returns>
        internal bool TrySetData(UnmanagedPool unmanagedPool)
        {
            if (Data.Pointer.ByteSize == 0)
            {
                Data = unmanagedPool.GetPoolPointer();
                IsUnmanaged = true;
                Clear();
                return true;
            }
            Clear();
            return false;
        }
        /// <summary>
        /// 尝试申请数据缓冲区并清空数据
        /// </summary>
        /// <param name="unmanagedPool"></param>
        /// <returns>flase 表示已经存在数据缓冲区</returns>
        internal bool TrySetDataCanResize(UnmanagedPool unmanagedPool)
        {
            if (Data.Pointer.ByteSize == 0)
            {
                Data = unmanagedPool.GetPoolPointer();
                IsUnmanaged = true;
                ClearCanResize();
                return true;
            }
            ClearCanResize();
            return false;
        }
        /// <summary>
        /// 重置数据（调用该方法之后应该调用 ResetEnd / Close 避免后续调用写入数据）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Reset(void* data, int size)
        {
            if (IsUnmanaged)
            {
                Data.PushSetNull();
                IsUnmanaged = false;
            }
            Data.Set(data, size);
        }
        /// <summary>
        /// 重置数据完成
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ResetEnd(void* data)
        {
            Data.Pointer.SetNull(data);
        }
        /// <summary>
        /// 复制数据缓冲区
        /// </summary>
        /// <param name="stream"></param>
        internal void CopyFromStart(UnmanagedStreamBase stream)
        {
            bool isSerializeCopyString = IsSerializeCopyString;
            IsResizeError = false;
            Data = stream.Data;
            IsSerializeCopyString = stream.IsSerializeCopyString;
            IsUnmanaged = stream.IsUnmanaged;
            CanResize = stream.CanResize;
            stream.IsSerializeCopyString = isSerializeCopyString;
        }
        /// <summary>
        /// 恢复数据缓冲区
        /// </summary>
        /// <param name="stream"></param>
        internal void CopyFromEnd(UnmanagedStreamBase stream)
        {
            bool isSerializeCopyString = IsSerializeCopyString;
            Data = stream.Data;
            IsSerializeCopyString = stream.IsSerializeCopyString;
            IsResizeError = stream.IsResizeError;
            IsUnmanaged = stream.IsUnmanaged;
            stream.IsSerializeCopyString = isSerializeCopyString;
        }
        /// <summary>
        /// 复制数据缓冲区并返回原数据缓冲区信息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        internal void ExchangeToBuffer(UnmanagedStreamBase stream, out UnmanagedStreamExchangeBuffer buffer)
        {
            buffer = new UnmanagedStreamExchangeBuffer(this);
            IsResizeError = false;
            Data = stream.Data;
            IsUnmanaged = stream.IsUnmanaged;
            CanResize = stream.CanResize;
            IsSerializeCopyString = stream.IsSerializeCopyString;
        }
        /// <summary>
        /// 恢复设置数据缓冲区
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        internal void ExchangeFromBuffer(UnmanagedStreamBase stream, ref UnmanagedStreamExchangeBuffer buffer)
        {
            stream.Data = Data;
            stream.IsUnmanaged = IsUnmanaged;
            stream.CanResize = CanResize;
            stream.IsResizeError = IsResizeError;
            stream.IsSerializeCopyString = IsSerializeCopyString;
            Data = buffer.Data;
            IsUnmanaged = buffer.IsUnmanaged;
            CanResize = buffer.CanResize;
            IsResizeError = buffer.IsResizeError;
            IsSerializeCopyString = buffer.IsSerializeCopyString;
        }
        /// <summary>
        /// 设置字符串二进制序列化直接复制内存数据
        /// </summary>
        /// <param name="isSerializeCopyString"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ExchangeIsSerializeCopyString(ref bool isSerializeCopyString)
        {
            bool value = isSerializeCopyString;
            isSerializeCopyString = IsSerializeCopyString;
            IsSerializeCopyString = value;
        }
        ///// <summary>
        ///// 复制数据缓冲区并返回原数据缓冲区信息
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="size"></param>
        ///// <param name="data"></param>
        ///// <param name="isUnmanaged"></param>
        //internal void Exchange(void* buffer, int size, out UnmanagedPoolPointer data, out bool isUnmanaged)
        //{
        //    data = Data;
        //    isUnmanaged = IsUnmanaged;
        //    Data.Set(buffer, size);
        //    IsUnmanaged = false;
        //}
        ///// <summary>
        ///// 恢复设置数据缓冲区
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="isUnmanaged"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void Exchange(ref UnmanagedPoolPointer data, bool isUnmanaged)
        //{
        //    if (IsUnmanaged) Data.PushSetNull();
        //    IsUnmanaged = isUnmanaged;
        //    Data = data;
        //}
        /// <summary>
        /// 设置是否允许扩展缓存区大小并返回当前数据位置
        /// </summary>
        /// <param name="canResize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCanResize(bool canResize)
        {
            CanResize = canResize;
            IsResizeError = false;
        }
        /// <summary>
        /// 设置是否允许扩展缓存区大小并返回当前数据位置
        /// </summary>
        /// <param name="canResize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SetCanResizeGetCurrentIndex(bool canResize)
        {
            SetCanResize(canResize);
            return Data.Pointer.CurrentIndex;
        }
        /// <summary>
        /// 设置预增数据流字节长度
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool setPrepSize(int size)
        {
            if (size > Data.Pointer.ByteSize)
            {
                if (CanResize)
                {
                    UnmanagedPoolPointer newData = UnmanagedPool.GetPoolPointer(Data.Pointer.ByteSize <= int.MaxValue >> 1 ? Math.Max(Data.Pointer.ByteSize << 1, size) : int.MaxValue);
                    Data.Pointer.CopyTo(ref newData.Pointer);
                    if (IsUnmanaged) Data.Push();
                    else IsUnmanaged = true;
                    Data = newData;
                    return true;
                }
                SetResizeError();
                return false;
            }
            throw new Exception(size.toString() + " <= " + Data.Pointer.FreeSize.toString());
        }
        /// <summary>
        /// 设置扩展错误状态
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetResizeError()
        {
            IsResizeError = true;
            Data.Pointer.MoveToEnd();
        }
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="size">增加字节长度</param>
        /// <returns>是否增加成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected internal bool PrepSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
#endif
            return size <= Data.Pointer.FreeSize || setPrepSize(size + Data.Pointer.CurrentIndex);
        }
#if AOT
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="size">必须大于 0</param>
        /// <returns>是否增加成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryPrepSize(int size)
        {
            return size > 0 && PrepSize(size);
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size">必须大于 0</param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryMoveSize(int size)
        {
            return size > 0 && MoveSize(size);
        }
#else
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="size">必须大于 0</param>
        /// <returns>是否增加成功</returns>
        internal bool TryPrepSize(int size) { return false; }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size">必须大于 0</param>
        /// <returns>是否成功</returns>
        internal bool TryMoveSize(int size) { return false; }
#endif
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="size">增加字节长度</param>
        /// <returns>是否增加成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool PrepSize(UnmanagedStreamBase unmanagedStream, int size)
        {
            return unmanagedStream.PrepSize(size);
        }
        /// <summary>
        /// 增加数据流字节长度并返回增加之前的位置
        /// </summary>
        /// <param name="size">增加字节长度</param>
        /// <returns>失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected internal byte* GetBeforeMove(int size)
        {
            return PrepSize(size) ? Data.Pointer.GetBeforeMove(size) : null;
        }
        /// <summary>
        /// 增加数据流字节长度并返回增加之前的位置
        /// </summary>
        /// <param name="size">增加字节长度</param>
        /// <returns>失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected internal int GetIndexBeforeMove(int size)
        {
            return PrepSize(size) ? Data.Pointer.GetIndexBeforeMove(size) : -1;
        }
        /// <summary>
        /// 增加数据流字节长度并返回增加之前的位置
        /// </summary>
        /// <param name="size">增加字节长度</param>
        /// <returns>失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected internal byte* GetCanResizeBeforeMove(int size)
        {
            CanResize = true;
            PrepSize(size);
            return Data.Pointer.GetBeforeMove(size);
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        /// <returns>失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte* GetPrepSizeCurrent(int size)
        {
            return PrepSize(size) ? Data.Pointer.Current : null;
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        /// <returns>失败返回 -1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int GetPrepSizeCurrentIndex(int size)
        {
            return PrepSize(size) ? Data.Pointer.CurrentIndex : -1;
        }
        /// <summary>
        /// 移动当前位置并返回当前位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns>失败返回 0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetMoveSize(int size)
        {
            return PrepSize(size) ? Data.Pointer.GetMoveSize(size) : 0;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool MoveSize(int size)
        {
            if (PrepSize(size))
            {
                Data.Pointer.MoveSize(size);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void MoveSize(UnmanagedStreamBase stream, int size)
        {
            stream.Data.Pointer.MoveSize(size);
        }
        /// <summary>
        /// 写入缓冲区字节数
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal bool SerializeMoveSize(int startIndex)
        {
            if (!IsResizeError)
            {
                int size = Data.Pointer.CurrentIndex - startIndex;
                switch (size & 3)
                {
                    case 1: if (PrepSize(3)) break; return false;
                    case 2: if (PrepSize(2)) break; return false;
                    case 3: if (PrepSize(1)) break; return false;
                }
                Data.Pointer.SerializeMoveSize(startIndex, size);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(Guid value)
        {
            if (PrepSize(sizeof(Guid))) Data.Pointer.Write(ref value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ref Guid value)
        {
            if (PrepSize(sizeof(Guid))) Data.Pointer.Write(ref value);
        }
        /// <summary>
        /// 二进制序列化 JSON 成员序列化补白对齐 4 字节并写入 JSON 字节长度
        /// </summary>
        /// <param name="startIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void JsonSerializeFill(int startIndex)
        {
            if (PrepSize(sizeof(char))) Data.Pointer.JsonSerializeFill(startIndex);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="stringFixed"></param>
        /// <param name="stringLength"></param>
        /// <param name="isCopy"></param>
        /// <returns>不包括补白的字节数，0 表示序列化失败</returns>
        internal int Serialize(char* stringFixed, int stringLength, bool isCopy = true)
        {
            int prepLength = ((stringLength << 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
            byte* writeFixed = GetPrepSizeCurrent(prepLength);
            if (writeFixed != null)
            {
                if (!IsSerializeCopyString && stringLength >= (*((byte*)stringFixed + 1) == 0 ? 3 : 7))
                {
                    byte* writeStart = writeFixed + sizeof(int), writeEnd = writeStart + (prepLength - sizeof(int) * 2), readStart = (byte*)stringFixed, readEnd = (byte*)(stringFixed + stringLength), write, read;
                    int lengthSize = (stringLength <= byte.MaxValue ? 1 : (stringLength <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                    if (*((byte*)stringFixed + 1) != 0)
                    {
                        switch (lengthSize)
                        {
                            case 1: *writeStart++ = 0; break;
                            case sizeof(ushort):
                                *(ushort*)writeStart = 0;
                                writeStart += sizeof(ushort);
                                break;
                            default:
                                *(int*)writeStart = 0;
                                writeStart += sizeof(int);
                                break;
                        }
                        goto CHAR;
                    }
                BYTE:
                    write = writeStart + lengthSize;
                    *write = *readStart;
                    read = readStart + sizeof(char);
                    ++write;
                    if (read != readEnd)
                    {
                        while (*(read + 1) == 0)
                        {
                            *write = *read;
                            read += sizeof(char);
                            ++write;
                            if (read == readEnd) goto END;
                        }
                        switch (lengthSize)
                        {
                            case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                            case sizeof(ushort):
                                *(ushort*)writeStart = (ushort)((read - readStart) >> 1);
                                if ((readEnd - read) <= ((int)byte.MaxValue << 1)) lengthSize = 1;
                                break;
                            default:
                                *(int*)writeStart = (int)((read - readStart) >> 1);
                                if ((readEnd - read) <= ((int)ushort.MaxValue << 1)) lengthSize = (readEnd - read) <= ((int)byte.MaxValue << 1) ? 1 : sizeof(ushort);
                                break;
                        }
                        readStart = read;
                        if ((writeEnd - write) > ((readEnd - read) >> 1) + lengthSize)
                        {
                            writeStart = write;
                            goto CHAR;
                        }
                        goto COPY;
                    }
                END:
                    *(int*)write = 0;
                    switch (lengthSize)
                    {
                        case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                        case sizeof(ushort): *(ushort*)writeStart = (ushort)((read - readStart) >> 1); break;
                        default: *(int*)writeStart = (int)((read - readStart) >> 1); break;
                    }
                    lengthSize = (int)(write - writeFixed);
                    *(int*)writeFixed = (stringLength << 1) + 1;
                    Data.Pointer.MoveSize(lengthSize + (-lengthSize & 3));
                    return lengthSize;
                CHAR:
                    write = writeStart + lengthSize;
                    *(char*)write = *(char*)readStart;
                    read = readStart + sizeof(char);
                    write += sizeof(char);
                    if (read == readEnd) goto END;
                    while (*(read + 1) != 0)
                    {
                        *(char*)write = *(char*)read;
                        read += sizeof(char);
                        write += sizeof(char);
                        if (read == readEnd)
                        {
                            if (write > writeEnd) goto COPY;
                            goto END;
                        }
                        if (write >= writeEnd) goto COPY;
                    }
                    switch (lengthSize)
                    {
                        case 1: *writeStart = (byte)((read - readStart) >> 1); break;
                        case sizeof(ushort):
                            *(ushort*)writeStart = (ushort)((read - readStart) >> 1);
                            if ((readEnd - read) <= ((int)byte.MaxValue << 1)) lengthSize = 1;
                            break;
                        default:
                            *(int*)writeStart = (int)((read - readStart) >> 1);
                            if ((readEnd - read) <= ((int)ushort.MaxValue << 1)) lengthSize = (readEnd - read) <= ((int)byte.MaxValue << 1) ? 1 : sizeof(ushort);
                            break;
                    }
                    readStart = read;
                    if ((writeEnd - write) >= ((readEnd - read) >> 1) + lengthSize)
                    {
                        writeStart = write;
                        goto BYTE;
                    }
                }
            COPY:
                if (isCopy)
                {
                    *(int*)writeFixed = (stringLength <<= 1);
                    AutoCSer.Common.CopyTo(stringFixed, writeFixed + sizeof(int), stringLength);
                    Data.Pointer.MoveSize(prepLength);
                    if ((stringLength & 2) != 0) *(char*)(Data.Pointer.Current - sizeof(char)) = (char)0;
                    return stringLength + sizeof(int);
                }
            }
            return 0;
        }
        /// <summary>
        /// 预增数据流长度并写入长度与数据(4字节对齐)
        /// </summary>
        /// <param name="data">数据,不能为null</param>
        /// <param name="arrayLength">数据数量</param>
        /// <param name="dataSize">复制数据数量</param>
        internal void Serialize(void* data, int arrayLength, int dataSize)
        {
            byte* write = GetBeforeMove((dataSize + (3 + sizeof(int))) & (int.MaxValue - 3));
            if (write != null)
            {
                *(int*)write = arrayLength;
                AutoCSer.Common.CopyTo(data, write + sizeof(int), dataSize);
                Data.Pointer.SerializeFillLeftByteSize(dataSize);
            }
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="pointer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ref Pointer pointer)
        {
            int size = pointer.CurrentIndex;
            if (PrepSize(size)) Data.Pointer.Write(pointer.Byte, size);
        }

        /// <summary>
        /// 写入 64 字节数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, ulong value3)
        {
            unmanagedStream.Data.Pointer.Write(value0, value1, value2, value3);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value, int size)
        {
            unmanagedStream.Data.Pointer.WriteSize(value, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, int size)
        {
            unmanagedStream.Data.Pointer.WriteSize(value0, value1, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, int size)
        {
            unmanagedStream.Data.Pointer.WriteSize(value0, value1, value2, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, ulong value3, int size)
        {
            unmanagedStream.Data.Pointer.WriteSize(value0, value1, value2, value3, size);
        }
    }
}
