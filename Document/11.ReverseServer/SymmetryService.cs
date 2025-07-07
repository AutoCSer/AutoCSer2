using System;

namespace AutoCSer.Document.ReverseServer
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
        Task<int> ISymmetryService.AddAsync(int left, int right) { return Task.FromResult(left + right); }
        /// <summary>
        /// Synchronous API implementation (It is not recommended to define the synchronous API in interface symmetric services, as the client synchronous blocking mode may cause performance bottlenecks)
        /// 同步 API 实现（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Add(int left, int right) { return left + right; }
    }
}
