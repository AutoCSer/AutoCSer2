using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 被守护进程信息
    /// </summary>
    internal sealed class GuardProcess : ReadWriteQueueNode
    {
        /// <summary>
        /// 进程守护节点
        /// </summary>
        private readonly ProcessGuardNode node;
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        public readonly ProcessGuardInfo ProcessInfo;
        /// <summary>
        /// 被守护进程
        /// </summary>
        private Process process;
        /// <summary>
        /// 进程退出事件
        /// </summary>
        private EventHandler guardHandle;
        /// <summary>
        /// 新进程
        /// </summary>
#if NetStandard21
        internal Process? NewProcess;
#else
        internal Process NewProcess;
#endif
        /// <summary>
        /// 是否重新启动进程
        /// </summary>
        private int isReStart;
        /// <summary>
        /// 是否已经被移除
        /// </summary>
        internal bool IsRemove;
        /// <summary>
        /// 是否初始化加载数据
        /// </summary>
        internal bool IsLoad;
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="processInfo"></param>
        /// <param name="process"></param>
        internal GuardProcess(ProcessGuardNode node, ProcessGuardInfo processInfo, Process process)
        {
            this.node = node;
            this.ProcessInfo = processInfo;
            this.process = process;
            if (node.StreamPersistenceMemoryDatabaseService.IsLoaded)
            {
                process.EnableRaisingEvents = true;//服务端需要以管理员身份运行，否则可能异常
                guardHandle = guard;
                process.Exited += guardHandle;
            }
            else
            {
                guardHandle = AutoCSer.Common.EmptyEventHandler;
                IsLoad = true;
            }
        }
        /// <summary>
        /// 被守护进程信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="processInfo"></param>
        internal GuardProcess(ProcessGuardNode node, ProcessGuardInfo processInfo)
        {
            this.node = node;
            this.ProcessInfo = processInfo;
            process = AutoCSer.Common.CurrentProcess;
            guardHandle = AutoCSer.Common.EmptyEventHandler;
            IsLoad = true;
        }
        /// <summary>
        /// 数据库冷启动初始化启动被守护进程
        /// </summary>
        internal void Loaded()
        {
            if (object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess))
            {
                try
                {
                    isReStart = 1;
                    NewProcess = ProcessInfo.Start();
                    ProcessGuardNode.Output(nameof(Loaded), ProcessInfo);
                }
                catch (Exception exception)
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.Server.Log.ExceptionIgnoreException(exception, Culture.Configuration.Default.GetGuardProcessStartFailed(ProcessInfo), LogLevelEnum.AutoCSer | LogLevelEnum.Exception | LogLevelEnum.Fatal);
                }
                finally
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(this);
                }
            }
            else
            {
                try
                {
                    if (!IsRemove)
                    {
                        setExited();
                        IsLoad = false;
                    }
                }
                catch (Exception exception)
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.Server.Log.ExceptionIgnoreException(exception, Culture.Configuration.Default.GetGuardProcessStartFailed(ProcessInfo), LogLevelEnum.AutoCSer | LogLevelEnum.Exception | LogLevelEnum.Fatal);
                }
            }
        }
        /// <summary>
        /// 设置进程退出事件
        /// </summary>
        private void setExited()
        {
            process.EnableRaisingEvents = true;//服务端需要以管理员身份运行，否则可能异常
            guardHandle = guard;
            process.Exited += guardHandle;
        }
        /// <summary>
        /// 进程退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#if NetStandard21
        private void guard(object? sender, EventArgs e)
#else
        private void guard(object sender, EventArgs e)
#endif
        {
            if (System.Threading.Interlocked.CompareExchange(ref isReStart, 1, 0) == 0)
            {
                try
                {
                    if (!IsRemove) NewProcess = ProcessInfo.Start();
                }
                catch (Exception exception)
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.Server.Log.ExceptionIgnoreException(exception, Culture.Configuration.Default.GetGuardProcessStartFailed(ProcessInfo), LogLevelEnum.AutoCSer | LogLevelEnum.Exception | LogLevelEnum.Fatal);
                }
                finally
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(this);
                    close();
                }
            }
        }
        /// <summary>
        /// 重新启动进程以后通知管理器
        /// </summary>
        public override void RunTask()
        {
            node.OnExited(this);
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
            if (!object.ReferenceEquals(process, AutoCSer.Common.CurrentProcess))
            {
                if (!object.ReferenceEquals(guardHandle, AutoCSer.Common.EmptyEventHandler))
                {
                    process.Exited -= guardHandle;
                    guardHandle = AutoCSer.Common.EmptyEventHandler;
                }
                process.Dispose();
            }
        }
    }
}
