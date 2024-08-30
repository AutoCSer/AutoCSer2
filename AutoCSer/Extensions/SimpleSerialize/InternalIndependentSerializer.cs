using System;

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class InternalIndependentSerializer<T> where T : struct
    {
        /// <summary>
        /// 数据
        /// </summary>
        private T value;
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">数据</param>
        public InternalIndependentSerializer(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">数据</param>
        public InternalIndependentSerializer(ref T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public unsafe void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref value);
        }
    }
}
