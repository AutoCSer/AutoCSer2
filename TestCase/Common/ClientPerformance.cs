﻿using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 测试客户端
    /// </summary>
    public abstract class ClientPerformance
    {
        /// <summary>
        /// 单并发线程每次循环获取任务数量
        /// </summary>
        public static readonly int LoopCountBit = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsRemote ? 6 : 12;
        /// <summary>
        /// 单并发线程每次循环获取任务数量
        /// </summary>
        public static readonly int LoopCount = 1 << LoopCountBit;
        /// <summary>
        /// 服务端同步最大测试请求次数
        /// </summary>
        protected static readonly int maxTestCount = 1 << (AutoCSer.TestCase.Common.JsonFileConfig.Default.IsRemote ? 24 : 26);
        /// <summary>
        /// 当前测试随机左值
        /// </summary>
        public static int Left;
        /// <summary>
        /// 测试验证位图
        /// </summary>
        private static readonly byte[] checkMap = new byte[(maxTestCount + 7) >> 3];
        /// <summary>
        /// 错误次数
        /// </summary>
        private static int errorCount;
        /// <summary>
        /// 未回调次数
        /// </summary>
        private static int callbackCount;
        /// <summary>
        /// 计时开始时间戳
        /// </summary>
        private static long startTimestamp;
        /// <summary>
        /// 测试完毕等待锁
        /// </summary>
        private static readonly SemaphoreSlim waitLock = new SemaphoreSlim(0, 1);
        /// <summary>
        /// 当前测试客户端
        /// </summary>
        private static CommandClient commandClient;
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int testCount;
        /// <summary>
        /// 测试并发数量
        /// </summary>
        protected static int concurrent;
        /// <summary>
        /// 重置测试数据
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="testCount"></param>
        /// <param name="concurrent"></param>
        /// <returns></returns>
        public static int Reset(CommandClient commandClient, int testCount, int concurrent = 1)
        {
            ClientPerformance.commandClient = commandClient;
            ClientPerformance.testCount = Math.Min(testCount, maxTestCount) & (int.MaxValue ^ (LoopCount - 1));
            ClientPerformance.concurrent = concurrent;
            Array.Clear(checkMap, 0, checkMap.Length);
            errorCount = 0;
            callbackCount = testCount;
            startTimestamp = Stopwatch.GetTimestamp();
            return ClientPerformance.testCount;
        }
        /// <summary>
        /// 测试回调验证
        /// </summary>
        /// <param name="value"></param>
        public static void CheckSynchronous(int value)
        {
            int right = value - Left;
            if ((uint)right < (uint)testCount)
            {
                int index = right >> 3, mapValue = 1 << (right & 7);
                if (((checkMap[index] ^= (byte)mapValue) & mapValue) != 0)
                {
                    if (--callbackCount == 0) waitLock.Release();
                    return;
                }
            }
            ++errorCount;
            if (--callbackCount == 0) waitLock.Release();
        }
        /// <summary>
        /// 测试回调验证
        /// </summary>
        /// <param name="value"></param>
        public static void CheckSynchronous(CommandClientReturnValue<int> value)
        {
            if (value.IsSuccess)
            {
                CheckSynchronous(value.Value);
                return;
            }
            Console.WriteLine(value.ReturnType);
            ++errorCount;
            if (--callbackCount == 0) waitLock.Release();
        }
        /// <summary>
        /// 测试回调验证委托
        /// </summary>
        public static readonly Action<CommandClientReturnValue<int>> CheckSynchronousHandle = CheckSynchronous;
        /// <summary>
        /// 测试回调验证
        /// </summary>
        /// <param name="value"></param>
        private static void checkSynchronous(CommandClientReturnValue<int> value, KeepCallbackCommand keepCallbackCommand)
        {
            CheckSynchronous(value);
        }
        /// <summary>
        /// 测试回调验证委托
        /// </summary>
        public static readonly Action<CommandClientReturnValue<int>, KeepCallbackCommand> CheckSynchronousKeepCallbackHandle = checkSynchronous;
        ///// <summary>
        ///// 测试随机回调验证锁
        ///// </summary>
        //private static AutoCSer.Threading.SpinLock checkLock;
        /// <summary>
        /// 测试随机回调验证
        /// </summary>
        /// <param name="value"></param>
        public static void CheckLock(CommandClientReturnValue<int> value)
        {
            if (value.IsSuccess)
            {
                if (Interlocked.Decrement(ref callbackCount) == 0) waitLock.Release();
                return;
                //int right = value.Value - Left;
                //if ((uint)right < (uint)testCount)
                //{
                //    int index = right >> 3, mapValue = 1 << (right & 7);
                //    checkLock.EnterYield();
                //    int checkValue = (checkMap[index] ^= (byte)mapValue) & mapValue;
                //    checkLock.Exit();
                //    if (checkValue != 0)
                //    {
                //        if (Interlocked.Decrement(ref callbackCount) == 0) waitLock.Release();
                //        return;
                //    }
                //}
            }
            Interlocked.Increment(ref errorCount);
            if (Interlocked.Decrement(ref callbackCount) == 0) waitLock.Release();
        }
        /// <summary>
        /// 单个并发测试完成
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public static void CheckLock(int success, int error)
        {
            Interlocked.Add(ref errorCount, error);
            int count = success + error;
            if (count != 0)
            {
                if (Interlocked.Add(ref callbackCount, -count) == 0) waitLock.Release();
            }
        }
        /// <summary>
        /// 循环完成输出
        /// </summary>
        /// <param name="clientTypeName">客户端测试类型名称</param>
        /// <param name="serverMethodName">调用服务端方法名称</param>
        /// <returns></returns>
        public static async Task LoopCompleted(string clientTypeName, string serverMethodName)
        {
            long milliseconds = AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp);
            Console.WriteLine($"{clientTypeName}+Server.{serverMethodName} Loop Completed {testCount.toString()}/{milliseconds.toString()}ms");
            await Wait(clientTypeName, serverMethodName);
        }
        /// <summary>
        /// 等待测试完成
        /// </summary>
        /// <param name="clientTypeName">客户端测试类型名称</param>
        /// <param name="serverMethodName">调用服务端方法名称</param>
        /// <returns></returns>
        public static async Task Wait(string clientTypeName, string serverMethodName)
        {
            await waitLock.WaitAsync();
            long milliseconds = AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp);
            string concurrentMessasge = concurrent == 1 ? null : $" {concurrent.toString()} Concurrent";
            Console.WriteLine($"Client[{clientTypeName}]+Server[{serverMethodName}]{concurrentMessasge} Completed {testCount.toString()}/{milliseconds.toString()}ms = {(testCount / milliseconds).toString()}/ms");
            if (errorCount != 0) ConsoleWriteQueue.WriteLine($"ERROR {errorCount}", ConsoleColor.Red);
            await Task.Delay(1000);
        }
    }
}
