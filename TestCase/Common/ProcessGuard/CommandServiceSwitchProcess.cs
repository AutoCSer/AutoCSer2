using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 命令服务进程切换
    /// </summary>
    public abstract class CommandServiceSwitchProcess : ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 命令服务进程切换
        /// </summary>
        /// <param name="args"></param>
        protected CommandServiceSwitchProcess(string[] args) : base(args) { }
        /// <summary>
        /// 命令服务端配置
        /// </summary>
        protected CommandServerConfig commandServerConfig;
        /// <summary>
        /// 业务数据服务
        /// </summary>
        protected CommandListener commandListener;
        /// <summary>
        /// 切换进程等待关闭处理退出
        /// </summary>
        protected override void switchExit()
        {
            commandListener?.DisposeSocket();
            base.switchExit();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onStart()
        {
            await base.onStart();

            if (await commandListener.Start())
            {
                ConsoleWriteQueue.WriteLine($"业务数据服务启动成功 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}");
            }
        }
    }
}
