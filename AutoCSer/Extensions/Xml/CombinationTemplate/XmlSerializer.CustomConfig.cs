using System;
/*DateTime;TimeSpan*/

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(DateTime value)
        {
            int size = CustomConfig.Write(this, value);
            if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, DateTime value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, DateTime? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}
