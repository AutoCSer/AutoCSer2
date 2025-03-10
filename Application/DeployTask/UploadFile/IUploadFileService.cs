using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(UploadFileServiceMethodEnum), MethodIndexEnumTypeCodeGeneratorPath = "", IsAutoMethodIndex = false)]
    public interface IUploadFileService
    {
        /// <summary>
        /// 根据上传类型获取文件上传路径
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="type">上传类型</param>
        /// <returns></returns>
        string GetPath(CommandServerSocket socket, string type);
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        string CombinePath(string left, string right);
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="pathArray"></param>
        /// <returns></returns>
        string CombinePathArray(string[] pathArray);
        /// <summary>
        /// 创建文件上传操作对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">上传根目录</param>
        /// <param name="backupPath">备份文件根目录</param>
        /// <param name="extensions">扩展名称集合，扩展名称包括小数点，比如 .txt</param>
        /// <param name="isCaseExtension">扩展名匹配是否区分大小写</param>
        /// <returns>上传索引与路径信息，Index 为负数表示调用错误状态 UploadFileStateEnum</returns>
        Task<UploaderInfo> CreateUploader(CommandServerSocket socket, string path, string backupPath, string[] extensions, bool isCaseExtension);
        /// <summary>
        /// 移除文件上传实例
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns></returns>
        Task<CommandServerSendOnly> RemoveUploader(CommandServerSocket socket, UploadFileIndex uploaderIndex);
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取文件信息集合回调委托</param>
        /// <returns></returns>
        Task GetFiles(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<UploadFileInfo> callback);
        /// <summary>
        /// 获取指定文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        Task<UploadFileInfo> GetFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName);
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        Task GetDirectoryNames(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, CommandServerKeepCallbackCount<DirectoryName> callback);
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <param name="directoryName">目录名称</param>
        /// <returns>返回 null 表示失败</returns>
#if NetStandard21
        Task<string?> CreateDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, string directoryName);
#else
        Task<string> CreateDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path, string directoryName);
#endif
        /// <summary>
        /// 创建上传文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="serverFileLength">服务端文件匹配长度</param>
        /// <returns>上传文件索引信息，Index 为负数表示错误状态 UploadFileStateEnum</returns>
        Task<UploadFileIndex> CreateFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo, long serverFileLength);
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">文件上传索引信息</param>
        /// <param name="fileIndex">上传文件索引信息</param>
        /// <returns></returns>
        Task<CommandServerSendOnly> RemoveFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, UploadFileIndex fileIndex);
        /// <summary>
        /// 上传文件写入数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        UploadFileStateEnum UploadFileData(CommandServerSocket socket, UploadFileBuffer buffer);
        /// <summary>
        /// 添加上传完成文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileInfo">对比文件信息</param>
        /// <returns></returns>
        Task<UploadFileInfo> AppendCompletedFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, SynchronousFileInfo fileInfo);
        /// <summary>
        /// 添加待删除文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="fileName">相对路径文件名称</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        Task<bool> AppendDeleteFile(CommandServerSocket socket, UploadFileIndex uploaderIndex, string fileName);
        /// <summary>
        /// 添加待删除目录
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <param name="path">相对路径</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象</returns>
        Task<bool> AppendDeleteDirectory(CommandServerSocket socket, UploadFileIndex uploaderIndex, string path);
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回 false 表示没有找到文件上传操作对象，或者上传未结束</returns>
        Task<bool> Completed(CommandServerSocket socket, UploadFileIndex uploaderIndex);
        /// <summary>
        /// 将上传完成操作数据写入文件并返回文件名称
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="uploaderIndex">上传文件索引信息</param>
        /// <returns>返回空字符串表示没有找到文件上传操作对象，或者上传未结束</returns>
        Task<string> GetCompletedFileName(CommandServerSocket socket, UploadFileIndex uploaderIndex);
        /// <summary>
        /// 获取切换进程的上传目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">默认上传目录</param>
        /// <param name="switchPath">默认切换进程上传目录</param>
        /// <param name="fileName">切换进程文件相对路径</param>
        /// <returns></returns>
        Task<SynchronousFileInfo> GetSwitchProcessPathFileInfo(CommandServerSocket socket, string path, string switchPath, string fileName);
    }
}
