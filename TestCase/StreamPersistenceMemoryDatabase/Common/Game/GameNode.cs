using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
    public class GameNode : IGameNode, ISnapshot<Monster>
    {
        private readonly Dictionary<int, Monster> monsters = DictionaryCreator.CreateInt<Monster>();
        public LeftArray<Monster> GetSnapshotArray()
        {
            return monsters.Values.getLeftArray();
        }
        public void Clear()
        {
            monsters.Clear();
        }
        [ServerMethod(IsSnapshotMethod = true)]
        public void AddMonster(Monster monster)
        {
            monsters[monster.id] = monster;
        }
        public void AddMonsters(Monster[] monsters)
        {
            foreach(Monster monster in monsters) monsters[monster.id] = monster;
        }
        public void SetSpeed(int id, int speed)
        {
            if (monsters.TryGetValue(id, out var monster)) monster.speed = speed;
        }
        public void SetSpeeds(KeyValue<int, int>[] speeds)
        {
            foreach (KeyValue<int, int> speed in speeds)
            {
                if (monsters.TryGetValue(speed.Key, out var monster)) monster.speed = speed.Value;
            }
        }
    }
}
