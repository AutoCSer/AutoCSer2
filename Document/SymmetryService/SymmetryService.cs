using System;

namespace AutoCSer.Document.SymmetryService
{
    /// <summary>
    /// Interface symmetry service
    /// 接口对称服务
    /// </summary>
    internal sealed class SymmetryService : ISymmetryService
    {
        /// <summary>
        /// Asynchronous API definition
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> ISymmetryService.AddAsync(int left, int right) { return Task.FromResult(left + right); }
        /// <summary>
        /// Synchronization API definition
        /// 同步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
    }
}
