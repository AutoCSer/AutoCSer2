using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 256-base fragment dictionary client node expansion operation
    /// 256 基分片字典 客户端节点扩展操作
    /// </summary>
    public static class ByteArrayFragmentDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
              where KT : IEquatable<KT>
        {
            return node.TryAdd(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static ResponseParameterAwaiter<bool> TryAddJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
             where KT : IEquatable<KT>
        {
            return node.TryAdd(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
             where KT : IEquatable<KT>
        {
            return node.Set(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static ResponseParameterAwaiter<bool> SetJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
               where KT : IEquatable<KT>
        {
            return node.Set(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter TryGetString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
               where KT : IEquatable<KT>
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> TryGetBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
             where KT : IEquatable<KT>
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> TryGetJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
              where KT : IEquatable<KT>
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.TryGetResponseParameter(responseParameter, key));
            return responseParameter;
        }

        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StringResponseParameterAwaiter GetRemoveString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
              where KT : IEquatable<KT>
        {
            StringResponseParameterAwaiter responseParameter = new StringResponseParameterAwaiter();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeResponseParameterValueResultAwaiter<T> GetRemoveBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
             where KT : IEquatable<KT>
        {
            BinarySerializeResponseParameterValueResultAwaiter<T> responseParameter = new BinarySerializeResponseParameterValueResultAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static JsonResponseParameterAwaiter<T> GetRemoveJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
             where KT : IEquatable<KT>
        {
            JsonResponseParameterAwaiter<T> responseParameter = new JsonResponseParameterAwaiter<T>();
            responseParameter.Set(node.GetRemoveResponseParameter(responseParameter, key));
            return responseParameter;
        }
    }
}
