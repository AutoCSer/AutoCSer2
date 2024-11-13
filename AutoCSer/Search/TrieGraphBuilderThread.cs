using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图创建器线程
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    internal sealed class TrieGraphBuilderThread<T> : TrieGraphBuilder<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// 建图线程等待
        /// </summary>
        private AutoCSer.Threading.OnceAutoWaitHandle threadWait;
        /// <summary>
        /// 建图线程任务完成计数
        /// </summary>
        private readonly AutoCSer.Threading.AutoWaitCount waitCount;
        /// <summary>
        /// 建图线程异常
        /// </summary>
#if NetStandard21
        internal Exception? Exception;
#else
        internal Exception Exception;
#endif
        /// <summary>
        /// Trie 图创建器
        /// </summary>
        /// <param name="boot">根节点</param>
        /// <param name="waitCount">建图线程任务完成计数</param>
        internal TrieGraphBuilderThread(TrieGraphNode<T> boot, AutoCSer.Threading.AutoWaitCount waitCount) : base(boot)
        {
            this.waitCount = waitCount;
            threadWait.Set(0);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(build);
        }
        /// <summary>
        /// 建图线程
        /// </summary>
        private void build()
        {
            try
            {
                do
                {
                    threadWait.Wait();
                    if (count == 0) return;
                    Build();
                    waitCount.Free();
                }
                while (true);
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(this.Exception = exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
            }
        }
        /// <summary>
        /// 释放建图线程
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeThread()
        {
            count = 0;
            threadWait.Set();
        }
        /// <summary>
        /// 设置当前处理节点集合
        /// </summary>
        /// <param name="reader">当前处理节点集合</param>
        /// <param name="startIndex">处理节点起始索引位置</param>
        /// <param name="count">处理节点节点索引位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetThread(TrieGraphNode<T>[] reader, int startIndex, int count)
        {
            if (count != 0)
            {
                Set(reader, startIndex, count);
                threadWait.Set();
            }
        }
        /// <summary>
        /// 设置当前处理节点集合
        /// </summary>
        /// <param name="reader">当前处理节点集合</param>
        /// <param name="startIndex">处理节点起始索引位置</param>
        /// <param name="count">处理节点节点索引位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(TrieGraphNode<T>[] reader, int startIndex, int count)
        {
            this.reader = reader;
            this.startIndex = startIndex;
            this.count = count;
        }
    }
}
