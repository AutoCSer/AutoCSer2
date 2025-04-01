using AutoCSer.TestCase.Common.Data;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 属性数据定义(值类型外壳)
    /// </summary>
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    internal struct StructProperty
    {
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        //public Int128 Int128 { get; set; }
        //public UInt128 UInt128 { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        //public Half Half { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public Guid Guid { get; set; }
        public char Char { get; set; }
        public string String { get; set; }
        public bool? BoolNull { get; set; }
        public byte? ByteNull { get; set; }
        public sbyte? SByteNull { get; set; }
        public short? ShortNull { get; set; }
        public ushort? UShortNull { get; set; }
        public int? IntNull { get; set; }
        public uint? UIntNull { get; set; }
        public long? LongNull { get; set; }
        public ulong? ULongNull { get; set; }
        //public Int128? Int128Null { get; set; }
        //public UInt128? UInt128Null { get; set; }
        public DateTime? DateTimeNull { get; set; }
        public TimeSpan? TimeSpanNull { get; set; }
        //public Half? HalfNull { get; set; }
        public float? FloatNull { get; set; }
        public double? DoubleNull { get; set; }
        public decimal? DecimalNull { get; set; }
        public Guid? GuidNull { get; set; }
        public char? CharNull { get; set; }
        public int[] Array { get; set; }
        public List<int> List { get; set; }
        public ByteEnum Enum { get; set; }
        public ByteFlagEnum FlagEnum { get; set; }
        public MemberClass Class { get; set; }
        public Dictionary<string, int> StringDictionary { get; set; }
        public Dictionary<int, string> IntDictionary { get; set; }
    }
}
