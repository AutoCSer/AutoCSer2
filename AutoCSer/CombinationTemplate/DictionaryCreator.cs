using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;DateTime,DateTime*/

namespace AutoCSer
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
        public static System.Collections.Generic.Dictionary<ulong, T> CreateULong<T>()
        {
#if AOT
            return new System.Collections.Generic.Dictionary<ulong, T>(AutoCSer.ULongComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<ulong, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<ulong, T> CreateULong<T>(int capacity)
        {
#if AOT
            return new System.Collections.Generic.Dictionary<ulong, T>(capacity, AutoCSer.ULongComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<ulong, T>(capacity);
#endif
        }
    }
#if AOT
    /// <summary>
    /// 字典关键字比较器
    /// </summary>
    public sealed class ULongComparer : System.Collections.Generic.IEqualityComparer<ulong>
    {
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<ulong>.Equals(ulong left, ulong right)
        {
            return left == right;
        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<ulong>.GetHashCode(ulong value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 默认比较器
        /// </summary>
        public static readonly ULongComparer Default = new ULongComparer();
    }
#endif
}
