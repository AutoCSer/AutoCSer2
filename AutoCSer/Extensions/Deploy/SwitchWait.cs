using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 切换进程等待关闭处理
    /// </summary>
    public sealed class SwitchWait
    {
        /// <summary>
        /// 等待事件
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private EventWaitHandle waitHandle;
        /// <summary>
        /// 关闭处理
        /// </summary>
        private readonly Action exit;
        /// <summary>
        /// 名称前缀，可用于区分环境版本
        /// </summary>
#if NetStandard21
        private readonly string? prefix;
#else
        private readonly string prefix;
#endif
        /// <summary>
        /// 切换进程等待关闭处理
        /// </summary>
        /// <param name="exit">关闭处理</param>
        /// <param name="prefix">名称前缀，可用于区分环境版本</param>
#if NetStandard21
        public SwitchWait(Action exit, string? prefix = null)
#else
        public SwitchWait(Action exit, string prefix = null)
#endif
        {
            this.exit = exit;
            this.prefix = prefix;
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(wait);
        }
        /// <summary>
        /// 等待关闭处理
        /// </summary>
#if NET8
        [MemberNotNull(nameof(waitHandle))]
#endif
        private void wait()
        {
            bool createdProcessWait;
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, prefix + Assembly.GetEntryAssembly().notNull().FullName, out createdProcessWait);
            if (!createdProcessWait)
            {
                waitHandle.Set();
                Thread.Sleep(1000);
            }
            waitHandle.Reset();
            waitHandle.WaitOne();
            exit();
        }

        /// <summary>
        /// 设置切换进程等待关闭处理
        /// </summary>
        /// <param name="prefix">名称前缀，可用于区分环境版本</param>
#if NetStandard21
        public static void Set(string? prefix = null)
#else
        public static void Set(string prefix = null)
#endif
        {
            using (EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, prefix + Assembly.GetEntryAssembly().notNull().FullName))
            {
                waitHandle.Set();
            }
        }
    }
}
