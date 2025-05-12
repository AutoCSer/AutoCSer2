using AutoCSer.Reflection;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建节点信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CreatingNodeInfo
    {
        /// <summary>
        /// 服务端节点信息
        /// </summary>
        private readonly NodeInfo nodeInfo;
        /// <summary>
        /// 节点索引
        /// </summary>
        internal readonly int Index;
        /// <summary>
        /// 创建节点信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nodeInfo"></param>
        internal CreatingNodeInfo(int index, NodeInfo nodeInfo)
        {
            this.Index = index;
            this.nodeInfo = nodeInfo;
        }
        /// <summary>
        /// 检查节点信息是否匹配
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check(NodeInfo nodeInfo)
        {
            return this.nodeInfo.RemoteType.Equals(nodeInfo.RemoteType);
        }
    }
}
