using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册日志客户端变更类型
    /// </summary>
    [Flags]
    public enum ServiceRegisterLogClientChangedType : byte
    {
        /// <summary>
        /// 主服务日志被修改
        /// </summary>
        Main = 1,
        /// <summary>
        /// 附加服务日志
        /// </summary>
        Append = 2,
        /// <summary>
        /// 删除服务日志
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 服务失联日志
        /// </summary>
        LostContact = 8
    }
}
