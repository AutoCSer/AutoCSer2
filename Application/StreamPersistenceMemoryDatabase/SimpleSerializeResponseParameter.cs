using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SimpleSerializeResponseParameter<T> : ResponseParameter<T>
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        internal SimpleSerializeResponseParameter() { }
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="value"></param>
        internal SimpleSerializeResponseParameter(T value) : base(value) { }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref Value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.SimpleDeserialize(ref Value);
        }
        /// <summary>
        /// 创建持续回调返回参数
        /// </summary>
        /// <returns></returns>
        internal override KeepCallbackResponseParameter CreateKeepCallback()
        {
            return new KeepCallbackResponseParameter(new ResponseParameterSimpleSerializer<T>(Value.ReturnValue));
        }
    }
}
