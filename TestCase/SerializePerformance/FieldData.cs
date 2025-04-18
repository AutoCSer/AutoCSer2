using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 字段测试数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [AutoCSer.JsonSerialize(CheckLoopReference = false)]
    partial class FieldData
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
        public DateTime DateTime;
        public TimeSpan TimeSpan;
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
        public DateTime? DateTimeNull;
        public TimeSpan? TimeSpanNull;
        public Guid? GuidNull;
        public char? CharNull;
    }
}