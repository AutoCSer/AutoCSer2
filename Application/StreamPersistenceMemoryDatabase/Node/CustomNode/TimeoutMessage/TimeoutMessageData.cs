using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// 超时任务消息序列化数据
    /// </summary>
    /// <typeparam name="T">任务消息数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TimeoutMessageData<T>
    {
        /// <summary>
        /// 任务消息标识
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 触发执行任务超时时间
        /// </summary>
        internal DateTime Timeout;
        /// <summary>
        /// 任务消息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T Task;
        /// <summary>
        /// 是否已经启动任务
        /// </summary>
        internal bool IsRunTask;
        /// <summary>
        /// 任务是否执行失败
        /// </summary>
        internal bool IsFailed;
        /// <summary>
        /// 是否已经删除
        /// </summary>
        internal bool IsRemoved;
        /// <summary>
        /// 是否快照任务链表节点
        /// </summary>
        internal bool IsLinkSnapshot;
        /// <summary>
        /// 设置节点当前分配任务标识
        /// </summary>
        /// <param name="currentIdentity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(long currentIdentity)
        {
            Identity = currentIdentity;
            Timeout = DateTime.MaxValue;
        }
        /// <summary>
        /// 超时检查
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckTimeout()
        {
            return !IsRunTask && !IsRemoved;
        }
        /// <summary>
        /// 添加立即执行任务
        /// </summary>
        /// <param name="currentIdentity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendRun(long currentIdentity)
        {
            Identity = currentIdentity;
            IsRunTask = true;
        }
        /// <summary>
        /// 判断是否需要启动任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckRunTask()
        {
            if (!IsRunTask)
            {
                IsRunTask = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断初始化加载数据是否需要启动任务
        /// </summary>
        /// <returns></returns>
        internal bool CheckLoadRunTask()
        {
            if (IsRunTask) return !IsFailed;
            if (Timeout <= AutoCSer.Threading.SecondTimer.UtcNow)
            {
                IsRunTask = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否快照任务链表节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckLinkSnapshot()
        {
            if (!IsRunTask || !IsFailed)
            {
                if (!IsRemoved)
                {
                    IsLinkSnapshot = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 任务执行失败
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Failed()
        {
            if (!IsFailed)
            {
                IsFailed = true;
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Cancel()
        {
            IsRemoved = true;
            if (!IsFailed)
            {
                IsFailed = true;
                return 0;
            }
            return 1;
        }
    }
}
