using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 256 基分片 HashString 字典客户端节点扩展
    /// </summary>
    public static class HashStringFragmentDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public static async Task<ResponseResult<bool>> TryAddJson<T>(this IHashStringFragmentDictionaryNodeClientNode<string> node, string key, T value)
        {
            if (key != null) return await node.TryAdd(key, AutoCSer.JsonSerializer.Serialize(value));
            return false;
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public static async Task<ResponseResult<bool>> SetJson<T>(this IHashStringFragmentDictionaryNodeClientNode<string> node, string key, T value)
        {
            if (key != null) return await node.Set(key, AutoCSer.JsonSerializer.Serialize(value));
            return false;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<ResponseResult<T>> TryGetFromJson<T>(this IHashStringFragmentDictionaryNodeClientNode<string> node, string key)
        {
            if (key != null) return (await node.TryGetValue(key)).FromJson<T>();
            return CallStateEnum.NullKey;
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<ResponseResult<T>> GetRemoveFromJson<T>(this IHashStringFragmentDictionaryNodeClientNode<string> node, string key)
        {
            if (key != null) return (await node.GetRemove(key)).FromJson<T>();
            return CallStateEnum.NullKey;
        }
    }
}
