using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// 服务端 IO 线程同步 API 一次性响应 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(int left, int right);
    }
    /// <summary>
    /// 服务端 IO 线程同步 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class SynchronousController: ISynchronousController
    {
        /// <summary>
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
