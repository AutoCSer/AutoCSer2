using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 客户端节点自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ClientNodeAttribute : Attribute
    {
        /// <summary>
        /// 本地客户端节点构造函数名称
        /// </summary>
        internal const string LocalClientNodeConstructorMethodName = "LocalClientNodeConstructor";
        /// <summary>
        /// 本地客户端节点激活 AOT 反射函数名称
        /// </summary>
        internal const string LocalClientNodeMethodName = "LocalClientNode";

        /// <summary>
        /// 匹配服务端节点接口类型
        /// </summary>
        public readonly Type ServerNodeType;
#if AOT
        /// <summary>
        /// 代码生成本地客户端类型
        /// </summary>
        internal readonly Type ClientNodeType;
        /// <summary>
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">匹配服务端节点接口类型</param>
        /// <param name="clientNodeType">代码生成本地客户端类型</param>
        public ClientNodeAttribute(Type serverNodeType, Type clientNodeType)
        {
            ServerNodeType = serverNodeType;
            ClientNodeType = clientNodeType;
        }
        /// <summary>
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">匹配服务端节点接口类型</param>
        internal ClientNodeAttribute(Type serverNodeType)
        {
            ServerNodeType = ClientNodeType = serverNodeType;
        }
#else
        /// <summary>
        /// 客户端节点自定义属性
        /// </summary>
        /// <param name="serverNodeType">匹配服务端节点接口类型</param>
        public ClientNodeAttribute(Type serverNodeType)
        {
            ServerNodeType = serverNodeType;
        }
#endif
    }
}
