using System;

namespace AutoCSer.Document.SymmetryService
{
    /// <summary>
    /// Interface symmetry API implementation
    /// 接口对称 API 实现
    /// </summary>
    internal sealed class SymmetryService : ISymmetryService
    {
        /// <summary>
        /// Asynchronous API implementation
        /// 异步 API 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        async System.Threading.Tasks.Task<int> ISymmetryService.AddAsync(int left, int right)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return left + right;
        }
        /// <summary>
        /// Synchronous API implementation
        /// 同步 API 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
    }
}
