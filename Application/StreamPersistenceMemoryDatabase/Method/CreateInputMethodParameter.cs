using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建调用方法与参数信息
    /// </summary>
    internal sealed class CreateInputMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        private CreateInputMethodParameter() : base(null) { }
        /// <summary>
        /// 复制调用方法与参数信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal override InputMethodParameter Clone(NodeIndex index, int methodIndex) { return null; }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns>下一个参数</returns>
        internal override MethodParameter PersistenceCallback() { throw new InvalidOperationException(); }
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal override MethodParameter PersistenceCallback(CallStateEnum state) { throw new InvalidOperationException(); }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer) { throw new InvalidOperationException(); }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer) { throw new InvalidOperationException(); }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer) { throw new InvalidOperationException(); }

        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal static readonly CreateInputMethodParameter Default = new CreateInputMethodParameter();
    }
}
