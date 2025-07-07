using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 256-base fragment HashBytes dictionary client node expansion operation
    /// 256 基分片 HashBytes 字典客户端节点扩展操作
    /// </summary>
    public static class HashBytesFragmentDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.TryAdd(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.TryAdd(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.Set(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key, T value)
#endif
        {
            return node.Set(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryGetString(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryGetBinaryDeserialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryGetJsonDeserialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }

        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter GetRemoveString(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> GetRemoveBinaryDeserialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> GetRemoveJsonDeserialize<T>(this IHashBytesFragmentDictionaryNodeClientNode node, ServerByteArray key)
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
    }
}
