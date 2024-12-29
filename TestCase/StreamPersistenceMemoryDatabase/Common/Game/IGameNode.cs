using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
    /// <summary>
    [ServerNode(IsAutoMethodIndex = false, IsMethodParameterCreator = true, IsLocalClient = true)]
    public partial interface IGameNode
    {
        void Clear();
        [ServerMethod(SnapshotMethodSort = 1)]
        void AddMonster(Monster monster);
        void AddMonsters(Monster[] monsters);
        void SetSpeed(int id, int speed);
        void SetSpeeds(KeyValue<int, int>[] speeds);
    }
}
