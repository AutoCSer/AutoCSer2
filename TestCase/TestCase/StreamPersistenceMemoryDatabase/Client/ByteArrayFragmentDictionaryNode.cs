using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// 字典客户端示例
    /// </summary>
    internal static class ByteArrayFragmentDictionaryNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayFragmentDictionaryNode<string>(nameof(ByteArrayFragmentDictionaryNode)));
        /// <summary>
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateByteArrayFragmentDictionaryNode<string>(nameof(ByteArrayFragmentDictionaryNode)));
        /// <summary>
        /// 字典客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// 字典客户端示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string>> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            #region byte[] 数据
            byte[] data = new byte[] { 1, 2, 3, 4 };
            var boolResult = await node.Set("ByteArray", data);//byte[] 支持隐式转换为 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var dataResult = await node.TryGetValue("ByteArray"); //获取 byte[] 使用 TryGetValue 方法
            if (!dataResult.IsSuccess || !AutoCSer.Common.SequenceEqual(data, dataResult.Value.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region string 数据
            boolResult = await node.Set("String", "AAA");//string 支持隐式转换为 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray 本质是二进制序列化为 byte[]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var stringResult = await node.TryGetString("String"); //获取 string 使用 TryGetString 扩展方法
            if (!stringResult.IsSuccess || stringResult.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region JSON 序列化
            Data.TestClass testData = new Data.TestClass { Int = 1, String = "AAA" };
            boolResult = await node.Set("JsonSerialize", AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.JsonSerialize(testData));
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var classResult = await node.TryGetJsonDeserialize<string, Data.TestClass>("JsonSerialize"); //获取 JSON 反序列化对象使用 TryGetJsonDeserialize 扩展方法
            if (!classResult.IsSuccess || classResult.Value?.String != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 二进制序列化
            boolResult = await node.Set("BinarySerialize", AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.BinarySerialize(testData));
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            classResult = await node.TryGetBinaryDeserialize<string, Data.TestClass>("BinarySerialize"); //获取二进制反序列化对象使用 TryGetBinaryDeserialize 扩展方法
            if (!classResult.IsSuccess || classResult.Value?.String != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            return true;
        }
    }
}
