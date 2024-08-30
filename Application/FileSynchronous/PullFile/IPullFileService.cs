using AutoCSer.CommandService.FileSynchronous;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件拉取服务接口
    /// </summary>
    public interface IPullFileService
    {
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">指定路径</param>
        /// <param name="callback">获取文件信息集合回调委托</param>
        /// <returns></returns>
        [CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        Task GetFiles(CommandServerSocket socket, string path, CommandServerKeepCallbackCount<SynchronousFileInfo> callback);
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">指定路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        [CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        Task GetDirectoryNames(CommandServerSocket socket, string path, CommandServerKeepCallbackCount<DirectoryName> callback);
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        Task<SynchronousFileInfo> GetFile(CommandServerSocket socket, string fileName);
        /// <summary>
        /// 获取指定文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="callback">获取文件数据回调委托</param>
        /// <returns></returns>
        [CommandServerMethod(KeepCallbackOutputCount = 1 << 8)]
        Task GetFileData(CommandServerSocket socket, SynchronousFileInfo fileInfo, CommandServerKeepCallbackCount<PullFileBuffer> callback);
    }
}
