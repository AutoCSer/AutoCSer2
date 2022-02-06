using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端套接字事件
    /// </summary>
    public class ProcessGuardClientSocketEvent : CommandClientSocketEvent
    {
        /// <summary>
        /// 当前进程信息
        /// </summary>
        private readonly ProcessGuardInfo processInfo;
        /// <summary>
        /// 是否移除守护
        /// </summary>
        private bool isRemoveGuard;
        /// <summary>
        /// 分布式锁客户端接口
        /// </summary>
        public IProcessGuardClient ProcessGuardClient { get; protected set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(IProcessGuard), typeof(IProcessGuardClient));
            }
        }
        /// <summary>
        /// 最后一次守护调用返回值
        /// </summary>
        public CommandClientReturnValue<bool> GuardReturnValue;
        /// <summary>
        /// 进程守护客户端套接字事件
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        public ProcessGuardClientSocketEvent(CommandClient commandClient) : base(commandClient)
        {
            processInfo = new ProcessGuardInfo(Process.GetCurrentProcess());
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <returns></returns>
        protected override async Task onMethodVerified()
        {
            if (!isRemoveGuard && ProcessGuardClient != null) GuardReturnValue = await ProcessGuardClient.GuardAsync(processInfo);
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns>await</returns>
        public ReturnCommand GetRemoveGuardAwaiter()
        {
            isRemoveGuard = true;
            return ProcessGuardClient?.RemoveAsync(processInfo.ProcessID, processInfo.ProcessName);
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns></returns>
        public async Task<CommandClientReturnValue> RemoveGuardAsync()
        {
            isRemoveGuard = true;
            if (ProcessGuardClient != null) return await ProcessGuardClient.RemoveAsync(processInfo.ProcessID, processInfo.ProcessName);
            return CommandClientReturnType.Unknown;
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <returns></returns>
        public CommandClientReturnValue RemoveGuard()
        {
            isRemoveGuard = true;
            if (ProcessGuardClient != null) return ProcessGuardClient.Remove(processInfo.ProcessID, processInfo.ProcessName);
            return CommandClientReturnType.Unknown;
        }
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveGuard(Action<CommandClientReturnValue> callback)
        {
            isRemoveGuard = true;
            ProcessGuardClient?.Remove(processInfo.ProcessID, processInfo.ProcessName, callback);
        }
    }
}
