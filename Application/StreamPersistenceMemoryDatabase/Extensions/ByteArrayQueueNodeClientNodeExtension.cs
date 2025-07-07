using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Queue client node (first-in-first-out) expansion operation
    /// 队列客户端节点（先进先出）扩展操作
    /// </summary>
    public static class ByteArrayQueueNodeClientNodeExtension
    {
        /// <summary>
        /// Add the data to the queue
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
        /// Add the data to the queue
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
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryDequeueString(this IByteArrayQueueNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryDequeueBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryDequeueJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryDequeueResponseParameter(responseParameter));
            return responseParameter;
        }

        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryPeekString(this IByteArrayQueueNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryPeekBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryPeekJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
    }
}
