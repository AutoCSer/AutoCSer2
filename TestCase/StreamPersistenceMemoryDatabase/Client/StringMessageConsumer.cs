﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Log;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal class StringMessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.StringMessageConsumer
    {
        internal StringMessageConsumer(CommandClient commandClient, IMessageNodeClientNode<StringMessage> node) : base(commandClient, node) { }
        protected override Task onMessage(string message)
        {
            if (isCompleted) throw new IgnoreException();
            lock (messageLock) messages.Remove(message);
            return AutoCSer.Common.CompletedTask;
        }

        private static bool isCompleted;
        private static HashSet<string> messages;
        private static readonly object messageLock = new object();
        internal static async Task Test(CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IMessageNodeClientNode<StringMessage>> node = await client.GetOrCreateNode<IMessageNodeClientNode<StringMessage>>(typeof(IMessageNodeClientNode<StringMessage>).FullName, (index, key, nodeInfo) => client.ClientNode.CreateStringMessageNode(index, key, nodeInfo, 1 << 10, 5, 1));
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;

            messages = new HashSet<string>();
            for (char message = 'A'; message <= 'Z'; ++message) messages.Add(message.ToString());

            isCompleted = false;
            using (StringMessageConsumer consumer = new StringMessageConsumer(commandClient, node.Value))
            {
                consumer.Start(1 << 10).NotWait();

                foreach (string message in messages.getLeftArray())
                {
                    result = await node.Value.AppendMessage(message);
                    if (!Program.Breakpoint(result)) return;
                }

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
                    ResponseResult<int> intResult = await node.Value.GetTotalCount();
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
                result = await node.Value.AppendMessage("0");
                if (!Program.Breakpoint(result)) return;
            }
            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
