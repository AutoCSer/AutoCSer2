using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务注册状态
    /// </summary>
    public enum ServerRegistryStateEnum : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 不支持的服务名称
        /// </summary>
        UnsupportedServerName,
        /// <summary>
        /// 不可识别服务注册日志操作类型
        /// </summary>
        UnrecognizableOperationType,
        /// <summary>
        /// 没有找到服务会话标识ID
        /// </summary>
        NotFoundServerSessionID,
        /// <summary>
        /// 没有找到有效的服务会话回调委托
        /// </summary>
        NotFoundServerSessionCallback,
        /// <summary>
        /// 没有找到服务名称
        /// </summary>
        NotFoundServerName,
        /// <summary>
        /// 注册服务版本过低
        /// </summary>
        VersionTooLow,
        /// <summary>
        /// 服务注册日志操作类型冲突
        /// </summary>
        OperationTypeConflict,

        ///// <summary>
        ///// 获取服务注册日志回调委托冲突
        ///// </summary>
        //NewLogCallback,
    }
}
