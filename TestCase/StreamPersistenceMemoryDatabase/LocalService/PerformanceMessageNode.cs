using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// 吞吐性能测试消息
    /// </summary>
    internal class PerformanceMessageNode : PerformanceClient
    {
        private IMessageNodeLocalClientNode<PerformanceMessage> node;
        private IMessageNodeLocalClientNode<PerformanceMessage> synchronousNode;
        internal PerformanceMessageNode() { }
        internal async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
            int taskCount = getTaskCount();
            LocalResult<IMessageNodeLocalClientNode<PerformanceMessage>> node = await client.GetOrCreateMessageNode<PerformanceMessage>(typeof(IMessageNodeLocalClientNode<PerformanceMessage>).FullName, taskCount, 5, 1);
            if (!Program.Breakpoint(node)) return;
            this.synchronousNode = LocalClientNode<IMessageNodeLocalClientNode<PerformanceMessage>>.GetSynchronousCallback(this.node = node.Value);
            LocalResult result = await this.node.Clear();
            if (!Program.Breakpoint(result)) return;
            string typeName = isReadWriteQueue ? $"{nameof(IReadWriteQueueService)}.{nameof(PerformanceMessageNode)}" : nameof(PerformanceMessageNode);

            using (IDisposable keepCallback = await this.synchronousNode.GetMessage(taskCount, onMessage))
            {
                await Task.WhenAll(getAppendMessageTask(true));
                await loopCompleted(typeName, nameof(AppendMessage), nameof(this.node.Completed));

                LocalResult<int> intResult = await this.node.GetCount();
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
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                int message = System.Threading.Interlocked.Decrement(ref testValue);
                if (message >= 0)
                {
                    LocalResult result = await synchronousNode.AppendMessage(new PerformanceMessage { Message = message });
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
        private void onMessage(LocalResult<PerformanceMessage> message)
        {
            if (message.IsSuccess)
            {
                if (message.Value != null)
                {
                    node.Completed(message.Value.MessageIdeneity);
                    checkMapBit(message.Value.Message);
                }
            }
            else checkCallbackCount();
        }
        //internal async Task GetMessage(LocalKeepCallback<PerformanceMessage> keepCallback)
        //{
        //    while (await keepCallback.MoveNext())
        //    {
        //        LocalResult<PerformanceMessage> message = keepCallback.Current;
        //        if (message.IsSuccess)
        //        {
        //            if (message.Value != null)
        //            {
        //                node.Completed(message.Value.MessageIdeneity);
        //                if (checkMapBit(message.Value.Message)) return;
        //            }
        //        }
        //        else
        //        {
        //            checkCallbackCount();
        //            return;
        //        }
        //    }
        //    //await foreach (LocalResult<PerformanceMessage> message in keepCallback.GetAsyncEnumerable())
        //    //{
        //    //    if (message.IsSuccess)
        //    //    {
        //    //        if (message.Value != null)
        //    //        {
        //    //            node.Completed(message.Value.MessageIdeneity);
        //    //            if (checkMapBit(message.Value.Message)) return;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        checkCallbackCount();
        //    //        return;
        //    //    }
        //    //}
        //}
    }
}
