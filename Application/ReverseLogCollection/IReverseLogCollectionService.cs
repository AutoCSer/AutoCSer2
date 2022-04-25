using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 反向日志收集服务（用于利用家庭宽带收集日志，需要每个写日志的进程启动一个服务，家庭带宽电脑上启动客户端，支持多客户端用于异地冗余）
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public interface IReverseLogCollectionService<T>
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">获取日志回调委托</param>
        [CommandServerMethod(IsOutputPool = true, AutoCancelKeep = false)]
        void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<T> callback);
    }
}
