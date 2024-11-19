using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志收集反向服务客户端
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public interface ILogCollectionReverseClient<T>
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志数据</param>
        /// <returns></returns>
        ReturnCommand Append(T log);
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志数据</param>
        /// <returns></returns>
#if NetStandard21
        SendOnlyCommand? AppendSendOnly(T log);
#else
        SendOnlyCommand AppendSendOnly(T log);
#endif
    }
}
