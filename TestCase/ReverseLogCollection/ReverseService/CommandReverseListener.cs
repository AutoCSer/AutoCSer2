using AutoCSer.CommandService;
using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    /// <summary>
    /// 反向命令服务客户端监听
    /// </summary>
    internal sealed class CommandReverseListener : AutoCSer.CommandService.ReverseLogCollection.CommandReverseListener<LogInfo>
    {
        /// <summary>
        /// 反向命令服务客户端监听
        /// </summary>
        /// <param name="config">反向命令服务客户端监听配置</param>
        internal CommandReverseListener(CommandClientConfig config) : base(config) { }
        /// <summary>
        /// 反向客户端验证
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected override async Task<bool> verify(CommandClient client)
        {
            long timestamp = timestampVerifyChecker.GetTimestamp();
            CommandClientReturnValue<ReverseServiceVerifyData<string>> result = await ((CommandClientSocketEvent)client.SocketEvent).TimestampVerifyReverseClient.GetVerifyData(timestamp);
            if (result.IsSuccess)
            {
                result.Value.Data = AutoCSer.TestCase.Common.Config.TimestampVerifyString;
                return result.Value.Verify(timestamp, md5);
            }
            return false;
        }
    }
}
