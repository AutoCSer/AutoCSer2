using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Guid*/

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref ulong value)
        {
            serializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref ulong? value)
        {
            if (IsValue() == 0) value = default(ulong);
            else
            {
                ulong newValue = default(ulong);
                XmlDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref ulong? value)
        {
            serializer.XmlDeserialize(ref value);
        }
    }
}
