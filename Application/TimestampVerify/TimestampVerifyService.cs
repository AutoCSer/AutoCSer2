using System;
using System.Security.Cryptography;
using AutoCSer.Net;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    public class TimestampVerifyService : ITimestampVerifyService, IDisposable
    {
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
        protected MD5CryptoServiceProvider md5;
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
        /// </summary>
        /// <param name="verifyString">服务认证验证字符串</param>
        /// <param name="maxSecondsDifference">最大时间差秒数，默认为 5</param>
        public TimestampVerifyService(string verifyString, byte maxSecondsDifference = 5)
        {
            this.verifyString = verifyString;
            timestampChecker = new TimestampVerifyChecker(maxSecondsDifference);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (md5 != null)
            {
                md5.Dispose();
                md5 = null;
            }
        }
        /// <summary>
        /// 尝试获取会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected virtual ITimestampVerifySession tryGetSessionObject(CommandServerSocket socket)
        {
            object sessionObject = socket.SessionObject;
            return sessionObject == null ? null : (ITimestampVerifySession)sessionObject;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected virtual ITimestampVerifySession createSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session =  new TimestampVerifySession();
            socket.SessionObject = session;
            return session;
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
        public virtual CommandServerVerifyState Verify(CommandServerSocket socket, CommandServerCallQueue queue, ulong randomPrefix, byte[] hashData, ref long timestamp)
        {
            if (hashData?.Length == 16)
            {
                ITimestampVerifySession session = tryGetSessionObject(socket);
                if (session == null)
                {
                    long serverTimestamp = 0;
                    switch(timestampChecker.Check(ref timestamp, ref serverTimestamp))
                    {
                        case CommandServerVerifyState.Success: break;
                        case CommandServerVerifyState.Retry:
                            session = createSessionObject(socket);
                            session.ServerTimestamp = serverTimestamp;
                            return CommandServerVerifyState.Retry;
                        default:
                            timestamp = 0;
                            return CommandServerVerifyState.Fail;
                    }
                }
                else
                {
                    long serverTimestamp = session.ServerTimestamp;
                    switch (timestampChecker.Check(ref timestamp, ref serverTimestamp))
                    {
                        case CommandServerVerifyState.Success: break;
                        case CommandServerVerifyState.Retry:
                            session.ServerTimestamp = serverTimestamp;
                            return CommandServerVerifyState.Retry;
                        default:
                            timestamp = 0;
                            return CommandServerVerifyState.Fail;
                    }
                }
                if (md5 == null) md5 = new MD5CryptoServiceProvider();
                if (AutoCSer.Net.TimestampVerify.Md5Equals(AutoCSer.Net.TimestampVerify.Md5(md5, verifyString, randomPrefix, timestamp), hashData) == 0)
                {
                    timestampChecker.Set(timestamp);
                    return CommandServerVerifyState.Success;
                }
            }
            timestamp = 0;
            return CommandServerVerifyState.Fail;
        }
    }
}
