using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// 吞吐性能测试消息
    /// </summary>
    internal class PerformanceMessageNode : PerformanceClient
    {
        private IMessageNodeLocalClientNode<PerformanceMessage> node;
        internal PerformanceMessageNode() { }
        internal async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isPersistence)
        {
            int taskCount = getTaskCount();
            ResponseResult<IMessageNodeLocalClientNode<PerformanceMessage>> node = await client.GetOrCreateNode<IMessageNodeLocalClientNode<PerformanceMessage>>(typeof(IMessageNodeLocalClientNode<PerformanceMessage>).FullName + isPersistence.ToString(), isPersistence ? (Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<ResponseResult<NodeIndex>>>)((nodeIndex, key, nodeInfo) => client.ClientNode.CreatePerformancePersistenceMessageNode(nodeIndex, key, nodeInfo, taskCount, 5, 1)) : (nodeIndex, key, nodeInfo) => client.ClientNode.CreatePerformanceMessageNode(nodeIndex, key, nodeInfo, taskCount, 5, 1));
            if (!Program.Breakpoint(node)) return;
            this.node = node.Value;
            ResponseResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            using (KeepCallbackResponse<PerformanceMessage> keepCallback = await this.node.GetMessage(taskCount))
            {
                GetMessage(keepCallback).NotWait();

                await Task.WhenAll(getAppendMessageTask(isPersistence));
                await loopCompleted(nameof(PerformanceMessageNode), nameof(AppendMessage), nameof(this.node.Completed));

                ResponseResult<int> intResult = await this.node.GetCount();
                if (!Program.Breakpoint(result)) return;
                if (intResult.Value != 0) Console.WriteLine($"未完成消息数量 {intResult.Value}");
                result = await this.node.Clear();
                if (!Program.Breakpoint(result)) return;
            }
        }
        private IEnumerable<Task> getAppendMessageTask(bool isPersistence)
        {
            int taskCount = getTaskCount();
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
            await foreach (ResponseResult<PerformanceMessage> message in keepCallback.GetAsyncEnumerable())
            {
                if (message.IsSuccess)
                {
                    if (message.Value != null)
                    {
                        node.Completed(message.Value.MessageIdeneity);
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
    }
}
