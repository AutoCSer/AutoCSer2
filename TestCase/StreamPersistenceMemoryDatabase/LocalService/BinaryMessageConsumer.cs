using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
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
    internal class BinaryMessageConsumer : LocalServiceBinaryMessageConsumer<TestClass>
    {
        internal BinaryMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<BinaryMessage<TestClass>> node) : base(client, node) { }
        protected override Task onMessage(TestClass message)
        {
            if (isCompleted) throw new IgnoreException();
            lock (messageLock) messages.Remove(message);
            return AutoCSer.Common.CompletedTask;
        }
        private static bool isCompleted;
        private static HashSet<TestClass> messages;
        private static readonly object messageLock = new object();
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client)
        {
            ResponseResult<IMessageNodeLocalClientNode<BinaryMessage<TestClass>>> node = await client.GetOrCreateNode<IMessageNodeLocalClientNode<BinaryMessage<TestClass>>>(typeof(IMessageNodeLocalClientNode<BinaryMessage<TestClass>>).FullName, (index, key, nodeInfo) => client.ClientNode.CreateBinaryMessageNode(index, key, nodeInfo, 1 << 10, 5, 1));
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;

            messages = new HashSet<TestClass>();
            for (char message = 'A'; message <= 'Z'; ++message) messages.Add(new TestClass { Int = message, String = message.ToString() });

            //new BinaryMessageConsumer(commandClient, node.Value).Start(1 << 10).NotWait();

            foreach (TestClass message in messages.getLeftArray())
            {
                result = await node.Value.AppendMessage(message);
                if (!Program.Breakpoint(result)) return;
            }

            isCompleted = false;
            using (BinaryMessageConsumer consumer = new BinaryMessageConsumer(client, node.Value))
            {
                consumer.Start(1 << 10).NotWait();

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
                result = await node.Value.AppendMessage(new TestClass { Int = '0', String = "0" });
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
