using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据缓冲区反序列化
    /// </summary>
    public abstract class ReadBufferDeserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal abstract void Deserialize(AutoCSer.BinaryDeserializer deserializer);

#if AOT
        /// <summary>
        /// 从独立数据缓冲区反序列化内部成员对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void InternalIndependentDeserializeNotReference<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields)]T>(AutoCSer.BinaryDeserializer deserializer, ref T value) where T : struct
        {
            deserializer.InternalIndependentDeserializeNotReference(ref value);
        }
#endif
    }
}
