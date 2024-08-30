using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public class TestClass : IEquatable<TestClass>
    {
        public int Int;
        public string String;

        public bool Equals(TestClass other)
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
