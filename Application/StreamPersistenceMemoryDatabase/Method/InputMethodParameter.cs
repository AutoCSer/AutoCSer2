using System;
using System.Runtime.CompilerServices;

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
        /// 清除信息
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void clearClone()
        {
            LinkNext = null;
            BeforePersistenceMethodParameter = null;
            persistenceCallbackExceptionPosition = 0;
            IsPersistenceCallback = false;
        }
        /// <summary>
        /// 复制调用方法与参数信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal abstract InputMethodParameter Clone(NodeIndex index, int methodIndex);
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
