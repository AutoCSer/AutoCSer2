using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令服务客户端接收数据回调类型
    /// </summary>
    internal enum ClientReceiveTypeEnum : byte
    {
        /// <summary>
        /// 获取命令回调序号
        /// </summary>
        CallbackIdentity,
        /// <summary>
        /// 继续获取命令回调序号
        /// </summary>
        CallbackIdentityAgain,
        /// <summary>
        /// 获取数据
        /// </summary>
        Data,
        /// <summary>
        /// 获取临时数据
        /// </summary>
        BigData,
    }
}
