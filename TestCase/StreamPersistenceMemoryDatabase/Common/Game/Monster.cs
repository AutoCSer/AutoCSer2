using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    public partial class Monster : SnapshotCloneObject<Monster>
    {
        public int id;
        public int speed;
        public int type;
        public Pos pos;
    }
}
