using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Snapshot method information
    /// 快照方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SnapshotMethod
    {
        /// <summary>
        /// Snapshot data type
        /// 快照数据类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// Server node method
        /// 服务端节点方法
        /// </summary>
        internal Method Method;
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        internal ServerNodeMethod NodeMethod;
        /// <summary>
        /// Snapshot method information
        /// 快照方法信息
        /// </summary>
        /// <param name="type">Snapshot data type
        /// 快照数据类型</param>
        /// <param name="method">Server node method
        /// 服务端节点方法</param>
        /// <param name="nodeMethod">Server node method information
        /// 服务端节点方法信息</param>
        internal SnapshotMethod(Type type, Method method, ServerNodeMethod nodeMethod)
        {
            Type = type;
            Method = method;
            NodeMethod = nodeMethod;
        }
    }
}
