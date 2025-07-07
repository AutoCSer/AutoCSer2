using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server-side node information
    /// 服务端节点信息 
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMix = true, IsReferenceMember = false)]
    [AutoCSer.JsonSerialize(Filter = AutoCSer.Metadata.MemberFiltersEnum.InstanceField)]
    public partial class NodeInfo
    {
        /// <summary>
        /// Server node interface type
        /// 服务端节点接口类型
        /// </summary>
        internal AutoCSer.Reflection.RemoteType RemoteType;

        /// <summary>
        /// Server-side node information
        /// 服务端节点信息
        /// </summary>
        /// <param name="type">Server node interface type
        /// 服务端节点接口类型</param>
        internal NodeInfo(Type type)
        {
            RemoteType = type;
        }
    }
}
