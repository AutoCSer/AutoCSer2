using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Snapshot Method information of generate server-side node
    /// 生成服务端节点快照方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotMethodCreatorInfo
    {
        /// <summary>
        /// Method array index position
        /// 方法数组索引位置
        /// </summary>
        public readonly int MethodArrayIndex;
        /// <summary>
        /// Snapshot data type
        /// 快照数据类型
        /// </summary>
        public readonly Type SnapshotType;
        /// <summary>
        /// Delegate for snapshot serialization
        /// 快照序列化委托
        /// </summary>
        public readonly Delegate SerializeDelegate;
        /// <summary>
        /// Snapshot Method information of generate server-side node
        /// 生成服务端节点快照方法信息
        /// </summary>
        /// <param name="methodArrayIndex"></param>
        /// <param name="snapshotType"></param>
        /// <param name="serializeDelegate"></param>
        public SnapshotMethodCreatorInfo(int methodArrayIndex, Type snapshotType, Delegate serializeDelegate)
        {
            SnapshotType = snapshotType;
            MethodArrayIndex = methodArrayIndex;
            SerializeDelegate = serializeDelegate;
        }
    }
}
