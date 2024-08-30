using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务客户端接口
    /// </summary>
    public interface IDeployTaskClient
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns>任务ID</returns>
        ReturnCommand<long> Create();
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="callback">任务ID</param>
        /// <returns></returns>
        CallbackCommand Create(Action<CommandClientReturnValue<long>> callback);

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <returns>返回 false 表示没有找到任务或者任务已经启动不允许取消</returns>
        ReturnCommand<bool> Cancel(long taskIdentity);
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="callback">返回 false 表示没有找到任务或者任务已经启动不允许取消</param>
        /// <returns></returns>
        CallbackCommand Cancel(long taskIdentity, Action<CommandClientReturnValue<long>> callback);

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <param name="callback">任务状态变更回调委托</param>
        /// <returns></returns>
        KeepCallbackCommand Start(long taskIdentity, DateTime startTime, Action<CommandClientReturnValue<DeployTaskLog>, KeepCallbackCommand> callback);
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <returns>任务状态变更</returns>
        EnumeratorCommand<DeployTaskLog> Start(long taskIdentity, DateTime startTime);

        /// <summary>
        /// 添加执行程序步骤
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startFileName">运行程序文件名称</param>
        /// <param name="arguments">运行程序参数</param>
        /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
        /// <returns></returns>
        ReturnCommand<DeployTaskAppendResult> AppendStartProcess(long taskIdentity, string startFileName, string arguments, bool isWait);
        /// <summary>
        /// 添加执行程序步骤
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startFileName">运行程序文件名称</param>
        /// <param name="arguments">运行程序参数</param>
        /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
        /// <param name="callback">添加任务步骤结果</param>
        /// <returns></returns>
        CallbackCommand AppendStartProcess(long taskIdentity, string startFileName, string arguments, bool isWait, Action<CommandClientReturnValue<DeployTaskAppendResult>> callback);

        /// <summary>
        /// 添加复制文件步骤
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="destinationPath">文件目标路径</param>
        /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
        /// <param name="isBackup">是否备份历史文件</param>
        /// <returns></returns>
        ReturnCommand<DeployTaskAppendResult> AppendCopyUploadFile(long taskIdentity, int index, string destinationPath, string checkSwitchFileName, bool isBackup);
        /// <summary>
        /// 添加复制文件步骤
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="destinationPath">文件目标路径</param>
        /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
        /// <param name="isBackup">是否备份历史文件</param>
        /// <param name="callback">添加任务步骤结果</param>
        /// <returns></returns>
        CallbackCommand AppendCopyUploadFile(long taskIdentity, int index, string destinationPath, string checkSwitchFileName, bool isBackup, Action<CommandClientReturnValue<DeployTaskAppendResult>> callback);

        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="bootPath">服务端文件根目录</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTimes">文件信息集合</param>
        /// <returns>比较结果</returns>
        ReturnCommand<bool[]> GetDifferent(long taskIdentity, int index, string bootPath, string path, DeployTask.FileTime[] fileTimes);
        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="bootPath">服务端文件根目录</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTimes">文件信息集合</param>
        /// <param name="callback">比较结果回调委托</param>
        /// <returns></returns>
        CallbackCommand GetDifferent(long taskIdentity, int index, string bootPath, string path, DeployTask.FileTime[] fileTimes, Action<CommandClientReturnValue<bool[]>> callback);

        /// <summary>
        /// 初始化上传文件
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTime">文件信息</param>
        /// <returns>初始化上传文件结果</returns>
        ReturnCommand<DeployTask.CreateUploadFileResult> CreateUploadFile(long taskIdentity, int index, string path, DeployTask.FileTime fileTime);
        /// <summary>
        /// 初始化上传文件
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="path">服务端文件相对目录</param>
        /// <param name="fileTime">文件信息</param>
        /// <param name="callback">初始化上传文件结果回调委托</param>
        /// <returns></returns>
        CallbackCommand CreateUploadFile(long taskIdentity, int index, string path, DeployTask.FileTime fileTime, Action<CommandClientReturnValue<DeployTask.CreateUploadFileResult>> callback);

        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">写入文件流索引</param>
        /// <param name="buffer">上传文件缓冲区</param>
        /// <returns>上传文件数据结果，上传文件流标识匹配失败则返回 false</returns>
        ReturnCommand<bool> WriteUploadFile(long taskIdentity, int index, DeployTask.UploadFileBuffer buffer);
        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">写入文件流索引</param>
        /// <param name="buffer">上传文件缓冲区</param>
        /// <param name="callback">上传文件数据结果回调委托，上传文件流标识匹配失败则返回 false</param>
        /// <returns></returns>
        CallbackCommand WriteUploadFile(long taskIdentity, int index, DeployTask.UploadFileBuffer buffer, Action<CommandClientReturnValue<bool>> callback);
    }
}
