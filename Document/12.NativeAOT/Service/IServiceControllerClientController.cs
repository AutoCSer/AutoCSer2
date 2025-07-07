using System;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// Example of service controller client interface 
    /// 服务控制器客户端接口示例
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServiceController))]
    public partial interface IServiceControllerClientController
    {
    }
}
