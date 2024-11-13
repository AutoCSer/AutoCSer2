using AutoCSer.CommandService.FileSynchronous;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 拉取文件客户端接口
    /// </summary>
    public interface IPullFileClient
    {
        /// <summary>
        /// 获取指定路径下的文件信息集合
        /// </summary>
        /// <param name="path">指定路径</param>
        /// <returns>文件信息集合</returns>
        EnumeratorCommand<SynchronousFileInfo> GetFiles(string path);
        /// <summary>
        /// 获取指定路径下的目录名称集合
        /// </summary>
        /// <param name="path">指定路径</param>
        /// <returns>目录名称集合</returns>
        EnumeratorCommand<DirectoryName> GetDirectoryNames(string path);
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        ReturnCommand<SynchronousFileInfo> GetFile(string fileName);
        /// <summary>
        /// 获取指定文件数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>文件数据</returns>
#if NetStandard21
        EnumeratorCommand<PullFileBuffer?> GetFileData(PullFileBuffer returnValue, SynchronousFileInfo fileInfo);
#else
        EnumeratorCommand<PullFileBuffer> GetFileData(PullFileBuffer returnValue, SynchronousFileInfo fileInfo);
#endif
    }
}
