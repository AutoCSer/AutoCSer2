using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Log;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal class ServerByteArrayMessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageConsumer
    {
        internal ServerByteArrayMessageConsumer(CommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node) : base(commandClient, node, 1 << 10) { }
        protected override Task<bool> onMessage(byte[] message)
        {
            if (isCompleted) AutoCSer.Common.GetCompletedTask(false);
            lock (messageLock) messages.Remove(message);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        private static bool isCompleted;
        private static HashSet<HashBytes> messages;
        private static readonly object messageLock = new object();
        internal static async Task Test(CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IMessageNodeClientNode<ServerByteArrayMessage>> node = await client.GetOrCreateServerByteArrayMessageNode(typeof(ServerByteArrayMessageConsumer).FullName, 1 << 10, 5, 1);
            if (!Program.Breakpoint(node)) return;
            ResponseResult result = await node.Value.Clear();
            if (!Program.Breakpoint(result)) return;

            messages = new HashSet<HashBytes>();
            for (char message = 'A'; message <= 'Z'; ++message) messages.Add(new byte[] { (byte)message });

            isCompleted = false;
            using (ServerByteArrayMessageConsumer consumer = new ServerByteArrayMessageConsumer(commandClient, node.Value))
            {
                foreach (HashBytes message in messages.AutoCSerExtensions<HashBytes>().GetArray())
                {
                    result = await node.Value.AppendMessage(((SubArray<byte>)message).GetArray());
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
                result = await node.Value.AppendMessage(new byte[] { (byte)'0' });
                if (!Program.Breakpoint(result)) return;
            }
            result = await node.Value.AppendMessage((byte[])null);
            if (!Program.Breakpoint(result)) return;
            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
