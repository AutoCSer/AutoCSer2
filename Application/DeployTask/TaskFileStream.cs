using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 发布任务写入文件流
    /// </summary>
    internal sealed class TaskFileStream
    {
        /// <summary>
        /// 写入文件流
        /// </summary>
        private readonly FileStream stream;
        /// <summary>
        /// 文件信息
        /// </summary>
        private readonly FileInfo file;
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        private readonly FileTime fileTime;
        /// <summary>
        /// 发布任务写入文件流
        /// </summary>
        /// <param name="stream">写入文件流</param>
        /// <param name="file">文件信息</param>
        /// <param name="fileTime">文件最后修改时间</param>
        internal TaskFileStream(FileStream stream, FileInfo file, ref FileTime fileTime)
        {
            this.stream = stream;
            this.file = file;
            this.fileTime = fileTime;
        }
        /// <summary>
        /// 关闭文件流
        /// </summary>
        /// <returns></returns>
        internal async Task Close()
        {
            if (stream != null) await close();
        }
#if NetStandard21
        /// <summary>
        /// 关闭文件流
        /// </summary>
        /// <returns></returns>
        private async Task close()
        {
            await stream.DisposeAsync();
            file.LastWriteTimeUtc = fileTime.LastWriteTimeUtc;
        }
#else
        /// <summary>
        /// 关闭文件流
        /// </summary>
        /// <returns></returns>
        private Task close()
        {
            stream.Dispose();
            file.LastWriteTimeUtc = fileTime.LastWriteTimeUtc;
            return AutoCSer.Common.CompletedTask;
        }
#endif
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal async Task<bool> Write(UploadFileBuffer buffer)
        {
            await stream.WriteAsync(buffer.Buffer, 0, buffer.Length);
            if (stream.Length == fileTime.Length)
            {
                await close();
                return true;
            }
            return false;
        }
    }
}
