using AutoCSer.Net;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的反向服务认证套接字事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TimestampVerifyReverseServiceCommandClientSocketEvent<T> : AutoCSer.Net.CommandClientSocketEventTask<T>
        where T : TimestampVerifyReverseServiceCommandClientSocketEvent<T>
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        private readonly MD5 md5;
        /// <summary>
        /// 递增登录时间戳检查器
        /// </summary>
        private AutoCSer.CommandService.TimestampVerifyChecker timestampVerifyChecker;
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证套接字事件
        /// </summary>
        /// <param name="client"></param>
        protected TimestampVerifyReverseServiceCommandClientSocketEvent(ICommandClient client) : base(client)
        {
            md5 = MD5.Create();
            timestampVerifyChecker = new AutoCSer.CommandService.TimestampVerifyChecker(0);
        }
        /// <summary>
        /// 关闭命令客户端当前套接字通知，默认操作为通知等待当前连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override void OnClosed(CommandClientSocket socket)
        {
            md5.Dispose();
            base.OnClosed(socket);
        }
        /// <summary>
        /// 反向命令服务客户端监听验证套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">附加数据</param>
        /// <returns></returns>
        protected async Task<bool> callVerify<VT>(AutoCSer.Net.CommandClientSocket socket, VT data)
        {
            long timestamp = timestampVerifyChecker.GetTimestamp();
            AutoCSer.Net.CommandClientReturnValue<AutoCSer.CommandService.TimestampVerify.ReverseServiceVerifyData<VT>> result = await ((AutoCSer.CommandService.ITimestampVerifyReverseServiceClientController<VT>)socket.Controller).GetVerifyData(timestamp);
            if (result.IsSuccess)
            {
                result.Value.Data = data;
                return result.Value.Verify(timestamp, md5);
            }
            return false;
        }
    }
}
