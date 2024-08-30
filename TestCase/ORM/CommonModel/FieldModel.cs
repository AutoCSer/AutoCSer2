using AutoCSer.TestCase.Common.Data;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.CommonModel.TableModel
{
    /// <summary>
    /// 字段测试模型（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参数据配置二进制序列化规则）
    /// </summary>
    [AutoCSer.BinarySerialize]
    public class FieldModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [AutoCSer.ORM.Member(PrimaryKeyType = AutoCSer.ORM.PrimaryKeyTypeEnum.PrimaryKey)]
        public long Key;

        /// <summary>
        /// 字符串默认为 nvarchar(max)
        /// </summary>
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string NVarCharMax;
        /// <summary>
        /// nvarchar(10) 不允许空字符串
        /// </summary>
        [AutoCSer.ORM.String(Size = 10)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string NVarChar;
        /// <summary>
        /// varchar(max)
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public string VarCharMax;
        /// <summary>
        /// varchar(10) 允许 null 值
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true, Size = 10, IsNullable = true)]
        public string VarChar;

        [AutoCSer.FieldEquals.Ignore]
        public DateTime DateTime;
        [AutoCSer.FieldEquals.Ignore]
        public DateTime? DateTimeNull;
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.DateTime2)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime DateTime2;
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.SmallDateTime)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime SmallDateTime;
        [AutoCSer.ORM.DateTime(Type = AutoCSer.ORM.DateTimeTypeEnum.Date)]
        [AutoCSer.FieldEquals.Ignore]
        public DateTime Date;
        public TimeSpan Time;
        public TimeSpan? TimeNull;

        /// <summary>
        /// 默认为 decimal(18,2)
        /// </summary>
        public decimal Decimal;
        /// <summary>
        /// decimal(38,4)
        /// </summary>
        [AutoCSer.ORM.Decimal(Precision = 38, Scale = 4)]
        public decimal? DecimalNull;
        [AutoCSer.ORM.Money]
        public decimal Money;
        [AutoCSer.ORM.Money(IsSmall = true)]
        public decimal SmallMoney;
        [AutoCSer.ORM.Money]
        public decimal? MoneyNull;
        [AutoCSer.ORM.Money(IsSmall = true)]
        public decimal? SmallMoneyNull;

        public bool Bool;
        public byte Byte;
        public sbyte SByte;
        public short Short;
        public ushort UShort;
        public int Int;
        public uint UInt;
        public long Long;
        public ulong ULong;
        public DateTimeOffset DateTimeOffset;
        public float Float;
        public double Double;
        public Guid Guid;
        public ByteEnum ByteEnum;
        public ByteFlagEnum ByteFlagEnum;
        public SByteEnum SByteEnum;
        public SByteFlagEnum SByteFlagEnum;
        public ShortEnum ShortEnum;
        public ShortFlagEnum ShortFlagEnum;
        public UShortEnum UShortEnum;
        public UShortFlagEnum UShortFlagEnum;
        public IntEnum IntEnum;
        public IntFlagEnum IntFlagEnum;
        public UIntEnum UIntEnum;
        public UIntFlagEnum UIntFlagEnum;
        public LongEnum LongEnum;
        public LongFlagEnum LongFlagEnum;
        public ULongEnum ULongEnum;
        public ULongFlagEnum ULongFlagEnum;

        public bool? BoolNull;
        public byte? ByteNull;
        public sbyte? SByteNull;
        public short? ShortNull;
        public ushort? UShortNull;
        public int? IntNull;
        public uint? UIntNull;
        public long? LongNull;
        public ulong? ULongNull;
        public DateTimeOffset? DateTimeOffsetNull;
        public float? FloatNull;
        public double? DoubleNull;
        public Guid? GuidNull;
        public ByteEnum? ByteEnumNull;
        public ByteFlagEnum? ByteFlagEnumNull;
        public SByteEnum? SByteEnumNull;
        public SByteFlagEnum? SByteFlagEnumNull;
        public ShortEnum? ShortEnumNull;
        public ShortFlagEnum? ShortFlagEnumNull;
        public UShortEnum? UShortEnumNull;
        public UShortFlagEnum? UShortFlagEnumNull;
        public IntEnum? IntEnumNull;
        public IntFlagEnum? IntFlagEnumNull;
        public UIntEnum? UIntEnumNull;
        public UIntFlagEnum? UIntFlagEnumNull;
        public LongEnum? LongEnumNull;
        public LongFlagEnum? LongFlagEnumNull;
        public ULongEnum? ULongEnumNull;
        public ULongFlagEnum? ULongFlagEnumNull;
    }
}
