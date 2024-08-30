using AutoCSer.Extensions;
using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public abstract partial class TrieGraph<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// 根节点
        /// </summary>
        internal readonly TrieGraphNode<T> Boot;
        /// <summary>
        /// Trie 图
        /// </summary>
        protected TrieGraph()
        {
            Boot = new TrieGraphNode<T>();
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="threadCount">建图并行线程数量，默认为 0 表示 CPU 逻辑处理器数量</param>
        internal void BuildGraph(int threadCount)
        {
            if (Boot.Nodes != null)
            {
                int maxThreadCount = Math.Min(Boot.Nodes.Count, AutoCSer.Common.ProcessorCount);
                if (threadCount <= 0 || threadCount > maxThreadCount) threadCount = maxThreadCount;
                if (threadCount > 1) buildGraph(threadCount);
                else buildGraph();
            }
        }
        /// <summary>
        /// 单线程建图
        /// </summary>
        private unsafe void buildGraph()
        {
            TrieGraphBuilder<T> builder = new TrieGraphBuilder<T>(Boot);
            for (LeftArray<TrieGraphNode<T>> reader = new LeftArray<TrieGraphNode<T>>(Boot.Nodes.Values.getArray()); reader.Length != 0; reader.Exchange(ref builder.Writer))
            {
                builder.Set(ref reader);
                builder.Build();
            }
        }
        /// <summary>
        /// 多线程并行建图
        /// </summary>
        /// <param name="threadCount">并行线程数量</param>
        private void buildGraph(int threadCount)
        {
            LeftArray<TrieGraphNode<T>> reader = new LeftArray<TrieGraphNode<T>>(Boot.Nodes.Values.getArray());
            int taskCount = threadCount - 1;
            bool isError = false;
            AutoCSer.Threading.AutoWaitCount waitCount = new AutoCSer.Threading.AutoWaitCount(taskCount);
            TrieGraphBuilderThread<T>[] threads = new TrieGraphBuilderThread<T>[threadCount];
            try
            {
                for (int builderIndex = 0; builderIndex != threads.Length; threads[builderIndex++] = new TrieGraphBuilderThread<T>(Boot, waitCount)) ;
                do
                {
                    TrieGraphNode<T>[] readerArray = reader.Array;
                    int count = reader.Length / threadCount, index = 0;
                    for (int builderIndex = 0; builderIndex != taskCount; ++builderIndex)
                    {
                        threads[builderIndex].SetThread(readerArray, index, count);
                        index += count;
                    }
                    threads[taskCount].Set(readerArray, index, reader.Length);
                    threads[taskCount].Build();
                    waitCount.WaitSet(taskCount);
                    reader.Length = 0;
                    foreach (TrieGraphBuilderThread<T> builder in threads)
                    {
                        if (builder.Exception == null) reader.Add(ref builder.Writer);
                        else isError = true;
                    }
                }
                while (reader.Length != 0 && !isError);
            }
            finally
            {
                foreach (TrieGraphBuilderThread<T> builder in threads)
                {
                    if (builder != null && builder.Exception == null) builder.FreeThread();
                }
            }
        }
    }
}
