using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

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
#if NetStandard21
        public string? NodeTypeFullName;
#else
        public string NodeTypeFullName;
#endif
        /// <summary>
        /// 修复节点方法名称信息
        /// </summary>
        /// <param name="method">静态方法信息</param>
        /// <param name="nodeType">节点类型</param>
#if NetStandard21
        internal RepairNodeMethodName(MethodInfo method, Type? nodeType = null)
#else
        internal RepairNodeMethodName(MethodInfo method, Type nodeType = null)
#endif
        {
            DeclaringTypeFullName = method.DeclaringType.notNull().FullName.notNull();
            Name = method.Name;
            NodeTypeFullName = nodeType?.FullName;
        }
        /// <summary>
        /// 设置修复节点方法名称信息
        /// </summary>
        /// <param name="declaringTypeFullName"></param>
        /// <param name="name"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(string declaringTypeFullName, string name)
        {
            DeclaringTypeFullName = declaringTypeFullName;
            Name = name;
        }
    }
}
