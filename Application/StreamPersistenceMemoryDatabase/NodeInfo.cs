using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点信息 
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true, IsReferenceMember = false)]
    [AutoCSer.JsonSerialize(Filter = AutoCSer.Metadata.MemberFiltersEnum.InstanceField)]
    public partial class NodeInfo
    {
        /// <summary>
        /// 服务端节点接口类型
        /// </summary>
        internal AutoCSer.Reflection.RemoteType RemoteType;

        /// <summary>
        /// 服务端节点信息
        /// </summary>
        /// <param name="type">服务端节点接口类型</param>
        internal NodeInfo(Type type)
        {
            RemoteType = type;
        }
    }
}
