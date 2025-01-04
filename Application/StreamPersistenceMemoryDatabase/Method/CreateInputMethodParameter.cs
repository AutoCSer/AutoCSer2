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
        /// <param name="node"></param>
        internal CreateInputMethodParameter(ServerNode node) : base(node) { }
        /// <summary>
        /// 复制调用方法与参数信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
#if NetStandard21
        internal override InputMethodParameter? Clone(NodeIndex index, int methodIndex)
#else
        internal override InputMethodParameter Clone(NodeIndex index, int methodIndex)
#endif
        {
            return null;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns>下一个参数</returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback()
#else
        internal override MethodParameter PersistenceCallback()
#endif
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback(CallStateEnum state)
#else
        internal override MethodParameter PersistenceCallback(CallStateEnum state)
#endif
        {
            throw new InvalidOperationException(); 
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#else
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#endif
        {
            throw new InvalidOperationException(); 
        }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer) { throw new InvalidOperationException(); }
        /// <summary>
        /// 输入参数反序列化（初始化加载持久化数据）
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer) { throw new InvalidOperationException(); }

        ///// <summary>
        ///// 创建调用方法与参数信息
        ///// </summary>
        //internal static readonly CreateInputMethodParameter Default = new CreateInputMethodParameter();
    }
}
