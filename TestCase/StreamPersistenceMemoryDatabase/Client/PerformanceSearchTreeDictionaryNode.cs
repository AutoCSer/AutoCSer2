using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    /// <summary>
    /// 吞吐性能测试二叉搜索树字典
    /// </summary>
    internal class PerformanceSearchTreeDictionaryNode : PerformanceClient
    {
        private ISearchTreeDictionaryNodeClientNode<int, int> node;
        private ISearchTreeDictionaryNodeClientNode<int, int> synchronousNode;
        internal PerformanceSearchTreeDictionaryNode() { }
        internal async Task Test(CommandClientConfig config, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<ISearchTreeDictionaryNodeClientNode<int, int>> node = await client.GetOrCreateSearchTreeDictionaryNode<int, int>(typeof(ISearchTreeDictionaryNodeClientNode<int, int>).FullName);
            if (!Program.Breakpoint(node)) return;
            int taskCount = getTaskCount(config), testCount = AutoCSer.TestCase.Common.Config.IsRemote ? (maxTestCount >> 3) : maxTestCount;
            this.synchronousNode = ClientNode<ISearchTreeDictionaryNodeClientNode<int, int>>.GetSynchronousCallback(this.node = node.Value);
            ResponseResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            testValue = reset(testCount, true, taskCount);
            while (--taskCount >= 0) Set().NotWait();
            await wait(nameof(PerformanceSearchTreeDictionaryNode), nameof(Set));

            taskCount = getTaskCount(config);
            testValue = reset(testCount, true, taskCount);
            while (--taskCount >= 0) TryGetValue().NotWait();
            await wait(nameof(PerformanceSearchTreeDictionaryNode), nameof(TryGetValue));

            result = await this.node.Clear();
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
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0) checkReturnValue(await synchronousNode.Set(value, value + 1));
                else return;
            }
            while (true);
        }
        internal async Task TryGetValue()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
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
