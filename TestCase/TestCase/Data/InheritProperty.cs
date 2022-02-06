using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 继承属性数据定义(引用类型外壳)
    /// </summary>
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    [AutoCSer.BinarySerialize(IsAnonymousFields = true)]
    internal class InheritProperty : Property
    {
        public bool Bool2 { get; set; }
        public byte Byte2 { get; set; }
        public sbyte SByte2 { get; set; }
        public short Short2 { get; set; }
        public ushort UShort2 { get; set; }
        public int Int2 { get; set; }
        public uint UInt2 { get; set; }
        public long Long2 { get; set; }
        public ulong ULong2 { get; set; }
        public DateTime DateTime2 { get; set; }
        public float Float2 { get; set; }
        public double Double2 { get; set; }
        public decimal Decimal2 { get; set; }
        public Guid Guid2 { get; set; }
        public char Char2 { get; set; }
        public string String2 { get; set; }
        public bool? BoolNull2 { get; set; }
        public byte? ByteNull2 { get; set; }
        public sbyte? SByteNull2 { get; set; }
        public short? ShortNull2 { get; set; }
        public ushort? UShortNull2 { get; set; }
        public int? IntNull2 { get; set; }
        public uint? UIntNull2 { get; set; }
        public long? LongNull2 { get; set; }
        public ulong? ULongNull2 { get; set; }
        public DateTime? DateTimeNull2 { get; set; }
        public float? FloatNull2 { get; set; }
        public double? DoubleNull2 { get; set; }
        public decimal? DecimalNull2 { get; set; }
        public Guid? GuidNull2 { get; set; }
        public char? CharNull2 { get; set; }
        public int[] Array2 { get; set; }
        public List<int> List2 { get; set; }
        public ByteEnum Enum2 { get; set; }
        public ByteFlagEnum FlagEnum2 { get; set; }
        public MemberClass Class2 { get; set; }
        public Dictionary<string, int> StringDictionary2 { get; set; }
        public Dictionary<int, string> IntDictionary2 { get; set; }
    }
}
