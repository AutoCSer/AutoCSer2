using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
    /// <summary>
    /// 初始化加载数据回调
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    internal sealed class LoadCallback<T> : QueueTaskNode
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 分词结果磁盘块索引信息节点
        /// </summary>
        private readonly WordIdentityBlockIndexNode<T> node;
        /// <summary>
        /// 创建分词结果磁盘块索引信息返回参数集合
        /// </summary>
        internal readonly ResponseParameterAwaiter<WordIdentityBlockIndexUpdateStateEnum>[] CreateResponses;
        /// <summary>
        /// 分词数据关键字集合
        /// </summary>
        internal T[] Keys;
        /// <summary>
        /// 回调等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim waitLock;
        /// <summary>
        /// 新关键字数量
        /// </summary>
        internal int NewCount;
        /// <summary>
        /// 初始化加载数据回调
        /// </summary>
        /// <param name="node">分词结果磁盘块索引信息节点</param>
        /// <param name="keys">分词数据关键字集合</param>
        internal LoadCallback(WordIdentityBlockIndexNode<T> node, T[] keys)
        {
            this.node = node;
            Keys = keys;
            CreateResponses = new ResponseParameterAwaiter<WordIdentityBlockIndexUpdateStateEnum>[keys.Length];
            waitLock = new System.Threading.SemaphoreSlim(0, 1);
        }
        /// <summary>
        /// 检查关键字
        /// </summary>
        public override void RunTask()
        {
            NewCount = -1;
            try
            {
                int index = 0;
                foreach (T key in Keys)
                {
                    if (!node.Datas.ContainsKey(key)) Keys[index++] = key;
                }
                NewCount = index;
            }
            finally { waitLock.Release(); }
        }
        /// <summary>
        /// 等待关键字检查完成
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task Wait()
        {
            return waitLock.WaitAsync();
        }
    }
}
