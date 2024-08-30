using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件拉取状态
    /// </summary>
    public enum PullFileStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 文件不存在
        /// </summary>
        NotExist,
        /// <summary>
        /// 文件长度不足
        /// </summary>
        LengthLess,
        /// <summary>
        /// 文件最后修改时间不匹配
        /// </summary>
        LastWriteTimeNotMatch,
        /// <summary>
        /// 拉取完成，存在部分错误
        /// </summary>
        Completed,
        /// <summary>
        /// 初始化操作仅允许调用一次
        /// </summary>
        InitializeOnlyOnce,
        /// <summary>
        /// 服务接口调用失败
        /// </summary>
        CallFail,
        /// <summary>
        /// 客户端异常
        /// </summary>
        ClientException,
        /// <summary>
        /// 客户端反序列化错误
        /// </summary>
        DeserializeError,
    }
}
