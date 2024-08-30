using AutoCSer.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务客户端
    /// </summary>
    public sealed class DeployTaskClient
    {
        /// <summary>
        /// 发布任务客户端套接字事件
        /// </summary>
        internal readonly IDeployTaskClientSocketEvent Client;
        /// <summary>
        /// 文件上传并发访问锁
        /// </summary>
        internal readonly SemaphoreSlim UploadFileLock;
        /// <summary>
        /// 文件上传缓冲区大小
        /// </summary>
        public readonly int UploadFileBufferSize;
        /// <summary>
        /// 发布任务客户端
        /// </summary>
        /// <param name="client">发布任务客户端套接字事件</param>
        /// <param name="uploadFileCount">文件上传并发数量</param>
        /// <param name="uploadFileBufferSize">文件上传缓冲区大小</param>
        public DeployTaskClient(IDeployTaskClientSocketEvent client, int uploadFileCount = 128, int uploadFileBufferSize = 128 << 10)
        {
            if (uploadFileCount <= 0) uploadFileCount = 1;
            Client = client;
            UploadFileBufferSize = Math.Max(uploadFileBufferSize, 4 << 10);
            UploadFileLock = new SemaphoreSlim(uploadFileCount, uploadFileCount);
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        public async Task<CommandClientReturnValue<DeployTaskClientBuilder>> Create()
        {
            CommandClientReturnValue<long> identity = await Client.DeployTaskClient.Create();
            if (!identity.IsSuccess) return new CommandClientReturnValue<DeployTaskClientBuilder>(identity.ReturnType, identity.ErrorMessage);
            try
            {
                DeployTaskClientBuilder builder = new DeployTaskClientBuilder(this, identity.Value);
                identity.Value = 0;
                return builder;
            }
            finally
            {
                if (identity.Value != 0) await Client.DeployTaskClient.Cancel(identity.Value);
            }
        }
    }
}
