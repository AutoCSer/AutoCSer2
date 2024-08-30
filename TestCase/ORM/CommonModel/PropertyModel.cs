using AutoCSer.TestCase.Common.Data;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.CommonModel.TableModel
{
    /// <summary>
    /// 属性测试模型（二进制序列化不支持属性，需要配置序列化匿名字段以支持 RPC 传参）
    /// </summary>
    [AutoCSer.ORM.Model(MemberFilters = Metadata.MemberFiltersEnum.PublicInstance)]
    [AutoCSer.BinarySerialize(IsAnonymousFields = true)]
    public class PropertyModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [AutoCSer.ORM.String(PrimaryKeyType = AutoCSer.ORM.PrimaryKeyTypeEnum.PrimaryKey, Size = 32)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string Key { get; set; }

        /// <summary>
        /// 字符串默认为 nvarchar(max)
        /// </summary>
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string NVarCharMax { get; set; }
        /// <summary>
        /// nvarchar(10) 不允许空字符串
        /// </summary>
        [AutoCSer.ORM.String(Size = 10)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string NVarChar { get; set; }
        /// <summary>
        /// varchar(max)
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string VarCharMax { get; set; }
        /// <summary>
        /// varchar(10) 允许 null 值
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true, Size = 10, IsNullable = true)]
        public string VarChar { get; set; }

        [AutoCSer.FieldEquals.Ignore]
        public DateTime DateTime { get; set; }
        [AutoCSer.FieldEquals.Ignore]
        public DateTime? DateTimeNull { get; set; }
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.DateTime2)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime DateTime2 { get; set; }
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.SmallDateTime)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime SmallDateTime { get; set; }
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.Date)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan? TimeNull { get; set; }

        /// <summary>
        /// 默认为 decimal(18,2)
        /// </summary>
        public decimal Decimal { get; set; }
        /// <summary>
        /// decimal(38,4)
        /// </summary>
        [AutoCSer.ORM.Decimal(Precision = 38, Scale = 4)]
        public decimal? DecimalNull { get; set; }
        [AutoCSer.ORM.Money]
        public decimal Money { get; set; }
        [AutoCSer.ORM.Money(IsSmall = true)]
        public decimal SmallMoney { get; set; }
        [AutoCSer.ORM.Money]
        public decimal? MoneyNull { get; set; }
        [AutoCSer.ORM.Money(IsSmall = true)]
        public decimal? SmallMoneyNull { get; set; }

        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public Guid Guid { get; set; }
        public ByteEnum ByteEnum { get; set; }
        public ByteFlagEnum ByteFlagEnum { get; set; }
        public SByteEnum SByteEnum { get; set; }
        public SByteFlagEnum SByteFlagEnum { get; set; }
        public ShortEnum ShortEnum { get; set; }
        public ShortFlagEnum ShortFlagEnum { get; set; }
        public UShortEnum UShortEnum { get; set; }
        public UShortFlagEnum UShortFlagEnum { get; set; }
        public IntEnum IntEnum { get; set; }
        public IntFlagEnum IntFlagEnum { get; set; }
        public UIntEnum UIntEnum { get; set; }
        public UIntFlagEnum UIntFlagEnum { get; set; }
        public LongEnum LongEnum { get; set; }
        public LongFlagEnum LongFlagEnum { get; set; }
        public ULongEnum ULongEnum { get; set; }
        public ULongFlagEnum ULongFlagEnum { get; set; }

        public bool? BoolNull { get; set; }
        public byte? ByteNull { get; set; }
        public sbyte? SByteNull { get; set; }
        public short? ShortNull { get; set; }
        public ushort? UShortNull { get; set; }
        public int? IntNull { get; set; }
        public uint? UIntNull { get; set; }
        public long? LongNull { get; set; }
        public ulong? ULongNull { get; set; }
        public DateTimeOffset? DateTimeOffsetNull { get; set; }
        public float? FloatNull { get; set; }
        public double? DoubleNull { get; set; }
        public Guid? GuidNull { get; set; }
        public ByteEnum? ByteEnumNull { get; set; }
        public ByteFlagEnum? ByteFlagEnumNull { get; set; }
        public SByteEnum? SByteEnumNull { get; set; }
        public SByteFlagEnum? SByteFlagEnumNull { get; set; }
        public ShortEnum? ShortEnumNull { get; set; }
        public ShortFlagEnum? ShortFlagEnumNull { get; set; }
        public UShortEnum? UShortEnumNull { get; set; }
        public UShortFlagEnum? UShortFlagEnumNull { get; set; }
        public IntEnum? IntEnumNull { get; set; }
        public IntFlagEnum? IntFlagEnumNull { get; set; }
        public UIntEnum? UIntEnumNull { get; set; }
        public UIntFlagEnum? UIntFlagEnumNull { get; set; }
        public LongEnum? LongEnumNull { get; set; }
        public LongFlagEnum? LongFlagEnumNull { get; set; }
        public ULongEnum? ULongEnumNull { get; set; }
        public ULongFlagEnum? ULongFlagEnumNull { get; set; }
    }
}
