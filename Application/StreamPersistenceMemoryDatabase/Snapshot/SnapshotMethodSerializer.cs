using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照方法序列化调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SnapshotMethodSerializer<T>
    {
        /// <summary>
        /// 二进制序列化
        /// </summary>
        private readonly AutoCSer.BinarySerializer serializer;
        /// <summary>
        /// 节点标识
        /// </summary>
        private readonly NodeIndex nodeIndex;
#if AOT
        /// <summary>
        /// 序列化委托
        /// </summary>
        private readonly Action<AutoCSer.BinarySerializer, T?> serializeDelegate;
#else
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        private readonly bool isSimpleSerializeParamter;
#endif
        /// <summary>
        /// Method Number
        /// 方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// 快照方法序列化调用
        /// </summary>
        /// <param name="serializer">二进制序列化</param>
        /// <param name="node">节点信息</param>
        internal SnapshotMethodSerializer(AutoCSer.BinarySerializer serializer, ServerNode node)
        {
            this.serializer = serializer;
            nodeIndex = node.Index;
#if AOT
            var serializeDelegate = default(Delegate);
            Method method = node.NodeCreator.GetSnapshotMethod(typeof(T), out serializeDelegate);
            this.serializeDelegate = (Action<AutoCSer.BinarySerializer, T?>)serializeDelegate;
#else
            Method method = node.NodeCreator.GetSnapshotMethod(typeof(T));
            isSimpleSerializeParamter = method.IsSimpleDeserializeParamter;
#endif
            methodIndex = method.Index;
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>是否序列化成功</returns>
#if NetStandard21
        internal unsafe bool Serialize(T? parameter)
#else
        internal unsafe bool Serialize(T parameter)
#endif
        {
            UnmanagedStream stream = serializer.Stream;
            byte* data = stream.GetBeforeMove(sizeof(NodeIndex) + sizeof(int));
            if (data != null)
            {
                *(NodeIndex*)data = nodeIndex;
                *(int*)(data + sizeof(NodeIndex)) = methodIndex;
#if AOT
                serializeDelegate(serializer, parameter);
#else
                if (isSimpleSerializeParamter)
                {
                    ServerReturnValue<T> snapshotMethodParameter = new ServerReturnValue<T>(parameter);
                    serializer.SimpleSerialize(ref snapshotMethodParameter);
                }
                else
                {
                    ServerReturnValue<T> snapshotMethodParameter = new ServerReturnValue<T>(parameter);
                    serializer.InternalIndependentSerializeNotNull(ref snapshotMethodParameter);
                }
#endif
                return !stream.IsResizeError;
            }
            return false;
        }
    }
}
