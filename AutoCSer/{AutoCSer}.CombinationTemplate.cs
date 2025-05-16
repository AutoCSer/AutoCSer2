//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, long[] destination)
        {
            fixed (long* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(long));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, uint[] destination)
        {
            fixed (uint* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(uint));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, int[] destination)
        {
            fixed (int* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(int));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, ushort[] destination)
        {
            fixed (ushort* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(ushort));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, short[] destination)
        {
            fixed (short* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(short));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, byte[] destination)
        {
            fixed (byte* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(byte));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, sbyte[] destination)
        {
            fixed (sbyte* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(sbyte));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, bool[] destination)
        {
            fixed (bool* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(bool));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, float[] destination)
        {
            fixed (float* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(float));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, double[] destination)
        {
            fixed (double* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(double));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, decimal[] destination)
        {
            fixed (decimal* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(decimal));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, char[] destination)
        {
            fixed (char* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(char));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, DateTime[] destination)
        {
            fixed (DateTime* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(DateTime));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, TimeSpan[] destination)
        {
            fixed (TimeSpan* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(TimeSpan));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Guid[] destination)
        {
            fixed (Guid* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Guid));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, UInt128[] destination)
        {
            fixed (UInt128* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(UInt128));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Int128[] destination)
        {
            fixed (Int128* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Int128));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Half[] destination)
        {
            fixed (Half* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Half));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Complex[] destination)
        {
            fixed (Complex* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Complex));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Plane[] destination)
        {
            fixed (Plane* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Plane));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Quaternion[] destination)
        {
            fixed (Quaternion* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Quaternion));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Matrix3x2[] destination)
        {
            fixed (Matrix3x2* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Matrix3x2));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Matrix4x4[] destination)
        {
            fixed (Matrix4x4* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Matrix4x4));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Vector2[] destination)
        {
            fixed (Vector2* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Vector2));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Vector3[] destination)
        {
            fixed (Vector3* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Vector3));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, Vector4[] destination)
        {
            fixed (Vector4* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(Vector4));
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<long, T> CreateLong<T>()
        {
#if AOT
            return new System.Collections.Generic.Dictionary<long, T>(AutoCSer.LongComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<long, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<long, T> CreateLong<T>(int capacity)
        {
#if AOT
            return new System.Collections.Generic.Dictionary<long, T>(capacity, AutoCSer.LongComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<long, T>(capacity);
#endif
        }
    }
#if AOT
    /// <summary>
    /// 字典关键字比较器
    /// </summary>
    public sealed class LongComparer : System.Collections.Generic.IEqualityComparer<long>
    {
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<long>.Equals(long left, long right)
        {
            return left == right;
        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<long>.GetHashCode(long value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 默认比较器
        /// </summary>
        public static readonly LongComparer Default = new LongComparer();
    }
#endif
}

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<uint, T> CreateUInt<T>()
        {
#if AOT
            return new System.Collections.Generic.Dictionary<uint, T>(AutoCSer.UIntComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<uint, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<uint, T> CreateUInt<T>(int capacity)
        {
#if AOT
            return new System.Collections.Generic.Dictionary<uint, T>(capacity, AutoCSer.UIntComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<uint, T>(capacity);
#endif
        }
    }
#if AOT
    /// <summary>
    /// 字典关键字比较器
    /// </summary>
    public sealed class UIntComparer : System.Collections.Generic.IEqualityComparer<uint>
    {
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<uint>.Equals(uint left, uint right)
        {
            return left == right;
        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<uint>.GetHashCode(uint value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 默认比较器
        /// </summary>
        public static readonly UIntComparer Default = new UIntComparer();
    }
#endif
}

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<int, T> CreateInt<T>()
        {
#if AOT
            return new System.Collections.Generic.Dictionary<int, T>(AutoCSer.IntComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<int, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<int, T> CreateInt<T>(int capacity)
        {
#if AOT
            return new System.Collections.Generic.Dictionary<int, T>(capacity, AutoCSer.IntComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<int, T>(capacity);
#endif
        }
    }
#if AOT
    /// <summary>
    /// 字典关键字比较器
    /// </summary>
    public sealed class IntComparer : System.Collections.Generic.IEqualityComparer<int>
    {
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<int>.Equals(int left, int right)
        {
            return left == right;
        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<int>.GetHashCode(int value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 默认比较器
        /// </summary>
        public static readonly IntComparer Default = new IntComparer();
    }
#endif
}

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<DateTime, T> CreateDateTime<T>()
        {
#if AOT
            return new System.Collections.Generic.Dictionary<DateTime, T>(AutoCSer.DateTimeComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<DateTime, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static System.Collections.Generic.Dictionary<DateTime, T> CreateDateTime<T>(int capacity)
        {
#if AOT
            return new System.Collections.Generic.Dictionary<DateTime, T>(capacity, AutoCSer.DateTimeComparer.Default);
#else
            return new System.Collections.Generic.Dictionary<DateTime, T>(capacity);
#endif
        }
    }
#if AOT
    /// <summary>
    /// 字典关键字比较器
    /// </summary>
    public sealed class DateTimeComparer : System.Collections.Generic.IEqualityComparer<DateTime>
    {
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<DateTime>.Equals(DateTime left, DateTime right)
        {
            return left == right;
        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<DateTime>.GetHashCode(DateTime value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 默认比较器
        /// </summary>
        public static readonly DateTimeComparer Default = new DateTimeComparer();
    }
#endif
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 指针快速排序
    /// </summary>
    internal static partial class QuickSort
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置 - sizeof(int)</param>
        internal unsafe static void SortInt(byte* startIndex, byte* endIndex)
        {
            do
            {
                System.Int64 distance = endIndex - startIndex;
                if (distance == sizeof(int))
                {
                    if (*(int*)endIndex < *(int*)startIndex)
                    {
                        int startValue = *(int*)startIndex;
                        *(int*)startIndex = *(int*)endIndex;
                        *(int*)endIndex = startValue;
                    }
                    break;
                }

                byte* averageIndex = startIndex + ((distance / (sizeof(int) * 2)) * sizeof(int));
                int value = *(int*)averageIndex, swapValue = *(int*)endIndex;
                if (value < *(int*)startIndex)
                {
                    if (swapValue < *(int*)startIndex)
                    {
                        *(int*)endIndex = *(int*)startIndex;
                        if (swapValue < value) *(int*)startIndex = swapValue;
                        else
                        {
                            *(int*)startIndex = value;
                            *(int*)averageIndex = value = swapValue;
                        }
                    }
                    else
                    {
                        *(int*)averageIndex = *(int*)startIndex;
                        *(int*)startIndex = value;
                        value = *(int*)averageIndex;
                    }
                }
                else if (*(int*)endIndex < value)
                {
                    *(int*)endIndex = value;
                    if (swapValue < *(int*)startIndex)
                    {
                        value = *(int*)startIndex;
                        *(int*)startIndex = swapValue;
                        *(int*)averageIndex = value;
                    }
                    else
                    {
                        *(int*)averageIndex = swapValue;
                        value = swapValue;
                    }
                }
                byte* leftIndex = startIndex + sizeof(int), rightIndex = endIndex - sizeof(int);
                do
                {
                    while (value > *(int*)leftIndex) leftIndex += sizeof(int);
                    while (*(int*)rightIndex > value) rightIndex -= sizeof(int);
                    if (leftIndex < rightIndex)
                    {
                        swapValue = *(int*)leftIndex;
                        *(int*)leftIndex = *(int*)rightIndex;
                        *(int*)rightIndex = swapValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            leftIndex += sizeof(int);
                            rightIndex -= sizeof(int);
                        }
                        break;
                    }
                }
                while ((leftIndex += sizeof(int)) <= (rightIndex -= sizeof(int)));
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) SortInt(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) SortInt(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(long value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(long));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(long*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(long);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(uint value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(uint));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(uint*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(uint);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(int value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(int));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(int*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(int);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(DateTime value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(DateTime));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(DateTime*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(DateTime);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(TimeSpan value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(TimeSpan));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(TimeSpan*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(TimeSpan);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(float value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(float));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(float*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(float);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(double value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(double));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(double*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(double);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(decimal value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(decimal));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(decimal*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(decimal);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(Guid value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(Guid));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(Guid*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(Guid);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(long value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(long));
#endif
            *(long*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(long);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(uint value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(uint));
#endif
            *(uint*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(uint);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(int value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int));
#endif
            *(int*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(int);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ushort value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(ushort));
#endif
            *(ushort*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(ushort);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(short value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(short));
#endif
            *(short*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(short);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(byte value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(byte));
#endif
            *(byte*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(byte);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(sbyte value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(sbyte));
#endif
            *(sbyte*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(sbyte);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(bool value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(bool));
#endif
            *(bool*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(bool);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(DateTime value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(DateTime));
#endif
            *(DateTime*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(DateTime);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(TimeSpan value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(TimeSpan));
#endif
            *(TimeSpan*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(TimeSpan);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(char value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(char));
#endif
            *(char*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(char);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(float value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(float));
#endif
            *(float*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(float);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(double value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(double));
#endif
            *(double*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(double);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(decimal value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(decimal));
#endif
            *(decimal*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(decimal);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Half value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Half));
#endif
            *(Half*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Half);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Int128 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Int128));
#endif
            *(Int128*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Int128);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(UInt128 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(UInt128));
#endif
            *(UInt128*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(UInt128);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Complex value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Complex));
#endif
            *(Complex*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Complex);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Plane value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Plane));
#endif
            *(Plane*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Plane);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Quaternion value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Quaternion));
#endif
            *(Quaternion*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Quaternion);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Matrix3x2 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Matrix3x2));
#endif
            *(Matrix3x2*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Matrix3x2);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Matrix4x4 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Matrix4x4));
#endif
            *(Matrix4x4*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Matrix4x4);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Vector2 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Vector2));
#endif
            *(Vector2*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Vector2);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Vector3 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Vector3));
#endif
            *(Vector3*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Vector3);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(Vector4 value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(Vector4));
#endif
            *(Vector4*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(Vector4);
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(long value)
        {
            if (PrepSize(sizeof(long)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(uint value)
        {
            if (PrepSize(sizeof(uint)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(int value)
        {
            if (PrepSize(sizeof(int)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(ushort value)
        {
            if (PrepSize(sizeof(ushort)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(short value)
        {
            if (PrepSize(sizeof(short)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(byte value)
        {
            if (PrepSize(sizeof(byte)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(sbyte value)
        {
            if (PrepSize(sizeof(sbyte)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(bool value)
        {
            if (PrepSize(sizeof(bool)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(DateTime value)
        {
            if (PrepSize(sizeof(DateTime)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(TimeSpan value)
        {
            if (PrepSize(sizeof(TimeSpan)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(char value)
        {
            if (PrepSize(sizeof(char)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(float value)
        {
            if (PrepSize(sizeof(float)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(double value)
        {
            if (PrepSize(sizeof(double)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(decimal value)
        {
            if (PrepSize(sizeof(decimal)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Half value)
        {
            if (PrepSize(sizeof(Half)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Int128 value)
        {
            if (PrepSize(sizeof(Int128)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(UInt128 value)
        {
            if (PrepSize(sizeof(UInt128)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Complex value)
        {
            if (PrepSize(sizeof(Complex)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Plane value)
        {
            if (PrepSize(sizeof(Plane)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Quaternion value)
        {
            if (PrepSize(sizeof(Quaternion)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Matrix3x2 value)
        {
            if (PrepSize(sizeof(Matrix3x2)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Matrix4x4 value)
        {
            if (PrepSize(sizeof(Matrix4x4)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Vector2 value)
        {
            if (PrepSize(sizeof(Vector2)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Vector3 value)
        {
            if (PrepSize(sizeof(Vector3)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否写入成功</returns>
        public bool Write(Vector4 value)
        {
            if (PrepSize(sizeof(Vector4)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumLong<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberSigned())
            {
                long intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumLong<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumLongDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsLong<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumLongDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumLong<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumLongDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsLong<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumLongDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsLongMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsLong<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumLongDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumLong(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumLong(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        long intValue = AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumUInt<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberUnsigned())
            {
                uint intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUInt<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUIntDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsUInt<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUIntDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumUInt<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUIntDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsUInt<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUIntDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsUIntMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsUInt<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUIntDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumUInt(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumUInt(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        uint intValue = AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumInt<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberSigned())
            {
                int intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumInt<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumIntDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsInt<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumIntDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumInt<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumIntDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsInt<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumIntDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsIntMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsInt<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumIntDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumInt(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumInt(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        int intValue = AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumUShort<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberUnsigned())
            {
                ushort intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUShort<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUShortDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsUShort<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUShortDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumUShort<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUShortDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsUShort<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumUShortDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsUShortMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsUShort<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUShortDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumUShort(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumUShort(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ushort intValue = AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumShort<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberSigned())
            {
                short intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumShort<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumShortDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsShort<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumShortDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumShort<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumShortDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsShort<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumShortDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsShortMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsShort<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumShortDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumShort(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumShort(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        short intValue = AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumByte<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberUnsigned())
            {
                byte intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumByte<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumByteDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsByte<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumByteDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumByte<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumByteDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsByte<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumByteDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsByteMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsByte<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumByteDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumByte(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumByte(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        byte intValue = AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumSByte<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberSigned())
            {
                sbyte intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumSByte<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumSByteDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsSByte<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumSByteDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumSByte<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumSByteDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsSByte<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumSByteDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteMethod;
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsSByteMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsSByte<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumSByteDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumSByte(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumSByte(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        sbyte intValue = AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref long[]? array)
#else
        public void JsonDeserialize(ref long[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref long[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref long[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref uint[]? array)
#else
        public void JsonDeserialize(ref uint[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref uint[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref uint[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref int[]? array)
#else
        public void JsonDeserialize(ref int[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref int[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref int[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref ushort[]? array)
#else
        public void JsonDeserialize(ref ushort[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ushort[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ushort[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref short[]? array)
#else
        public void JsonDeserialize(ref short[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref short[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref short[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref byte[]? array)
#else
        public void JsonDeserialize(ref byte[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref byte[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref byte[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref sbyte[]? array)
#else
        public void JsonDeserialize(ref sbyte[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref sbyte[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref sbyte[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref DateTime[]? array)
#else
        public void JsonDeserialize(ref DateTime[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref DateTime[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref DateTime[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref TimeSpan[]? array)
#else
        public void JsonDeserialize(ref TimeSpan[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref TimeSpan[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref TimeSpan[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref object? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref object value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Type? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Type value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref long value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref uint value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref int value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ushort value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref short value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref byte value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref sbyte value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref bool value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref float value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref double value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref decimal value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Guid value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref char value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref DateTime value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref TimeSpan value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref SubString value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref JsonNode value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref UInt128 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Int128 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Half value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Complex value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Complex value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Complex)
                {
                    value = *(Complex*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Complex)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotComplex;
                return;
            }
            SerializeComplex serializeComplex = default(SerializeComplex);
            AutoCSer.Json.TypeDeserializer<SerializeComplex>.DefaultDeserializer(this, ref serializeComplex);
            value = new ComplexUnion { SerializeValue = serializeComplex }.Complex;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Plane value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Plane value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Plane)
                {
                    value = *(Plane*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Plane)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotPlane;
                return;
            }
            SerializePlane serializePlane = default(SerializePlane);
            AutoCSer.Json.TypeDeserializer<SerializePlane>.DefaultDeserializer(this, ref serializePlane);
            value = new PlaneUnion { SerializeValue = serializePlane }.Plane;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Quaternion value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Quaternion value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Quaternion)
                {
                    value = *(Quaternion*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Quaternion)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotQuaternion;
                return;
            }
            SerializeQuaternion serializeQuaternion = default(SerializeQuaternion);
            AutoCSer.Json.TypeDeserializer<SerializeQuaternion>.DefaultDeserializer(this, ref serializeQuaternion);
            value = new QuaternionUnion { SerializeValue = serializeQuaternion }.Quaternion;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Matrix3x2 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Matrix3x2 value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Matrix3x2)
                {
                    value = *(Matrix3x2*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Matrix3x2)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotMatrix3x2;
                return;
            }
            SerializeMatrix3x2 serializeMatrix3x2 = default(SerializeMatrix3x2);
            AutoCSer.Json.TypeDeserializer<SerializeMatrix3x2>.DefaultDeserializer(this, ref serializeMatrix3x2);
            value = new Matrix3x2Union { SerializeValue = serializeMatrix3x2 }.Matrix3x2;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Matrix4x4 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Matrix4x4 value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Matrix4x4)
                {
                    value = *(Matrix4x4*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Matrix4x4)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotMatrix4x4;
                return;
            }
            SerializeMatrix4x4 serializeMatrix4x4 = default(SerializeMatrix4x4);
            AutoCSer.Json.TypeDeserializer<SerializeMatrix4x4>.DefaultDeserializer(this, ref serializeMatrix4x4);
            value = new Matrix4x4Union { SerializeValue = serializeMatrix4x4 }.Matrix4x4;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Vector3 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Vector3 value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector3)
                {
                    value = *(Vector3*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Vector3)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotVector3;
                return;
            }
            SerializeVector3 serializeVector3 = default(SerializeVector3);
            AutoCSer.Json.TypeDeserializer<SerializeVector3>.DefaultDeserializer(this, ref serializeVector3);
            value = new Vector3Union { SerializeValue = serializeVector3 }.Vector3;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Vector4 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Vector4 value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector4)
                {
                    value = *(Vector4*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Vector4)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotVector4;
                return;
            }
            SerializeVector4 serializeVector4 = default(SerializeVector4);
            AutoCSer.Json.TypeDeserializer<SerializeVector4>.DefaultDeserializer(this, ref serializeVector4);
            value = new Vector4Union { SerializeValue = serializeVector4 }.Vector4;
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref long? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref uint? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref int? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ushort? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref short? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref byte? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref sbyte? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref bool? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref float? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref double? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref decimal? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref char? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref DateTime? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref TimeSpan? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Guid? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(long[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (long value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (long value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, long[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(uint[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (uint value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (uint value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, uint[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(int[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (int value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (int value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, int[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(ushort[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (ushort value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (ushort value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ushort[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(short[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (short value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (short value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, short[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(byte[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (byte value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (byte value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, byte[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        public void JsonSerialize(sbyte[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (sbyte value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (sbyte value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, sbyte[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, long value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, uint value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, int value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ushort value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, short value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, byte value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, sbyte value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, bool value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, float value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, double value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, decimal value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, char value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, DateTime value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, TimeSpan value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, string value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, SubString value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Type value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, UInt128 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Int128 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Half value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumLong<T>(T value) where T : struct, IConvertible
#else
        internal void EnumLong<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumLong<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumLong(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumUInt<T>(T value) where T : struct, IConvertible
#else
        internal void EnumUInt<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUInt<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumUInt(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumInt<T>(T value) where T : struct, IConvertible
#else
        internal void EnumInt<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumInt<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumInt(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumUShort<T>(T value) where T : struct, IConvertible
#else
        internal void EnumUShort<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUShort<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumUShort(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumShort<T>(T value) where T : struct, IConvertible
#else
        internal void EnumShort<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumShort<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumShort(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumByte<T>(T value) where T : struct, IConvertible
#else
        internal void EnumByte<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumByte<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumByte(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumSByte<T>(T value) where T : struct, IConvertible
#else
        internal void EnumSByte<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumSByte<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumSByte(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteMethod;
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Complex value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Complex value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Complex));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Complex;
                    *(Complex*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeComplex>.Serialize(this, new ComplexUnion { Complex = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Plane value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Plane value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Plane));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Plane;
                    *(Plane*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializePlane>.Serialize(this, new PlaneUnion { Plane = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Quaternion value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Quaternion value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Quaternion));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Quaternion;
                    *(Quaternion*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeQuaternion>.Serialize(this, new QuaternionUnion { Quaternion = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Matrix3x2 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Matrix3x2 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Matrix3x2));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Matrix3x2;
                    *(Matrix3x2*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeMatrix3x2>.Serialize(this, new Matrix3x2Union { Matrix3x2 = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Matrix4x4 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Matrix4x4 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Matrix4x4));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Matrix4x4;
                    *(Matrix4x4*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeMatrix4x4>.Serialize(this, new Matrix4x4Union { Matrix4x4 = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Vector3 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Vector3 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Vector3));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector3;
                    *(Vector3*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeVector3>.Serialize(this, new Vector3Union { Vector3 = value }.SerializeValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Vector4 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Vector4 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Vector4));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector4;
                    *(Vector4*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeVector4>.Serialize(this, new Vector4Union { Vector4 = value }.SerializeValue);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, long? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(long? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, uint? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(uint? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, int? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(int? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ushort? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(ushort? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, short? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(short? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, byte? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(byte? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, sbyte? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(sbyte? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, bool? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(bool? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, float? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(float? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, double? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(double? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, decimal? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(decimal? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, char? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(char? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, DateTime? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(DateTime? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, TimeSpan? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(TimeSpan? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Guid? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(Guid? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref long value)
        {
            value = *(long*)Current;
            Current += sizeof(long);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref long value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref long? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(long*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(long);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableLong(BinaryDeserializer deserializer)
        {
            long? value = default(long?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref long? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out long value)
        {
            value = *(long*)Current;
            if ((Current += sizeof(long)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref uint value)
        {
            value = *(uint*)Current;
            Current += sizeof(uint);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref uint value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref uint? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(uint*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(uint);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableUInt(BinaryDeserializer deserializer)
        {
            uint? value = default(uint?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref uint? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out uint value)
        {
            value = *(uint*)Current;
            if ((Current += sizeof(uint)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref int value)
        {
            value = *(int*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref int value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref int? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(int*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(int);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableInt(BinaryDeserializer deserializer)
        {
            int? value = default(int?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref int? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out int value)
        {
            value = *(int*)Current;
            if ((Current += sizeof(int)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref float value)
        {
            value = *(float*)Current;
            Current += sizeof(float);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref float value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref float? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(float*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(float);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableFloat(BinaryDeserializer deserializer)
        {
            float? value = default(float?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref float? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out float value)
        {
            value = *(float*)Current;
            if ((Current += sizeof(float)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref double value)
        {
            value = *(double*)Current;
            Current += sizeof(double);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref double value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref double? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(double*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(double);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableDouble(BinaryDeserializer deserializer)
        {
            double? value = default(double?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref double? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out double value)
        {
            value = *(double*)Current;
            if ((Current += sizeof(double)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref decimal value)
        {
            value = *(decimal*)Current;
            Current += sizeof(decimal);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref decimal value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref decimal? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(decimal*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(decimal);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableDecimal(BinaryDeserializer deserializer)
        {
            decimal? value = default(decimal?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref decimal? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out decimal value)
        {
            value = *(decimal*)Current;
            if ((Current += sizeof(decimal)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref DateTime value)
        {
            value = *(DateTime*)Current;
            Current += sizeof(DateTime);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref DateTime value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref DateTime? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(DateTime*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(DateTime);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableDateTime(BinaryDeserializer deserializer)
        {
            DateTime? value = default(DateTime?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref DateTime? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out DateTime value)
        {
            value = *(DateTime*)Current;
            if ((Current += sizeof(DateTime)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref TimeSpan value)
        {
            value = *(TimeSpan*)Current;
            Current += sizeof(TimeSpan);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref TimeSpan value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref TimeSpan? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(TimeSpan*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(TimeSpan);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableTimeSpan(BinaryDeserializer deserializer)
        {
            TimeSpan? value = default(TimeSpan?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref TimeSpan? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out TimeSpan value)
        {
            value = *(TimeSpan*)Current;
            if ((Current += sizeof(TimeSpan)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Guid value)
        {
            value = *(Guid*)Current;
            Current += sizeof(Guid);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Guid value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref Guid? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(Guid*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(Guid);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableGuid(BinaryDeserializer deserializer)
        {
            Guid? value = default(Guid?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Guid? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Guid value)
        {
            value = *(Guid*)Current;
            if ((Current += sizeof(Guid)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Int128 value)
        {
            value = *(Int128*)Current;
            Current += sizeof(Int128);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Int128 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Int128 value)
        {
            value = *(Int128*)Current;
            if ((Current += sizeof(Int128)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Complex value)
        {
            value = *(Complex*)Current;
            Current += sizeof(Complex);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Complex value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Complex value)
        {
            value = *(Complex*)Current;
            if ((Current += sizeof(Complex)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Plane value)
        {
            value = *(Plane*)Current;
            Current += sizeof(Plane);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Plane value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Plane value)
        {
            value = *(Plane*)Current;
            if ((Current += sizeof(Plane)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Quaternion value)
        {
            value = *(Quaternion*)Current;
            Current += sizeof(Quaternion);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Quaternion value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Quaternion value)
        {
            value = *(Quaternion*)Current;
            if ((Current += sizeof(Quaternion)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Matrix3x2 value)
        {
            value = *(Matrix3x2*)Current;
            Current += sizeof(Matrix3x2);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix3x2 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Matrix3x2 value)
        {
            value = *(Matrix3x2*)Current;
            if ((Current += sizeof(Matrix3x2)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Matrix4x4 value)
        {
            value = *(Matrix4x4*)Current;
            Current += sizeof(Matrix4x4);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix4x4 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Matrix4x4 value)
        {
            value = *(Matrix4x4*)Current;
            if ((Current += sizeof(Matrix4x4)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Vector2 value)
        {
            value = *(Vector2*)Current;
            Current += sizeof(Vector2);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector2 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Vector2 value)
        {
            value = *(Vector2*)Current;
            if ((Current += sizeof(Vector2)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Vector3 value)
        {
            value = *(Vector3*)Current;
            Current += sizeof(Vector3);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector3 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Vector3 value)
        {
            value = *(Vector3*)Current;
            if ((Current += sizeof(Vector3)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Vector4 value)
        {
            value = *(Vector4*)Current;
            Current += sizeof(Vector4);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector4 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out Vector4 value)
        {
            value = *(Vector4*)Current;
            if ((Current += sizeof(Vector4)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref long[]? array)
#else
        public void BinaryDeserialize(ref long[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(long) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeLongArray(BinaryDeserializer deserializer)
        {
            var array = default(long[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeLongListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<long>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeLongLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<long>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableLongArray(BinaryDeserializer deserializer)
        {
            var array = default(long?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref long[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref long[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<long>? array)
#else
        public void BinaryDeserialize(ref ListArray<long> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(long) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<long>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<long> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<long> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(long) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<long> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref long?[]? array)
#else
        public void BinaryDeserialize(ref long?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(long*)Current;
                                Current += sizeof(long);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref long?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref long?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, long[]?> getBuffer, out long[]? buffer)
#else
        public int DeserializeBuffer(Func<int, long[]> getBuffer, out long[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(long) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (long* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(long));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<long>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref uint[]? array)
#else
        public void BinaryDeserialize(ref uint[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(uint) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeUIntArray(BinaryDeserializer deserializer)
        {
            var array = default(uint[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeUIntListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<uint>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeUIntLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<uint>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableUIntArray(BinaryDeserializer deserializer)
        {
            var array = default(uint?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref uint[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref uint[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<uint>? array)
#else
        public void BinaryDeserialize(ref ListArray<uint> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(uint) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<uint>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<uint> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<uint> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(uint) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<uint> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref uint?[]? array)
#else
        public void BinaryDeserialize(ref uint?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(uint*)Current;
                                Current += sizeof(uint);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref uint?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref uint?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, uint[]?> getBuffer, out uint[]? buffer)
#else
        public int DeserializeBuffer(Func<int, uint[]> getBuffer, out uint[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(uint) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (uint* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(uint));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<uint>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref int[]? array)
#else
        public void BinaryDeserialize(ref int[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(int) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeIntArray(BinaryDeserializer deserializer)
        {
            var array = default(int[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeIntListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<int>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeIntLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<int>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableIntArray(BinaryDeserializer deserializer)
        {
            var array = default(int?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref int[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref int[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<int>? array)
#else
        public void BinaryDeserialize(ref ListArray<int> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(int) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<int>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<int> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<int> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(int) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<int> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref int?[]? array)
#else
        public void BinaryDeserialize(ref int?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(int*)Current;
                                Current += sizeof(int);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref int?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref int?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, int[]?> getBuffer, out int[]? buffer)
#else
        public int DeserializeBuffer(Func<int, int[]> getBuffer, out int[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(int) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (int* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(int));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<int>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref float[]? array)
#else
        public void BinaryDeserialize(ref float[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(float) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeFloatArray(BinaryDeserializer deserializer)
        {
            var array = default(float[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeFloatListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<float>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeFloatLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<float>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableFloatArray(BinaryDeserializer deserializer)
        {
            var array = default(float?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref float[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref float[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<float>? array)
#else
        public void BinaryDeserialize(ref ListArray<float> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(float) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<float>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<float> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<float> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(float) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<float> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref float?[]? array)
#else
        public void BinaryDeserialize(ref float?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(float*)Current;
                                Current += sizeof(float);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref float?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref float?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, float[]?> getBuffer, out float[]? buffer)
#else
        public int DeserializeBuffer(Func<int, float[]> getBuffer, out float[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(float) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (float* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(float));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<float>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref double[]? array)
#else
        public void BinaryDeserialize(ref double[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(double) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDoubleArray(BinaryDeserializer deserializer)
        {
            var array = default(double[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDoubleListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<double>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDoubleLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<double>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableDoubleArray(BinaryDeserializer deserializer)
        {
            var array = default(double?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref double[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref double[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<double>? array)
#else
        public void BinaryDeserialize(ref ListArray<double> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(double) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<double>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<double> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<double> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(double) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<double> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref double?[]? array)
#else
        public void BinaryDeserialize(ref double?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(double*)Current;
                                Current += sizeof(double);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref double?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref double?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, double[]?> getBuffer, out double[]? buffer)
#else
        public int DeserializeBuffer(Func<int, double[]> getBuffer, out double[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(double) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (double* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(double));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<double>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref decimal[]? array)
#else
        public void BinaryDeserialize(ref decimal[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(decimal) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDecimalArray(BinaryDeserializer deserializer)
        {
            var array = default(decimal[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDecimalListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<decimal>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDecimalLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<decimal>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableDecimalArray(BinaryDeserializer deserializer)
        {
            var array = default(decimal?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref decimal[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref decimal[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<decimal>? array)
#else
        public void BinaryDeserialize(ref ListArray<decimal> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(decimal) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<decimal>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<decimal> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<decimal> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(decimal) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<decimal> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref decimal?[]? array)
#else
        public void BinaryDeserialize(ref decimal?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(decimal*)Current;
                                Current += sizeof(decimal);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref decimal?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref decimal?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, decimal[]?> getBuffer, out decimal[]? buffer)
#else
        public int DeserializeBuffer(Func<int, decimal[]> getBuffer, out decimal[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(decimal) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (decimal* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(decimal));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<decimal>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref DateTime[]? array)
#else
        public void BinaryDeserialize(ref DateTime[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(DateTime) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDateTimeArray(BinaryDeserializer deserializer)
        {
            var array = default(DateTime[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeDateTimeListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<DateTime>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDateTimeLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<DateTime>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableDateTimeArray(BinaryDeserializer deserializer)
        {
            var array = default(DateTime?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref DateTime[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref DateTime[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<DateTime>? array)
#else
        public void BinaryDeserialize(ref ListArray<DateTime> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(DateTime) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<DateTime>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<DateTime> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<DateTime> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(DateTime) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<DateTime> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref DateTime?[]? array)
#else
        public void BinaryDeserialize(ref DateTime?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(DateTime*)Current;
                                Current += sizeof(DateTime);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref DateTime?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref DateTime?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, DateTime[]?> getBuffer, out DateTime[]? buffer)
#else
        public int DeserializeBuffer(Func<int, DateTime[]> getBuffer, out DateTime[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(DateTime) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (DateTime* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(DateTime));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<DateTime>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref TimeSpan[]? array)
#else
        public void BinaryDeserialize(ref TimeSpan[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(TimeSpan) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeTimeSpanArray(BinaryDeserializer deserializer)
        {
            var array = default(TimeSpan[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeTimeSpanListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<TimeSpan>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeTimeSpanLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<TimeSpan>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableTimeSpanArray(BinaryDeserializer deserializer)
        {
            var array = default(TimeSpan?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref TimeSpan[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref TimeSpan[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<TimeSpan>? array)
#else
        public void BinaryDeserialize(ref ListArray<TimeSpan> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(TimeSpan) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<TimeSpan>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<TimeSpan> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<TimeSpan> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(TimeSpan) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<TimeSpan> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref TimeSpan?[]? array)
#else
        public void BinaryDeserialize(ref TimeSpan?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(TimeSpan*)Current;
                                Current += sizeof(TimeSpan);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref TimeSpan?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref TimeSpan?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, TimeSpan[]?> getBuffer, out TimeSpan[]? buffer)
#else
        public int DeserializeBuffer(Func<int, TimeSpan[]> getBuffer, out TimeSpan[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(TimeSpan) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (TimeSpan* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(TimeSpan));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<TimeSpan>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Guid[]? array)
#else
        public void BinaryDeserialize(ref Guid[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Guid) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeGuidArray(BinaryDeserializer deserializer)
        {
            var array = default(Guid[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeGuidListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<Guid>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeGuidLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<Guid>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableGuidArray(BinaryDeserializer deserializer)
        {
            var array = default(Guid?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Guid[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Guid[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<Guid>? array)
#else
        public void BinaryDeserialize(ref ListArray<Guid> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Guid) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<Guid>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<Guid> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<Guid> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(Guid) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<Guid> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Guid?[]? array)
#else
        public void BinaryDeserialize(ref Guid?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(Guid*)Current;
                                Current += sizeof(Guid);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Guid?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Guid?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Guid[]?> getBuffer, out Guid[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Guid[]> getBuffer, out Guid[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Guid) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Guid* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Guid));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Guid>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Int128[]? array)
#else
        public void BinaryDeserialize(ref Int128[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Int128) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeInt128Array(BinaryDeserializer deserializer)
        {
            var array = default(Int128[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Int128[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Int128[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Int128[]?> getBuffer, out Int128[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Int128[]> getBuffer, out Int128[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Int128) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Int128* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Int128));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Int128>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Complex[]? array)
#else
        public void BinaryDeserialize(ref Complex[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Complex) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeComplexArray(BinaryDeserializer deserializer)
        {
            var array = default(Complex[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Complex[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Complex[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Complex[]?> getBuffer, out Complex[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Complex[]> getBuffer, out Complex[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Complex) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Complex* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Complex));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Complex>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Plane[]? array)
#else
        public void BinaryDeserialize(ref Plane[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Plane) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializePlaneArray(BinaryDeserializer deserializer)
        {
            var array = default(Plane[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Plane[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Plane[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Plane[]?> getBuffer, out Plane[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Plane[]> getBuffer, out Plane[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Plane) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Plane* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Plane));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Plane>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Quaternion[]? array)
#else
        public void BinaryDeserialize(ref Quaternion[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Quaternion) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeQuaternionArray(BinaryDeserializer deserializer)
        {
            var array = default(Quaternion[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Quaternion[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Quaternion[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Quaternion[]?> getBuffer, out Quaternion[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Quaternion[]> getBuffer, out Quaternion[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Quaternion) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Quaternion* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Quaternion));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Quaternion>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Matrix3x2[]? array)
#else
        public void BinaryDeserialize(ref Matrix3x2[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Matrix3x2) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeMatrix3x2Array(BinaryDeserializer deserializer)
        {
            var array = default(Matrix3x2[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix3x2[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix3x2[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Matrix3x2[]?> getBuffer, out Matrix3x2[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Matrix3x2[]> getBuffer, out Matrix3x2[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Matrix3x2) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Matrix3x2* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Matrix3x2));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Matrix3x2>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Matrix4x4[]? array)
#else
        public void BinaryDeserialize(ref Matrix4x4[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Matrix4x4) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeMatrix4x4Array(BinaryDeserializer deserializer)
        {
            var array = default(Matrix4x4[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix4x4[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Matrix4x4[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Matrix4x4[]?> getBuffer, out Matrix4x4[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Matrix4x4[]> getBuffer, out Matrix4x4[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Matrix4x4) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Matrix4x4* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Matrix4x4));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Matrix4x4>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Vector2[]? array)
#else
        public void BinaryDeserialize(ref Vector2[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Vector2) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeVector2Array(BinaryDeserializer deserializer)
        {
            var array = default(Vector2[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector2[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector2[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Vector2[]?> getBuffer, out Vector2[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Vector2[]> getBuffer, out Vector2[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Vector2) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Vector2* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Vector2));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Vector2>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Vector3[]? array)
#else
        public void BinaryDeserialize(ref Vector3[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Vector3) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeVector3Array(BinaryDeserializer deserializer)
        {
            var array = default(Vector3[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector3[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector3[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Vector3[]?> getBuffer, out Vector3[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Vector3[]> getBuffer, out Vector3[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Vector3) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Vector3* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Vector3));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Vector3>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref Vector4[]? array)
#else
        public void BinaryDeserialize(ref Vector4[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(Vector4) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeVector4Array(BinaryDeserializer deserializer)
        {
            var array = default(Vector4[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector4[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Vector4[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Vector4[]?> getBuffer, out Vector4[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Vector4[]> getBuffer, out Vector4[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(Vector4) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Vector4* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Vector4));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Vector4>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long FixedEnumLong()
        {
            long value = *(long*)Current;
            Current += sizeof(long);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberLongReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(long);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumLongArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumLong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumLongListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumLong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumLongLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumLong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberLongReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumLongMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(*(long*)deserializer.Current);
#endif
            deserializer.Current += sizeof(long);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumLong<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumLong<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumLong<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(long), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(long)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(*(long*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumLongArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumLongArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumLongArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumLong(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumLong<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumLong<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumLong<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(long), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(long)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(*(long*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumLongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumLongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumLongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumLong(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumLong<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumLong<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(long), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(long)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(*(long*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumLongLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumLongLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumLong(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint FixedEnumUInt()
        {
            uint value = *(uint*)Current;
            Current += sizeof(uint);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberUIntReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(uint);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumUIntArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumUInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumUIntListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumUInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumUIntLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumUInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberUIntReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUIntMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(*(uint*)deserializer.Current);
#endif
            deserializer.Current += sizeof(uint);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUInt<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumUInt<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumUInt<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(uint), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(uint)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(*(uint*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUIntArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumUIntArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumUIntArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumUInt(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUInt<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumUInt<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumUInt<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(uint), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(uint)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(*(uint*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumUIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumUIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumUInt(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUInt<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumUInt<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(uint), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(uint)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(*(uint*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUIntLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumUIntLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumUInt(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int FixedEnumInt()
        {
            int value = *(int*)Current;
            Current += sizeof(int);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberIntReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(int);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumIntArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumIntListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumIntLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumInt(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberIntReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumIntMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(*(int*)deserializer.Current);
#endif
            deserializer.Current += sizeof(int);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumInt<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumInt<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumInt<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(int), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(int)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(*(int*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumIntArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumIntArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumIntArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumInt(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumInt<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumInt<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumInt<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(int), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(int)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(*(int*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumIntListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumInt(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumInt<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumInt<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(int), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(int)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(*(int*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumIntLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumIntLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumInt(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ushort FixedEnumUShort()
        {
            ushort value = *(ushort*)Current;
            Current += sizeof(ushort);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberUShortReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(ushort);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumUShortArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumUShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumUShortListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumUShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumUShortLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumUShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberUShortReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUShortMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)deserializer.Current);
#endif
            deserializer.Current += sizeof(ushort);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUShort<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumUShort<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumUShort<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ushort), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(ushort)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUShortArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumUShortArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumUShortArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumUShort(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUShort<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumUShort<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumUShort<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ushort), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(ushort)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumUShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumUShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumUShort(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumUShort<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumUShort<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ushort), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(ushort)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumUShortLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumUShortLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumUShort(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public short FixedEnumShort()
        {
            short value = *(short*)Current;
            Current += sizeof(short);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberShortReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(short);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumShortArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumShortListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumShortLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumShort(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberShortReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumShortMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(*(short*)deserializer.Current);
#endif
            deserializer.Current += sizeof(short);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumShort<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumShort<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumShort<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(short), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(short)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(*(short*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumShortArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumShortArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumShortArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumShort(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumShort<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumShort<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumShort<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(short), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(short)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(*(short*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumShortListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumShort(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumShort<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumShort<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(short), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(short)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(*(short*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumShortLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumShortLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumShort(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public byte FixedEnumByte()
        {
            byte value = *(byte*)Current;
            Current += sizeof(byte);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberByteReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(byte);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumByteArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumByteListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumByteLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberByteReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumByteMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*(byte*)deserializer.Current);
#endif
            deserializer.Current += sizeof(byte);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumByte<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumByte<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumByte<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(byte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(byte)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*(byte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumByteArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumByteArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumByteArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumByte(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumByte<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumByte<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumByte<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(byte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(byte)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*(byte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumByte(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumByte<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumByte<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(byte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(byte)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*(byte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumByteLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumByteLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumByte(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public sbyte FixedEnumSByte()
        {
            sbyte value = *(sbyte*)Current;
            Current += sizeof(sbyte);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberSByteReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(sbyte);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumSByteArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumSByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumSByteListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumSByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumSByteLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumSByte(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberSByteReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumSByteMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(*(sbyte*)deserializer.Current);
#endif
            deserializer.Current += sizeof(sbyte);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumSByte<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumSByte<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumSByte<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(sbyte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(sbyte)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(*(sbyte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumSByteArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumSByteArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumSByteArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumSByte(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumSByte<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumSByte<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumSByte<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(sbyte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(sbyte)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(*(sbyte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumSByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumSByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumSByteListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumSByte(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumSByte<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumSByte<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(sbyte), dataLength = AutoCSer.BinarySerializer.GetSize4(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(sbyte)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(*(sbyte*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumSByteLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumSByteLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumSByte(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        private void primitiveDeserialize(ref short? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(short*)Current;
            else value = null;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeShortArray(BinaryDeserializer deserializer)
        {
            var array = default(short[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeShortListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<short>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static object primitiveMemberDeserializeShortLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<short>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableShortArray(BinaryDeserializer deserializer)
        {
            var array = default(short?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">逻辑值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref short[]? array)
#else
        public void BinaryDeserialize(ref short[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(short) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<short>? array)
#else
        public void BinaryDeserialize(ref ListArray<short> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(short) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<short>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<short> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<short> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = ((long)length * sizeof(short) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<short> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref short?[]? array)
#else
        public void BinaryDeserialize(ref short?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        byte* start = Current;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(short*)Current;
                                Current += sizeof(short);
                            }
                        }
                        Current += (int)(start - Current) & 3;
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, short[]?> getBuffer, out short[]? buffer)
#else
        public int DeserializeBuffer(Func<int, short[]> getBuffer, out short[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(short) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (short* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(short));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<short>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        private void primitiveDeserialize(ref sbyte? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(sbyte*)Current;
            else value = null;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeSByteArray(BinaryDeserializer deserializer)
        {
            var array = default(sbyte[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeSByteListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<sbyte>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static object primitiveMemberDeserializeSByteLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<sbyte>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableSByteArray(BinaryDeserializer deserializer)
        {
            var array = default(sbyte?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">逻辑值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref sbyte[]? array)
#else
        public void BinaryDeserialize(ref sbyte[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(sbyte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<sbyte>? array)
#else
        public void BinaryDeserialize(ref ListArray<sbyte> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(sbyte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<sbyte>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<sbyte> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<sbyte> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = ((long)length * sizeof(sbyte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<sbyte> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref sbyte?[]? array)
#else
        public void BinaryDeserialize(ref sbyte?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        byte* start = Current;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(sbyte*)Current;
                                Current += sizeof(sbyte);
                            }
                        }
                        Current += (int)(start - Current) & 3;
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, sbyte[]?> getBuffer, out sbyte[]? buffer)
#else
        public int DeserializeBuffer(Func<int, sbyte[]> getBuffer, out sbyte[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(sbyte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (sbyte* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(sbyte));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<sbyte>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        private void primitiveDeserialize(ref byte? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(byte*)Current;
            else value = null;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeByteArray(BinaryDeserializer deserializer)
        {
            var array = default(byte[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeByteListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<byte>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static object primitiveMemberDeserializeByteLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<byte>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableByteArray(BinaryDeserializer deserializer)
        {
            var array = default(byte?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">逻辑值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref byte[]? array)
#else
        public void BinaryDeserialize(ref byte[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(byte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<byte>? array)
#else
        public void BinaryDeserialize(ref ListArray<byte> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(byte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<byte>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<byte> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<byte> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = ((long)length * sizeof(byte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<byte> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref byte?[]? array)
#else
        public void BinaryDeserialize(ref byte?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        byte* start = Current;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(byte*)Current;
                                Current += sizeof(byte);
                            }
                        }
                        Current += (int)(start - Current) & 3;
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, byte[]?> getBuffer, out byte[]? buffer)
#else
        public int DeserializeBuffer(Func<int, byte[]> getBuffer, out byte[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(byte) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (byte* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(byte));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<byte>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        private void primitiveDeserialize(ref char? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(char*)Current;
            else value = null;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeCharArray(BinaryDeserializer deserializer)
        {
            var array = default(char[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeCharListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<char>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static object primitiveMemberDeserializeCharLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<char>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableCharArray(BinaryDeserializer deserializer)
        {
            var array = default(char?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">逻辑值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref char[]? array)
#else
        public void BinaryDeserialize(ref char[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(char) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<char>? array)
#else
        public void BinaryDeserialize(ref ListArray<char> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(char) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<char>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<char> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<char> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = ((long)length * sizeof(char) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<char> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref char?[]? array)
#else
        public void BinaryDeserialize(ref char?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        byte* start = Current;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(char*)Current;
                                Current += sizeof(char);
                            }
                        }
                        Current += (int)(start - Current) & 3;
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, char[]?> getBuffer, out char[]? buffer)
#else
        public int DeserializeBuffer(Func<int, char[]> getBuffer, out char[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(char) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (char* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(char));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<char>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref long value)
        {
            value = *(long*)Current;
            Current += sizeof(long);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeLong(BinaryDeserializer deserializer)
        {
            long value = default(long);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref long value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref uint value)
        {
            value = *(uint*)Current;
            Current += sizeof(uint);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeUInt(BinaryDeserializer deserializer)
        {
            uint value = default(uint);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref uint value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref int value)
        {
            value = *(int*)Current;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeInt(BinaryDeserializer deserializer)
        {
            int value = default(int);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref int value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref ushort value)
        {
            value = *(ushort*)Current;
            Current += sizeof(ushort);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeUShort(BinaryDeserializer deserializer)
        {
            ushort value = default(ushort);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref ushort value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref short value)
        {
            value = *(short*)Current;
            Current += sizeof(short);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeShort(BinaryDeserializer deserializer)
        {
            short value = default(short);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref short value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref byte value)
        {
            value = *(byte*)Current;
            Current += sizeof(byte);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeByte(BinaryDeserializer deserializer)
        {
            byte value = default(byte);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref byte value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref sbyte value)
        {
            value = *(sbyte*)Current;
            Current += sizeof(sbyte);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeSByte(BinaryDeserializer deserializer)
        {
            sbyte value = default(sbyte);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref sbyte value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref bool value)
        {
            value = *(bool*)Current;
            Current += sizeof(bool);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeBool(BinaryDeserializer deserializer)
        {
            bool value = default(bool);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref bool value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref float value)
        {
            value = *(float*)Current;
            Current += sizeof(float);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeFloat(BinaryDeserializer deserializer)
        {
            float value = default(float);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref float value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref double value)
        {
            value = *(double*)Current;
            Current += sizeof(double);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDouble(BinaryDeserializer deserializer)
        {
            double value = default(double);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref double value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref decimal value)
        {
            value = *(decimal*)Current;
            Current += sizeof(decimal);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDecimal(BinaryDeserializer deserializer)
        {
            decimal value = default(decimal);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref decimal value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref char value)
        {
            value = *(char*)Current;
            Current += sizeof(char);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeChar(BinaryDeserializer deserializer)
        {
            char value = default(char);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref char value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref DateTime value)
        {
            value = *(DateTime*)Current;
            Current += sizeof(DateTime);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeDateTime(BinaryDeserializer deserializer)
        {
            DateTime value = default(DateTime);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref DateTime value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref TimeSpan value)
        {
            value = *(TimeSpan*)Current;
            Current += sizeof(TimeSpan);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeTimeSpan(BinaryDeserializer deserializer)
        {
            TimeSpan value = default(TimeSpan);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref TimeSpan value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Guid value)
        {
            value = *(Guid*)Current;
            Current += sizeof(Guid);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeGuid(BinaryDeserializer deserializer)
        {
            Guid value = default(Guid);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Guid value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Int128 value)
        {
            value = *(Int128*)Current;
            Current += sizeof(Int128);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeInt128(BinaryDeserializer deserializer)
        {
            Int128 value = default(Int128);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Int128 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Half value)
        {
            value = *(Half*)Current;
            Current += sizeof(Half);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeHalf(BinaryDeserializer deserializer)
        {
            Half value = default(Half);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Half value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Complex value)
        {
            value = *(Complex*)Current;
            Current += sizeof(Complex);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeComplex(BinaryDeserializer deserializer)
        {
            Complex value = default(Complex);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Complex value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Plane value)
        {
            value = *(Plane*)Current;
            Current += sizeof(Plane);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializePlane(BinaryDeserializer deserializer)
        {
            Plane value = default(Plane);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Plane value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Quaternion value)
        {
            value = *(Quaternion*)Current;
            Current += sizeof(Quaternion);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeQuaternion(BinaryDeserializer deserializer)
        {
            Quaternion value = default(Quaternion);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Quaternion value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Matrix3x2 value)
        {
            value = *(Matrix3x2*)Current;
            Current += sizeof(Matrix3x2);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeMatrix3x2(BinaryDeserializer deserializer)
        {
            Matrix3x2 value = default(Matrix3x2);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Matrix3x2 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Matrix4x4 value)
        {
            value = *(Matrix4x4*)Current;
            Current += sizeof(Matrix4x4);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeMatrix4x4(BinaryDeserializer deserializer)
        {
            Matrix4x4 value = default(Matrix4x4);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Matrix4x4 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Vector2 value)
        {
            value = *(Vector2*)Current;
            Current += sizeof(Vector2);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeVector2(BinaryDeserializer deserializer)
        {
            Vector2 value = default(Vector2);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Vector2 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Vector3 value)
        {
            value = *(Vector3*)Current;
            Current += sizeof(Vector3);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeVector3(BinaryDeserializer deserializer)
        {
            Vector3 value = default(Vector3);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Vector3 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref Vector4 value)
        {
            value = *(Vector4*)Current;
            Current += sizeof(Vector4);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeVector4(BinaryDeserializer deserializer)
        {
            Vector4 value = default(Vector4);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref Vector4 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, long value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(long? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(long) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeLong(BinarySerializer binarySerializer, object objectValue)
        {
            long? value = (long?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, long? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, uint value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(uint? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(uint) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeUInt(BinarySerializer binarySerializer, object objectValue)
        {
            uint? value = (uint?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, uint? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, int value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(int? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(int) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeInt(BinarySerializer binarySerializer, object objectValue)
        {
            int? value = (int?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, int? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, float value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(float? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(float) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeFloat(BinarySerializer binarySerializer, object objectValue)
        {
            float? value = (float?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, float? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, double value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(double? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(double) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeDouble(BinarySerializer binarySerializer, object objectValue)
        {
            double? value = (double?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, double? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, decimal value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(decimal? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(decimal) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeDecimal(BinarySerializer binarySerializer, object objectValue)
        {
            decimal? value = (decimal?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, decimal? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, DateTime value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(DateTime? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(DateTime) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeDateTime(BinarySerializer binarySerializer, object objectValue)
        {
            DateTime? value = (DateTime?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, DateTime? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, TimeSpan value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(TimeSpan? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(TimeSpan) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeTimeSpan(BinarySerializer binarySerializer, object objectValue)
        {
            TimeSpan? value = (TimeSpan?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, TimeSpan? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Int128 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Complex value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Plane value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Quaternion value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix3x2 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix4x4 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector2 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector3 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector4 value)
        {
            binarySerializer.Stream.Write(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(long[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (long* arrayFixed = array)
                {
                    int dataSize = count * sizeof(long);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(long?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(long));
                if (arrayMap.WriteIndex != -1)
                {
                    long* write = (long*)Stream.Data.Pointer.Current;
                    foreach (long? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (long)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(uint[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (uint* arrayFixed = array)
                {
                    int dataSize = count * sizeof(uint);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(uint?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(uint));
                if (arrayMap.WriteIndex != -1)
                {
                    uint* write = (uint*)Stream.Data.Pointer.Current;
                    foreach (uint? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (uint)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(int[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (int* arrayFixed = array)
                {
                    int dataSize = count * sizeof(int);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(int?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(int));
                if (arrayMap.WriteIndex != -1)
                {
                    int* write = (int*)Stream.Data.Pointer.Current;
                    foreach (int? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (int)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(float[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (float* arrayFixed = array)
                {
                    int dataSize = count * sizeof(float);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(float?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(float));
                if (arrayMap.WriteIndex != -1)
                {
                    float* write = (float*)Stream.Data.Pointer.Current;
                    foreach (float? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (float)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(double[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (double* arrayFixed = array)
                {
                    int dataSize = count * sizeof(double);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(double?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(double));
                if (arrayMap.WriteIndex != -1)
                {
                    double* write = (double*)Stream.Data.Pointer.Current;
                    foreach (double? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (double)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(decimal[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (decimal* arrayFixed = array)
                {
                    int dataSize = count * sizeof(decimal);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(decimal?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(decimal));
                if (arrayMap.WriteIndex != -1)
                {
                    decimal* write = (decimal*)Stream.Data.Pointer.Current;
                    foreach (decimal? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (decimal)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(DateTime[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (DateTime* arrayFixed = array)
                {
                    int dataSize = count * sizeof(DateTime);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(DateTime?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(DateTime));
                if (arrayMap.WriteIndex != -1)
                {
                    DateTime* write = (DateTime*)Stream.Data.Pointer.Current;
                    foreach (DateTime? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (DateTime)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(TimeSpan[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (TimeSpan* arrayFixed = array)
                {
                    int dataSize = count * sizeof(TimeSpan);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(TimeSpan?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(TimeSpan));
                if (arrayMap.WriteIndex != -1)
                {
                    TimeSpan* write = (TimeSpan*)Stream.Data.Pointer.Current;
                    foreach (TimeSpan? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (TimeSpan)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Guid[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Guid* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Guid);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(Guid?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(Guid));
                if (arrayMap.WriteIndex != -1)
                {
                    Guid* write = (Guid*)Stream.Data.Pointer.Current;
                    foreach (Guid? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (Guid)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Int128[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Int128* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Int128);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Complex[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Complex* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Complex);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Plane[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Plane* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Plane);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Quaternion[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Quaternion* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Quaternion);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Matrix3x2[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Matrix3x2* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Matrix3x2);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Matrix4x4[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Matrix4x4* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Matrix4x4);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Vector2[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Vector2* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Vector2);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Vector3[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Vector3* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Vector3);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Vector4[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Vector4* arrayFixed = array)
                {
                    int dataSize = count * sizeof(Vector4);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(long[]? array)
#else
        public void BinarySerialize(long[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, long[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, long[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<long>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<long> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<long>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<long> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<long> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeLongArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((long[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableLongArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((long?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeLongLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<long> array = (AutoCSer.LeftArray<long>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeLongListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<long>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<long> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(long?[]? array)
#else
        public void BinarySerialize(long?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, long?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, long?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(long[]? array)
#else
        public void SerializeBuffer(long[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(long[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(uint[]? array)
#else
        public void BinarySerialize(uint[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, uint[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, uint[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<uint>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<uint> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<uint>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<uint> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<uint> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeUIntArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((uint[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableUIntArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((uint?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeUIntLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<uint> array = (AutoCSer.LeftArray<uint>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeUIntListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<uint>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<uint> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(uint?[]? array)
#else
        public void BinarySerialize(uint?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, uint?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, uint?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(uint[]? array)
#else
        public void SerializeBuffer(uint[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(uint[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(int[]? array)
#else
        public void BinarySerialize(int[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, int[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, int[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<int>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<int> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<int>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<int> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<int> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeIntArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((int[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableIntArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((int?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeIntLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<int> array = (AutoCSer.LeftArray<int>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeIntListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<int>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<int> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(int?[]? array)
#else
        public void BinarySerialize(int?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, int?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, int?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(int[]? array)
#else
        public void SerializeBuffer(int[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(int[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(ushort[]? array)
#else
        public void BinarySerialize(ushort[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<ushort>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<ushort> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<ushort>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<ushort> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<ushort> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeUShortArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((ushort[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableUShortArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((ushort?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeUShortLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<ushort> array = (AutoCSer.LeftArray<ushort>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeUShortListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<ushort>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<ushort> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(ushort?[]? array)
#else
        public void BinarySerialize(ushort?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(ushort[]? array)
#else
        public void SerializeBuffer(ushort[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(ushort[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(short[]? array)
#else
        public void BinarySerialize(short[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, short[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, short[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<short>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<short> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<short>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<short> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<short> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeShortArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((short[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableShortArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((short?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeShortLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<short> array = (AutoCSer.LeftArray<short>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeShortListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<short>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<short> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(short?[]? array)
#else
        public void BinarySerialize(short?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, short?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, short?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(short[]? array)
#else
        public void SerializeBuffer(short[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(short[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(byte[]? array)
#else
        public void BinarySerialize(byte[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<byte>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<byte> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<byte>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<byte> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<byte> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeByteArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((byte[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableByteArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((byte?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeByteLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<byte> array = (AutoCSer.LeftArray<byte>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeByteListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<byte>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<byte> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(byte?[]? array)
#else
        public void BinarySerialize(byte?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(byte[]? array)
#else
        public void SerializeBuffer(byte[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(byte[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(sbyte[]? array)
#else
        public void BinarySerialize(sbyte[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<sbyte>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<sbyte> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<sbyte>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<sbyte> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<sbyte> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeSByteArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((sbyte[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableSByteArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((sbyte?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeSByteLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<sbyte> array = (AutoCSer.LeftArray<sbyte>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeSByteListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<sbyte>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<sbyte> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(sbyte?[]? array)
#else
        public void BinarySerialize(sbyte?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(sbyte[]? array)
#else
        public void SerializeBuffer(sbyte[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(sbyte[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(bool[]? array)
#else
        public void BinarySerialize(bool[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<bool>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<bool> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<bool>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<bool> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<bool> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeBoolArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((bool[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableBoolArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((bool?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeBoolLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<bool> array = (AutoCSer.LeftArray<bool>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeBoolListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<bool>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<bool> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(bool?[]? array)
#else
        public void BinarySerialize(bool?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(bool[]? array)
#else
        public void SerializeBuffer(bool[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(bool[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(float[]? array)
#else
        public void BinarySerialize(float[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, float[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, float[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<float>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<float> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<float>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<float> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<float> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeFloatArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((float[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableFloatArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((float?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeFloatLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<float> array = (AutoCSer.LeftArray<float>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeFloatListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<float>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<float> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(float?[]? array)
#else
        public void BinarySerialize(float?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, float?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, float?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(float[]? array)
#else
        public void SerializeBuffer(float[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(float[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(double[]? array)
#else
        public void BinarySerialize(double[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, double[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, double[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<double>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<double> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<double>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<double> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<double> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDoubleArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((double[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableDoubleArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((double?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeDoubleLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<double> array = (AutoCSer.LeftArray<double>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDoubleListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<double>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<double> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(double?[]? array)
#else
        public void BinarySerialize(double?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, double?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, double?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(double[]? array)
#else
        public void SerializeBuffer(double[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(double[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(decimal[]? array)
#else
        public void BinarySerialize(decimal[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, decimal[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, decimal[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<decimal>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<decimal> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<decimal>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<decimal> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<decimal> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDecimalArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((decimal[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableDecimalArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((decimal?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeDecimalLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<decimal> array = (AutoCSer.LeftArray<decimal>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDecimalListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<decimal>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<decimal> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(decimal?[]? array)
#else
        public void BinarySerialize(decimal?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, decimal?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, decimal?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(decimal[]? array)
#else
        public void SerializeBuffer(decimal[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(decimal[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(char[]? array)
#else
        public void BinarySerialize(char[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, char[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, char[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<char>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<char> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<char>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<char> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<char> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeCharArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((char[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableCharArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((char?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeCharLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<char> array = (AutoCSer.LeftArray<char>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeCharListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<char>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<char> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(char?[]? array)
#else
        public void BinarySerialize(char?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, char?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, char?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(char[]? array)
#else
        public void SerializeBuffer(char[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(char[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(DateTime[]? array)
#else
        public void BinarySerialize(DateTime[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, DateTime[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, DateTime[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<DateTime>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<DateTime> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<DateTime>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<DateTime> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<DateTime> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDateTimeArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((DateTime[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableDateTimeArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((DateTime?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeDateTimeLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<DateTime> array = (AutoCSer.LeftArray<DateTime>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeDateTimeListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<DateTime>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<DateTime> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(DateTime?[]? array)
#else
        public void BinarySerialize(DateTime?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, DateTime?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, DateTime?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(DateTime[]? array)
#else
        public void SerializeBuffer(DateTime[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(DateTime[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(TimeSpan[]? array)
#else
        public void BinarySerialize(TimeSpan[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, TimeSpan[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, TimeSpan[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<TimeSpan>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<TimeSpan> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<TimeSpan>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<TimeSpan> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<TimeSpan> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeTimeSpanArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((TimeSpan[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableTimeSpanArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((TimeSpan?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeTimeSpanLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<TimeSpan> array = (AutoCSer.LeftArray<TimeSpan>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeTimeSpanListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<TimeSpan>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<TimeSpan> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(TimeSpan?[]? array)
#else
        public void BinarySerialize(TimeSpan?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, TimeSpan?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, TimeSpan?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(TimeSpan[]? array)
#else
        public void SerializeBuffer(TimeSpan[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(TimeSpan[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Guid[]? array)
#else
        public void BinarySerialize(Guid[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Guid[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Guid[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<Guid>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<Guid> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<Guid>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<Guid> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<Guid> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeGuidArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Guid[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableGuidArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Guid?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeGuidLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<Guid> array = (AutoCSer.LeftArray<Guid>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeGuidListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<Guid>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<Guid> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Guid?[]? array)
#else
        public void BinarySerialize(Guid?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Guid?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Guid?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Guid[]? array)
#else
        public void SerializeBuffer(Guid[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Guid[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Int128[]? array)
#else
        public void BinarySerialize(Int128[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Int128[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Int128[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeInt128Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Int128[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Int128[]? array)
#else
        public void SerializeBuffer(Int128[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Int128[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Half[]? array)
#else
        public void BinarySerialize(Half[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Half[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Half[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeHalfArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Half[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Half[]? array)
#else
        public void SerializeBuffer(Half[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Half[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Complex[]? array)
#else
        public void BinarySerialize(Complex[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Complex[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Complex[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeComplexArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Complex[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Complex[]? array)
#else
        public void SerializeBuffer(Complex[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Complex[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Plane[]? array)
#else
        public void BinarySerialize(Plane[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Plane[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Plane[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializePlaneArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Plane[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Plane[]? array)
#else
        public void SerializeBuffer(Plane[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Plane[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Quaternion[]? array)
#else
        public void BinarySerialize(Quaternion[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Quaternion[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Quaternion[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeQuaternionArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Quaternion[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Quaternion[]? array)
#else
        public void SerializeBuffer(Quaternion[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Quaternion[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Matrix3x2[]? array)
#else
        public void BinarySerialize(Matrix3x2[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix3x2[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix3x2[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeMatrix3x2Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Matrix3x2[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Matrix3x2[]? array)
#else
        public void SerializeBuffer(Matrix3x2[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Matrix3x2[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Matrix4x4[]? array)
#else
        public void BinarySerialize(Matrix4x4[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix4x4[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Matrix4x4[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeMatrix4x4Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Matrix4x4[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Matrix4x4[]? array)
#else
        public void SerializeBuffer(Matrix4x4[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Matrix4x4[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Vector2[]? array)
#else
        public void BinarySerialize(Vector2[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector2[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector2[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeVector2Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Vector2[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Vector2[]? array)
#else
        public void SerializeBuffer(Vector2[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Vector2[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Vector3[]? array)
#else
        public void BinarySerialize(Vector3[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector3[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector3[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeVector3Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Vector3[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Vector3[]? array)
#else
        public void SerializeBuffer(Vector3[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Vector3[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(Vector4[]? array)
#else
        public void BinarySerialize(Vector4[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector4[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Vector4[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeVector4Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((Vector4[]?)array);
        }
#endif
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(Vector4[]? array)
#else
        public void SerializeBuffer(Vector4[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(Vector4[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumLong<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumLongArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumLong<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumLongArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumLongArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumLong((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumLongListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumLongArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumLongLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumLongArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberLongReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumLongArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(long);
                byte* write = Stream.GetBeforeMove(GetSize(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(long*)write = AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value);
                        if ((write += sizeof(long)) == end) break;
                    }
                    FillSize(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumLong<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumLong<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumLongArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumLongArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumLongArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumLongArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumLong(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumLongArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumLongArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumLongArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumLongArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumLongArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumLongListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumLongArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumLongLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumLongArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUInt<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumUIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUInt<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumUIntArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumUIntArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumUInt((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumUIntListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumUIntArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumUIntLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumUIntArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberUIntReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumUIntArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(uint);
                byte* write = Stream.GetBeforeMove(GetSize(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(uint*)write = AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value);
                        if ((write += sizeof(uint)) == end) break;
                    }
                    FillSize(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumUInt<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumUInt<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumUIntArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumUIntArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumUIntArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUIntArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumUInt(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumUIntArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumUIntArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumUIntArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumUIntArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumUIntArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUIntListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumUIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUIntLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumUIntArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumInt<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumInt<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumIntArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumIntArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumInt((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumIntListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumIntArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumIntLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumIntArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberIntReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumIntArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(int);
                byte* write = Stream.GetBeforeMove(GetSize(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(int*)write = AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value);
                        if ((write += sizeof(int)) == end) break;
                    }
                    FillSize(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumInt<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumInt<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumIntArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumIntArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumIntArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumIntArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumInt(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumIntArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumIntArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumIntArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumIntArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumIntArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumIntListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumIntLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumIntArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUShort<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumUShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumUShort<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumUShortArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumUShortArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumUShort((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumUShortListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumUShortArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumUShortLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumUShortArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberUShortReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumUShortArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(ushort);
                byte* write = Stream.GetBeforeMove(GetSize4(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize2(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(ushort*)write = AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value);
                        if ((write += sizeof(ushort)) == end) break;
                    }
                    FillSize2(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumUShort<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumUShort<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumUShortArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumUShortArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumUShortArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUShortArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumUShort(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumUShortArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumUShortArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumUShortArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumUShortArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumUShortArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUShortListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumUShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUShortLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumUShortArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumShort<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumShort<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumShortArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumShortArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumShort((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumShortListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumShortArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumShortLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumShortArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberShortReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumShortArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(short);
                byte* write = Stream.GetBeforeMove(GetSize4(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize2(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(short*)write = AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value);
                        if ((write += sizeof(short)) == end) break;
                    }
                    FillSize2(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumShort<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumShort<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumShortArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumShortArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumShortArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumShortArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumShort(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumShortArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumShortArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumShortArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumShortArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumShortArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumShortListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumShortLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumShortArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumByte<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumByte<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumByteArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumByteArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumByte((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumByteListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumByteArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumByteLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumByteArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberByteReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumByteArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(byte);
                byte* write = Stream.GetBeforeMove(GetSize4(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize4(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(byte*)write = AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value);
                        if ((write += sizeof(byte)) == end) break;
                    }
                    FillSize4(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumByte<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumByte<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumByteArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumByteArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumByteArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumByteArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumByte(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumByteArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumByteArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumByteArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumByteArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumByteArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumByteListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumByteLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumByteArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumSByte<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumSByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumSByte<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumSByteArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumSByteArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumSByte((T[]?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        public static void EnumSByteListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumSByteArray((ListArray<T>?)array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举数组序列化</param>
        public static void EnumSByteLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumSByteArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteListArrayMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteLeftArrayMethod;
        /// <summary>
        /// 枚举序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberSByteReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        /// <param name="count"></param>
        private unsafe void enumSByteArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(sbyte);
                byte* write = Stream.GetBeforeMove(GetSize4(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize4(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(sbyte*)write = AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value);
                        if ((write += sizeof(sbyte)) == end) break;
                    }
                    FillSize4(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        public void EnumSByte<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumSByte<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumSByteArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumSByteArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumSByteArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumSByteArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumSByte(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
#if NetStandard21
        private void enumSByteArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumSByteArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumSByteArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumSByteArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumSByteArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumSByteListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumSByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumSByteLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumSByteArrayOnly(array.Array, array.Length);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(short[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (short* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(short));
            }
        }
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(short?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, (array.Length * sizeof(short) + 3) & (int.MaxValue - 3));
                if (arrayMap.WriteIndex != -1)
                {
                    short* write = (short*)Stream.Data.Pointer.Current;
                    foreach (short? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (short)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SerializeFillByteSize(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(sbyte[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (sbyte* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(sbyte));
            }
        }
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(sbyte?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, (array.Length * sizeof(sbyte) + 3) & (int.MaxValue - 3));
                if (arrayMap.WriteIndex != -1)
                {
                    sbyte* write = (sbyte*)Stream.Data.Pointer.Current;
                    foreach (sbyte? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (sbyte)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SerializeFillByteSize(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(byte[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (byte* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(byte));
            }
        }
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(byte?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, (array.Length * sizeof(byte) + 3) & (int.MaxValue - 3));
                if (arrayMap.WriteIndex != -1)
                {
                    byte* write = (byte*)Stream.Data.Pointer.Current;
                    foreach (byte? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (byte)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SerializeFillByteSize(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(char[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (char* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(char));
            }
        }
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(char?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, (array.Length * sizeof(char) + 3) & (int.MaxValue - 3));
                if (arrayMap.WriteIndex != -1)
                {
                    char* write = (char*)Stream.Data.Pointer.Current;
                    foreach (char? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (char)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SerializeFillByteSize(write);
                }
            }
            else Stream.Write(0);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(long value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberLongReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((long)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, long value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(uint value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberUIntReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((uint)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, uint value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(int value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberIntReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((int)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, int value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(ushort value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberUShortReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((ushort)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, ushort value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(short value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberShortReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((short)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, short value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(byte value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberByteReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((byte)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, byte value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(sbyte value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberSByteReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((sbyte)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, sbyte value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(bool value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberBoolReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((bool)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, bool value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(float value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberFloatReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((float)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, float value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(double value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberDoubleReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((double)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, double value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(decimal value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberDecimalReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((decimal)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, decimal value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(char value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberCharReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((char)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, char value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(DateTime value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberDateTimeReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((DateTime)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, DateTime value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(TimeSpan value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberTimeSpanReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((TimeSpan)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, TimeSpan value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Int128 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberInt128Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Int128)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Int128 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Half value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberHalfReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Half)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Half value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Complex value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberComplexReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Complex)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Complex value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Plane value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberPlaneReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Plane)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Plane value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Quaternion value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberQuaternionReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Quaternion)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Quaternion value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Matrix3x2 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberMatrix3x2Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Matrix3x2)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Matrix3x2 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Matrix4x4 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberMatrix4x4Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Matrix4x4)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Matrix4x4 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Vector2 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberVector2Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Vector2)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Vector2 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Vector3 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberVector3Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Vector3)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Vector3 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Vector4 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberVector4Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((Vector4)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, Vector4 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref long? value)
        {
            if (*(int*)data == 0)
            {
                value = *(long*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(long));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref uint? value)
        {
            if (*(int*)data == 0)
            {
                value = *(uint*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(uint));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref int? value)
        {
            if (*(int*)data == 0)
            {
                value = *(int*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(int));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref float? value)
        {
            if (*(int*)data == 0)
            {
                value = *(float*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(float));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref double? value)
        {
            if (*(int*)data == 0)
            {
                value = *(double*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(double));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref decimal? value)
        {
            if (*(int*)data == 0)
            {
                value = *(decimal*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(decimal));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref DateTime? value)
        {
            if (*(int*)data == 0)
            {
                value = *(DateTime*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(DateTime));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref TimeSpan? value)
        {
            if (*(int*)data == 0)
            {
                value = *(TimeSpan*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(TimeSpan));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref Guid? value)
        {
            if (*(int*)data == 0)
            {
                value = *(Guid*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(Guid));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref long value)
        {
            value = *(long*)data;
            return data + sizeof(long);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref uint value)
        {
            value = *(uint*)data;
            return data + sizeof(uint);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref int value)
        {
            value = *(int*)data;
            return data + sizeof(int);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref ushort value)
        {
            value = *(ushort*)data;
            return data + sizeof(ushort);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref short value)
        {
            value = *(short*)data;
            return data + sizeof(short);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref byte value)
        {
            value = *(byte*)data;
            return data + sizeof(byte);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref sbyte value)
        {
            value = *(sbyte*)data;
            return data + sizeof(sbyte);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref bool value)
        {
            value = *(bool*)data;
            return data + sizeof(bool);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref float value)
        {
            value = *(float*)data;
            return data + sizeof(float);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref double value)
        {
            value = *(double*)data;
            return data + sizeof(double);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref decimal value)
        {
            value = *(decimal*)data;
            return data + sizeof(decimal);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref char value)
        {
            value = *(char*)data;
            return data + sizeof(char);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref DateTime value)
        {
            value = *(DateTime*)data;
            return data + sizeof(DateTime);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref TimeSpan value)
        {
            value = *(TimeSpan*)data;
            return data + sizeof(TimeSpan);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Guid value)
        {
            value = *(Guid*)data;
            return data + sizeof(Guid);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Half value)
        {
            value = *(Half*)data;
            return data + sizeof(Half);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Int128 value)
        {
            value = *(Int128*)data;
            return data + sizeof(Int128);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref UInt128 value)
        {
            value = *(UInt128*)data;
            return data + sizeof(UInt128);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Complex value)
        {
            value = *(Complex*)data;
            return data + sizeof(Complex);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Plane value)
        {
            value = *(Plane*)data;
            return data + sizeof(Plane);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Quaternion value)
        {
            value = *(Quaternion*)data;
            return data + sizeof(Quaternion);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Matrix3x2 value)
        {
            value = *(Matrix3x2*)data;
            return data + sizeof(Matrix3x2);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Matrix4x4 value)
        {
            value = *(Matrix4x4*)data;
            return data + sizeof(Matrix4x4);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Vector2 value)
        {
            value = *(Vector2*)data;
            return data + sizeof(Vector2);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Vector3 value)
        {
            value = *(Vector3*)data;
            return data + sizeof(Vector3);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref Vector4 value)
        {
            value = *(Vector4*)data;
            return data + sizeof(Vector4);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumLong<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(*(long*)data);
#endif
            return data + sizeof(long);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumUInt<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(*(uint*)data);
#endif
            return data + sizeof(uint);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumInt<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(*(int*)data);
#endif
            return data + sizeof(int);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumUShort<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)data);
#endif
            return data + sizeof(ushort);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumShort<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(*(short*)data);
#endif
            return data + sizeof(short);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumByte<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*(byte*)data);
#endif
            return data + sizeof(byte);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumSByte<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(*(sbyte*)data);
#endif
            return data + sizeof(sbyte);
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, long? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, long? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(long) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, uint? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, uint? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(uint) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, int? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, int? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(int) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, float? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, float? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(float) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, double? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, double? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(double) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, decimal? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, decimal? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(decimal) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, DateTime? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, DateTime? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(DateTime) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, TimeSpan? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, TimeSpan? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(TimeSpan) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Guid? value)
        {
            if (value.HasValue) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else stream.Data.Pointer.Write(AutoCSer.BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Guid? value)
        {
            if (value.HasValue)
            {
                if (stream.PrepSize(sizeof(Guid) + sizeof(int))) stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else stream.Write(AutoCSer.BinarySerializer.NullValue);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, long value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, long value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, uint value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, uint value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, int value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, int value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, ushort value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, ushort value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, short value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, short value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, byte value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, byte value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, sbyte value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, sbyte value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, bool value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, bool value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, float value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, float value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, double value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, double value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, decimal value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, decimal value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, char value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, char value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, DateTime value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, DateTime value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, TimeSpan value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, TimeSpan value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Half value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Half value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Int128 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Int128 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, UInt128 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, UInt128 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Complex value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Complex value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Plane value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Plane value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Quaternion value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Quaternion value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Matrix3x2 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Matrix3x2 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Matrix4x4 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Matrix4x4 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Vector2 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Vector2 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Vector3 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Vector3 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, Vector4 value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, Vector4 value)
        {
            stream.Write(value);
        }
#endif
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumLong<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUInt<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumInt<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUShort<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumShort<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumByte<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value));
        }
    }
}

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumSByte<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value));
        }
    }
}

#endif