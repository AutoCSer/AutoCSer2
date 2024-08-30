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
        protected override Task onStart()
        {
            AutoCSer.Threading.CatchTask.AddIgnoreException(processGuardCommandClient());
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 获取守护进程客户端配置
        /// </summary>
        /// <returns></returns>
        protected abstract CommandClientConfig getCommandClientConfig();
        /// <summary>
        /// 进程守护客户端
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        protected virtual IProcessGuardClientSocketEvent getProcessGuardClientSocketEvent(CommandClient commandClient) { return (IProcessGuardClientSocketEvent)commandClient.SocketEvent; }
        /// <summary>
        /// 进程守护调用客户端
        /// </summary>
        protected ProcessGuardClient processGuardClient;
        /// <summary>
        /// 启动守护
        /// </summary>
        /// <returns></returns>
        protected virtual async Task processGuardCommandClient()
        {
            CommandClientConfig commandClientConfig = getCommandClientConfig();
            CommandClient commandClient = new CommandClient(commandClientConfig);
            await commandClient.GetSocketAsync();
            processGuardClient = getProcessGuardClientSocketEvent(commandClient).ProcessGuardClient;
            await onProcessGuardClient();
        }
        /// <summary>
        /// 创建守护进程客户端后续处理
        /// </summary>
        /// <returns></returns>
        protected virtual Task onProcessGuardClient() { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 退出运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onExit()
        {
            await processGuardClient.RemoveGuard();
            await base.onExit();
        }
    }
}
