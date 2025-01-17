using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 进程守护客户端套接字事件
    /// </summary>
    internal sealed class ProcessGuardClientSocketEvent : AutoCSer.CommandService.ProcessGuardClientSocketEvent
    {
        /// <summary>
        /// 进程守护客户端套接字事件
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        internal ProcessGuardClientSocketEvent(CommandClient commandClient) : base(commandClient) { }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnSetController(CommandClientSocket socket)
        {
            await base.OnSetController(socket);

            if (ProcessGuardClient.GuardReturnValue.IsSuccess)
            {
                if (ProcessGuardClient.GuardReturnValue.Value) ConsoleWriteQueue.WriteLine("进程守护请求调用成功");
                else ConsoleWriteQueue.WriteLine("进程守护请求调用失败", ConsoleColor.Red);
            }
            else ConsoleWriteQueue.WriteLine($"进程守护请求调用失败 {ProcessGuardClient.GuardReturnValue.ReturnType} {ProcessGuardClient.GuardReturnValue.ErrorMessage}", ConsoleColor.Red);
        }
    }
}
