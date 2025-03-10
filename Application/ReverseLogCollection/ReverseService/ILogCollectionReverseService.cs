using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志收集反向服务（用于利用家庭宽带收集日志，需要每个写日志的进程启动一个监听客户端，家庭带宽电脑上启动服务端）
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(LogCollectionReverseServiceMethodEnum), MethodIndexEnumTypeCodeGeneratorPath = "", IsAutoMethodIndex = false)]
    public interface ILogCollectionReverseService<T>
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log">日志数据</param>
        void Append(CommandServerSocket socket, CommandServerCallQueue queue, T log);
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log">日志数据</param>
        /// <returns></returns>
        CommandServerSendOnly AppendSendOnly(CommandServerSocket socket, CommandServerCallQueue queue, T log);
    }
}
