#if !NetStandard21
using System.Threading.Tasks;

namespace System
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
