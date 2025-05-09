using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点信息 
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct NodeInfo
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
