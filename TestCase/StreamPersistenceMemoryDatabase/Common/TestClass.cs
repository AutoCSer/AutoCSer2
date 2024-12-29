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

        public const string String1 = "A";
        public const string String2 = "BB";
        public const string String3 = "CCC";
        public const string String4 = "DDDD";
        public const string String5 = "EEEEE";
        public const string String6 = "FFFFFF";
        public const string String7 = "GGGGGGG";
        public static string RandomString()
        {
            int size = AutoCSer.Random.Default.NextByte() & 15;
            switch (size)
            {
                case 0: return String1;
                case 1: return String2;
                case 2: return String3;
                case 3: return String4;
                case 4: return String5;
                case 5: return String6;
                case 6: return String7;
                default: return new string((char)('A' + size), size);
            }
        }
    }
}
