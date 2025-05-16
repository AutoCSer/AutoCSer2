using System;
/*ulong;long;uint;int;float;double;decimal;DateTime;TimeSpan;Guid*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, ulong? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, ulong? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(ulong) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}
