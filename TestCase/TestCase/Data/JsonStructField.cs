using AutoCSer.TestCase.Common.Data;
using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.Data
{
    /// <summary>
    ///  JSON 混杂二进制序列化
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
#endif
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    internal partial struct JsonStructField
    {
        public bool Bool;
        public byte Byte;
        public sbyte SByte;
        public short Short;
        public ushort UShort;
        public int Int;
        public uint UInt;
        public long Long;
        public ulong ULong;
        //public Int128 Int128;
        //public UInt128 UInt128;
        public DateTime DateTime;
        public TimeSpan TimeSpan;
        //public Half Half;
        public float Float;
        public double Double;
        public decimal Decimal;
        public Guid Guid;
        public char Char;
        public string String;
        public bool? BoolNull;
        public byte? ByteNull;
        public sbyte? SByteNull;
        public short? ShortNull;
        public ushort? UShortNull;
        public int? IntNull;
        public uint? UIntNull;
        public long? LongNull;
        public ulong? ULongNull;
        //public Int128? Int128Null;
        //public UInt128? UInt128Null;
        public DateTime? DateTimeNull;
        public TimeSpan? TimeSpanNull;
        //public Half? HalfNull;
        public float? FloatNull;
        public double? DoubleNull;
        public decimal? DecimalNull;
        public Guid? GuidNull;
        public char? CharNull;
        public int[] Array;
        public List<int> List;
        public ByteEnum Enum;
        public ByteFlagEnum FlagEnum;
        public MemberClass Class;
        public Dictionary<string, int> StringDictionary;
        public Dictionary<int, string> IntDictionary;
    }
}
