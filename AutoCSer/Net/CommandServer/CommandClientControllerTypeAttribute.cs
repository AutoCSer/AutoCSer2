using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令客户端控制器类型
    /// </summary>
    public sealed class CommandClientControllerTypeAttribute : Attribute
    {
        /// <summary>
        /// 代码生成客户端类型
        /// </summary>
        internal readonly Type ClientType;
        /// <summary>
        /// 命令客户端控制器类型
        /// </summary>
        /// <param name="clientType">命令客户端控制器类型</param>
        public CommandClientControllerTypeAttribute(Type clientType)
        {
            ClientType = clientType;
        }
    }
}
