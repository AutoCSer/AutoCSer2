using System;
/*char;bool;Guid*/

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
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, char value)
        {
            serializer.XmlSerialize(value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void XmlSerialize(char? value)
        {
            if (value.HasValue) XmlSerialize(value.Value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, char? value)
        {
            serializer.XmlSerialize(value);
        }
    }
}
