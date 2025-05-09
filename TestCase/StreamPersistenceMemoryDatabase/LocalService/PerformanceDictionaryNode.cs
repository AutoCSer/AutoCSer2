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
#if AOT
        private IPerformanceDictionaryNodeLocalClientNode node;
#else
        private IDictionaryNodeLocalClientNode<int, int> node;
#endif
        internal PerformanceDictionaryNode() { }
        internal async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<IPerformanceDictionaryNodeLocalClientNode> node = await client.GetOrCreateNode<IPerformanceDictionaryNodeLocalClientNode>(typeof(IPerformanceDictionaryNodeLocalClientNode).FullName, (index, nodeKey, nodeInfo) => client.ClientNode.CreatePerformanceDictionaryNode(index, nodeKey, nodeInfo, 0, ReusableDictionaryGroupTypeEnum.HashIndex));
            if (!Program.Breakpoint(node)) return;
            this.node = LocalClientNode<IPerformanceDictionaryNodeLocalClientNode>.GetSynchronousCallback(node.Value);
#else
            LocalResult<IDictionaryNodeLocalClientNode<int, int>> node = await client.GetOrCreateDictionaryNode<int, int>(typeof(IDictionaryNodeLocalClientNode<int, int>).FullName, 0);
            if (!Program.Breakpoint(node)) return;
            this.node = LocalClientNode<IDictionaryNodeLocalClientNode<int, int>>.GetSynchronousCallback(node.Value);
#endif
            LocalResult result = await this.node.Renew(maxTestCount);
            if (!Program.Breakpoint(result)) return;

            string typeName = isReadWriteQueue ? $"ReadWriteQueue.{nameof(PerformanceDictionaryNode)}" : nameof(PerformanceDictionaryNode);
            int taskCount = getTaskCount();
            testValue = reset(maxTestCount, true, taskCount);
            while (--taskCount >= 0) Set().NotWait();
            await wait(typeName, nameof(Set));

            taskCount = getTaskCount();
            testValue = reset(maxTestCount, true, taskCount);
            while (--taskCount >= 0) TryGetValue().NotWait();
            await wait(typeName, nameof(TryGetValue));

            result = await this.node.Renew(0);
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
