using System;
using System.Threading;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 上传文件缓冲区
    /// </summary>
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[0])]
    public sealed class UploadFileBuffer : AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>
    {
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal byte[] Buffer;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// 上传文件缓冲区
        /// </summary>
        private UploadFileBuffer()
        {
            Buffer = EmptyArray<byte>.Array;
        }
        /// <summary>
        /// 上传文件缓冲区
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        internal UploadFileBuffer(byte[] buffer)
        {
            this.Buffer = buffer;
        }
        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        internal void Free()
        {
            byte[] buffer = Interlocked.Exchange(ref this.Buffer, EmptyArray<byte>.Array);
            if (!object.ReferenceEquals(buffer, EmptyArray<byte>.Array))
            {
                Monitor.Enter(bufferLock);
                buffers.TryAdd(buffer);
                Monitor.Exit(bufferLock);
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(Buffer, Length);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var buffer = default(byte[]);
            Length = deserializer.DeserializeBuffer(getBufferHandle, out buffer);
            Buffer = buffer ?? EmptyArray<byte>.Array;
        }

        /// <summary>
        /// 发布任务配置
        /// </summary>
        internal static readonly DeployTaskConfig DeployTaskConfig;
        /// <summary>
        /// 缓冲区集合
        /// </summary>
        private static LeftArray<byte[]> buffers;
        /// <summary>
        /// 缓冲区集合访问锁
        /// </summary>
        private static readonly object bufferLock = new object();
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
#if NetStandard21
        private static byte[]? getBuffer(int length)
#else
        private static byte[] getBuffer(int length)
#endif
        {
            if (length <= DeployTaskConfig.UploadFileBufferSize)
            {
                var buffer = default(byte[]);
                Monitor.Enter(bufferLock);
                if (buffers.TryPop(out buffer))
                {
                    Monitor.Exit(bufferLock);
                    return buffer;
                }
                Monitor.Exit(bufferLock);
                return AutoCSer.Common.GetUninitializedArray<byte>(DeployTaskConfig.UploadFileBufferSize);
            }
            return null;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
#if NetStandard21
        private static readonly Func<int, byte[]?> getBufferHandle = getBuffer;
#else
        private static readonly Func<int, byte[]> getBufferHandle = getBuffer;
#endif
        static UploadFileBuffer()
        {
            DeployTaskConfig = AutoCSer.Configuration.Common.Get<DeployTaskConfig>()?.Value ?? new DeployTaskConfig();
            buffers = new LeftArray<byte[]>(DeployTaskConfig.UploadFileBufferCount);
        }
    }
}
