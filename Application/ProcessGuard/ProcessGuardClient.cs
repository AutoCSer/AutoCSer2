using AutoCSer.Net;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端
    /// </summary>
    public sealed class ProcessGuardClient
    {
        /// <summary>
        /// 进程守护客户端套接字事件
        /// </summary>
        private readonly IProcessGuardClientSocketEvent socketEvent;
        /// <summary>
        /// 当前进程信息
        /// </summary>
        private readonly ProcessGuardInfo processInfo;
        /// <summary>
        /// 是否移除守护
        /// </summary>
        private bool isRemoveGuard;
        /// <summary>
        /// 最后一次守护调用返回值
        /// </summary>
        public CommandClientReturnValue<bool> GuardReturnValue;
        /// <summary>
        /// 进程守护客户端
        /// </summary>
        /// <param name="socketEvent">进程守护客户端套接字事件</param>
        public ProcessGuardClient(IProcessGuardClientSocketEvent socketEvent)
        {
            this.socketEvent = socketEvent;
            processInfo = new ProcessGuardInfo(Process.GetCurrentProcess());
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <returns></returns>
        internal async Task OnClientMethodVerified()
        {
            if (!isRemoveGuard && socketEvent.IProcessGuardClient != null) GuardReturnValue = await socketEvent.IProcessGuardClient.Guard(processInfo);
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns>await</returns>
#if NetStandard21
        public ReturnCommand? GetRemoveGuardAwaiter()
#else
        public ReturnCommand GetRemoveGuardAwaiter()
#endif
        {
            isRemoveGuard = true;
            return socketEvent.IProcessGuardClient?.Remove(processInfo.ProcessID, processInfo.ProcessName);
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns></returns>
        public async Task<CommandClientReturnValue> RemoveGuard()
        {
            isRemoveGuard = true;
            if (socketEvent.IProcessGuardClient != null) return await socketEvent.IProcessGuardClient.Remove(processInfo.ProcessID, processInfo.ProcessName);
            return CommandClientReturnTypeEnum.Unknown;
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveGuard(Action<CommandClientReturnValue> callback)
        {
            isRemoveGuard = true;
            socketEvent.IProcessGuardClient?.Remove(processInfo.ProcessID, processInfo.ProcessName, callback);
        }
    }
}
