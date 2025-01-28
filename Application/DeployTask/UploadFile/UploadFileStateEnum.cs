using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传状态
    /// </summary>
    public enum UploadFileStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 文件信息不匹配
        /// </summary>
        FileInfoNotMatch,
        /// <summary>
        /// 没有找到文件上传信息
        /// </summary>
        NotFoundUploader,
        /// <summary>
        /// 客户端未找到上传目录
        /// </summary>
        NotFoundDirectory,
        /// <summary>
        /// 客户端未找到上传文件
        /// </summary>
        NotFoundFile,
        /// <summary>
        /// 上传完成，存在部分错误
        /// </summary>
        Completed,
        /// <summary>
        /// 服务端任务未完成
        /// </summary>
        NotCompleted,
        /// <summary>
        /// 服务接口调用失败
        /// </summary>
        CallFail,
        /// <summary>
        /// 客户端异常
        /// </summary>
        ClientException,
        /// <summary>
        /// 不支持的备份路径
        /// </summary>
        NotSupportBackupPath,
        /// <summary>
        /// 指定的上传已经创建实例
        /// </summary>
        PathUploading,
        /// <summary>
        /// 服务端反序列化输入参数没有找到套接字会话对象
        /// </summary>
        NotFoundSessionObject,
        /// <summary>
        /// 没有找到上传文件信息
        /// </summary>
        NotFoundUploadFile,
        /// <summary>
        /// 服务端执行异常
        /// </summary>
        ServerException,
    }
}
