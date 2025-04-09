using System;
using System.Collections.Generic;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<HashSubString, T> CreateHashSubString<T>()
        {
#if AOT
            return new Dictionary<HashSubString, T>(AutoCSer.EquatableComparer<HashSubString>.Default);
#else
            return new Dictionary<HashSubString, T>();
#endif
        }
    }
}
