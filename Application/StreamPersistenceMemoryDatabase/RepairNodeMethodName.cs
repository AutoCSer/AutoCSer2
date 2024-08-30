using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复节点方法名称信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct RepairNodeMethodName
    {
        /// <summary>
        /// 静态方法定义类型名称
        /// </summary>
        public string DeclaringTypeFullName;
        /// <summary>
        /// 修复静态方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息
        /// </summary>
        public string Name;
        /// <summary>
        /// 节点类型名称
        /// </summary>
        public string NodeTypeFullName;
        /// <summary>
        /// 修复节点方法名称信息
        /// </summary>
        /// <param name="method">静态方法信息</param>
        /// <param name="nodeType">节点类型</param>
        internal RepairNodeMethodName(MethodInfo method, Type nodeType = null)
        {
            DeclaringTypeFullName = method.DeclaringType.FullName;
            Name = method.Name;
            NodeTypeFullName = nodeType?.FullName;
        }
    }
}
