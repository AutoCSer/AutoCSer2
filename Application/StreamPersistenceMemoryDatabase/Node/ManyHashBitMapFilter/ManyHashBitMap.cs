using AutoCSer.Algorithm;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ManyHashBitMap
    {
        /// <summary>
        /// 位图数据
        /// </summary>
        internal ulong[] Map;
        /// <summary>
        /// 位数量取余操作
        /// </summary>
        internal IntegerDivision SizeDivision;
        /// <summary>
        /// 位图大小（位数量）
        /// </summary>
        internal int Size { get { return (int)SizeDivision.Divisor; } }
        /// <summary>
        /// 设置位图大小
        /// </summary>
        /// <param name="size"></param>
        internal void Set(int size)
        {
            SizeDivision.Set(Math.Max(size, 2));
            Map = new ulong[((long)Size + 63) >> 6];
        }
        /// <summary>
        /// 获取位数据
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ulong GetBitValueBeforePersistence(int bit)
        {
            if ((uint)bit < (uint)Size) return GetBitValue(bit);
            return 1;
        }
        /// <summary>
        /// 设置位
        /// </summary>
        /// <param name="bit"></param>
        /// <returns>是否设置新位</returns>
        internal bool CheckSetBit(int bit)
        {
            ulong value = 1UL << (bit & 63);
            int index = bit >> 6;
            if ((Map[index] & value) != 0) return false;
            Map[index] |= value;
            return true;
        }
        /// <summary>
        /// 根据哈希值获取位置
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int GetBitByHashCode(uint hashCode)
        {
            return (int)SizeDivision.GetMod(hashCode);
            //int bit = hashCode % Size;
            //return bit >= 0 ? bit : (bit + Size);
        }
        /// <summary>
        /// 获取位数据
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ulong GetBitValue(int bit)
        {
            return Map[bit >> 6] & (1UL << (bit & 63));
        }
        /// <summary>
        /// 获取位数据
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ulong GetBitValueByHashCode(uint hashCode)
        {
            return GetBitValue(GetBitByHashCode(hashCode));
        }
        /// <summary>
        /// 合并位图数据
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        internal unsafe bool Merge(ManyHashBitMap map)
        {
            if (((Size ^ map.Size) | (this.Map.Length ^ map.Map.Length)) == 0)
            {
                fixed (ulong* mapFixed = this.Map, mergeMapFixed = map.Map)
                {
                    byte* read = (byte*)mergeMapFixed, write = (byte*)mapFixed;
                    int length = this.Map.Length;
                    while (length >= 4)
                    {
                        *(ulong*)write |= *(ulong*)read;
                        *(ulong*)(write + sizeof(ulong)) |= *(ulong*)(read + sizeof(ulong));
                        *(ulong*)(write + sizeof(ulong) * 2) |= *(ulong*)(read + sizeof(ulong) * 2);
                        *(ulong*)(write + sizeof(ulong) * 3) |= *(ulong*)(read + sizeof(ulong) * 3);
                        length -= 4;
                        read += sizeof(ulong) * 4;
                        write += sizeof(ulong) * 4;
                    }
                    if ((length & 2) != 0)
                    {
                        *(ulong*)write |= *(ulong*)read;
                        *(ulong*)(write + sizeof(ulong)) |= *(ulong*)(read + sizeof(ulong));
                        read += sizeof(ulong) * 2;
                        write += sizeof(ulong) * 2;
                    }
                    if ((length & 1) != 0) *(ulong*)write |= *(ulong*)read;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置位
        /// </summary>
        /// <param name="bit"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBit(int bit)
        {
            Map[bit >> 6] |= 1UL << (bit & 63);
        }
    }
}
