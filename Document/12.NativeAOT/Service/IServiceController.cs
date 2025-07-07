using System;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// Example of service controller interface
    /// 服务控制器接口示例
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface IServiceController
    {
        /// <summary>
        /// Test API
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<int> Add(int left, int right);
    }
}
