﻿using System;
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
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref ulong value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}