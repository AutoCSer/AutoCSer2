using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// TCP 服务端接收数据错误类型
    /// </summary>
    public enum ServerReceiveErrorTypeEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异常
        /// </summary>
        Exception,
        /// <summary>
        /// 验证超时
        /// </summary>
        VerifyTimeout,
        /// <summary>
        /// 验证命令数据不足
        /// </summary>
        VerifyCommandSizeLess,
        /// <summary>
        /// 验证命令序号不匹配
        /// </summary>
        VerifyCommandIdentityError,
        /// <summary>
        /// 验证数据长度解析错误
        /// </summary>
        VerifyDataSizeError,
        /// <summary>
        /// 验证数据不足
        /// </summary>
        VerifyDataSizeLess,
        /// <summary>
        /// 验证数据长度超出最大限制
        /// </summary>
        VerifyDataSizeLimitError,
        /// <summary>
        /// 验证数据长度超出解析长度
        /// </summary>
        VerifyDataSizeOutOfRange,
        /// <summary>
        /// 验证数据解码失败
        /// </summary>
        VerifyDataDecodeError,
        /// <summary>
        /// 验证失败
        /// </summary>
        VerifyError,
        /// <summary>
        /// 接收数据不完整时连续两次接收数据不足
        /// </summary>
        ReceiceSizeLess,
        /// <summary>
        /// 命令数据不足
        /// </summary>
        CommandSizeLess,
        /// <summary>
        /// 命令不可识别
        /// </summary>
        CommandError,
        /// <summary>
        /// 命令数据长度解析错误
        /// </summary>
        DataSizeError,
        /// <summary>
        /// 命令数据超出最大限制
        /// </summary>
        DataSizeLimitError,
        /// <summary>
        /// 数据解码失败
        /// </summary>
        DataDecodeError,
        /// <summary>
        /// 临时数据解码失败
        /// </summary>
        BigDataDecodeError,
        /// <summary>
        /// 合并命令数据超出最大限制
        /// </summary>
        MergeDataSizeLimitError,
        /// <summary>
        /// 合并命令数据长度不足
        /// </summary>
        MergeDataSizeLess,
        /// <summary>
        /// 自定义命令数据长度解析错误
        /// </summary>
        CustomDataSizeError,
        /// <summary>
        /// 自定义命令处理错误
        /// </summary>
        CustomCommandError,
        /// <summary>
        /// 短连接不支持系统命令
        /// </summary>
        ShortLinkCommandError,
        /// <summary>
        /// 短连接接收数据长度超出命令范围（客户端可能是长连接模式并且发送了多个命令数据）
        /// </summary>
        ShortLinkDataSizeError,
    }
}
