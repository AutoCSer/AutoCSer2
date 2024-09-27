using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// 吞吐性能测试客户端
    /// </summary>
    internal abstract class PerformanceClient
    {
        /// <summary>
        /// 单并发线程每次循环获取任务数量
        /// </summary>
        internal const int LoopCountBit = 12;
        /// <summary>
        /// 单并发线程每次循环获取任务数量
        /// </summary>
        internal const int LoopCount = 1 << LoopCountBit;

        /// <summary>
        /// 当前测试值
        /// </summary>
        protected static int testValue;
        /// <summary>
        /// 最大测试请求次数
        /// </summary>
#if DEBUG
        protected const int maxTestCount = 1 << 10;
#else
        protected const int maxTestCount = 1 << 23;
#endif
        /// <summary>
        /// 测试验证位图
        /// </summary>
        private static readonly byte[] checkMap = new byte[(maxTestCount + 7) >> 3];
        /// <summary>
        /// 测试完毕等待锁
        /// </summary>
        private static readonly SemaphoreSlim waitLock = new SemaphoreSlim(0, 1);
        /// <summary>
        /// 计时开始时间错
        /// </summary>
        private static long startTimestamp;
        /// <summary>
        /// 测试请求次数
        /// </summary>
        private static int testCount;
        /// <summary>
        /// 测试并发数量
        /// </summary>
        private static int concurrent;
        /// <summary>
        /// 错误次数
        /// </summary>
        private static int errorCount;
        /// <summary>
        /// 未回调次数
        /// </summary>
        private static int callbackCount;
        /// <summary>
        /// 是否持久化
        /// </summary>
        private static bool isPersistence;
        /// <summary>
        /// 获取并发任务数量
        /// </summary>
        /// <returns></returns>
        protected static int getTaskCount()
        {
            return Math.Min(1 << 13, Math.Min(maxTestCount, Math.Max(maxTestCount >> 6, 4)));
        }
        /// <summary>
        /// 重置测试数据
        /// </summary>
        /// <param name="testCount"></param>
        /// <param name="concurrent"></param>
        /// <returns></returns>
        protected static int reset(int testCount, bool isPersistence, int concurrent = 1)
        {
            PerformanceClient.testCount = Math.Min(testCount, maxTestCount);
            if (PerformanceClient.testCount >= LoopCount) PerformanceClient.testCount &= (int.MaxValue ^ (LoopCount - 1));
            PerformanceClient.isPersistence = isPersistence;
            PerformanceClient.concurrent = concurrent;
            Array.Clear(checkMap, 0, checkMap.Length);
            errorCount = 0;
            callbackCount = testCount;
            startTimestamp = Stopwatch.GetTimestamp();
            return PerformanceClient.testCount;
        }
        /// <summary>
        /// 返回值验证
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        protected static void checkReturnValue(int success, int error)
        {
            Interlocked.Add(ref errorCount, error);
            if (Interlocked.Add(ref callbackCount, -(success + error)) == 0) waitLock.Release();
        }
        /// <summary>
        /// 检查未回调次数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected static bool checkCallbackCount()
        {
            if (--callbackCount != 0) return false;
            waitLock.Release();
            return true;
        }
        /// <summary>
        /// 测试验证位图
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static bool checkMapBit(int value)
        {
            if ((uint)value < (uint)testCount)
            {
                int index = value >> 3, mapValue = 1 << (value & 7);
                if (((checkMap[index] ^= (byte)mapValue) & mapValue) != 0) return checkCallbackCount();
            }
            ++errorCount;
            return checkCallbackCount();
        }
        /// <summary>
        /// 循环完成输出
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="waitMethodName"></param>
        /// <returns></returns>
        protected static async Task loopCompleted(string typeName, string methodName, string waitMethodName)
        {
            long milliseconds = AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp);
            string persistenceMessasge = isPersistence ? " + Persistence" : null;
            Console.WriteLine($"{typeName}.{methodName}{persistenceMessasge} Loop Completed {milliseconds.toString()}ms");
            await wait(typeName, waitMethodName);
        }
        /// <summary>
        /// 等待测试完成
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        protected static async Task wait(string typeName, string methodName)
        {
            await waitLock.WaitAsync();
            long milliseconds = AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp);
            string concurrentMessasge = concurrent == 1 ? null : $" {concurrent.toString()} Concurrent";
            string persistenceMessasge = isPersistence ? " + Persistence" : null;
            string countMessage = milliseconds != 0 ? (testCount >= milliseconds ? $"{(testCount / milliseconds).toString()}/ms" : $"{(testCount * 1000 / milliseconds).toString()}/s") : testCount.toString();
            Console.WriteLine($"{typeName}.{methodName}{persistenceMessasge} {concurrentMessasge} Completed {milliseconds.toString()}ms {countMessage}");
            if (errorCount != 0) ConsoleWriteQueue.WriteLine($"ERROR {errorCount}", ConsoleColor.Red);
            await Task.Delay(1000);
        }
    }
}
