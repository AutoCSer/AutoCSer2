using AutoCSer.Extensions;
using System;
using System.Linq;

namespace AutoCSer.TestCase
{
    internal class Sort
    {
        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            if (!intArray(getQuickSortArraySize())) return false;
            if (!uintArray(getQuickSortArraySize())) return false;
            if (!longArray(getQuickSortArraySize())) return false;
            if (!ulongArray(getQuickSortArraySize())) return false;

            if (!intArray(getRadixSortArraySize())) return false;
            if (!uintArray(getRadixSortArraySize())) return false;
            if (!longArray(getRadixSortArraySize())) return false;
            if (!ulongArray(getRadixSortArraySize())) return false;

            if (!intDataArray(getQuickSortArraySize())) return false;
            if (!uintDataArray(getQuickSortArraySize())) return false;
            if (!longDataArray(getQuickSortArraySize())) return false;
            if (!ulongDataArray(getQuickSortArraySize())) return false;

            if (!intDataArray(getRadixSortArraySize())) return false;
            if (!uintDataArray(getRadixSortArraySize())) return false;
            if (!longDataArray(getRadixSortArraySize())) return false;
            if (!ulongDataArray(getRadixSortArraySize())) return false;

            if (!intDataArrayDesc(1 << 10)) return false;
            if (!uintDataArrayDesc(1 << 10)) return false;
            if (!longDataArrayDesc(1 << 10)) return false;
            if (!ulongDataArrayDesc(1 << 10)) return false;
            return true;
        }
        private static int getQuickSortArraySize()
        {
            return (80 - 32) + AutoCSer.Random.Default.Next(32);//快排数组长度小于 80
        }
        private const int radixSortArraySize = 160;
        private static int getRadixSortArraySize()
        {
            return radixSortArraySize + AutoCSer.Random.Default.Next(64);//基数排序数组长度大于 160
        }
        private static bool intArray(int arraySize)
        {
            int[] array = new int[arraySize];
            for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.Next()) ;
            int minValue = array.Min(), maxValue = array.Max();
            array.AutoCSerExtensions().Sort();
            if (!checkArray(array, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.Next() & 0xffffff) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.Next() & 0xffff) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.Next() & 0xff) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool uintArray(int arraySize)
        {
            uint[] array = new uint[arraySize];
            for (int index = array.Length; index != 0; array[--index] = (uint)AutoCSer.Random.Default.Next()) ;
            uint minValue = array.Min(), maxValue = array.Max();
            array.AutoCSerExtensions().Sort();
            if (!checkArray(array, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = (uint)AutoCSer.Random.Default.Next() & 0xffffffU) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (uint)AutoCSer.Random.Default.Next() & 0xffffU) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (uint)AutoCSer.Random.Default.Next() & 0xffU) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool longArray(int arraySize)
        {
            long[] array = new long[arraySize];
            for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong()) ;
            long minValue = array.Min(), maxValue = array.Max();
            array.AutoCSerExtensions().Sort();
            if (!checkArray(array, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = (long)AutoCSer.Random.Default.NextULong() & 0xffL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool ulongArray(int arraySize)
        {
            ulong[] array = new ulong[arraySize];
            for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong()) ;
            ulong minValue = array.Min(), maxValue = array.Max();
            array.AutoCSerExtensions().Sort();
            if (!checkArray(array, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffffffffffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffffffffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffffffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = AutoCSer.Random.Default.NextULong() & 0xffUL) ;
                minValue = array.Min();
                maxValue = array.Max();
                array.AutoCSerExtensions().Sort();
                if (!checkArray(array, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool checkArray<T>(T[] array, T minValue, T maxValue) where T : IComparable<T>
        {
            if (array[0].CompareTo(minValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            if (array[array.Length - 1].CompareTo(maxValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            foreach (T value in array)
            {
                if (value.CompareTo(minValue) >= 0) minValue = value;
                else return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        private static bool intDataArray(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() }) ;
            Func<Data.Property, int> getKey = p => p.Int;
            int minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().Sort(getKey);
            if (!checkArray(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xffffff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xffff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool uintDataArray(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() }) ;
            Func<Data.Property, uint> getKey = p => p.UInt;
            uint minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().Sort(getKey);
            if (!checkArray(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffffffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool longDataArray(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() }) ;
            Func<Data.Property, long> getKey = p => p.Long;
            long minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().Sort(getKey);
            if (!checkArray(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool ulongDataArray(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() }) ;
            Func<Data.Property, ulong> getKey = p => p.ULong;
            ulong minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().Sort(getKey);
            if (!checkArray(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().Sort(getKey);
                if (!checkArray(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool checkArray<T, KT>(T[] array, Func<T, KT> getKey, KT minValue, KT maxValue) where KT : IComparable<KT>
        {
            if (getKey(array[0]).CompareTo(minValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            if (getKey(array[array.Length - 1]).CompareTo(maxValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            foreach (T value in array)
            {
                KT nextKey = getKey(value);
                if (nextKey.CompareTo(minValue) >= 0) minValue = nextKey;
                else return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        private static bool intDataArrayDesc(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() }) ;
            Func<Data.Property, int> getKey = p => p.Int;
            int minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().SortDesc(getKey);
            if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xffffff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xffff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Int = AutoCSer.Random.Default.Next() & 0xff }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool uintDataArrayDesc(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() }) ;
            Func<Data.Property, uint> getKey = p => p.UInt;
            uint minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().SortDesc(getKey);
            if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffffffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { UInt = (uint)AutoCSer.Random.Default.Next() & 0xffU }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool longDataArrayDesc(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() }) ;
            Func<Data.Property, long> getKey = p => p.Long;
            long minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().SortDesc(getKey);
            if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { Long = (long)AutoCSer.Random.Default.NextULong() & 0xffL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool ulongDataArrayDesc(int arraySize)
        {
            Data.Property[] array = new Data.Property[arraySize];
            for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() }) ;
            Func<Data.Property, ulong> getKey = p => p.ULong;
            ulong minValue = array.Min(getKey), maxValue = array.Max(getKey);
            array.AutoCSerExtensions().SortDesc(getKey);
            if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

            if (arraySize >= radixSortArraySize)
            {
                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;

                for (int index = array.Length; index != 0; array[--index] = new Data.Property { ULong = AutoCSer.Random.Default.NextULong() & 0xffUL }) ;
                minValue = array.Min(getKey);
                maxValue = array.Max(getKey);
                array.AutoCSerExtensions().SortDesc(getKey);
                if (!checkArrayDesc(array, getKey, minValue, maxValue)) return false;
            }
            return true;
        }
        private static bool checkArrayDesc<T, KT>(T[] array, Func<T, KT> getKey, KT minValue, KT maxValue) where KT : IComparable<KT>
        {
            if (getKey(array[0]).CompareTo(maxValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            if (getKey(array[array.Length - 1]).CompareTo(minValue) != 0) return AutoCSer.Breakpoint.ReturnFalse();
            foreach (T value in array)
            {
                KT nextKey = getKey(value);
                if (nextKey.CompareTo(maxValue) <= 0) maxValue = nextKey;
                else return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
