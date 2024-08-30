using AutoCSer.CommandService.FileSynchronous;
using System;
using System.IO;

namespace AutoCSer.TestCase.FileSynchronousClient
{
    /// <summary>
    /// 文件上传客户端
    /// </summary>
    internal sealed class UploadFileClient : AutoCSer.CommandService.FileSynchronous.UploadFileClient
    {
        /// <summary>
        /// 文件上传客户端
        /// </summary>
        /// <param name="client">文件上传客户端接口</param>
        /// <param name="clientPath">客户端路径</param>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="isDelete">当客户端不存在时，是否删除服务端文件与目录</param>
        /// <param name="concurrency">同步并发数</param>
        public UploadFileClient(IUploadFileClientSocketEvent client, string clientPath, string serverPath, bool isDelete = true, int concurrency = 16) : base(client, clientPath, serverPath, isDelete, concurrency)
        {
        }
        /// <summary>
        /// 文件同步完成
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        protected override void onCompleted(FileInfo clientFile, string serverFileName)
        {
            Console.WriteLine($"文件上传完成 {serverFileName}");
        }
        /// <summary>
        /// 目录同步错误
        /// </summary>
        /// <param name="clientDirectory">客户端目录信息</param>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="state">错误状态</param>
        protected override void onPathError(DirectoryInfo clientDirectory, string serverPath, UploadFileStateEnum state)
        {
            onPathError(state);
            AutoCSer.ConsoleWriteQueue.Breakpoint($"目录上传错误 {state} {clientDirectory.FullName}");
        }
        /// <summary>
        /// 文件同步错误
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        /// <param name="state">错误状态</param>
        protected override void onFileError(FileInfo clientFile, string serverFileName, UploadFileStateEnum state)
        {
            onFileError(state);
            AutoCSer.ConsoleWriteQueue.Breakpoint($"文件上传错误 {state} {clientFile.FullName}");
        }
    }
}
