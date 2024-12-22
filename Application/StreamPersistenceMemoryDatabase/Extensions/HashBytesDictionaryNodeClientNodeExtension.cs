using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字典节点扩展操作
    /// </summary>
    public static class HashBytesDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.TryAdd(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.TryAdd(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.Set(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.Set(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryGetString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryGetBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryGetJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }

        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter GetRemoveString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> GetRemoveBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> GetRemoveJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
    }
}
