using System;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// Example of service controller
    /// 服务控制器示例
    /// </summary>
    internal class ServiceController : IServiceController
    {
        /// <summary>
        /// Test API
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> IServiceController.Add(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }
    }
}
