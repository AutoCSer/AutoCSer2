#if !NetStandard21
using System;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 异步释放资源
    /// </summary>
    public interface IAsyncDisposable
    {
        /// <summary>
        /// 异步释放资源
        /// </summary>
        /// <returns></returns>
        Task DisposeAsync();
    }
}
#endif
