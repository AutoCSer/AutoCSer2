using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte*/

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(ulong value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ulong value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ulong? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
        }
    }
}
