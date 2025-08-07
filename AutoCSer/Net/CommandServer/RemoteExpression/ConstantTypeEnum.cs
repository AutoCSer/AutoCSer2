using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程表达式序列化常量类型
    /// </summary>
    internal enum ConstantTypeEnum : byte
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown,
        /// <summary>
        /// false
        /// </summary>
        Bool,
        /// <summary>
        /// byte
        /// </summary>
        Byte,
        /// <summary>
        /// sbyte
        /// </summary>
        SByte,
        /// <summary>
        /// short
        /// </summary>
        Short,
        /// <summary>
        /// ushort
        /// </summary>
        UShort,
        /// <summary>
        /// int
        /// </summary>
        Int,
        /// <summary>
        /// uint
        /// </summary>
        UInt,
        /// <summary>
        /// long
        /// </summary>
        Long,
        /// <summary>
        /// ulong
        /// </summary>
        ULong,
        /// <summary>
        /// float
        /// </summary>
        Float,
        /// <summary>
        /// double
        /// </summary>
        Double,
        /// <summary>
        /// decimal
        /// </summary>
        Decimal,
        /// <summary>
        /// char
        /// </summary>
        Char,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime,
        /// <summary>
        /// TimeSpan
        /// </summary>
        TimeSpan,
        /// <summary>
        /// Guid
        /// </summary>
        Guid,
        /// <summary>
        /// bool? false
        /// </summary>
        NullableBool,
        /// <summary>
        /// byte?
        /// </summary>
        NullableByte,
        /// <summary>
        /// sbyte?
        /// </summary>
        NullableSByte,
        /// <summary>
        /// short?
        /// </summary>
        NullableShort,
        /// <summary>
        /// ushort?
        /// </summary>
        NullableUShort,
        /// <summary>
        /// int?
        /// </summary>
        NullableInt,
        /// <summary>
        /// uint?
        /// </summary>
        NullableUInt,
        /// <summary>
        /// long?
        /// </summary>
        NullableLong,
        /// <summary>
        /// ulong?
        /// </summary>
        NullableULong,
        /// <summary>
        /// float?
        /// </summary>
        NullableFloat,
        /// <summary>
        /// double?
        /// </summary>
        NullableDouble,
        /// <summary>
        /// decimal?
        /// </summary>
        NullableDecimal,
        /// <summary>
        /// char?
        /// </summary>
        NullableChar,
        /// <summary>
        /// DateTime?
        /// </summary>
        NullableDateTime,
        /// <summary>
        /// TimeSpan?
        /// </summary>
        NullableTimeSpan,
        /// <summary>
        /// Guid?
        /// </summary>
        NullableGuid,
        /// <summary>
        /// System.Numerics.Complex
        /// </summary>
        Complex,
        /// <summary>
        /// System.Numerics.Plane
        /// </summary>
        Plane,
        /// <summary>
        /// System.Numerics.Quaternion
        /// </summary>
        Quaternion,
        /// <summary>
        /// System.Numerics.Matrix3x2
        /// </summary>
        Matrix3x2,
        /// <summary>
        /// System.Numerics.Matrix4x4
        /// </summary>
        Matrix4x4,
        /// <summary>
        /// System.Numerics.Vector2
        /// </summary>
        Vector2,
        /// <summary>
        /// System.Numerics.Vector3
        /// </summary>
        Vector3,
        /// <summary>
        /// System.Numerics.Vector4
        /// </summary>
        Vector4,
        /// <summary>
        /// Half
        /// </summary>
        Half,
        /// <summary>
        /// Int128
        /// </summary>
        Int128,
        /// <summary>
        /// UInt128
        /// </summary>
        UInt128,
        /// <summary>
        /// byte[]
        /// </summary>
        ByteArray,
        /// <summary>
        /// string
        /// </summary>
        String,
        /// <summary>
        /// string null
        /// </summary>
        NullString,
        /// <summary>
        /// string[0]
        /// </summary>
        EmptyString,
        /// <summary>
        /// string[1]
        /// </summary>
        CharString,
        /// <summary>
        /// byte[] null
        /// </summary>
        NullByteArray,
        /// <summary>
        /// byte[0]
        /// </summary>
        EmptyByteArray,
        /// <summary>
        /// byte[1]
        /// </summary>
        ByteArray1,
        /// <summary>
        /// byte[2]
        /// </summary>
        ByteArray2,
        /// <summary>
        /// true
        /// </summary>
        True,
        /// <summary>
        /// bool? true
        /// </summary>
        NullableBoolTrue,
        /// <summary>
        /// bool? null
        /// </summary>
        NullBool,
        /// <summary>
        /// byte? null
        /// </summary>
        NullByte,
        /// <summary>
        /// sbyte? null
        /// </summary>
        NullSByte,
        /// <summary>
        /// short? null
        /// </summary>
        NullShort,
        /// <summary>
        /// ushort? null
        /// </summary>
        NullUShort,
        /// <summary>
        /// int? null
        /// </summary>
        NullInt,
        /// <summary>
        /// uint? null
        /// </summary>
        NullUInt,
        /// <summary>
        /// long? null
        /// </summary>
        NullLong,
        /// <summary>
        /// ulong? null
        /// </summary>
        NullULong,
        /// <summary>
        /// float? null
        /// </summary>
        NullFloat,
        /// <summary>
        /// double? null
        /// </summary>
        NullDouble,
        /// <summary>
        /// decimal? null
        /// </summary>
        NullDecimal,
        /// <summary>
        /// char? null
        /// </summary>
        NullChar,
        /// <summary>
        /// DateTime? null
        /// </summary>
        NullDateTime,
        /// <summary>
        /// TimeSpan? null
        /// </summary>
        NullTimeSpan,
        /// <summary>
        /// Guid? null
        /// </summary>
        NullGuid,
    }
}
