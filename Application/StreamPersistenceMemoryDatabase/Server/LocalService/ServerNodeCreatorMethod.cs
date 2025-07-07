using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Method information of generate server-side node
    /// 生成服务端节点方法信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ServerNodeCreatorMethod
    {
        /// <summary>
        /// Node method collection
        /// 节点方法集合
        /// </summary>
        public readonly Method?[] Methods;
        /// <summary>
        /// Node method information collection
        /// 节点方法信息集合
        /// </summary>
        public readonly ServerNodeMethodInfo?[] NodeMethods;
        /// <summary>
        /// Snapshot method collection
        /// 快照方法集合
        /// </summary>
        public readonly SnapshotMethodInfo[] SnapshotMethods;
        /// <summary>
        /// Method information of generate server-side node
        /// 生成服务端节点方法信息
        /// </summary>
        /// <param name="methods">Node method collection
        /// 节点方法集合</param>
        /// <param name="nodeMethods">Node method information collection
        /// 节点方法信息集合</param>
        /// <param name="snapshotTypes">Snapshot method collection
        /// 快照数据类型集合</param>
        public ServerNodeCreatorMethod(Method?[] methods, ServerNodeMethodInfo?[] nodeMethods, SnapshotMethodCreatorInfo[] snapshotTypes)
        {
            Methods = methods;
            NodeMethods = nodeMethods;
            LeftArray<SnapshotMethodInfo> snapshotMethods = new LeftArray<SnapshotMethodInfo>(0);
            foreach (SnapshotMethodCreatorInfo type in snapshotTypes) snapshotMethods.Add(new SnapshotMethodInfo(type.SnapshotType, methods[type.MethodArrayIndex].notNull(), type.SerializeDelegate));
            SnapshotMethods = snapshotMethods.ToArray();
        }
    }
}
