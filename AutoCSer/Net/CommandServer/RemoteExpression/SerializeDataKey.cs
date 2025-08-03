using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct SerializeDataKey : IEquatable<SerializeDataKey>
    {
        /// <summary>
        /// 远程表达式序列化数据
        /// </summary>
        private readonly byte[] data;
        /// <summary>
        /// 远程表达式反序列化数据
        /// </summary>
        internal AutoCSer.Memory.Pointer DeserializeData;
        /// <summary>
        /// 远程表达式关键字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="hashCode"></param>
        internal SerializeDataKey(byte* data, int size, int hashCode)
        {
            this.data = EmptyArray<byte>.Array;
            DeserializeData = new AutoCSer.Memory.Pointer(data, size, hashCode);
        }
        /// <summary>
        /// 远程表达式关键字
        /// </summary>
        /// <param name="key"></param>
        internal SerializeDataKey(ref SerializeDataKey key)
        {
            DeserializeData = key.DeserializeData;
            data = DeserializeData.GetBufferArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SerializeDataKey other)
        {
            if (data.Length != 0)
            {
                if (other.data.Length != 0) return AutoCSer.Common.SequenceEqual(data, other.data);
                return other.DeserializeData.BufferSequenceEqual(data);
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
            return Equals(obj.castValue<SerializeDataKey>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DeserializeData.CurrentIndex;
        }
    }
}
