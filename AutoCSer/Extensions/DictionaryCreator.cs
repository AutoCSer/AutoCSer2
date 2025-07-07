using System;
using System.Collections.Generic;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Create the dictionary
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// Create a dictionary
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns>Dictionary</returns>
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
