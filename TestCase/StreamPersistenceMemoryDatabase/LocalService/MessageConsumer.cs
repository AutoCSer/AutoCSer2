﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Log;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal class MessageConsumer : LocalServiceMessageConsumer<TestClassMessage>
    {
#if AOT
        private readonly ITestClassMessageNodeLocalClientNode node;
        internal MessageConsumer(LocalClient client, ITestClassMessageNodeLocalClientNode node) : base(client, (LocalClientNode)node, 1 << 10) 
        {
            this.node = node;
            start().NotWait();
        }
        /// <summary>
        /// Start receiving and processing messages
        /// 开始接收并处理消息
        /// </summary>
        /// <returns></returns>
        private async Task start()
        {
            try
            {
                keepCallback = await node.GetMessage(maxMessageCount, onMessage);
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
        }
        /// <summary>
        /// Message processing
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override async Task checkOnMessage(TestClassMessage message)
        {
            bool isMessage = false;
            try
            {
                isMessage = await onMessage(message);
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (isMessage) node.Completed(message.MessageIdeneity);
                else node.Failed(message.MessageIdeneity);
            }
        }
#else
        internal MessageConsumer(LocalClient client, IMessageNodeLocalClientNode<TestClassMessage> node) : base(client, node, 1 << 10) { }
#endif
        protected override Task<bool> onMessage(TestClassMessage message)
        {
            if (isCompleted) AutoCSer.Common.GetCompletedTask(false);
            lock (messageLock) messages.Remove(message);
            return AutoCSer.Common.GetCompletedTask(true);
        }
        private static bool isCompleted;
        private static HashSet<TestClassMessage> messages;
        private static readonly object messageLock = new object();
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client, bool isReadWriteQueue)
        {
#if AOT
            LocalResult<ITestClassMessageNodeLocalClientNode> node = await client.GetOrCreateNode<ITestClassMessageNodeLocalClientNode>(typeof(ITestClassMessageNodeLocalClientNode).FullName, (index, nodeKey, nodeInfo) => client.ClientNode.CreateTestClassMessageNode(index, nodeKey, nodeInfo, 1 << 10, 5, 1));
#else
            LocalResult<IMessageNodeLocalClientNode<TestClassMessage>> node = await client.GetOrCreateMessageNode<TestClassMessage>(typeof(IMessageNodeLocalClientNode<TestClassMessage>).FullName, 1 << 10, 5, 1);
#endif
            if (!Program.Breakpoint(node)) return;
            LocalResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;

            messages = new HashSet<TestClassMessage>();
            for (char message = 'A'; message <= 'Z'; ++message) messages.Add(new TestClassMessage { Int = message, String = message.ToString() });

            //new BinaryMessageConsumer(commandClient, node.Value).Start(1 << 10).NotWait();

            foreach (TestClassMessage message in messages.getLeftArray())
            {
                result = await node.Value.AppendMessage(message);
                if (!Program.Breakpoint(result)) return;
            }

            isCompleted = false;
            using (MessageConsumer consumer = new MessageConsumer(client, node.Value))
            {
                long timeout = Stopwatch.GetTimestamp() + AutoCSer.Date.GetTimestampBySeconds(10);
                while (messages.Count != 0)
                {
                    if (timeout < Stopwatch.GetTimestamp())
                    {
                        ConsoleWriteQueue.Breakpoint("*ERROR+TIMEOUT+ERROR*");
                        return;
                    }
                    await Task.Delay(1);
                }
                do
                {
                    LocalResult<int> intResult = await node.Value.GetTotalCount();
                    if (!Program.Breakpoint(intResult)) return;
                    if (intResult.Value == 0) break;
                    if (timeout < Stopwatch.GetTimestamp())
                    {
                        ConsoleWriteQueue.Breakpoint("*ERROR+TIMEOUT+ERROR*");
                        return;
                    }
                    await Task.Delay(1);
                }
                while (true);

                isCompleted = true;
                result = await node.Value.AppendMessage(new TestClassMessage { Int = '0', String = "0" });
                if (!Program.Breakpoint(result)) return;
            }

            completed(isReadWriteQueue);
        }
        private static void completed(bool isReadWriteQueue)
        {
            string readWriteQueue = isReadWriteQueue ? "ReadWriteQueue" : null;
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} {readWriteQueue} Completed*");
        }
    }
}
