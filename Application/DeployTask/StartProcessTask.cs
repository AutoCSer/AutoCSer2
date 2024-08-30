using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 执行程序任务
    /// </summary>
    internal sealed class StartProcessTask : IDeployTask
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        DeployTaskOperationTypeEnum IDeployTask.OperationType { get { return DeployTaskOperationTypeEnum.StartProcess; } }
        /// <summary>
        /// 运行程序文件名称
        /// </summary>
        private readonly string startFileName;
        /// <summary>
        /// 运行程序参数
        /// </summary>
        private readonly string arguments;
        /// <summary>
        /// 执行任务流程是否等待程序运行结束
        /// </summary>
        private readonly bool isWait;
        /// <summary>
        /// 执行程序任务
        /// </summary>
        /// <param name="startFileName"></param>
        /// <param name="arguments">运行程序参数</param>
        /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
        internal StartProcessTask(string startFileName, string arguments, bool isWait)
        {
            this.startFileName = startFileName;
            this.arguments = arguments;
            this.isWait = isWait;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public async Task<DeployTaskLog> Run()
        {
            FileInfo fileInfo = new FileInfo(startFileName);
            //Task.Delay(Sleep);
            if (isWait)
            {
                using (System.Diagnostics.Process process = await AutoCSer.Deploy.SwitchProcess.GetStartProcessDirectoryAsync(fileInfo, arguments))
                {
                    if (process == null) return new DeployTaskLog { OperationState = DeployTaskOperationStateEnum.NotFoundFile };
                    process.WaitForExit();
                }
            }
            else if (!await AutoCSer.Deploy.SwitchProcess.StartProcessDirectoryAsync(fileInfo, arguments))
            {
                return new DeployTaskLog { OperationState = DeployTaskOperationStateEnum.NotFoundFile };
            }
            return new DeployTaskLog { OperationState = DeployTaskOperationStateEnum.Completed };
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        public Task Cancel() { return AutoCSer.Common.CompletedTask; }
    }
}
