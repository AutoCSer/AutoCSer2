using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 命令服务切换进程
    /// </summary>
    public abstract class CommandListenerSwitchProcess : ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 命令服务端监听
        /// </summary>
#if NetStandard21
        private AutoCSer.Net.CommandListener? commandListener;
#else
        private AutoCSer.Net.CommandListener commandListener;
#endif
        /// <summary>
        /// 服务启动失败输出错误信息
        /// </summary>
        protected virtual string startErrorMessage { get { return Culture.Configuration.Default.GetCommandListenerStartFailed(commandListener.notNull()); } }
        /// <summary>
        /// 命令服务切换进程
        /// </summary>
        /// <param name="args"></param>
        protected CommandListenerSwitchProcess(string[] args) : base(args) { }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onStart()
        {
            commandListener = await createCommandListener();
            if (!await commandListener.Start())
            {
                string errorMessage = startErrorMessage;
                Console.WriteLine(errorMessage);
                await commandListener.Log.Fatal(errorMessage);
            }
        }
        /// <summary>
        /// 创建命令服务端监听
        /// </summary>
        /// <returns></returns>
        protected abstract Task<AutoCSer.Net.CommandListener> createCommandListener();
        /// <summary>
        /// 退出运行
        /// </summary>
        /// <returns></returns>
        protected override Task onExit()
        {
            commandListener?.Dispose();
            return base.onExit();
        }
    }
}
