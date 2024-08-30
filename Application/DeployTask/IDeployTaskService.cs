using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务服务端接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 1, TaskQueueWaitCount = 256)]
    public interface IDeployTaskService
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns>任务ID</returns>
        Task<long> Create(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue);
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <returns>返回 false 表示没有找到任务或者任务已经启动不允许取消</returns>
        Task<bool> Cancel(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity);
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <param name="callback">任务状态变更回调委托</param>
        /// <returns></returns>
        [AutoCSer.Net.CommandServerMethod(AutoCancelKeep = false)]
        Task Start(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, DateTime startTime, CommandServerKeepCallback<DeployTaskLog> callback);

        /// <summary>
        /// 添加执行程序步骤
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startFileName">运行程序文件名称</param>
        /// <param name="arguments">运行程序参数</param>
        /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
        /// <returns></returns>
        Task<DeployTaskAppendResult> AppendStartProcess(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, string startFileName, string arguments, bool isWait);
        /// <summary>
        /// 添加复制文件步骤
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="destinationPath">文件目标路径</param>
        /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
        /// <param name="isBackup">是否备份历史文件</param>
        /// <returns></returns>
        Task<DeployTaskAppendResult> AppendCopyUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string destinationPath, string checkSwitchFileName, bool isBackup);

        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="bootPath">服务端文件根目录</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTimes">文件信息集合</param>
        /// <param name="callback">比较结果回调委托</param>
        /// <returns></returns>
        Task GetDifferent(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, CommandServerCallback<bool[]> callback);
        /// <summary>
        /// 初始化上传文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTime">文件信息</param>
        /// <param name="callback">初始化上传文件结果回调委托</param>
        /// <returns></returns>
        Task CreateUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string path, DeployTask.FileTime fileTime, CommandServerCallback<DeployTask.CreateUploadFileResult> callback);
        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">写入文件流索引</param>
        /// <param name="buffer">上传文件缓冲区</param>
        /// <param name="callback">上传文件数据结果回调委托，上传文件流标识匹配失败则返回 false</param>
        /// <returns></returns>
        Task WriteUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, DeployTask.UploadFileBuffer buffer, CommandServerCallback<bool> callback);
    }
}
