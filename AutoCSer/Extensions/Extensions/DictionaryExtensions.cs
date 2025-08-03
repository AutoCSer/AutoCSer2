using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Dictionary expansion operation
    /// 字典扩展操作
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Get the matching data array based on the keyword collection
        /// 根据关键字集合获取匹配数据数组
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal static VT[] getValueArray<KT, VT>(this Dictionary<KT, VT> dictionary, KT[] keys)
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
        internal static int removeKeys<KT, VT>(this Dictionary<KT, VT> dictionary, KT[] keys)
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
    /// <summary>
    /// Dictionary expansion operation
    /// 字典扩展操作
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DictionaryExtensions<KT, VT>
#if NetStandard21
             where KT : notnull
#endif
    {
        /// <summary>
        /// Dictionary
        /// </summary>
        private readonly Dictionary<KT, VT> dictionary;
        /// <summary>
        /// Dictionary expansion operation
        /// 字典扩展操作
        /// </summary>
        /// <param name="dictionary"></param>
        public DictionaryExtensions(Dictionary<KT, VT> dictionary)
        {
            this.dictionary = dictionary;
        }
        /// <summary>
        /// Get the matching data array based on the keyword collection
        /// 根据关键字集合获取匹配数据数组
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT[] GetValueArray(KT[] keys)
        {
            return dictionary.getValueArray(keys);
        }
        /// <summary>
        /// Delete the matching data based on the keyword collection
        /// 根据关键字集合删除匹配数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int RemoveKeys(KT[] keys)
        {
            return dictionary.removeKeys(keys);
        }
    }
}
