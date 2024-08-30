using AutoCSer.Memory;
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
        /// 压缩持久化输出流起始位置，用于记录压缩后的数据长度
        /// </summary>
        internal const int CompressPersistenceStartIndex = PersistenceStartIndex + sizeof(int);

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
        internal abstract MethodParameter PersistenceCallback();
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
        internal abstract MethodParameter PersistenceCallback(CallStateEnum state);
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal MethodParameter PersistenceCallbackIgnoreException(CallStateEnum state)
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
        internal MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer, long persistenceCallbackExceptionPosition)
        {
            this.persistenceCallbackExceptionPosition = persistenceCallbackExceptionPosition;
            return PersistenceSerialize(serializer);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        internal abstract MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer);
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        protected unsafe MethodParameter persistenceSerialize(AutoCSer.BinarySerializer serializer, long methodIndex)
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
        /// 持久化序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal unsafe MethodParameter PersistenceSerialize<T>(AutoCSer.BinarySerializer serializer, Method method, ref T parameter)
            where T : struct
        {
            UnmanagedStream stream = serializer.Stream;
            int index = stream.GetMoveSize(sizeof(NodeIndex) + sizeof(int));
            if (index != 0)
            {
                if (method.IsSimpleDeserializeParamter) serializer.SimpleSerialize(ref parameter);
                else serializer.InternalIndependentSerializeNotReference(ref parameter);
                if (!stream.IsResizeError)
                {
                    byte* data = stream.Data.Pointer.Byte + (index - (sizeof(NodeIndex) + sizeof(int)));
                    *(NodeIndex*)data = Node.Index;
                    *(int*)(data + sizeof(NodeIndex)) = method.Index;
                    return LinkNext;
                }
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
        public virtual object GetBeforePersistenceCustomSessionObject() { return null; }
        /// <summary>
        /// 获取服务端节点
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ServerNode GetNode(MethodParameter methodParameter)
        {
            return methodParameter.Node;
        }
    }
}
