using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EnumDeserialize<T> : AutoCSer.TextSerialize.EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举名称查找数据
        /// </summary>
        protected readonly static StateSearcher enumSearcher = new StateSearcher(ref enumSearchData);
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        protected static void deserialize(XmlDeserializer deserializer, ref T value)
        {
            int index = enumSearcher.SearchEnum(deserializer);
            if (index != -1) value = enumValues[index];
            else if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">第一个枚举索引</param>
        /// <param name="nextIndex">第二个枚举索引</param>
        protected static void getIndex(XmlDeserializer deserializer, ref T value, out int index, ref int nextIndex)
        {
            if ((index = enumSearcher.SearchFlagEnum(deserializer)) == -1)
            {
                if (deserializer.Config.IsMatchEnum)
                {
                    deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    return;
                }
                else
                {
                    do
                    {
                        if (deserializer.IsNextFlagEnum() == 0) return;
                    }
                    while ((index = enumSearcher.SearchFlagEnum(deserializer)) == -1);
                }
            }
            do
            {
                if (deserializer.IsNextFlagEnum() == 0)
                {
                    value = enumValues[index];
                    return;
                }
                if ((nextIndex = enumSearcher.SearchFlagEnum(deserializer)) != -1) break;
            }
            while (true);
        }
    }
}
