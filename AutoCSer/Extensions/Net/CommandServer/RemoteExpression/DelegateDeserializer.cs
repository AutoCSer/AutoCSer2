using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 表达式反序列化信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DelegateDeserializer
    {
        /// <summary>
        /// 远程表达式序列化信息
        /// </summary>
        internal SerializeInfo SerializeInfo;
        /// <summary>
        /// Command server socket
        /// 命令服务套接字
        /// </summary>
        internal readonly CommandServerSocket Socket;
        /// <summary>
        /// Remote expression server metadata information
        /// 远程表达式服务端元数据信息
        /// </summary>
        private readonly ServerMetadata metadata;
        /// <summary>
        /// 远程表达式序列化状态
        /// </summary>
        internal RemoteExpressionSerializeStateEnum State;
        /// <summary>
        /// 表达式反序列化信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="metadata"></param>
        /// <param name="deserializer"></param>
        /// <param name="parameterTypes"></param>
        internal DelegateDeserializer(CommandServerSocket socket, ServerMetadata metadata, BinaryDeserializer deserializer, Type[] parameterTypes)
        {
            SerializeInfo = new SerializeInfo(deserializer);
            this.Socket = socket;
            this.metadata = metadata;
            if (SerializeInfo.ConstantParameterCount >= 0)
            {
                if (SerializeInfo.HashCode == 0)
                {
                    State = socket.GetRemoteExpressionFormatDeserialize().Format(ref SerializeInfo, deserializer, parameterTypes);
                }
                else State = RemoteExpressionSerializeStateEnum.Success;
            }
            else State = RemoteExpressionSerializeStateEnum.UnknownSerializeInfo;
        }
        /// <summary>
        /// 获取远程表达式集合
        /// </summary>
        /// <returns></returns>
        internal Dictionary<HashBuffer, CallDelegate>[] LockExpressionArray()
        {
            Dictionary<HashBuffer, CallDelegate>[] expressionArray = metadata.ExpressionArray;
            Monitor.Enter(expressionArray);
            return expressionArray;
        }
        /// <summary>
        /// 获取创建表达式委托实例
        /// </summary>
        /// <param name="expressionArray"></param>
        /// <param name="index"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallDelegate? GetExpression(Dictionary<HashBuffer, CallDelegate>[] expressionArray, int index, out Dictionary<HashBuffer, CallDelegate> expressions)
#else
        internal CallDelegate GetExpression(Dictionary<HashBuffer, CallDelegate>[] expressionArray, int index, out Dictionary<HashBuffer, CallDelegate> expressions)
#endif
        {
            expressions = expressionArray[index];
            if (expressions != null)
            {
                var expression = default(CallDelegate);
                if (expressions.TryGetValue(SerializeInfo.Key, out expression)) return expression;
            }
            else expressionArray[index] = expressions = DictionaryCreator<HashBuffer>.Create<CallDelegate>();
            return null;
        }
        /// <summary>
        /// 获取常量参数类型信息
        /// </summary>
        /// <returns></returns>
        internal unsafe ServerMethodParameter GetConstantParameterType()
        {
            ServerMethodParameter constantParameterType = metadata.GetConstantParameterType(ref SerializeInfo);
            Socket.GetRemoteExpressionFormatDeserialize().SetExpression((byte*)SerializeInfo.Key.Buffer.Data, constantParameterType.Fields);
            return constantParameterType;
        }
        /// <summary>
        /// 获取格式化远程表达式反序列化数据
        /// </summary>
        /// <returns></returns>
        internal unsafe FormatDeserialize GetFormatDeserialize()
        {
            FormatDeserialize formatDeserialize = Socket.GetRemoteExpressionFormatDeserialize();
            formatDeserialize.SetExpression((byte*)SerializeInfo.Key.Buffer.Data, EmptyArray<FieldInfo>.Array);
            return formatDeserialize;
        }

        /// <summary>
        /// 获取表达式反序列化信息
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static DelegateDeserializer Deserialize(BinaryDeserializer deserializer, Type[] parameterTypes, ref RemoteExpressionSerializeStateEnum state)
        {
            state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
            int stateValue;
            if (deserializer.Read(out stateValue))
            {
                if (stateValue == (byte)RemoteExpressionSerializeStateEnum.Success)
                {
                    var socket = deserializer.Context.castType<CommandServerSocket>();
                    if (socket != null)
                    {
                        var metadata = socket.Server.RemoteMetadata;
                        if (metadata != null)
                        {
                            DelegateDeserializer delegateDeserializer = new DelegateDeserializer(socket, metadata, deserializer, parameterTypes);
                            if (delegateDeserializer.State != RemoteExpressionSerializeStateEnum.Success) state = delegateDeserializer.State;
                            return delegateDeserializer;
                        }
                        state = RemoteExpressionSerializeStateEnum.NotFoundRemoteMetadata;
                    }
                    else state = RemoteExpressionSerializeStateEnum.NotFoundCommandServerSocket;
                }
                else state = (RemoteExpressionSerializeStateEnum)(byte)stateValue;
            }
            return default(DelegateDeserializer);
        }
    }
}
