using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 数组链表，用于替代固定数组环 RingPool{T}（数组环并发性能远不如 Link，所以该结构仅支持一读一写）
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed class ArrayQueue<T> : IDisposable
    {
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad0;
        /// <summary>
        /// 数组大小
        /// </summary>
        public readonly int ArraySize;
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed != 0; }
        }
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad1;
        /// <summary>
        /// 等待读取队列
        /// </summary>
        private LinkStack<ArrayQueueNode<T>> readQueue;
        /// <summary>
        /// 读取队列访问锁
        /// </summary>
        private OnceAutoWaitHandle readLock;
        /// <summary>
        /// 空闲节点链表
        /// </summary>
        private LinkStack<ArrayQueueNode<T>> freeLink;
        /// <summary>
        /// 读取并发检测（非严格检查）
        /// </summary>
        private int isRead;
        /// <summary>
        /// 保留字段
        /// </summary>
        private int readReserve;
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad2;
        /// <summary>
        /// 当前写入节点
        /// </summary>
        internal volatile ArrayQueueNode<T> WriteNode;
        /// <summary>
        /// 写入并发检测（非严格检查）
        /// </summary>
        private int isWrite;
        /// <summary>
        /// 保留字段
        /// </summary>
        private int writeReserve;
        /// <summary>
        /// The CPU cache is filled with data blocks
        /// </summary>
        private readonly CpuCachePad pad3;
        /// <summary>
        /// 数组链表
        /// </summary>
        /// <param name="arraySize">数组大小，默认为 4096</param>
        public ArrayQueue(int arraySize = 1 << 12)
        {
            ArraySize = Math.Max(arraySize, 1);
            WriteNode = new ArrayQueueNode<T>(this);
            readLock.Set(new object());
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            isDisposed = 1;
            if (readQueue.IsEmpty) readLock.Set();
        }
        /// <summary>
        /// 添加数据（仅支持单线程操作）
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            if (isWrite == 0)
            {//写入并发检测（非严格检查）
                isWrite = 1;
                while (!WriteNode.Push(value)) ;
                isWrite = 0;
                return;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 添加数据完毕检查读取等待状态
        /// </summary>
        internal void OnPush()
        {
            if(readLock.IsWait != 0 && readQueue.TryPushHead(WriteNode))
            {
                WriteNode.Index = ArraySize;
                readLock.Set();
                getWriteNode();
            }
        }
        /// <summary>
        /// 获取写入数组
        /// </summary>
        private void getWriteNode()
        {
            var node = freeLink.Pop();
            if (node != null)
            {
                WriteNode = node.Reset();
                return;
            }
            try
            {
                WriteNode = new ArrayQueueNode<T>(this);
                return;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            while (isDisposed == 0)
            {
                try
                {
                    System.Threading.Thread.Sleep(0);
                    WriteNode = new ArrayQueueNode<T>(this);
                    return;
                }
                catch { }
            }
        }
        /// <summary>
        /// 写入数组添加到读取队列
        /// </summary>
        internal void OnFullPush()
        {
            if (readQueue.IsPushHead(WriteNode)) readLock.Set();
            getWriteNode();
        }
        /// <summary>
        /// 获取链表首节点
        /// </summary>
        /// <returns>返回 null 表示已释放链表</returns>
#if NetStandard21
        public ArrayQueueNode<T>? Get()
#else
        public ArrayQueueNode<T> Get()
#endif
        {
            if (isRead == 0)
            {//读取并发检测（非严格检查）
                isRead = 1;
                if (!readQueue.IsEmpty || !WriteNode.TryGet())
                {
                    if (isDisposed == 0) readLock.Wait();
                    else
                    {
                        isRead = 0;
                        return null;
                    }
                }
                var node = readQueue.GetQueue();
                isRead = 0;
                return node;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 尝试获取链表首节点
        /// </summary>
        /// <returns></returns>
        internal bool TryGet()
        {
            if (readQueue.TryPushHead(WriteNode))
            {
                WriteNode = freeLink.Pop()?.Reset() ?? new ArrayQueueNode<T>(this);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free(ArrayQueueNode<T> node)
        {
            freeLink.Push(node);
        }
    }
}
