using AutoCSer.Memory;
using System;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 HashBytes 字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HashBytesFragmentDictionary256<T> : FragmentDictionary256<HashBytes, T>
    {
        /// <summary>
        /// 获取分片索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override int GetIndex(HashBytes key)
        {
            return (int)((uint)(key.HashCode >> 32) & 0xff);
        }
    }
}
