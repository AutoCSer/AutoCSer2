using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 写入数据缓冲区，来源于同步 IO 数据缓冲区，需要同步处理数据，否则需要复制数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct WriteBuffer : AutoCSer.BinarySerialize.ICustomSerialize<WriteBuffer>
    {
        /// <summary>
        /// 自定义序列化，必须与 SubArray[byte] 操作结果一致，length[int] + data + fill(4)
        /// </summary>
        private readonly Action<BinarySerializer> serializer;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        public WriteBuffer(SubArray<byte> buffer)
        {
            serializer = null;
            Buffer = buffer;
        }
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        /// <param name="serializer"></param>
        internal WriteBuffer(Action<BinarySerializer> serializer)
        {
            this.serializer = serializer;
            Buffer = default(SubArray<byte>);
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="buffer"></param>
        public static implicit operator WriteBuffer(SubArray<byte> buffer) { return new WriteBuffer(buffer); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="serializer">自定义序列化，必须与 SubArray[byte] 操作结果一致，length[int] + data + fill(4)</param>
        public static implicit operator WriteBuffer(Action<BinarySerializer> serializer) { return new WriteBuffer(serializer); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator WriteBuffer(string value) { return new WriteBuffer(new StringSerializer(value).Serialize); }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<WriteBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            if (this.serializer != null) this.serializer(serializer);
            else serializer.SerializeBuffer(ref Buffer);
        }
        /// <summary>
        /// 反序列化错误日志
        /// </summary>
        private static bool isDeserializeLog;
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<WriteBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            bool isBuffer = AutoCSer.Net.CommandServerSocket.CheckSynchronousIO(deserializer);
            deserializer.DeserializeBuffer(ref Buffer, isBuffer);
            if (!isBuffer && !isDeserializeLog)
            {
                isDeserializeLog = true;
                AutoCSer.LogHelper.ErrorIgnoreException($"{typeof(WriteBuffer).FullName} 反序列化同步 IO 环境错误");
            }
        }
    }
}
