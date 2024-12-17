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
        public static Task<ResponseResult<bool>> TryAddBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static Task<ResponseResult<bool>> TryAddBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
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
        public static Task<ResponseResult<bool>> TryAddJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static Task<ResponseResult<bool>> TryAddJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
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
        public static Task<ResponseResult<bool>> SetBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static Task<ResponseResult<bool>> SetBinarySerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
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
        public static Task<ResponseResult<bool>> SetJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T? value)
#else
        public static Task<ResponseResult<bool>> SetJsonSerialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key, T value)
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
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> TryGetString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<string>>> TryGetString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryGetBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryGetBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> TryGetJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> TryGetJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.TryGetResponseParameter(responseParameter, key));
        }

        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<string?>>> GetRemoveString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<string>>> GetRemoveString(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            StringResponseParameter responseParameter = new StringResponseParameter();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> GetRemoveBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> GetRemoveBinaryDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            BinarySerializeResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ResponseResult<ValueResult<T?>>> GetRemoveJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#else
        public static async Task<ResponseResult<ValueResult<T>>> GetRemoveJsonDeserialize<T>(this IHashBytesDictionaryNodeClientNode node, ServerByteArray key)
#endif
        {
            JsonResponseParameter<T> responseParameter = new JsonResponseParameter<T>();
            return responseParameter.Get(await node.GetRemoveResponseParameter(responseParameter, key));
        }
    }
}
