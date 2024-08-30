using System;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 HashString 字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FragmentHashStringDictionary256<T> : FragmentDictionary256<HashString, T>
    {
        /// <summary>
        /// 获取分片索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override int GetIndex(HashString key)
        {
            return (int)((uint)(key.HashCode >> 32) & 0xff);
        }
    }
}
