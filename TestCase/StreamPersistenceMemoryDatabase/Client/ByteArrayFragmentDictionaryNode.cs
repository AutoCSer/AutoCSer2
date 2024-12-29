using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class ByteArrayFragmentDictionaryNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {

            ResponseResult<IByteArrayFragmentDictionaryNodeClientNode<string>> node = await client.GetOrCreateByteArrayFragmentDictionaryNode<string>(typeof(IByteArrayFragmentDictionaryNodeClientNode<string>).FullName);

            if (!Program.Breakpoint(node)) return;
            if (!await stringTest(node.Value)) return;
            if (!await byteArrayTest(node.Value)) return;
            if (!await jsonSerializeTest(node.Value)) return;
            if (!await binarySerializeTest(node.Value)) return;
            completed();
        }
        private static async Task<bool> stringTest(IByteArrayFragmentDictionaryNodeClientNode<string> node)
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
            string key = TestClass.RandomString(), value = TestClass.RandomString();
            ResponseResult<bool> boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<string> valueResult = await node.TryGetString(key);
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value != value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.ContainsKey(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.ClearArray();
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Remove(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.GetRemoveString(key);
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
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            return true;
        }
        private static async Task<bool> byteArrayTest(IByteArrayFragmentDictionaryNodeClientNode<string> node)
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
            string key = TestClass.RandomString();
            byte[] value = System.Text.Encoding.UTF8.GetBytes(TestClass.RandomString());
            ResponseResult<bool> boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            ResponseResult<ValueResult<byte[]>> valueResult = await node.TryGetValue(key);
            if (!Program.Breakpoint(valueResult)) return false;
            if (!AutoCSer.Common.SequenceEqual(valueResult.Value.Value, value))
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.ContainsKey(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.ClearArray();
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Remove(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAdd(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.GetRemove(key);
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
            boolResult = await node.Set(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            return true;
        }
        private static async Task<bool> binarySerializeTest(IByteArrayFragmentDictionaryNodeClientNode<string> node)
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
            string key = TestClass.RandomString();
            TestClass value = new TestClass { String = TestClass.RandomString() };
            ResponseResult<bool> boolResult = await node.TryAddBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAddBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<TestClass> valueResult = await node.TryGetBinaryDeserialize<string, TestClass>(key);
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.ContainsKey(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.ClearArray();
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.SetBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Remove(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAddBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.SetBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.GetRemoveBinaryDeserialize<string, TestClass>(key);
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
            boolResult = await node.SetBinarySerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            return true;
        }
        private static async Task<bool> jsonSerializeTest(IByteArrayFragmentDictionaryNodeClientNode<string> node)
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
            string key = TestClass.RandomString();
            TestClass value = new TestClass { String = TestClass.RandomString() };
            ResponseResult<bool> boolResult = await node.TryAddJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAddJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            ResponseValueResult<TestClass> valueResult = await node.TryGetJsonDeserialize<string, TestClass>(key);
            if (!Program.Breakpoint(valueResult)) return false;
            if (valueResult.Value.String != value.String)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{valueResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.ContainsKey(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            result = await node.ClearArray();
            if (!Program.Breakpoint(result)) return false;
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.SetJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.Remove(key);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value != 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.TryAddJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            intResult = await node.Count();
            if (!Program.Breakpoint(intResult)) return false;
            if (intResult.Value == 0)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{intResult.Value}+ERROR*");
                return false;
            }
            boolResult = await node.SetJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            valueResult = await node.GetRemoveJsonDeserialize<string, TestClass>(key);
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
            boolResult = await node.SetJsonSerialize(key, value);
            if (!Program.Breakpoint(boolResult)) return false;
            if (!boolResult.Value)
            {
                ConsoleWriteQueue.Breakpoint($"*ERROR+{boolResult.Value}+ERROR*");
                return false;
            }
            return true;
        }

        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
