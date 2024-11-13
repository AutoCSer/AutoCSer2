using AutoCSer.Net;
using System;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 客户端任务创建器
    /// </summary>
    public class DeployTaskClientBuilder : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 发布任务客户端套接字事件
        /// </summary>
        public readonly DeployTaskClient Client;
        /// <summary>
        /// 发布任务编号
        /// </summary>
        public readonly long Identity;
        /// <summary>
        /// 上传文件批次号
        /// </summary>
        internal int UploadIndex;
        /// <summary>
        /// 客户端任务创建器
        /// </summary>
        /// <param name="client">发布任务客户端套接字事件</param>
        /// <param name="identity">发布任务编号</param>
        public DeployTaskClientBuilder(DeployTaskClient client, long identity)
        {
            this.Client = client;
            this.Identity = identity;
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        public void Dispose()
        {
            Client.Client.DeployTaskClient.Cancel(Identity, null);
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await Client.Client.DeployTaskClient.Cancel(Identity);
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="startTime">运行任务时间</param>
        /// <param name="callback">任务状态变更回调委托</param>
        /// <returns></returns>
        public KeepCallbackCommand Start(DateTime startTime, Action<CommandClientReturnValue<DeployTaskLog>, KeepCallbackCommand> callback)
        {
            return Client.Client.DeployTaskClient.Start(Identity, startTime, callback);
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="startTime">运行任务时间</param>
        /// <returns>任务状态变更</returns>
        public EnumeratorCommand<DeployTaskLog> Start(DateTime startTime = default(DateTime))
        {
            return Client.Client.DeployTaskClient.Start(Identity, startTime);
        }
        /// <summary>
        /// 添加执行程序步骤
        /// </summary>
        /// <param name="startFileName">运行程序文件名称</param>
        /// <param name="arguments">运行程序参数</param>
        /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
        /// <returns></returns>
#if NetStandard21
        public ReturnCommand<DeployTaskAppendResult> AppendStartProcess(string startFileName, string? arguments = null, bool isWait = false)
#else
        public ReturnCommand<DeployTaskAppendResult> AppendStartProcess(string startFileName, string arguments = null, bool isWait = false)
#endif
        {
            return Client.Client.DeployTaskClient.AppendStartProcess(Identity, startFileName, arguments ?? string.Empty, isWait);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path">客户端路径</param>
        /// <param name="serverPath">服务端路径</param>
        /// <returns></returns>
        public async Task<DeployTaskUploadFileResult> UploadFileAsync(string path, string serverPath)
        {
            return await new DeployTask.UploadFileClient(this, serverPath).UploadFileAsync(path);
        }
    }
}
