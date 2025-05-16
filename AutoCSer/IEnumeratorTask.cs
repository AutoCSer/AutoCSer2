using System;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 异步任务枚举
    /// </summary>
    public interface IEnumeratorTask : IAsyncDisposable
    {
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        Task<bool> MoveNextAsync();
    }
    /// <summary>
    /// 异步任务枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumeratorTask<T> : IEnumeratorTask
    {
        /// <summary>
        /// 获取当前数据
        /// </summary>
        T Current { get; }
    }
}
