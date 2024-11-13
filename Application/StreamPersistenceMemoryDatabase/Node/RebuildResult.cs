using AutoCSer.Reflection;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 重建持久化文件调用结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RebuildResult
    {
        /// <summary>
        /// 调用返回状态，Success 表示提交成功正在处理中
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// 初始化加载执行异常节点状态
        /// </summary>
        public CallStateEnum LoadExceptionNodeState;
        /// <summary>
        /// 初始化加载执行异常节点名称
        /// </summary>
        public string LoadExceptionNodeKey;
        /// <summary>
        /// 初始化加载执行异常节点接口类型
        /// </summary>
        public RemoteType LoadExceptionNodeType;
        /// <summary>
        /// 重建持久化文件调用结果
        /// </summary>
        /// <param name="callState"></param>
        internal RebuildResult(CallStateEnum callState)
        {
            CallState = callState;
            LoadExceptionNodeState = CallStateEnum.Unknown;
            LoadExceptionNodeKey = string.Empty;
            LoadExceptionNodeType = default(RemoteType);
        }
        /// <summary>
        /// 重建持久化文件调用结果
        /// </summary>
        /// <param name="node"></param>
        internal RebuildResult(ServerNode node)
        {
            CallState = CallStateEnum.LoadException;
            LoadExceptionNodeState = node.CallState;
            LoadExceptionNodeKey = node.Key;
            LoadExceptionNodeType = new RemoteType(node.NodeCreator.Type);
        }
    }
}
