using System;
using System.Security.Cryptography;
using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Extensions;
using AutoCSer.Net;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Service authentication interface based on incremental login timestamp verification (in conjunction with HASH to prevent replay login operations)
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    [CommandServerController(InterfaceType = typeof(ITimestampVerifyService))]
    public class TimestampVerifyService : ITimestampVerifyService, IDisposable
    {
        /// <summary>
        /// The session object operates the interface instance
        /// 会话对象操作接口实例
        /// </summary>
        private readonly ICommandListenerSession<ITimestampVerifySession> socketSessionObject;
        /// <summary>
        /// Increment the login timestamp checker
        /// 递增登录时间戳检查器
        /// </summary>
        protected TimestampVerifyChecker timestampChecker;
        /// <summary>
        /// Server authentication verification string
        /// 服务认证验证字符串
        /// </summary>
        protected readonly string verifyString;
        /// <summary>
        /// MD5 encryption
        /// MD5 加密
        /// </summary>
        protected readonly MD5 md5;
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        protected bool isDisposed;
        /// <summary>
        /// Service authentication interface based on incremental login timestamp verification (in conjunction with HASH to prevent replay login operations)
        /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
        /// </summary>
        /// <param name="listener">SessionObject must implement AutoCSer.Net.ICommandListenerSession{AutoCSer.CommandService.ITimestampVerifySession}
        /// SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession{AutoCSer.CommandService.ITimestampVerifySession}</param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        /// <param name="maxSecondsDifference">The maximum time difference in seconds is defaulted to 5
        /// 最大时间差秒数，默认为 5</param>
        public TimestampVerifyService(CommandListener listener, string verifyString, byte maxSecondsDifference = 5)
        {
            this.verifyString = verifyString;
            timestampChecker = new TimestampVerifyChecker(maxSecondsDifference);
            socketSessionObject = listener.GetSessionObject<ICommandListenerSession<ITimestampVerifySession>>() ?? CommandListenerSession.Default;
            md5 = MD5.Create();
        }
        /// <summary>
        /// Release resources
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
        /// The verification method defaults to using MD5 for Hash calculation
        /// 验证方法，默认采用 MD5 做 Hash 计算
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="randomPrefix">Random prefix
        /// 随机前缀</param>
        /// <param name="hashData">Hash data to be verified
        /// 待验证 Hash 数据</param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns></returns>
        public virtual CommandServerVerifyStateEnum Verify(CommandServerSocket socket, CommandServerCallQueue queue, ulong randomPrefix, byte[] hashData, ref long timestamp)
        {
            if (hashData?.Length == 16)
            {
                var session = socketSessionObject.TryGetSessionObject(socket);
                if (session == null)
                {
                    long serverTimestamp = 0;
                    switch(timestampChecker.CheckQueue(ref timestamp, ref serverTimestamp))
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
                    switch (timestampChecker.CheckQueue(ref timestamp, ref serverTimestamp))
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
                    timestampChecker.SetQueue(timestamp);
                    return CommandServerVerifyStateEnum.Success;
                }
            }
            timestamp = 0;
            return CommandServerVerifyStateEnum.Fail;
        }
    }
}
