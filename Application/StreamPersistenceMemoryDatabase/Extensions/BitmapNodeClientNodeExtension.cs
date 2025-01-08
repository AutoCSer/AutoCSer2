using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BitmapNodeResponseAwaiter GetBool(this IBitmapNodeClientNode node, uint index)
        {
            return node.GetBit(index);
        }
        /// <summary>
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>清除操作之前的状态，索引超出返回也返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BitmapNodeResponseAwaiter GetBoolClearBit(this IBitmapNodeClientNode node, uint index)
        {
            return node.GetBitClearBit(index);
        }
        /// <summary>
        /// 状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>取反操作之前的状态，索引超出返回也返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BitmapNodeResponseAwaiter GetBoolInvertBit(this IBitmapNodeClientNode node, uint index)
        {
            return node.GetBitInvertBit(index);
        }
        /// <summary>
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">位索引</param>
        /// <returns>设置之前的状态，索引超出返回也返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BitmapNodeResponseAwaiter GetBoolSetBit(this IBitmapNodeClientNode node, uint index)
        {
            return node.GetBitSetBit(index);
        }

    }
}
