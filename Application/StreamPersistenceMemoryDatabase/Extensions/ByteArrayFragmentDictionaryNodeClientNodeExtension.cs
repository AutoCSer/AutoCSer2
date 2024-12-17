using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 256 基分片字典 节点
    /// </summary>
    public static class ByteArrayFragmentDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult<bool>> TryAddBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static Task<ResponseResult<bool>> TryAddBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
              where KT : IEquatable<KT>
        {
            return node.TryAdd(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult<bool>> TryAddJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static Task<ResponseResult<bool>> TryAddJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
             where KT : IEquatable<KT>
        {
            return node.TryAdd(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult<bool>> SetBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static Task<ResponseResult<bool>> SetBinarySerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
             where KT : IEquatable<KT>
        {
            return node.Set(key, ServerByteArray.BinarySerialize(value));
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ResponseResult<bool>> SetJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T? value)
#else
        public static Task<ResponseResult<bool>> SetJsonSerialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key, T value)
#endif
               where KT : IEquatable<KT>
        {
            return node.Set(key, ServerByteArray.JsonSerialize(value));
        }

        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryGetString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryGetString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
               where KT : IEquatable<KT>
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryGetBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryGetBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
             where KT : IEquatable<KT>
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryGetJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryGetJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
              where KT : IEquatable<KT>
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }

        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> GetRemoveString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<string>>> GetRemoveString<KT>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
              where KT : IEquatable<KT>
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> GetRemoveBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> GetRemoveBinaryDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
             where KT : IEquatable<KT>
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> GetRemoveJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> GetRemoveJsonDeserialize<KT, T>(this IByteArrayFragmentDictionaryNodeClientNode<KT> node, KT key)
#endif
             where KT : IEquatable<KT>
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
    }
}
