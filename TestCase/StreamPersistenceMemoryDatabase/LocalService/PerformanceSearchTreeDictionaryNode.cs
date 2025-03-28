using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// 吞吐性能测试二叉搜索树字典
    /// </summary>
    internal class PerformanceSearchTreeDictionaryNode : PerformanceClient
    {
        private ISearchTreeDictionaryNodeLocalClientNode<int, int> node;
        internal PerformanceSearchTreeDictionaryNode() { }
        internal async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
            LocalResult<ISearchTreeDictionaryNodeLocalClientNode<int, int>> node = await client.GetOrCreateSearchTreeDictionaryNode<int, int>(typeof(ISearchTreeDictionaryNodeLocalClientNode<int, int>).FullName);
            if (!Program.Breakpoint(node)) return;
            this.node = LocalClientNode<ISearchTreeDictionaryNodeLocalClientNode<int, int>>.GetSynchronousCallback(node.Value);
            LocalResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            string typeName = isReadWriteQueue ? $"{nameof(IReadWriteQueueService)}.{nameof(PerformanceSearchTreeDictionaryNode)}" : nameof(PerformanceSearchTreeDictionaryNode);
            int taskCount = getTaskCount();
            testValue = reset(maxTestCount >> 1, true, taskCount);
            while (--taskCount >= 0) Set().NotWait();
            await wait(typeName, nameof(Set));

            taskCount = getTaskCount();
            testValue = reset(maxTestCount >> 1, true, taskCount);
            while (--taskCount >= 0) TryGetValue().NotWait();
            await wait(typeName, nameof(TryGetValue));

            result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            LocalResult<bool> boolResult = await this.node.Set(0, 0);
            if (!Program.Breakpoint(boolResult)) return;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
            }
        }
        internal async Task Set()
        {
            int success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0)
                {
                    LocalResult<bool> result = await node.Set(value, value + 1);
                    if (result.IsSuccess && result.Value) ++success;
                    else ++error;
                }
                else
                {
                    checkReturnValue(success, error);
                    return;
                }
            }
            while (true);
        }
        internal async Task TryGetValue()
        {
            int success = 0, error = 0;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0)
                {
                    LocalResult<ValueResult<int>> result = await node.TryGetValue(value);
                    if (result.IsSuccess && result.Value.IsValue && result.Value.Value == value + 1) ++success;
                    else ++error;
                }
                else
                {
                    checkReturnValue(success, error);
                    return;
                }
            }
            while (true);
        }
    }
}
