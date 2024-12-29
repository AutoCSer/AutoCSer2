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
        public int GetSnapshotCapacity(ref object customObject)
        {
            return monsters.Count;
        }
        public SnapshotResult<Monster> GetSnapshotResult(Monster[] snapshotArray, object customObject)
        {
            return new SnapshotResult<Monster>(snapshotArray, monsters.Values);
        }
        public void SetSnapshotResult(ref LeftArray<Monster> array, ref LeftArray<Monster> newArray) { }
        public void Clear()
        {
            monsters.Clear();
        }
        [ServerMethod(SnapshotMethodSort = 1)]
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
