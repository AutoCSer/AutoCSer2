using AutoCSer.Net;
using System;
using System.Diagnostics;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 被守护进程信息
    /// </summary>
    public class GuardProcess : CommandServerCallQueueNode
    {
        /// <summary>
        /// 进程守护服务端管理器
        /// </summary>
        protected readonly ProcessGuardManager guardManager;
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        public readonly ProcessGuardInfo ProcessInfo;
        /// <summary>
        /// 被守护进程
        /// </summary>
        protected readonly Process process;
        /// <summary>
        /// 进程退出事件
        /// </summary>
        private readonly EventHandler guardHandle;
        /// <summary>
        /// 新进程
        /// </summary>
        internal Process NewProcess;
        /// <summary>
        /// 是否重新启动进程
        /// </summary>
        private bool isReStart;
        /// <summary>
        /// 是否已经被移除
        /// </summary>
        public bool IsRemove { get; private set; }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="guardManager"></param>
        /// <param name="processInfo"></param>
        /// <param name="process"></param>
        internal GuardProcess(ProcessGuardManager guardManager, ProcessGuardInfo processInfo, Process process)
        {
            this.guardManager = guardManager;
            this.ProcessInfo = processInfo;
            this.process = process;
            guardHandle = guard;
            process.EnableRaisingEvents = true;//服务端需要以管理员身份运行，否则可能异常
            process.Exited += guardHandle;
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="guardProcess"></param>
        internal GuardProcess(GuardProcess guardProcess)
        {
            guardManager = guardProcess.guardManager;
            process = guardProcess.NewProcess;
            ProcessInfo = new ProcessGuardInfo(process);
            guardHandle = guard;
            process.EnableRaisingEvents = true;//服务端需要以管理员身份运行，否则可能异常
            process.Exited += guardHandle;
        }
        /// <summary>
        /// 进程退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guard(object sender, EventArgs e)
        {
            if (!isReStart)
            {
                isReStart = true;
                try
                {
                    if (!IsRemove) NewProcess = ProcessInfo.Start();
                }
                catch (Exception exception)
                {
                    guardManager.OnProcessStartError(exception, this);
                }
                finally
                {
                    guardManager.Controller.AddQueueLowPriority(this);
                    close();
                }
            }
        }
        /// <summary>
        /// 重新启动进程以后通知管理器
        /// </summary>
        public override void RunTask()
        {
            guardManager.OnStart(this);
        }
        /// <summary>
        /// 移除被守护进程
        /// </summary>
        internal void Remove()
        {
            IsRemove = true;
            close();
        }
        /// <summary>
        /// 释放进程
        /// </summary>
        private void close()
        {
            process.Exited -= guardHandle;
            process.Dispose();
        }
    }
}
