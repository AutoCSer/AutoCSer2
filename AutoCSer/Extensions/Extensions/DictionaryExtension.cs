using System;
using System.Collections.Generic;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字典相关扩展
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Get the matching data array based on the keyword collection
        /// 根据关键字集合获取匹配数据数组
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static VT[] getValueArray<KT, VT>(this Dictionary<KT, VT> dictionary, KT[] keys)
#if NetStandard21
             where KT : notnull
#endif
        {
            if (keys != null && keys.Length != 0)
            {
                VT[] values = new VT[keys.Length];
                var value = default(VT);
                int index = 0;
                foreach (KT key in keys)
                {
                    if (key != null && dictionary.TryGetValue(key, out value)) values[index] = value;
                    ++index;
                }
                return values;
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// Delete the matching data based on the keyword collection
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        public static int removeKeys<KT, VT>(this Dictionary<KT, VT> dictionary, KT[] keys)
#if NetStandard21
             where KT : notnull
#endif
        {
            int count = 0;
            foreach (KT key in keys)
            {
                if (key != null && dictionary.Remove(key)) ++count;
            }
            return count;
        }
    }
}
