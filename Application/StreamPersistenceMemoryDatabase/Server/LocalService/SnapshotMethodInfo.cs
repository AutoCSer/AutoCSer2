using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Snapshot method information
    /// 快照方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotMethodInfo
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
        internal readonly Method Method;
        /// <summary>
        /// Delegate for snapshot serialization
        /// 快照序列化委托
        /// </summary>
        internal readonly Delegate SerializeDelegate;
        /// <summary>
        /// Snapshot method information
        /// 快照方法信息
        /// </summary>
        /// <param name="type">Snapshot data type
        /// 快照数据类型</param>
        /// <param name="method">Server node method
        /// 服务端节点方法</param>
        /// <param name="serializeDelegate">Delegate for snapshot serialization
        /// 快照序列化委托</param>
        internal SnapshotMethodInfo(Type type, Method method, Delegate serializeDelegate)
        {
            Type = type;
            Method = method;
            SerializeDelegate = serializeDelegate;
        }
    }
}
