using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
    /// <summary>
    [ServerNode(MethodIndexEnumType = typeof(GameNodeMethodEnum), IsAutoMethodIndex = false, IsMethodParameterCreator = true, IsLocalClient = true)]
    public interface IGameNode
    {
        void Clear();
        [ServerMethod(IsSnapshotMethod = true)]
        void AddMonster(Monster monster);
        void AddMonsters(Monster[] monsters);
        void SetSpeed(int id, int speed);
        void SetSpeeds(KeyValue<int, int>[] speeds);
    }
}
