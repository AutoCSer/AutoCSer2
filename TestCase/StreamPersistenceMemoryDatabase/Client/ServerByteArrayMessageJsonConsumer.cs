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
    internal class ServerByteArrayMessageJsonConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageJsonConsumer<TestClass>
    {
        internal ServerByteArrayMessageJsonConsumer(CommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node) : base(commandClient, node, 1 << 10) { }
        protected override Task<bool> onMessage(TestClass message)
        {
            if (isCompleted) AutoCSer.Common.GetCompletedTask(false);
            lock (messageLock) messages.Remove(message);
            return AutoCSer.Common.GetCompletedTask(true);
        }
        private static bool isCompleted;
        private static HashSet<TestClass> messages;
        private static readonly object messageLock = new object();
        internal static async Task Test(CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IMessageNodeClientNode<ServerByteArrayMessage>> node = await client.GetOrCreateServerByteArrayMessageNode(typeof(ServerByteArrayMessageJsonConsumer).FullName, 1 << 10, 5, 1);
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;

            messages = new HashSet<TestClass>();
            for (char message = 'A'; message <= 'Z'; ++message) messages.Add(new TestClass { Int = message, String = message.ToString() });

            isCompleted = false;
            using (ServerByteArrayMessageJsonConsumer consumer = new ServerByteArrayMessageJsonConsumer(commandClient, node.Value))
            {
                foreach (TestClass message in messages.getLeftArray())
                {
                    result = await node.Value.AppendMessage(ServerByteArrayMessage.JsonSerialize(message));
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
                result = await node.Value.AppendMessage(ServerByteArrayMessage.JsonSerialize(new TestClass { Int = '0', String = "0" }));
                if (!Program.Breakpoint(result)) return;
            }
            result = await node.Value.AppendMessage(ServerByteArrayMessage.JsonSerialize((TestClass)null));
            if (!Program.Breakpoint(result)) return;
            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
