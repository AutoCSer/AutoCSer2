using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回服务端字节数组
    /// </summary>
    public sealed class ResponseServerByteArray : ResponseParameter
    {
        /// <summary>
        /// 服务端字节数组
        /// </summary>
#if NetStandard21
        private readonly byte[]? buffer;
#else
        private readonly byte[] buffer;
#endif
        /// <summary>
        /// 返回服务端字节数组
        /// </summary>
        /// <param name="buffer"></param>
#if NetStandard21
        private ResponseServerByteArray(byte[]? buffer)
#else
        private ResponseServerByteArray(byte[] buffer)
#endif
        {
            this.buffer = buffer;
        }
        /// <summary>
        /// 返回服务端字节数组
        /// </summary>
        /// <param name="state">调用状态</param>
        private ResponseServerByteArray(CallStateEnum state) : base(state) { }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">服务端字节数组</param>
        /// <returns></returns>
#if NetStandard21
        public static implicit operator ResponseServerByteArray(byte[]? value)
#else
        public static implicit operator ResponseServerByteArray(byte[] value)
#endif
        {
            if (value != null)
            {
                if (value.Length != 0) return new ResponseServerByteArray(value);
                return EmptyArray;
            }
            return Null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static implicit operator ResponseServerByteArray(CallStateEnum state)
        {
            int index = (byte)state;
            ResponseServerByteArray value = stateArray[index];
            if (value == null) stateArray[index] = value = new ResponseServerByteArray(state);
            return value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(buffer);
        }

        /// <summary>
        /// null 字节数组
        /// </summary>
        public static readonly ResponseServerByteArray Null = new ResponseServerByteArray(null);
        /// <summary>
        /// 0 字节数组
        /// </summary>
        public static readonly ResponseServerByteArray EmptyArray = new ResponseServerByteArray(EmptyArray<byte>.Array);
        /// <summary>
        /// 调用状态数组
        /// </summary>
        private static readonly ResponseServerByteArray[] stateArray = new ResponseServerByteArray[(byte)CallStateEnum.Callbacked];
    }
}
