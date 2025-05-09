using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
#endif
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    public partial class Monster : SnapshotCloneObject<Monster>
    {
        public int id;
        public int speed;
        public int type;
        public Pos pos;
    }
}
