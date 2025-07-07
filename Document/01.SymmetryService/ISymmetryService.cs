using System;

namespace AutoCSer.Document.SymmetryService
{
    /// <summary>
    /// Interface symmetry API interface definition (Interface Symmetric apis do not need to generate client interfaces so set IsCodeGeneratorClientInterface to false)
    /// 接口对称 API 接口定义（接口对称 API 不需要生成客户端接口所以设置 IsCodeGeneratorClientInterface 为 false）
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(IsCodeGeneratorClientInterface = false)]
    public partial interface ISymmetryService
    {
        /// <summary>
        /// Asynchronous API definition
        /// 异步 API 定义
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> AddAsync(int left, int right);
        /// <summary>
        /// Synchronous API definition (It is not recommended to define the synchronous API in interface symmetric services, as the client synchronous blocking mode may cause performance bottlenecks)
        /// 同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        //[AutoCSer.Net.CommandServerMethod(MethodIndex = 2)]
        int Add(int left, int right);
    }
}
