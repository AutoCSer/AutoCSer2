using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 上传文件最后移动文件操作任务
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class UploadCompletedTask : IStepTask
    {
        /// <summary>
        /// 上传完成操作数据文件名称
        /// </summary>
        public readonly string UploadCompletedDataFileName;
        /// <summary>
        /// 上传文件最后移动文件操作任务
        /// </summary>
        private UploadCompletedTask()
        {
#if NetStandard21
            UploadCompletedDataFileName = string.Empty;
#endif
        }
        /// <summary>
        /// 上传文件最后移动文件操作任务
        /// </summary>
        /// <param name="uploadCompletedDataFileName">上传完成操作数据文件名称</param>
        internal UploadCompletedTask(string uploadCompletedDataFileName)
        {
            UploadCompletedDataFileName = uploadCompletedDataFileName;
        }
        /// <summary>
        /// 上传文件最后移动文件操作任务信息
        /// </summary>
        public StepTaskData StepTaskData { get { return new StepTaskData(StepTypeEnum.UploadCompleted, this); } }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task<OperationStateEnum> IStepTask.Run()
        {
            return UploadCompletedFiles.Completed(UploadCompletedDataFileName);
        }
        /// <summary>
        /// 移除任务
        /// </summary>
        /// <returns></returns>
        Task IStepTask.Remove()
        {
            return AutoCSer.Common.CompletedTask;
        }
    }
}
