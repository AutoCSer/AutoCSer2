using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 资源释放操作
    /// </summary>
    internal static class IDisposableExtension
    {
#if !NetStandard21
        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Task DisposeAsync(this IDisposable value)
        {
            value.Dispose();
            return AutoCSer.Common.CompletedTask;
        }
#endif
    }
}
