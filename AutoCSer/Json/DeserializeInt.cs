using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 整数反序列化
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeInt
    {
        /// <summary>
        /// 整数
        /// </summary>
        public uint Number;
        /// <summary>
        /// 正负符号
        /// </summary>
        public byte Sign;
        /// <summary>
        /// 是否 null
        /// </summary>
        public byte IsNull;
        ///// <summary>
        ///// 获取整数值
        ///// </summary>
        //internal int Int
        //{
        //    get { return Sign == 0 ? (int)Number : -(int)Number; }
        //}
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="number"></param>
        internal DeserializeInt(uint number)
        {
            Number = number;
            Sign = IsNull = 0;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal byte? GetByteNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0)
            {
                if ((Number & 0xffffff00U) != 0) state = DeserializeStateEnum.NumberOutOfRange;
                return (byte)Number;
            }
            return null;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal sbyte GetSByte(ref DeserializeStateEnum state)
        {
            if (Sign == 0)
            {
                if (Number <= sbyte.MaxValue) return (sbyte)(byte)Number;
            }
            else if (Number <= -sbyte.MinValue) return (sbyte)-(int)Number;
            state = DeserializeStateEnum.NumberOutOfRange;
            return 0;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal sbyte? GetSByteNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0) return GetSByte(ref state);
            return null;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal ushort? GetUShortNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0)
            {
                if ((Number & 0xffff0000U) != 0) state = DeserializeStateEnum.NumberOutOfRange;
                return (ushort)Number;
            }
            return null;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal short GetShort(ref DeserializeStateEnum state)
        {
            if (Sign == 0)
            {
                if (Number <= short.MaxValue) return (short)(ushort)Number;
            }
            else if (Number <= -short.MinValue) return (short)-(int)Number;
            state = DeserializeStateEnum.NumberOutOfRange;
            return 0;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal short? GetShortNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0) return GetShort(ref state);
            return null;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint? GetUIntNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0) return (uint)Number;
            return null;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal int GetInt(ref DeserializeStateEnum state)
        {
            if (Sign == 0)
            {
                if (Number <= int.MaxValue) return (int)Number;
            }
            else
            {
                if (Number < (1U << 31)) return -(int)Number;
                if (Number == (1U << 31)) return int.MinValue;
            }
            state = DeserializeStateEnum.NumberOutOfRange;
            return 0;
        }
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int? GetIntNull(ref DeserializeStateEnum state)
        {
            if (IsNull == 0) return GetInt(ref state);
            return null;
        }
    }
}
