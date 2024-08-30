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
        /// 匹配服务端节点接口类型
        /// </summary>
        public Type ServerNodeType;
    }
}
