using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Create node index information
    /// 创建节点索引信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed partial class CreateNodeIndex
    {
        /// <summary>
        /// Node index information
        /// 节点索引信息
        /// </summary>
        internal NodeIndex Index;
    }
}
