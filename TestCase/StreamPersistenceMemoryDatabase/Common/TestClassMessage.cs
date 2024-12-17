using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public class TestClassMessage : Message<TestClassMessage>, IEquatable<TestClassMessage>
    {
        public int Int;
        public string String;

        public bool Equals(TestClassMessage other)
        {
            return Int == other.Int && String == other.String;
        }
        public override bool Equals(object obj)
        {
            return Equals((TestClass)obj);
        }
        public override int GetHashCode()
        {
            return Int ^ String.GetHashCode();
        }
    }
}
