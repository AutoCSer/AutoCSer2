using System;
using AutoCSer.TestCase.Data;

namespace AutoCSer.TestCase
{
    class Json
    {
        /// <summary>
        /// 随机对象生成参数
        /// </summary>
        internal static readonly AutoCSer.RandomObject.Config RandomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
        /// <summary>
        /// 带成员位图的JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig jsonSerializeConfig = new AutoCSer.JsonSerializeConfig();
        /// <summary>
        /// Javascript 时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig javascriptDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.Javascript, IsBoolToInt = true, IsIntegerToHex = true };
        /// <summary>
        /// SQL 时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig sqlDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.Sql };
        /// <summary>
        /// 第三方时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig thirdPartyDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.ThirdParty };
        /// <summary>
        /// 自定义格式时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig customDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeType.CustomFormat, DateTimeCustomFormat = "yyyy-MM-dd  HH:mm:ss" };

        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型字段成员JSON序列化测试
            Field filedData = AutoCSer.RandomObject.Creator<Field>.Create(RandomConfig);
            string jsonString = AutoCSer.JsonSerializer.Serialize(filedData);
            //AutoCSer.Log.Trace.Console(jsonString);
            Field newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Field>.Equals(filedData, newField))
            {
                return Program.Breakpoint();
            }
            //AutoCSer.Log.Trace.Console(AutoCSer.JsonSerializer.Serialize(newField));
            #endregion

            #region 带成员位图的引用类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Field>.NewFull();
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, jsonSerializeConfig);
            newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Field>.MemberMapEquals(filedData, newField, jsonSerializeConfig.MemberMap))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 引用类型属性成员JSON序列化测试
            Property propertyData = AutoCSer.RandomObject.Creator<Property>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(propertyData);
            Property newProperty = AutoCSer.JsonDeserializer.Deserialize<Property>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Property>.Equals(propertyData, newProperty))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 值类型字段成员JSON序列化测试
            StructField structField = AutoCSer.RandomObject.Creator<StructField>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structField);
            StructField newStructField = AutoCSer.JsonDeserializer.Deserialize<StructField>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<StructField>.Equals(structField, newStructField))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 带成员位图的值类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<StructField>.NewFull();
            jsonString = AutoCSer.JsonSerializer.Serialize(structField, jsonSerializeConfig);
            newStructField = AutoCSer.JsonDeserializer.Deserialize<StructField>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<StructField>.MemberMapEquals(structField, newStructField, jsonSerializeConfig.MemberMap))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 值类型属性成员JSON序列化测试
            StructProperty structProperty = AutoCSer.RandomObject.Creator<StructProperty>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structProperty);
            StructProperty newStructProperty = AutoCSer.JsonDeserializer.Deserialize<StructProperty>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<StructProperty>.Equals(structProperty, newStructProperty))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 16进制整数JSON序列化测试
            filedData = AutoCSer.RandomObject.Creator<Field>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, javascriptDateTimeSerializeConfig);
            newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Field>.Equals(filedData, newField))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 时间格式JSON序列化测试
            MemberClass memberClassData = AutoCSer.RandomObject.Creator<MemberClass>.Create(RandomConfig);
            memberClassData.DateTime = new DateTime(memberClassData.DateTime.Ticks, DateTimeKind.Local);
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData);
            MemberClass newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return Program.Breakpoint();
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, sqlDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return Program.Breakpoint();
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, thirdPartyDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return Program.Breakpoint();
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, customDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<MemberClass>.Equals(memberClassData, newMemberClassData))
            {
                return Program.Breakpoint();
            }
            #endregion

            if (AutoCSer.JsonDeserializer.Deserialize<int>(jsonString = AutoCSer.JsonSerializer.Serialize<int>(1)) != 1)
            {
                return Program.Breakpoint();
            }
            if (AutoCSer.JsonDeserializer.Deserialize<string>(jsonString = AutoCSer.JsonSerializer.Serialize<string>("1")) != "1")
            {
                return Program.Breakpoint();
            }

            Float floatData = AutoCSer.JsonDeserializer.Deserialize<Float>(@"{Double4:-4.0,Double2:2.0,Double6:-6.0,Double5:5.0,Double3:-3.0}");
            if (floatData.Double2 != 2 || floatData.Double3 != -3 || floatData.Double4 != -4 || floatData.Double5 != 5 || floatData.Double6 != -6)
            {
                return Program.Breakpoint();
            }

            floatData = new Float { FloatPositiveInfinity = float.NaN, FloatNegativeInfinity = float.NaN, DoublePositiveInfinity = double.NaN, DoubleNegativeInfinity = double.NaN };
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData);
            Float newFloatData = AutoCSer.JsonDeserializer.Deserialize<Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Float>.Equals(floatData, newFloatData))
            {
                return Program.Breakpoint();
            }

            floatData = new Float();
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData, new AutoCSer.JsonSerializeConfig { IsInfinityToNaN = false });
            newFloatData = AutoCSer.JsonDeserializer.Deserialize<Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Float>.Equals(floatData, newFloatData))
            {
                return Program.Breakpoint();
            }
            return true;
        }
    }
}
