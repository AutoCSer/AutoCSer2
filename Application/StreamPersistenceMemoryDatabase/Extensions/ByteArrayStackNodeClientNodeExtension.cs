using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 栈节点（后进先出）扩展操作
    /// </summary>
    public static class ByteArrayStackNodeClientNodeExtension
    {
        /// <summary>
        /// 将数据添加到栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult> PushBinarySerialize<T>(this IByteArrayStackNodeClientNode node, T? value)
#else
        public static Task<ResponseResult> PushBinarySerialize<T>(this IByteArrayStackNodeClientNode node, T value)
#endif
        {
            return node.Push(ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 将数据添加到栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult> PushJsonSerialize<T>(this IByteArrayStackNodeClientNode node, T? value)
#else
        public static Task<ResponseResult> PushJsonSerialize<T>(this IByteArrayStackNodeClientNode node, T value)
#endif
        {
            return node.Push(ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryPopString(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryPopString(this IByteArrayStackNodeClientNode node)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryPopResponseParameter(responseParameter));
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPopBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPopBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryPopResponseParameter(responseParameter));
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPopJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPopJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryPopResponseParameter(responseParameter));
        }

        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryPeekString(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryPeekString(this IByteArrayStackNodeClientNode node)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPeekBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPeekBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryPeekJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryPeekJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryPeekResponseParameter(responseParameter));
        }
    }
}
