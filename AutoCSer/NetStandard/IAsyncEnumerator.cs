#if !NetStandard21
using System;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 异步枚举器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncEnumerator<T> : IAsyncDisposable
    {
        /// <summary>
        /// 当前数据
        /// </summary>
        T Current { get; }
        /// <summary>
        /// 转到下一个数据元素
        /// </summary>
        /// <returns>是否存在下一个数据元素</returns>
        Task<bool> MoveNextAsync();
    }
}
#endif