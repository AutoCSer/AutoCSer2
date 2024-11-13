using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照方法序列化调用
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SnapshotMethodSerializer
    {
        /// <summary>
        /// 二进制序列化
        /// </summary>
        private readonly AutoCSer.BinarySerializer serializer;
        /// <summary>
        /// 节点标识
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        private readonly bool isSimpleSerializeParamter;
        /// <summary>
        /// 快照方法序列化调用
        /// </summary>
        /// <param name="serializer">二进制序列化</param>
        /// <param name="node">节点信息</param>
        internal SnapshotMethodSerializer(AutoCSer.BinarySerializer serializer, ServerNode node)
        {
            this.serializer = serializer;
            nodeIndex = node.Index;
            Method method = node.NodeCreator.SnapshotMethod.notNull();
            methodIndex = method.Index;
            isSimpleSerializeParamter = method.IsSimpleDeserializeParamter;
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns>是否序列化成功</returns>
        internal unsafe bool Serialize<T>(T parameter)
        {
            UnmanagedStream stream = serializer.Stream;
            byte* data = stream.GetBeforeMove(sizeof(NodeIndex) + sizeof(int));
            if (data != null)
            {
                *(NodeIndex*)data = nodeIndex;
                *(int*)(data + sizeof(NodeIndex)) = methodIndex;

                ServerReturnValue<T> returnValue = new ServerReturnValue<T>(parameter);
                if (isSimpleSerializeParamter) serializer.SimpleSerialize(ref returnValue);
                else serializer.InternalIndependentSerializeNotReference(ref returnValue);
                return !stream.IsResizeError;
            }
            return false;
        }
    }
}
