﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient
{
    internal static class ManyHashBitMapClientFilterNode
    {
        internal static async Task Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<ICustomServiceNodeClientNode> client)
        {
            AutoCSer.Algorithm.ManyHashBitMapCapacity capacity = new Algorithm.ManyHashBitMapCapacity(1 << 10);
            ResponseResult<IManyHashBitMapClientFilterNodeClientNode> node = await client.GetOrCreateManyHashBitMapClientFilterNode(typeof(IManyHashBitMapClientFilterNodeClientNode).FullName, capacity.GetHashCapacity());
            if (!Program.Breakpoint(node)) return;

            var dataResult = await node.Value.GetData();
            if (!Program.Breakpoint(dataResult)) return;
            foreach (uint hashCode in ManyHashBitMapClientFilter.GetHashCode4(TestClass.String7))
            {
                var result = await node.Value.SetBit(dataResult.Value.GetBitByHashCode(hashCode));
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
