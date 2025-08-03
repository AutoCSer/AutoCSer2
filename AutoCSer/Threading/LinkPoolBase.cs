using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 缓存对象链表（用于冲突概率低的场景）
    /// </summary>
    /// <typeparam name="T">链表节点泛型类型</typeparam>
    internal abstract class LinkPoolBase<T> : AutoCSer.Threading.SecondTimerArrayNode
        where T : Link<T>
    {
        /// <summary>
        /// 无锁栈
        /// </summary>
        private LinkStack<T> stack;
        /// <summary>
        /// 缓存数量
        /// </summary>
        protected int count;
        /// <summary>
        /// 最大缓存数量（非精确数量）
        /// </summary>
        protected readonly int maxCount;
        /// <summary>
        /// 是否返回了 null 值
        /// </summary>
        private bool isNewValue;
        /// <summary>
        /// 是否需要释放资源
        /// </summary>
        private readonly bool isDisponse;
        /// <summary>
        /// 是否需要异步释放资源
        /// </summary>
        private readonly bool isAsyncDisponse;
        /// <summary>
        /// 链表
        /// </summary>
        /// <param name="parameter">默认链表缓存池参数</param>
        /// <param name="type">缓存对象链表类型</param>
        /// <param name="isDisponse">是否需要释放资源</param>
        /// <param name="isAsyncDisponse">是否需要异步释放资源</param>
        protected LinkPoolBase(LinkPoolParameter parameter, Type type, bool isDisponse, bool isAsyncDisponse) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, SecondTimerKeepModeEnum.After)
        {
            maxCount = parameter.MaxObjectCount;
            this.isDisponse = isDisponse;
            this.isAsyncDisponse = isAsyncDisponse;
            if (parameter.MaxObjectCount > 0)
            {
                if (parameter.ReleaseFreeTimeoutSeconds > 0) AppendTaskArray(parameter.ReleaseFreeTimeoutSeconds);
#if !AOT
                AutoCSer.Memory.ObjectRoot.ScanType.Add(type);
#endif
            }
        }
        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void push(T value)
        {
            System.Threading.Interlocked.Increment(ref count);
            stack.Push(value);
        }
        /// <summary>
        /// Pop-up node
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        protected T? pop()
#else
        protected T pop()
#endif
        {
            var value = stack.Pop();
            if(value != null) System.Threading.Interlocked.Decrement(ref count);
            else isNewValue = true;
            return value;
        }
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        protected internal override void OnTimer()
        {
            if (!stack.IsEmpty && !isNewValue) Task.Run((Action)onTimer);
            else isNewValue = false;
        }
        /// <summary>
        /// 释放队形
        /// </summary>
        /// <param name="value"></param>
        protected void dispose(T value)
        {
            if (isAsyncDisponse) ((IAsyncDisposable)value).DisposeAsync().AutoCSerNotWait();
            else ((IDisposable)value).Dispose();
        }
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        private void onTimer()
        {
            var head = stack.Get();
            System.Threading.Interlocked.Exchange(ref count, 0);
            if (head != null)
            {
                int nodeCount;
                T middle = Link<T>.GetMiddle(head), end = Link<T>.GetEnd(middle, out nodeCount);
                System.Threading.Interlocked.Add(ref count, nodeCount);
                stack.PushLink(middle, end);
                while (!object.ReferenceEquals(head, middle))
                {
                    if (isDisponse) dispose(head.notNull());
                    head = head.notNull().LinkNext;
                }
            }
            isNewValue = false;
        }
    }
}
