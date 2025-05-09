using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 生成服务端节点快照方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotMethodCreatorInfo
    {
        /// <summary>
        /// 方法数组索引位置
        /// </summary>
        public readonly int MethodArrayIndex;
        /// <summary>
        /// 快照数据类型
        /// </summary>
        public readonly Type SnapshotType;
        /// <summary>
        /// 快照序列化委托
        /// </summary>
        public readonly Delegate SerializeDelegate;
        /// <summary>
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
