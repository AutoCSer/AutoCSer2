using AutoCSer.CommandService.FileSynchronous;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.FileSynchronousClient
{
    /// <summary>
    /// 客户端拉取文件
    /// </summary>
    internal sealed class PullFileClient : AutoCSer.CommandService.FileSynchronous.PullFileClient
    {
        /// <summary>
        /// 拉取文件客户端
        /// </summary>
        /// <param name="client">拉取文件客户端接口</param>
        /// <param name="isDelete">当服务端不存在时，是否删除客户端文件与目录</param>
        /// <param name="concurrency">同步并发数</param>
        public PullFileClient(IPullFileClientSocketEvent client, bool isDelete = true, int concurrency = 16) : base(client, isDelete, concurrency) { }
        /// <summary>
        /// 文件同步完成
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        protected override void onCompleted(FileInfo clientFile, string serverFileName)
        {
            Console.WriteLine($"文件拉取完成 {clientFile.FullName}");
        }
        /// <summary>
        /// 目录同步错误
        /// </summary>
        /// <param name="clientDirectory">客户端目录信息</param>
        /// <param name="serverPath">服务端路径</param>
        /// <param name="state">错误状态</param>
        protected override void onPathError(DirectoryInfo clientDirectory, string serverPath, PullFileStateEnum state)
        {
            onPathError(state);
            AutoCSer.ConsoleWriteQueue.Breakpoint($"目录拉取错误 {state} {serverPath}");
        }
        /// <summary>
        /// 文件同步错误
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        /// <param name="state">错误状态</param>
        protected override void onFileError(FileInfo clientFile, string serverFileName, PullFileStateEnum state)
        {
            onFileError(state);
            AutoCSer.ConsoleWriteQueue.Breakpoint($"文件拉取错误 {state} {serverFileName}");
        }
    }
}
