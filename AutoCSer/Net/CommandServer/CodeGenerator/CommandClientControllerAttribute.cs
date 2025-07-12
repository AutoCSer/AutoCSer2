using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// AOT client command controller code generation configuration
    /// AOT 客户端命令控制器代码生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class CommandClientControllerAttribute : Attribute
    {
        /// <summary>
        /// The method name of the constructor of the command client controller is called
        /// 命令客户端控制器构造函数调用方法名称
        /// </summary>
        internal const string CommandClientControllerConstructorMethodName = "__CommandClientControllerConstructor__";
        /// <summary>
        /// The method name of get client interface method information
        /// 获取客户端接口方法信息方法名称
        /// </summary>
        internal const string CommandClientControllerMethodName = "__CommandClientControllerMethods__";

        /// <summary>
        /// Server-side controller interface type
        /// 服务端控制器接口类型
        /// </summary>
        internal readonly Type ServerInterfaceType;
        /// <summary>
        /// Generate the controller name
        /// 生成控制器名称
        /// </summary>
        internal readonly string? ControllerTypeName;
        /// <summary>
        /// Whether to generate the default controller for the client
        /// 是否生成客户端默认控制器
        /// </summary>
        internal readonly bool IsDefaultController;
        /// <summary>
        /// AOT client command controller code generation configuration
        /// AOT 客户端命令控制器代码生成配置
        /// </summary>
        /// <param name="serverInterfaceType">Server controller interface type, asymmetric interface parameter typeof(void)
        /// 服务端控制器接口类型，非对称接口传参 typeof(void)</param>
        /// <param name="isDefaultController">A default value of false indicates that no default controller for the client is generated
        /// 默认为 false 表示不生成客户端默认控制器</param>
        /// <param name="controllerTypeName">The code generates the type name of the client controller. By default, null represents the default rule name
        /// 代码生成客户端控制器类型名称，默认为 null 表示默认规则名称</param>
        public CommandClientControllerAttribute(Type serverInterfaceType, bool isDefaultController = false, string? controllerTypeName = null)
        {
            ServerInterfaceType = serverInterfaceType;
            IsDefaultController = isDefaultController;
            ControllerTypeName = controllerTypeName;
        }
    }
}
