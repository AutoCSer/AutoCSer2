using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 128b 哈希值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct HashCode128
    {
        /// <summary>
        /// 高 64b
        /// </summary>
        private readonly ulong high;
        /// <summary>
        /// 低 64b
        /// </summary>
        private readonly ulong low;
        /// <summary>
        /// 128b 哈希值
        /// </summary>
        /// <param name="value"></param>
        internal unsafe HashCode128(string value)
        {
            if (value != null)
            {
                if (value.Length != 0)
                {
                    fixed (char* valueFixed = value)
                    {
                        ulong length = (ulong)value.Length << 1;
                        if (length >= sizeof(ulong) * 2)
                        {
                            byte* start = (byte*)valueFixed;
                            ulong low = *(ulong*)start, high = *(ulong*)(start + sizeof(ulong));
                            for (byte* end = start + (length & (int.MaxValue - sizeof(ulong) * 2 + 1)); (start += sizeof(ulong) * 2) != end;)
                            {
                                low += AutoCSer.Memory.Common.AddHashCode;
                                high += AutoCSer.Memory.Common.AddHashCode;
                                low = *(ulong*)start ^ (low << 53) ^ (low >> 11);
                                high = *(ulong*)(start + sizeof(ulong)) ^ (high << 11) ^ (high >> 53);
                            }
                            if ((length & (sizeof(ulong) * 2 - 1)) != 0)
                            {
                                low += AutoCSer.Memory.Common.AddHashCode;
                                this.low = *(ulong*)start ^ (low << 53) ^ (low >> 11);
                                start += sizeof(ulong);
                            }
                            else this.low = low;
                            if ((length & (sizeof(ulong) - 1)) != 0)
                            {
                                high += AutoCSer.Memory.Common.AddHashCode;
                                high = (*(ulong*)start << ((sizeof(ulong) - (int)(length & (sizeof(ulong) - 1))) << 3)) ^ (high << 11) ^ (high >> 53);
                            }
                            this.high = high ^ ((ulong)high << 19);
                        }
                        else if (length >= sizeof(ulong))
                        {
                            low = *(ulong*)valueFixed;
                            if (length != sizeof(ulong)) high = (*(ulong*)((byte*)valueFixed + sizeof(ulong)) << ((sizeof(ulong) - (int)length) << 3)) ^ (length << 19);
                            else high = (ulong)value.Length;
                        }
                        else
                        {
                            low = *(ulong*)valueFixed << ((sizeof(ulong) - (int)length) << 3);
                            high = (ulong)value.Length;
                        }
                    }
                }
                else high = low = 0;
            }
            else high = low = ulong.MaxValue;
        }
        /// <summary>
        /// 128b 哈希值
        /// </summary>
        /// <param name="high">高 64b</param>
        /// <param name="low">低 64b</param>
        private HashCode128(ulong high, ulong low)
        {
            this.high = high;
            this.low = low;
        }
        /// <summary>
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        private ulong getHashCode2()
        {
            ulong hashCode = (high + ((AutoCSer.Memory.Common.AddHashCode << 23) | (AutoCSer.Memory.Common.AddHashCode >> 41))) ^ ((low << 53) | (low >> 11));
            hashCode = (hashCode + ((AutoCSer.Memory.Common.AddHashCode << 43) | (AutoCSer.Memory.Common.AddHashCode >> 21))) ^ ((hashCode << 29) | (hashCode >> 35));
            return (hashCode + AutoCSer.Memory.Common.AddHashCode) ^ ((hashCode << 17) | (hashCode >> 47));
        }
        /// <summary>
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<uint> GetHashCode2()
        {
            ulong hashCode = getHashCode2();
            yield return (uint)hashCode;
            yield return (uint)(hashCode >> 32);
        }
        /// <summary>
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint[] GetHashCodeArray2()
        {
            ulong hashCode = getHashCode2();
            return new uint[] { (uint)hashCode, (uint)(hashCode >> 32) };
        }
        /// <summary>
        /// 获取 4 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        private HashCode128 getHashCode()
        {
            ulong high = this.high + ((AutoCSer.Memory.Common.AddHashCode << 11) | (AutoCSer.Memory.Common.AddHashCode >> 53)), low = this.low + ((AutoCSer.Memory.Common.AddHashCode << 53) | (AutoCSer.Memory.Common.AddHashCode >> 11));
            high ^= (this.high << 61) | (this.low >> 3);
            low ^= (this.low << 61) | (this.high >> 3);
            ulong nextHigh = high + ((AutoCSer.Memory.Common.AddHashCode << 23) | (AutoCSer.Memory.Common.AddHashCode >> 41)), nextLow = low + ((AutoCSer.Memory.Common.AddHashCode << 41) | (AutoCSer.Memory.Common.AddHashCode >> 23));
            nextHigh ^= (high << 29) | (low >> 35);
            nextLow ^= (low << 29) | (high >> 35);
            high = nextHigh + ((AutoCSer.Memory.Common.AddHashCode << 31) | (AutoCSer.Memory.Common.AddHashCode >> 33));
            low = nextLow + AutoCSer.Memory.Common.AddHashCode;
            return new HashCode128(high ^ ((nextHigh << 17) | (nextLow >> 47)), low ^ ((nextLow << 17) | (nextHigh >> 47)));
        }
        /// <summary>
        /// 获取 3 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<uint> GetHashCode3()
        {
            HashCode128 hashCode = getHashCode();
            yield return (uint)hashCode.low;
            yield return (uint)(hashCode.low >> 32);
            yield return (uint)hashCode.high;
        }
        /// <summary>
        /// 获取 3 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint[] GetHashCodeArray3()
        {
            HashCode128 hashCode = getHashCode();
            return new uint[] { (uint)hashCode.low, (uint)(hashCode.low >> 32), (uint)hashCode.high };
        }
        /// <summary>
        /// 获取 4 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<uint> GetHashCode4()
        {
            HashCode128 hashCode = getHashCode();
            yield return (uint)hashCode.low;
            yield return (uint)(hashCode.low >> 32);
            yield return (uint)hashCode.high;
            yield return (uint)(hashCode.high >> 32);
        }
        /// <summary>
        /// 获取 4 个 32b 的哈希值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint[] GetHashCodeArray4()
        {
            HashCode128 hashCode = getHashCode();
            return new uint[] { (uint)hashCode.low, (uint)(hashCode.low >> 32), (uint)hashCode.high, (uint)(hashCode.high >> 32) };
        }
        ///// <summary>
        ///// 位混淆计算
        ///// </summary>
        ///// <param name="hashCode"></param>
        ///// <returns></returns>
        //internal static ulong BitConfusion(ulong hashCode)
        //{
        //    hashCode = (hashCode + addValue) ^ ((hashCode << 32) | (hashCode >> 32));
        //    hashCode = (hashCode + ((addValue << 11) | (addValue >> 53))) ^ ((hashCode << 16) | (hashCode >> 48));
        //    hashCode = (hashCode + ((addValue << 23) | (addValue >> 41))) ^ ((hashCode << 8) | (hashCode >> 56));
        //    hashCode = (hashCode + ((addValue << 31) | (addValue >> 33))) ^ ((hashCode << 4) | (hashCode >> 60));
        //    hashCode = (hashCode + ((addValue << 41) | (addValue >> 23))) ^ ((hashCode << 2) | (hashCode >> 62));
        //    return (hashCode + ((addValue << 53) | (addValue >> 11))) ^ ((hashCode << 1) | (hashCode >> 63));
        //}
    }
}
