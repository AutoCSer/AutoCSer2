using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块
    /// </summary>
    public abstract class Block : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 磁盘块服务
        /// </summary>
        protected readonly DiskBlockService service;
        /// <summary>
        /// 读取数据请求缓存
        /// </summary>
        private readonly Dictionary<long, ReadRequest> readCache = AutoCSer.Extensions.DictionaryCreator.CreateLong<ReadRequest>();
        /// <summary>
        /// 文件流起始写入位置
        /// </summary>
        internal readonly long StartIndex;
        /// <summary>
        /// 读取数据请求链表
        /// </summary>
        private ReadRequest.YieldQueue readQueue;
        /// <summary>
        /// 磁盘块当前写入位置
        /// </summary>
        internal long Position;
        /// <summary>
        /// 是否已经启动读取操作
        /// </summary>
        private int isRead;
        /// <summary>
        /// 当前是否存在读取操作
        /// </summary>
        internal bool IsRead
        {
            get
            {
                return isRead != 0 || !readQueue.IsEmpty;
            }
        }
        /// <summary>
        /// 磁盘块
        /// </summary>
        /// <param name="service">磁盘块服务</param>
        /// <param name="startIndex">文件流起始写入位置</param>
        protected Block(DiskBlockService service, long startIndex)
        {
            this.service = service;
            StartIndex = startIndex;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public abstract ValueTask DisposeAsync();
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ServiceDisposeCallback()
        {
            service.DisposeCallback();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns>磁盘块当前写入位置</returns>
        internal abstract Task<long> Write(WriteRequest request);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns>磁盘块当前写入位置</returns>
        public abstract Task<long> Flush();
        /// <summary>
        /// 写入数据回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FlushCallback()
        {
            service.FlushCallback(Position);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index">磁盘块索引信息</param>
        /// <param name="callback"></param>
        internal void Read(ref BlockIndex index, ref CommandServerCallback<ReadBuffer> callback)
        {
            ReadRequest cacheRequest;
            ReadRequest request = null;
            try
            {
                if (readCache.TryGetValue(index.Index, out cacheRequest)) cacheRequest.AppendCallback(index.Size, ref callback);
                else
                {
                    readCache.Add(index.Index, request = new ReadRequest(this, ref index, ref callback));
                    if (readQueue.IsPushHead(ref request) && Interlocked.CompareExchange(ref isRead, 1, 0) == 0) read().NotWait();
                }
            }
            finally { request?.CancelCallback(new ReadBuffer(ReadBufferStateEnum.Unknown)); }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private async Task read()
        {
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            do
            {
                object context = null;
                do
                {
                    bool isQueue = false;
                    ReadRequest request = readQueue.GetClear();
                    do
                    {
                        try
                        {
                            do
                            {
                                if (!service.IsDisposed)
                                {
                                    if (context == null) context = await getReadContext();
                                    isQueue = false;
                                    await read(request, context);
                                }
                                isQueue = true;
                                service.CommandServerCallQueue.AddOnly(request.BlockCallback);
                                request = request.LinkNext;
                            }
                            while (request != null);
                            break;
                        }
                        catch (Exception exception)
                        {
                            if (!exceptionRepeat.IsRepeat(exception)) await AutoCSer.LogHelper.Exception(exception);
                        }
                        if(!isQueue) service.CommandServerCallQueue.AddOnly(request.BlockCallback);
                        request = request.LinkNext;
                    }
                    while (request != null);
                }
                while (!readQueue.IsEmpty);
                Interlocked.Exchange(ref isRead, 0);
                try
                {
                    await freeReadContext(context);
                }
                catch (Exception exception)
                {
                    await AutoCSer.LogHelper.Exception(exception);
                }
            }
            while (!readQueue.IsEmpty && Interlocked.CompareExchange(ref isRead, 1, 0) == 0);
        }
        /// <summary>
        /// 获取读取数据上下文
        /// </summary>
        /// <returns>不允许返回 null</returns>
        protected virtual Task<object> getReadContext() { return Task.FromResult((object)this); }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context">获取读取数据上下文</param>
        /// <returns></returns>
        protected abstract Task read(ReadRequest request, object context);
        /// <summary>
        /// 释放读取数据上下文
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task freeReadContext(object context) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 读取数据请求回调操作
        /// </summary>
        /// <param name="request"></param>
        internal void ReadCallback(ReadRequest request)
        {
            try
            {
                readCache.Remove(request.Index);
                request.Callback();
            }
            finally { service.ReadCallback(request); }
        }
        /// <summary>
        /// 删除磁盘块
        /// </summary>
        /// <returns>是否删除成功</returns>
        internal abstract Task<bool> Delete();
    }
}
