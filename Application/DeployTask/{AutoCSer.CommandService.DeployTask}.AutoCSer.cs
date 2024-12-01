//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 发布任务服务端接口 客户端接口
        /// </summary>
        public partial interface IDeployTaskServiceClientController
        {
            /// <summary>
            /// 添加复制文件步骤
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="index">当前任务临时文件目录序号</param>
            /// <param name="destinationPath">文件目标路径</param>
            /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
            /// <param name="isBackup">是否备份历史文件</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTaskAppendResult> AppendCopyUploadFile(long taskIdentity, int index, string destinationPath, string checkSwitchFileName, bool isBackup);
            /// <summary>
            /// 添加执行程序步骤
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="startFileName">运行程序文件名称</param>
            /// <param name="arguments">运行程序参数</param>
            /// <param name="isWait">执行任务流程是否等待程序运行结束</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTaskAppendResult> AppendStartProcess(long taskIdentity, string startFileName, string arguments, bool isWait);
            /// <summary>
            /// 取消任务
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <returns>返回 false 表示没有找到任务或者任务已经启动不允许取消</returns>
            AutoCSer.Net.ReturnCommand<bool> Cancel(long taskIdentity);
            /// <summary>
            /// 创建任务
            /// </summary>
            /// <returns>任务ID</returns>
            AutoCSer.Net.ReturnCommand<long> Create();
            /// <summary>
            /// 初始化上传文件
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="index">当前任务临时文件目录序号</param>
            /// <param name="path">服务端文件相对目录</param>
            /// <param name="fileTime">文件信息</param>
            /// <returns>初始化上传文件结果回调委托</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.CreateUploadFileResult> CreateUploadFile(long taskIdentity, int index, string path, AutoCSer.CommandService.DeployTask.FileTime fileTime);
            /// <summary>
            /// 比较文件最后修改时间
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="index">当前任务临时文件目录序号</param>
            /// <param name="bootPath">服务端文件根目录</param>
            /// <param name="path">服务端文件相对目录</param>
            /// <param name="fileTimes">文件信息集合</param>
            /// <returns>比较结果回调委托，返回 null 表示没有找到任务或者异常</returns>
            AutoCSer.Net.ReturnCommand<bool[]> GetDifferent(long taskIdentity, int index, string bootPath, string path, AutoCSer.CommandService.DeployTask.FileTime[] fileTimes);
            /// <summary>
            /// 启动任务
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="startTime">运行任务时间</param>
            /// <returns>任务状态变更回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTaskLog> Start(long taskIdentity, System.DateTime startTime);
            /// <summary>
            /// 上传文件数据
            /// </summary>
            /// <param name="taskIdentity">任务ID</param>
            /// <param name="index">写入文件流索引</param>
            /// <param name="buffer">上传文件缓冲区</param>
            /// <returns>上传文件数据结果回调委托，上传文件流标识匹配失败则返回 false</returns>
            AutoCSer.Net.ReturnCommand<bool> WriteUploadFile(long taskIdentity, int index, AutoCSer.CommandService.DeployTask.UploadFileBuffer buffer);
        }
}
#endif