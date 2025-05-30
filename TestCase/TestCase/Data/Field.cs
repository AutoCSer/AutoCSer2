﻿using AutoCSer.TestCase.Common.Data;
using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 字段数据定义(引用类型外壳)
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
    [AutoCSer.CodeGenerator.MemberCopy]
#endif
    [AutoCSer.BinarySerialize(IsMemberMap = true)]
    internal partial class Field
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
        public Int128 Int128;
        public UInt128 UInt128;
        public DateTime DateTime;
        public TimeSpan TimeSpan;
        public Half Half;
        public float Float;
        public double Double;
        public decimal Decimal;
        public Guid Guid;
        public char Char;
        public System.Numerics.Complex Complex;
        public System.Numerics.Plane Plane;
        public System.Numerics.Quaternion Quaternion;
        public System.Numerics.Matrix3x2 Matrix3x2;
        public System.Numerics.Matrix4x4 Matrix4x4;
        public System.Numerics.Vector2 Vector2;
        public System.Numerics.Vector3 Vector3;
        public System.Numerics.Vector4 Vector4;
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
        public KeyValuePair<string, int> KeyValuePair;
        public KeyValue<int, string> KeyValue;

        public bool? BoolNull;
        public byte? ByteNull;
        public sbyte? SByteNull;
        public short? ShortNull;
        public ushort? UShortNull;
        public int? IntNull;
        public uint? UIntNull;
        public long? LongNull;
        public ulong? ULongNull;
        public Int128? Int128Null;
        public UInt128? UInt128Null;
        public DateTime? DateTimeNull;
        public TimeSpan? TimeSpanNull;
        public Half? HalfNull;
        public float? FloatNull;
        public double? DoubleNull;
        public decimal? DecimalNull;
        public Guid? GuidNull;
        public char? CharNull;
        public System.Numerics.Complex? ComplexNull;
        public System.Numerics.Plane? PlaneNull;
        public System.Numerics.Quaternion? QuaternionNull;
        public System.Numerics.Matrix3x2? Matrix3x2Null;
        public System.Numerics.Matrix4x4? Matrix4x4Null;
        public System.Numerics.Vector2? Vector2Null;
        public System.Numerics.Vector3? Vector3Null;
        public System.Numerics.Vector4? Vector4Null;
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
        public KeyValuePair<string, int?>? KeyValuePairNull;
        public KeyValue<int?, string>? KeyValueNull;

        public string String;
        public string String2;
        public SubString SubString;
        public SubString SubString2;
        public MemberClass MemberClass;
        public MemberClass MemberClass2;
        public NoMemberClass NoMemberClass;
        public NoMemberClass NoMemberClass2;
        public NoMemberClass NoMemberClass3;

        public bool[] BoolArray;
        public byte[] ByteArray;
        public sbyte[] SByteArray;
        public short[] ShortArray;
        public ushort[] UShortArray;
        public int[] IntArray;
        public uint[] UIntArray;
        public long[] LongArray;
        public ulong[] ULongArray;
        public Int128[] Int128Array;
        public UInt128[] UInt128Array;
        public DateTime[] DateTimeArray;
        public TimeSpan[] TimeSpanArray;
        public Half[] HalfArray;
        public float[] FloatArray;
        public double[] DoubleArray;
        public decimal[] DecimalArray;
        public Guid[] GuidArray;
        public char[] CharArray;
        public System.Numerics.Complex[] ComplexArray;
        public System.Numerics.Plane[] PlaneArray;
        public System.Numerics.Quaternion[] QuaternionArray;
        public System.Numerics.Matrix3x2[] Matrix3x2Array;
        public System.Numerics.Matrix4x4[] Matrix4x4Array;
        public System.Numerics.Vector2[] Vector2Array;
        public System.Numerics.Vector3[] Vector3Array;
        public System.Numerics.Vector4[] Vector4Array;
        public ByteEnum[] ByteEnumArray;
        public ByteFlagEnum[] ByteFlagEnumArray;
        public SByteEnum[] SByteEnumArray;
        public SByteFlagEnum[] SByteFlagEnumArray;
        public ShortEnum[] ShortEnumArray;
        public ShortFlagEnum[] ShortFlagEnumArray;
        public UShortEnum[] UShortEnumArray;
        public UShortFlagEnum[] UShortFlagEnumArray;
        public IntEnum[] IntEnumArray;
        public IntFlagEnum[] IntFlagEnumArray;
        public UIntEnum[] UIntEnumArray;
        public UIntFlagEnum[] UIntFlagEnumArray;
        public LongEnum[] LongEnumArray;
        public LongFlagEnum[] LongFlagEnumArray;
        public ULongEnum[] ULongEnumArray;
        public ULongFlagEnum[] ULongFlagEnumArray;
        public KeyValuePair<string, int>[] KeyValuePairArray;
        public KeyValue<int, string>[] KeyValueArray;

        public bool?[] BoolNullArray;
        public byte?[] ByteNullArray;
        public sbyte?[] SByteNullArray;
        public short?[] ShortNullArray;
        public ushort?[] UShortNullArray;
        public int?[] IntNullArray;
        public uint?[] UIntNullArray;
        public long?[] LongNullArray;
        public ulong?[] ULongNullArray;
        public Int128?[] Int128NullArray;
        public UInt128?[] UInt128NullArray;
        public DateTime?[] DateTimeNullArray;
        public TimeSpan?[] TimeSpanNullArray;
        public Half?[] HalfNullArray;
        public float?[] FloatNullArray;
        public double?[] DoubleNullArray;
        public decimal?[] DecimalNullArray;
        public Guid?[] GuidNullArray;
        public char?[] CharNullArray;
        public System.Numerics.Complex?[] ComplexNullArray;
        public System.Numerics.Plane?[] PlaneNullArray;
        public System.Numerics.Quaternion?[] QuaternionNullArray;
        public System.Numerics.Matrix3x2?[] Matrix3x2NullArray;
        public System.Numerics.Matrix4x4?[] Matrix4x4NullArray;
        public System.Numerics.Vector2?[] Vector2NullArray;
        public System.Numerics.Vector3?[] Vector3NullArray;
        public System.Numerics.Vector4?[] Vector4NullArray;
        public ByteEnum?[] ByteEnumNullArray;
        public ByteFlagEnum?[] ByteFlagEnumNullArray;
        public SByteEnum?[] SByteEnumNullArray;
        public SByteFlagEnum?[] SByteFlagEnumNullArray;
        public ShortEnum?[] ShortEnumNullArray;
        public ShortFlagEnum?[] ShortFlagEnumNullArray;
        public UShortEnum?[] UShortEnumNullArray;
        public UShortFlagEnum?[] UShortFlagEnumNullArray;
        public IntEnum?[] IntEnumNullArray;
        public IntFlagEnum?[] IntFlagEnumNullArray;
        public UIntEnum?[] UIntEnumNullArray;
        public UIntFlagEnum?[] UIntFlagEnumNullArray;
        public LongEnum?[] LongEnumNullArray;
        public LongFlagEnum?[] LongFlagEnumNullArray;
        public ULongEnum?[] ULongEnumNullArray;
        public ULongFlagEnum?[] ULongFlagEnumNullArray;
        public KeyValuePair<string, int?>?[] KeyValuePairNullArray;
        public KeyValue<int?, string>?[] KeyValueNullArray;

        public string[] StringArray;
        public SubString[] SubStringArray;
        public MemberClass[] MemberClassArray;
        public NoMemberClass[] NoMemberClassArray;

        public bool[] BoolArray2;
        public byte[] ByteArray2;
        public sbyte[] SByteArray2;
        public short[] ShortArray2;
        public ushort[] UShortArray2;
        public int[] IntArray2;
        public uint[] UIntArray2;
        public long[] LongArray2;
        public ulong[] ULongArray2;
        public Int128[] Int128Array2;
        public UInt128[] UInt128Array2;
        public DateTime[] DateTimeArray2;
        public Half[] HalfArray2;
        public float[] FloatArray2;
        public double[] DoubleArray2;
        public decimal[] DecimalArray2;
        public Guid[] GuidArray2;
        public char[] CharArray2;
        public System.Numerics.Complex[] ComplexArray2;
        public System.Numerics.Plane[] PlaneArray2;
        public System.Numerics.Quaternion[] QuaternionArray2;
        public System.Numerics.Matrix3x2[] Matrix3x2Array2;
        public System.Numerics.Matrix4x4[] Matrix4x4Array2;
        public System.Numerics.Vector2[] Vector2Array2;
        public System.Numerics.Vector3[] Vector3Array2;
        public System.Numerics.Vector4[] Vector4Array2;
        public ByteEnum[] ByteEnumArray2;
        public ByteFlagEnum[] ByteFlagEnumArray2;
        public SByteEnum[] SByteEnumArray2;
        public SByteFlagEnum[] SByteFlagEnumArray2;
        public ShortEnum[] ShortEnumArray2;
        public ShortFlagEnum[] ShortFlagEnumArray2;
        public UShortEnum[] UShortEnumArray2;
        public UShortFlagEnum[] UShortFlagEnumArray2;
        public IntEnum[] IntEnumArray2;
        public IntFlagEnum[] IntFlagEnumArray2;
        public UIntEnum[] UIntEnumArray2;
        public UIntFlagEnum[] UIntFlagEnumArray2;
        public LongEnum[] LongEnumArray2;
        public LongFlagEnum[] LongFlagEnumArray2;
        public ULongEnum[] ULongEnumArray2;
        public ULongFlagEnum[] ULongFlagEnumArray2;
        public KeyValuePair<string, int>[] KeyValuePairArray2;
        public KeyValue<int, string>[] KeyValueArray2;

        public bool?[] BoolNullArray2;
        public byte?[] ByteNullArray2;
        public sbyte?[] SByteNullArray2;
        public short?[] ShortNullArray2;
        public ushort?[] UShortNullArray2;
        public int?[] IntNullArray2;
        public uint?[] UIntNullArray2;
        public long?[] LongNullArray2;
        public ulong?[] ULongNullArray2;
        public Int128?[] Int128NullArray2;
        public UInt128?[] UInt128NullArray2;
        public DateTime?[] DateTimeNullArray2;
        public Half?[] HalfNullArray2;
        public float?[] FloatNullArray2;
        public double?[] DoubleNullArray2;
        public decimal?[] DecimalNullArray2;
        public Guid?[] GuidNullArray2;
        public char?[] CharNullArray2;
        public System.Numerics.Complex?[] ComplexNullArray2;
        public System.Numerics.Plane?[] PlaneNullArray2;
        public System.Numerics.Quaternion?[] QuaternionNullArray2;
        public System.Numerics.Matrix3x2?[] Matrix3x2NullArray2;
        public System.Numerics.Matrix4x4?[] Matrix4x4NullArray2;
        public System.Numerics.Vector2?[] Vector2NullArray2;
        public System.Numerics.Vector3?[] Vector3NullArray2;
        public System.Numerics.Vector4?[] Vector4NullArray2;
        public ByteEnum?[] ByteEnumNullArray2;
        public ByteFlagEnum?[] ByteFlagEnumNullArray2;
        public SByteEnum?[] SByteEnumNullArray2;
        public SByteFlagEnum?[] SByteFlagEnumNullArray2;
        public ShortEnum?[] ShortEnumNullArray2;
        public ShortFlagEnum?[] ShortFlagEnumNullArray2;
        public UShortEnum?[] UShortEnumNullArray2;
        public UShortFlagEnum?[] UShortFlagEnumNullArray2;
        public IntEnum?[] IntEnumNullArray2;
        public IntFlagEnum?[] IntFlagEnumNullArray2;
        public UIntEnum?[] UIntEnumNullArray2;
        public UIntFlagEnum?[] UIntFlagEnumNullArray2;
        public LongEnum?[] LongEnumNullArray2;
        public LongFlagEnum?[] LongFlagEnumNullArray2;
        public ULongEnum?[] ULongEnumNullArray2;
        public ULongFlagEnum?[] ULongFlagEnumNullArray2;
        public KeyValuePair<string, int?>?[] KeyValuePairNullArray2;
        public KeyValue<int?, string>?[] KeyValueNullArray2;

        public string[] StringArray2;
        public SubString[] SubStringArray2;
        public MemberClass[] MemberClassArray2;
        public NoMemberClass[] NoMemberClassArray2;
        public NoMemberClass[] NoMemberClassArray3;

        public List<bool> BoolList;
        public List<byte> ByteList;
        public List<sbyte> SByteList;
        public List<short> ShortList;
        public List<ushort> UShortList;
        public List<int> IntList;
        public List<uint> UIntList;
        public List<long> LongList;
        public List<ulong> ULongList;
        public List<Int128> Int128List;
        public List<UInt128> UInt128List;
        public List<DateTime> DateTimeList;
        public List<TimeSpan> TimeSpanList;
        public List<Half> HalfList;
        public List<float> FloatList;
        public List<double> DoubleList;
        public List<decimal> DecimalList;
        public List<Guid> GuidList;
        public List<char> CharList;
        public List<System.Numerics.Complex> ComplexList;
        public List<System.Numerics.Plane> PlaneList;
        public List<System.Numerics.Quaternion> QuaternionList;
        public List<System.Numerics.Matrix3x2> Matrix3x2List;
        public List<System.Numerics.Matrix4x4> Matrix4x4List;
        public List<System.Numerics.Vector2> Vector2List;
        public List<System.Numerics.Vector3> Vector3List;
        public List<System.Numerics.Vector4> Vector4List;
        public List<ByteEnum> ByteEnumList;
        public List<ByteFlagEnum> ByteFlagEnumList;
        public List<SByteEnum> SByteEnumList;
        public List<SByteFlagEnum> SByteFlagEnumList;
        public List<ShortEnum> ShortEnumList;
        public List<ShortFlagEnum> ShortFlagEnumList;
        public List<UShortEnum> UShortEnumList;
        public List<UShortFlagEnum> UShortFlagEnumList;
        public List<IntEnum> IntEnumList;
        public List<IntFlagEnum> IntFlagEnumList;
        public List<UIntEnum> UIntEnumList;
        public List<UIntFlagEnum> UIntFlagEnumList;
        public List<LongEnum> LongEnumList;
        public List<LongFlagEnum> LongFlagEnumList;
        public List<ULongEnum> ULongEnumList;
        public List<ULongFlagEnum> ULongFlagEnumList;
        public List<KeyValuePair<string, int>> KeyValuePairList;
        public List<KeyValue<int, string>> KeyValueList;

        public List<bool> BoolList2;
        public List<byte> ByteList2;
        public List<sbyte> SByteList2;
        public List<short> ShortList2;
        public List<ushort> UShortList2;
        public List<int> IntList2;
        public List<uint> UIntList2;
        public List<long> LongList2;
        public List<ulong> ULongList2;
        public List<Int128> Int128List2;
        public List<UInt128> UInt128List2;
        public List<DateTime> DateTimeList2;
        public List<Half> HalfList2;
        public List<float> FloatList2;
        public List<double> DoubleList2;
        public List<decimal> DecimalList2;
        public List<Guid> GuidList2;
        public List<char> CharList2;
        public List<System.Numerics.Complex> ComplexList2;
        public List<System.Numerics.Plane> PlaneList2;
        public List<System.Numerics.Quaternion> QuaternionList2;
        public List<System.Numerics.Matrix3x2> Matrix3x2List2;
        public List<System.Numerics.Matrix4x4> Matrix4x4List2;
        public List<System.Numerics.Vector2> Vector2List2;
        public List<System.Numerics.Vector3> Vector3List2;
        public List<System.Numerics.Vector4> Vector4List2;
        public List<ByteEnum> ByteEnumList2;
        public List<ByteFlagEnum> ByteFlagEnumList2;
        public List<SByteEnum> SByteEnumList2;
        public List<SByteFlagEnum> SByteFlagEnumList2;
        public List<ShortEnum> ShortEnumList2;
        public List<ShortFlagEnum> ShortFlagEnumList2;
        public List<UShortEnum> UShortEnumList2;
        public List<UShortFlagEnum> UShortFlagEnumList2;
        public List<IntEnum> IntEnumList2;
        public List<IntFlagEnum> IntFlagEnumList2;
        public List<UIntEnum> UIntEnumList2;
        public List<UIntFlagEnum> UIntFlagEnumList2;
        public List<LongEnum> LongEnumList2;
        public List<LongFlagEnum> LongFlagEnumList2;
        public List<ULongEnum> ULongEnumList2;
        public List<ULongFlagEnum> ULongFlagEnumList2;
        public List<KeyValuePair<string, int>> KeyValuePairList2;
        public List<KeyValue<int, string>> KeyValueList2;

        public List<bool?> BoolNullList;
        public List<byte?> ByteNullList;
        public List<sbyte?> SByteNullList;
        public List<short?> ShortNullList;
        public List<ushort?> UShortNullList;
        public List<int?> IntNullList;
        public List<uint?> UIntNullList;
        public List<long?> LongNullList;
        public List<ulong?> ULongNullList;
        public List<Int128?> Int128NullList;
        public List<UInt128?> UInt128NullList;
        public List<DateTime?> DateTimeNullList;
        public List<TimeSpan?> TimeSpanNullList;
        public List<Half?> HalfNullList;
        public List<float?> FloatNullList;
        public List<double?> DoubleNullList;
        public List<decimal?> DecimalNullList;
        public List<Guid?> GuidNullList;
        public List<char?> CharNullList;
        public List<System.Numerics.Complex?> ComplexNullList;
        public List<System.Numerics.Plane?> PlaneNullList;
        public List<System.Numerics.Quaternion?> QuaternionNullList;
        public List<System.Numerics.Matrix3x2?> Matrix3x2NullList;
        public List<System.Numerics.Matrix4x4?> Matrix4x4NullList;
        public List<System.Numerics.Vector2?> Vector2NullList;
        public List<System.Numerics.Vector3?> Vector3NullList;
        public List<System.Numerics.Vector4?> Vector4NullList;
        public List<ByteEnum?> ByteEnumNullList;
        public List<ByteFlagEnum?> ByteFlagEnumNullList;
        public List<SByteEnum?> SByteEnumNullList;
        public List<SByteFlagEnum?> SByteFlagEnumNullList;
        public List<ShortEnum?> ShortEnumNullList;
        public List<ShortFlagEnum?> ShortFlagEnumNullList;
        public List<UShortEnum?> UShortEnumNullList;
        public List<UShortFlagEnum?> UShortFlagEnumNullList;
        public List<IntEnum?> IntEnumNullList;
        public List<IntFlagEnum?> IntFlagEnumNullList;
        public List<UIntEnum?> UIntEnumNullList;
        public List<UIntFlagEnum?> UIntFlagEnumNullList;
        public List<LongEnum?> LongEnumNullList;
        public List<LongFlagEnum?> LongFlagEnumNullList;
        public List<ULongEnum?> ULongEnumNullList;
        public List<ULongFlagEnum?> ULongFlagEnumNullList;
        public List<KeyValuePair<string, int?>?> KeyValuePairNullList;
        public List<KeyValue<int?, string>?> KeyValueNullList;

        public List<string> StringList;
        public List<SubString> SubStringList;
        public List<string> StringList2;
        public List<SubString> SubStringList2;
        public List<MemberClass> MemberClassList;
        public List<MemberClass> MemberClassList2;
        public List<NoMemberClass> NoMemberClassList;
        public List<NoMemberClass> NoMemberClassList2;
        public List<NoMemberClass> NoMemberClassList3;

        public ListArray<bool> BoolListArray;
        public ListArray<byte> ByteListArray;
        public ListArray<sbyte> SByteListArray;
        public ListArray<short> ShortListArray;
        public ListArray<ushort> UShortListArray;
        public ListArray<int> IntListArray;
        public ListArray<uint> UIntListArray;
        public ListArray<long> LongListArray;
        public ListArray<ulong> ULongListArray;
        public ListArray<Int128> Int128ListArray;
        public ListArray<UInt128> UInt128ListArray;
        public ListArray<DateTime> DateTimeListArray;
        public ListArray<TimeSpan> TimeSpanListArray;
        public ListArray<Half> HalfListArray;
        public ListArray<float> FloatListArray;
        public ListArray<double> DoubleListArray;
        public ListArray<decimal> DecimalListArray;
        public ListArray<Guid> GuidListArray;
        public ListArray<char> CharListArray;
        public ListArray<System.Numerics.Complex> ComplexListArray;
        public ListArray<System.Numerics.Plane> PlaneListArray;
        public ListArray<System.Numerics.Quaternion> QuaternionListArray;
        public ListArray<System.Numerics.Matrix3x2> Matrix3x2ListArray;
        public ListArray<System.Numerics.Matrix4x4> Matrix4x4ListArray;
        public ListArray<System.Numerics.Vector2> Vector2ListArray;
        public ListArray<System.Numerics.Vector3> Vector3ListArray;
        public ListArray<System.Numerics.Vector4> Vector4ListArray;
        public ListArray<ByteEnum> ByteEnumListArray;
        public ListArray<ByteFlagEnum> ByteFlagEnumListArray;
        public ListArray<SByteEnum> SByteEnumListArray;
        public ListArray<SByteFlagEnum> SByteFlagEnumListArray;
        public ListArray<ShortEnum> ShortEnumListArray;
        public ListArray<ShortFlagEnum> ShortFlagEnumListArray;
        public ListArray<UShortEnum> UShortEnumListArray;
        public ListArray<UShortFlagEnum> UShortFlagEnumListArray;
        public ListArray<IntEnum> IntEnumListArray;
        public ListArray<IntFlagEnum> IntFlagEnumListArray;
        public ListArray<UIntEnum> UIntEnumListArray;
        public ListArray<UIntFlagEnum> UIntFlagEnumListArray;
        public ListArray<LongEnum> LongEnumListArray;
        public ListArray<LongFlagEnum> LongFlagEnumListArray;
        public ListArray<ULongEnum> ULongEnumListArray;
        public ListArray<ULongFlagEnum> ULongFlagEnumListArray;
        public ListArray<KeyValuePair<string, int>> KeyValuePairListArray;
        public ListArray<KeyValue<int, string>> KeyValueListArray;

        public ListArray<bool> BoolListArray2;
        public ListArray<byte> ByteListArray2;
        public ListArray<sbyte> SByteListArray2;
        public ListArray<short> ShortListArray2;
        public ListArray<ushort> UShortListArray2;
        public ListArray<int> IntListArray2;
        public ListArray<uint> UIntListArray2;
        public ListArray<long> LongListArray2;
        public ListArray<ulong> ULongListArray2;
        public ListArray<Int128> Int128ListArray2;
        public ListArray<UInt128> UInt128ListArray2;
        public ListArray<DateTime> DateTimeListArray2;
        public ListArray<Half> HalfListArray2;
        public ListArray<float> FloatListArray2;
        public ListArray<double> DoubleListArray2;
        public ListArray<decimal> DecimalListArray2;
        public ListArray<Guid> GuidListArray2;
        public ListArray<char> CharListArray2;
        public ListArray<System.Numerics.Complex> ComplexListArray2;
        public ListArray<System.Numerics.Plane> PlaneListArray2;
        public ListArray<System.Numerics.Quaternion> QuaternionListArray2;
        public ListArray<System.Numerics.Matrix3x2> Matrix3x2ListArray2;
        public ListArray<System.Numerics.Matrix4x4> Matrix4x4ListArray2;
        public ListArray<System.Numerics.Vector2> Vector2ListArray2;
        public ListArray<System.Numerics.Vector3> Vector3ListArray2;
        public ListArray<System.Numerics.Vector4> Vector4ListArray2;
        public ListArray<ByteEnum> ByteEnumListArray2;
        public ListArray<ByteFlagEnum> ByteFlagEnumListArray2;
        public ListArray<SByteEnum> SByteEnumListArray2;
        public ListArray<SByteFlagEnum> SByteFlagEnumListArray2;
        public ListArray<ShortEnum> ShortEnumListArray2;
        public ListArray<ShortFlagEnum> ShortFlagEnumListArray2;
        public ListArray<UShortEnum> UShortEnumListArray2;
        public ListArray<UShortFlagEnum> UShortFlagEnumListArray2;
        public ListArray<IntEnum> IntEnumListArray2;
        public ListArray<IntFlagEnum> IntFlagEnumListArray2;
        public ListArray<UIntEnum> UIntEnumListArray2;
        public ListArray<UIntFlagEnum> UIntFlagEnumListArray2;
        public ListArray<LongEnum> LongEnumListArray2;
        public ListArray<LongFlagEnum> LongFlagEnumListArray2;
        public ListArray<ULongEnum> ULongEnumListArray2;
        public ListArray<ULongFlagEnum> ULongFlagEnumListArray2;
        public ListArray<KeyValuePair<string, int>> KeyValuePairListArray2;
        public ListArray<KeyValue<int, string>> KeyValueListArray2;

        public ListArray<bool?> BoolNullListArray;
        public ListArray<byte?> ByteNullListArray;
        public ListArray<sbyte?> SByteNullListArray;
        public ListArray<short?> ShortNullListArray;
        public ListArray<ushort?> UShortNullListArray;
        public ListArray<int?> IntNullListArray;
        public ListArray<uint?> UIntNullListArray;
        public ListArray<long?> LongNullListArray;
        public ListArray<ulong?> ULongNullListArray;
        public ListArray<Int128?> Int128NullListArray;
        public ListArray<UInt128?> UInt128NullListArray;
        public ListArray<DateTime?> DateTimeNullListArray;
        public ListArray<TimeSpan?> TimeSpanNullListArray;
        public ListArray<Half?> HalfNullListArray;
        public ListArray<float?> FloatNullListArray;
        public ListArray<double?> DoubleNullListArray;
        public ListArray<decimal?> DecimalNullListArray;
        public ListArray<Guid?> GuidNullListArray;
        public ListArray<char?> CharNullListArray;
        public ListArray<System.Numerics.Complex?> ComplexNullListArray;
        public ListArray<System.Numerics.Plane?> PlaneNullListArray;
        public ListArray<System.Numerics.Quaternion?> QuaternionNullListArray;
        public ListArray<System.Numerics.Matrix3x2?> Matrix3x2NullListArray;
        public ListArray<System.Numerics.Matrix4x4?> Matrix4x4NullListArray;
        public ListArray<System.Numerics.Vector2?> Vector2NullListArray;
        public ListArray<System.Numerics.Vector3?> Vector3NullListArray;
        public ListArray<System.Numerics.Vector4?> Vector4NullListArray;
        public ListArray<ByteEnum?> ByteEnumNullListArray;
        public ListArray<ByteFlagEnum?> ByteFlagEnumNullListArray;
        public ListArray<SByteEnum?> SByteEnumNullListArray;
        public ListArray<SByteFlagEnum?> SByteFlagEnumNullListArray;
        public ListArray<ShortEnum?> ShortEnumNullListArray;
        public ListArray<ShortFlagEnum?> ShortFlagEnumNullListArray;
        public ListArray<UShortEnum?> UShortEnumNullListArray;
        public ListArray<UShortFlagEnum?> UShortFlagEnumNullListArray;
        public ListArray<IntEnum?> IntEnumNullListArray;
        public ListArray<IntFlagEnum?> IntFlagEnumNullListArray;
        public ListArray<UIntEnum?> UIntEnumNullListArray;
        public ListArray<UIntFlagEnum?> UIntFlagEnumNullListArray;
        public ListArray<LongEnum?> LongEnumNullListArray;
        public ListArray<LongFlagEnum?> LongFlagEnumNullListArray;
        public ListArray<ULongEnum?> ULongEnumNullListArray;
        public ListArray<ULongFlagEnum?> ULongFlagEnumNullListArray;
        public ListArray<KeyValuePair<string, int?>?> KeyValuePairNullListArray;
        public ListArray<KeyValue<int?, string>?> KeyValueNullListArray;

        public ListArray<string> StringListArray;
        public ListArray<SubString> SubStringListArray;
        public ListArray<string> StringListArray2;
        public ListArray<SubString> SubStringListArray2;
        public ListArray<MemberClass> MemberClassListArray;
        public ListArray<MemberClass> MemberClassListArray2;
        public ListArray<NoMemberClass> NoMemberClassListArray;
        public ListArray<NoMemberClass> NoMemberClassListArray2;
        public ListArray<NoMemberClass> NoMemberClassListArray3;

        public LeftArray<bool> BoolLeftArray;
        public LeftArray<byte> ByteLeftArray;
        public LeftArray<sbyte> SByteLeftArray;
        public LeftArray<short> ShortLeftArray;
        public LeftArray<ushort> UShortLeftArray;
        public LeftArray<int> IntLeftArray;
        public LeftArray<uint> UIntLeftArray;
        public LeftArray<long> LongLeftArray;
        public LeftArray<ulong> ULongLeftArray;
        public LeftArray<Int128> Int128LeftArray;
        public LeftArray<UInt128> UInt128LeftArray;
        public LeftArray<DateTime> DateTimeLeftArray;
        public LeftArray<TimeSpan> TimeSpanLeftArray;
        public LeftArray<Half> HalfLeftArray;
        public LeftArray<float> FloatLeftArray;
        public LeftArray<double> DoubleLeftArray;
        public LeftArray<decimal> DecimalLeftArray;
        public LeftArray<Guid> GuidLeftArray;
        public LeftArray<char> CharLeftArray;
        public LeftArray<System.Numerics.Complex> ComplexLeftArray;
        public LeftArray<System.Numerics.Plane> PlaneLeftArray;
        public LeftArray<System.Numerics.Quaternion> QuaternionLeftArray;
        public LeftArray<System.Numerics.Matrix3x2> Matrix3x2LeftArray;
        public LeftArray<System.Numerics.Matrix4x4> Matrix4x4LeftArray;
        public LeftArray<System.Numerics.Vector2> Vector2LeftArray;
        public LeftArray<System.Numerics.Vector3> Vector3LeftArray;
        public LeftArray<System.Numerics.Vector4> Vector4LeftArray;
        public LeftArray<ByteEnum> ByteEnumLeftArray;
        public LeftArray<ByteFlagEnum> ByteFlagEnumLeftArray;
        public LeftArray<SByteEnum> SByteEnumLeftArray;
        public LeftArray<SByteFlagEnum> SByteFlagEnumLeftArray;
        public LeftArray<ShortEnum> ShortEnumLeftArray;
        public LeftArray<ShortFlagEnum> ShortFlagEnumLeftArray;
        public LeftArray<UShortEnum> UShortEnumLeftArray;
        public LeftArray<UShortFlagEnum> UShortFlagEnumLeftArray;
        public LeftArray<IntEnum> IntEnumLeftArray;
        public LeftArray<IntFlagEnum> IntFlagEnumLeftArray;
        public LeftArray<UIntEnum> UIntEnumLeftArray;
        public LeftArray<UIntFlagEnum> UIntFlagEnumLeftArray;
        public LeftArray<LongEnum> LongEnumLeftArray;
        public LeftArray<LongFlagEnum> LongFlagEnumLeftArray;
        public LeftArray<ULongEnum> ULongEnumLeftArray;
        public LeftArray<ULongFlagEnum> ULongFlagEnumLeftArray;
        public LeftArray<KeyValuePair<string, int>> KeyValuePairLeftArray;
        public LeftArray<KeyValue<int, string>> KeyValueLeftArray;

        public LeftArray<bool?> BoolNullLeftArray;
        public LeftArray<byte?> ByteNullLeftArray;
        public LeftArray<sbyte?> SByteNullLeftArray;
        public LeftArray<short?> ShortNullLeftArray;
        public LeftArray<ushort?> UShortNullLeftArray;
        public LeftArray<int?> IntNullLeftArray;
        public LeftArray<uint?> UIntNullLeftArray;
        public LeftArray<long?> LongNullLeftArray;
        public LeftArray<ulong?> ULongNullLeftArray;
        public LeftArray<Int128?> Int128NullLeftArray;
        public LeftArray<UInt128?> UInt128NullLeftArray;
        public LeftArray<DateTime?> DateTimeNullLeftArray;
        public LeftArray<TimeSpan?> TimeSpanNullLeftArray;
        public LeftArray<Half?> HalfNullLeftArray;
        public LeftArray<float?> FloatNullLeftArray;
        public LeftArray<double?> DoubleNullLeftArray;
        public LeftArray<decimal?> DecimalNullLeftArray;
        public LeftArray<Guid?> GuidNullLeftArray;
        public LeftArray<char?> CharNullLeftArray;
        public LeftArray<System.Numerics.Complex?> ComplexNullLeftArray;
        public LeftArray<System.Numerics.Plane?> PlaneNullLeftArray;
        public LeftArray<System.Numerics.Quaternion?> QuaternionNullLeftArray;
        public LeftArray<System.Numerics.Matrix3x2?> Matrix3x2NullLeftArray;
        public LeftArray<System.Numerics.Matrix4x4?> Matrix4x4NullLeftArray;
        public LeftArray<System.Numerics.Vector2?> Vector2NullLeftArray;
        public LeftArray<System.Numerics.Vector3?> Vector3NullLeftArray;
        public LeftArray<System.Numerics.Vector4?> Vector4NullLeftArray;
        public LeftArray<ByteEnum?> ByteEnumNullLeftArray;
        public LeftArray<ByteFlagEnum?> ByteFlagEnumNullLeftArray;
        public LeftArray<SByteEnum?> SByteEnumNullLeftArray;
        public LeftArray<SByteFlagEnum?> SByteFlagEnumNullLeftArray;
        public LeftArray<ShortEnum?> ShortEnumNullLeftArray;
        public LeftArray<ShortFlagEnum?> ShortFlagEnumNullLeftArray;
        public LeftArray<UShortEnum?> UShortEnumNullLeftArray;
        public LeftArray<UShortFlagEnum?> UShortFlagEnumNullLeftArray;
        public LeftArray<IntEnum?> IntEnumNullLeftArray;
        public LeftArray<IntFlagEnum?> IntFlagEnumNullLeftArray;
        public LeftArray<UIntEnum?> UIntEnumNullLeftArray;
        public LeftArray<UIntFlagEnum?> UIntFlagEnumNullLeftArray;
        public LeftArray<LongEnum?> LongEnumNullLeftArray;
        public LeftArray<LongFlagEnum?> LongFlagEnumNullLeftArray;
        public LeftArray<ULongEnum?> ULongEnumNullLeftArray;
        public LeftArray<ULongFlagEnum?> ULongFlagEnumNullLeftArray;
        public LeftArray<KeyValuePair<string, int?>?> KeyValuePairNullLeftArray;
        public LeftArray<KeyValue<int?, string>?> KeyValueNullLeftArray;

        public LeftArray<string> StringLeftArray;
        public LeftArray<SubString> SubStringLeftArray;
        public LeftArray<string> StringLeftArray2;
        public LeftArray<SubString> SubStringLeftArray2;
        public LeftArray<MemberClass> MemberClassLeftArray;
        public LeftArray<MemberClass> MemberClassLeftArray2;

        public Dictionary<string, int> StringDictionary;
        public Dictionary<string, int> StringDictionary2;
        public Dictionary<int, string> IntDictionary;
        public Dictionary<int, string> IntDictionary2;
    }
}
