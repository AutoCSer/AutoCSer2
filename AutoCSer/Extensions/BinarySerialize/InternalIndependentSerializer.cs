using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class InternalIndependentSerializer<T>
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
        ///// <summary>
        ///// 序列化
        ///// </summary>
        ///// <param name="serializer"></param>
        //public void Serialize(AutoCSer.BinarySerializer serializer)
        //{
        //    serializer.InternalIndependentSerialize(ref value);
        //}
        /// <summary>
        /// 序列化，仅支持 struct 并且不能是自定义序列化类型
        /// </summary>
        /// <param name="serializer"></param>
        internal void SerializeNotReference(AutoCSer.BinarySerializer serializer)
        {
            serializer.InternalIndependentSerializeNotReference(ref value);
        }
    }
}
