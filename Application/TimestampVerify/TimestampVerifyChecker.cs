﻿using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Increment the login timestamp checker
    /// 递增登录时间戳检查器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TimestampVerifyChecker
    {
        /// <summary>
        /// The initialization timestamp
        /// 初始化时间戳
        /// </summary>
        private static readonly long startTimestamp = AutoCSer.Date.GetTimestampByTicks(AutoCSer.Date.StartTime.Ticks - new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
        /// <summary>
        /// Get the current timestamp
        /// 获取当前时间戳
        /// </summary>
        public static long CurrentTimestamp { get { return startTimestamp + AutoCSer.Date.TimestampDifference; } }

        /// <summary>
        /// The timestamp of the last verification
        /// 最后一次验证时间戳
        /// </summary>
        private long lastTimestamp;
        /// <summary>
        /// The maximum verification time difference of the timestamp
        /// 时间戳最大验证时间差
        /// </summary>
        private long maxTimestampDifference;
        /// <summary>
        /// Increment the login timestamp checker
        /// 递增登录时间戳检查器
        /// </summary>
        /// <param name="maxSecondsDifference">Maximum time difference in seconds
        /// 最大时间差秒数</param>
        public TimestampVerifyChecker(byte maxSecondsDifference)
        {
            maxTimestampDifference = AutoCSer.Date.GetTimestampBySeconds(Math.Max((int)maxSecondsDifference, 1));
            lastTimestamp = CurrentTimestamp;
        }
        /// <summary>
        /// Detect the current timestamp
        /// 检测当前时间戳
        /// </summary>
        /// <param name="timestamp">The timestamp requested by the client
        /// 客户端请求的时间戳</param>
        /// <param name="serverTimestamp">The timestamp allocated by the server side
        /// 服务端分配的时间戳</param>
        /// <returns>The command service verifies the result status
        /// 命令服务验证结果状态</returns>
        public CommandServerVerifyStateEnum Check(ref long timestamp, ref long serverTimestamp)
        {
            if (serverTimestamp != 0) return timestamp == serverTimestamp ? CommandServerVerifyStateEnum.Success : CommandServerVerifyStateEnum.Fail;
            if (timestamp > lastTimestamp && timestamp < CurrentTimestamp + maxTimestampDifference)
            {
                serverTimestamp = timestamp;
                return CommandServerVerifyStateEnum.Success;
            }
            timestamp = serverTimestamp = System.Threading.Interlocked.Increment(ref lastTimestamp);
            return CommandServerVerifyStateEnum.Retry;
        }
        /// <summary>
        /// Detect the current timestamp
        /// 检测当前时间戳
        /// </summary>
        /// <param name="timestamp">The timestamp requested by the client
        /// 客户端请求的时间戳</param>
        /// <param name="serverTimestamp">The timestamp allocated by the server side
        /// 服务端分配的时间戳</param>
        /// <returns>The command service verifies the result status
        /// 命令服务验证结果状态</returns>
        internal CommandServerVerifyStateEnum CheckQueue(ref long timestamp, ref long serverTimestamp)
        {
            if (serverTimestamp != 0) return timestamp == serverTimestamp ? CommandServerVerifyStateEnum.Success : CommandServerVerifyStateEnum.Fail;
            if (timestamp > lastTimestamp && timestamp < CurrentTimestamp + maxTimestampDifference)
            {
                serverTimestamp = timestamp;
                return CommandServerVerifyStateEnum.Success;
            }
            serverTimestamp = ++lastTimestamp;
            timestamp = serverTimestamp;
            return CommandServerVerifyStateEnum.Retry;
        }
        /// <summary>
        /// Set the timestamp of the last verification
        /// 设置最后一次验证时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        public void Set(long timestamp)
        {
            do
            {
                long lastTimestamp = this.lastTimestamp;
                if (timestamp > lastTimestamp)
                {
                    if (System.Threading.Interlocked.CompareExchange(ref this.lastTimestamp, timestamp, lastTimestamp) == lastTimestamp) return;
                }
                else return;
            }
            while (true);
        }
        /// <summary>
        /// Set the timestamp of the last verification
        /// 设置最后一次验证时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetQueue(long timestamp)
        {
            if (timestamp > lastTimestamp) lastTimestamp = timestamp;
        }
        /// <summary>
        /// Get the available timestamp
        /// 获取可用时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimestamp()
        {
            long timestamp = CurrentTimestamp;
            do
            {
                long lastTimestamp = this.lastTimestamp;
                if (timestamp > lastTimestamp)
                {
                    if (System.Threading.Interlocked.CompareExchange(ref this.lastTimestamp, timestamp, lastTimestamp) == lastTimestamp) return timestamp;
                }
                else return System.Threading.Interlocked.Increment(ref this.lastTimestamp);
            }
            while (true);
        }

        ///// <summary>
        ///// 客户端创建套接字连接以后调用认证 API
        ///// </summary>
        ///// <param name="controller"></param>
        ///// <param name="verifyString">验证字符串</param>
        ///// <returns></returns>
        //public static CommandClientReturnValue<CommandServerVerifyStateEnum> Verify(CommandClientController controller, string verifyString)
        //{
        //    ITimestampVerifyClient client = (ITimestampVerifyClient)controller;
        //    long timestamp = CurrentTimestamp;
        //    bool isRetry = false;
        //    using (MD5 md5 = MD5.Create())
        //    {
        //        do
        //        {
        //            ulong randomPrefix = Random.Default.SecureNextULongNotZero();
        //            long lastTimestamp = timestamp;
        //            CommandClientReturnValue<CommandServerVerifyStateEnum> verifyState = client.Verify(randomPrefix, AutoCSer.Net.TimestampVerify.Md5(md5, verifyString, randomPrefix, timestamp), ref timestamp);
        //            if (verifyState.Value != CommandServerVerifyStateEnum.Retry
        //                || !verifyState.IsSuccess || isRetry) return verifyState;
        //            isRetry = true;
        //        }
        //        while (true);
        //    }
        //}
        /// <summary>
        /// After the client creates a socket connection, it calls the authentication API
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        /// <returns></returns>
        public static CommandClientReturnValue<CommandServerVerifyStateEnum> Verify(CommandClientController controller, string verifyString)
        {
            ITimestampVerifyClient client = (ITimestampVerifyClient)controller;
            long timestamp = CurrentTimestamp;
            bool isRetry = false;
            using (MD5 md5 = MD5.Create())
            {
                do
                {
                    ulong randomPrefix = Random.Default.SecureNextULongNotZero();
                    long lastTimestamp = timestamp;
                    CommandClientReturnValue<CommandServerVerifyStateEnum> verifyState = client.Verify(randomPrefix, AutoCSer.Net.TimestampVerify.Md5(md5, verifyString, randomPrefix, timestamp), ref timestamp);
                    if (verifyState.Value != CommandServerVerifyStateEnum.Retry
                        || !verifyState.IsSuccess || isRetry) return verifyState;
                    isRetry = true;
                }
                while (true);
            }
        }

    }
}
