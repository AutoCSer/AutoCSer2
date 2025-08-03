using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
        public readonly DiskBlockService Service;
        /// <summary>
        /// 读取数据请求缓存
        /// </summary>
        private readonly Dictionary<long, ReadRequest> readCache = AutoCSer.DictionaryCreator.CreateLong<ReadRequest>();
        /// <summary>
        /// 文件流起始写入位置
        /// </summary>
        internal readonly long StartIndex;
        /// <summary>
        /// 读取数据请求链表
        /// </summary>
        private LinkStack<ReadRequest> readQueue;
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
        /// 默认空磁盘块
        /// </summary>
        internal Block()
        {
            Service = new DiskBlockService(this);
            StartIndex = long.MaxValue;
            Position = long.MinValue;
        }
        /// <summary>
        /// 磁盘块
        /// </summary>
        /// <param name="service">磁盘块服务</param>
        /// <param name="startIndex">文件流起始写入位置</param>
        protected Block(DiskBlockService service, long startIndex)
        {
            this.Service = service;
            StartIndex = startIndex;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public abstract ValueTask DisposeAsync();
#else
        public abstract Task DisposeAsync();
#endif
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ServiceDisposeCallback()
        {
            Service.DisposeCallback();
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
            Service.FlushCallback(Position);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index">磁盘块索引信息</param>
        /// <param name="callback"></param>
        /// <param name="isCallback"></param>
        internal void Read(ref BlockIndex index, CommandServerCallback<ReadBuffer> callback, ref bool isCallback)
        {
            var cacheRequest = default(ReadRequest);
            var request = default(ReadRequest);
            try
            {
                if (readCache.TryGetValue(index.Index, out cacheRequest))
                {
                    cacheRequest.AppendCallback(index.Size, callback);
                    isCallback = false;
                }
                else
                {
                    request = new ReadRequest(this, ref index, callback);
                    isCallback = false;
                    readCache.Add(index.Index, request);
                    if (readQueue.IsPushHead(request))
                    {
                        request = null;
                        if (Interlocked.CompareExchange(ref isRead, 1, 0) == 0) read().AutoCSerNotWait();
                    }
                    else request = null;
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
                var context = default(object);
                do
                {
                    bool isQueue = false;
                    var request = readQueue.GetQueue().notNull();
                    do
                    {
                        try
                        {
                            do
                            {
                                if (!Service.IsDisposed)
                                {
                                    if (context == null) context = await getReadContext();
                                    isQueue = false;
                                    await read(request, context);
                                }
                                isQueue = true;
                                Service.CommandServerCallQueue.AddOnly(request.BlockCallback);
                                request = request.LinkNext;
                            }
                            while (request != null);
                            break;
                        }
                        catch (Exception exception)
                        {
                            if (!exceptionRepeat.IsRepeat(exception)) await AutoCSer.LogHelper.Exception(exception);
                        }
                        if(!isQueue) Service.CommandServerCallQueue.AddOnly(request.notNull().BlockCallback);
                        request = request.notNull().LinkNext;
                    }
                    while (request != null);
                }
                while (!readQueue.IsEmpty);
                Interlocked.Exchange(ref isRead, 0);
                if (context != null)
                {
                    try
                    {
                        await freeReadContext(context);
                    }
                    catch (Exception exception)
                    {
                        await AutoCSer.LogHelper.Exception(exception);
                    }
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
            finally { Service.ReadCallback(request); }
        }
        /// <summary>
        /// 删除磁盘块
        /// </summary>
        /// <returns>是否删除成功</returns>
        internal abstract Task<bool> Delete();
    }
}
