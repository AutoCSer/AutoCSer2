using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传服务客户端接口
    /// </summary>
    public interface IUploadFileClient
    {
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ReturnCommand<string> CombinePath(string left, string right);
        /// <summary>
        /// 创建文件上传操作对象
        /// </summary>
        /// <param name="path">上传根目录</param>
        /// <param name="backupPath">备份文件根目录</param>
        /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
        ReturnCommand<UploaderInfo> CreateUploader(string path, string backupPath);
        /// <summary>
        /// 移除文件上传实例
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns></returns>
        SendOnlyCommand RemoveUploader(UploadFileIndex uploaderIndex);
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>文件信息集合</returns>
        EnumeratorCommand<UploadFileInfo> GetFiles(UploadFileIndex uploaderIndex, string path);
        /// <summary>
        /// 获取指定文件信息集合
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        ReturnCommand<UploadFileInfo> GetFile(UploadFileIndex uploaderIndex, string fileName);
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>目录名称集合</returns>
        EnumeratorCommand<DirectoryName> GetDirectoryNames(UploadFileIndex uploaderIndex, string path);
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="directoryName">目录名称</param>
        /// <returns>返回 null 表示失败</returns>
        ReturnCommand<string> CreateDirectory(UploadFileIndex uploaderIndex, string path, string directoryName);
        /// <summary>
        /// 创建上传文件
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="serverFileLength">服务端文件匹配长度</param>
        /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
        ReturnCommand<UploadFileIndex> CreateFile(UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo, long serverFileLength);
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="uploaderIndex">文件上传索引信息</param>
        /// <param name="fileIndex">上传文件索引信息</param>
        /// <returns></returns>
        SendOnlyCommand RemoveFile(UploadFileIndex uploaderIndex, UploadFileIndex fileIndex);
        /// <summary>
        /// 上传文件写入数据
        /// </summary>
        /// <param name="buffer">文件上传数据</param>
        /// <returns></returns>
        ReturnCommand<UploadFileStateEnum> UploadFileData(UploadFileBuffer buffer);
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">对比文件信息</param>
        /// <returns></returns>
        ReturnCommand<UploadFileInfo> AppendCompletedFile(UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo);
        /// <summary>
        /// 添加待删除文件
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径文件名称</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        ReturnCommand<bool> AppendDeleteFile(UploadFileIndex uploaderIndex, string fileName);
        /// <summary>
        /// 添加待删除目录
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        ReturnCommand<bool> AppendDeleteDirectory(UploadFileIndex uploaderIndex, string path);
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        ReturnCommand<bool> Completed(UploadFileIndex uploaderIndex);
    }
}
