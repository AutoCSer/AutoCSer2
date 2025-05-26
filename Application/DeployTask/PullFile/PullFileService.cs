using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 拉取文件服务
    /// </summary>
    public class PullFileService : IPullFileService
    {
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">指定路径</param>
        /// <param name="callback">获取文件信息集合回调委托</param>
        /// <returns></returns>
        public virtual async Task GetFiles(CommandServerSocket socket, string path, CommandServerKeepCallbackCount<SynchronousFileInfo> callback)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory))
                {
                    if (!await callback.CallbackAsync(new SynchronousFileInfo(file))) return;
                }
            }
        }
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">指定路径</param>
        /// <param name="callback">获取目录名称集合回调委托</param>
        /// <returns></returns>
        public virtual async Task GetDirectoryNames(CommandServerSocket socket, string path, CommandServerKeepCallbackCount<DirectoryName> callback)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (await AutoCSer.Common.DirectoryExists(directory))
            {
                foreach (DirectoryInfo directoryInfo in await AutoCSer.Common.GetDirectories(directory))
                {
                    if (!await callback.CallbackAsync(new DirectoryName(directoryInfo))) return;
                }
            }
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public virtual async Task<SynchronousFileInfo> GetFile(CommandServerSocket socket, string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            if (await AutoCSer.Common.FileExists(file)) return new SynchronousFileInfo(file);
            return default(SynchronousFileInfo);
        }
        /// <summary>
        /// 获取指定文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="callback">获取文件数据回调委托，返回 null 表示服务端未知异常</param>
        /// <returns></returns>
#if NetStandard21
        public virtual async Task GetFileData(CommandServerSocket socket, SynchronousFileInfo fileInfo, CommandServerKeepCallbackCount<PullFileBuffer?> callback)
#else
        public virtual async Task GetFileData(CommandServerSocket socket, SynchronousFileInfo fileInfo, CommandServerKeepCallbackCount<PullFileBuffer> callback)
#endif
        {
            var readFileBuffer = default(PullFileBuffer);
            bool iscompleted = false;
            try
            {
#pragma warning disable CS8620
                readFileBuffer = new PullFileBuffer(callback);
#pragma warning restore CS8620
                iscompleted = await readFileBuffer.Read(fileInfo, socket.Server.SendBufferPool.Size);
            }
            finally 
            {
                if (!iscompleted) await callback.CallbackAsync(readFileBuffer);
            }
        }
    }
}
