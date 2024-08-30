using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 客户端同步文件
    /// </summary>
    public abstract class SynchronousFile
    {
        /// <summary>
        /// 客户端文件信息
        /// </summary>
        internal readonly FileInfo ClientFile;
        /// <summary>
        /// 服务端文件信息
        /// </summary>
        internal SynchronousFileInfo FileInfo;
        /// <summary>
        /// 正在处理的文件索引位置
        /// </summary>
        internal int FileIndex;
        /// <summary>
        /// 客户端同步文件
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="fileInfo">文件信息</param>
        internal SynchronousFile(FileInfo clientFile, ref SynchronousFileInfo fileInfo)
        {
            FileIndex = -1;
            ClientFile = clientFile;
            FileInfo = fileInfo;
        }
        /// <summary>
        /// 客户端读文件操作
        /// </summary>
        /// <param name="clientFile">客户端文件信息</param>
        /// <param name="serverFileName">服务端文件名称</param>
        internal SynchronousFile(FileInfo clientFile, string serverFileName)
        {
            FileIndex = -1;
            ClientFile = clientFile;
            FileInfo.SetUpload(clientFile, serverFileName);
        }
        /// <summary>
        /// 文件同步
        /// </summary>
        /// <returns></returns>
        internal abstract Task Synchronous();
    }
}
