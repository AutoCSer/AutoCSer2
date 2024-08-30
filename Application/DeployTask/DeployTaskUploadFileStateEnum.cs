using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 文件上传状态
    /// </summary>
    public enum DeployTaskUploadFileStateEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 没有找到上传文件目录
        /// </summary>
        NotFoundDirectory,
        /// <summary>
        /// 客户端比较文件最后修改时间调用失败
        /// </summary>
        GetDifferentError,
        /// <summary>
        /// 文件上传之前大小被修改
        /// </summary>
        FileLengthChanged,
        /// <summary>
        /// 文件上传之前最后修改时间被修改
        /// </summary>
        FileTimeChanged,
        /// <summary>
        /// 初始化上传文件调用失败
        /// </summary>
        CreateUploadFileError,
        /// <summary>
        /// 初始化上传文件未知错误
        /// </summary>
        CreateUploadFileUnknownError,
        /// <summary>
        /// 上传文件数据调用失败
        /// </summary>
        WriteUploadFileError,
        /// <summary>
        /// 上传文件数据未知错误
        /// </summary>
        WriteUploadFileUnknownError,
    }
}
