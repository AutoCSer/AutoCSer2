using System;
using System.Diagnostics;
using AutoCSer.Extensions;

namespace AutoCSer.TestCase.SerializePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 100 * 10000;
            bool isJson = true, isJsonThreadStatic = true, isXml = true, isXmlThreadStatic = true, isBinary = true, isBinaryJson = true;
            AutoCSer.RandomObject.Config randomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
            AutoCSer.BinarySerializeConfig PropertySerializeConfig = new BinarySerializeConfig { };
            int sortTotalCount = 1000 * 10000, sortArraySize = 10 * 10000;
            int[] intArray = new int[sortArraySize];
            uint[] uintArray = new uint[sortArraySize];
            long[] longArray = new long[sortArraySize];
            ulong[] ulongArray = new ulong[sortArraySize];
            PropertyData[] dataArray = new PropertyData[sortArraySize];
            do
            {
                PropertyData propertyData = AutoCSer.RandomObject.Creator<PropertyData>.CreateNotNull(randomConfig);
                if (isJson) json(propertyData, count);
                if (isJsonThreadStatic) jsonThreadStatic(propertyData, count);
                if (isXml) xml(propertyData, count);
                if (isXmlThreadStatic) xmlThreadStatic(propertyData, count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatPropertyData floatPropertyData = AutoCSer.RandomObject.Creator<FloatPropertyData>.CreateNotNull(randomConfig);
                if (isJson) json(floatPropertyData, count);
                if (isJsonThreadStatic) jsonThreadStatic(floatPropertyData, count);
                if (isXml) xml(floatPropertyData, count);
                if (isXmlThreadStatic) xmlThreadStatic(floatPropertyData, count);

                FieldData filedData = AutoCSer.RandomObject.Creator<FieldData>.CreateNotNull(randomConfig);
                if (isJson) json(filedData, count);
                if (isJsonThreadStatic) jsonThreadStatic(filedData, count);
                if (isXml) xml(filedData, count);
                if (isXmlThreadStatic) xmlThreadStatic(filedData, count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatFieldData floatFiledData = AutoCSer.RandomObject.Creator<FloatFieldData>.CreateNotNull(randomConfig);
                if (isJson) json(floatFiledData, count);
                if (isJsonThreadStatic) jsonThreadStatic(floatFiledData, count);
                if (isXml) xml(floatFiledData, count);
                if (isXmlThreadStatic) xmlThreadStatic(floatFiledData, count);

                //浮点数对二进制序列化无影响
                if (isBinary)
                {
                    binary(floatFiledData, count);
                    binarThreadStatic(floatFiledData, count);

                    floatPropertyData = AutoCSer.RandomObject.Creator<FloatPropertyData>.CreateNotNull();
                    binary(floatPropertyData, count);
                    binarThreadStatic(floatPropertyData, count);
                }

                if (isBinaryJson)
                {
                    JsonFloatPropertyData jsonFloatPropertyData = AutoCSer.RandomObject.Creator<JsonFloatPropertyData>.CreateNotNull();
                    binary(jsonFloatPropertyData, count);
                    binarThreadStatic(jsonFloatPropertyData, count);

                    JsonFloatFieldData jsonFloatFieldData = AutoCSer.RandomObject.Creator<JsonFloatFieldData>.CreateNotNull();
                    binary(jsonFloatFieldData, count);
                    binarThreadStatic(jsonFloatFieldData, count);
                }


                for (int index = 0; index != sortArraySize; ++index)
                {
                    ulong value = AutoCSer.Random.Default.NextULong();
                    dataArray[index] = new PropertyData { Int = (int)value, UInt = (uint)value, Long = (long)value, ULong = value };
                    intArray[index] = (int)value;
                    uintArray[index] = (uint)value;
                    longArray[index] = (long)value;
                    ulongArray[index] = value;
                }

                //单纯的 int[] 整数排序在数据量较小（比如数组长度小于 100）的情况下 System.Array.Sort 存在一些性能优势
                //在计算使用的数据缓冲区大小不超过 CPU 末级缓存的场景下，数据量越大 AutoSer 排序性能优势越明显，最高可到 7 倍性能差距；数据量超过极限值以后性能差距会逐步缩小直到 1 到 2 倍
                sort(intArray, sortTotalCount / sortArraySize);
                sort(uintArray, sortTotalCount / sortArraySize);
                sort(longArray, sortTotalCount / sortArraySize);
                sort(ulongArray, sortTotalCount / sortArraySize);
                //在计算使用的数据缓冲区大小不超过 CPU 末级缓存的场景下，数据量越大 AutoSer 排序性能优势越明显，最高可超过 5 倍性能差距；数据量超过极限值以后性能差距会逐步缩小直到 1 倍
                sort(dataArray, sortTotalCount / sortArraySize);

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
#if AOT
                AutoCSer.TestCase.SerializePerformance.AotMethod.Call();
#endif
            }
            while (true);
        }
        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void json<T>(T value, int count, string title = null, bool isOutput = false)
        {
//            Console.WriteLine(title ?? @"
//http://www.AutoCSer.com/Serialize/Json.html");
            string json = string.Empty;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index)
            {
                json = AutoCSer.JsonSerializer.Serialize(value);
            }
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Json Serialize " + ((json.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            if (isOutput) Console.WriteLine(json);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.Deserialize<T>(json);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Json Deserialize " + ((json.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }
        /// <summary>
        /// JSON 序列化测试（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void jsonThreadStatic<T>(T value, int count, string title = null, bool isOutput = false)
        {
//            Console.WriteLine(title ?? @"
//http://www.AutoCSer.com/Serialize/Json.html");
            string json = string.Empty;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index)
            {
                json = AutoCSer.JsonSerializer.ThreadStaticSerialize(value);
            }
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Json Serialize ThreadStatic " + ((json.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            if (isOutput) Console.WriteLine(json);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.ThreadStaticDeserialize<T>(json);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Json Deserialize ThreadStatic " + ((json.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private static void binary<T>(T value, int count)
        {
//            Console.WriteLine(@"
//http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerializer.Serialize(value);
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Binary Serialize " + ((bytes.Length * count) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.Deserialize<T>(bytes);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Binary Deserialize " + ((bytes.Length * count) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private static void binarThreadStatic<T>(T value, int count)
        {
//            Console.WriteLine(@"
//http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerializer.ThreadStaticSerialize(value);
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Binary Serialize ThreadStatic " + ((bytes.Length * count) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.ThreadStaticDeserialize<T>(bytes);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Binary Deserialize ThreadStatic " + ((bytes.Length * count) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }
        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void xml<T>(T value, int count, string title = null, bool isOutput = false)
        {
//            Console.WriteLine(title ?? @"
//http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = string.Empty;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index)
            {
                xml = AutoCSer.XmlSerializer.Serialize(value);
            }
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Xml Serialize " + ((xml.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            if (isOutput) Console.WriteLine(xml);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.XmlDeserializer.Deserialize<T>(xml);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Xml Deserialize " + ((xml.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }
        /// <summary>
        /// XML 序列化测试（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void xmlThreadStatic<T>(T value, int count, string title = null, bool isOutput = false)
        {
//            Console.WriteLine(title ?? @"
//http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = string.Empty;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index)
            {
                xml = AutoCSer.XmlSerializer.ThreadStaticSerialize(value);
            }
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Xml Serialize ThreadStatic " + ((xml.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");

            if (isOutput) Console.WriteLine(xml);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.XmlDeserializer.ThreadStaticDeserialize<T>(xml);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).AutoCSerExtensions().ToString() + "W " + typeof(T).Name + " Xml Deserialize ThreadStatic " + ((xml.Length * count * 2) >> 20).AutoCSerExtensions().ToString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).AutoCSerExtensions().ToString() + "ms");
        }

        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <param name="values"></param>
        private static void sort(int[] values, int count)
        {
            long timestampSum = 0, startTimestamp;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[int] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort();
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[int] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");
        }
        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <param name="values"></param>
        private static void sort(uint[] values, int count)
        {
            long timestampSum = 0, startTimestamp;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[uint] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort();
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[uint] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");
        }
        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <param name="values"></param>
        private static void sort(long[] values, int count)
        {
            long timestampSum = 0, startTimestamp;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[long] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort();
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[long] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");
        }
        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <param name="values"></param>
        private static void sort(ulong[] values, int count)
        {
            long timestampSum = 0, startTimestamp;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[ulong] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort();
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[ulong] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");
        }
        /// <summary>
        /// 数组排序测试
        /// </summary>
        /// <param name="values"></param>
        private static void sort(PropertyData[] values, int count)
        {
            long timestampSum = 0, startTimestamp;
            var intComparison = (Comparison<PropertyData>)sortInt;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values, intComparison);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[class.int] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            Func<PropertyData, int> getIntKey = getInt;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().QuickSort(getIntKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.QuickSort[class.int] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort(getIntKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[class.int] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");


            timestampSum = 0;
            var uintComparison = (Comparison<PropertyData>)sortUInt;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values, uintComparison);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[class.uint] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            Func<PropertyData, uint> getUIntKey = getUInt;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().QuickSort(getUIntKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.QuickSort[class.uint] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort(getUIntKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[class.uint] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");


            timestampSum = 0;
            var longComparison = (Comparison<PropertyData>)sortLong;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values, longComparison);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[class.long] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            Func<PropertyData, long> getLongKey = getLong;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().QuickSort(getLongKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.QuickSort[class.long] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort(getLongKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[class.long] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");


            timestampSum = 0;
            var ulongComparison = (Comparison<PropertyData>)sortULong;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                Array.Sort(values, ulongComparison);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W System.Array.Sort[class.ulong] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            Func<PropertyData, ulong> getULongKey = getULong;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().QuickSort(getULongKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.QuickSort[class.ulong] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");

            timestampSum = 0;
            for (int index = 0; index != count; ++index)
            {
                values.AutoCSerExtensions().RandomSort();
                startTimestamp = Stopwatch.GetTimestamp();
                values.AutoCSerExtensions().Sort(getULongKey);
                timestampSum += Stopwatch.GetTimestamp() - startTimestamp;
            }
            Console.WriteLine(values.Length.ToString() + "*" + count.ToString() + "=" + ((values.Length * count) / 10000).AutoCSerExtensions().ToString() + "W AutoCSer.Sort[class.ulong] " + AutoCSer.Date.GetMillisecondsByTimestamp(timestampSum).AutoCSerExtensions().ToString() + "ms ");
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sortInt(PropertyData left, PropertyData right)
        {
            return left.Int.CompareTo(right.Int);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sortUInt(PropertyData left, PropertyData right)
        {
            return left.UInt.CompareTo(right.UInt);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sortLong(PropertyData left, PropertyData right)
        {
            return left.Long.CompareTo(right.Long);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sortULong(PropertyData left, PropertyData right)
        {
            return left.ULong.CompareTo(right.ULong);
        }
        /// <summary>
        /// 获取排序关键字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static int getInt(PropertyData data)
        {
            return data.Int;
        }
        /// <summary>
        /// 获取排序关键字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static uint getUInt(PropertyData data)
        {
            return data.UInt;
        }
        /// <summary>
        /// 获取排序关键字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static long getLong(PropertyData data)
        {
            return data.Long;
        }
        /// <summary>
        /// 获取排序关键字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static ulong getULong(PropertyData data)
        {
            return data.ULong;
        }
    }
}
