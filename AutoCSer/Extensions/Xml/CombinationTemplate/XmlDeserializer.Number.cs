using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte*/

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">data</param>
        public void XmlDeserialize(ref ulong value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}
