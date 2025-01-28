//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.DeployTask
{
        /// <summary>
        /// 数据库备份服务接口 客户端接口
        /// </summary>
        public partial interface IDatabaseBackupServiceClientController
        {
            /// <summary>
            /// 备份数据库并返回文件名称
            /// </summary>
            /// <param name="database">数据库名称</param>
            /// <returns>重写必须保证回调执行，返回空字符串表示没有找到数据库</returns>
            AutoCSer.Net.ReturnCommand<string> Backup(string database);
            /// <summary>
            /// 下载备份文件
            /// </summary>
            /// <param name="backupFullName">备份文件名称</param>
            /// <param name="startIndex">读取文件起始位置</param>
            /// <returns>下载文件数据回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.DatabaseBackupDownloadBuffer> Download(string backupFullName, long startIndex);
            /// <summary>
            /// 获取可备份数据库名称集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string[]> GetDatabase();
            /// <summary>
            /// 获取可备份数据库表格名称集合
            /// </summary>
            /// <param name="database">数据库名称</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string[]> GetTableName(string database);
        }
}namespace AutoCSer.CommandService.DeployTask
{
        /// <summary>
        /// 文件拉取服务接口 客户端接口
        /// </summary>
        public partial interface IPullFileServiceClientController
        {
            /// <summary>
            /// 获取指定路径下的目录名称集合
            /// </summary>
            /// <param name="path">指定路径</param>
            /// <returns>获取目录名称集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.DirectoryName> GetDirectoryNames(string path);
            /// <summary>
            /// 获取文件信息
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.SynchronousFileInfo> GetFile(string fileName);
            /// <summary>
            /// 获取指定文件数据
            /// </summary>
            /// <param name="fileInfo">文件信息</param>
            /// <returns>获取文件数据回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.PullFileBuffer> GetFileData(AutoCSer.CommandService.DeployTask.SynchronousFileInfo fileInfo);
            /// <summary>
            /// 获取指定路径下的文件信息集合
            /// </summary>
            /// <param name="path">指定路径</param>
            /// <returns>获取文件信息集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.SynchronousFileInfo> GetFiles(string path);
        }
}namespace AutoCSer.CommandService.DeployTask
{
        /// <summary>
        /// 文件上传服务接口 客户端接口
        /// </summary>
        public partial interface IUploadFileServiceClientController
        {
            /// <summary>
            /// 添加上传完成文件
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileInfo">对比文件信息</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.UploadFileInfo> AppendCompletedFile(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, AutoCSer.CommandService.DeployTask.SynchronousFileInfo fileInfo);
            /// <summary>
            /// 添加待删除目录
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
            AutoCSer.Net.ReturnCommand<bool> AppendDeleteDirectory(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 添加待删除文件
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileName">相对路径文件名称</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
            AutoCSer.Net.ReturnCommand<bool> AppendDeleteFile(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string fileName);
            /// <summary>
            /// 拼接路径
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> CombinePath(string left, string right);
            /// <summary>
            /// 拼接路径
            /// </summary>
            /// <param name="pathArray"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> CombinePathArray(string[] pathArray);
            /// <summary>
            /// 上传完成最后移动文件操作
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象，或者上传未结束</returns>
            AutoCSer.Net.ReturnCommand<bool> Completed(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex);
            /// <summary>
            /// 创建目录
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <param name="directoryName">目录名称</param>
            /// <returns>返回 null 表示失败</returns>
            AutoCSer.Net.ReturnCommand<string> CreateDirectory(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string path, string directoryName);
            /// <summary>
            /// 创建上传文件
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="serverFileLength">服务端文件匹配长度</param>
            /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.UploadFileIndex> CreateFile(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, AutoCSer.CommandService.DeployTask.SynchronousFileInfo fileInfo, long serverFileLength);
            /// <summary>
            /// 创建文件上传操作对象
            /// </summary>
            /// <param name="path">上传根目录</param>
            /// <param name="backupPath">备份文件根目录</param>
            /// <param name="extensions">扩展名称集合，扩展名称包括小数点，比如 .txt</param>
            /// <param name="isCaseExtension">扩展名匹配是否区分大小写</param>
            /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.UploaderInfo> CreateUploader(string path, string backupPath, string[] extensions, bool isCaseExtension);
            /// <summary>
            /// 将上传完成操作数据写入文件并返回文件名称
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <returns>返回空字符串表示没有找到文件上传操作对象，或者上传未结束</returns>
            AutoCSer.Net.ReturnCommand<string> GetCompletedFileName(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex);
            /// <summary>
            /// 获取指定路径下的目录名称集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>获取目录名称集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.DirectoryName> GetDirectoryNames(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 获取指定文件信息集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileName">相对路径</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.UploadFileInfo> GetFile(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string fileName);
            /// <summary>
            /// 获取指定路径下的文件信息集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>获取文件信息集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.DeployTask.UploadFileInfo> GetFiles(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 根据上传类型获取文件上传路径
            /// </summary>
            /// <param name="type">上传类型</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> GetPath(string type);
            /// <summary>
            /// 获取切换进程的上传目录与文件信息
            /// </summary>
            /// <param name="path">默认上传目录</param>
            /// <param name="switchPath">默认切换进程上传目录</param>
            /// <param name="fileName">切换进程文件相对路径</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.SynchronousFileInfo> GetSwitchProcessPathFileInfo(string path, string switchPath, string fileName);
            /// <summary>
            /// 移除文件
            /// </summary>
            /// <param name="uploaderIndex">文件上传索引信息</param>
            /// <param name="fileIndex">上传文件索引信息</param>
            AutoCSer.Net.SendOnlyCommand RemoveFile(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex, AutoCSer.CommandService.DeployTask.UploadFileIndex fileIndex);
            /// <summary>
            /// 移除文件上传实例
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            AutoCSer.Net.SendOnlyCommand RemoveUploader(AutoCSer.CommandService.DeployTask.UploadFileIndex uploaderIndex);
            /// <summary>
            /// 上传文件写入数据
            /// </summary>
            /// <param name="buffer"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.DeployTask.UploadFileStateEnum> UploadFileData(AutoCSer.CommandService.DeployTask.UploadFileBuffer buffer);
        }
}namespace AutoCSer.CommandService
{
        /// <summary>
        /// 发布任务节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.IDeployTaskNode))]
        public partial interface IDeployTaskNodeClientNode
        {
            /// <summary>
            /// 添加子任务
            /// </summary>
            /// <param name="identity">任务标识ID</param>
            /// <param name="task">执行程序任务</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> AppendStepTask(long identity, AutoCSer.CommandService.DeployTask.StepTaskData task);
            /// <summary>
            /// 创建任务
            /// </summary>
            /// <returns>任务标识ID</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Create();
            /// <summary>
            /// 获取发布任务状态变更回调日志
            /// </summary>
            /// <param name="identity">任务标识ID</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.DeployTaskLog>> GetLog(long identity);
            /// <summary>
            /// 移除已结束或者未开始任务
            /// </summary>
            /// <param name="identity">任务标识ID</param>
            /// <param name="closeTime">关闭任务时间</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> Remove(long identity, System.DateTime closeTime);
            /// <summary>
            /// 启动任务
            /// </summary>
            /// <param name="identity">任务标识ID</param>
            /// <param name="startTime">运行任务时间</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> Start(long identity, System.DateTime startTime);
        }
}namespace AutoCSer.CommandService
{
        /// <summary>
        /// 发布任务节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDeployTaskNodeMethodEnum))]
        public partial interface IDeployTaskNode { }
        /// <summary>
        /// 发布任务节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IDeployTaskNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加子任务
            /// long identity 任务标识ID
            /// AutoCSer.CommandService.DeployTask.StepTaskData task 执行程序任务
            /// 返回值 AutoCSer.CommandService.DeployTask.OperationStateEnum 
            /// </summary>
            AppendStepTask = 0,
            /// <summary>
            /// [1] 创建任务
            /// 返回值 long 任务标识ID
            /// </summary>
            Create = 1,
            /// <summary>
            /// [2] 获取发布任务状态变更回调日志
            /// long identity 任务标识ID
            /// 返回值 AutoCSer.CommandService.DeployTaskLog 
            /// </summary>
            GetLog = 2,
            /// <summary>
            /// [3] 移除已结束或者未开始任务
            /// long identity 任务标识ID
            /// System.DateTime closeTime 关闭任务时间
            /// 返回值 AutoCSer.CommandService.DeployTask.OperationStateEnum 
            /// </summary>
            Remove = 3,
            /// <summary>
            /// [4] 移除已结束或者未开始任务
            /// long identity 任务标识ID
            /// System.DateTime closeTime 关闭任务时间
            /// 返回值 AutoCSer.CommandService.DeployTask.OperationStateEnum 
            /// </summary>
            RemoveLoadPersistence = 4,
            /// <summary>
            /// [5] 快照设置数据
            /// AutoCSer.CommandService.DeployTask.TaskData value 数据
            /// </summary>
            SnapshotSet = 5,
            /// <summary>
            /// [6] 启动任务
            /// long identity 任务标识ID
            /// System.DateTime startTime 运行任务时间
            /// 返回值 AutoCSer.CommandService.DeployTask.OperationStateEnum 
            /// </summary>
            Start = 6,
            /// <summary>
            /// [7] 启动任务
            /// long identity 任务标识ID
            /// System.DateTime startTime 运行任务时间 Utc
            /// 返回值 AutoCSer.CommandService.DeployTask.OperationStateEnum 
            /// </summary>
            StartLoadPersistence = 7,
            /// <summary>
            /// [8] 关闭任务
            /// long identity 任务标识ID
            /// System.DateTime closeTime 关闭任务时间
            /// </summary>
            Close = 8,
        }
}
#endif