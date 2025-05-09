using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持久化文件缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public sealed class PersistenceFileBuffer : AutoCSer.BinarySerialize.ICustomSerialize<PersistenceFileBuffer>
    {
        /// <summary>
        /// 序列化结束等待锁
        /// </summary>
        internal AutoCSer.Threading.OnceAutoWaitHandle SerializeWaitLock;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 持久化流已写入位置
        /// </summary>
        private long position;
        /// <summary>
        /// 是否持久化回调异常位置文件，false 表示为持久化文件
        /// </summary>
        private bool isPersistenceCallbackExceptionPosition;
        /// <summary>
        /// 调用状态
        /// </summary>
        internal CallStateEnum CallState;
        /// <summary>
        /// 持久化文件缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isPersistenceCallbackExceptionPosition"></param>
        internal PersistenceFileBuffer(ref ByteArrayBuffer buffer, bool isPersistenceCallbackExceptionPosition)
        {
            SerializeWaitLock.Set(new object());
            Buffer = buffer.GetSubArray(0);
            this.isPersistenceCallbackExceptionPosition = isPersistenceCallbackExceptionPosition;
            this.CallState = CallStateEnum.Success;
        }
        /// <summary>
        /// 持久化文件缓冲区
        /// </summary>
        /// <param name="callState"></param>
        /// <param name="isPersistenceCallbackExceptionPosition"></param>
        internal PersistenceFileBuffer(CallStateEnum callState, bool isPersistenceCallbackExceptionPosition)
        {
            this.isPersistenceCallbackExceptionPosition = isPersistenceCallbackExceptionPosition;
            this.CallState = callState;
        }
        /// <summary>
        /// 设置缓冲区字节数
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <param name="position"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBuffer(int bufferSize, long position)
        {
            Buffer.Length = bufferSize;
            this.position = position;
        }
        /// <summary>
        /// 释放序列化结束等待锁
        /// </summary>
        internal void SetSerializeWaitLock()
        {
            Buffer.Length = -1;
            SerializeWaitLock.Set();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<PersistenceFileBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write((int)(byte)CallState + (isPersistenceCallbackExceptionPosition ? (1 << 8) : 0));
            if (CallState == CallStateEnum.Success)
            {
                serializer.Stream.Write(position);
                int serializeSize = serializer.CustomWriteFree(Buffer.Array, Buffer.Start, Buffer.Length);
                if (serializeSize > 0)
                {
                    Buffer.Length = serializeSize;
                    SerializeWaitLock.Set();
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<PersistenceFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            if (deserializer.Read(out state))
            {
                CallState = (CallStateEnum)(byte)state;
                if (CallState == CallStateEnum.Success && deserializer.Read(out position) && deserializer.DeserializeBuffer(ref Buffer, true))
                {
                    var loader = SlaveLoader.GetSessionObject(deserializer);
                    if (loader != null)
                    {
                        switch (state >> 8)
                        {
                            case 0: loader.GetPersistenceFile(position, ref Buffer); return;
                            case 1: loader.GetPersistenceCallbackExceptionPositionFile(position, ref Buffer); return;
                        }
                    }
                    else CallState = CallStateEnum.NotFoundSessionObject;
                }
            }
        }
    }
}
