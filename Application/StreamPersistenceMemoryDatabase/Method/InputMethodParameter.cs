using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public abstract class InputMethodParameter : MethodParameter
    {
        /// <summary>
        /// 持久化之前检查参数的调用方法与参数信息
        /// </summary>
        internal CallInputOutputMethodParameter BeforePersistenceMethodParameter;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        public InputMethodParameter(ServerNode node) : base(node) { }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal abstract void Deserialize(AutoCSer.BinaryDeserializer deserializer);
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal abstract bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer);
    }
}
