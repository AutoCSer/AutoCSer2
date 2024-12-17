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
        public static Task<ResponseResult> EnqueueBinarySerialize<T>(this IByteArrayQueueNodeClientNode node, T? value)
#else
        public static Task<ResponseResult> EnqueueBinarySerialize<T>(this IByteArrayQueueNodeClientNode node, T value)
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
        public static Task<ResponseResult> EnqueueJsonSerialize<T>(this IByteArrayQueueNodeClientNode node, T? value)
#else
        public static Task<ResponseResult> EnqueueJsonSerialize<T>(this IByteArrayQueueNodeClientNode node, T value)
#endif
        {
            return node.Enqueue(ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryDequeueString(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryDequeueString(this IByteArrayQueueNodeClientNode node)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryDequeueResponseParameter(responseParameter));
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryDequeueBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryDequeueBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryDequeueResponseParameter(responseParameter));
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryDequeueJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryDequeueJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryDequeueResponseParameter(responseParameter));
        }

        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryPeekString(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryPeekString(this IByteArrayQueueNodeClientNode node)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPeekBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPeekBinaryDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPeekJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPeekJsonDeserialize<T>(this IByteArrayQueueNodeClientNode node)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
    }
}
