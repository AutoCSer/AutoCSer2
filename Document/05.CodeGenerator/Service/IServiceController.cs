using System;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// An example of generate the API definition of the client controller interface
    /// 生成客户端控制器接口 API 定义示例
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
        int Add(int left, int right);
    }
}
