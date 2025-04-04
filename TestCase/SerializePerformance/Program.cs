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
            do
            {
                //AOT（NoJIT）模式应该尽量使用属性而非字段
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

                //AOT（NoJIT）模式尽量不要使用二进制序列化
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
//            Console.WriteLine(title ?? @"
//http://www.AutoCSer.com/Serialize/Json.html");
            string json = string.Empty;
            long startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index)
            {
                json = AutoCSer.JsonSerializer.Serialize(value);
            }
            long endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Serialize " + ((json.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.Deserialize<T>(json);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Deserialize " + ((json.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Serialize ThreadStatic " + ((json.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.JsonDeserializer.ThreadStaticDeserialize<T>(json);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Json Deserialize ThreadStatic " + ((json.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Serialize " + ((bytes.Length * count) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.Deserialize<T>(bytes);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Deserialize " + ((bytes.Length * count) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Serialize ThreadStatic " + ((bytes.Length * count) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.BinaryDeserializer.ThreadStaticDeserialize<T>(bytes);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Binary Deserialize ThreadStatic " + ((bytes.Length * count) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Xml Serialize " + ((xml.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            if (isOutput) Console.WriteLine(xml);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.XmlDeserializer.Deserialize<T>(xml);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Xml Deserialize " + ((xml.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Xml Serialize ThreadStatic " + ((xml.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");

            if (isOutput) Console.WriteLine(xml);

            startTimestamp = Stopwatch.GetTimestamp();
            for (int index = count; index != 0; --index) value = AutoCSer.XmlDeserializer.ThreadStaticDeserialize<T>(xml);
            endTimestamp = Stopwatch.GetTimestamp();
            Console.WriteLine((count / 10000).toString() + "W " + typeof(T).Name + " Xml Deserialize ThreadStatic " + ((xml.Length * count * 2) >> 20).toString() + "MB " + AutoCSer.Date.GetMillisecondsByTimestamp(endTimestamp - startTimestamp).toString() + "ms");
        }
    }
}
