using AutoCSer.CommandService.DeployTask;
using AutoCSer.Net;
using System;
using System.IO;

namespace AutoCSer.TestCase.DeployTask
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    internal sealed class UploadFileService : AutoCSer.CommandService.DeployTask.UploadFileService
    {
        /// <summary>
        /// 文件上传服务
        /// </summary>
        /// <param name="config">文件上传服务端配置</param>
        public UploadFileService(UploadFileServiceConfig config) : base(config) { }
        /// <summary>
        /// 根据上传类型获取文件上传路径
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="type">上传类型</param>
        /// <returns></returns>
        public override string GetPath(CommandServerSocket socket, string type)
        {
            return Path.Combine(AutoCSer.TestCase.Common.Config.UploadPath, type);
        }
    }
}
