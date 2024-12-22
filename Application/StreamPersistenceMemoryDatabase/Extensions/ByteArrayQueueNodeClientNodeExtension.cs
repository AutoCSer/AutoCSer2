using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 队列节点（先进先出）扩展操作
    /// </summary>
    public static class ByteArrayQueueNodeClientNodeExtension
    {
        /// <summary>
        /// 将数据添加到队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseResultAwaiter EnqueueBinarySerialize<T>(this IByteArrayQueueNodeClientNode node, T? value)
#else
        public static ResponseParameterAwaiter EnqueueBinarySerialize<T>(this IByteArrayQueueNodeClientNode node, T value)
#endif
        {
            return node.Enqueue(ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 将数据添加到队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseResultAwaiter EnqueueJsonSerialize<T>(this IByteArrayQueueNodeClientNode node, T? value)
#else
        public static ResponseParameterAwaiter EnqueueJsonSerialize<T>(this IByteArrayQueueNodeClientNode node, T value)
#endif
        {
            return node.Enqueue(ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryDequeueString(this IByteArrayQueueNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryDequeueBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryDequeueJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }

        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryPeekString(this IByteArrayQueueNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryPeekBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryPeekJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
    }
}
