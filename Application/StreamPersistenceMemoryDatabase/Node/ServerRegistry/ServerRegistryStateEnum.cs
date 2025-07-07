using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server registration status
    /// 服务注册状态
    /// </summary>
    public enum ServerRegistryStateEnum : byte
    {
        /// <summary>
        /// Unknown error
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// The operation was successful
        /// 操作成功
        /// </summary>
        Success,
        /// <summary>
        /// Unsupported service name
        /// 不支持的服务名称
        /// </summary>
        UnsupportedServerName,
        /// <summary>
        /// The operation type of the server registration log cannot be identified
        /// 不可识别服务注册日志操作类型
        /// </summary>
        UnrecognizableOperationType,
        /// <summary>
        /// The service session identity was not found
        /// 没有找到服务会话标识
        /// </summary>
        NotFoundServerSessionID,
        /// <summary>
        /// No valid service session callback delegate was found
        /// 没有找到有效的服务会话回调委托
        /// </summary>
        NotFoundServerSessionCallback,
        /// <summary>
        /// The service name was not found
        /// 没有找到服务名称
        /// </summary>
        NotFoundServerName,
        /// <summary>
        /// The registration service version is too low
        /// 注册服务版本过低
        /// </summary>
        VersionTooLow,
        /// <summary>
        /// Server registration log operation type conflict
        /// 服务注册日志操作类型冲突
        /// </summary>
        OperationTypeConflict,

        ///// <summary>
        ///// 获取服务注册日志回调委托冲突
        ///// </summary>
        //NewLogCallback,
    }
}
