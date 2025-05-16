using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 客户端命令控制器代码生成配置（用于 AOT 环境）
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class CommandClientControllerAttribute : Attribute
    {
        /// <summary>
        /// 命令客户端控制器构造函数调用方法名称
        /// </summary>
        internal const string CommandClientControllerConstructorMethodName = "__CommandClientControllerConstructor__";
        /// <summary>
        /// 获取客户端接口方法信息方法名称
        /// </summary>
        internal const string CommandClientControllerMethodName = "__CommandClientControllerMethods__";

        /// <summary>
        /// 服务端控制器接口类型
        /// </summary>
        internal readonly Type ServerInterfaceType;
        /// <summary>
        /// 生成控制器名称
        /// </summary>
        internal readonly string? ControllerTypeName;
        /// <summary>
        /// 客户端命令控制器代码生成配置
        /// </summary>
        /// <param name="serverInterfaceType">服务端控制器接口类型，非对称接口传参 typeof(void)</param>
        /// <param name="controllerTypeName">代码生成客户端控制器类型名称，默认为 null 表示默认规则名称</param>
        public CommandClientControllerAttribute(Type serverInterfaceType, string? controllerTypeName = null)
        {
            ServerInterfaceType = serverInterfaceType;
            ControllerTypeName = controllerTypeName;
        }
    }
}
