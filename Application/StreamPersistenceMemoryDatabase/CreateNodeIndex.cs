using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建节点索引信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class CreateNodeIndex
    {
        /// <summary>
        /// 节点索引信息
        /// </summary>
        internal NodeIndex Index;
    }
}
