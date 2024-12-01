//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 进程守护服务端接口（服务端需要以管理员身份运行） 客户端接口
        /// </summary>
        public partial interface IProcessGuardServiceClientController
        {
            /// <summary>
            /// 添加待守护进程
            /// </summary>
            /// <param name="processInfo">进程信息</param>
            /// <returns>是否添加成功</returns>
            AutoCSer.Net.ReturnCommand<bool> Guard(AutoCSer.CommandService.ProcessGuardInfo processInfo);
            /// <summary>
            /// 删除被守护进程
            /// </summary>
            /// <param name="processId">进程标识</param>
            /// <param name="processName">进程名称</param>
            AutoCSer.Net.ReturnCommand Remove(int processId, string processName);
        }
}
#endif