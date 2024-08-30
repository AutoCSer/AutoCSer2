using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.TimestampVerify
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证接口
    /// </summary>
    [CommandServerController(InterfaceType = typeof(ITimestampVerify))]
    internal class TimestampVerify : AutoCSer.CommandService.TimestampVerifyService, ITimestampVerify
    {
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
        /// </summary>
        /// <param name="server"></param>
        public TimestampVerify(CommandListener server) : base(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ITimestampVerify.Add(int left, int right)
        {
            return left + right;
        }
    }
}
