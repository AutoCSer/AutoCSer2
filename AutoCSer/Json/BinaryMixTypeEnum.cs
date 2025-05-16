using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 二进制混杂类型
    /// </summary>
    internal enum BinaryMixTypeEnum : byte
    {
        /// <summary>
        /// byte
        /// </summary>
        Byte = 0x80,
        /// <summary>
        /// ushort
        /// </summary>
        UShort,
        /// <summary>
        /// uint
        /// </summary>
        UInt,
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
        /// 无意义数据，用于填充对齐数据
        /// </summary>
        None,
        /// <summary>
        /// 长度用 1 Byte 表示的字符串
        /// </summary>
        StringByte,
        /// <summary>
        /// 长度用 3 Byte 表示的字符串
        /// </summary>
        StringByte3,
        /// <summary>
        /// 长度用 4 Byte 表示的字符串
        /// </summary>
        String,
        /// <summary>
        /// Int128
        /// </summary>
        UInt128,
        /// <summary>
        /// 长度用 1 Byte 表示的数组
        /// </summary>
        ArrayByte,
        /// <summary>
        /// 长度用 3 Byte 表示的数组
        /// </summary>
        ArrayByte3,
        /// <summary>
        /// 长度用 4 Byte 表示的数组
        /// </summary>
        Array,
        /// <summary>
        /// bool 数组 false
        /// </summary>
        ArrayFalse,
        /// <summary>
        /// bool 数组 true
        /// </summary>
        ArrayTrue,
        /// <summary>
        /// false
        /// </summary>
        False,
        /// <summary>
        /// true
        /// </summary>
        True,
        /// <summary>
        /// 浮点数
        /// </summary>
        Half,
        /// <summary>
        /// 复数 System.Numerics.Complex
        /// </summary>
        Complex,
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
    }
}
