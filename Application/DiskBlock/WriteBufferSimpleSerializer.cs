using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal sealed class WriteBufferSimpleSerializer<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields)]
#endif
    T> : WriteBufferSerializer
        where T : struct
    {
        /// <summary>
        /// 数据
        /// </summary>
        private T value;
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">data</param>
        internal WriteBufferSimpleSerializer(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化，仅支持 struct 并且不能是自定义序列化类型
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref value);
        }
    }
}
