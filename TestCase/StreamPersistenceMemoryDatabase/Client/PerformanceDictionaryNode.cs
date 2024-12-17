using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    /// <summary>
    /// 吞吐性能测试字典
    /// </summary>
    internal class PerformanceDictionaryNode : PerformanceClient
    {
        private IDictionaryNodeClientNode<int, int> node;
        internal PerformanceDictionaryNode() { }
        internal async Task Test(CommandClientConfig config, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IDictionaryNodeClientNode<int, int>> node = await client.GetOrCreateDictionaryNode<int, int>(typeof(IDictionaryNodeClientNode<int, int>).FullName, 0);
            if (!Program.Breakpoint(node)) return;
            this.node = node.Value;
            ResponseResult result = await this.node.Renew(maxTestCount);
            if (!Program.Breakpoint(result)) return;

            int taskCount = getTaskCount(config);
            testValue = reset(maxTestCount, true, taskCount);
            while (--taskCount >= 0) Set().NotWait();
            await wait(nameof(PerformanceDictionaryNode), nameof(Set));

            taskCount = getTaskCount(config);
            testValue = reset(maxTestCount, true, taskCount);
            while (--taskCount >= 0) TryGetValue().NotWait();
            await wait(nameof(PerformanceDictionaryNode), nameof(TryGetValue));

            result = await this.node.Renew(0);
            if (!Program.Breakpoint(result)) return;

            ResponseResult<bool> boolResult = await this.node.Set(0, 0);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
            }
        }
        internal async Task Set()
        {
            await Task.Yield();
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0) checkReturnValue(await node.Set(value, value + 1));
                else return;
            }
            while (true);
        }
        internal async Task TryGetValue()
        {
            await Task.Yield();
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0) checkReturnValue(value + 1, await node.TryGetValue(value));
                else return;
            }
            while (true);
        }
    }
}
