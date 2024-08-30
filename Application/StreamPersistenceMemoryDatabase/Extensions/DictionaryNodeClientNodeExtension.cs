using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字典客户端节点扩展
    /// </summary>
    public static class DictionaryNodeClientNodeExtension
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
        public static async Task<ResponseResult<bool>> TryAddJson<KT, T>(this IDictionaryNodeClientNode<KT, string> node, KT key, T value)
        {
            if (key != null) return await node.TryAdd(key, AutoCSer.JsonSerializer.Serialize(value));
            return false;
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public static async Task<ResponseResult<bool>> SetJson<KT, T>(this IDictionaryNodeClientNode<KT, string> node, KT key, T value)
        {
            if (key != null) return await node.Set(key, AutoCSer.JsonSerializer.Serialize(value));
            return false;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<ResponseResult<T>> TryGetFromJson<KT, T>(this IDictionaryNodeClientNode<KT, string> node, KT key)
        {
            if (key != null) return (await node.TryGetValue(key)).FromJson<T>();
            return CallStateEnum.NullKey;
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<ResponseResult<T>> GetRemoveFromJson<KT, T>(this IDictionaryNodeClientNode<KT, string> node, KT key)
        {
            if (key != null) return (await node.GetRemove(key)).FromJson<T>();
            return CallStateEnum.NullKey;
        }
    }
}
