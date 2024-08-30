using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 请求参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct RequestParameter : AutoCSer.BinarySerialize.ICustomSerialize<RequestParameter>
    {
        /// <summary>
        /// 节点索引信息
        /// </summary>
        private NodeIndex index;
        /// <summary>
        /// 调用方法编号
        /// </summary>
        private int methodIndex;
        /// <summary>
        /// 调用状态
        /// </summary>
        internal CallStateEnum CallState;
        /// <summary>
        /// 序列化委托
        /// </summary>
        private readonly Action<BinarySerializer> serializer;
        /// <summary>
        /// 调用方法信息
        /// </summary>
        internal InputMethodParameter MethodParameter;
        /// <summary>
        /// 请求参数序列化
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="serializer">序列化委托</param>
        internal RequestParameter(NodeIndex index, int methodIndex, Action<BinarySerializer> serializer)
        {
            this.index = index;
            this.methodIndex = methodIndex;
            this.serializer = serializer;
            CallState = CallStateEnum.Success;
            MethodParameter = null;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RequestParameter>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.Write(index.Index);
            stream.Write(index.Identity);
            stream.Write(methodIndex);
            this.serializer(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RequestParameter>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (deserializer.Read(out index.Index) && deserializer.Read(out index.Identity) && deserializer.Read(out methodIndex))
            {
                CommandServerSocketSessionObjectService service = CommandServerSocketSessionObjectService.GetSessionObject(deserializer);
                if (service != null)
                {
                    MethodParameter = service.CreateInputMethodParameter(index, methodIndex, out CallState);
                    if (MethodParameter != null)
                    {
                        MethodParameter.Deserialize(deserializer);
                        return;
                    }
                }
                if (CallState == CallStateEnum.Unknown) CallState = CallStateEnum.NotFoundSessionObject;
                SubArray<byte> buffer = default(SubArray<byte>);
                deserializer.DeserializeBuffer(ref buffer, true);
            }
        }
    }
}
