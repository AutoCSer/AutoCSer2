using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.ReadWriteQueue
{
    /// <summary>
    /// 服务端 读写队列 API 一次性响应 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(AutoCSer.Net.CommandServerCallReadQueue queue, int left, int right);
    }
    /// <summary>
    /// 服务端 读写队列 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class SynchronousController: ISynchronousController
    {
        /// <summary>
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ISynchronousController.Add(AutoCSer.Net.CommandServerCallReadQueue queue, int left, int right)
        {
            return left + right;
        }
    }
}
