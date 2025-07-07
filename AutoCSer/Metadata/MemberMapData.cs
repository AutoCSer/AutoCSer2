using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct MemberMapData<T> : IEquatable<MemberMapData<T>>
    {
        /// <summary>
        /// 64 位成员位图
        /// </summary>
        private ulong map64;
        /// <summary>
        /// 超过 64 位的成员位图，null 表示默认全部成员有效
        /// </summary>
#if NetStandard21
        private ulong[]? map;
#else
        private ulong[] map;
#endif
        /// <summary>
        /// 是否默认全部成员有效
        /// </summary>
        internal bool IsDefault { get { return map == null; } }
        /// <summary>
        /// 是否存在成员（未初始化表示不存在成员）
        /// </summary>
        internal bool IsAnyMember
        {
            get
            {
                if (map == null) return false;
                if (map64 != 0) return true;
                foreach (ulong nextMap in map)
                {
                    if (nextMap != 0) return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <param name="map64"></param>
        /// <param name="map"></param>
#if NetStandard21
        private MemberMapData(ulong map64, ulong[]? map)
#else
        private MemberMapData(ulong map64, ulong[] map)
#endif
        {
            this.map64 = map64;
            this.map = map;
        }
        /// <summary>
        /// 成员位图比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MemberMapData<T> other)
        {
            if (map == null) return other.map == null;
            if (other.map != null)
            {
                ulong value = map64 ^ other.map64;
                if (map.Length != 0)
                {
                    fixed (ulong* start = map)
                    {
                        ulong* read = start;
                        foreach (ulong otherMap in other.map) value |= otherMap ^ *read++;
                    }
                }
                return value == 0;
            }
            return false;
        }
        /// <summary>
        /// 成员位图比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<MemberMapData<T>>());
        }
        /// <summary>
        /// Calculate the hash value
        /// 计算哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            ulong value = GetHashCode64();
            return (int)((uint)value ^ (uint)(value >> 32));
        }
        /// <summary>
        /// Calculate the hash value
        /// 计算哈希值
        /// </summary>
        /// <returns></returns>
        internal ulong GetHashCode64()
        {
            if (map == null) return 0;
            ulong value = map64;
            foreach (ulong nextMap in map) value ^= nextMap;
            return value;
        }
        /// <summary>
        /// 设置成员数组值
        /// </summary>
        /// <param name="value"></param>
        private void setMap(ulong value)
        {
            //AutoCSer.Common.Fill(map, value);
            fixed (ulong* start = map)
            {
                ulong* end = start + memberMapArraySize;
                do
                {
                    *--end = value;
                }
                while (end != start);
            }
        }
        /// <summary>
        /// 清空所有成员
        /// </summary>
        internal void Empty()
        {
            if (map == null) map = memberMapArraySize == 0 ? EmptyArray<ulong>.Array : new ulong[memberMapArraySize];
            else if (map.Length != 0) setMap(0);
            map64 = 0;
        }
        /// <summary>
        /// 添加所有成员
        /// </summary>
        internal void Full()
        {
            if (map == null)
            {
                if (memberMapArraySize == 0) map = EmptyArray<ulong>.Array;
                else
                {
                    map = AutoCSer.Common.GetUninitializedArray<ulong>(memberMapArraySize);
                    setMap(ulong.MaxValue);
                }
            }
            else if (map.Length != 0) setMap(ulong.MaxValue);
            map64 = ulong.MaxValue;
        }
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        internal bool IsMember(int memberIndex)
        {
            if (map == null) return true;
            if (memberIndex < 64) return (map64 & (1UL << memberIndex)) != 0;
            return (map[(memberIndex >> 6) - 1] & (1UL << (memberIndex & 63))) != 0;
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        internal void SetMember(int memberIndex)
        {
            if (map == null) map = memberMapArraySize == 0 ? EmptyArray<ulong>.Array : new ulong[memberMapArraySize];
            if (memberIndex < 64) map64 |= 1UL << memberIndex;
            else map[(memberIndex >> 6) - 1] |= 1UL << (memberIndex & 63);
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>Return false on failure</returns>
#if NetStandard21
        internal bool SetMember(string? memberName)
#else
        internal bool SetMember(string memberName)
#endif
        {
            if (memberName != null)
            {
                int index = nameIndexSearcher.Search(memberName);
                if (index >= 0)
                {
                    SetMember(index);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        internal void ClearMember(int memberIndex)
        {
            if (map != null)
            {
                if (memberIndex < 64) map64 &= ulong.MaxValue ^ (1UL << memberIndex);
                else map[(memberIndex >> 6) - 1] &= ulong.MaxValue ^ (1UL << (memberIndex & 63));
            }
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberName">成员名称</param>
#if NetStandard21
        public void ClearMember(string? memberName)
#else
        public void ClearMember(string memberName)
#endif
        {
            if (memberName != null)
            {
                int index = nameIndexSearcher.Search(memberName);
                if (index >= 0) ClearMember(index);
            }
        }
        /// <summary>
        /// 成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal MemberMapData<T> Copy()
        {
            return new MemberMapData<T>(map64, AutoCSer.Common.GetUninitializedArray(map));
        }
        /// <summary>
        /// 成员交集运算
        /// </summary>
        /// <param name="other">成员位图</param>
        internal void And(MemberMapData<T> other)
        {
            if (map == null)
            {
                map = AutoCSer.Common.GetUninitializedArray(other.map);
                map64 = other.map64;
            }
            else if (other.map != null)
            {
                if (map.Length != 0)
                {
                    fixed (ulong* start = map)
                    {
                        ulong* write = start;
                        foreach (ulong otherMap in other.map) *write++ &= otherMap;
                    }
                }
                map64 &= other.map64;
            }
        }
        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="stream"></param>
        internal void Serialize(UnmanagedStream stream)
        {
            if (map == null) stream.Write(0);
            else
            {
                int size = (fieldCount + (31 + 32)) >> 5;
                byte* data = stream.GetBeforeMove(size << 2);
                if (data != null)
                {
                    *(int*)data = fieldCount + 1;
                    switch (size)
                    {
                        case 1: return;
                        case 2: *(uint*)(data += sizeof(int)) = (uint)map64; return;
                        default:
                            *(ulong*)(data += sizeof(int)) = map64;
                            if ((size -= 3) != 0)
                            {
                                foreach (ulong nextMap in map)
                                {
                                    data += sizeof(ulong);
                                    if (size == 1)
                                    {
                                        *(uint*)data = (uint)nextMap;
                                        return;
                                    }
                                    *(ulong*)data = nextMap;
                                    if ((size -= 2) == 0) return;
                                }
                            }
                            return;
                    }
                }
            }
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal bool Deserialize(BinaryDeserializer deserializer)
        {
            byte* read = deserializer.Current;
            if (*(int*)read == 0)
            {
                map64 = 0;
                map = null;
                deserializer.Current += sizeof(int);
                return true;
            }
            if (*(int*)read == fieldCount + 1)
            {
                int size = ((fieldCount + 31) >> 5) << 2;
                if (size <= (int)(deserializer.End - (read += sizeof(int))))
                {
                    if (map == null) map = memberMapArraySize == 0 ? EmptyArray<ulong>.Array : AutoCSer.Common.GetUninitializedArray<ulong>(memberMapArraySize);
                    if (size == 4)
                    {
                        map64 = *(uint*)read;
                        read += sizeof(uint);
                    }
                    else if (size != 0)
                    {
                        map64 = *(ulong*)read;
                        read += sizeof(ulong);
                        if ((size -= sizeof(ulong)) != 0)
                        {
                            fixed (ulong* start = map)
                            {
                                ulong* write = start;
                                for (ulong* end = start + (size >> 3); write != end; read += sizeof(ulong)) *write++ = *(ulong*)read;
                                if ((size & 4) != 0)
                                {
                                    *write++ = *(uint*)read;
                                    read += sizeof(uint);
                                }
                            }
                        }
                    }
                    deserializer.Current = read;
                    return true;
                }
                deserializer.State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else deserializer.State = AutoCSer.BinarySerialize.DeserializeStateEnum.MemberMapVerify;
            return false;
        }

        /// <summary>
        /// 获取成员位图索引
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetMemberIndex(string memberName)
        {
            return nameIndexSearcher.Search(memberName);
        }
        /// <summary>
        /// 名称索引查找数据
        /// </summary>
        private static readonly AutoCSer.StateSearcher.CharSearcher nameIndexSearcher;
        /// <summary>
        /// 成员数量
        /// </summary>
        internal static readonly int MemberCount;
        /// <summary>
        /// 字段成员数量
        /// </summary>
        private static readonly int fieldCount;
        /// <summary>
        /// 成员位图字节数量
        /// </summary>
        private static readonly int memberMapSize;
        /// <summary>
        /// 成员位图数组大小
        /// </summary>
        private static readonly int memberMapArraySize;
        static MemberMapData()
        {
            MemberIndexGroup memberIndexGroup = MemberIndexGroup.GetGroup(typeof(T));
            MemberIndexInfo[] members = memberIndexGroup.GetAllMembers();
            MemberCount = members.Length;
            fieldCount = memberIndexGroup.FieldCount;
            memberMapSize = ((MemberCount + 63) >> 6) << 3;
            memberMapArraySize = Math.Max((memberMapSize >> 3) - 1, 0);
            //BinarySerializeSize = ((fieldCount + 31) >> 5) << 2;
            nameIndexSearcher = new AutoCSer.StateSearcher.CharSearcher(AutoCSer.StateSearcher.CharBuilder.Create(members.getArray(value => value.Member.Name), true));
        }
    }
}
