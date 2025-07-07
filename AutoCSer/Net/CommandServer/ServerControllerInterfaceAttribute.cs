using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The command service controller interface generates configuration
    /// 命令服务控制器接口生成配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServerControllerInterfaceAttribute : Attribute
    {
        /// <summary>
        /// Method sequence number maps enumeration type
        /// 方法序号映射枚举类型
        /// </summary>
#if NetStandard21
        public readonly Type? MethodIndexEnumType;
#else
        public readonly Type MethodIndexEnumType;
#endif
        /// <summary>
        /// The command service controller interface generates configuration
        /// 命令服务控制器接口生成配置
        /// </summary>
        /// <param name="methodIndexEnumType">Method sequence number maps enumeration type
        /// 方法序号映射枚举类型</param>
#if NetStandard21
        public ServerControllerInterfaceAttribute(Type? methodIndexEnumType = null)
#else
        public ServerControllerInterfaceAttribute(Type methodIndexEnumType = null)
#endif
        {
            MethodIndexEnumType = methodIndexEnumType;
        }

        /// <summary>
        /// Default configuration
        /// </summary>
        internal static readonly ServerControllerInterfaceAttribute Default = new ServerControllerInterfaceAttribute();
    }
}
