using System;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// The collection enumerates asynchronous tasks
    /// 集合枚举异步任务
    /// </summary>
    public interface IEnumeratorTask : IAsyncDisposable
    {
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        Task<bool> MoveNextAsync();
    }
    /// <summary>
    /// The collection enumerates asynchronous tasks
    /// 集合枚举异步任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumeratorTask<T> : IEnumeratorTask
    {
        /// <summary>
        /// Get the current data
        /// 获取当前数据
        /// </summary>
        T Current { get; }
    }
}
