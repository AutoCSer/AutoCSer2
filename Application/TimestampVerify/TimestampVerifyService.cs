using System;
using System.Security.Cryptography;
using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Extensions;
using AutoCSer.Net;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    [CommandServerController(InterfaceType = typeof(ITimestampVerifyService))]
    public class TimestampVerifyService : ITimestampVerifyService, IDisposable
    {
        /// <summary>
        /// 会话对象操作接口
        /// </summary>
        private readonly ICommandListenerSession<ITimestampVerifySession> socketSessionObject;
        /// <summary>
        /// 递增登录时间戳检查器
        /// </summary>
        protected TimestampVerifyChecker timestampChecker;
        /// <summary>
        /// 服务认证验证字符串
        /// </summary>
        protected readonly string verifyString;
        /// <summary>
        /// MD5 加密
        /// </summary>
        protected readonly MD5 md5;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected bool isDisposed;
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
        /// </summary>
        /// <param name="listener">SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession[AutoCSer.CommandService.ITimestampVerifySession]</param>
        /// <param name="verifyString">服务认证验证字符串</param>
        /// <param name="maxSecondsDifference">最大时间差秒数，默认为 5</param>
        public TimestampVerifyService(CommandListener listener, string verifyString, byte maxSecondsDifference = 5)
        {
            this.verifyString = verifyString;
            timestampChecker = new TimestampVerifyChecker(maxSecondsDifference);
            socketSessionObject = listener.GetSessionObject<ICommandListenerSession<ITimestampVerifySession>>() ?? AutoCSer.CommandService.TimestampVerify.CommandListenerSession.Default;
            md5 = MD5.Create();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                md5.Dispose();
            }
        }
        /// <summary>
        /// 验证函数，默认采用 MD5 做 Hash 计算
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="hashData">验证 Hash 数据</param>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns></returns>
        public virtual CommandServerVerifyStateEnum Verify(CommandServerSocket socket, CommandServerCallQueue queue, ulong randomPrefix, byte[] hashData, ref long timestamp)
        {
            if (hashData?.Length == 16)
            {
                var session = socketSessionObject.TryGetSessionObject(socket);
                if (session == null)
                {
                    long serverTimestamp = 0;
                    switch(timestampChecker.Check(ref timestamp, ref serverTimestamp))
                    {
                        case CommandServerVerifyStateEnum.Success: break;
                        case CommandServerVerifyStateEnum.Retry:
                            session = socketSessionObject.CreateSessionObject(socket);
                            session.ServerTimestamp = serverTimestamp;
                            return CommandServerVerifyStateEnum.Retry;
                        default:
                            timestamp = 0;
                            return CommandServerVerifyStateEnum.Fail;
                    }
                }
                else
                {
                    long serverTimestamp = session.ServerTimestamp;
                    switch (timestampChecker.Check(ref timestamp, ref serverTimestamp))
                    {
                        case CommandServerVerifyStateEnum.Success: break;
                        case CommandServerVerifyStateEnum.Retry:
                            session.ServerTimestamp = serverTimestamp;
                            return CommandServerVerifyStateEnum.Retry;
                        default:
                            timestamp = 0;
                            return CommandServerVerifyStateEnum.Fail;
                    }
                }
                if (AutoCSer.Net.TimestampVerify.Md5Equals(AutoCSer.Net.TimestampVerify.Md5(md5, verifyString, randomPrefix, timestamp), hashData) == 0)
                {
                    timestampChecker.Set(timestamp);
                    return CommandServerVerifyStateEnum.Success;
                }
            }
            timestamp = 0;
            return CommandServerVerifyStateEnum.Fail;
        }
    }
}
