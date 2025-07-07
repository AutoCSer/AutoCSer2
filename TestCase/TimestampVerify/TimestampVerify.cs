using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.TimestampVerify
{
    /// <summary>
    /// Service authentication interface based on incremental login timestamp verification (in conjunction with HASH to prevent replay login operations)
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    [CommandServerController(InterfaceType = typeof(ITimestampVerify))]
    internal class TimestampVerify : AutoCSer.CommandService.TimestampVerifyService, ITimestampVerify
    {
        /// <summary>
        /// Service authentication interface based on incremental login timestamp verification (in conjunction with HASH to prevent replay login operations)
        /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
        /// </summary>
        /// <param name="server"></param>
        public TimestampVerify(CommandListener server) : base(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString) { }

        /// <summary>
        /// Test
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
