using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务创建器
    /// </summary>
    public class DeployTaskBuilder
    {
        /// <summary>
        /// 添加任务步骤成功
        /// </summary>
        protected static readonly Task<DeployTaskAppendResult> appendSuccessTask = Task.FromResult(new DeployTaskAppendResult { AppendState = DeployTaskAppendStateEnum.Success });

        /// <summary>
        /// 发布任务编号
        /// </summary>
        public readonly long Identity;
        /// <summary>
        /// 发布任务服务端
        /// </summary>
        protected readonly DeployTaskService service;
        /// <summary>
        /// 发布任务集合
        /// </summary>
        protected readonly List<IDeployTask> tasks = new List<IDeployTask>();
        /// <summary>
        /// 写入文件流集合
        /// </summary>
#if NetStandard21
        private readonly List<DeployTask.TaskFileStream?> files = new List<DeployTask.TaskFileStream?>();
#else
        private readonly List<DeployTask.TaskFileStream> files = new List<DeployTask.TaskFileStream>();
#endif
        /// <summary>
        /// 任务状态变更回调委托
        /// </summary>
#if NetStandard21
        private CommandServerKeepCallback<DeployTaskLog>? callback;
#else
        private CommandServerKeepCallback<DeployTaskLog> callback;
#endif
        /// <summary>
        /// 发布任务创建器
        /// </summary>
        /// <param name="service">发布任务服务端</param>
        public DeployTaskBuilder(DeployTaskService service)
        {
            this.service = service;
            Identity = service.IdentityGenerator.GetNext();
        }
        /// <summary>
        /// 释放写入文件流
        /// </summary>
        /// <returns></returns>
        private async Task disposeFiles()
        {
            if (files.Count == 0) return;
            foreach (var file in files)
            {
                if (file != null) await file.Close();
            }
            files.Clear();
        }
        /// <summary>
        /// 添加任务步骤
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual Task<DeployTaskAppendResult> Append(IDeployTask task)
        {
            tasks.Add(task);
            return appendSuccessTask;
        }
        /// <summary>
        /// 添加自定义任务步骤
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="getTask">获取执行任务</param>
        /// <param name="getCancelTask">获取取消任务</param>
#if NetStandard21
        public virtual async Task<DeployTaskAppendResult> AppendCustom(ushort operationType, Func<Task<DeployTaskLog>> getTask, Func<Task>? getCancelTask = null)
#else
        public virtual async Task<DeployTaskAppendResult> AppendCustom(ushort operationType, Func<Task<DeployTaskLog>> getTask, Func<Task> getCancelTask = null)
#endif
        {
            return await Append(new CustomDeployTask(operationType, getTask, getCancelTask));
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        public virtual async Task Cancel()
        {
            service.Tasks.Remove(Identity);
            await disposeFiles();
            if (callback != null) callback.CallbackCancelKeep(new DeployTaskLog { TaskIdentity = Identity, OperationState = DeployTaskOperationStateEnum.Cancel });
            AutoCSer.Threading.CatchTask.Add(cancel());
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        protected virtual async Task cancel()
        {
            foreach (IDeployTask task in tasks) await task.Cancel();
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual async Task Start(DateTime startTime, CommandServerKeepCallback<DeployTaskLog> callback)
        {
            if (this.callback != null)
            {
                DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.StartedError);
                return;
            }
            if (tasks.Count == 0)
            {
                DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.EmptyTask);
                return;
            }
            this.callback = callback;
            bool isCallback = true;
            try
            {
                long seconds = (long)(startTime - AutoCSer.Threading.SecondTimer.Now).TotalSeconds;
                if (seconds <= 0)
                {
                    service.Tasks.Remove(Identity);
                    await disposeFiles();
                    AutoCSer.Threading.CatchTask.Add(run());
                }
                else if (seconds <= int.MaxValue)
                {
                    AutoCSer.Threading.SecondTimer.TaskArray.Append(startTimer, (int)seconds, AutoCSer.Threading.SecondTimerTaskThreadModeEnum.Synchronous);
                }
                else
                {
                    DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.StartTimeError);
                    this.callback = null;
                }
                isCallback = false;
            }
            finally
            {
                if (isCallback) DeployTaskLog.CallbackError(callback, DeployTaskOperationStateEnum.Exception);
            }
        }
        /// <summary>
        /// 定时启动任务
        /// </summary>
        protected virtual void startTimer()
        {
            service.Controller.AddLowPriority(start);
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        protected virtual async Task start()
        {
            if (service.Tasks.Remove(Identity))
            {
                await disposeFiles();
                service.Controller.AddLowPriority(run);
            }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        protected virtual async Task run()
        {
            var callback = this.callback.notNull();
            DeployTaskLog log = new DeployTaskLog { TaskIdentity = Identity };
            try
            {
                foreach (IDeployTask task in tasks)
                {
                    log.OperationType = task.OperationType;
                    log.OperationState = DeployTaskOperationStateEnum.StartRun;
                    callback.Callback(log);
                    if(!log.CheckRun(await task.Run()))
                    {
                        callback.Callback(log);
                        return;
                    }
                    ++log.TaskIndex;
                }
                log.OperationState = DeployTaskOperationStateEnum.Completed;
            }
            catch(Exception exception)
            {
                log.OperationState = DeployTaskOperationStateEnum.Exception;
                log.Message = exception.Message;
                callback.Callback(log);
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally
            {
                if (log.OperationState == DeployTaskOperationStateEnum.Completed) callback.Callback(log);
                callback.CancelKeep();
            }
        }
        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="bootPath">服务端文件根目录</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTimes">文件信息集合</param>
        /// <param name="callback">比较结果回调委托</param>
        /// <returns></returns>
#if NetStandard21
        public virtual async Task GetDifferent(int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, CommandServerCallback<bool[]?> callback)
#else
        public virtual async Task GetDifferent(int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, CommandServerCallback<bool[]> callback)
#endif
        {
            var isDifferents = default(bool[]);
            try
            {
                DirectoryInfo taskDirectory = new DirectoryInfo(Path.Combine(service.UploadFilePath, Identity.toString(), index.toString(), path));
                await AutoCSer.Common.TryCreateDirectory(taskDirectory);
                if (fileTimes.Length == 0)
                {
                    isDifferents = EmptyArray<bool>.Array;
                    return;
                }
                var checkFileDictionary = await getFileNameDictionary(new DirectoryInfo(Path.Combine(bootPath, path)));
                var switchFileDictionary = await getFileNameDictionary(new DirectoryInfo(Path.Combine(bootPath, service.SwitchDirectoryName, path)));
                string taskPath = taskDirectory.FullName;
                isDifferents = new bool[fileTimes.Length];
                for (int fileIndex = fileTimes.Length; fileIndex != 0;)
                {
                    var file = fileTimes[--fileIndex].Check(checkFileDictionary, switchFileDictionary);
                    if (file == null) isDifferents[fileIndex] = true;
                    else await AutoCSer.Common.FileCopyTo(file, Path.Combine(taskPath, file.Name), true);
                }
            }
            catch (Exception exception)
            {
                isDifferents = null;
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally { callback.Callback(isDifferents); }
        }
        /// <summary>
        /// 获取文件名称字典集合
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
#if NetStandard21
        private static async Task<Dictionary<HashString, FileInfo>?> getFileNameDictionary(DirectoryInfo directory)
#else
        private static async Task<Dictionary<HashString, FileInfo>> getFileNameDictionary(DirectoryInfo directory)
#endif
        {
            if (!await AutoCSer.Common.DirectoryExists(directory)) return null;
            FileInfo[] files = await AutoCSer.Common.DirectoryGetFiles(directory);
            Dictionary<HashString, FileInfo> fileDictionary = DictionaryCreator.CreateHashString<FileInfo>(files.Length);
            foreach (FileInfo file in files) fileDictionary.Add(file.Name, file);
            return fileDictionary;
        }
        /// <summary>
        /// 初始化上传文件
        /// </summary>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTime">文件信息</param>
        /// <param name="callback">初始化上传文件结果回调委托</param>
        /// <returns></returns>
        public virtual async Task CreateUploadFile(int index, string path, DeployTask.FileTime fileTime, CommandServerCallback<DeployTask.CreateUploadFileResult> callback)
        {
            DeployTask.CreateUploadFileResult result = default(DeployTask.CreateUploadFileResult);
            var fileStream = default(FileStream);
            try
            {
                if (this.callback != null) return;
                FileInfo file = new FileInfo(Path.Combine(service.UploadFilePath, Identity.toString(), index.toString(), path, fileTime.FileName.notNull()));
                if (await AutoCSer.Common.FileExists(file))
                {
                    if (file.LastWriteTimeUtc == fileTime.LastWriteTimeUtc && file.Length <= fileTime.Length)
                    {
                        if (file.Length == fileTime.Length)
                        {
                            result.Set(fileTime.Length, 0);
                            return;
                        }
                        fileStream = await AutoCSer.Common.CreateFileStream(file.FullName, FileMode.Open, FileAccess.Write);
                        await AutoCSer.Common.Seek(fileStream, 0, SeekOrigin.End);
                    }
                    else await AutoCSer.Common.DeleteFile(file);
                }
                if (fileStream == null)
                {
                    fileStream = await AutoCSer.Common.CreateFileStream(file.FullName, FileMode.Create, FileAccess.Write);
                    if (fileTime.Length == 0)
                    {
                        file.LastWriteTimeUtc = fileTime.LastWriteTimeUtc;
                        result.Set(0, 0);
                        return;
                    }
                }
                files.Add(new DeployTask.TaskFileStream(fileStream, file, ref fileTime));
                result.Set(fileStream.Length, files.Count - 1);
                fileStream = null;
            }
            finally
            {
                callback.Callback(result);
                if (fileStream != null) await fileStream.DisposeAsync();
            }
        }
        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual async Task WriteUploadFile(int index, DeployTask.UploadFileBuffer buffer, CommandServerCallback<bool> callback)
        {
            bool isWriteFile = false;
            try
            {
                var file = files[index];
                if (file != null)
                {
                    if (await file.Write(buffer)) files[index] = null;
                    isWriteFile = true;
                }
            }
            finally
            {
                callback.Callback(isWriteFile);
                buffer.Free();
            }
        }
    }
}
