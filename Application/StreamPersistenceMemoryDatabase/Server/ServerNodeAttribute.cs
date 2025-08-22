using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node interface configuration
    /// 服务端节点接口配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServerNodeAttribute : AutoCSer.Net.CommandServer.InterfaceMethodIndexAttribute
    {
        /// <summary>
        /// The default is true, indicating the generation of remote client interface code (Only for JIT mode)
        /// 默认为 true 表示生成远程客户端接口代码（仅用于 JIT 模式）
        /// </summary>
        public bool IsClient = true;
        /// <summary>
        /// A default value of true indicates the API encapsulation type that generates a direct return value
        /// 默认为 true 表示生成直接返回值的 API 封装类型
        /// </summary>
        public bool IsReturnValueNode = true;
#if AOT
        /// <summary>
        /// The default is true, indicating the generation of the local client interface code
        /// 默认为 true 表示生成本地客户端接口代码
        /// </summary>
        public bool IsLocalClient = true;
        /// <summary>
        /// 默认为 false 表示 AutoCSer 内置节点不生成直接返回值的 API 封装类型（用于泛型接口定义）
        /// </summary>
        internal const bool DefaultIsReturnValueNode = false;
#else
        /// <summary>
        /// The default is false, indicating that no local client interface code is generated
        /// 默认为 false 表示不生成本地客户端接口代码
        /// </summary>
        public bool IsLocalClient;
        /// <summary>
        /// 默认为 true 表示 AutoCSer 内置节点生成直接返回值的 API 封装类型（用于泛型接口定义）
        /// </summary>
        internal const bool DefaultIsReturnValueNode = true;
#endif
        /// <summary>
        /// Generate the local client node name (Only for.NET NativeAOT)
        /// 生成本地客户端节点名称（仅用于 .NET NativeAOT）
        /// </summary>
#if NetStandard21
        public string? LocalClientTypeName;
#else
        public string LocalClientTypeName;
#endif
    }
}
