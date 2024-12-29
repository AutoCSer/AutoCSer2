using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务服务端
    /// </summary>
    public class DeployTaskService : CommandServerBindController, IDeployTaskService
    {
        /// <summary>
        /// 默认备份目录名称
        /// </summary>
        public const string DefaultBackupDirectoryName = "Backup";

        /// <summary>
        /// 任务标识生成器
        /// </summary>
        public readonly DistributedMillisecondIdentityGenerator IdentityGenerator;
        /// <summary>
        /// 上传文件目录
        /// </summary>
        public readonly string UploadFilePath;
        /// <summary>
        /// 切换目录名称
        /// </summary>
        public readonly string SwitchDirectoryName;
        /// <summary>
        /// 备份目录名称
        /// </summary>
        public readonly string BackupDirectoryName;
        /// <summary>
        /// 发布任务集合，需要增加超时释放机制
        /// </summary>
        public readonly Dictionary<long, DeployTaskBuilder> Tasks = AutoCSer.Extensions.DictionaryCreator.CreateLong<DeployTaskBuilder>();
        /// <summary>
        /// 发布任务服务端
        /// </summary>
        /// <param name="identityGenerator">任务标识生成器</param>
        /// <param name="uploadFilePath">上传文件目录</param>
        /// <param name="switchDirectoryName">切换目录名称</param>
        /// <param name="backupDirectoryName">切换目录名称</param>
#if NetStandard21
        public DeployTaskService(DistributedMillisecondIdentityGenerator? identityGenerator = null, string? uploadFilePath = null, string switchDirectoryName = AutoCSer.Deploy.SwitchProcess.DefaultSwitchDirectoryName, string backupDirectoryName = DefaultBackupDirectoryName)
#else
        public DeployTaskService(DistributedMillisecondIdentityGenerator identityGenerator = null, string uploadFilePath = null, string switchDirectoryName = AutoCSer.Deploy.SwitchProcess.DefaultSwitchDirectoryName, string backupDirectoryName = DefaultBackupDirectoryName)
#endif
        {
            IdentityGenerator = identityGenerator
             ?? AutoCSer.Configuration.Common.Get<DistributedMillisecondIdentityGenerator>()?.Value
             ?? new DistributedMillisecondIdentityGenerator();
            UploadFilePath = uploadFilePath ?? AutoCSer.Common.ApplicationDirectory.FullName;
            SwitchDirectoryName = switchDirectoryName ?? AutoCSer.Deploy.SwitchProcess.DefaultSwitchDirectoryName;
            BackupDirectoryName = backupDirectoryName ?? DefaultBackupDirectoryName;
        }
        /// <summary>
        /// 发布任务创建器
        /// </summary>
        /// <returns></returns>
        protected virtual DeployTaskBuilder createBuilder()
        {
            return new DeployTaskBuilder(this);
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns>任务ID</returns>
        public virtual Task<long> Create(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue)
        {
            DeployTaskBuilder builder = createBuilder();
            Tasks.Add(builder.Identity, builder);
            return Task.FromResult(builder.Identity);
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <returns>返回 false 表示没有找到任务或者任务已经启动不允许取消</returns>
        public virtual Task<bool> Cancel(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity)
        {
            var builder = default(DeployTaskBuilder);
            if (Tasks.TryGetValue(taskIdentity, out builder)) return builder.Cancel();
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <param name="callback">任务状态变更回调委托</param>
        /// <returns>返回 false 表示没有找到任务、任务已经取消或者任务已经调用启动</returns>
        public virtual Task Start(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, DateTime startTime, CommandServerKeepCallback<DeployTaskLog> callback)
        {
            bool IsCallback = false;
            try
            {
                var builder = default(DeployTaskBuilder);
                if (Tasks.TryGetValue(taskIdentity, out builder))
                {
                    IsCallback = true;
                    return builder.Start(startTime, callback);
                }
                else
                {
                    DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.NotFoundTask);
                    IsCallback = true;
                }
            }
            finally
            {
                if (!IsCallback) DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.Exception);
            }
            return AutoCSer.Common.CompletedTask;
        }

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
        public virtual Task<DeployTaskAppendResult> AppendStartProcess(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, string startFileName, string arguments, bool isWait)
        {
            var builder = default(DeployTaskBuilder);
            if (Tasks.TryGetValue(taskIdentity, out builder))
            {
                return builder.Append(new DeployTask.StartProcessTask(startFileName, arguments, isWait));
            }
            return Task.FromResult(new DeployTaskAppendResult { AppendState = DeployTaskAppendStateEnum.NotFound });
        }
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
        public virtual Task<DeployTaskAppendResult> AppendCopyUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string destinationPath, string checkSwitchFileName, bool isBackup)
        {
            var builder = default(DeployTaskBuilder);
            if (Tasks.TryGetValue(taskIdentity, out builder))
            {
                return builder.Append(new DeployTask.CopyFileTask(this, taskIdentity, index, destinationPath, checkSwitchFileName, isBackup));
            }
            return Task.FromResult(new DeployTaskAppendResult { AppendState = DeployTaskAppendStateEnum.NotFound });
        }

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
        /// <param name="callback">比较结果回调委托，返回 null 表示没有找到任务或者异常</param>
        /// <returns></returns>
#if NetStandard21
        public virtual Task GetDifferent(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, CommandServerCallback<bool[]?> callback)
#else
        public virtual Task GetDifferent(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, CommandServerCallback<bool[]> callback)
#endif
        {
            var builder = default(DeployTaskBuilder);
            if (Tasks.TryGetValue(taskIdentity, out builder))
            {
                AutoCSer.Threading.CatchTask.Add(builder.GetDifferent(index, bootPath, path, fileTimes, callback));
            }
            else callback.Callback(default(bool[]));
            return AutoCSer.Common.CompletedTask;
        }
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
        public virtual Task CreateUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, string path, DeployTask.FileTime fileTime, CommandServerCallback<DeployTask.CreateUploadFileResult> callback)
        {
            var builder = default(DeployTaskBuilder);
            if (Tasks.TryGetValue(taskIdentity, out builder)) return builder.CreateUploadFile(index, path, fileTime, callback);
            callback.Callback(default(DeployTask.CreateUploadFileResult));
            return AutoCSer.Common.CompletedTask;
        }
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
        public virtual Task WriteUploadFile(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity, int index, DeployTask.UploadFileBuffer buffer, CommandServerCallback<bool> callback)
        {
            var builder = default(DeployTaskBuilder);
            if (buffer.Buffer.Length != 0 && Tasks.TryGetValue(taskIdentity, out builder)) AutoCSer.Threading.CatchTask.Add(builder.WriteUploadFile(index, buffer, callback));
            else
            {
                callback.Callback(false);
                buffer.Free();
            }
            return AutoCSer.Common.CompletedTask;
        }
    }
}
