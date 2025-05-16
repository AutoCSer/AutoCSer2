using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程池
    /// </summary>
    public sealed class ThreadPool
    {
        /// <summary>
        /// 最低线程堆栈大小 128KB
        /// </summary>
        internal const int TinyStackSize = 128 << 10;
        /// <summary>
        /// 默认线程堆栈大小 1MB
        /// </summary>
        private const int defaultStackSize = 1 << 20;

        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 线程堆栈大小
        /// </summary>
        internal readonly int StackSize;
        /// <summary>
        /// 是否后台线程
        /// </summary>
        internal readonly bool IsBackground;
        /// <summary>
        /// 线程链表
        /// </summary>
        private LinkStack<Thread> threads;
        /// <summary>
        /// 空闲线程数量
        /// </summary>
        private volatile int freeThreadCount;
        /// <summary>
        /// 线程池
        /// </summary>
        /// <param name="stackSize">线程堆栈大小</param>
        /// <param name="isBackground">是否后台线程</param>
        private ThreadPool(int stackSize = defaultStackSize, bool isBackground = false)
        {
            StackSize = Math.Max(stackSize, TinyStackSize);
            IsBackground = isBackground;
        }
        /// <summary>
        /// 后台线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushBackground(Thread thread)
        {
            threads.Push(thread);
            Interlocked.Increment(ref freeThreadCount);
        }
        /// <summary>
        /// 前台线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        /// <returns>线程池是否已经释放</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Push(Thread thread)
        {
            if (isDisposed == 0)
            {
                threads.Push(thread);
                Interlocked.Increment(ref freeThreadCount);
                if (isDisposed == 0) return false;
            }
            return true;
        }
        /// <summary>
        /// 前台线程退出
        /// </summary>
        private void exit()
        {
            isDisposed = 1;
            Interlocked.Exchange(ref freeThreadCount, 0);
            for (var thread = threads.Get(); thread != null; thread = thread.StopLink()) ;
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        internal void FastStart(Action task)
        {
            var thread = threads.Pop();
            if (thread == null) new Thread(this, task);
            else
            {
                thread.RunTask(task);
                Interlocked.Decrement(ref freeThreadCount);
            }
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Start(Action task)
        {
            if (task != null && isDisposed == 0)
            {
                FastStart(task);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否空闲线程
        /// </summary>
        private void releaseFree()
        {
            int count = AutoCSer.Common.ProcessorCount;
            while (freeThreadCount > count)
            {
                var thread = threads.Pop();
                if (thread == null) return;
                Interlocked.Decrement(ref freeThreadCount);
                thread.Stop();
            }
        }
        /// <summary>
        /// 是否空闲线程
        /// </summary>
        private void releaseFreeBackground()
        {
            var exitThread = default(Thread);
            int count = AutoCSer.Common.ProcessorCount;
            while (freeThreadCount > count)
            {
                var thread = threads.Pop();
                if (thread == null) break;
                if (object.ReferenceEquals(thread, BackgroundExitThread)) exitThread = thread;
                else
                {
                    Interlocked.Decrement(ref freeThreadCount);
                    thread.Stop();
                }
            }
            if (exitThread != null) threads.Push(exitThread);
        }

        /// <summary>
        /// 微型线程池,堆栈 128K
        /// </summary>
        public static readonly ThreadPool Tiny = new ThreadPool(TinyStackSize);
        /// <summary>
        /// 微型后台线程池,堆栈 128K
        /// </summary>
        public static readonly ThreadPool TinyBackground = new ThreadPool(TinyStackSize, true);
        /// <summary>
        /// 后台退出测试线程
        /// </summary>
        internal static readonly Thread BackgroundExitThread = new Thread(TinyBackground);
        /// <summary>
        /// 前台退出测试
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckExit()
        {
            if (BackgroundExitThread.IsAborted)
            {
                TinyBackground.isDisposed = 1;
                Tiny.exit();
            }
        }
        /// <summary>
        /// 释放多余线程数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void releaseFreeThread()
        {
            Tiny.releaseFree();
            TinyBackground.releaseFreeBackground();
        }
        static ThreadPool()
        {
            int internalTaskSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (internalTaskSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(releaseFreeThread, internalTaskSeconds, Threading.SecondTimerKeepModeEnum.After, internalTaskSeconds);
#if !AOT
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(ThreadPool));
#endif
        }
    }
}
