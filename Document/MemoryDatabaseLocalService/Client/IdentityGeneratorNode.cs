﻿using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// 64 位自增 ID 生成客户端示例
    /// </summary>
    internal static class IdentityGeneratorNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeLocalClientNode> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateIdentityGeneratorNode(nameof(IdentityGeneratorNode)));
        /// <summary>
        /// 64 位自增 ID 生成客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeLocalClientNode node = nodeResult.Value.notNull();
            var valueResult = await node.Next();
            if (!valueResult.IsSuccess && valueResult.Value > 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var fragmentResult = await node.NextFragment(16);
            if (!fragmentResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}