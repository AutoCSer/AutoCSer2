﻿using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public abstract class MethodParameter : AutoCSer.Threading.Link<MethodParameter>
    {
        /// <summary>
        /// 持久化输出流起始位置，用于记录当前输出数据长度
        /// </summary>
        internal const int PersistenceStartIndex = sizeof(int);
        /// <summary>
        /// 编码持久化输出流起始位置，用于记录编码后的数据长度
        /// </summary>
        internal const int EncodePersistenceStartIndex = PersistenceStartIndex + sizeof(int);

        /// <summary>
        /// 服务端节点
        /// </summary>
        internal readonly ServerNode Node;
        /// <summary>
        /// 持久化异常位置信息
        /// </summary>
        protected long persistenceCallbackExceptionPosition;
        /// <summary>
        /// 持久化回调是否成功
        /// </summary>
        internal bool IsPersistenceCallback;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        public MethodParameter(ServerNode node)
        {
            Node = node;
        }
        /// <summary>
        /// 复制调用方法与参数信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal MethodParameter Clone()
        {
            MethodParameter methodParameter = (MethodParameter)base.MemberwiseClone();
            methodParameter.LinkNext = null;
            return methodParameter;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns>下一个参数</returns>
#if NetStandard21
        internal abstract MethodParameter? PersistenceCallback();
#else
        internal abstract MethodParameter PersistenceCallback();
#endif
        ///// <summary>
        ///// 设置持久化回调错误
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal MethodParameter SetPersistenceCallbackException()
        //{
        //    if (!isPersistenceCallback) Node.SetPersistenceCallbackException();
        //    return LinkNext;
        //}
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract MethodParameter? PersistenceCallback(CallStateEnum state);
#else
        internal abstract MethodParameter PersistenceCallback(CallStateEnum state);
#endif
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal MethodParameter? PersistenceCallbackIgnoreException(CallStateEnum state)
#else
        internal MethodParameter PersistenceCallbackIgnoreException(CallStateEnum state)
#endif
        {
            try
            {
                return PersistenceCallback(state);
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            return LinkNext;
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <param name="persistenceCallbackExceptionPosition"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal MethodParameter? PersistenceSerialize(AutoCSer.BinarySerializer serializer, long persistenceCallbackExceptionPosition)
#else
        internal MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer, long persistenceCallbackExceptionPosition)
#endif
        {
            this.persistenceCallbackExceptionPosition = persistenceCallbackExceptionPosition;
            return PersistenceSerialize(serializer);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
#if NetStandard21
        internal abstract MethodParameter? PersistenceSerialize(AutoCSer.BinarySerializer serializer);
#else
        internal abstract MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer);
#endif
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
#if NetStandard21
        protected unsafe MethodParameter? persistenceSerialize(AutoCSer.BinarySerializer serializer, long methodIndex)
#else
        protected unsafe MethodParameter persistenceSerialize(AutoCSer.BinarySerializer serializer, long methodIndex)
#endif
        {
            UnmanagedStream stream = serializer.Stream;
            byte* data = stream.GetBeforeMove(sizeof(NodeIndex) + sizeof(long));
            if (data != null)
            {
                *(NodeIndex*)data = Node.Index;
                *(long*)(data + sizeof(NodeIndex)) = methodIndex;
                return LinkNext;
            }
            return this;
        }
        /// <summary>
        /// 设置自定义状态对象
        /// </summary>
        /// <param name="sessionObject">自定义状态对象</param>
        public virtual void SetBeforePersistenceCustomSessionObject(object sessionObject) { }
        /// <summary>
        /// 获取自定义状态对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public virtual object? GetBeforePersistenceCustomSessionObject() { return null; }
#else
        public virtual object GetBeforePersistenceCustomSessionObject() { return null; }
#endif
        ///// <summary>
        ///// 获取服务端节点
        ///// </summary>
        ///// <param name="methodParameter"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static ServerNode GetNode(MethodParameter methodParameter)
        //{
        //    return methodParameter.Node;
        //}
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetNodeTarget<T>(MethodParameter methodParameter)
        {
            return ((ServerNode<T>)methodParameter.Node).Target;
        }
    }
}
