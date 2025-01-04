using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
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
        private IMessageNodeClientNode<PerformanceMessage> synchronousNode;
        internal PerformanceMessageNode() { }
        internal async Task Test(CommandClientConfig config, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IMessageNodeClientNode<PerformanceMessage>> node = await client.GetOrCreateMessageNode<PerformanceMessage>(typeof(IMessageNodeClientNode<PerformanceMessage>).FullName, config.CommandQueueCount, 5, 1);
            if (!Program.Breakpoint(node)) return;
            synchronousNode = ClientNode<IMessageNodeClientNode<PerformanceMessage>>.GetSynchronousCallback(this.node = node.Value);
            ResponseResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;

            using (var keepCallback = await this.node.GetMessage(config.CommandQueueCount, onMessage))
            {
                await Task.WhenAll(getAppendMessageTask(config, true));
                await loopCompleted(nameof(PerformanceMessageNode), nameof(AppendMessage), nameof(this.node.Completed));
                keepCallback.Close();

                ResponseResult<int> intResult = await this.node.GetCount();
                if (!Program.Breakpoint(result)) return;
                if (intResult.Value != 0) Console.WriteLine($"未完成消息数量 {intResult.Value}");
                ResponseResult<int> intResult2 = await this.node.GetTotalCount();
                if (!Program.Breakpoint(intResult2)) return;
                if (intResult2.Value != intResult.Value) Console.WriteLine($"未完成失败消息数量 {intResult2.Value - intResult.Value}");
                result = await this.node.Clear();
                if (!Program.Breakpoint(result)) return;
            }
        }
        private IEnumerable<Task> getAppendMessageTask(CommandClientConfig config, bool isPersistence)
        {
            int taskCount = getTaskCount(config);
            testValue = reset(maxTestCount >> 1, isPersistence, taskCount);
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
                    ResponseResult result = await synchronousNode.AppendMessage(message);
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
        private void onMessage(ResponseResult<PerformanceMessage> message, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null)
                {
                    synchronousNode.Completed(message.Value.MessageIdeneity).Discard();
                    checkMapBit(message.Value.Message);
                }
            }
            else checkCallbackCount();
        }
        //internal async Task GetMessage(KeepCallbackResponse<PerformanceMessage> keepCallback)
        //{
        //    var command = keepCallback.EnumeratorCommand;
        //    if (command != null)
        //    {
        //        await using (command)
        //        {
        //            while(await command.MoveNext())
        //            {
        //                ResponseResult<PerformanceMessage> message = keepCallback.Current;
        //                if (message.IsSuccess)
        //                {
        //                    if (message.Value != null)
        //                    {
        //                        node.Completed(message.Value.MessageIdeneity).Discard();
        //                        if (checkMapBit(message.Value.Message)) return;
        //                    }
        //                }
        //                else
        //                {
        //                    checkCallbackCount();
        //                    return;
        //                }
        //            }
        //        }
        //        //await foreach (ResponseResult<PerformanceMessage> message in keepCallback.GetAsyncEnumerable())
        //        //{
        //        //    if (message.IsSuccess)
        //        //    {
        //        //        if (message.Value != null)
        //        //        {
        //        //            //await node.Completed(message.Value.MessageIdeneity);
        //        //            node.Completed(message.Value.MessageIdeneity).Discard();
        //        //            if (checkMapBit(message.Value.Message)) return;
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        checkCallbackCount();
        //        //        return;
        //        //    }
        //        //}
        //    }
        //    else ConsoleWriteQueue.Breakpoint("keepCallback.EnumeratorCommand is null");
        //}
    }
}
