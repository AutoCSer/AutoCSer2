using AutoCSer.Extensions;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class ByteArrayStackNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            ResponseResult<IByteArrayStackNodeClientNode> node = await client.GetOrCreateByteArrayStackNode(typeof(IByteArrayStackNodeClientNode).FullName, 0);
            if (!Program.Breakpoint(node)) return;
            if (!await stringTest(node.Value)) return;
            if (!await byteArrayTest(node.Value)) return;
            if (!await jsonSerializeTest(node.Value)) return;
            if (!await binarySerializeTest(node.Value)) return;
            completed();
        }
        private static async Task<bool> stringTest(IByteArrayStackNodeClientNode node)
        {
            ResponseResult result = await node.Clear();
            if (!Program.Breakpoint(result)) return false;
            ResponseResult<int> intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            string value = TestClass.RandomString();
            result = await node.Push(value);
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<string> valueResult = await node.TryPeekString();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.TryPopString();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.Push(value);
            if (!Program.Breakpoint(result)) return false;
            return true;
        }
        private static async Task<bool> byteArrayTest(IByteArrayStackNodeClientNode node)
        {
            ResponseResult result = await node.Clear();
            if (!Program.Breakpoint(result)) return false;
            ResponseResult<int> intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            byte[] value = System.Text.Encoding.UTF8.GetBytes(TestClass.RandomString());
            result = await node.Push(value);
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            ResponseResult<ValueResult<byte[]>> valueResult = await node.TryPeek();
            if (!Program.Breakpoint(valueResult)) return false;
            if (!AutoCSer.Common.SequenceEqual(valueResult.Value.Value, value))
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.TryPop();
            if (!Program.Breakpoint(valueResult)) return false;
            if (!AutoCSer.Common.SequenceEqual(valueResult.Value.Value, value))
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.Push(value);
            if (!Program.Breakpoint(result)) return false;
            return true;
        }
        private static async Task<bool> jsonSerializeTest(IByteArrayStackNodeClientNode node)
        {
            ResponseResult result = await node.Clear();
            if (!Program.Breakpoint(result)) return false;
            ResponseResult<int> intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            TestClass value = new TestClass { String = TestClass.RandomString() };
            result = await node.PushJsonSerialize(value);
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<TestClass> valueResult = await node.TryPeekJsonDeserialize<TestClass>();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.TryPopJsonDeserialize<TestClass>();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.PushJsonSerialize(value);
            if (!Program.Breakpoint(result)) return false;
            return true;
        }
        private static async Task<bool> binarySerializeTest(IByteArrayStackNodeClientNode node)
        {
            ResponseResult result = await node.Clear();
            if (!Program.Breakpoint(result)) return false;
            ResponseResult<int> intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            TestClass value = new TestClass { String = TestClass.RandomString() };
            result = await node.PushBinarySerialize(value);
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 1)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<TestClass> valueResult = await node.TryPeekBinaryDeserialize<TestClass>();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.TryPopBinaryDeserialize<TestClass>();
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.PushBinarySerialize(value);
            if (!Program.Breakpoint(result)) return false;
            return true;
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
