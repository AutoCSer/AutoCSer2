﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer
{
    /// <summary>
    /// Random number
    /// 随机数
    /// </summary>
    public unsafe sealed class Random : IDisposable
    {
        /// <summary>
        /// Number of seed bytes
        /// 种子字节数量
        /// </summary>
        internal const int SecureSeedsSize = 64 * sizeof(uint) + 5 * 11 * sizeof(uint) + sizeof(uint);
        /// <summary>
        /// The default seed array of system random numbers
        /// 系统随机数默认种子数组
        /// </summary>
#if NetStandard21
        internal static readonly FieldInfo? SeedArrayField = typeof(System.Random).GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
#else
        internal static readonly FieldInfo SeedArrayField = typeof(System.Random).GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

        /// <summary>
        /// Public seeds
        /// 公用种子
        /// </summary>
        private uint* seeds;
        /// <summary>
        /// Safe seeds
        /// 安全种子
        /// </summary>
        private uint* secureSeeds;
        /// <summary>
        /// 32-bit seed position
        /// 32 位种子位置
        /// </summary>
        private int current;
        /// <summary>
        /// 64-bit seed position
        /// 64 位种子位置
        /// </summary>
        private int current64;
        /// <summary>
        /// 64-bit seed location access lock
        /// 64 位种子位置访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock currentLock;
        /// <summary>
        /// Random bit cache
        /// 随机位缓存
        /// </summary>
        private uint bits;
        /// <summary>
        /// Random bit cache quantity
        /// 随机位缓存数量
        /// </summary>
        private int bitCount;
        /// <summary>
        /// Byte cache access lock
        /// 字节缓存访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock byteLock;
        /// <summary>
        /// 字节缓存
        /// </summary>
        /// Byte cache
        private ulong bytes;
        /// <summary>
        /// Byte cache quantity
        /// 字节缓存数量
        /// </summary>
        private int byteCount;
        /// <summary>
        /// 双字节缓存访问锁
        /// </summary>
        /// Double-byte cache access lock
        private AutoCSer.Threading.SpinLock ushortLock;
        /// <summary>
        /// 双字节缓存
        /// </summary>
        /// Double-byte cache
        private ulong ushorts;
        /// <summary>
        /// The number of double-byte caches
        /// 双字节缓存数量
        /// </summary>
        private int ushortCount;
        /// <summary>
        /// Random number
        /// 随机数
        /// </summary>
        /// <param name="isAutoCSerStatic">Whether the random seed uses the default static memory
        /// 随机种子是否采用默认静态内存</param>
        private Random(bool isAutoCSerStatic = false)
        {
            secureSeeds = isAutoCSerStatic ? AutoCSer.Memory.Unmanaged.GetRandomSecureSeeds().UInt : (uint*)AutoCSer.Memory.Unmanaged.Get(SecureSeedsSize, false);
            seeds = secureSeeds + 64;
            current64 = 5 * 11 - 2;
            ulong tick = (ulong)AutoCSer.Date.StartTime.Ticks ^ (ulong)System.Diagnostics.Stopwatch.GetTimestamp() ^ (ulong)Environment.TickCount ^ ((ulong)AutoCSer.Threading.SecondTimer.TimerInterval << 24);
            int isSeedArray = 0;
            if (SeedArrayField != null)
            {
                var seedArray = SeedArrayField.GetValue(new System.Random()) as int[];
                if (seedArray?.Length == 5 * 11 + 1)
                {
                    tick *= 0xb163dUL;
                    fixed (int* seedFixed = seedArray)
                    {
                        for (uint* write = seeds, end = seeds + 5 * 11, read = (uint*)seedFixed; write != end; tick >>= 1)
                        {
                            *write++ = *++read ^ (((uint)tick & 1U) << 31);
                        }
                    }
                    isSeedArray = 1;
                }
            }
            if (isSeedArray == 0)
            {
                for (uint* start = seeds, end = start + 5 * 11; start != end; ++start)
                {
                    *start = (uint)tick ^ (uint)(tick >> 32);
                    tick *= 0xb163dUL;
                    tick += tick >> 32;
                }
            }
            for (ulong* start = (ulong*)secureSeeds; start != seeds; *start++ = NextULong()) ;
            bits = (uint)Next();
            bitCount = 32;
        }
        /// <summary>
        /// Release the seed memory
        /// 释放种子内存
        /// </summary>
        public void Dispose()
        {
            uint* secureSeeds = this.secureSeeds;
            if (secureSeeds != Default.secureSeeds)
            {
                this.secureSeeds = Default.secureSeeds;
                seeds = Default.seeds;
                AutoCSer.Memory.Unmanaged.Free(secureSeeds, SecureSeedsSize);
            }
        }
        /// <summary>
        /// Get the random seed position
        /// 获取随机种子位置
        /// </summary>
        /// <returns></returns>
        private int nextIndex()
        {
            int index = Interlocked.Increment(ref current);
            if (index >= 5 * 11)
            {
                int cacheIndex = index;
                do
                {
                    index -= 5 * 11;
                }
                while (index >= 5 * 11);
                Interlocked.CompareExchange(ref current, index, cacheIndex);
            }
            return index;
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public int Next()
        {
            int index = nextIndex();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7)) return (int)(*seed -= *(seed + 3 * 7));
            return (int)(*seed ^= *(seed - (5 * 11 - 3 * 7)));
        }
#if NET8
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Half NextHalf()
        {
            ushort value = NextUShort();
            return *(Half*)&value;
        }
#endif
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public float NextFloat()
        {
            int index = nextIndex();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7)) *seed -= *(seed + 3 * 7);
            else *seed ^= *(seed - (5 * 11 - 3 * 7));
            return *(float*)seed;
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        /// <param name="mod">Take the modulus of the remainder
        /// 求余取模数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int Next(int mod)
        {
            if (mod <= 1) return 0;
            int value = Next() % mod;
            return value >= 0 ? value : (value + mod);
        }
        /// <summary>
        /// Get the next random bit
        /// 获取下一个随机位
        /// </summary>
        /// <returns></returns>
        public uint NextBit()
        {
            int count = Interlocked.Decrement(ref bitCount);
            while (count < 0)
            {
                while (bitCount <= 0) AutoCSer.Threading.ThreadYield.Yield();
                count = Interlocked.Decrement(ref bitCount);
            }
            if (count == 0)
            {
                uint value = bits & 1;
                bits = (uint)Next();
                Interlocked.Exchange(ref bitCount, 32);
                return value;
            }
            return bits & (1U << count);
        }
        /// <summary>
        /// Get the next random byte
        /// 获取下一个随机字节
        /// </summary>
        /// <returns></returns>
        public byte NextByte()
        {
            do
            {
                byteLock.EnterYield();
                if (byteCount == 0)
                {
                    byteCount = -1;
                    byteLock.Exit();
                    byte value = (byte)(bytes = NextULong());
                    bytes >>= 8;
                    byteLock.EnterYield();
                    byteCount = 7;
                    byteLock.Exit();
                    return value;
                }
                if (byteCount > 0)
                {
                    byte value = (byte)bytes;
                    --byteCount;
                    bytes >>= 8;
                    byteLock.Exit();
                    return value;
                }
                byteLock.Exit();
                while (byteCount < 0) AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// Get the next random double byte
        /// 获取下一个随机双字节
        /// </summary>
        /// <returns></returns>
        public ushort NextUShort()
        {
            do
            {
                ushortLock.EnterYield();
                if (ushortCount == 0)
                {
                    ushortCount = -1;
                    ushortLock.Exit();
                    ushort value = (ushort)(ushorts = NextULong());
                    ushorts >>= 16;
                    ushortLock.EnterYield();
                    ushortCount = 3;
                    ushortLock.Exit();
                    return value;
                }
                if (ushortCount > 0)
                {
                    ushort value = (ushort)ushorts;
                    --ushortCount;
                    ushorts >>= 16;
                    ushortLock.Exit();
                    return value;
                }
                ushortLock.Exit();
                while (ushortCount < 0) AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// Get the random seed position
        /// 获取随机种子位置
        /// </summary>
        /// <returns></returns>
        private int nextIndex64()
        {
            currentLock.EnterYield();
            int index = current64;
            if ((current64 -= 2) < 0) current64 = (5 * 11 - 4) - current64;
            currentLock.Exit();
            return index;
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public ulong NextULong()
        {
            int index = nextIndex64();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7 - 1)) return *(ulong*)seed -= *(ulong*)(seed + 3 * 7);
            if (index == (5 * 11 - 3 * 7 - 1)) return *(ulong*)seed -= *(ulong*)seeds;
            return *(ulong*)seed ^= *(ulong*)(seed - (5 * 11 - 3 * 7));
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public double NextDouble()
        {
            int index = nextIndex64();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7 - 1)) *(ulong*)seed -= *(ulong*)(seed + 3 * 7);
            else if (index == (5 * 11 - 3 * 7 - 1)) *(ulong*)seed -= *(ulong*)seeds;
            else *(ulong*)seed ^= *(ulong*)(seed - (5 * 11 - 3 * 7));
            return *(double*)seed;
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public int SecureNext()
        {
            int seed = Next(), leftIndex = seed & 63, rightIndex = (seed >> 6) & 63;
            if (leftIndex == rightIndex) return (int)((secureSeeds[leftIndex] ^= (uint)seed) - (uint)seed);
            if ((seed & (1 << ((seed >> 12) & 31))) == 0) return (int)((secureSeeds[leftIndex] -= secureSeeds[rightIndex]) ^ (uint)seed);
            return (int)((secureSeeds[leftIndex] ^= secureSeeds[rightIndex]) - (uint)seed);
        }
        /// <summary>
        /// Get the next nonzero random number
        /// 获取下一个非 0 随机数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint SecureNextUIntNotZero()
        {
            uint value = (uint)SecureNext();
            while (value == 0) value = (uint)SecureNext();
            return value;
        }
        /// <summary>
        /// Get the next random number
        /// 获取下一个随机数
        /// </summary>
        public ulong SecureNextULong()
        {
            ulong seed = NextULong();
            int leftIndex = (int)(uint)seed & 63, rightIndex = (int)((uint)seed >> 6) & 63;
            if (leftIndex == 63) leftIndex = 62;
            if (rightIndex == 63) rightIndex = 62;
            if (leftIndex == rightIndex) return (*(ulong*)(secureSeeds + leftIndex) ^= seed) - seed;
            if (((uint)seed & (1U << ((int)((uint)seed >> 12) & 31))) == 0) return (*(ulong*)(secureSeeds + leftIndex) -= *(ulong*)(secureSeeds + rightIndex)) ^ seed;
            return (*(ulong*)(secureSeeds + leftIndex) ^= *(ulong*)(secureSeeds + rightIndex)) - seed;
        }
        /// <summary>
        /// Get the next nonzero random number
        /// 获取下一个非 0 随机数
        /// </summary>
        public ulong SecureNextULongNotZero()
        {
            ulong value = SecureNextULong();
            while (value == 0) value = SecureNextULong();
            return value;
        }
        /// <summary>
        /// Default random number
        /// 默认随机数
        /// </summary>
        public static readonly Random Default;
        /// <summary>
        /// Random Hash value (for anti-construction)
        /// 随机 Hash 值（用于防构造）
        /// </summary>
        public static readonly ulong Hash64;
        /// <summary>
        /// Random Hash value (for anti-construction)
        /// </summary>
        public static readonly int Hash;

        static Random()
        {
            Default = new Random(true);
            do
            {
                Hash64 = Default.NextULong();
            }
            while (Hash64 == 0);
            for (Hash = (int)((uint)Hash64 ^ (uint)(Hash64 >> 32)); Hash == 0; Hash = Default.Next()) ;
        }
    }
}
