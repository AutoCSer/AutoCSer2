using AutoCSer.Diagnostics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 执行程序任务
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class StartProcessTask : ProcessInfo, IStepTask
    {
        /// <summary>
        /// 执行任务流程是否等待程序运行结束
        /// </summary>
        public readonly bool IsWaitForExit;
        /// <summary>
        /// 执行程序任务信息
        /// </summary>
        public StepTaskData StepTaskData { get { return new StepTaskData(StepTypeEnum.StartProcess, this); } }
        /// <summary>
        /// 执行程序任务
        /// </summary>
        private StartProcessTask() { }
        /// <summary>
        /// 执行程序任务
        /// </summary>
        /// <param name="fileName">运行文件</param>
        /// <param name="isWaitForExit">执行任务流程是否等待程序运行结束</param>
        /// <param name="arguments">命令行参数集合</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="windowStyle">进程启动时要使用的窗口状态</param>
        /// <param name="useShellExecute">是否使用操作系统外壳启动进程</param>
        /// <param name="isErrorDialog">是否显示错误弹窗</param>
        /// <param name="isCreateWindow">是否在新窗口中启动进程</param>
#if NetStandard21
        public StartProcessTask(string fileName, bool isWaitForExit = false, string[]? arguments = null, string? workingDirectory = null
#else
        public StartProcessTask(string fileName, bool isWaitForExit = false, string[] arguments = null, string workingDirectory = null
#endif
            , ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal, bool useShellExecute = true, bool isErrorDialog = false, bool isCreateWindow = true)
            : base(fileName, arguments, workingDirectory ?? string.Empty, windowStyle, useShellExecute, isErrorDialog, isCreateWindow)
        {
            IsWaitForExit = isWaitForExit;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        async Task<OperationStateEnum> IStepTask.Run()
        {
            var process = await StartAsync();
            if (process == null) return OperationStateEnum.NotFoundFile;
            using (process)
            {
                if (IsWaitForExit) process.WaitForExit();
            }
            return OperationStateEnum.Success;
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
