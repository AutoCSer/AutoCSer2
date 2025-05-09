using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotMethodInfo
    {
        /// <summary>
        /// 快照数据类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        internal readonly Method Method;
        /// <summary>
        /// 快照序列化委托
        /// </summary>
        internal readonly Delegate SerializeDelegate;
        /// <summary>
        /// 快照方法信息
        /// </summary>
        /// <param name="type">快照数据类型</param>
        /// <param name="method">服务端节点方法</param>
        /// <param name="serializeDelegate">快照序列化委托</param>
        internal SnapshotMethodInfo(Type type, Method method, Delegate serializeDelegate)
        {
            Type = type;
            Method = method;
            SerializeDelegate = serializeDelegate;
        }
    }
}
