using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// Server IO thread synchronization one-time response API sample interface
    /// 服务端 IO 线程同步 一次性响应 API 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// Synchronous API example, supporting ref/out parameters
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
    /// <summary>
    /// Server IO thread synchronization one-time response API sample controller
    /// 服务端 IO 线程同步 一次性响应 API 示例控制器
    /// </summary>
    internal sealed class SynchronousController : ISynchronousController
    {
        /// <summary>
        /// Synchronous API example, supporting ref/out parameters
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ISynchronousController.Add(int left, int right)
        {
            return left + right;
        }
    }
}
