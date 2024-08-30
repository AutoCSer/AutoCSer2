using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 写入数据请求信息
    /// </summary>
    public sealed class WriteRequest : AutoCSer.Threading.Link<WriteRequest>
    {
        /// <summary>
        /// 写入位置回调委托
        /// </summary>
        private readonly CommandServerCallback<BlockIndex> callback;
        /// <summary>
        /// 写入位置回调委托
        /// </summary>
        private CommandServerCallback<BlockIndex>.Link callbacks;
        /// <summary>
        /// 写入数据位置
        /// </summary>
        internal BlockIndex Index;
        /// <summary>
        /// 字节数组缓冲区，前面 4 个字节保留用于记录字节数
        /// </summary>
        internal ByteArrayBuffer Buffer;
        /// <summary>
        /// 哈希值
        /// </summary>
        private ulong hashCode;
        /// <summary>
        /// 写操作类型
        /// </summary>
        internal readonly WriteOperationTypeEnum OperationType;
        /// <summary>
        /// 是否已经触发回调
        /// </summary>
        private bool isCallback;

        /// <summary>
        /// 写入数据请求信息
        /// </summary>
        /// <param name="index">写入数据位置</param>
        /// <param name="hashBytes">写入数据缓冲区</param>
        /// <param name="callback">写入位置回调委托</param>
        internal WriteRequest(ref BlockIndex index, ref HashBytes hashBytes, ref CommandServerCallback<BlockIndex> callback)
        {
            ByteArrayPool.GetBuffer(ref Buffer, hashBytes.SubArray.Length + sizeof(int));
            Buffer.CopyFromSetSize(ref hashBytes.SubArray);
            this.callback = callback;
            this.Index = index;
            hashCode = hashBytes.HashCode;
            callback = null;
            OperationType = WriteOperationTypeEnum.Append;
        }
        /// <summary>
        /// 写入数据请求信息
        /// </summary>
        /// <param name="operationType">写操作类型</param>
        /// <param name="callback">写入位置回调委托</param>
        internal WriteRequest(WriteOperationTypeEnum operationType, ref CommandServerCallback<BlockIndex> callback)
        {
            this.callback = callback;
            OperationType = operationType;
            callback = null;
        }
        /// <summary>
        /// 获取哈希字节数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal HashBytes GetHashBytes()
        {
            return new HashBytes(Buffer.GetSeekSubArray(sizeof(int)), hashCode);
        }
        /// <summary>
        /// 添加写入位置回调委托
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendCallback(ref CommandServerCallback<BlockIndex> callback)
        {
            callbacks.PushHead(callback);
            callback = null;
        }
        /// <summary>
        /// 添加写入位置回调委托
        /// </summary>
        /// <param name="request"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendCallback(ref WriteRequest request)
        {
            callbacks.PushHead(request.callback);
            request.Buffer.Free();
            request = null;
        }
        /// <summary>
        /// 错误取消写入操作
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelCallback(ref BlockIndex index)
        {
            Buffer.Free();
            callback.Callback(index);
        }
        /// <summary>
        /// 释放缓冲区并触发写入位置回调
        /// </summary>
        /// <returns></returns>
        internal WriteRequest FreeCallback()
        {
            if (!isCallback)
            {
                isCallback = true;
                Buffer.Free();
                callback.Callback(Index);
                if (OperationType == WriteOperationTypeEnum.Append) callbacks.Callback(Index);
            }
            return LinkNext;
        }
    }
}
