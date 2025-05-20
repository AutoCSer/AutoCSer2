using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public partial class Thread : Link<Thread>
    {
        /// <summary>
        /// 线程池
        /// </summary>
        private readonly ThreadPool threadPool;
        /// <summary>
        /// 线程句柄
        /// </summary>
        public readonly System.Threading.Thread Handle;
        /// <summary>
        /// 线程是否已经退出
        /// </summary>
        internal bool IsAborted
        {
            get
            {
                return Handle.ThreadState == System.Threading.ThreadState.Aborted;
            }
        }
        /// <summary>
        /// 任务
        /// </summary>
#if NetStandard21
        public Action? Task { get; private set; }
#else
        public Action Task { get; private set; }
#endif
        /// <summary>
        /// 等待事件
        /// </summary>
        private readonly System.Threading.AutoResetEvent waitHandle;
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        internal Thread(ThreadPool threadPool)
        {
            waitHandle = new System.Threading.AutoResetEvent(false);
            this.threadPool = threadPool;
            Handle = new System.Threading.Thread(exitTest, threadPool.StackSize);
            start(true);
        }
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        /// <param name="task">任务委托</param>
        internal Thread(ThreadPool threadPool, Action task)
        {
            Task = task;
            waitHandle = new System.Threading.AutoResetEvent(false);
            this.threadPool = threadPool;
            if (threadPool.IsBackground)
            {
                Handle = new System.Threading.Thread(runBackground, threadPool.StackSize);
                start(true);
            }
            else
            {
                Handle = new System.Threading.Thread(run, threadPool.StackSize);
                start(false);
            }
        }
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="isBackground"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void start(bool isBackground)
        {
            Handle.IsBackground = isBackground;
            Handle.Start();
        }
        /// <summary>
        /// 退出测试线程
        /// </summary>
        private void exitTest()
        {
            threadPool.PushBackground(this);
            waitHandle.WaitOne();
            if (Task != null) runBackground();
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void runBackground()
        {
            do
            {
                try
                {
                    do
                    {
                        Task.notNull()();
                        Task = null;
                        threadPool.PushBackground(this);
                        waitHandle.WaitOne();
                    }
                    while (Task != null);
                    return;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    Task = null;
                }
                threadPool.PushBackground(this);
                waitHandle.WaitOne();
            }
            while (Task != null);
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void run()
        {
            do
            {
                try
                {
                    do
                    {
                        Task.notNull()();
                        Task = null;
                        if (!threadPool.Push(this)) waitHandle.WaitOne();
                        else
                        {
                            waitHandle.Dispose();
                            return;
                        }
                    }
                    while (Task != null);
                    return;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    Task = null;
                }
                if (!threadPool.Push(this)) waitHandle.WaitOne();
                else
                {
                    waitHandle.Dispose();
                    return;
                }
            }
            while (Task != null);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RunTask(Action task)
        {
            this.Task = task;
            waitHandle.Set();
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Stop()
        {
            Task = null;
            waitHandle.setDispose();
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Thread? StopLink()
#else
        internal Thread StopLink()
#endif
        {
            var next = LinkNext;
            Task = null;
            LinkNext = null;
            waitHandle.setDispose();
            return next;
        }

        /// <summary>
        /// System.Threading.Thread.BeginThreadAffinity() 调用状态
        /// </summary>
        private static int beginThreadAffinityState = 2;
        /// <summary>
        /// System.Threading.Thread.BeginThreadAffinity()
        /// </summary>
        /// <returns>调用是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool BeginThreadAffinity()
        {
            switch (beginThreadAffinityState)
            {
                case 0:
                    System.Threading.Thread.BeginThreadAffinity();
                    return true;
                case 1: return false;
            }
            return beginThreadAffinity();
        }
        /// <summary>
        /// System.Threading.Thread.BeginThreadAffinity()
        /// </summary>
        /// <returns></returns>
        private static bool beginThreadAffinity()
        {
            try
            {
                System.Threading.Thread.BeginThreadAffinity();
                beginThreadAffinityState = 0;
                return true;
            }
            catch(Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            beginThreadAffinityState = 1;
            return false;
        }
    }
}
