using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    /// <summary>
    /// 吞吐性能测试消息
    /// </summary>
    internal class PerformanceMessageNode : PerformanceClient
    {
        private IMessageNodeClientNode<PerformanceMessage> node;
        internal PerformanceMessageNode() { }
        internal async Task Test(CommandClientConfig config, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client, bool isPersistence)
        {
            ResponseResult<IMessageNodeClientNode<PerformanceMessage>> node = await client.GetOrCreateNode<IMessageNodeClientNode<PerformanceMessage>>(typeof(IMessageNodeClientNode<PerformanceMessage>).FullName + isPersistence.ToString(), isPersistence ? (Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>>)((nodeIndex, key, nodeInfo) => client.ClientNode.CreatePerformancePersistenceMessageNode(nodeIndex, key, nodeInfo, config.CommandQueueCount, 5, 1)) : (nodeIndex, key, nodeInfo) => client.ClientNode.CreatePerformanceMessageNode(nodeIndex, key, nodeInfo, config.CommandQueueCount, 5, 1));
            if (!Program.Breakpoint(node)) return;
            this.node = node.Value;
            ResponseResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            using (KeepCallbackResponse<PerformanceMessage> keepCallback = await this.node.GetMessage(config.CommandQueueCount))
            {
                GetMessage(keepCallback).NotWait();

                await Task.WhenAll(getAppendMessageTask(config, isPersistence));
                await loopCompleted(nameof(PerformanceMessageNode), nameof(AppendMessage), nameof(this.node.Completed));

                ResponseResult<int> intResult = await this.node.GetCount();
                if (!Program.Breakpoint(result)) return;
                if (intResult.Value != 0) Console.WriteLine($"未完成消息数量 {intResult.Value}");
                result = await this.node.Clear();
                if (!Program.Breakpoint(result)) return;
            }
        }
        private IEnumerable<Task> getAppendMessageTask(CommandClientConfig config, bool isPersistence)
        {
            int taskCount = getTaskCount(config);
            testValue = reset(maxTestCount, isPersistence, taskCount);
            while (--taskCount >= 0) yield return AppendMessage();
        }
        internal async Task AppendMessage()
        {
            await Task.Yield();
            PerformanceMessage message = new PerformanceMessage();
            do
            {
                message.Message = System.Threading.Interlocked.Decrement(ref testValue);
                if (message.Message >= 0)
                {
                    ResponseResult result = await node.AppendMessage(message);
                    if (!result.IsSuccess)
                    {
                        Program.Breakpoint(result);
                        return;
                    }
                }
                else return;
            }
            while (true);
        }
        internal async Task GetMessage(KeepCallbackResponse<PerformanceMessage> keepCallback)
        {
            if (keepCallback.IsSuccess)
            {
                await foreach (ResponseResult<PerformanceMessage> message in keepCallback.GetAsyncEnumerable())
                {
                    if (message.IsSuccess)
                    {
                        if (message.Value != null)
                        {
                            await node.Completed(message.Value.MessageIdeneity);
                            if (checkMapBit(message.Value.Message)) return;
                        }
                    }
                    else
                    {
                        checkCallbackCount();
                        return;
                    }
                }
            }
            else ConsoleWriteQueue.Breakpoint("keepCallback.IsSuccess is Flase");
        }
    }
}
