using System;

namespace AutoCSer.Json
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
        protected readonly static StateSearcher enumSearcher = new StateSearcher(enumSearchData);
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        protected static void deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            int index = enumSearcher.SearchString(jsonDeserializer);
            if (index != -1) value = enumValues[index];
            else jsonDeserializer.CheckMatchEnum();
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        /// <param name="index">第一个枚举索引</param>
        /// <param name="nextIndex">第二个枚举索引</param>
        protected static void getIndex(JsonDeserializer jsonDeserializer, ref T value, out int index, ref int nextIndex)
        {
            if ((index = enumSearcher.SearchFlagEnum(jsonDeserializer)) == -1)
            {
                if (!jsonDeserializer.Config.IsMatchEnum)
                {
                    do
                    {
                        if (!jsonDeserializer.CheckQuote()) return;
                    }
                    while ((index = enumSearcher.NextFlagEnum(jsonDeserializer)) == -1);
                }
                else
                {
                    jsonDeserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    return;
                }
            }
            do
            {
                if (jsonDeserializer.Quote == 0)
                {
                    value = enumValues[index];
                    return;
                }
                if ((nextIndex = enumSearcher.NextFlagEnum(jsonDeserializer)) != -1) break;
            }
            while (jsonDeserializer.State == DeserializeStateEnum.Success);
        }
    }
}
