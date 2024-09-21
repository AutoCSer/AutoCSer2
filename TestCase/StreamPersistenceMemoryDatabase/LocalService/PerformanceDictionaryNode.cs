using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// 吞吐性能测试字典
    /// </summary>
    internal class PerformanceDictionaryNode : PerformanceClient
    {
        private IDictionaryNodeLocalClientNode<int, int> node;
        internal PerformanceDictionaryNode() { }
        internal async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isPersistence)
        {
            ResponseResult<IDictionaryNodeLocalClientNode<int, int>> node = await client.GetOrCreateNode<IDictionaryNodeLocalClientNode<int, int>, int>(typeof(IDictionaryNodeLocalClientNode<int, int>).FullName + isPersistence.ToString(), 0, isPersistence ? (Func<NodeIndex, string, NodeInfo, int, LocalServiceQueueNode<ResponseResult<NodeIndex>>>)client.ClientNode.CreatePerformancePersistenceDictionaryNode : client.ClientNode.CreatePerformanceDictionaryNode);
            if (!Program.Breakpoint(node)) return;
            this.node = node.Value;
            ResponseResult result = await this.node.Renew(maxTestCount);
            if (!Program.Breakpoint(result)) return;

            int taskCount = getTaskCount();
            testValue = reset(maxTestCount, isPersistence, taskCount);
            while (--taskCount >= 0) Set().NotWait();
            await wait(nameof(PerformanceDictionaryNode), nameof(Set));

            taskCount = getTaskCount();
            testValue = reset(maxTestCount, isPersistence, taskCount);
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
            int success = 0, error = 0;
            await Task.Yield();
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0)
                {
                    ResponseResult<bool> result = await node.Set(value, value + 1);
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
            await Task.Yield();
            do
            {
                int value = System.Threading.Interlocked.Decrement(ref testValue);
                if (value >= 0)
                {
                    ResponseResult<ValueResult<int>> result = await node.TryGetValue(value);
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
