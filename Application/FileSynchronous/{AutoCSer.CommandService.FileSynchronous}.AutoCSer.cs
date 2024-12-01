//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.FileSynchronous
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
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.FileSynchronous.DirectoryName> GetDirectoryNames(string path);
            /// <summary>
            /// 获取文件信息
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.SynchronousFileInfo> GetFile(string fileName);
            /// <summary>
            /// 获取指定文件数据
            /// </summary>
            /// <param name="fileInfo">文件信息</param>
            /// <returns>获取文件数据回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.FileSynchronous.PullFileBuffer> GetFileData(AutoCSer.CommandService.FileSynchronous.SynchronousFileInfo fileInfo);
            /// <summary>
            /// 获取指定路径下的文件信息集合
            /// </summary>
            /// <param name="path">指定路径</param>
            /// <returns>获取文件信息集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.FileSynchronous.SynchronousFileInfo> GetFiles(string path);
        }
}namespace AutoCSer.CommandService.FileSynchronous
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
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.UploadFileInfo> AppendCompletedFile(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, AutoCSer.CommandService.FileSynchronous.SynchronousFileInfo fileInfo);
            /// <summary>
            /// 添加待删除目录
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
            AutoCSer.Net.ReturnCommand<bool> AppendDeleteDirectory(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 添加待删除文件
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileName">相对路径文件名称</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
            AutoCSer.Net.ReturnCommand<bool> AppendDeleteFile(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string fileName);
            /// <summary>
            /// 拼接路径
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> CombinePath(string left, string right);
            /// <summary>
            /// 上传完成最后移动文件操作
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
            AutoCSer.Net.ReturnCommand<bool> Completed(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex);
            /// <summary>
            /// 创建目录
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <param name="directoryName">目录名称</param>
            /// <returns>返回 null 表示失败</returns>
            AutoCSer.Net.ReturnCommand<string> CreateDirectory(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string path, string directoryName);
            /// <summary>
            /// 创建上传文件
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="serverFileLength">服务端文件匹配长度</param>
            /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.UploadFileIndex> CreateFile(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, AutoCSer.CommandService.FileSynchronous.SynchronousFileInfo fileInfo, long serverFileLength);
            /// <summary>
            /// 创建文件上传操作对象
            /// </summary>
            /// <param name="path">上传根目录</param>
            /// <param name="backupPath">备份文件根目录</param>
            /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.UploaderInfo> CreateUploader(string path, string backupPath);
            /// <summary>
            /// 获取指定路径下的目录名称集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>获取目录名称集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.FileSynchronous.DirectoryName> GetDirectoryNames(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 获取指定文件信息集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="fileName">相对路径</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.UploadFileInfo> GetFile(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string fileName);
            /// <summary>
            /// 获取指定路径下的文件信息集合
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            /// <param name="path">相对路径</param>
            /// <returns>获取文件信息集合回调委托</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.FileSynchronous.UploadFileInfo> GetFiles(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, string path);
            /// <summary>
            /// 移除文件
            /// </summary>
            /// <param name="uploaderIndex">文件上传索引信息</param>
            /// <param name="fileIndex">上传文件索引信息</param>
            AutoCSer.Net.SendOnlyCommand RemoveFile(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex, AutoCSer.CommandService.FileSynchronous.UploadFileIndex fileIndex);
            /// <summary>
            /// 移除文件上传实例
            /// </summary>
            /// <param name="uploaderIndex">上传文件索引信息</param>
            AutoCSer.Net.SendOnlyCommand RemoveUploader(AutoCSer.CommandService.FileSynchronous.UploadFileIndex uploaderIndex);
            /// <summary>
            /// 上传文件写入数据
            /// </summary>
            /// <param name="buffer"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.FileSynchronous.UploadFileStateEnum> UploadFileData(AutoCSer.CommandService.FileSynchronous.UploadFileBuffer buffer);
        }
}
#endif