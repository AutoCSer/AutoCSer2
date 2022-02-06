using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Server.Synchronous
{
    /// <summary>
    /// 服务端 IO线程同步调用 同步返回数据 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 同步返回数据，支持 ref / out 参数
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter">普通参数</param>
        /// <param name="refParameter">ref 参数</param>
        /// <param name="outParameter">out 参数</param>
        /// <returns></returns>
        int SynchronousReturn(CommandServerSocket socket, int parameter, ref int refParameter, out long outParameter);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        void SynchronousCall(int parameter1, int parameter2);
    }
    /// <summary>
    /// 服务端 IO线程同步调用 同步返回数据 示例接口实例
    /// </summary>
    internal sealed class SynchronousController : ISynchronousController
    {
        /// <summary>
        /// 同步返回数据，支持 ref / out 参数
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter">普通参数</param>
        /// <param name="refParameter">ref 参数</param>
        /// <param name="outParameter">out 参数</param>
        /// <returns></returns>
        int ISynchronousController.SynchronousReturn(CommandServerSocket socket, int parameter, ref int refParameter, out long outParameter)
        {
            refParameter = parameter + 1;
            outParameter = (long)parameter * refParameter;
            return parameter + 2;
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        void ISynchronousController.SynchronousCall(int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
        }
    }
}
