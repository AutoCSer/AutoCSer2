using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 字段信息
    /// </summary>
    internal unsafe class FieldSize
    {
        /// <summary>
        /// 字段索引
        /// </summary>
        public readonly FieldIndex FieldIndex;
        /// <summary>
        /// 字段信息
        /// </summary>
        public FieldInfo Field { get { return FieldIndex.Member; } }
        /// <summary>
        /// 成员索引
        /// </summary>
        public int MemberIndex { get { return FieldIndex.MemberIndex; } }
        /// <summary>
        /// 成员选择类型
        /// </summary>
        internal MemberFiltersEnum MemberFilters { get { return FieldIndex.MemberFilters; } }
        /// <summary>
        /// 固定分组排序字节数
        /// </summary>
        internal readonly byte FixedSize;
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field"></param>
        internal FieldSize(FieldIndex field)
        {
            this.FieldIndex = field;
            if (Field.FieldType.IsEnum) fixedSizes.TryGetValue(System.Enum.GetUnderlyingType(Field.FieldType), out FixedSize);
            else fixedSizes.TryGetValue(Field.FieldType, out FixedSize);
        }
        /// <summary>
        /// 固定分组排序字节数排序比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int FixedSizeSort(FieldSize left, FieldSize right)
        {
            return (int)((uint)right.FixedSize & (0U - (uint)right.FixedSize)) - (int)((uint)left.FixedSize & (0U - (uint)left.FixedSize));
        }

        /// <summary>
        /// 固定类型字节数
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, byte> fixedSizes;
        /// <summary>
        /// 是否固定字节数类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsFixedSize(Type type)
        {
            return type.IsEnum || fixedSizes.ContainsKey(type);
        }

        static FieldSize()
        {
            fixedSizes = DictionaryCreator.CreateHashObject<System.Type, byte>();
            fixedSizes.Add(typeof(bool), sizeof(bool));
            fixedSizes.Add(typeof(byte), sizeof(byte));
            fixedSizes.Add(typeof(sbyte), sizeof(sbyte));
            fixedSizes.Add(typeof(short), sizeof(short));
            fixedSizes.Add(typeof(ushort), sizeof(ushort));
            fixedSizes.Add(typeof(int), sizeof(int));
            fixedSizes.Add(typeof(uint), sizeof(uint));
            fixedSizes.Add(typeof(long), sizeof(long));
            fixedSizes.Add(typeof(ulong), sizeof(ulong));
            fixedSizes.Add(typeof(Int128), (byte)sizeof(Int128));
            fixedSizes.Add(typeof(UInt128), (byte)sizeof(UInt128));
            fixedSizes.Add(typeof(char), sizeof(char));
            fixedSizes.Add(typeof(DateTime), sizeof(long));
            fixedSizes.Add(typeof(TimeSpan), sizeof(long));
            //fixedSizes.Add(typeof(DateTimeOffset), (byte)sizeof(DateTimeOffset));//16
            //fixedSizes.Add(typeof(TimeOnly), (byte)sizeof(TimeOnly));//8
            //fixedSizes.Add(typeof(DateOnly), (byte)sizeof(DateOnly));//4
            fixedSizes.Add(typeof(Half), (byte)sizeof(Half));
            fixedSizes.Add(typeof(float), sizeof(float));
            fixedSizes.Add(typeof(double), sizeof(double));
            fixedSizes.Add(typeof(System.Numerics.Complex), (byte)sizeof(System.Numerics.Complex));//16
            fixedSizes.Add(typeof(System.Numerics.Plane), (byte)sizeof(System.Numerics.Plane));//16
            fixedSizes.Add(typeof(System.Numerics.Quaternion), (byte)sizeof(System.Numerics.Quaternion));//16
            fixedSizes.Add(typeof(System.Numerics.Matrix3x2), (byte)sizeof(System.Numerics.Matrix3x2));//24
            fixedSizes.Add(typeof(System.Numerics.Matrix4x4), (byte)sizeof(System.Numerics.Matrix4x4));//64
            fixedSizes.Add(typeof(System.Numerics.Vector2), (byte)sizeof(System.Numerics.Vector2));//8
            fixedSizes.Add(typeof(System.Numerics.Vector3), (byte)sizeof(System.Numerics.Vector3));//12
            fixedSizes.Add(typeof(System.Numerics.Vector4), (byte)sizeof(System.Numerics.Vector4));//16
            fixedSizes.Add(typeof(decimal), sizeof(decimal));
            fixedSizes.Add(typeof(Guid), (byte)sizeof(Guid));
            fixedSizes.Add(typeof(bool?), sizeof(byte));
            fixedSizes.Add(typeof(byte?), sizeof(ushort));
            fixedSizes.Add(typeof(sbyte?), sizeof(ushort));
            fixedSizes.Add(typeof(short?), sizeof(uint));
            fixedSizes.Add(typeof(ushort?), sizeof(uint));
            fixedSizes.Add(typeof(int?), sizeof(int) + sizeof(int));
            fixedSizes.Add(typeof(uint?), sizeof(uint) + sizeof(int));
            fixedSizes.Add(typeof(long?), sizeof(long) + sizeof(int));
            fixedSizes.Add(typeof(ulong?), sizeof(ulong) + sizeof(int));
            //fixedSizes.Add(typeof(Int128?), (byte)(sizeof(Int128) + sizeof(int)));
            //fixedSizes.Add(typeof(UInt128?), (byte)(sizeof(UInt128) + sizeof(int)));
            fixedSizes.Add(typeof(char?), sizeof(uint));
            fixedSizes.Add(typeof(DateTime?), sizeof(long) + sizeof(int));
            fixedSizes.Add(typeof(TimeSpan?), sizeof(long) + sizeof(int));
            //fixedSizes.Add(typeof(DateTimeOffset?), (byte)(sizeof(DateTimeOffset) + sizeof(int)));
            //fixedSizes.Add(typeof(TimeOnly?), (byte)(sizeof(TimeOnly) + sizeof(int)));
            //fixedSizes.Add(typeof(DateOnly?), (byte)(sizeof(DateOnly) + sizeof(int)));
            //fixedSizes.Add(typeof(Half?), sizeof(int));
            fixedSizes.Add(typeof(float?), sizeof(float) + sizeof(int));
            fixedSizes.Add(typeof(double?), sizeof(double) + sizeof(int));
            //fixedSizes.Add(typeof(System.Numerics.Complex?), (byte)(sizeof(System.Numerics.Complex) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Plane?), (byte)(sizeof(System.Numerics.Plane) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Quaternion?), (byte)(sizeof(System.Numerics.Quaternion) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Matrix3x2?), (byte)(sizeof(System.Numerics.Matrix3x2) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Matrix4x4?), (byte)(sizeof(System.Numerics.Matrix4x4) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Vector2?), (byte)(sizeof(System.Numerics.Vector2) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Vector3?), (byte)(sizeof(System.Numerics.Vector3) + sizeof(int)));
            //fixedSizes.Add(typeof(System.Numerics.Vector4?), (byte)(sizeof(System.Numerics.Vector4) + sizeof(int)));
            fixedSizes.Add(typeof(decimal?), sizeof(decimal) + sizeof(int));
            fixedSizes.Add(typeof(Guid?), (byte)(sizeof(Guid) + sizeof(int)));
        }
    }
}
