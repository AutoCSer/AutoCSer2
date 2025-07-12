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
        /// 代码生成客户端默认控制器类型
        /// </summary>
        internal readonly Type? DefaultControllerType;
        /// <summary>
        /// 命令客户端控制器类型
        /// </summary>
        /// <param name="clientType">命令客户端控制器类型</param>
        /// <param name="defaultControllerType">代码生成客户端默认控制器类型</param>
        public CommandClientControllerTypeAttribute(Type clientType, Type? defaultControllerType = null)
        {
            ClientType = clientType;
            DefaultControllerType = defaultControllerType;
        }
    }
}
