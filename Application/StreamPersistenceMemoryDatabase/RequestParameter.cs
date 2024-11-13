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
        internal NodeIndex Index;
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
        private readonly RequestParameterSerializer serializer;
        /// <summary>
        /// 调用方法信息
        /// </summary>
#if NetStandard21
        internal InputMethodParameter? MethodParameter;
#else
        internal InputMethodParameter MethodParameter;
#endif
        /// <summary>
        /// 请求参数序列化
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="serializer">序列化委托</param>
        internal RequestParameter(NodeIndex index, int methodIndex, RequestParameterSerializer serializer)
        {
            this.Index = index;
            this.methodIndex = methodIndex;
            this.serializer = serializer;
            CallState = CallStateEnum.Success;
            MethodParameter = null;
        }
        ///// <summary>
        ///// 请求参数序列化
        ///// </summary>
        ///// <param name="index">节点索引信息</param>
        ///// <param name="methodIndex">调用方法编号</param>
        ///// <param name="parameter">调用方法信息</param>
        //internal RequestParameter(NodeIndex index, int methodIndex, InputMethodParameter parameter)
        //{
        //    this.Index = index;
        //    this.methodIndex = methodIndex;
        //    this.serializer = null;
        //    CallState = CallStateEnum.Success;
        //    MethodParameter = parameter;
        //}
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RequestParameter>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.Write(Index.Index);
            stream.Write(Index.Identity);
            stream.Write(methodIndex);
            this.serializer.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RequestParameter>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (deserializer.Read(out Index.Index) && deserializer.Read(out Index.Identity) && deserializer.Read(out methodIndex))
            {
                CommandServerSocket socket = deserializer.Context.castType<CommandServerSocket>().notNull();
                if (!object.ReferenceEquals(socket, CommandServerSocket.CommandServerSocketContext))
                {
                    CommandServerController<IStreamPersistenceMemoryDatabaseService> controller = (CommandServerController<IStreamPersistenceMemoryDatabaseService>)socket.CurrentController;
                    StreamPersistenceMemoryDatabaseService service = (StreamPersistenceMemoryDatabaseService)controller.Controller;
                    MethodParameter = service.CreateInputMethodParameter(Index, methodIndex, out CallState);
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
