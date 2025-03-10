using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 数组索引
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    internal sealed class ArrayIndex<T> : IIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引数据
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get { return array.Length; } }
        /// <summary>
        /// 数组索引
        /// </summary>
        /// <param name="array">索引数据</param>
        private ArrayIndex(T[] array)
        {
            this.array = array;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            foreach (T arrayValue in array)
            {
                if (value.Equals(arrayValue)) return true;
            }
            return false;
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<T> bufferHashSet)
        {
            foreach (T value in array) bufferHashSet.Add(value);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            if (array.Length != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(array.Length);
                buffer.CopyFrom(array);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }

        /// <summary>
        /// 获取数组索引
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ArrayIndex<T> Get(T[] array)
        {
            return array.Length != 0 ? new ArrayIndex<T>(array) : Empty;
        }
        /// <summary>
        /// 空数组索引
        /// </summary>
        internal static readonly ArrayIndex<T> Empty = new ArrayIndex<T>(EmptyArray<T>.Array);
    }
}
