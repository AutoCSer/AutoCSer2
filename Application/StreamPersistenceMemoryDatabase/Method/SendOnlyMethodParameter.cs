﻿using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public abstract class SendOnlyMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        internal readonly SendOnlyMethod Method;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public SendOnlyMethodParameter(ServerNode node, SendOnlyMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        internal void SendOnly()
        {
            if (Method.IsClientCall)
            {
                StreamPersistenceMemoryDatabaseServiceBase service = Node.NodeCreator.Service;
                if (Method.IsPersistence)
                {
                    if (Node.CallState != CallStateEnum.Success) return;
                    if (Node.IsPersistence && !service.IsMaster) return;
                    if (Method.BeforePersistenceMethodIndex >= 0)
                    {
                        CallInputOutputMethod beforePersistenceMethod = (CallInputOutputMethod)Node.NodeCreator.Methods[Method.BeforePersistenceMethodIndex].notNull();
                        BeforePersistenceMethodParameter = CreateBeforePersistenceMethodParameter(beforePersistenceMethod);
                        service.CurrentMethodParameter = BeforePersistenceMethodParameter;
                        if (!beforePersistenceMethod.CallBeforePersistence(BeforePersistenceMethodParameter)) return;
                    }
                    if (Node.IsPersistence) service.PushPersistenceMethodParameter(this);
                    else service.CommandServerCallQueue.AppendWriteOnly(new MethodParameterPersistenceCallback(this));
                    return;
                }
                service.SetCurrentMethodParameter(this);
                Method.SendOnly(this);
            }
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback()
#else
        internal override MethodParameter PersistenceCallback()
#endif
        {
            if (Node.IsPersistence)
            {
                try
                {
                    Node.IsPersistenceCallbackChanged = false;
                    Method.SendOnly(this);
                    IsPersistenceCallback = true;
                }
                finally
                {
                    StreamPersistenceMemoryDatabaseServiceBase service = Node.NodeCreator.Service;
                    var rebuilder = service.Rebuilder;
                    if (IsPersistenceCallback)
                    {
                        if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
                    }
                    else
                    {
                        bool isPersistenceCallbackException = true;
                        try
                        {
                            if (Method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
                            {
                                service.WritePersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition);
                                rebuilder = null;
                                isPersistenceCallbackException = false;
                            }
                        }
                        finally
                        {
                            if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                            if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
                        }
                    }
                }
            }
            else Method.SendOnly(this);
            return LinkNext;
        }
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback(CallStateEnum state)
#else
        internal override MethodParameter PersistenceCallback(CallStateEnum state)
#endif
        {
            return LinkNext;
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal abstract CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod);
    }
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SendOnlyMethodParameter<T> : SendOnlyMethodParameter
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        internal T Parameter;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public SendOnlyMethodParameter(ServerNode node, SendOnlyMethod method) : base(node, method) { }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public SendOnlyMethodParameter(ServerNode node, int methodIndex, ref T parameter) : base(node, (SendOnlyMethod)node.NodeCreator.Methods[methodIndex].notNull()) 
        {
            this.Parameter = parameter;
        }
//        /// <summary>
//        /// 复制调用方法与参数信息
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="methodIndex"></param>
//        /// <returns></returns>
//#if NetStandard21
//        internal override InputMethodParameter? Clone(NodeIndex index, int methodIndex)
//#else
//        internal override InputMethodParameter Clone(NodeIndex index, int methodIndex)
//#endif
//        {
//            if (Method.Index == methodIndex && index.Equals(Node.Index))
//            {
//                SendOnlyMethodParameter<T> methodParameter = (SendOnlyMethodParameter<T>)base.MemberwiseClone();
//                methodParameter.clearClone();
//                methodParameter.Parameter = default(T);
//                return methodParameter;
//            }
//            return null;
//        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            Deserialize(deserializer, Method, ref Parameter);
        }
        /// <summary>
        /// 输入参数反序列化（初始化加载持久化数据）
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer)
        {
            return Deserialize(deserializer, ref buffer, Method, ref Parameter);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#else
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#endif
        {
            return PersistenceSerialize(serializer, Method, ref Parameter);
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal override CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod)
        {
            return new BeforePersistenceMethodParameter<T>(Node, beforePersistenceMethod, Parameter);
        }
        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetParameter(SendOnlyMethodParameter<T> parameter)
        {
            return parameter.Parameter;
        }
    }
}
