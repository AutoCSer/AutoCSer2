using AutoCSer.Deploy;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 切换进程
    /// </summary>
    public abstract class ProcessGuardSwitchProcess : SwitchProcess
    {
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="args"></param>
        protected ProcessGuardSwitchProcess(string[] args) : base(args) { }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onStart()
        {
            AutoCSer.Threading.CatchTask.AddIgnoreException(processGuardCommandClient());
        }
        /// <summary>
        /// 进程守护调用客户端
        /// </summary>
        protected ProcessGuardClientSocketEvent processGuardClient;
        /// <summary>
        /// 获取守护进程客户端配置
        /// </summary>
        /// <returns></returns>
        protected abstract ProcessGuardCommandClientConfig getCommandClientConfig();
        /// <summary>
        /// 启动守护
        /// </summary>
        /// <returns></returns>
        protected virtual async Task processGuardCommandClient()
        {
            await Task.Yield();
            ProcessGuardCommandClientConfig commandClientConfig = getCommandClientConfig();
            CommandClient commandClient = new CommandClient(commandClientConfig);
            await commandClient.GetSocketAsync();
            processGuardClient = (ProcessGuardClientSocketEvent)commandClient.SocketEvent;
            await onProcessGuardClient();
        }
        /// <summary>
        /// 创建守护进程客户端后续处理
        /// </summary>
        /// <returns></returns>
        protected virtual async Task onProcessGuardClient() { }
        /// <summary>
        /// 退出运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onExit()
        {
            if (processGuardClient != null) await processGuardClient.RemoveGuardAsync();
            await base.onExit();
        }
    }
}
