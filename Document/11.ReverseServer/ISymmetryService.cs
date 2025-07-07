using System;

namespace AutoCSer.Document.ReverseServer
{
    /// <summary>
    /// Interface symmetry API interface definition
    /// 接口对称 API 接口定义
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface ISymmetryService
    {
        /// <summary>
        /// Asynchronous API Definition
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Task<int> AddAsync(int left, int right);
        /// <summary>
        /// Synchronous API definition (It is not recommended to define the synchronous API in interface symmetric services, as the client synchronous blocking mode may cause performance bottlenecks)
        /// 同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
}
