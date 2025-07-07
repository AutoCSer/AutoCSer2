using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Custom configuration of client node
    /// 客户端节点自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ClientNodeAttribute : Attribute
    {
        /// <summary>
        /// Local client node constructor name
        /// 本地客户端节点构造函数名称
        /// </summary>
        internal const string LocalClientNodeConstructorMethodName = "LocalClientNodeConstructor";
        /// <summary>
        /// The name of the method for activating AOT reflection at the local client node
        /// 本地客户端节点激活 AOT 反射的方法名称
        /// </summary>
        internal const string LocalClientNodeMethodName = "LocalClientNode";

        /// <summary>
        /// The matching server node interface type
        /// 匹配服务端节点接口类型
        /// </summary>
        public readonly Type ServerNodeType;
#if AOT
        /// <summary>
        /// The code generates the local client type
        /// 代码生成本地客户端类型
        /// </summary>
        internal readonly Type ClientNodeType;
        /// <summary>
        /// Custom configuration of client node
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">The matching server node interface type
        /// 匹配服务端节点接口类型</param>
        /// <param name="clientNodeType">The code generates the local client type
        /// 代码生成本地客户端类型</param>
        public ClientNodeAttribute(Type serverNodeType, Type clientNodeType)
        {
            ServerNodeType = serverNodeType;
            ClientNodeType = clientNodeType;
        }
        /// <summary>
        /// Custom configuration of client node
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">The matching server node interface type
        /// 匹配服务端节点接口类型</param>
        internal ClientNodeAttribute(Type serverNodeType)
        {
            ServerNodeType = ClientNodeType = serverNodeType;
        }
#else
        /// <summary>
        /// Custom configuration of client node
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">The matching server node interface type
        /// 匹配服务端节点接口类型</param>
        public ClientNodeAttribute(Type serverNodeType)
        {
            ServerNodeType = serverNodeType;
        }
#endif
    }
}
