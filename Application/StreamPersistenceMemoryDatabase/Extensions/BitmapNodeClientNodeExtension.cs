using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 位图客户端节点扩展
    /// </summary>
    public static class BitmapNodeClientNodeExtension
    {
        /// <summary>
        /// 读取位状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>索引超出返回也返回 false</returns>
        public static async Task<ResponseResult<bool>> GetBool(this IBitmapNodeClientNode node, uint index)
        {
            return (await node.GetBit(index)).ToBool();
        }
        /// <summary>
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>清除操作之前的状态，索引超出返回也返回 false</returns>
        public static async Task<ResponseResult<bool>> GetBoolClearBit(this IBitmapNodeClientNode node, uint index)
        {
            return (await node.GetBitClearBit(index)).ToBool();
        }
        /// <summary>
        /// 状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>取反操作之前的状态，索引超出返回也返回 false</returns>
        public static async Task<ResponseResult<bool>> GetBoolInvertBit(this IBitmapNodeClientNode node, uint index)
        {
            return (await node.GetBitInvertBit(index)).ToBool();
        }
        /// <summary>
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>设置之前的状态，索引超出返回也返回 false</returns>
        public static async Task<ResponseResult<bool>> GetBoolSetBit(this IBitmapNodeClientNode node, uint index)
        {
            return (await node.GetBitSetBit(index)).ToBool();
        }
    }
}
