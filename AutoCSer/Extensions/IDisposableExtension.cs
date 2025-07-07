using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 资源释放操作
    /// </summary>
    public static class IDisposableExtension
    {
#if !NetStandard21
        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task DisposeAsync(this IDisposable value)
        {
            value.Dispose();
            return AutoCSer.Common.CompletedTask;
        }
#endif
    }
}
