using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传服务接口 客户端接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IUploadFileService))]
    public partial interface IUploadFileServiceClientController
    {
    }
}
