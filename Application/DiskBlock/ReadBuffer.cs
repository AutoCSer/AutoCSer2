﻿using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据缓冲区，来源于同步 IO 数据缓冲区，需要同步处理数据，否则需要复制数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct ReadBuffer : AutoCSer.BinarySerialize.ICustomSerialize<ReadBuffer>
    {
        /// <summary>
        /// 读取数据状态
        /// </summary>
        internal ReadBufferStateEnum State;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 自定义序列化
        /// </summary>
        private readonly Action<BinaryDeserializer> deserializer;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        /// <param name="state"></param>
        internal ReadBuffer(ReadBufferStateEnum state)
        {
            State = state;
            Buffer = default(SubArray<byte>);
            deserializer = null;
        }
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal ReadBuffer(ref SubArray<byte> buffer)
        {
            State = ReadBufferStateEnum.Success;
            Buffer = buffer;
            deserializer = null;
        }
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        /// <param name="deserializer"></param>
        internal ReadBuffer(Action<BinaryDeserializer> deserializer)
        {
            State = default(ReadBufferStateEnum);
            Buffer = default(SubArray<byte>);
            this.deserializer = deserializer;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="deserializer"></param>
        public static implicit operator ReadBuffer(Action<BinaryDeserializer> deserializer) { return new ReadBuffer(deserializer); }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(byte[] buffer)
        {
            State = ReadBufferStateEnum.Success;
            Buffer.Set(buffer);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ReadBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write((int)State);
            if (State == ReadBufferStateEnum.Success) serializer.SerializeBuffer(ref Buffer);
        }
        /// <summary>
        /// 反序列化错误日志
        /// </summary>
        private static bool isDeserializeLog;
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ReadBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            if (deserializer.Read(out state))
            {
                State = (ReadBufferStateEnum)state;
                if (state == (int)ReadBufferStateEnum.Success)
                {
                    if (this.deserializer != null) this.deserializer(deserializer);
                    else
                    {
                        bool isBuffer = AutoCSer.Net.CommandClientSocket.CheckSynchronousIO(deserializer);
                        deserializer.DeserializeBuffer(ref Buffer, isBuffer);
                        if (!isBuffer && !isDeserializeLog)
                        {
                            isDeserializeLog = true;
                            AutoCSer.LogHelper.ErrorIgnoreException($"{typeof(ReadBuffer).FullName} 反序列化同步 IO 环境错误");
                        }
                    }
                }
            }
        }
    }
}