using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    internal static class GameNode
    {
        internal static async Task Test(LocalClient<ICustomServiceNodeLocalClientNode> client)
        {
            ResponseResult<IGameNodeLocalClientNode> node = await client.GetOrCreateNode<IGameNodeLocalClientNode>(typeof(IGameNodeLocalClientNode).FullName, client.ClientNode.CreateGameNode);
            if (!Program.Breakpoint(node)) return;
            IGameNodeLocalClientNode nodeValue = node.Value;
            ResponseResult result = await nodeValue.Clear();
            if (!Program.Breakpoint(result)) return;

            //https://www.zhihu.com/question/595091316
            int count = 10000;
            LeftArray<LocalServiceQueueNode<ResponseResult>> requests = new LeftArray<LocalServiceQueueNode<ResponseResult>>(count);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int id = 1; id <= count; ++id)
            {
                requests.Add(nodeValue.AddMonster(new Monster { id = id, pos = new Pos { x = 1, y = 1 }, speed = 20, type = 101 }));
            }
            foreach(var request in requests)
            {
                result = await request;
                if (!Program.Breakpoint(result)) return;
            }
            requests.Clear();
            for (int id = 1; id <= count; ++id)
            {
                requests.Add(nodeValue.SetSpeed(id, 30));
            }
            foreach (var request in requests)
            {
                result = await request;
                if (!Program.Breakpoint(result)) return;
            }
            stopwatch.Stop();
            Console.WriteLine($"{nameof(IGameNodeLocalClientNode)} pipeline {stopwatch.Elapsed.TotalSeconds}s");

            stopwatch.Restart();
            LeftArray<Monster> monsters = new LeftArray<Monster>(count);
            for (int id = 1; id <= count; ++id)
            {
                monsters.Add(new Monster { id = id, pos = new Pos { x = 1, y = 1 }, speed = 20, type = 101 });
            }
            result = await nodeValue.AddMonsters(monsters.ToArray());
            if (!Program.Breakpoint(result)) return;

            LeftArray<KeyValue<int, int>> speeds = new LeftArray<KeyValue<int, int>>(count);
            for (int id = 1; id <= count; ++id)
            {
                speeds.Add(new KeyValue<int, int>(id, 30));
            }
            result = await nodeValue.SetSpeeds(speeds.ToArray());
            if (!Program.Breakpoint(result)) return;

            stopwatch.Stop();
            Console.WriteLine($"{nameof(IGameNodeLocalClientNode)} array {stopwatch.Elapsed.TotalSeconds}s");

            completed();
        }
        private static void completed()
        {
            Console.WriteLine($"*{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name} Completed*");
        }
    }
}
