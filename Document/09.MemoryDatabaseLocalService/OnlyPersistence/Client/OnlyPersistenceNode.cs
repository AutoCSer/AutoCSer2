using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.OnlyPersistence.Client
{
    /// <summary>
    /// Example of an archive-only data client
    /// 仅存档数据节点客户端示例
    /// </summary>
    internal static class OnlyPersistenceNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IOnlyPersistenceNodeLocalClientNode<Data.TestClass>> nodeCache = OnlyPersistence.ServiceConfig.Client.CreateNode(client => client.GetOrCreateOnlyPersistenceNode<Data.TestClass>(nameof(OnlyPersistenceNode)));
        /// <summary>
        /// Example of an archive-only data client
        /// 仅存档数据节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IOnlyPersistenceNodeLocalClientNode<Data.TestClass> node = nodeResult.Value.AutoCSerExtensions().NotNull();
            var result = await node.Save(AutoCSer.RandomObject.Creator<Data.TestClass>.CreateNotNull());
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            node.SaveSendOnly(AutoCSer.RandomObject.Creator<Data.TestClass>.CreateNotNull());
            return true;
        }
    }
}
