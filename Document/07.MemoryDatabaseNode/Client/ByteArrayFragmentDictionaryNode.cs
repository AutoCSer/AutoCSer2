using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Dictionary node client example
    /// 字典节点客户端示例
    /// </summary>
    internal static class ByteArrayFragmentDictionaryNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayFragmentDictionaryNode<string>(nameof(ByteArrayFragmentDictionaryNode)));
        /// <summary>
        /// Dictionary node client example
        /// 字典节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNodeClientNode<string> node = nodeResult.Value.AutoCSerExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            #region byte[]
            byte[] data = new byte[] { 1, 2, 3, 4 };
            //byte[] supports implicit conversion to AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray
            //byte[] 支持隐式转换为 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray
            var boolResult = await node.Set("ByteArray", data);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            //Get byte[] using the TryGetValue method
            //获取 byte[] 使用 TryGetValue 方法
            var dataResult = await node.TryGetValue("ByteArray");
            if (!dataResult.IsSuccess || !AutoCSer.Common.SequenceEqual(data, dataResult.Value.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region string
            //string supports implicit conversion to AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray, which is essentially binary serialization to byte[]
            //string 支持隐式转换为 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray 本质是二进制序列化为 byte[]
            boolResult = await node.Set("String", "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            //Get the string using the TryGetString extension method
            //获取 string 使用 TryGetString 扩展方法
            var stringResult = await node.TryGetString("String");
            if (!stringResult.IsSuccess || stringResult.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region JSON mixed binary serialization
            Data.TestClass testData = new Data.TestClass { Int = 1, String = "AAA" };
            boolResult = await node.Set("JsonSerialize", AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.JsonSerialize(testData));
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            //Get the JSON mixed binary deserialized object using the TryGetJsonDeserialize extension method
            //获取 JSON 混杂二进制反序列化对象使用 TryGetJsonDeserialize 扩展方法
            var classResult = await node.TryGetJsonDeserialize<string, Data.TestClass>("JsonSerialize");
            if (!classResult.IsSuccess || classResult.Value?.String != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region Binary serialization
            boolResult = await node.Set("BinarySerialize", AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.BinarySerialize(testData));
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            //Get the binary deserialized object using the TryGetBinaryDeserialize extension method
            //获取二进制反序列化对象使用 TryGetBinaryDeserialize 扩展方法
            classResult = await node.TryGetBinaryDeserialize<string, Data.TestClass>("BinarySerialize");
            if (!classResult.IsSuccess || classResult.Value?.String != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            return true;
        }
    }
}
