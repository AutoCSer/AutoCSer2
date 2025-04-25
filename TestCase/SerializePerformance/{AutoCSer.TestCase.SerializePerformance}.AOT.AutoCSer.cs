//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FieldData
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.SerializePerformance.FieldData value)
            {
                if (serializer.WriteMemberCountVerify(172, 1073741851)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(Guid);
                __serializer__.BinarySerialize(Long);
                __serializer__.BinarySerialize(DateTime);
                __serializer__.BinarySerialize(TimeSpan);
                __serializer__.BinarySerialize(IntNull);
                __serializer__.BinarySerialize(UIntNull);
                __serializer__.BinarySerialize(ULong);
                __serializer__.BinarySerialize(UInt);
                __serializer__.BinarySerialize(LongNull);
                __serializer__.BinarySerialize(ShortNull);
                __serializer__.BinarySerialize(ULongNull);
                __serializer__.BinarySerialize(TimeSpanNull);
                __serializer__.BinarySerialize(UShortNull);
                __serializer__.BinarySerialize(Int);
                __serializer__.BinarySerialize(DateTimeNull);
                __serializer__.BinarySerialize(CharNull);
                __serializer__.BinarySerialize(GuidNull);
                __serializer__.BinarySerialize(Short);
                __serializer__.BinarySerialize(UShort);
                __serializer__.BinarySerialize(ByteNull);
                __serializer__.BinarySerialize(Char);
                __serializer__.BinarySerialize(SByteNull);
                __serializer__.BinarySerialize(SByte);
                __serializer__.BinarySerialize(BoolNull);
                __serializer__.BinarySerialize(Byte);
                __serializer__.BinarySerialize(Bool);
                __serializer__.FixedFillSize(2);
                __serializer__.BinarySerialize(String);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FieldData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.Guid);
                __deserializer__.BinaryDeserialize(ref this.Long);
                __deserializer__.BinaryDeserialize(ref this.DateTime);
                __deserializer__.BinaryDeserialize(ref this.TimeSpan);
                __deserializer__.BinaryDeserialize(ref this.IntNull);
                __deserializer__.BinaryDeserialize(ref this.UIntNull);
                __deserializer__.BinaryDeserialize(ref this.ULong);
                __deserializer__.BinaryDeserialize(ref this.UInt);
                __deserializer__.BinaryDeserialize(ref this.LongNull);
                __deserializer__.BinaryDeserialize(ref this.ShortNull);
                __deserializer__.BinaryDeserialize(ref this.ULongNull);
                __deserializer__.BinaryDeserialize(ref this.TimeSpanNull);
                __deserializer__.BinaryDeserialize(ref this.UShortNull);
                __deserializer__.BinaryDeserialize(ref this.Int);
                __deserializer__.BinaryDeserialize(ref this.DateTimeNull);
                __deserializer__.BinaryDeserialize(ref this.CharNull);
                __deserializer__.BinaryDeserialize(ref this.GuidNull);
                __deserializer__.BinaryDeserialize(ref this.Short);
                __deserializer__.BinaryDeserialize(ref this.UShort);
                __deserializer__.BinaryDeserialize(ref this.ByteNull);
                __deserializer__.BinaryDeserialize(ref this.Char);
                __deserializer__.BinaryDeserialize(ref this.SByteNull);
                __deserializer__.BinaryDeserialize(ref this.SByte);
                __deserializer__.BinaryDeserialize(ref this.BoolNull);
                __deserializer__.BinaryDeserialize(ref this.Byte);
                __deserializer__.BinaryDeserialize(ref this.Bool);
                __deserializer__.FixedFillSize(2);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.String);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 27, 1073741851);
                typeInfo.Add(typeof(System.Guid));
                typeInfo.Add(typeof(long));
                typeInfo.Add(typeof(System.DateTime));
                typeInfo.Add(typeof(System.TimeSpan));
                typeInfo.Add(typeof(int?));
                typeInfo.Add(typeof(uint?));
                typeInfo.Add(typeof(ulong));
                typeInfo.Add(typeof(uint));
                typeInfo.Add(typeof(long?));
                typeInfo.Add(typeof(short?));
                typeInfo.Add(typeof(ulong?));
                typeInfo.Add(typeof(System.TimeSpan?));
                typeInfo.Add(typeof(ushort?));
                typeInfo.Add(typeof(int));
                typeInfo.Add(typeof(System.DateTime?));
                typeInfo.Add(typeof(char?));
                typeInfo.Add(typeof(System.Guid?));
                typeInfo.Add(typeof(short));
                typeInfo.Add(typeof(ushort));
                typeInfo.Add(typeof(byte?));
                typeInfo.Add(typeof(char));
                typeInfo.Add(typeof(sbyte?));
                typeInfo.Add(typeof(sbyte));
                typeInfo.Add(typeof(bool?));
                typeInfo.Add(typeof(byte));
                typeInfo.Add(typeof(bool));
                typeInfo.Add(typeof(string));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FieldData value = default(AutoCSer.TestCase.SerializePerformance.FieldData);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FieldData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatFieldData
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatFieldData value)
            {
                if (serializer.WriteMemberCountVerify(240, 1073741857)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(Guid);
                __serializer__.BinarySerialize(Decimal);
                __serializer__.BinarySerialize(TimeSpan);
                __serializer__.BinarySerialize(Double);
                __serializer__.BinarySerialize(Long);
                __serializer__.BinarySerialize(FloatNull);
                __serializer__.BinarySerialize(IntNull);
                __serializer__.BinarySerialize(DateTime);
                __serializer__.BinarySerialize(ULong);
                __serializer__.BinarySerialize(UIntNull);
                __serializer__.BinarySerialize(ULongNull);
                __serializer__.BinarySerialize(LongNull);
                __serializer__.BinarySerialize(UInt);
                __serializer__.BinarySerialize(TimeSpanNull);
                __serializer__.BinarySerialize(ShortNull);
                __serializer__.BinarySerialize(GuidNull);
                __serializer__.BinarySerialize(Int);
                __serializer__.BinarySerialize(UShortNull);
                __serializer__.BinarySerialize(DateTimeNull);
                __serializer__.BinarySerialize(CharNull);
                __serializer__.BinarySerialize(DecimalNull);
                __serializer__.BinarySerialize(Float);
                __serializer__.BinarySerialize(DoubleNull);
                __serializer__.BinarySerialize(SByteNull);
                __serializer__.BinarySerialize(UShort);
                __serializer__.BinarySerialize(ByteNull);
                __serializer__.BinarySerialize(Char);
                __serializer__.BinarySerialize(Short);
                __serializer__.BinarySerialize(SByte);
                __serializer__.BinarySerialize(BoolNull);
                __serializer__.BinarySerialize(Byte);
                __serializer__.BinarySerialize(Bool);
                __serializer__.FixedFillSize(2);
                __serializer__.BinarySerialize(String);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatFieldData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.Guid);
                __deserializer__.BinaryDeserialize(ref this.Decimal);
                __deserializer__.BinaryDeserialize(ref this.TimeSpan);
                __deserializer__.BinaryDeserialize(ref this.Double);
                __deserializer__.BinaryDeserialize(ref this.Long);
                __deserializer__.BinaryDeserialize(ref this.FloatNull);
                __deserializer__.BinaryDeserialize(ref this.IntNull);
                __deserializer__.BinaryDeserialize(ref this.DateTime);
                __deserializer__.BinaryDeserialize(ref this.ULong);
                __deserializer__.BinaryDeserialize(ref this.UIntNull);
                __deserializer__.BinaryDeserialize(ref this.ULongNull);
                __deserializer__.BinaryDeserialize(ref this.LongNull);
                __deserializer__.BinaryDeserialize(ref this.UInt);
                __deserializer__.BinaryDeserialize(ref this.TimeSpanNull);
                __deserializer__.BinaryDeserialize(ref this.ShortNull);
                __deserializer__.BinaryDeserialize(ref this.GuidNull);
                __deserializer__.BinaryDeserialize(ref this.Int);
                __deserializer__.BinaryDeserialize(ref this.UShortNull);
                __deserializer__.BinaryDeserialize(ref this.DateTimeNull);
                __deserializer__.BinaryDeserialize(ref this.CharNull);
                __deserializer__.BinaryDeserialize(ref this.DecimalNull);
                __deserializer__.BinaryDeserialize(ref this.Float);
                __deserializer__.BinaryDeserialize(ref this.DoubleNull);
                __deserializer__.BinaryDeserialize(ref this.SByteNull);
                __deserializer__.BinaryDeserialize(ref this.UShort);
                __deserializer__.BinaryDeserialize(ref this.ByteNull);
                __deserializer__.BinaryDeserialize(ref this.Char);
                __deserializer__.BinaryDeserialize(ref this.Short);
                __deserializer__.BinaryDeserialize(ref this.SByte);
                __deserializer__.BinaryDeserialize(ref this.BoolNull);
                __deserializer__.BinaryDeserialize(ref this.Byte);
                __deserializer__.BinaryDeserialize(ref this.Bool);
                __deserializer__.FixedFillSize(2);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.String);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 33, 1073741857);
                typeInfo.Add(typeof(System.Guid));
                typeInfo.Add(typeof(decimal));
                typeInfo.Add(typeof(System.TimeSpan));
                typeInfo.Add(typeof(double));
                typeInfo.Add(typeof(long));
                typeInfo.Add(typeof(float?));
                typeInfo.Add(typeof(int?));
                typeInfo.Add(typeof(System.DateTime));
                typeInfo.Add(typeof(ulong));
                typeInfo.Add(typeof(uint?));
                typeInfo.Add(typeof(ulong?));
                typeInfo.Add(typeof(long?));
                typeInfo.Add(typeof(uint));
                typeInfo.Add(typeof(System.TimeSpan?));
                typeInfo.Add(typeof(short?));
                typeInfo.Add(typeof(System.Guid?));
                typeInfo.Add(typeof(int));
                typeInfo.Add(typeof(ushort?));
                typeInfo.Add(typeof(System.DateTime?));
                typeInfo.Add(typeof(char?));
                typeInfo.Add(typeof(decimal?));
                typeInfo.Add(typeof(float));
                typeInfo.Add(typeof(double?));
                typeInfo.Add(typeof(sbyte?));
                typeInfo.Add(typeof(ushort));
                typeInfo.Add(typeof(byte?));
                typeInfo.Add(typeof(char));
                typeInfo.Add(typeof(short));
                typeInfo.Add(typeof(sbyte));
                typeInfo.Add(typeof(bool?));
                typeInfo.Add(typeof(byte));
                typeInfo.Add(typeof(bool));
                typeInfo.Add(typeof(string));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatFieldData value = default(AutoCSer.TestCase.SerializePerformance.FloatFieldData);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FloatFieldData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatPropertyData
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                if (serializer.WriteMemberCountVerify(240, 1073741857)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(Guid);
                __serializer__.BinarySerialize(Decimal);
                __serializer__.BinarySerialize(TimeSpan);
                __serializer__.BinarySerialize(Double);
                __serializer__.BinarySerialize(Long);
                __serializer__.BinarySerialize(FloatNull);
                __serializer__.BinarySerialize(IntNull);
                __serializer__.BinarySerialize(DateTime);
                __serializer__.BinarySerialize(ULong);
                __serializer__.BinarySerialize(UIntNull);
                __serializer__.BinarySerialize(ULongNull);
                __serializer__.BinarySerialize(LongNull);
                __serializer__.BinarySerialize(UInt);
                __serializer__.BinarySerialize(TimeSpanNull);
                __serializer__.BinarySerialize(ShortNull);
                __serializer__.BinarySerialize(GuidNull);
                __serializer__.BinarySerialize(Int);
                __serializer__.BinarySerialize(UShortNull);
                __serializer__.BinarySerialize(DateTimeNull);
                __serializer__.BinarySerialize(CharNull);
                __serializer__.BinarySerialize(DecimalNull);
                __serializer__.BinarySerialize(Float);
                __serializer__.BinarySerialize(DoubleNull);
                __serializer__.BinarySerialize(SByteNull);
                __serializer__.BinarySerialize(UShort);
                __serializer__.BinarySerialize(ByteNull);
                __serializer__.BinarySerialize(Char);
                __serializer__.BinarySerialize(Short);
                __serializer__.BinarySerialize(SByte);
                __serializer__.BinarySerialize(BoolNull);
                __serializer__.BinarySerialize(Byte);
                __serializer__.BinarySerialize(Bool);
                __serializer__.FixedFillSize(2);
                __serializer__.BinarySerialize(String);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> memberMap, AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                int startIndex = serializer.Stream.GetPrepSizeCurrentIndex(240);
                if (startIndex >= 0) value.binarySerialize(memberMap, serializer, startIndex);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__startIndex__"></param>
            private void binarySerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> __memberMap__, AutoCSer.BinarySerializer __serializer__, int __startIndex__)
            {
                if (__memberMap__.IsMember(14))
                {
                    __serializer__.BinarySerialize(Guid);
                }
                if (__memberMap__.IsMember(8))
                {
                    __serializer__.BinarySerialize(Decimal);
                }
                if (__memberMap__.IsMember(25))
                {
                    __serializer__.BinarySerialize(TimeSpan);
                }
                if (__memberMap__.IsMember(10))
                {
                    __serializer__.BinarySerialize(Double);
                }
                if (__memberMap__.IsMember(18))
                {
                    __serializer__.BinarySerialize(Long);
                }
                if (__memberMap__.IsMember(13))
                {
                    __serializer__.BinarySerialize(FloatNull);
                }
                if (__memberMap__.IsMember(17))
                {
                    __serializer__.BinarySerialize(IntNull);
                }
                if (__memberMap__.IsMember(6))
                {
                    __serializer__.BinarySerialize(DateTime);
                }
                if (__memberMap__.IsMember(29))
                {
                    __serializer__.BinarySerialize(ULong);
                }
                if (__memberMap__.IsMember(28))
                {
                    __serializer__.BinarySerialize(UIntNull);
                }
                if (__memberMap__.IsMember(30))
                {
                    __serializer__.BinarySerialize(ULongNull);
                }
                if (__memberMap__.IsMember(19))
                {
                    __serializer__.BinarySerialize(LongNull);
                }
                if (__memberMap__.IsMember(27))
                {
                    __serializer__.BinarySerialize(UInt);
                }
                if (__memberMap__.IsMember(26))
                {
                    __serializer__.BinarySerialize(TimeSpanNull);
                }
                if (__memberMap__.IsMember(23))
                {
                    __serializer__.BinarySerialize(ShortNull);
                }
                if (__memberMap__.IsMember(15))
                {
                    __serializer__.BinarySerialize(GuidNull);
                }
                if (__memberMap__.IsMember(16))
                {
                    __serializer__.BinarySerialize(Int);
                }
                if (__memberMap__.IsMember(32))
                {
                    __serializer__.BinarySerialize(UShortNull);
                }
                if (__memberMap__.IsMember(7))
                {
                    __serializer__.BinarySerialize(DateTimeNull);
                }
                if (__memberMap__.IsMember(5))
                {
                    __serializer__.BinarySerialize(CharNull);
                }
                if (__memberMap__.IsMember(9))
                {
                    __serializer__.BinarySerialize(DecimalNull);
                }
                if (__memberMap__.IsMember(12))
                {
                    __serializer__.BinarySerialize(Float);
                }
                if (__memberMap__.IsMember(11))
                {
                    __serializer__.BinarySerialize(DoubleNull);
                }
                if (__memberMap__.IsMember(21))
                {
                    __serializer__.BinarySerialize(SByteNull);
                }
                if (__memberMap__.IsMember(31))
                {
                    __serializer__.BinarySerialize(UShort);
                }
                if (__memberMap__.IsMember(3))
                {
                    __serializer__.BinarySerialize(ByteNull);
                }
                if (__memberMap__.IsMember(4))
                {
                    __serializer__.BinarySerialize(Char);
                }
                if (__memberMap__.IsMember(22))
                {
                    __serializer__.BinarySerialize(Short);
                }
                if (__memberMap__.IsMember(20))
                {
                    __serializer__.BinarySerialize(SByte);
                }
                if (__memberMap__.IsMember(1))
                {
                    __serializer__.BinarySerialize(BoolNull);
                }
                if (__memberMap__.IsMember(2))
                {
                    __serializer__.BinarySerialize(Byte);
                }
                if (__memberMap__.IsMember(0))
                {
                    __serializer__.BinarySerialize(Bool);
                }
                __serializer__.SerializeFill(__startIndex__);
                if (__memberMap__.IsMember(24)) __serializer__.BinarySerialize(String);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                var Guid = this.Guid;
                __deserializer__.BinaryDeserialize(ref Guid);
                this.Guid = Guid;
                var Decimal = this.Decimal;
                __deserializer__.BinaryDeserialize(ref Decimal);
                this.Decimal = Decimal;
                var TimeSpan = this.TimeSpan;
                __deserializer__.BinaryDeserialize(ref TimeSpan);
                this.TimeSpan = TimeSpan;
                var Double = this.Double;
                __deserializer__.BinaryDeserialize(ref Double);
                this.Double = Double;
                var Long = this.Long;
                __deserializer__.BinaryDeserialize(ref Long);
                this.Long = Long;
                var FloatNull = this.FloatNull;
                __deserializer__.BinaryDeserialize(ref FloatNull);
                this.FloatNull = FloatNull;
                var IntNull = this.IntNull;
                __deserializer__.BinaryDeserialize(ref IntNull);
                this.IntNull = IntNull;
                var DateTime = this.DateTime;
                __deserializer__.BinaryDeserialize(ref DateTime);
                this.DateTime = DateTime;
                var ULong = this.ULong;
                __deserializer__.BinaryDeserialize(ref ULong);
                this.ULong = ULong;
                var UIntNull = this.UIntNull;
                __deserializer__.BinaryDeserialize(ref UIntNull);
                this.UIntNull = UIntNull;
                var ULongNull = this.ULongNull;
                __deserializer__.BinaryDeserialize(ref ULongNull);
                this.ULongNull = ULongNull;
                var LongNull = this.LongNull;
                __deserializer__.BinaryDeserialize(ref LongNull);
                this.LongNull = LongNull;
                var UInt = this.UInt;
                __deserializer__.BinaryDeserialize(ref UInt);
                this.UInt = UInt;
                var TimeSpanNull = this.TimeSpanNull;
                __deserializer__.BinaryDeserialize(ref TimeSpanNull);
                this.TimeSpanNull = TimeSpanNull;
                var ShortNull = this.ShortNull;
                __deserializer__.BinaryDeserialize(ref ShortNull);
                this.ShortNull = ShortNull;
                var GuidNull = this.GuidNull;
                __deserializer__.BinaryDeserialize(ref GuidNull);
                this.GuidNull = GuidNull;
                var Int = this.Int;
                __deserializer__.BinaryDeserialize(ref Int);
                this.Int = Int;
                var UShortNull = this.UShortNull;
                __deserializer__.BinaryDeserialize(ref UShortNull);
                this.UShortNull = UShortNull;
                var DateTimeNull = this.DateTimeNull;
                __deserializer__.BinaryDeserialize(ref DateTimeNull);
                this.DateTimeNull = DateTimeNull;
                var CharNull = this.CharNull;
                __deserializer__.BinaryDeserialize(ref CharNull);
                this.CharNull = CharNull;
                var DecimalNull = this.DecimalNull;
                __deserializer__.BinaryDeserialize(ref DecimalNull);
                this.DecimalNull = DecimalNull;
                var Float = this.Float;
                __deserializer__.BinaryDeserialize(ref Float);
                this.Float = Float;
                var DoubleNull = this.DoubleNull;
                __deserializer__.BinaryDeserialize(ref DoubleNull);
                this.DoubleNull = DoubleNull;
                var SByteNull = this.SByteNull;
                __deserializer__.BinaryDeserialize(ref SByteNull);
                this.SByteNull = SByteNull;
                var UShort = this.UShort;
                __deserializer__.BinaryDeserialize(ref UShort);
                this.UShort = UShort;
                var ByteNull = this.ByteNull;
                __deserializer__.BinaryDeserialize(ref ByteNull);
                this.ByteNull = ByteNull;
                var Char = this.Char;
                __deserializer__.BinaryDeserialize(ref Char);
                this.Char = Char;
                var Short = this.Short;
                __deserializer__.BinaryDeserialize(ref Short);
                this.Short = Short;
                var SByte = this.SByte;
                __deserializer__.BinaryDeserialize(ref SByte);
                this.SByte = SByte;
                var BoolNull = this.BoolNull;
                __deserializer__.BinaryDeserialize(ref BoolNull);
                this.BoolNull = BoolNull;
                var Byte = this.Byte;
                __deserializer__.BinaryDeserialize(ref Byte);
                this.Byte = Byte;
                var Bool = this.Bool;
                __deserializer__.BinaryDeserialize(ref Bool);
                this.Bool = Bool;
                __deserializer__.FixedFillSize(2);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                var String = this.String;
                __deserializer__.BinaryDeserialize(ref String);
                this.String = String;
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> memberMap, AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                value.binaryDeserialize(memberMap, deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.SetFixedCurrent();
                if (__memberMap__.IsMember(14))
                {
                    var Guid = this.Guid;
                    __deserializer__.BinaryDeserialize(ref Guid);
                    this.Guid = Guid;
                }
                if (__memberMap__.IsMember(8))
                {
                    var Decimal = this.Decimal;
                    __deserializer__.BinaryDeserialize(ref Decimal);
                    this.Decimal = Decimal;
                }
                if (__memberMap__.IsMember(25))
                {
                    var TimeSpan = this.TimeSpan;
                    __deserializer__.BinaryDeserialize(ref TimeSpan);
                    this.TimeSpan = TimeSpan;
                }
                if (__memberMap__.IsMember(10))
                {
                    var Double = this.Double;
                    __deserializer__.BinaryDeserialize(ref Double);
                    this.Double = Double;
                }
                if (__memberMap__.IsMember(18))
                {
                    var Long = this.Long;
                    __deserializer__.BinaryDeserialize(ref Long);
                    this.Long = Long;
                }
                if (__memberMap__.IsMember(13))
                {
                    var FloatNull = this.FloatNull;
                    __deserializer__.BinaryDeserialize(ref FloatNull);
                    this.FloatNull = FloatNull;
                }
                if (__memberMap__.IsMember(17))
                {
                    var IntNull = this.IntNull;
                    __deserializer__.BinaryDeserialize(ref IntNull);
                    this.IntNull = IntNull;
                }
                if (__memberMap__.IsMember(6))
                {
                    var DateTime = this.DateTime;
                    __deserializer__.BinaryDeserialize(ref DateTime);
                    this.DateTime = DateTime;
                }
                if (__memberMap__.IsMember(29))
                {
                    var ULong = this.ULong;
                    __deserializer__.BinaryDeserialize(ref ULong);
                    this.ULong = ULong;
                }
                if (__memberMap__.IsMember(28))
                {
                    var UIntNull = this.UIntNull;
                    __deserializer__.BinaryDeserialize(ref UIntNull);
                    this.UIntNull = UIntNull;
                }
                if (__memberMap__.IsMember(30))
                {
                    var ULongNull = this.ULongNull;
                    __deserializer__.BinaryDeserialize(ref ULongNull);
                    this.ULongNull = ULongNull;
                }
                if (__memberMap__.IsMember(19))
                {
                    var LongNull = this.LongNull;
                    __deserializer__.BinaryDeserialize(ref LongNull);
                    this.LongNull = LongNull;
                }
                if (__memberMap__.IsMember(27))
                {
                    var UInt = this.UInt;
                    __deserializer__.BinaryDeserialize(ref UInt);
                    this.UInt = UInt;
                }
                if (__memberMap__.IsMember(26))
                {
                    var TimeSpanNull = this.TimeSpanNull;
                    __deserializer__.BinaryDeserialize(ref TimeSpanNull);
                    this.TimeSpanNull = TimeSpanNull;
                }
                if (__memberMap__.IsMember(23))
                {
                    var ShortNull = this.ShortNull;
                    __deserializer__.BinaryDeserialize(ref ShortNull);
                    this.ShortNull = ShortNull;
                }
                if (__memberMap__.IsMember(15))
                {
                    var GuidNull = this.GuidNull;
                    __deserializer__.BinaryDeserialize(ref GuidNull);
                    this.GuidNull = GuidNull;
                }
                if (__memberMap__.IsMember(16))
                {
                    var Int = this.Int;
                    __deserializer__.BinaryDeserialize(ref Int);
                    this.Int = Int;
                }
                if (__memberMap__.IsMember(32))
                {
                    var UShortNull = this.UShortNull;
                    __deserializer__.BinaryDeserialize(ref UShortNull);
                    this.UShortNull = UShortNull;
                }
                if (__memberMap__.IsMember(7))
                {
                    var DateTimeNull = this.DateTimeNull;
                    __deserializer__.BinaryDeserialize(ref DateTimeNull);
                    this.DateTimeNull = DateTimeNull;
                }
                if (__memberMap__.IsMember(5))
                {
                    var CharNull = this.CharNull;
                    __deserializer__.BinaryDeserialize(ref CharNull);
                    this.CharNull = CharNull;
                }
                if (__memberMap__.IsMember(9))
                {
                    var DecimalNull = this.DecimalNull;
                    __deserializer__.BinaryDeserialize(ref DecimalNull);
                    this.DecimalNull = DecimalNull;
                }
                if (__memberMap__.IsMember(12))
                {
                    var Float = this.Float;
                    __deserializer__.BinaryDeserialize(ref Float);
                    this.Float = Float;
                }
                if (__memberMap__.IsMember(11))
                {
                    var DoubleNull = this.DoubleNull;
                    __deserializer__.BinaryDeserialize(ref DoubleNull);
                    this.DoubleNull = DoubleNull;
                }
                if (__memberMap__.IsMember(21))
                {
                    var SByteNull = this.SByteNull;
                    __deserializer__.BinaryDeserialize(ref SByteNull);
                    this.SByteNull = SByteNull;
                }
                if (__memberMap__.IsMember(31))
                {
                    var UShort = this.UShort;
                    __deserializer__.BinaryDeserialize(ref UShort);
                    this.UShort = UShort;
                }
                if (__memberMap__.IsMember(3))
                {
                    var ByteNull = this.ByteNull;
                    __deserializer__.BinaryDeserialize(ref ByteNull);
                    this.ByteNull = ByteNull;
                }
                if (__memberMap__.IsMember(4))
                {
                    var Char = this.Char;
                    __deserializer__.BinaryDeserialize(ref Char);
                    this.Char = Char;
                }
                if (__memberMap__.IsMember(22))
                {
                    var Short = this.Short;
                    __deserializer__.BinaryDeserialize(ref Short);
                    this.Short = Short;
                }
                if (__memberMap__.IsMember(20))
                {
                    var SByte = this.SByte;
                    __deserializer__.BinaryDeserialize(ref SByte);
                    this.SByte = SByte;
                }
                if (__memberMap__.IsMember(1))
                {
                    var BoolNull = this.BoolNull;
                    __deserializer__.BinaryDeserialize(ref BoolNull);
                    this.BoolNull = BoolNull;
                }
                if (__memberMap__.IsMember(2))
                {
                    var Byte = this.Byte;
                    __deserializer__.BinaryDeserialize(ref Byte);
                    this.Byte = Byte;
                }
                if (__memberMap__.IsMember(0))
                {
                    var Bool = this.Bool;
                    __deserializer__.BinaryDeserialize(ref Bool);
                    this.Bool = Bool;
                }
                __deserializer__.SetFixedCurrentEnd();
                if (__memberMap__.IsMember(24))
                {
                    var String = this.String;
                    __deserializer__.BinaryDeserialize(ref String);
                    this.String = String;
                }
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 33, 1073741857);
                typeInfo.Add(typeof(System.Guid));
                typeInfo.Add(typeof(decimal));
                typeInfo.Add(typeof(System.TimeSpan));
                typeInfo.Add(typeof(double));
                typeInfo.Add(typeof(long));
                typeInfo.Add(typeof(float?));
                typeInfo.Add(typeof(int?));
                typeInfo.Add(typeof(System.DateTime));
                typeInfo.Add(typeof(ulong));
                typeInfo.Add(typeof(uint?));
                typeInfo.Add(typeof(ulong?));
                typeInfo.Add(typeof(long?));
                typeInfo.Add(typeof(uint));
                typeInfo.Add(typeof(System.TimeSpan?));
                typeInfo.Add(typeof(short?));
                typeInfo.Add(typeof(System.Guid?));
                typeInfo.Add(typeof(int));
                typeInfo.Add(typeof(ushort?));
                typeInfo.Add(typeof(System.DateTime?));
                typeInfo.Add(typeof(char?));
                typeInfo.Add(typeof(decimal?));
                typeInfo.Add(typeof(float));
                typeInfo.Add(typeof(double?));
                typeInfo.Add(typeof(sbyte?));
                typeInfo.Add(typeof(ushort));
                typeInfo.Add(typeof(byte?));
                typeInfo.Add(typeof(char));
                typeInfo.Add(typeof(short));
                typeInfo.Add(typeof(sbyte));
                typeInfo.Add(typeof(bool?));
                typeInfo.Add(typeof(byte));
                typeInfo.Add(typeof(bool));
                typeInfo.Add(typeof(string));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatPropertyData value = default(AutoCSer.TestCase.SerializePerformance.FloatPropertyData);
                BinarySerialize(null, value);
                BinarySerializeMemberMap(null, null, value);
                BinaryDeserialize(null, ref value);
                BinaryDeserializeMemberMap(null, null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FloatPropertyData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FieldData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.FieldData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.FieldData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FieldData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatFieldData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.FloatFieldData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.FloatFieldData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FloatFieldData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatPropertyData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.FloatPropertyData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.FloatPropertyData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.FloatPropertyData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class JsonFloatFieldData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class JsonFloatPropertyData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class PropertyData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.SerializePerformance.PropertyData DefaultConstructor()
            {
                return new AutoCSer.TestCase.SerializePerformance.PropertyData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.SerializePerformance.PropertyData>();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FieldData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FieldData value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FieldData value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Bool"":");
                __serializer__.JsonSerialize(Bool);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""BoolNull"":");
                __serializer__.JsonSerialize(BoolNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Byte"":");
                __serializer__.JsonSerialize(Byte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ByteNull"":");
                __serializer__.JsonSerialize(ByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Char"":");
                __serializer__.JsonSerialize(Char);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""CharNull"":");
                __serializer__.JsonSerialize(CharNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTime"":");
                __serializer__.JsonSerialize(DateTime);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTimeNull"":");
                __serializer__.JsonSerialize(DateTimeNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Guid"":");
                __serializer__.JsonSerialize(Guid);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""GuidNull"":");
                __serializer__.JsonSerialize(GuidNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Int"":");
                __serializer__.JsonSerialize(Int);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""IntNull"":");
                __serializer__.JsonSerialize(IntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Long"":");
                __serializer__.JsonSerialize(Long);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""LongNull"":");
                __serializer__.JsonSerialize(LongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByte"":");
                __serializer__.JsonSerialize(SByte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByteNull"":");
                __serializer__.JsonSerialize(SByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Short"":");
                __serializer__.JsonSerialize(Short);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ShortNull"":");
                __serializer__.JsonSerialize(ShortNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""String"":");
                if (String == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(String);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpan"":");
                __serializer__.JsonSerialize(TimeSpan);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpanNull"":");
                __serializer__.JsonSerialize(TimeSpanNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UInt"":");
                __serializer__.JsonSerialize(UInt);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UIntNull"":");
                __serializer__.JsonSerialize(UIntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULong"":");
                __serializer__.JsonSerialize(ULong);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULongNull"":");
                __serializer__.JsonSerialize(ULongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShort"":");
                __serializer__.JsonSerialize(UShort);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShortNull"":");
                __serializer__.JsonSerialize(UShortNull);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Bool"":");
                    __serializer__.JsonSerialize(Bool);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""BoolNull"":");
                    __serializer__.JsonSerialize(BoolNull);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Byte"":");
                    __serializer__.JsonSerialize(Byte);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ByteNull"":");
                    __serializer__.JsonSerialize(ByteNull);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Char"":");
                    __serializer__.JsonSerialize(Char);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""CharNull"":");
                    __serializer__.JsonSerialize(CharNull);
                }
                if (__memberMap__.IsMember(6))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTime"":");
                    __serializer__.JsonSerialize(DateTime);
                }
                if (__memberMap__.IsMember(7))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTimeNull"":");
                    __serializer__.JsonSerialize(DateTimeNull);
                }
                if (__memberMap__.IsMember(8))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Guid"":");
                    __serializer__.JsonSerialize(Guid);
                }
                if (__memberMap__.IsMember(9))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""GuidNull"":");
                    __serializer__.JsonSerialize(GuidNull);
                }
                if (__memberMap__.IsMember(10))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Int"":");
                    __serializer__.JsonSerialize(Int);
                }
                if (__memberMap__.IsMember(11))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""IntNull"":");
                    __serializer__.JsonSerialize(IntNull);
                }
                if (__memberMap__.IsMember(12))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Long"":");
                    __serializer__.JsonSerialize(Long);
                }
                if (__memberMap__.IsMember(13))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""LongNull"":");
                    __serializer__.JsonSerialize(LongNull);
                }
                if (__memberMap__.IsMember(14))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByte"":");
                    __serializer__.JsonSerialize(SByte);
                }
                if (__memberMap__.IsMember(15))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByteNull"":");
                    __serializer__.JsonSerialize(SByteNull);
                }
                if (__memberMap__.IsMember(16))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Short"":");
                    __serializer__.JsonSerialize(Short);
                }
                if (__memberMap__.IsMember(17))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ShortNull"":");
                    __serializer__.JsonSerialize(ShortNull);
                }
                if (__memberMap__.IsMember(18))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""String"":");
                    if (String == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(String);
                }
                if (__memberMap__.IsMember(19))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpan"":");
                    __serializer__.JsonSerialize(TimeSpan);
                }
                if (__memberMap__.IsMember(20))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpanNull"":");
                    __serializer__.JsonSerialize(TimeSpanNull);
                }
                if (__memberMap__.IsMember(21))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UInt"":");
                    __serializer__.JsonSerialize(UInt);
                }
                if (__memberMap__.IsMember(22))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UIntNull"":");
                    __serializer__.JsonSerialize(UIntNull);
                }
                if (__memberMap__.IsMember(23))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULong"":");
                    __serializer__.JsonSerialize(ULong);
                }
                if (__memberMap__.IsMember(24))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULongNull"":");
                    __serializer__.JsonSerialize(ULongNull);
                }
                if (__memberMap__.IsMember(25))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShort"":");
                    __serializer__.JsonSerialize(UShort);
                }
                if (__memberMap__.IsMember(26))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShortNull"":");
                    __serializer__.JsonSerialize(UShortNull);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FieldData value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FieldData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Bool);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.BoolNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Byte);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ByteNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Char);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.CharNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTime);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTimeNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Guid);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.GuidNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Int);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.IntNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Long);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.LongNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByte);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByteNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Short);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ShortNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.String);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpan);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpanNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UInt);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UIntNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULong);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULongNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShort);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShortNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Bool);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.BoolNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Byte);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ByteNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Char);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.CharNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTime);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(6);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTimeNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(7);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Guid);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(8);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.GuidNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(9);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Int);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(10);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.IntNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(11);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Long);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(12);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.LongNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(13);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByte);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(14);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByteNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(15);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Short);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(16);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ShortNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(17);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.String);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(18);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpan);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(19);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpanNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(20);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UInt);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(21);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UIntNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(22);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULong);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(23);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULongNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(24);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShort);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(25);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShortNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(26);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FieldData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.Bool);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.BoolNull);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.Byte);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.ByteNull);
                        return;
                    case 4:
                        __deserializer__.JsonDeserialize(ref __value__.Char);
                        return;
                    case 5:
                        __deserializer__.JsonDeserialize(ref __value__.CharNull);
                        return;
                    case 6:
                        __deserializer__.JsonDeserialize(ref __value__.DateTime);
                        return;
                    case 7:
                        __deserializer__.JsonDeserialize(ref __value__.DateTimeNull);
                        return;
                    case 8:
                        __deserializer__.JsonDeserialize(ref __value__.Guid);
                        return;
                    case 9:
                        __deserializer__.JsonDeserialize(ref __value__.GuidNull);
                        return;
                    case 10:
                        __deserializer__.JsonDeserialize(ref __value__.Int);
                        return;
                    case 11:
                        __deserializer__.JsonDeserialize(ref __value__.IntNull);
                        return;
                    case 12:
                        __deserializer__.JsonDeserialize(ref __value__.Long);
                        return;
                    case 13:
                        __deserializer__.JsonDeserialize(ref __value__.LongNull);
                        return;
                    case 14:
                        __deserializer__.JsonDeserialize(ref __value__.SByte);
                        return;
                    case 15:
                        __deserializer__.JsonDeserialize(ref __value__.SByteNull);
                        return;
                    case 16:
                        __deserializer__.JsonDeserialize(ref __value__.Short);
                        return;
                    case 17:
                        __deserializer__.JsonDeserialize(ref __value__.ShortNull);
                        return;
                    case 18:
                        __deserializer__.JsonDeserialize(ref __value__.String);
                        return;
                    case 19:
                        __deserializer__.JsonDeserialize(ref __value__.TimeSpan);
                        return;
                    case 20:
                        __deserializer__.JsonDeserialize(ref __value__.TimeSpanNull);
                        return;
                    case 21:
                        __deserializer__.JsonDeserialize(ref __value__.UInt);
                        return;
                    case 22:
                        __deserializer__.JsonDeserialize(ref __value__.UIntNull);
                        return;
                    case 23:
                        __deserializer__.JsonDeserialize(ref __value__.ULong);
                        return;
                    case 24:
                        __deserializer__.JsonDeserialize(ref __value__.ULongNull);
                        return;
                    case 25:
                        __deserializer__.JsonDeserialize(ref __value__.UShort);
                        return;
                    case 26:
                        __deserializer__.JsonDeserialize(ref __value__.UShortNull);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(27);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(27);
                names.Add(nameof(Bool));
                indexs.Add(0);
                names.Add(nameof(BoolNull));
                indexs.Add(1);
                names.Add(nameof(Byte));
                indexs.Add(2);
                names.Add(nameof(ByteNull));
                indexs.Add(3);
                names.Add(nameof(Char));
                indexs.Add(4);
                names.Add(nameof(CharNull));
                indexs.Add(5);
                names.Add(nameof(DateTime));
                indexs.Add(6);
                names.Add(nameof(DateTimeNull));
                indexs.Add(7);
                names.Add(nameof(Guid));
                indexs.Add(8);
                names.Add(nameof(GuidNull));
                indexs.Add(9);
                names.Add(nameof(Int));
                indexs.Add(10);
                names.Add(nameof(IntNull));
                indexs.Add(11);
                names.Add(nameof(Long));
                indexs.Add(12);
                names.Add(nameof(LongNull));
                indexs.Add(13);
                names.Add(nameof(SByte));
                indexs.Add(14);
                names.Add(nameof(SByteNull));
                indexs.Add(15);
                names.Add(nameof(Short));
                indexs.Add(16);
                names.Add(nameof(ShortNull));
                indexs.Add(17);
                names.Add(nameof(String));
                indexs.Add(18);
                names.Add(nameof(TimeSpan));
                indexs.Add(19);
                names.Add(nameof(TimeSpanNull));
                indexs.Add(20);
                names.Add(nameof(UInt));
                indexs.Add(21);
                names.Add(nameof(UIntNull));
                indexs.Add(22);
                names.Add(nameof(ULong));
                indexs.Add(23);
                names.Add(nameof(ULongNull));
                indexs.Add(24);
                names.Add(nameof(UShort));
                indexs.Add(25);
                names.Add(nameof(UShortNull));
                indexs.Add(26);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(27);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FieldData value = default(AutoCSer.TestCase.SerializePerformance.FieldData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                JsonSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatFieldData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatFieldData value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatFieldData value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Bool"":");
                __serializer__.JsonSerialize(Bool);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""BoolNull"":");
                __serializer__.JsonSerialize(BoolNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Byte"":");
                __serializer__.JsonSerialize(Byte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ByteNull"":");
                __serializer__.JsonSerialize(ByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Char"":");
                __serializer__.JsonSerialize(Char);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""CharNull"":");
                __serializer__.JsonSerialize(CharNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTime"":");
                __serializer__.JsonSerialize(DateTime);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTimeNull"":");
                __serializer__.JsonSerialize(DateTimeNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Decimal"":");
                __serializer__.JsonSerialize(Decimal);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DecimalNull"":");
                __serializer__.JsonSerialize(DecimalNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Double"":");
                __serializer__.JsonSerialize(Double);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DoubleNull"":");
                __serializer__.JsonSerialize(DoubleNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Float"":");
                __serializer__.JsonSerialize(Float);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""FloatNull"":");
                __serializer__.JsonSerialize(FloatNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Guid"":");
                __serializer__.JsonSerialize(Guid);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""GuidNull"":");
                __serializer__.JsonSerialize(GuidNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Int"":");
                __serializer__.JsonSerialize(Int);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""IntNull"":");
                __serializer__.JsonSerialize(IntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Long"":");
                __serializer__.JsonSerialize(Long);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""LongNull"":");
                __serializer__.JsonSerialize(LongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByte"":");
                __serializer__.JsonSerialize(SByte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByteNull"":");
                __serializer__.JsonSerialize(SByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Short"":");
                __serializer__.JsonSerialize(Short);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ShortNull"":");
                __serializer__.JsonSerialize(ShortNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""String"":");
                if (String == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(String);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpan"":");
                __serializer__.JsonSerialize(TimeSpan);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpanNull"":");
                __serializer__.JsonSerialize(TimeSpanNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UInt"":");
                __serializer__.JsonSerialize(UInt);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UIntNull"":");
                __serializer__.JsonSerialize(UIntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULong"":");
                __serializer__.JsonSerialize(ULong);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULongNull"":");
                __serializer__.JsonSerialize(ULongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShort"":");
                __serializer__.JsonSerialize(UShort);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShortNull"":");
                __serializer__.JsonSerialize(UShortNull);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Bool"":");
                    __serializer__.JsonSerialize(Bool);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""BoolNull"":");
                    __serializer__.JsonSerialize(BoolNull);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Byte"":");
                    __serializer__.JsonSerialize(Byte);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ByteNull"":");
                    __serializer__.JsonSerialize(ByteNull);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Char"":");
                    __serializer__.JsonSerialize(Char);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""CharNull"":");
                    __serializer__.JsonSerialize(CharNull);
                }
                if (__memberMap__.IsMember(6))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTime"":");
                    __serializer__.JsonSerialize(DateTime);
                }
                if (__memberMap__.IsMember(7))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTimeNull"":");
                    __serializer__.JsonSerialize(DateTimeNull);
                }
                if (__memberMap__.IsMember(8))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Decimal"":");
                    __serializer__.JsonSerialize(Decimal);
                }
                if (__memberMap__.IsMember(9))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DecimalNull"":");
                    __serializer__.JsonSerialize(DecimalNull);
                }
                if (__memberMap__.IsMember(10))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Double"":");
                    __serializer__.JsonSerialize(Double);
                }
                if (__memberMap__.IsMember(11))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DoubleNull"":");
                    __serializer__.JsonSerialize(DoubleNull);
                }
                if (__memberMap__.IsMember(12))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Float"":");
                    __serializer__.JsonSerialize(Float);
                }
                if (__memberMap__.IsMember(13))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""FloatNull"":");
                    __serializer__.JsonSerialize(FloatNull);
                }
                if (__memberMap__.IsMember(14))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Guid"":");
                    __serializer__.JsonSerialize(Guid);
                }
                if (__memberMap__.IsMember(15))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""GuidNull"":");
                    __serializer__.JsonSerialize(GuidNull);
                }
                if (__memberMap__.IsMember(16))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Int"":");
                    __serializer__.JsonSerialize(Int);
                }
                if (__memberMap__.IsMember(17))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""IntNull"":");
                    __serializer__.JsonSerialize(IntNull);
                }
                if (__memberMap__.IsMember(18))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Long"":");
                    __serializer__.JsonSerialize(Long);
                }
                if (__memberMap__.IsMember(19))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""LongNull"":");
                    __serializer__.JsonSerialize(LongNull);
                }
                if (__memberMap__.IsMember(20))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByte"":");
                    __serializer__.JsonSerialize(SByte);
                }
                if (__memberMap__.IsMember(21))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByteNull"":");
                    __serializer__.JsonSerialize(SByteNull);
                }
                if (__memberMap__.IsMember(22))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Short"":");
                    __serializer__.JsonSerialize(Short);
                }
                if (__memberMap__.IsMember(23))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ShortNull"":");
                    __serializer__.JsonSerialize(ShortNull);
                }
                if (__memberMap__.IsMember(24))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""String"":");
                    if (String == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(String);
                }
                if (__memberMap__.IsMember(25))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpan"":");
                    __serializer__.JsonSerialize(TimeSpan);
                }
                if (__memberMap__.IsMember(26))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpanNull"":");
                    __serializer__.JsonSerialize(TimeSpanNull);
                }
                if (__memberMap__.IsMember(27))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UInt"":");
                    __serializer__.JsonSerialize(UInt);
                }
                if (__memberMap__.IsMember(28))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UIntNull"":");
                    __serializer__.JsonSerialize(UIntNull);
                }
                if (__memberMap__.IsMember(29))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULong"":");
                    __serializer__.JsonSerialize(ULong);
                }
                if (__memberMap__.IsMember(30))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULongNull"":");
                    __serializer__.JsonSerialize(ULongNull);
                }
                if (__memberMap__.IsMember(31))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShort"":");
                    __serializer__.JsonSerialize(UShort);
                }
                if (__memberMap__.IsMember(32))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShortNull"":");
                    __serializer__.JsonSerialize(UShortNull);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatFieldData value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatFieldData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Bool);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.BoolNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Byte);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ByteNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Char);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.CharNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTime);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTimeNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Decimal);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DecimalNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Double);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DoubleNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Float);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.FloatNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Guid);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.GuidNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Int);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.IntNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Long);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.LongNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByte);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByteNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Short);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ShortNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.String);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpan);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpanNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UInt);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UIntNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULong);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULongNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShort);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShortNull);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Bool);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.BoolNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Byte);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ByteNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Char);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.CharNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTime);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(6);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DateTimeNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(7);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Decimal);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(8);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DecimalNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(9);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Double);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(10);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.DoubleNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(11);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Float);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(12);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.FloatNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(13);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Guid);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(14);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.GuidNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(15);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Int);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(16);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.IntNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(17);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Long);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(18);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.LongNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(19);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByte);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(20);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.SByteNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(21);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Short);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(22);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ShortNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(23);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.String);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(24);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpan);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(25);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.TimeSpanNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(26);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UInt);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(27);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UIntNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(28);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULong);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(29);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ULongNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(30);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShort);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(31);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.UShortNull);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(32);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FloatFieldData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.Bool);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.BoolNull);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.Byte);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.ByteNull);
                        return;
                    case 4:
                        __deserializer__.JsonDeserialize(ref __value__.Char);
                        return;
                    case 5:
                        __deserializer__.JsonDeserialize(ref __value__.CharNull);
                        return;
                    case 6:
                        __deserializer__.JsonDeserialize(ref __value__.DateTime);
                        return;
                    case 7:
                        __deserializer__.JsonDeserialize(ref __value__.DateTimeNull);
                        return;
                    case 8:
                        __deserializer__.JsonDeserialize(ref __value__.Decimal);
                        return;
                    case 9:
                        __deserializer__.JsonDeserialize(ref __value__.DecimalNull);
                        return;
                    case 10:
                        __deserializer__.JsonDeserialize(ref __value__.Double);
                        return;
                    case 11:
                        __deserializer__.JsonDeserialize(ref __value__.DoubleNull);
                        return;
                    case 12:
                        __deserializer__.JsonDeserialize(ref __value__.Float);
                        return;
                    case 13:
                        __deserializer__.JsonDeserialize(ref __value__.FloatNull);
                        return;
                    case 14:
                        __deserializer__.JsonDeserialize(ref __value__.Guid);
                        return;
                    case 15:
                        __deserializer__.JsonDeserialize(ref __value__.GuidNull);
                        return;
                    case 16:
                        __deserializer__.JsonDeserialize(ref __value__.Int);
                        return;
                    case 17:
                        __deserializer__.JsonDeserialize(ref __value__.IntNull);
                        return;
                    case 18:
                        __deserializer__.JsonDeserialize(ref __value__.Long);
                        return;
                    case 19:
                        __deserializer__.JsonDeserialize(ref __value__.LongNull);
                        return;
                    case 20:
                        __deserializer__.JsonDeserialize(ref __value__.SByte);
                        return;
                    case 21:
                        __deserializer__.JsonDeserialize(ref __value__.SByteNull);
                        return;
                    case 22:
                        __deserializer__.JsonDeserialize(ref __value__.Short);
                        return;
                    case 23:
                        __deserializer__.JsonDeserialize(ref __value__.ShortNull);
                        return;
                    case 24:
                        __deserializer__.JsonDeserialize(ref __value__.String);
                        return;
                    case 25:
                        __deserializer__.JsonDeserialize(ref __value__.TimeSpan);
                        return;
                    case 26:
                        __deserializer__.JsonDeserialize(ref __value__.TimeSpanNull);
                        return;
                    case 27:
                        __deserializer__.JsonDeserialize(ref __value__.UInt);
                        return;
                    case 28:
                        __deserializer__.JsonDeserialize(ref __value__.UIntNull);
                        return;
                    case 29:
                        __deserializer__.JsonDeserialize(ref __value__.ULong);
                        return;
                    case 30:
                        __deserializer__.JsonDeserialize(ref __value__.ULongNull);
                        return;
                    case 31:
                        __deserializer__.JsonDeserialize(ref __value__.UShort);
                        return;
                    case 32:
                        __deserializer__.JsonDeserialize(ref __value__.UShortNull);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(33);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(33);
                names.Add(nameof(Bool));
                indexs.Add(0);
                names.Add(nameof(BoolNull));
                indexs.Add(1);
                names.Add(nameof(Byte));
                indexs.Add(2);
                names.Add(nameof(ByteNull));
                indexs.Add(3);
                names.Add(nameof(Char));
                indexs.Add(4);
                names.Add(nameof(CharNull));
                indexs.Add(5);
                names.Add(nameof(DateTime));
                indexs.Add(6);
                names.Add(nameof(DateTimeNull));
                indexs.Add(7);
                names.Add(nameof(Decimal));
                indexs.Add(8);
                names.Add(nameof(DecimalNull));
                indexs.Add(9);
                names.Add(nameof(Double));
                indexs.Add(10);
                names.Add(nameof(DoubleNull));
                indexs.Add(11);
                names.Add(nameof(Float));
                indexs.Add(12);
                names.Add(nameof(FloatNull));
                indexs.Add(13);
                names.Add(nameof(Guid));
                indexs.Add(14);
                names.Add(nameof(GuidNull));
                indexs.Add(15);
                names.Add(nameof(Int));
                indexs.Add(16);
                names.Add(nameof(IntNull));
                indexs.Add(17);
                names.Add(nameof(Long));
                indexs.Add(18);
                names.Add(nameof(LongNull));
                indexs.Add(19);
                names.Add(nameof(SByte));
                indexs.Add(20);
                names.Add(nameof(SByteNull));
                indexs.Add(21);
                names.Add(nameof(Short));
                indexs.Add(22);
                names.Add(nameof(ShortNull));
                indexs.Add(23);
                names.Add(nameof(String));
                indexs.Add(24);
                names.Add(nameof(TimeSpan));
                indexs.Add(25);
                names.Add(nameof(TimeSpanNull));
                indexs.Add(26);
                names.Add(nameof(UInt));
                indexs.Add(27);
                names.Add(nameof(UIntNull));
                indexs.Add(28);
                names.Add(nameof(ULong));
                indexs.Add(29);
                names.Add(nameof(ULongNull));
                indexs.Add(30);
                names.Add(nameof(UShort));
                indexs.Add(31);
                names.Add(nameof(UShortNull));
                indexs.Add(32);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(33);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(decimal));
                types.Add(typeof(decimal?));
                types.Add(typeof(double));
                types.Add(typeof(double?));
                types.Add(typeof(float));
                types.Add(typeof(float?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatFieldData value = default(AutoCSer.TestCase.SerializePerformance.FloatFieldData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                JsonSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatPropertyData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Bool"":");
                __serializer__.JsonSerialize(Bool);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""BoolNull"":");
                __serializer__.JsonSerialize(BoolNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Byte"":");
                __serializer__.JsonSerialize(Byte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ByteNull"":");
                __serializer__.JsonSerialize(ByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Char"":");
                __serializer__.JsonSerialize(Char);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""CharNull"":");
                __serializer__.JsonSerialize(CharNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTime"":");
                __serializer__.JsonSerialize(DateTime);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTimeNull"":");
                __serializer__.JsonSerialize(DateTimeNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Decimal"":");
                __serializer__.JsonSerialize(Decimal);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DecimalNull"":");
                __serializer__.JsonSerialize(DecimalNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Double"":");
                __serializer__.JsonSerialize(Double);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DoubleNull"":");
                __serializer__.JsonSerialize(DoubleNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Float"":");
                __serializer__.JsonSerialize(Float);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""FloatNull"":");
                __serializer__.JsonSerialize(FloatNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Guid"":");
                __serializer__.JsonSerialize(Guid);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""GuidNull"":");
                __serializer__.JsonSerialize(GuidNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Int"":");
                __serializer__.JsonSerialize(Int);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""IntNull"":");
                __serializer__.JsonSerialize(IntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Long"":");
                __serializer__.JsonSerialize(Long);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""LongNull"":");
                __serializer__.JsonSerialize(LongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByte"":");
                __serializer__.JsonSerialize(SByte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByteNull"":");
                __serializer__.JsonSerialize(SByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Short"":");
                __serializer__.JsonSerialize(Short);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ShortNull"":");
                __serializer__.JsonSerialize(ShortNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""String"":");
                if (String == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(String);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpan"":");
                __serializer__.JsonSerialize(TimeSpan);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpanNull"":");
                __serializer__.JsonSerialize(TimeSpanNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UInt"":");
                __serializer__.JsonSerialize(UInt);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UIntNull"":");
                __serializer__.JsonSerialize(UIntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULong"":");
                __serializer__.JsonSerialize(ULong);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULongNull"":");
                __serializer__.JsonSerialize(ULongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShort"":");
                __serializer__.JsonSerialize(UShort);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShortNull"":");
                __serializer__.JsonSerialize(UShortNull);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Bool"":");
                    __serializer__.JsonSerialize(Bool);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""BoolNull"":");
                    __serializer__.JsonSerialize(BoolNull);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Byte"":");
                    __serializer__.JsonSerialize(Byte);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ByteNull"":");
                    __serializer__.JsonSerialize(ByteNull);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Char"":");
                    __serializer__.JsonSerialize(Char);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""CharNull"":");
                    __serializer__.JsonSerialize(CharNull);
                }
                if (__memberMap__.IsMember(6))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTime"":");
                    __serializer__.JsonSerialize(DateTime);
                }
                if (__memberMap__.IsMember(7))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTimeNull"":");
                    __serializer__.JsonSerialize(DateTimeNull);
                }
                if (__memberMap__.IsMember(8))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Decimal"":");
                    __serializer__.JsonSerialize(Decimal);
                }
                if (__memberMap__.IsMember(9))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DecimalNull"":");
                    __serializer__.JsonSerialize(DecimalNull);
                }
                if (__memberMap__.IsMember(10))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Double"":");
                    __serializer__.JsonSerialize(Double);
                }
                if (__memberMap__.IsMember(11))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DoubleNull"":");
                    __serializer__.JsonSerialize(DoubleNull);
                }
                if (__memberMap__.IsMember(12))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Float"":");
                    __serializer__.JsonSerialize(Float);
                }
                if (__memberMap__.IsMember(13))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""FloatNull"":");
                    __serializer__.JsonSerialize(FloatNull);
                }
                if (__memberMap__.IsMember(14))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Guid"":");
                    __serializer__.JsonSerialize(Guid);
                }
                if (__memberMap__.IsMember(15))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""GuidNull"":");
                    __serializer__.JsonSerialize(GuidNull);
                }
                if (__memberMap__.IsMember(16))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Int"":");
                    __serializer__.JsonSerialize(Int);
                }
                if (__memberMap__.IsMember(17))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""IntNull"":");
                    __serializer__.JsonSerialize(IntNull);
                }
                if (__memberMap__.IsMember(18))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Long"":");
                    __serializer__.JsonSerialize(Long);
                }
                if (__memberMap__.IsMember(19))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""LongNull"":");
                    __serializer__.JsonSerialize(LongNull);
                }
                if (__memberMap__.IsMember(20))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByte"":");
                    __serializer__.JsonSerialize(SByte);
                }
                if (__memberMap__.IsMember(21))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByteNull"":");
                    __serializer__.JsonSerialize(SByteNull);
                }
                if (__memberMap__.IsMember(22))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Short"":");
                    __serializer__.JsonSerialize(Short);
                }
                if (__memberMap__.IsMember(23))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ShortNull"":");
                    __serializer__.JsonSerialize(ShortNull);
                }
                if (__memberMap__.IsMember(24))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""String"":");
                    if (String == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(String);
                }
                if (__memberMap__.IsMember(25))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpan"":");
                    __serializer__.JsonSerialize(TimeSpan);
                }
                if (__memberMap__.IsMember(26))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpanNull"":");
                    __serializer__.JsonSerialize(TimeSpanNull);
                }
                if (__memberMap__.IsMember(27))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UInt"":");
                    __serializer__.JsonSerialize(UInt);
                }
                if (__memberMap__.IsMember(28))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UIntNull"":");
                    __serializer__.JsonSerialize(UIntNull);
                }
                if (__memberMap__.IsMember(29))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULong"":");
                    __serializer__.JsonSerialize(ULong);
                }
                if (__memberMap__.IsMember(30))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULongNull"":");
                    __serializer__.JsonSerialize(ULongNull);
                }
                if (__memberMap__.IsMember(31))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShort"":");
                    __serializer__.JsonSerialize(UShort);
                }
                if (__memberMap__.IsMember(32))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShortNull"":");
                    __serializer__.JsonSerialize(UShortNull);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    var Bool = this.Bool;
                    __deserializer__.JsonDeserialize(ref Bool);
                    this.Bool = Bool;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var BoolNull = this.BoolNull;
                    __deserializer__.JsonDeserialize(ref BoolNull);
                    this.BoolNull = BoolNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Byte = this.Byte;
                    __deserializer__.JsonDeserialize(ref Byte);
                    this.Byte = Byte;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ByteNull = this.ByteNull;
                    __deserializer__.JsonDeserialize(ref ByteNull);
                    this.ByteNull = ByteNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Char = this.Char;
                    __deserializer__.JsonDeserialize(ref Char);
                    this.Char = Char;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var CharNull = this.CharNull;
                    __deserializer__.JsonDeserialize(ref CharNull);
                    this.CharNull = CharNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTime = this.DateTime;
                    __deserializer__.JsonDeserialize(ref DateTime);
                    this.DateTime = DateTime;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTimeNull = this.DateTimeNull;
                    __deserializer__.JsonDeserialize(ref DateTimeNull);
                    this.DateTimeNull = DateTimeNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Decimal = this.Decimal;
                    __deserializer__.JsonDeserialize(ref Decimal);
                    this.Decimal = Decimal;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DecimalNull = this.DecimalNull;
                    __deserializer__.JsonDeserialize(ref DecimalNull);
                    this.DecimalNull = DecimalNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Double = this.Double;
                    __deserializer__.JsonDeserialize(ref Double);
                    this.Double = Double;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DoubleNull = this.DoubleNull;
                    __deserializer__.JsonDeserialize(ref DoubleNull);
                    this.DoubleNull = DoubleNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Float = this.Float;
                    __deserializer__.JsonDeserialize(ref Float);
                    this.Float = Float;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var FloatNull = this.FloatNull;
                    __deserializer__.JsonDeserialize(ref FloatNull);
                    this.FloatNull = FloatNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Guid = this.Guid;
                    __deserializer__.JsonDeserialize(ref Guid);
                    this.Guid = Guid;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var GuidNull = this.GuidNull;
                    __deserializer__.JsonDeserialize(ref GuidNull);
                    this.GuidNull = GuidNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Int = this.Int;
                    __deserializer__.JsonDeserialize(ref Int);
                    this.Int = Int;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var IntNull = this.IntNull;
                    __deserializer__.JsonDeserialize(ref IntNull);
                    this.IntNull = IntNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Long = this.Long;
                    __deserializer__.JsonDeserialize(ref Long);
                    this.Long = Long;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var LongNull = this.LongNull;
                    __deserializer__.JsonDeserialize(ref LongNull);
                    this.LongNull = LongNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByte = this.SByte;
                    __deserializer__.JsonDeserialize(ref SByte);
                    this.SByte = SByte;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByteNull = this.SByteNull;
                    __deserializer__.JsonDeserialize(ref SByteNull);
                    this.SByteNull = SByteNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Short = this.Short;
                    __deserializer__.JsonDeserialize(ref Short);
                    this.Short = Short;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ShortNull = this.ShortNull;
                    __deserializer__.JsonDeserialize(ref ShortNull);
                    this.ShortNull = ShortNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var String = this.String;
                    __deserializer__.JsonDeserialize(ref String);
                    this.String = String;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpan = this.TimeSpan;
                    __deserializer__.JsonDeserialize(ref TimeSpan);
                    this.TimeSpan = TimeSpan;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpanNull = this.TimeSpanNull;
                    __deserializer__.JsonDeserialize(ref TimeSpanNull);
                    this.TimeSpanNull = TimeSpanNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UInt = this.UInt;
                    __deserializer__.JsonDeserialize(ref UInt);
                    this.UInt = UInt;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UIntNull = this.UIntNull;
                    __deserializer__.JsonDeserialize(ref UIntNull);
                    this.UIntNull = UIntNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULong = this.ULong;
                    __deserializer__.JsonDeserialize(ref ULong);
                    this.ULong = ULong;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULongNull = this.ULongNull;
                    __deserializer__.JsonDeserialize(ref ULongNull);
                    this.ULongNull = ULongNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShort = this.UShort;
                    __deserializer__.JsonDeserialize(ref UShort);
                    this.UShort = UShort;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShortNull = this.UShortNull;
                    __deserializer__.JsonDeserialize(ref UShortNull);
                    this.UShortNull = UShortNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    var Bool = this.Bool;
                    __deserializer__.JsonDeserialize(ref Bool);
                    this.Bool = Bool;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var BoolNull = this.BoolNull;
                    __deserializer__.JsonDeserialize(ref BoolNull);
                    this.BoolNull = BoolNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Byte = this.Byte;
                    __deserializer__.JsonDeserialize(ref Byte);
                    this.Byte = Byte;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ByteNull = this.ByteNull;
                    __deserializer__.JsonDeserialize(ref ByteNull);
                    this.ByteNull = ByteNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Char = this.Char;
                    __deserializer__.JsonDeserialize(ref Char);
                    this.Char = Char;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var CharNull = this.CharNull;
                    __deserializer__.JsonDeserialize(ref CharNull);
                    this.CharNull = CharNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTime = this.DateTime;
                    __deserializer__.JsonDeserialize(ref DateTime);
                    this.DateTime = DateTime;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(6);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTimeNull = this.DateTimeNull;
                    __deserializer__.JsonDeserialize(ref DateTimeNull);
                    this.DateTimeNull = DateTimeNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(7);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Decimal = this.Decimal;
                    __deserializer__.JsonDeserialize(ref Decimal);
                    this.Decimal = Decimal;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(8);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DecimalNull = this.DecimalNull;
                    __deserializer__.JsonDeserialize(ref DecimalNull);
                    this.DecimalNull = DecimalNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(9);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Double = this.Double;
                    __deserializer__.JsonDeserialize(ref Double);
                    this.Double = Double;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(10);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DoubleNull = this.DoubleNull;
                    __deserializer__.JsonDeserialize(ref DoubleNull);
                    this.DoubleNull = DoubleNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(11);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Float = this.Float;
                    __deserializer__.JsonDeserialize(ref Float);
                    this.Float = Float;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(12);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var FloatNull = this.FloatNull;
                    __deserializer__.JsonDeserialize(ref FloatNull);
                    this.FloatNull = FloatNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(13);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Guid = this.Guid;
                    __deserializer__.JsonDeserialize(ref Guid);
                    this.Guid = Guid;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(14);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var GuidNull = this.GuidNull;
                    __deserializer__.JsonDeserialize(ref GuidNull);
                    this.GuidNull = GuidNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(15);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Int = this.Int;
                    __deserializer__.JsonDeserialize(ref Int);
                    this.Int = Int;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(16);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var IntNull = this.IntNull;
                    __deserializer__.JsonDeserialize(ref IntNull);
                    this.IntNull = IntNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(17);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Long = this.Long;
                    __deserializer__.JsonDeserialize(ref Long);
                    this.Long = Long;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(18);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var LongNull = this.LongNull;
                    __deserializer__.JsonDeserialize(ref LongNull);
                    this.LongNull = LongNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(19);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByte = this.SByte;
                    __deserializer__.JsonDeserialize(ref SByte);
                    this.SByte = SByte;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(20);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByteNull = this.SByteNull;
                    __deserializer__.JsonDeserialize(ref SByteNull);
                    this.SByteNull = SByteNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(21);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Short = this.Short;
                    __deserializer__.JsonDeserialize(ref Short);
                    this.Short = Short;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(22);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ShortNull = this.ShortNull;
                    __deserializer__.JsonDeserialize(ref ShortNull);
                    this.ShortNull = ShortNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(23);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var String = this.String;
                    __deserializer__.JsonDeserialize(ref String);
                    this.String = String;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(24);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpan = this.TimeSpan;
                    __deserializer__.JsonDeserialize(ref TimeSpan);
                    this.TimeSpan = TimeSpan;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(25);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpanNull = this.TimeSpanNull;
                    __deserializer__.JsonDeserialize(ref TimeSpanNull);
                    this.TimeSpanNull = TimeSpanNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(26);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UInt = this.UInt;
                    __deserializer__.JsonDeserialize(ref UInt);
                    this.UInt = UInt;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(27);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UIntNull = this.UIntNull;
                    __deserializer__.JsonDeserialize(ref UIntNull);
                    this.UIntNull = UIntNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(28);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULong = this.ULong;
                    __deserializer__.JsonDeserialize(ref ULong);
                    this.ULong = ULong;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(29);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULongNull = this.ULongNull;
                    __deserializer__.JsonDeserialize(ref ULongNull);
                    this.ULongNull = ULongNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(30);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShort = this.UShort;
                    __deserializer__.JsonDeserialize(ref UShort);
                    this.UShort = UShort;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(31);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShortNull = this.UShortNull;
                    __deserializer__.JsonDeserialize(ref UShortNull);
                    this.UShortNull = UShortNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(32);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        var Bool = __value__.Bool;
                        __deserializer__.JsonDeserialize(ref Bool);
                        __value__.Bool = Bool;
                        return;
                    case 1:
                        var BoolNull = __value__.BoolNull;
                        __deserializer__.JsonDeserialize(ref BoolNull);
                        __value__.BoolNull = BoolNull;
                        return;
                    case 2:
                        var Byte = __value__.Byte;
                        __deserializer__.JsonDeserialize(ref Byte);
                        __value__.Byte = Byte;
                        return;
                    case 3:
                        var ByteNull = __value__.ByteNull;
                        __deserializer__.JsonDeserialize(ref ByteNull);
                        __value__.ByteNull = ByteNull;
                        return;
                    case 4:
                        var Char = __value__.Char;
                        __deserializer__.JsonDeserialize(ref Char);
                        __value__.Char = Char;
                        return;
                    case 5:
                        var CharNull = __value__.CharNull;
                        __deserializer__.JsonDeserialize(ref CharNull);
                        __value__.CharNull = CharNull;
                        return;
                    case 6:
                        var DateTime = __value__.DateTime;
                        __deserializer__.JsonDeserialize(ref DateTime);
                        __value__.DateTime = DateTime;
                        return;
                    case 7:
                        var DateTimeNull = __value__.DateTimeNull;
                        __deserializer__.JsonDeserialize(ref DateTimeNull);
                        __value__.DateTimeNull = DateTimeNull;
                        return;
                    case 8:
                        var Decimal = __value__.Decimal;
                        __deserializer__.JsonDeserialize(ref Decimal);
                        __value__.Decimal = Decimal;
                        return;
                    case 9:
                        var DecimalNull = __value__.DecimalNull;
                        __deserializer__.JsonDeserialize(ref DecimalNull);
                        __value__.DecimalNull = DecimalNull;
                        return;
                    case 10:
                        var Double = __value__.Double;
                        __deserializer__.JsonDeserialize(ref Double);
                        __value__.Double = Double;
                        return;
                    case 11:
                        var DoubleNull = __value__.DoubleNull;
                        __deserializer__.JsonDeserialize(ref DoubleNull);
                        __value__.DoubleNull = DoubleNull;
                        return;
                    case 12:
                        var Float = __value__.Float;
                        __deserializer__.JsonDeserialize(ref Float);
                        __value__.Float = Float;
                        return;
                    case 13:
                        var FloatNull = __value__.FloatNull;
                        __deserializer__.JsonDeserialize(ref FloatNull);
                        __value__.FloatNull = FloatNull;
                        return;
                    case 14:
                        var Guid = __value__.Guid;
                        __deserializer__.JsonDeserialize(ref Guid);
                        __value__.Guid = Guid;
                        return;
                    case 15:
                        var GuidNull = __value__.GuidNull;
                        __deserializer__.JsonDeserialize(ref GuidNull);
                        __value__.GuidNull = GuidNull;
                        return;
                    case 16:
                        var Int = __value__.Int;
                        __deserializer__.JsonDeserialize(ref Int);
                        __value__.Int = Int;
                        return;
                    case 17:
                        var IntNull = __value__.IntNull;
                        __deserializer__.JsonDeserialize(ref IntNull);
                        __value__.IntNull = IntNull;
                        return;
                    case 18:
                        var Long = __value__.Long;
                        __deserializer__.JsonDeserialize(ref Long);
                        __value__.Long = Long;
                        return;
                    case 19:
                        var LongNull = __value__.LongNull;
                        __deserializer__.JsonDeserialize(ref LongNull);
                        __value__.LongNull = LongNull;
                        return;
                    case 20:
                        var SByte = __value__.SByte;
                        __deserializer__.JsonDeserialize(ref SByte);
                        __value__.SByte = SByte;
                        return;
                    case 21:
                        var SByteNull = __value__.SByteNull;
                        __deserializer__.JsonDeserialize(ref SByteNull);
                        __value__.SByteNull = SByteNull;
                        return;
                    case 22:
                        var Short = __value__.Short;
                        __deserializer__.JsonDeserialize(ref Short);
                        __value__.Short = Short;
                        return;
                    case 23:
                        var ShortNull = __value__.ShortNull;
                        __deserializer__.JsonDeserialize(ref ShortNull);
                        __value__.ShortNull = ShortNull;
                        return;
                    case 24:
                        var String = __value__.String;
                        __deserializer__.JsonDeserialize(ref String);
                        __value__.String = String;
                        return;
                    case 25:
                        var TimeSpan = __value__.TimeSpan;
                        __deserializer__.JsonDeserialize(ref TimeSpan);
                        __value__.TimeSpan = TimeSpan;
                        return;
                    case 26:
                        var TimeSpanNull = __value__.TimeSpanNull;
                        __deserializer__.JsonDeserialize(ref TimeSpanNull);
                        __value__.TimeSpanNull = TimeSpanNull;
                        return;
                    case 27:
                        var UInt = __value__.UInt;
                        __deserializer__.JsonDeserialize(ref UInt);
                        __value__.UInt = UInt;
                        return;
                    case 28:
                        var UIntNull = __value__.UIntNull;
                        __deserializer__.JsonDeserialize(ref UIntNull);
                        __value__.UIntNull = UIntNull;
                        return;
                    case 29:
                        var ULong = __value__.ULong;
                        __deserializer__.JsonDeserialize(ref ULong);
                        __value__.ULong = ULong;
                        return;
                    case 30:
                        var ULongNull = __value__.ULongNull;
                        __deserializer__.JsonDeserialize(ref ULongNull);
                        __value__.ULongNull = ULongNull;
                        return;
                    case 31:
                        var UShort = __value__.UShort;
                        __deserializer__.JsonDeserialize(ref UShort);
                        __value__.UShort = UShort;
                        return;
                    case 32:
                        var UShortNull = __value__.UShortNull;
                        __deserializer__.JsonDeserialize(ref UShortNull);
                        __value__.UShortNull = UShortNull;
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(33);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(33);
                names.Add(nameof(Bool));
                indexs.Add(0);
                names.Add(nameof(BoolNull));
                indexs.Add(1);
                names.Add(nameof(Byte));
                indexs.Add(2);
                names.Add(nameof(ByteNull));
                indexs.Add(3);
                names.Add(nameof(Char));
                indexs.Add(4);
                names.Add(nameof(CharNull));
                indexs.Add(5);
                names.Add(nameof(DateTime));
                indexs.Add(6);
                names.Add(nameof(DateTimeNull));
                indexs.Add(7);
                names.Add(nameof(Decimal));
                indexs.Add(8);
                names.Add(nameof(DecimalNull));
                indexs.Add(9);
                names.Add(nameof(Double));
                indexs.Add(10);
                names.Add(nameof(DoubleNull));
                indexs.Add(11);
                names.Add(nameof(Float));
                indexs.Add(12);
                names.Add(nameof(FloatNull));
                indexs.Add(13);
                names.Add(nameof(Guid));
                indexs.Add(14);
                names.Add(nameof(GuidNull));
                indexs.Add(15);
                names.Add(nameof(Int));
                indexs.Add(16);
                names.Add(nameof(IntNull));
                indexs.Add(17);
                names.Add(nameof(Long));
                indexs.Add(18);
                names.Add(nameof(LongNull));
                indexs.Add(19);
                names.Add(nameof(SByte));
                indexs.Add(20);
                names.Add(nameof(SByteNull));
                indexs.Add(21);
                names.Add(nameof(Short));
                indexs.Add(22);
                names.Add(nameof(ShortNull));
                indexs.Add(23);
                names.Add(nameof(String));
                indexs.Add(24);
                names.Add(nameof(TimeSpan));
                indexs.Add(25);
                names.Add(nameof(TimeSpanNull));
                indexs.Add(26);
                names.Add(nameof(UInt));
                indexs.Add(27);
                names.Add(nameof(UIntNull));
                indexs.Add(28);
                names.Add(nameof(ULong));
                indexs.Add(29);
                names.Add(nameof(ULongNull));
                indexs.Add(30);
                names.Add(nameof(UShort));
                indexs.Add(31);
                names.Add(nameof(UShortNull));
                indexs.Add(32);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(33);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(decimal));
                types.Add(typeof(decimal?));
                types.Add(typeof(double));
                types.Add(typeof(double?));
                types.Add(typeof(float));
                types.Add(typeof(float?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatPropertyData value = default(AutoCSer.TestCase.SerializePerformance.FloatPropertyData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                JsonSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class PropertyData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.PropertyData value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.SerializePerformance.PropertyData value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Bool"":");
                __serializer__.JsonSerialize(Bool);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""BoolNull"":");
                __serializer__.JsonSerialize(BoolNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Byte"":");
                __serializer__.JsonSerialize(Byte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ByteNull"":");
                __serializer__.JsonSerialize(ByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Char"":");
                __serializer__.JsonSerialize(Char);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""CharNull"":");
                __serializer__.JsonSerialize(CharNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTime"":");
                __serializer__.JsonSerialize(DateTime);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""DateTimeNull"":");
                __serializer__.JsonSerialize(DateTimeNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Guid"":");
                __serializer__.JsonSerialize(Guid);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""GuidNull"":");
                __serializer__.JsonSerialize(GuidNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Int"":");
                __serializer__.JsonSerialize(Int);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""IntNull"":");
                __serializer__.JsonSerialize(IntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Long"":");
                __serializer__.JsonSerialize(Long);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""LongNull"":");
                __serializer__.JsonSerialize(LongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByte"":");
                __serializer__.JsonSerialize(SByte);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""SByteNull"":");
                __serializer__.JsonSerialize(SByteNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Short"":");
                __serializer__.JsonSerialize(Short);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ShortNull"":");
                __serializer__.JsonSerialize(ShortNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""String"":");
                if (String == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(String);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpan"":");
                __serializer__.JsonSerialize(TimeSpan);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""TimeSpanNull"":");
                __serializer__.JsonSerialize(TimeSpanNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UInt"":");
                __serializer__.JsonSerialize(UInt);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UIntNull"":");
                __serializer__.JsonSerialize(UIntNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULong"":");
                __serializer__.JsonSerialize(ULong);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""ULongNull"":");
                __serializer__.JsonSerialize(ULongNull);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShort"":");
                __serializer__.JsonSerialize(UShort);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""UShortNull"":");
                __serializer__.JsonSerialize(UShortNull);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Bool"":");
                    __serializer__.JsonSerialize(Bool);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""BoolNull"":");
                    __serializer__.JsonSerialize(BoolNull);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Byte"":");
                    __serializer__.JsonSerialize(Byte);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ByteNull"":");
                    __serializer__.JsonSerialize(ByteNull);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Char"":");
                    __serializer__.JsonSerialize(Char);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""CharNull"":");
                    __serializer__.JsonSerialize(CharNull);
                }
                if (__memberMap__.IsMember(6))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTime"":");
                    __serializer__.JsonSerialize(DateTime);
                }
                if (__memberMap__.IsMember(7))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""DateTimeNull"":");
                    __serializer__.JsonSerialize(DateTimeNull);
                }
                if (__memberMap__.IsMember(8))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Guid"":");
                    __serializer__.JsonSerialize(Guid);
                }
                if (__memberMap__.IsMember(9))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""GuidNull"":");
                    __serializer__.JsonSerialize(GuidNull);
                }
                if (__memberMap__.IsMember(10))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Int"":");
                    __serializer__.JsonSerialize(Int);
                }
                if (__memberMap__.IsMember(11))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""IntNull"":");
                    __serializer__.JsonSerialize(IntNull);
                }
                if (__memberMap__.IsMember(12))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Long"":");
                    __serializer__.JsonSerialize(Long);
                }
                if (__memberMap__.IsMember(13))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""LongNull"":");
                    __serializer__.JsonSerialize(LongNull);
                }
                if (__memberMap__.IsMember(14))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByte"":");
                    __serializer__.JsonSerialize(SByte);
                }
                if (__memberMap__.IsMember(15))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""SByteNull"":");
                    __serializer__.JsonSerialize(SByteNull);
                }
                if (__memberMap__.IsMember(16))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Short"":");
                    __serializer__.JsonSerialize(Short);
                }
                if (__memberMap__.IsMember(17))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ShortNull"":");
                    __serializer__.JsonSerialize(ShortNull);
                }
                if (__memberMap__.IsMember(18))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""String"":");
                    if (String == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(String);
                }
                if (__memberMap__.IsMember(19))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpan"":");
                    __serializer__.JsonSerialize(TimeSpan);
                }
                if (__memberMap__.IsMember(20))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""TimeSpanNull"":");
                    __serializer__.JsonSerialize(TimeSpanNull);
                }
                if (__memberMap__.IsMember(21))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UInt"":");
                    __serializer__.JsonSerialize(UInt);
                }
                if (__memberMap__.IsMember(22))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UIntNull"":");
                    __serializer__.JsonSerialize(UIntNull);
                }
                if (__memberMap__.IsMember(23))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULong"":");
                    __serializer__.JsonSerialize(ULong);
                }
                if (__memberMap__.IsMember(24))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ULongNull"":");
                    __serializer__.JsonSerialize(ULongNull);
                }
                if (__memberMap__.IsMember(25))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShort"":");
                    __serializer__.JsonSerialize(UShort);
                }
                if (__memberMap__.IsMember(26))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""UShortNull"":");
                    __serializer__.JsonSerialize(UShortNull);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.PropertyData value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.SerializePerformance.PropertyData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    var Bool = this.Bool;
                    __deserializer__.JsonDeserialize(ref Bool);
                    this.Bool = Bool;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var BoolNull = this.BoolNull;
                    __deserializer__.JsonDeserialize(ref BoolNull);
                    this.BoolNull = BoolNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Byte = this.Byte;
                    __deserializer__.JsonDeserialize(ref Byte);
                    this.Byte = Byte;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ByteNull = this.ByteNull;
                    __deserializer__.JsonDeserialize(ref ByteNull);
                    this.ByteNull = ByteNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Char = this.Char;
                    __deserializer__.JsonDeserialize(ref Char);
                    this.Char = Char;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var CharNull = this.CharNull;
                    __deserializer__.JsonDeserialize(ref CharNull);
                    this.CharNull = CharNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTime = this.DateTime;
                    __deserializer__.JsonDeserialize(ref DateTime);
                    this.DateTime = DateTime;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTimeNull = this.DateTimeNull;
                    __deserializer__.JsonDeserialize(ref DateTimeNull);
                    this.DateTimeNull = DateTimeNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Guid = this.Guid;
                    __deserializer__.JsonDeserialize(ref Guid);
                    this.Guid = Guid;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var GuidNull = this.GuidNull;
                    __deserializer__.JsonDeserialize(ref GuidNull);
                    this.GuidNull = GuidNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Int = this.Int;
                    __deserializer__.JsonDeserialize(ref Int);
                    this.Int = Int;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var IntNull = this.IntNull;
                    __deserializer__.JsonDeserialize(ref IntNull);
                    this.IntNull = IntNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Long = this.Long;
                    __deserializer__.JsonDeserialize(ref Long);
                    this.Long = Long;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var LongNull = this.LongNull;
                    __deserializer__.JsonDeserialize(ref LongNull);
                    this.LongNull = LongNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByte = this.SByte;
                    __deserializer__.JsonDeserialize(ref SByte);
                    this.SByte = SByte;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByteNull = this.SByteNull;
                    __deserializer__.JsonDeserialize(ref SByteNull);
                    this.SByteNull = SByteNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Short = this.Short;
                    __deserializer__.JsonDeserialize(ref Short);
                    this.Short = Short;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ShortNull = this.ShortNull;
                    __deserializer__.JsonDeserialize(ref ShortNull);
                    this.ShortNull = ShortNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var String = this.String;
                    __deserializer__.JsonDeserialize(ref String);
                    this.String = String;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpan = this.TimeSpan;
                    __deserializer__.JsonDeserialize(ref TimeSpan);
                    this.TimeSpan = TimeSpan;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpanNull = this.TimeSpanNull;
                    __deserializer__.JsonDeserialize(ref TimeSpanNull);
                    this.TimeSpanNull = TimeSpanNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UInt = this.UInt;
                    __deserializer__.JsonDeserialize(ref UInt);
                    this.UInt = UInt;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UIntNull = this.UIntNull;
                    __deserializer__.JsonDeserialize(ref UIntNull);
                    this.UIntNull = UIntNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULong = this.ULong;
                    __deserializer__.JsonDeserialize(ref ULong);
                    this.ULong = ULong;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULongNull = this.ULongNull;
                    __deserializer__.JsonDeserialize(ref ULongNull);
                    this.ULongNull = ULongNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShort = this.UShort;
                    __deserializer__.JsonDeserialize(ref UShort);
                    this.UShort = UShort;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShortNull = this.UShortNull;
                    __deserializer__.JsonDeserialize(ref UShortNull);
                    this.UShortNull = UShortNull;
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    var Bool = this.Bool;
                    __deserializer__.JsonDeserialize(ref Bool);
                    this.Bool = Bool;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var BoolNull = this.BoolNull;
                    __deserializer__.JsonDeserialize(ref BoolNull);
                    this.BoolNull = BoolNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Byte = this.Byte;
                    __deserializer__.JsonDeserialize(ref Byte);
                    this.Byte = Byte;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ByteNull = this.ByteNull;
                    __deserializer__.JsonDeserialize(ref ByteNull);
                    this.ByteNull = ByteNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Char = this.Char;
                    __deserializer__.JsonDeserialize(ref Char);
                    this.Char = Char;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var CharNull = this.CharNull;
                    __deserializer__.JsonDeserialize(ref CharNull);
                    this.CharNull = CharNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTime = this.DateTime;
                    __deserializer__.JsonDeserialize(ref DateTime);
                    this.DateTime = DateTime;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(6);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var DateTimeNull = this.DateTimeNull;
                    __deserializer__.JsonDeserialize(ref DateTimeNull);
                    this.DateTimeNull = DateTimeNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(7);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Guid = this.Guid;
                    __deserializer__.JsonDeserialize(ref Guid);
                    this.Guid = Guid;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(8);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var GuidNull = this.GuidNull;
                    __deserializer__.JsonDeserialize(ref GuidNull);
                    this.GuidNull = GuidNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(9);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Int = this.Int;
                    __deserializer__.JsonDeserialize(ref Int);
                    this.Int = Int;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(10);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var IntNull = this.IntNull;
                    __deserializer__.JsonDeserialize(ref IntNull);
                    this.IntNull = IntNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(11);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Long = this.Long;
                    __deserializer__.JsonDeserialize(ref Long);
                    this.Long = Long;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(12);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var LongNull = this.LongNull;
                    __deserializer__.JsonDeserialize(ref LongNull);
                    this.LongNull = LongNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(13);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByte = this.SByte;
                    __deserializer__.JsonDeserialize(ref SByte);
                    this.SByte = SByte;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(14);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var SByteNull = this.SByteNull;
                    __deserializer__.JsonDeserialize(ref SByteNull);
                    this.SByteNull = SByteNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(15);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var Short = this.Short;
                    __deserializer__.JsonDeserialize(ref Short);
                    this.Short = Short;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(16);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ShortNull = this.ShortNull;
                    __deserializer__.JsonDeserialize(ref ShortNull);
                    this.ShortNull = ShortNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(17);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var String = this.String;
                    __deserializer__.JsonDeserialize(ref String);
                    this.String = String;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(18);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpan = this.TimeSpan;
                    __deserializer__.JsonDeserialize(ref TimeSpan);
                    this.TimeSpan = TimeSpan;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(19);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var TimeSpanNull = this.TimeSpanNull;
                    __deserializer__.JsonDeserialize(ref TimeSpanNull);
                    this.TimeSpanNull = TimeSpanNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(20);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UInt = this.UInt;
                    __deserializer__.JsonDeserialize(ref UInt);
                    this.UInt = UInt;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(21);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UIntNull = this.UIntNull;
                    __deserializer__.JsonDeserialize(ref UIntNull);
                    this.UIntNull = UIntNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(22);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULong = this.ULong;
                    __deserializer__.JsonDeserialize(ref ULong);
                    this.ULong = ULong;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(23);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var ULongNull = this.ULongNull;
                    __deserializer__.JsonDeserialize(ref ULongNull);
                    this.ULongNull = ULongNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(24);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShort = this.UShort;
                    __deserializer__.JsonDeserialize(ref UShort);
                    this.UShort = UShort;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(25);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    var UShortNull = this.UShortNull;
                    __deserializer__.JsonDeserialize(ref UShortNull);
                    this.UShortNull = UShortNull;
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(26);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.PropertyData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        var Bool = __value__.Bool;
                        __deserializer__.JsonDeserialize(ref Bool);
                        __value__.Bool = Bool;
                        return;
                    case 1:
                        var BoolNull = __value__.BoolNull;
                        __deserializer__.JsonDeserialize(ref BoolNull);
                        __value__.BoolNull = BoolNull;
                        return;
                    case 2:
                        var Byte = __value__.Byte;
                        __deserializer__.JsonDeserialize(ref Byte);
                        __value__.Byte = Byte;
                        return;
                    case 3:
                        var ByteNull = __value__.ByteNull;
                        __deserializer__.JsonDeserialize(ref ByteNull);
                        __value__.ByteNull = ByteNull;
                        return;
                    case 4:
                        var Char = __value__.Char;
                        __deserializer__.JsonDeserialize(ref Char);
                        __value__.Char = Char;
                        return;
                    case 5:
                        var CharNull = __value__.CharNull;
                        __deserializer__.JsonDeserialize(ref CharNull);
                        __value__.CharNull = CharNull;
                        return;
                    case 6:
                        var DateTime = __value__.DateTime;
                        __deserializer__.JsonDeserialize(ref DateTime);
                        __value__.DateTime = DateTime;
                        return;
                    case 7:
                        var DateTimeNull = __value__.DateTimeNull;
                        __deserializer__.JsonDeserialize(ref DateTimeNull);
                        __value__.DateTimeNull = DateTimeNull;
                        return;
                    case 8:
                        var Guid = __value__.Guid;
                        __deserializer__.JsonDeserialize(ref Guid);
                        __value__.Guid = Guid;
                        return;
                    case 9:
                        var GuidNull = __value__.GuidNull;
                        __deserializer__.JsonDeserialize(ref GuidNull);
                        __value__.GuidNull = GuidNull;
                        return;
                    case 10:
                        var Int = __value__.Int;
                        __deserializer__.JsonDeserialize(ref Int);
                        __value__.Int = Int;
                        return;
                    case 11:
                        var IntNull = __value__.IntNull;
                        __deserializer__.JsonDeserialize(ref IntNull);
                        __value__.IntNull = IntNull;
                        return;
                    case 12:
                        var Long = __value__.Long;
                        __deserializer__.JsonDeserialize(ref Long);
                        __value__.Long = Long;
                        return;
                    case 13:
                        var LongNull = __value__.LongNull;
                        __deserializer__.JsonDeserialize(ref LongNull);
                        __value__.LongNull = LongNull;
                        return;
                    case 14:
                        var SByte = __value__.SByte;
                        __deserializer__.JsonDeserialize(ref SByte);
                        __value__.SByte = SByte;
                        return;
                    case 15:
                        var SByteNull = __value__.SByteNull;
                        __deserializer__.JsonDeserialize(ref SByteNull);
                        __value__.SByteNull = SByteNull;
                        return;
                    case 16:
                        var Short = __value__.Short;
                        __deserializer__.JsonDeserialize(ref Short);
                        __value__.Short = Short;
                        return;
                    case 17:
                        var ShortNull = __value__.ShortNull;
                        __deserializer__.JsonDeserialize(ref ShortNull);
                        __value__.ShortNull = ShortNull;
                        return;
                    case 18:
                        var String = __value__.String;
                        __deserializer__.JsonDeserialize(ref String);
                        __value__.String = String;
                        return;
                    case 19:
                        var TimeSpan = __value__.TimeSpan;
                        __deserializer__.JsonDeserialize(ref TimeSpan);
                        __value__.TimeSpan = TimeSpan;
                        return;
                    case 20:
                        var TimeSpanNull = __value__.TimeSpanNull;
                        __deserializer__.JsonDeserialize(ref TimeSpanNull);
                        __value__.TimeSpanNull = TimeSpanNull;
                        return;
                    case 21:
                        var UInt = __value__.UInt;
                        __deserializer__.JsonDeserialize(ref UInt);
                        __value__.UInt = UInt;
                        return;
                    case 22:
                        var UIntNull = __value__.UIntNull;
                        __deserializer__.JsonDeserialize(ref UIntNull);
                        __value__.UIntNull = UIntNull;
                        return;
                    case 23:
                        var ULong = __value__.ULong;
                        __deserializer__.JsonDeserialize(ref ULong);
                        __value__.ULong = ULong;
                        return;
                    case 24:
                        var ULongNull = __value__.ULongNull;
                        __deserializer__.JsonDeserialize(ref ULongNull);
                        __value__.ULongNull = ULongNull;
                        return;
                    case 25:
                        var UShort = __value__.UShort;
                        __deserializer__.JsonDeserialize(ref UShort);
                        __value__.UShort = UShort;
                        return;
                    case 26:
                        var UShortNull = __value__.UShortNull;
                        __deserializer__.JsonDeserialize(ref UShortNull);
                        __value__.UShortNull = UShortNull;
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(27);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(27);
                names.Add(nameof(Bool));
                indexs.Add(0);
                names.Add(nameof(BoolNull));
                indexs.Add(1);
                names.Add(nameof(Byte));
                indexs.Add(2);
                names.Add(nameof(ByteNull));
                indexs.Add(3);
                names.Add(nameof(Char));
                indexs.Add(4);
                names.Add(nameof(CharNull));
                indexs.Add(5);
                names.Add(nameof(DateTime));
                indexs.Add(6);
                names.Add(nameof(DateTimeNull));
                indexs.Add(7);
                names.Add(nameof(Guid));
                indexs.Add(8);
                names.Add(nameof(GuidNull));
                indexs.Add(9);
                names.Add(nameof(Int));
                indexs.Add(10);
                names.Add(nameof(IntNull));
                indexs.Add(11);
                names.Add(nameof(Long));
                indexs.Add(12);
                names.Add(nameof(LongNull));
                indexs.Add(13);
                names.Add(nameof(SByte));
                indexs.Add(14);
                names.Add(nameof(SByteNull));
                indexs.Add(15);
                names.Add(nameof(Short));
                indexs.Add(16);
                names.Add(nameof(ShortNull));
                indexs.Add(17);
                names.Add(nameof(String));
                indexs.Add(18);
                names.Add(nameof(TimeSpan));
                indexs.Add(19);
                names.Add(nameof(TimeSpanNull));
                indexs.Add(20);
                names.Add(nameof(UInt));
                indexs.Add(21);
                names.Add(nameof(UIntNull));
                indexs.Add(22);
                names.Add(nameof(ULong));
                indexs.Add(23);
                names.Add(nameof(ULongNull));
                indexs.Add(24);
                names.Add(nameof(UShort));
                indexs.Add(25);
                names.Add(nameof(UShortNull));
                indexs.Add(26);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(27);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.PropertyData value = default(AutoCSer.TestCase.SerializePerformance.PropertyData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.PropertyData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.PropertyData));
                JsonSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FieldData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.FieldData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
                Bool = AutoCSer.RandomObject.Creator.Create<bool>(__config__, true);
                Byte = AutoCSer.RandomObject.Creator.Create<byte>(__config__, true);
                SByte = AutoCSer.RandomObject.Creator.Create<sbyte>(__config__, true);
                Short = AutoCSer.RandomObject.Creator.Create<short>(__config__, true);
                UShort = AutoCSer.RandomObject.Creator.Create<ushort>(__config__, true);
                Int = AutoCSer.RandomObject.Creator.Create<int>(__config__, true);
                UInt = AutoCSer.RandomObject.Creator.Create<uint>(__config__, true);
                Long = AutoCSer.RandomObject.Creator.Create<long>(__config__, true);
                ULong = AutoCSer.RandomObject.Creator.Create<ulong>(__config__, true);
                DateTime = AutoCSer.RandomObject.Creator.Create<System.DateTime>(__config__, true);
                TimeSpan = AutoCSer.RandomObject.Creator.Create<System.TimeSpan>(__config__, true);
                Guid = AutoCSer.RandomObject.Creator.Create<System.Guid>(__config__, true);
                Char = AutoCSer.RandomObject.Creator.Create<char>(__config__, true);
                String = AutoCSer.RandomObject.Creator.Create<string>(__config__, true);
                BoolNull = AutoCSer.RandomObject.Creator.Create<bool?>(__config__, true);
                ByteNull = AutoCSer.RandomObject.Creator.Create<byte?>(__config__, true);
                SByteNull = AutoCSer.RandomObject.Creator.Create<sbyte?>(__config__, true);
                ShortNull = AutoCSer.RandomObject.Creator.Create<short?>(__config__, true);
                UShortNull = AutoCSer.RandomObject.Creator.Create<ushort?>(__config__, true);
                IntNull = AutoCSer.RandomObject.Creator.Create<int?>(__config__, true);
                UIntNull = AutoCSer.RandomObject.Creator.Create<uint?>(__config__, true);
                LongNull = AutoCSer.RandomObject.Creator.Create<long?>(__config__, true);
                ULongNull = AutoCSer.RandomObject.Creator.Create<ulong?>(__config__, true);
                DateTimeNull = AutoCSer.RandomObject.Creator.Create<System.DateTime?>(__config__, true);
                TimeSpanNull = AutoCSer.RandomObject.Creator.Create<System.TimeSpan?>(__config__, true);
                GuidNull = AutoCSer.RandomObject.Creator.Create<System.Guid?>(__config__, true);
                CharNull = AutoCSer.RandomObject.Creator.Create<char?>(__config__, true);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.FieldData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatFieldData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.FloatFieldData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
                AutoCSer.TestCase.SerializePerformance.FieldData baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<AutoCSer.TestCase.SerializePerformance.FieldData>(ref baseValue, config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
                Float = AutoCSer.RandomObject.Creator.Create<float>(__config__, true);
                Double = AutoCSer.RandomObject.Creator.Create<double>(__config__, true);
                Decimal = AutoCSer.RandomObject.Creator.Create<decimal>(__config__, true);
                FloatNull = AutoCSer.RandomObject.Creator.Create<float?>(__config__, true);
                DoubleNull = AutoCSer.RandomObject.Creator.Create<double?>(__config__, true);
                DecimalNull = AutoCSer.RandomObject.Creator.Create<decimal?>(__config__, true);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.FloatFieldData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatPropertyData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
                AutoCSer.TestCase.SerializePerformance.PropertyData baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<AutoCSer.TestCase.SerializePerformance.PropertyData>(ref baseValue, config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
                Float = AutoCSer.RandomObject.Creator.Create<float>(__config__, true);
                Double = AutoCSer.RandomObject.Creator.Create<double>(__config__, true);
                Decimal = AutoCSer.RandomObject.Creator.Create<decimal>(__config__, true);
                FloatNull = AutoCSer.RandomObject.Creator.Create<float?>(__config__, true);
                DoubleNull = AutoCSer.RandomObject.Creator.Create<double?>(__config__, true);
                DecimalNull = AutoCSer.RandomObject.Creator.Create<decimal?>(__config__, true);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.FloatPropertyData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class JsonFloatFieldData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
                AutoCSer.TestCase.SerializePerformance.FloatFieldData baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<AutoCSer.TestCase.SerializePerformance.FloatFieldData>(ref baseValue, config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class JsonFloatPropertyData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
                AutoCSer.TestCase.SerializePerformance.FloatPropertyData baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<AutoCSer.TestCase.SerializePerformance.FloatPropertyData>(ref baseValue, config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class PropertyData
    {
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void CreateRandomObject(ref AutoCSer.TestCase.SerializePerformance.PropertyData value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
                Bool = AutoCSer.RandomObject.Creator.Create<bool>(__config__, true);
                Byte = AutoCSer.RandomObject.Creator.Create<byte>(__config__, true);
                SByte = AutoCSer.RandomObject.Creator.Create<sbyte>(__config__, true);
                Short = AutoCSer.RandomObject.Creator.Create<short>(__config__, true);
                UShort = AutoCSer.RandomObject.Creator.Create<ushort>(__config__, true);
                Int = AutoCSer.RandomObject.Creator.Create<int>(__config__, true);
                UInt = AutoCSer.RandomObject.Creator.Create<uint>(__config__, true);
                Long = AutoCSer.RandomObject.Creator.Create<long>(__config__, true);
                ULong = AutoCSer.RandomObject.Creator.Create<ulong>(__config__, true);
                DateTime = AutoCSer.RandomObject.Creator.Create<System.DateTime>(__config__, true);
                TimeSpan = AutoCSer.RandomObject.Creator.Create<System.TimeSpan>(__config__, true);
                Guid = AutoCSer.RandomObject.Creator.Create<System.Guid>(__config__, true);
                Char = AutoCSer.RandomObject.Creator.Create<char>(__config__, true);
                String = AutoCSer.RandomObject.Creator.Create<string>(__config__, true);
                BoolNull = AutoCSer.RandomObject.Creator.Create<bool?>(__config__, true);
                ByteNull = AutoCSer.RandomObject.Creator.Create<byte?>(__config__, true);
                SByteNull = AutoCSer.RandomObject.Creator.Create<sbyte?>(__config__, true);
                ShortNull = AutoCSer.RandomObject.Creator.Create<short?>(__config__, true);
                UShortNull = AutoCSer.RandomObject.Creator.Create<ushort?>(__config__, true);
                IntNull = AutoCSer.RandomObject.Creator.Create<int?>(__config__, true);
                UIntNull = AutoCSer.RandomObject.Creator.Create<uint?>(__config__, true);
                LongNull = AutoCSer.RandomObject.Creator.Create<long?>(__config__, true);
                ULongNull = AutoCSer.RandomObject.Creator.Create<ulong?>(__config__, true);
                DateTimeNull = AutoCSer.RandomObject.Creator.Create<System.DateTime?>(__config__, true);
                TimeSpanNull = AutoCSer.RandomObject.Creator.Create<System.TimeSpan?>(__config__, true);
                GuidNull = AutoCSer.RandomObject.Creator.Create<System.Guid?>(__config__, true);
                CharNull = AutoCSer.RandomObject.Creator.Create<char?>(__config__, true);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void CreateRandomObject()
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<AutoCSer.TestCase.SerializePerformance.PropertyData>();
                CreateRandomObject(ref value, null);
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FieldData
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FieldData value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> memberMap, XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FieldData value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FieldData> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (__memberMap__.IsMember(1) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (__memberMap__.IsMember(3) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (__memberMap__.IsMember(5) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                if (__memberMap__.IsMember(6))
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (__memberMap__.IsMember(7) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                if (__memberMap__.IsMember(8))
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (__memberMap__.IsMember(9) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                if (__memberMap__.IsMember(10))
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (__memberMap__.IsMember(11) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                if (__memberMap__.IsMember(12))
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (__memberMap__.IsMember(13) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                if (__memberMap__.IsMember(14))
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (__memberMap__.IsMember(15) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                if (__memberMap__.IsMember(16))
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (__memberMap__.IsMember(17) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (__memberMap__.IsMember(18) && AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                if (__memberMap__.IsMember(19))
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (__memberMap__.IsMember(20) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                if (__memberMap__.IsMember(21))
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (__memberMap__.IsMember(22) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                if (__memberMap__.IsMember(23))
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (__memberMap__.IsMember(24) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                if (__memberMap__.IsMember(25))
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (__memberMap__.IsMember(26) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FieldData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.Bool);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.BoolNull);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.Byte);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.ByteNull);
                        return;
                    case 4:
                        __deserializer__.XmlDeserialize(ref __value__.Char);
                        return;
                    case 5:
                        __deserializer__.XmlDeserialize(ref __value__.CharNull);
                        return;
                    case 6:
                        __deserializer__.XmlDeserialize(ref __value__.DateTime);
                        return;
                    case 7:
                        __deserializer__.XmlDeserialize(ref __value__.DateTimeNull);
                        return;
                    case 8:
                        __deserializer__.XmlDeserialize(ref __value__.Guid);
                        return;
                    case 9:
                        __deserializer__.XmlDeserialize(ref __value__.GuidNull);
                        return;
                    case 10:
                        __deserializer__.XmlDeserialize(ref __value__.Int);
                        return;
                    case 11:
                        __deserializer__.XmlDeserialize(ref __value__.IntNull);
                        return;
                    case 12:
                        __deserializer__.XmlDeserialize(ref __value__.Long);
                        return;
                    case 13:
                        __deserializer__.XmlDeserialize(ref __value__.LongNull);
                        return;
                    case 14:
                        __deserializer__.XmlDeserialize(ref __value__.SByte);
                        return;
                    case 15:
                        __deserializer__.XmlDeserialize(ref __value__.SByteNull);
                        return;
                    case 16:
                        __deserializer__.XmlDeserialize(ref __value__.Short);
                        return;
                    case 17:
                        __deserializer__.XmlDeserialize(ref __value__.ShortNull);
                        return;
                    case 18:
                        __deserializer__.XmlDeserialize(ref __value__.String);
                        return;
                    case 19:
                        __deserializer__.XmlDeserialize(ref __value__.TimeSpan);
                        return;
                    case 20:
                        __deserializer__.XmlDeserialize(ref __value__.TimeSpanNull);
                        return;
                    case 21:
                        __deserializer__.XmlDeserialize(ref __value__.UInt);
                        return;
                    case 22:
                        __deserializer__.XmlDeserialize(ref __value__.UIntNull);
                        return;
                    case 23:
                        __deserializer__.XmlDeserialize(ref __value__.ULong);
                        return;
                    case 24:
                        __deserializer__.XmlDeserialize(ref __value__.ULongNull);
                        return;
                    case 25:
                        __deserializer__.XmlDeserialize(ref __value__.UShort);
                        return;
                    case 26:
                        __deserializer__.XmlDeserialize(ref __value__.UShortNull);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(27);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(27);
                names.Add(nameof(Bool));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(BoolNull));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Byte));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(ByteNull));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(Char));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(CharNull));
                indexs.Add(new KeyValue<int, string>(5, null));
                names.Add(nameof(DateTime));
                indexs.Add(new KeyValue<int, string>(6, null));
                names.Add(nameof(DateTimeNull));
                indexs.Add(new KeyValue<int, string>(7, null));
                names.Add(nameof(Guid));
                indexs.Add(new KeyValue<int, string>(8, null));
                names.Add(nameof(GuidNull));
                indexs.Add(new KeyValue<int, string>(9, null));
                names.Add(nameof(Int));
                indexs.Add(new KeyValue<int, string>(10, null));
                names.Add(nameof(IntNull));
                indexs.Add(new KeyValue<int, string>(11, null));
                names.Add(nameof(Long));
                indexs.Add(new KeyValue<int, string>(12, null));
                names.Add(nameof(LongNull));
                indexs.Add(new KeyValue<int, string>(13, null));
                names.Add(nameof(SByte));
                indexs.Add(new KeyValue<int, string>(14, null));
                names.Add(nameof(SByteNull));
                indexs.Add(new KeyValue<int, string>(15, null));
                names.Add(nameof(Short));
                indexs.Add(new KeyValue<int, string>(16, null));
                names.Add(nameof(ShortNull));
                indexs.Add(new KeyValue<int, string>(17, null));
                names.Add(nameof(String));
                indexs.Add(new KeyValue<int, string>(18, null));
                names.Add(nameof(TimeSpan));
                indexs.Add(new KeyValue<int, string>(19, null));
                names.Add(nameof(TimeSpanNull));
                indexs.Add(new KeyValue<int, string>(20, null));
                names.Add(nameof(UInt));
                indexs.Add(new KeyValue<int, string>(21, null));
                names.Add(nameof(UIntNull));
                indexs.Add(new KeyValue<int, string>(22, null));
                names.Add(nameof(ULong));
                indexs.Add(new KeyValue<int, string>(23, null));
                names.Add(nameof(ULongNull));
                indexs.Add(new KeyValue<int, string>(24, null));
                names.Add(nameof(UShort));
                indexs.Add(new KeyValue<int, string>(25, null));
                names.Add(nameof(UShortNull));
                indexs.Add(new KeyValue<int, string>(26, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(27);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FieldData value = default(AutoCSer.TestCase.SerializePerformance.FieldData);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FieldData));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatFieldData
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatFieldData value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> memberMap, XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatFieldData value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Decimal>");
                    __serializer__.XmlSerialize(Decimal);
                    __stream__.SimpleWrite(@"</Decimal>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DecimalNull))
                {
                    __stream__.SimpleWrite(@"<DecimalNull>");
                    __serializer__.XmlSerialize(DecimalNull);
                    __stream__.SimpleWrite(@"</DecimalNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Double>");
                    __serializer__.XmlSerialize(Double);
                    __stream__.SimpleWrite(@"</Double>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DoubleNull))
                {
                    __stream__.SimpleWrite(@"<DoubleNull>");
                    __serializer__.XmlSerialize(DoubleNull);
                    __stream__.SimpleWrite(@"</DoubleNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Float>");
                    __serializer__.XmlSerialize(Float);
                    __stream__.SimpleWrite(@"</Float>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, FloatNull))
                {
                    __stream__.SimpleWrite(@"<FloatNull>");
                    __serializer__.XmlSerialize(FloatNull);
                    __stream__.SimpleWrite(@"</FloatNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatFieldData> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (__memberMap__.IsMember(1) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (__memberMap__.IsMember(3) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (__memberMap__.IsMember(5) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                if (__memberMap__.IsMember(6))
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (__memberMap__.IsMember(7) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                if (__memberMap__.IsMember(8))
                {
                    __stream__.SimpleWrite(@"<Decimal>");
                    __serializer__.XmlSerialize(Decimal);
                    __stream__.SimpleWrite(@"</Decimal>");
                }
                if (__memberMap__.IsMember(9) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DecimalNull))
                {
                    __stream__.SimpleWrite(@"<DecimalNull>");
                    __serializer__.XmlSerialize(DecimalNull);
                    __stream__.SimpleWrite(@"</DecimalNull>");
                }
                if (__memberMap__.IsMember(10))
                {
                    __stream__.SimpleWrite(@"<Double>");
                    __serializer__.XmlSerialize(Double);
                    __stream__.SimpleWrite(@"</Double>");
                }
                if (__memberMap__.IsMember(11) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DoubleNull))
                {
                    __stream__.SimpleWrite(@"<DoubleNull>");
                    __serializer__.XmlSerialize(DoubleNull);
                    __stream__.SimpleWrite(@"</DoubleNull>");
                }
                if (__memberMap__.IsMember(12))
                {
                    __stream__.SimpleWrite(@"<Float>");
                    __serializer__.XmlSerialize(Float);
                    __stream__.SimpleWrite(@"</Float>");
                }
                if (__memberMap__.IsMember(13) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, FloatNull))
                {
                    __stream__.SimpleWrite(@"<FloatNull>");
                    __serializer__.XmlSerialize(FloatNull);
                    __stream__.SimpleWrite(@"</FloatNull>");
                }
                if (__memberMap__.IsMember(14))
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (__memberMap__.IsMember(15) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                if (__memberMap__.IsMember(16))
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (__memberMap__.IsMember(17) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                if (__memberMap__.IsMember(18))
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (__memberMap__.IsMember(19) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                if (__memberMap__.IsMember(20))
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (__memberMap__.IsMember(21) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                if (__memberMap__.IsMember(22))
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (__memberMap__.IsMember(23) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (__memberMap__.IsMember(24) && AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                if (__memberMap__.IsMember(25))
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (__memberMap__.IsMember(26) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                if (__memberMap__.IsMember(27))
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (__memberMap__.IsMember(28) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                if (__memberMap__.IsMember(29))
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (__memberMap__.IsMember(30) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                if (__memberMap__.IsMember(31))
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (__memberMap__.IsMember(32) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FloatFieldData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.Bool);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.BoolNull);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.Byte);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.ByteNull);
                        return;
                    case 4:
                        __deserializer__.XmlDeserialize(ref __value__.Char);
                        return;
                    case 5:
                        __deserializer__.XmlDeserialize(ref __value__.CharNull);
                        return;
                    case 6:
                        __deserializer__.XmlDeserialize(ref __value__.DateTime);
                        return;
                    case 7:
                        __deserializer__.XmlDeserialize(ref __value__.DateTimeNull);
                        return;
                    case 8:
                        __deserializer__.XmlDeserialize(ref __value__.Decimal);
                        return;
                    case 9:
                        __deserializer__.XmlDeserialize(ref __value__.DecimalNull);
                        return;
                    case 10:
                        __deserializer__.XmlDeserialize(ref __value__.Double);
                        return;
                    case 11:
                        __deserializer__.XmlDeserialize(ref __value__.DoubleNull);
                        return;
                    case 12:
                        __deserializer__.XmlDeserialize(ref __value__.Float);
                        return;
                    case 13:
                        __deserializer__.XmlDeserialize(ref __value__.FloatNull);
                        return;
                    case 14:
                        __deserializer__.XmlDeserialize(ref __value__.Guid);
                        return;
                    case 15:
                        __deserializer__.XmlDeserialize(ref __value__.GuidNull);
                        return;
                    case 16:
                        __deserializer__.XmlDeserialize(ref __value__.Int);
                        return;
                    case 17:
                        __deserializer__.XmlDeserialize(ref __value__.IntNull);
                        return;
                    case 18:
                        __deserializer__.XmlDeserialize(ref __value__.Long);
                        return;
                    case 19:
                        __deserializer__.XmlDeserialize(ref __value__.LongNull);
                        return;
                    case 20:
                        __deserializer__.XmlDeserialize(ref __value__.SByte);
                        return;
                    case 21:
                        __deserializer__.XmlDeserialize(ref __value__.SByteNull);
                        return;
                    case 22:
                        __deserializer__.XmlDeserialize(ref __value__.Short);
                        return;
                    case 23:
                        __deserializer__.XmlDeserialize(ref __value__.ShortNull);
                        return;
                    case 24:
                        __deserializer__.XmlDeserialize(ref __value__.String);
                        return;
                    case 25:
                        __deserializer__.XmlDeserialize(ref __value__.TimeSpan);
                        return;
                    case 26:
                        __deserializer__.XmlDeserialize(ref __value__.TimeSpanNull);
                        return;
                    case 27:
                        __deserializer__.XmlDeserialize(ref __value__.UInt);
                        return;
                    case 28:
                        __deserializer__.XmlDeserialize(ref __value__.UIntNull);
                        return;
                    case 29:
                        __deserializer__.XmlDeserialize(ref __value__.ULong);
                        return;
                    case 30:
                        __deserializer__.XmlDeserialize(ref __value__.ULongNull);
                        return;
                    case 31:
                        __deserializer__.XmlDeserialize(ref __value__.UShort);
                        return;
                    case 32:
                        __deserializer__.XmlDeserialize(ref __value__.UShortNull);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(33);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(33);
                names.Add(nameof(Bool));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(BoolNull));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Byte));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(ByteNull));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(Char));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(CharNull));
                indexs.Add(new KeyValue<int, string>(5, null));
                names.Add(nameof(DateTime));
                indexs.Add(new KeyValue<int, string>(6, null));
                names.Add(nameof(DateTimeNull));
                indexs.Add(new KeyValue<int, string>(7, null));
                names.Add(nameof(Decimal));
                indexs.Add(new KeyValue<int, string>(8, null));
                names.Add(nameof(DecimalNull));
                indexs.Add(new KeyValue<int, string>(9, null));
                names.Add(nameof(Double));
                indexs.Add(new KeyValue<int, string>(10, null));
                names.Add(nameof(DoubleNull));
                indexs.Add(new KeyValue<int, string>(11, null));
                names.Add(nameof(Float));
                indexs.Add(new KeyValue<int, string>(12, null));
                names.Add(nameof(FloatNull));
                indexs.Add(new KeyValue<int, string>(13, null));
                names.Add(nameof(Guid));
                indexs.Add(new KeyValue<int, string>(14, null));
                names.Add(nameof(GuidNull));
                indexs.Add(new KeyValue<int, string>(15, null));
                names.Add(nameof(Int));
                indexs.Add(new KeyValue<int, string>(16, null));
                names.Add(nameof(IntNull));
                indexs.Add(new KeyValue<int, string>(17, null));
                names.Add(nameof(Long));
                indexs.Add(new KeyValue<int, string>(18, null));
                names.Add(nameof(LongNull));
                indexs.Add(new KeyValue<int, string>(19, null));
                names.Add(nameof(SByte));
                indexs.Add(new KeyValue<int, string>(20, null));
                names.Add(nameof(SByteNull));
                indexs.Add(new KeyValue<int, string>(21, null));
                names.Add(nameof(Short));
                indexs.Add(new KeyValue<int, string>(22, null));
                names.Add(nameof(ShortNull));
                indexs.Add(new KeyValue<int, string>(23, null));
                names.Add(nameof(String));
                indexs.Add(new KeyValue<int, string>(24, null));
                names.Add(nameof(TimeSpan));
                indexs.Add(new KeyValue<int, string>(25, null));
                names.Add(nameof(TimeSpanNull));
                indexs.Add(new KeyValue<int, string>(26, null));
                names.Add(nameof(UInt));
                indexs.Add(new KeyValue<int, string>(27, null));
                names.Add(nameof(UIntNull));
                indexs.Add(new KeyValue<int, string>(28, null));
                names.Add(nameof(ULong));
                indexs.Add(new KeyValue<int, string>(29, null));
                names.Add(nameof(ULongNull));
                indexs.Add(new KeyValue<int, string>(30, null));
                names.Add(nameof(UShort));
                indexs.Add(new KeyValue<int, string>(31, null));
                names.Add(nameof(UShortNull));
                indexs.Add(new KeyValue<int, string>(32, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(33);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(decimal));
                types.Add(typeof(decimal?));
                types.Add(typeof(double));
                types.Add(typeof(double?));
                types.Add(typeof(float));
                types.Add(typeof(float?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatFieldData value = default(AutoCSer.TestCase.SerializePerformance.FloatFieldData);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatFieldData));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class FloatPropertyData
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> memberMap, XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.FloatPropertyData value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Decimal>");
                    __serializer__.XmlSerialize(Decimal);
                    __stream__.SimpleWrite(@"</Decimal>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DecimalNull))
                {
                    __stream__.SimpleWrite(@"<DecimalNull>");
                    __serializer__.XmlSerialize(DecimalNull);
                    __stream__.SimpleWrite(@"</DecimalNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Double>");
                    __serializer__.XmlSerialize(Double);
                    __stream__.SimpleWrite(@"</Double>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DoubleNull))
                {
                    __stream__.SimpleWrite(@"<DoubleNull>");
                    __serializer__.XmlSerialize(DoubleNull);
                    __stream__.SimpleWrite(@"</DoubleNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Float>");
                    __serializer__.XmlSerialize(Float);
                    __stream__.SimpleWrite(@"</Float>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, FloatNull))
                {
                    __stream__.SimpleWrite(@"<FloatNull>");
                    __serializer__.XmlSerialize(FloatNull);
                    __stream__.SimpleWrite(@"</FloatNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.FloatPropertyData> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (__memberMap__.IsMember(1) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (__memberMap__.IsMember(3) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (__memberMap__.IsMember(5) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                if (__memberMap__.IsMember(6))
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (__memberMap__.IsMember(7) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                if (__memberMap__.IsMember(8))
                {
                    __stream__.SimpleWrite(@"<Decimal>");
                    __serializer__.XmlSerialize(Decimal);
                    __stream__.SimpleWrite(@"</Decimal>");
                }
                if (__memberMap__.IsMember(9) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DecimalNull))
                {
                    __stream__.SimpleWrite(@"<DecimalNull>");
                    __serializer__.XmlSerialize(DecimalNull);
                    __stream__.SimpleWrite(@"</DecimalNull>");
                }
                if (__memberMap__.IsMember(10))
                {
                    __stream__.SimpleWrite(@"<Double>");
                    __serializer__.XmlSerialize(Double);
                    __stream__.SimpleWrite(@"</Double>");
                }
                if (__memberMap__.IsMember(11) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DoubleNull))
                {
                    __stream__.SimpleWrite(@"<DoubleNull>");
                    __serializer__.XmlSerialize(DoubleNull);
                    __stream__.SimpleWrite(@"</DoubleNull>");
                }
                if (__memberMap__.IsMember(12))
                {
                    __stream__.SimpleWrite(@"<Float>");
                    __serializer__.XmlSerialize(Float);
                    __stream__.SimpleWrite(@"</Float>");
                }
                if (__memberMap__.IsMember(13) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, FloatNull))
                {
                    __stream__.SimpleWrite(@"<FloatNull>");
                    __serializer__.XmlSerialize(FloatNull);
                    __stream__.SimpleWrite(@"</FloatNull>");
                }
                if (__memberMap__.IsMember(14))
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (__memberMap__.IsMember(15) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                if (__memberMap__.IsMember(16))
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (__memberMap__.IsMember(17) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                if (__memberMap__.IsMember(18))
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (__memberMap__.IsMember(19) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                if (__memberMap__.IsMember(20))
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (__memberMap__.IsMember(21) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                if (__memberMap__.IsMember(22))
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (__memberMap__.IsMember(23) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (__memberMap__.IsMember(24) && AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                if (__memberMap__.IsMember(25))
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (__memberMap__.IsMember(26) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                if (__memberMap__.IsMember(27))
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (__memberMap__.IsMember(28) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                if (__memberMap__.IsMember(29))
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (__memberMap__.IsMember(30) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                if (__memberMap__.IsMember(31))
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (__memberMap__.IsMember(32) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.FloatPropertyData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        var Bool = __value__.Bool;
                        __deserializer__.XmlDeserialize(ref Bool);
                        __value__.Bool = Bool;
                        return;
                    case 1:
                        var BoolNull = __value__.BoolNull;
                        __deserializer__.XmlDeserialize(ref BoolNull);
                        __value__.BoolNull = BoolNull;
                        return;
                    case 2:
                        var Byte = __value__.Byte;
                        __deserializer__.XmlDeserialize(ref Byte);
                        __value__.Byte = Byte;
                        return;
                    case 3:
                        var ByteNull = __value__.ByteNull;
                        __deserializer__.XmlDeserialize(ref ByteNull);
                        __value__.ByteNull = ByteNull;
                        return;
                    case 4:
                        var Char = __value__.Char;
                        __deserializer__.XmlDeserialize(ref Char);
                        __value__.Char = Char;
                        return;
                    case 5:
                        var CharNull = __value__.CharNull;
                        __deserializer__.XmlDeserialize(ref CharNull);
                        __value__.CharNull = CharNull;
                        return;
                    case 6:
                        var DateTime = __value__.DateTime;
                        __deserializer__.XmlDeserialize(ref DateTime);
                        __value__.DateTime = DateTime;
                        return;
                    case 7:
                        var DateTimeNull = __value__.DateTimeNull;
                        __deserializer__.XmlDeserialize(ref DateTimeNull);
                        __value__.DateTimeNull = DateTimeNull;
                        return;
                    case 8:
                        var Decimal = __value__.Decimal;
                        __deserializer__.XmlDeserialize(ref Decimal);
                        __value__.Decimal = Decimal;
                        return;
                    case 9:
                        var DecimalNull = __value__.DecimalNull;
                        __deserializer__.XmlDeserialize(ref DecimalNull);
                        __value__.DecimalNull = DecimalNull;
                        return;
                    case 10:
                        var Double = __value__.Double;
                        __deserializer__.XmlDeserialize(ref Double);
                        __value__.Double = Double;
                        return;
                    case 11:
                        var DoubleNull = __value__.DoubleNull;
                        __deserializer__.XmlDeserialize(ref DoubleNull);
                        __value__.DoubleNull = DoubleNull;
                        return;
                    case 12:
                        var Float = __value__.Float;
                        __deserializer__.XmlDeserialize(ref Float);
                        __value__.Float = Float;
                        return;
                    case 13:
                        var FloatNull = __value__.FloatNull;
                        __deserializer__.XmlDeserialize(ref FloatNull);
                        __value__.FloatNull = FloatNull;
                        return;
                    case 14:
                        var Guid = __value__.Guid;
                        __deserializer__.XmlDeserialize(ref Guid);
                        __value__.Guid = Guid;
                        return;
                    case 15:
                        var GuidNull = __value__.GuidNull;
                        __deserializer__.XmlDeserialize(ref GuidNull);
                        __value__.GuidNull = GuidNull;
                        return;
                    case 16:
                        var Int = __value__.Int;
                        __deserializer__.XmlDeserialize(ref Int);
                        __value__.Int = Int;
                        return;
                    case 17:
                        var IntNull = __value__.IntNull;
                        __deserializer__.XmlDeserialize(ref IntNull);
                        __value__.IntNull = IntNull;
                        return;
                    case 18:
                        var Long = __value__.Long;
                        __deserializer__.XmlDeserialize(ref Long);
                        __value__.Long = Long;
                        return;
                    case 19:
                        var LongNull = __value__.LongNull;
                        __deserializer__.XmlDeserialize(ref LongNull);
                        __value__.LongNull = LongNull;
                        return;
                    case 20:
                        var SByte = __value__.SByte;
                        __deserializer__.XmlDeserialize(ref SByte);
                        __value__.SByte = SByte;
                        return;
                    case 21:
                        var SByteNull = __value__.SByteNull;
                        __deserializer__.XmlDeserialize(ref SByteNull);
                        __value__.SByteNull = SByteNull;
                        return;
                    case 22:
                        var Short = __value__.Short;
                        __deserializer__.XmlDeserialize(ref Short);
                        __value__.Short = Short;
                        return;
                    case 23:
                        var ShortNull = __value__.ShortNull;
                        __deserializer__.XmlDeserialize(ref ShortNull);
                        __value__.ShortNull = ShortNull;
                        return;
                    case 24:
                        var String = __value__.String;
                        __deserializer__.XmlDeserialize(ref String);
                        __value__.String = String;
                        return;
                    case 25:
                        var TimeSpan = __value__.TimeSpan;
                        __deserializer__.XmlDeserialize(ref TimeSpan);
                        __value__.TimeSpan = TimeSpan;
                        return;
                    case 26:
                        var TimeSpanNull = __value__.TimeSpanNull;
                        __deserializer__.XmlDeserialize(ref TimeSpanNull);
                        __value__.TimeSpanNull = TimeSpanNull;
                        return;
                    case 27:
                        var UInt = __value__.UInt;
                        __deserializer__.XmlDeserialize(ref UInt);
                        __value__.UInt = UInt;
                        return;
                    case 28:
                        var UIntNull = __value__.UIntNull;
                        __deserializer__.XmlDeserialize(ref UIntNull);
                        __value__.UIntNull = UIntNull;
                        return;
                    case 29:
                        var ULong = __value__.ULong;
                        __deserializer__.XmlDeserialize(ref ULong);
                        __value__.ULong = ULong;
                        return;
                    case 30:
                        var ULongNull = __value__.ULongNull;
                        __deserializer__.XmlDeserialize(ref ULongNull);
                        __value__.ULongNull = ULongNull;
                        return;
                    case 31:
                        var UShort = __value__.UShort;
                        __deserializer__.XmlDeserialize(ref UShort);
                        __value__.UShort = UShort;
                        return;
                    case 32:
                        var UShortNull = __value__.UShortNull;
                        __deserializer__.XmlDeserialize(ref UShortNull);
                        __value__.UShortNull = UShortNull;
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(33);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(33);
                names.Add(nameof(Bool));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(BoolNull));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Byte));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(ByteNull));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(Char));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(CharNull));
                indexs.Add(new KeyValue<int, string>(5, null));
                names.Add(nameof(DateTime));
                indexs.Add(new KeyValue<int, string>(6, null));
                names.Add(nameof(DateTimeNull));
                indexs.Add(new KeyValue<int, string>(7, null));
                names.Add(nameof(Decimal));
                indexs.Add(new KeyValue<int, string>(8, null));
                names.Add(nameof(DecimalNull));
                indexs.Add(new KeyValue<int, string>(9, null));
                names.Add(nameof(Double));
                indexs.Add(new KeyValue<int, string>(10, null));
                names.Add(nameof(DoubleNull));
                indexs.Add(new KeyValue<int, string>(11, null));
                names.Add(nameof(Float));
                indexs.Add(new KeyValue<int, string>(12, null));
                names.Add(nameof(FloatNull));
                indexs.Add(new KeyValue<int, string>(13, null));
                names.Add(nameof(Guid));
                indexs.Add(new KeyValue<int, string>(14, null));
                names.Add(nameof(GuidNull));
                indexs.Add(new KeyValue<int, string>(15, null));
                names.Add(nameof(Int));
                indexs.Add(new KeyValue<int, string>(16, null));
                names.Add(nameof(IntNull));
                indexs.Add(new KeyValue<int, string>(17, null));
                names.Add(nameof(Long));
                indexs.Add(new KeyValue<int, string>(18, null));
                names.Add(nameof(LongNull));
                indexs.Add(new KeyValue<int, string>(19, null));
                names.Add(nameof(SByte));
                indexs.Add(new KeyValue<int, string>(20, null));
                names.Add(nameof(SByteNull));
                indexs.Add(new KeyValue<int, string>(21, null));
                names.Add(nameof(Short));
                indexs.Add(new KeyValue<int, string>(22, null));
                names.Add(nameof(ShortNull));
                indexs.Add(new KeyValue<int, string>(23, null));
                names.Add(nameof(String));
                indexs.Add(new KeyValue<int, string>(24, null));
                names.Add(nameof(TimeSpan));
                indexs.Add(new KeyValue<int, string>(25, null));
                names.Add(nameof(TimeSpanNull));
                indexs.Add(new KeyValue<int, string>(26, null));
                names.Add(nameof(UInt));
                indexs.Add(new KeyValue<int, string>(27, null));
                names.Add(nameof(UIntNull));
                indexs.Add(new KeyValue<int, string>(28, null));
                names.Add(nameof(ULong));
                indexs.Add(new KeyValue<int, string>(29, null));
                names.Add(nameof(ULongNull));
                indexs.Add(new KeyValue<int, string>(30, null));
                names.Add(nameof(UShort));
                indexs.Add(new KeyValue<int, string>(31, null));
                names.Add(nameof(UShortNull));
                indexs.Add(new KeyValue<int, string>(32, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(33);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(decimal));
                types.Add(typeof(decimal?));
                types.Add(typeof(double));
                types.Add(typeof(double?));
                types.Add(typeof(float));
                types.Add(typeof(float?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.FloatPropertyData value = default(AutoCSer.TestCase.SerializePerformance.FloatPropertyData);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.FloatPropertyData));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    internal partial class PropertyData
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.PropertyData value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> memberMap, XmlSerializer serializer, AutoCSer.TestCase.SerializePerformance.PropertyData value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SerializePerformance.PropertyData> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<Bool>");
                    __serializer__.XmlSerialize(Bool);
                    __stream__.SimpleWrite(@"</Bool>");
                }
                if (__memberMap__.IsMember(1) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, BoolNull))
                {
                    __stream__.SimpleWrite(@"<BoolNull>");
                    __serializer__.XmlSerialize(BoolNull);
                    __stream__.SimpleWrite(@"</BoolNull>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Byte>");
                    __serializer__.XmlSerialize(Byte);
                    __stream__.SimpleWrite(@"</Byte>");
                }
                if (__memberMap__.IsMember(3) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ByteNull))
                {
                    __stream__.SimpleWrite(@"<ByteNull>");
                    __serializer__.XmlSerialize(ByteNull);
                    __stream__.SimpleWrite(@"</ByteNull>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<Char>");
                    __serializer__.XmlSerialize(Char);
                    __stream__.SimpleWrite(@"</Char>");
                }
                if (__memberMap__.IsMember(5) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, CharNull))
                {
                    __stream__.SimpleWrite(@"<CharNull>");
                    __serializer__.XmlSerialize(CharNull);
                    __stream__.SimpleWrite(@"</CharNull>");
                }
                if (__memberMap__.IsMember(6))
                {
                    __stream__.SimpleWrite(@"<DateTime>");
                    __serializer__.XmlSerialize(DateTime);
                    __stream__.SimpleWrite(@"</DateTime>");
                }
                if (__memberMap__.IsMember(7) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, DateTimeNull))
                {
                    __stream__.SimpleWrite(@"<DateTimeNull>");
                    __serializer__.XmlSerialize(DateTimeNull);
                    __stream__.SimpleWrite(@"</DateTimeNull>");
                }
                if (__memberMap__.IsMember(8))
                {
                    __stream__.SimpleWrite(@"<Guid>");
                    __serializer__.XmlSerialize(Guid);
                    __stream__.SimpleWrite(@"</Guid>");
                }
                if (__memberMap__.IsMember(9) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, GuidNull))
                {
                    __stream__.SimpleWrite(@"<GuidNull>");
                    __serializer__.XmlSerialize(GuidNull);
                    __stream__.SimpleWrite(@"</GuidNull>");
                }
                if (__memberMap__.IsMember(10))
                {
                    __stream__.SimpleWrite(@"<Int>");
                    __serializer__.XmlSerialize(Int);
                    __stream__.SimpleWrite(@"</Int>");
                }
                if (__memberMap__.IsMember(11) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, IntNull))
                {
                    __stream__.SimpleWrite(@"<IntNull>");
                    __serializer__.XmlSerialize(IntNull);
                    __stream__.SimpleWrite(@"</IntNull>");
                }
                if (__memberMap__.IsMember(12))
                {
                    __stream__.SimpleWrite(@"<Long>");
                    __serializer__.XmlSerialize(Long);
                    __stream__.SimpleWrite(@"</Long>");
                }
                if (__memberMap__.IsMember(13) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, LongNull))
                {
                    __stream__.SimpleWrite(@"<LongNull>");
                    __serializer__.XmlSerialize(LongNull);
                    __stream__.SimpleWrite(@"</LongNull>");
                }
                if (__memberMap__.IsMember(14))
                {
                    __stream__.SimpleWrite(@"<SByte>");
                    __serializer__.XmlSerialize(SByte);
                    __stream__.SimpleWrite(@"</SByte>");
                }
                if (__memberMap__.IsMember(15) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, SByteNull))
                {
                    __stream__.SimpleWrite(@"<SByteNull>");
                    __serializer__.XmlSerialize(SByteNull);
                    __stream__.SimpleWrite(@"</SByteNull>");
                }
                if (__memberMap__.IsMember(16))
                {
                    __stream__.SimpleWrite(@"<Short>");
                    __serializer__.XmlSerialize(Short);
                    __stream__.SimpleWrite(@"</Short>");
                }
                if (__memberMap__.IsMember(17) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ShortNull))
                {
                    __stream__.SimpleWrite(@"<ShortNull>");
                    __serializer__.XmlSerialize(ShortNull);
                    __stream__.SimpleWrite(@"</ShortNull>");
                }
                if (__memberMap__.IsMember(18) && AutoCSer.XmlSerializer.IsOutputString(__serializer__, String))
                {
                    __stream__.SimpleWrite(@"<String>");
                    if (String != null) __serializer__.XmlSerialize(String);
                    __stream__.SimpleWrite(@"</String>");
                }
                if (__memberMap__.IsMember(19))
                {
                    __stream__.SimpleWrite(@"<TimeSpan>");
                    __serializer__.XmlSerialize(TimeSpan);
                    __stream__.SimpleWrite(@"</TimeSpan>");
                }
                if (__memberMap__.IsMember(20) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, TimeSpanNull))
                {
                    __stream__.SimpleWrite(@"<TimeSpanNull>");
                    __serializer__.XmlSerialize(TimeSpanNull);
                    __stream__.SimpleWrite(@"</TimeSpanNull>");
                }
                if (__memberMap__.IsMember(21))
                {
                    __stream__.SimpleWrite(@"<UInt>");
                    __serializer__.XmlSerialize(UInt);
                    __stream__.SimpleWrite(@"</UInt>");
                }
                if (__memberMap__.IsMember(22) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UIntNull))
                {
                    __stream__.SimpleWrite(@"<UIntNull>");
                    __serializer__.XmlSerialize(UIntNull);
                    __stream__.SimpleWrite(@"</UIntNull>");
                }
                if (__memberMap__.IsMember(23))
                {
                    __stream__.SimpleWrite(@"<ULong>");
                    __serializer__.XmlSerialize(ULong);
                    __stream__.SimpleWrite(@"</ULong>");
                }
                if (__memberMap__.IsMember(24) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, ULongNull))
                {
                    __stream__.SimpleWrite(@"<ULongNull>");
                    __serializer__.XmlSerialize(ULongNull);
                    __stream__.SimpleWrite(@"</ULongNull>");
                }
                if (__memberMap__.IsMember(25))
                {
                    __stream__.SimpleWrite(@"<UShort>");
                    __serializer__.XmlSerialize(UShort);
                    __stream__.SimpleWrite(@"</UShort>");
                }
                if (__memberMap__.IsMember(26) && AutoCSer.XmlSerializer.IsOutputNullable(__serializer__, UShortNull))
                {
                    __stream__.SimpleWrite(@"<UShortNull>");
                    __serializer__.XmlSerialize(UShortNull);
                    __stream__.SimpleWrite(@"</UShortNull>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.TestCase.SerializePerformance.PropertyData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        var Bool = __value__.Bool;
                        __deserializer__.XmlDeserialize(ref Bool);
                        __value__.Bool = Bool;
                        return;
                    case 1:
                        var BoolNull = __value__.BoolNull;
                        __deserializer__.XmlDeserialize(ref BoolNull);
                        __value__.BoolNull = BoolNull;
                        return;
                    case 2:
                        var Byte = __value__.Byte;
                        __deserializer__.XmlDeserialize(ref Byte);
                        __value__.Byte = Byte;
                        return;
                    case 3:
                        var ByteNull = __value__.ByteNull;
                        __deserializer__.XmlDeserialize(ref ByteNull);
                        __value__.ByteNull = ByteNull;
                        return;
                    case 4:
                        var Char = __value__.Char;
                        __deserializer__.XmlDeserialize(ref Char);
                        __value__.Char = Char;
                        return;
                    case 5:
                        var CharNull = __value__.CharNull;
                        __deserializer__.XmlDeserialize(ref CharNull);
                        __value__.CharNull = CharNull;
                        return;
                    case 6:
                        var DateTime = __value__.DateTime;
                        __deserializer__.XmlDeserialize(ref DateTime);
                        __value__.DateTime = DateTime;
                        return;
                    case 7:
                        var DateTimeNull = __value__.DateTimeNull;
                        __deserializer__.XmlDeserialize(ref DateTimeNull);
                        __value__.DateTimeNull = DateTimeNull;
                        return;
                    case 8:
                        var Guid = __value__.Guid;
                        __deserializer__.XmlDeserialize(ref Guid);
                        __value__.Guid = Guid;
                        return;
                    case 9:
                        var GuidNull = __value__.GuidNull;
                        __deserializer__.XmlDeserialize(ref GuidNull);
                        __value__.GuidNull = GuidNull;
                        return;
                    case 10:
                        var Int = __value__.Int;
                        __deserializer__.XmlDeserialize(ref Int);
                        __value__.Int = Int;
                        return;
                    case 11:
                        var IntNull = __value__.IntNull;
                        __deserializer__.XmlDeserialize(ref IntNull);
                        __value__.IntNull = IntNull;
                        return;
                    case 12:
                        var Long = __value__.Long;
                        __deserializer__.XmlDeserialize(ref Long);
                        __value__.Long = Long;
                        return;
                    case 13:
                        var LongNull = __value__.LongNull;
                        __deserializer__.XmlDeserialize(ref LongNull);
                        __value__.LongNull = LongNull;
                        return;
                    case 14:
                        var SByte = __value__.SByte;
                        __deserializer__.XmlDeserialize(ref SByte);
                        __value__.SByte = SByte;
                        return;
                    case 15:
                        var SByteNull = __value__.SByteNull;
                        __deserializer__.XmlDeserialize(ref SByteNull);
                        __value__.SByteNull = SByteNull;
                        return;
                    case 16:
                        var Short = __value__.Short;
                        __deserializer__.XmlDeserialize(ref Short);
                        __value__.Short = Short;
                        return;
                    case 17:
                        var ShortNull = __value__.ShortNull;
                        __deserializer__.XmlDeserialize(ref ShortNull);
                        __value__.ShortNull = ShortNull;
                        return;
                    case 18:
                        var String = __value__.String;
                        __deserializer__.XmlDeserialize(ref String);
                        __value__.String = String;
                        return;
                    case 19:
                        var TimeSpan = __value__.TimeSpan;
                        __deserializer__.XmlDeserialize(ref TimeSpan);
                        __value__.TimeSpan = TimeSpan;
                        return;
                    case 20:
                        var TimeSpanNull = __value__.TimeSpanNull;
                        __deserializer__.XmlDeserialize(ref TimeSpanNull);
                        __value__.TimeSpanNull = TimeSpanNull;
                        return;
                    case 21:
                        var UInt = __value__.UInt;
                        __deserializer__.XmlDeserialize(ref UInt);
                        __value__.UInt = UInt;
                        return;
                    case 22:
                        var UIntNull = __value__.UIntNull;
                        __deserializer__.XmlDeserialize(ref UIntNull);
                        __value__.UIntNull = UIntNull;
                        return;
                    case 23:
                        var ULong = __value__.ULong;
                        __deserializer__.XmlDeserialize(ref ULong);
                        __value__.ULong = ULong;
                        return;
                    case 24:
                        var ULongNull = __value__.ULongNull;
                        __deserializer__.XmlDeserialize(ref ULongNull);
                        __value__.ULongNull = ULongNull;
                        return;
                    case 25:
                        var UShort = __value__.UShort;
                        __deserializer__.XmlDeserialize(ref UShort);
                        __value__.UShort = UShort;
                        return;
                    case 26:
                        var UShortNull = __value__.UShortNull;
                        __deserializer__.XmlDeserialize(ref UShortNull);
                        __value__.UShortNull = UShortNull;
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(27);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(27);
                names.Add(nameof(Bool));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(BoolNull));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Byte));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(ByteNull));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(Char));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(CharNull));
                indexs.Add(new KeyValue<int, string>(5, null));
                names.Add(nameof(DateTime));
                indexs.Add(new KeyValue<int, string>(6, null));
                names.Add(nameof(DateTimeNull));
                indexs.Add(new KeyValue<int, string>(7, null));
                names.Add(nameof(Guid));
                indexs.Add(new KeyValue<int, string>(8, null));
                names.Add(nameof(GuidNull));
                indexs.Add(new KeyValue<int, string>(9, null));
                names.Add(nameof(Int));
                indexs.Add(new KeyValue<int, string>(10, null));
                names.Add(nameof(IntNull));
                indexs.Add(new KeyValue<int, string>(11, null));
                names.Add(nameof(Long));
                indexs.Add(new KeyValue<int, string>(12, null));
                names.Add(nameof(LongNull));
                indexs.Add(new KeyValue<int, string>(13, null));
                names.Add(nameof(SByte));
                indexs.Add(new KeyValue<int, string>(14, null));
                names.Add(nameof(SByteNull));
                indexs.Add(new KeyValue<int, string>(15, null));
                names.Add(nameof(Short));
                indexs.Add(new KeyValue<int, string>(16, null));
                names.Add(nameof(ShortNull));
                indexs.Add(new KeyValue<int, string>(17, null));
                names.Add(nameof(String));
                indexs.Add(new KeyValue<int, string>(18, null));
                names.Add(nameof(TimeSpan));
                indexs.Add(new KeyValue<int, string>(19, null));
                names.Add(nameof(TimeSpanNull));
                indexs.Add(new KeyValue<int, string>(20, null));
                names.Add(nameof(UInt));
                indexs.Add(new KeyValue<int, string>(21, null));
                names.Add(nameof(UIntNull));
                indexs.Add(new KeyValue<int, string>(22, null));
                names.Add(nameof(ULong));
                indexs.Add(new KeyValue<int, string>(23, null));
                names.Add(nameof(ULongNull));
                indexs.Add(new KeyValue<int, string>(24, null));
                names.Add(nameof(UShort));
                indexs.Add(new KeyValue<int, string>(25, null));
                names.Add(nameof(UShortNull));
                indexs.Add(new KeyValue<int, string>(26, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(27);
                types.Add(typeof(bool));
                types.Add(typeof(bool?));
                types.Add(typeof(byte));
                types.Add(typeof(byte?));
                types.Add(typeof(char));
                types.Add(typeof(char?));
                types.Add(typeof(System.DateTime));
                types.Add(typeof(System.DateTime?));
                types.Add(typeof(System.Guid));
                types.Add(typeof(System.Guid?));
                types.Add(typeof(int));
                types.Add(typeof(int?));
                types.Add(typeof(long));
                types.Add(typeof(long?));
                types.Add(typeof(sbyte));
                types.Add(typeof(sbyte?));
                types.Add(typeof(short));
                types.Add(typeof(short?));
                types.Add(typeof(string));
                types.Add(typeof(System.TimeSpan));
                types.Add(typeof(System.TimeSpan?));
                types.Add(typeof(uint));
                types.Add(typeof(uint?));
                types.Add(typeof(ulong));
                types.Add(typeof(ulong?));
                types.Add(typeof(ushort));
                types.Add(typeof(ushort?));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.TestCase.SerializePerformance.PropertyData value = default(AutoCSer.TestCase.SerializePerformance.PropertyData);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.PropertyData));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.SerializePerformance.PropertyData));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 触发 AOT 编译
    /// </summary>
    public static class AotMethod
    {
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    AutoCSer.TestCase.SerializePerformance.FieldData/**/.BinarySerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatFieldData/**/.BinarySerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatPropertyData/**/.BinarySerialize();
                    AutoCSer.TestCase.SerializePerformance.FieldData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.FloatFieldData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.FloatPropertyData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.PropertyData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.SerializePerformance.FieldData/**/.JsonSerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatFieldData/**/.JsonSerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatPropertyData/**/.JsonSerialize();
                    AutoCSer.TestCase.SerializePerformance.PropertyData/**/.JsonSerialize();
                    AutoCSer.TestCase.SerializePerformance.FieldData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.FloatFieldData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.FloatPropertyData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.PropertyData/**/.CreateRandomObject();
                    AutoCSer.TestCase.SerializePerformance.FieldData/**/.XmlSerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatFieldData/**/.XmlSerialize();
                    AutoCSer.TestCase.SerializePerformance.FloatPropertyData/**/.XmlSerialize();
                    AutoCSer.TestCase.SerializePerformance.PropertyData/**/.XmlSerialize();
                    AutoCSer.RandomObject.Creator.CreateBool(null);
                    AutoCSer.RandomObject.Creator.CreateByte(null);
                    AutoCSer.RandomObject.Creator.CreateSByte(null);
                    AutoCSer.RandomObject.Creator.CreateShort(null);
                    AutoCSer.RandomObject.Creator.CreateUShort(null);
                    AutoCSer.RandomObject.Creator.CreateInt(null);
                    AutoCSer.RandomObject.Creator.CreateUInt(null);
                    AutoCSer.RandomObject.Creator.CreateLong(null);
                    AutoCSer.RandomObject.Creator.CreateULong(null);
                    AutoCSer.RandomObject.Creator.CreateDateTime(null);
                    AutoCSer.RandomObject.Creator.CreateTimeSpan(null);
                    AutoCSer.RandomObject.Creator.CreateGuid(null);
                    AutoCSer.RandomObject.Creator.CreateChar(null);
                    AutoCSer.RandomObject.Creator.CreateString(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<bool>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<byte>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<sbyte>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<short>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<ushort>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<int>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<uint>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<long>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<ulong>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<System.DateTime>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<System.TimeSpan>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<System.Guid>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<char>(null);
                    AutoCSer.RandomObject.Creator.CreateFloat(null);
                    AutoCSer.RandomObject.Creator.CreateDouble(null);
                    AutoCSer.RandomObject.Creator.CreateDecimal(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<float>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<double>(null);
                    AutoCSer.RandomObject.Creator.CreateNullable<decimal>(null);

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.Guid>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<long>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.DateTime>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.TimeSpan>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<uint?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ulong>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<uint>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<long?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<short?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ulong?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.TimeSpan?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ushort?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.DateTime?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<char?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<System.Guid?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<short>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ushort>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<byte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<char>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<sbyte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<sbyte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<bool?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<byte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<bool>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<decimal>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<double>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<float?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<decimal?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<float>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<double?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>));
                    AutoCSer.BinarySerializer.Json<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>(null, default(AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>));
                    AutoCSer.BinarySerializer.Json<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>(null, default(AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData));
                    binaryDeserializeMemberTypes();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<bool>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<bool?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<byte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<byte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<char>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<char?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.DateTime>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.DateTime?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.Guid>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.Guid?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<int?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<long>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<long?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<sbyte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<sbyte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<short>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<short?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.TimeSpan>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<System.TimeSpan?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<uint>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<uint?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<ulong>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<ulong?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<ushort>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<ushort?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<decimal>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<decimal?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<double>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<double?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<float>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<float?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>));
                    AutoCSer.JsonSerializer.Base<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData, AutoCSer.TestCase.SerializePerformance.FloatFieldData>(null, default(AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>));
                    AutoCSer.JsonSerializer.Base<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData, AutoCSer.TestCase.SerializePerformance.FloatPropertyData>(null, default(AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData));
                    jsonDeserializeMemberTypes();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<bool>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<bool?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<byte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<byte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<char>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<char?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.DateTime>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.DateTime?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.Guid>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.Guid?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<int?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<long>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<long?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<sbyte>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<sbyte?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<short>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<short?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.TimeSpan>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<System.TimeSpan?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<uint>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<uint?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<ulong>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<ulong?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<ushort>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<ushort?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<decimal>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<decimal?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<double>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<double?>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<float>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<float?>));
                    AutoCSer.XmlSerializer.NullableHasValue<bool>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<byte>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<char>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<System.DateTime>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<System.Guid>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<int>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<long>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<sbyte>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<short>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<System.TimeSpan>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<uint>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<ulong>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<ushort>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<decimal>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<double>(null);
                    AutoCSer.XmlSerializer.NullableHasValue<float>(null);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
                AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData t1 = default(AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData);
                AutoCSer.BinaryDeserializer.Json<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData>(null, ref t1);
                AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData t2 = default(AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData);
                AutoCSer.BinaryDeserializer.Json<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData>(null, ref t2);
            }
            /// <summary>
            /// JSON 反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void jsonDeserializeMemberTypes()
            {
                AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData t3 = default(AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData);
                AutoCSer.JsonDeserializer.Base<AutoCSer.TestCase.SerializePerformance.JsonFloatFieldData, AutoCSer.TestCase.SerializePerformance.FloatFieldData>(null, ref t3);
                AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData t4 = default(AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData);
                AutoCSer.JsonDeserializer.Base<AutoCSer.TestCase.SerializePerformance.JsonFloatPropertyData, AutoCSer.TestCase.SerializePerformance.FloatPropertyData>(null, ref t4);
            }
    }
}
#endif