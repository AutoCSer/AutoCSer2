using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    internal sealed class RequestParameterJsonSerializer<T> : RequestParameterSerializer
    {
        /// <summary>
        /// 数据
        /// </summary>
#if NetStandard21
        private T? value;
#else
        private T value;
#endif
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">数据</param>
#if NetStandard21
        internal RequestParameterJsonSerializer(T? value)
#else
        internal RequestParameterJsonSerializer(T value)
#endif
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.JsonSerializeBuffer(ref value);
        }
    }
}
