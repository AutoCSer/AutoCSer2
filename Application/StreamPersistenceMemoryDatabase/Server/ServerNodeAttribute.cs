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
        ///// <summary>
        ///// 默认为 false 表示仅生成当前接口定义方法，否则生成所有被继承接口方法
        ///// </summary>
        //public bool IsClientCodeGeneratorOnlyDeclaringMethod;
#if AOT
        ///// <summary>
        ///// Whether it is the AOT code generation mode
        ///// 是否 AOT 代码生成模式
        ///// </summary>
        //internal const bool IsAOT = true;
        /// <summary>
        /// The default is true, indicating the generation of the local client interface code
        /// 默认为 true 表示生成本地客户端接口代码
        /// </summary>
        public bool IsLocalClient = true;
        /// <summary>
        /// Generate the local client node name
        /// 生成本地客户端节点名称
        /// </summary>
        public string? LocalClientTypeName;
#else
        /// <summary>
        /// The default is true, indicating the generation of remote client interface code
        /// 默认为 true 表示生成远程客户端接口代码
        /// </summary>
        public bool IsClient = true;
        /// <summary>
        /// The default is false, indicating that no local client interface code is generated
        /// 默认为 false 表示不生成本地客户端接口代码
        /// </summary>
        public bool IsLocalClient;
        ///// <summary>
        ///// Whether it is the AOT code generation mode
        ///// 是否 AOT 代码生成模式
        ///// </summary>
        //internal const bool IsAOT = false;
#endif
    }
}
