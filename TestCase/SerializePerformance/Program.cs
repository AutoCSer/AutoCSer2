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
            bool isJson = true, isJsonThreadStatic = true, isXml = true, isBinary = true;
            AutoCSer.RandomObject.Config randomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
            do
            {
                //AOT（NoJIT）模式应该尽量使用属性而非字段
                PropertyData propertyData = AutoCSer.RandomObject.Creator<PropertyData>.CreateNotNull(randomConfig);
                if (isJson) json(propertyData, count);
                if (isJsonThreadStatic) jsonThreadStatic(propertyData, count);
                //if (isXml) xml(propertyData, count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatPropertyData floatPropertyData = AutoCSer.RandomObject.Creator<FloatPropertyData>.CreateNotNull(randomConfig);
                if (isJson) json(floatPropertyData, count);
                if (isJsonThreadStatic) jsonThreadStatic(floatPropertyData, count);
                //if (isXml) xml(floatPropertyData, count);

                FieldData filedData = AutoCSer.RandomObject.Creator<FieldData>.CreateNotNull(randomConfig);
                if (isJson) json(filedData, count);
                if (isJsonThreadStatic) jsonThreadStatic(filedData, count);
                //if (isXml) xml(filedData, count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatFieldData floatFiledData = AutoCSer.RandomObject.Creator<FloatFieldData>.CreateNotNull(randomConfig);
                if (isJson) json(floatFiledData, count);
                if (isJsonThreadStatic) jsonThreadStatic(floatFiledData, count);
                //if (isXml) xml(floatFiledData, count);

                //AOT（NoJIT）模式尽量不要使用二进制序列化
                //浮点数对二进制序列化无影响
                if (isBinary)
                {
                    binary(floatFiledData, count);
                    binarThreadStatic(floatFiledData, count);
                }

                Console.WriteLine(@"Sleep 3000ms
");
                System.Threading.Thread.Sleep(3000);
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
            Console.WriteLine(title ?? @"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = string.Empty;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index)
            {
                json = AutoCSer.JsonSerializer.Serialize(value);
            }
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Serialize " + ((json.Length * count * 2) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.Deserialize<T>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Deserialize " + ((json.Length * count * 2) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");
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
            Console.WriteLine(title ?? @"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = string.Empty;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index)
            {
                json = AutoCSer.JsonSerializer.ThreadStaticSerialize(value);
            }
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Serialize ThreadStatic " + ((json.Length * count * 2) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.ThreadStaticDeserialize<T>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Deserialize ThreadStatic " + ((json.Length * count * 2) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private static void binary<T>(T value, int count)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Serialize " + ((bytes.Length * count) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.Deserialize<T>(bytes);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Deserialize " + ((bytes.Length * count) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private static void binarThreadStatic<T>(T value, int count)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerializer.ThreadStaticSerialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Serialize ThreadStatic " + ((bytes.Length * count) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.ThreadStaticDeserialize<T>(bytes);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Deserialize ThreadStatic " + ((bytes.Length * count) >> 20).toString() + "MB " + time.ElapsedMilliseconds.toString() + "ms");
        }
    }
}
