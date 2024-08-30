using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点接口配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServerNodeAttribute : AutoCSer.Net.CommandServer.InterfaceMethodIndexAttribute
    {
        /// <summary>
        /// 默认为 false 表示仅生成当前接口定义方法，否则生成所有被继承接口方法
        /// </summary>
        public bool IsClientCodeGeneratorOnlyDeclaringMethod;
    }
}
