using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// TCP 服务端接收数据回调类型
    /// </summary>
    internal enum ServerReceiveTypeEnum : byte
    {
        None,
        /// <summary>
        /// 获取验证命令
        /// </summary>
        VerifyCommand,
        /// <summary>
        /// 获取验证数据
        /// </summary>
        VerifyData,
        /// <summary>
        /// 继续获取验证数据
        /// </summary>
        VerifyDataAgain,
        /// <summary>
        /// 获取命令
        /// </summary>
        Command,
        /// <summary>
        /// 继续获取命令
        /// </summary>
        CommandAgain,
        /// <summary>
        /// 获取数据
        /// </summary>
        Data,
        /// <summary>
        /// 获取临时数据
        /// </summary>
        BigData,
        /// <summary>
        /// 获取压缩数据
        /// </summary>
        CompressionData,
        /// <summary>
        /// 获取临时压缩数据
        /// </summary>
        CompressionBigData,
    }
}
