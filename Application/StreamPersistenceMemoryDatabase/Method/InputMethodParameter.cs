using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public abstract class InputMethodParameter : MethodParameter
    {
        /// <summary>
        /// 持久化之前检查参数的调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal CallInputOutputMethodParameter? BeforePersistenceMethodParameter;
#else
        internal CallInputOutputMethodParameter BeforePersistenceMethodParameter;
#endif
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        public InputMethodParameter(ServerNode node) : base(node) { }
        ///// <summary>
        ///// 清除信息
        ///// </summary>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //protected void clearClone()
        //{
        //    LinkNext = null;
        //    BeforePersistenceMethodParameter = null;
        //    persistenceCallbackExceptionPosition = 0;
        //    IsPersistenceCallback = false;
        //}
//        /// <summary>
//        /// 复制调用方法与参数信息
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="methodIndex"></param>
//        /// <returns></returns>
//#if NetStandard21
//        internal abstract InputMethodParameter? Clone(NodeIndex index, int methodIndex);
//#else
//        internal abstract InputMethodParameter Clone(NodeIndex index, int methodIndex);
//#endif
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal abstract void Deserialize(AutoCSer.BinaryDeserializer deserializer);
        /// <summary>
        /// 输入参数反序列化（初始化加载持久化数据）
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal abstract bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer);
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe MethodParameter? PersistenceSerialize<T>(AutoCSer.BinarySerializer serializer, Method method, ref T parameter)
#else
        internal unsafe MethodParameter PersistenceSerialize<T>(AutoCSer.BinarySerializer serializer, Method method, ref T parameter)
#endif
            where T : struct
        {
            UnmanagedStream stream = serializer.Stream;
            int index = stream.GetMoveSize(sizeof(NodeIndex) + sizeof(int));
            if (index != 0)
            {
                if (method.IsSimpleDeserializeParamter) serializer.SimpleSerialize(ref parameter);
                else serializer.InternalIndependentSerializeNotNull(ref parameter);
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
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void Deserialize<T>(AutoCSer.BinaryDeserializer deserializer, Method method, ref T parameter)
            where T : struct
        {
            if (method.IsSimpleDeserializeParamter) deserializer.SimpleDeserialize(ref parameter);
            else deserializer.InternalIndependentDeserializeNotReference(ref parameter); 
        }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal unsafe bool Deserialize<T>(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer, Method method, ref T parameter)
            where T : struct
        {
            if (method.IsSimpleDeserializeParamter) return deserializer.SimpleDeserialize(ref buffer, ref parameter);
            return deserializer.InternalIndependentDeserializeNotReference(ref buffer, ref parameter);
        }
    }
}
