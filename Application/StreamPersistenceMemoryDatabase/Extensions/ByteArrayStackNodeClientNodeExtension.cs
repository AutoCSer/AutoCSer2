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
        public static ResponseResultAwaiter PushBinarySerialize<T>(this IByteArrayStackNodeClientNode node, T? value)
#else
        public static ResponseParameterAwaiter PushBinarySerialize<T>(this IByteArrayStackNodeClientNode node, T value)
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
        public static ResponseResultAwaiter PushJsonSerialize<T>(this IByteArrayStackNodeClientNode node, T? value)
#else
        public static ResponseParameterAwaiter PushJsonSerialize<T>(this IByteArrayStackNodeClientNode node, T value)
#endif
        {
            return node.Push(ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryPopString(this IByteArrayStackNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryPopResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryPopBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryPopResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryPopJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryPopResponseParameter(responseParameter));
            return responseParameter;
        }

        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryPeekString(this IByteArrayStackNodeClientNode node)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryPeekBinaryDeserialize<T>(this IByteArrayStackNodeClientNode node)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <param name="node"></param>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryPeekJsonDeserialize<T>(this IByteArrayStackNodeClientNode node)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryPeekResponseParameter(responseParameter));
            return responseParameter;
        }
    }
}
