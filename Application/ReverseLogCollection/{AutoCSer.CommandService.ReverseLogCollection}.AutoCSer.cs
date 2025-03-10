//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 日志收集反向服务（用于利用家庭宽带收集日志，需要每个写日志的进程启动一个监听客户端，家庭带宽电脑上启动服务端） 客户端接口
        /// </summary>
        public partial interface ILogCollectionReverseServiceClientController<T>
        {
            /// <summary>
            /// 添加日志
            /// </summary>
            /// <param name="log">日志数据</param>
            AutoCSer.Net.ReturnCommand Append(T log);
            /// <summary>
            /// 添加日志
            /// </summary>
            /// <param name="log">日志数据</param>
            AutoCSer.Net.SendOnlyCommand AppendSendOnly(T log);
        }
}namespace AutoCSer.CommandService
{
    public enum LogCollectionReverseServiceMethodEnum
    {
            /// <summary>
            /// [0] 添加日志
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// T log 日志数据
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加日志
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// T log 日志数据
            /// </summary>
            AppendSendOnly = 1,
    }
}
#endif