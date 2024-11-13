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
        /// JavaScript 时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig javascriptDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeTypeEnum.JavaScript, IsBoolToInt = true, IsIntegerToHex = true };
        ///// <summary>
        ///// SQL 时间值JSON序列化参数配置
        ///// </summary>
        //private static readonly AutoCSer.JsonSerializeConfig sqlDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeTypeEnum.Sql };
        /// <summary>
        /// 第三方时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig thirdPartyDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeTypeEnum.ThirdParty };
        /// <summary>
        /// 自定义格式时间值JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.JsonSerializeConfig customDateTimeSerializeConfig = new AutoCSer.JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeTypeEnum.CustomFormat, DateTimeCustomFormat = "yyyy-MM-dd  HH:mm:ss" };

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
            Field newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(filedData, newField))
            {
                return false;
            }
            #endregion

            #region 带成员位图的引用类型字段成员JSON序列化测试
            AutoCSer.Metadata.MemberMap<Field> fieldMemberMap = AutoCSer.Metadata.MemberMap<Field>.NewFull();
            jsonSerializeConfig.MemberMap = fieldMemberMap;
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, jsonSerializeConfig);
            newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(filedData, newField, fieldMemberMap))
            {
                return false;
            }
            #endregion

            #region 引用类型属性成员JSON序列化测试
            Property propertyData = AutoCSer.RandomObject.Creator<Property>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(propertyData);
            Property newProperty = AutoCSer.JsonDeserializer.Deserialize<Property>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(propertyData, newProperty))
            {
                return false;
            }
            #endregion

            #region 值类型字段成员JSON序列化测试
            StructField structField = AutoCSer.RandomObject.Creator<StructField>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structField);
            StructField newStructField = AutoCSer.JsonDeserializer.Deserialize<StructField>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structField, newStructField))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型字段成员JSON序列化测试
            AutoCSer.Metadata.MemberMap<StructField> structFieldMemberMap = AutoCSer.Metadata.MemberMap<StructField>.NewFull();
            jsonSerializeConfig.MemberMap = structFieldMemberMap;
            jsonString = AutoCSer.JsonSerializer.Serialize(structField, jsonSerializeConfig);
            newStructField = AutoCSer.JsonDeserializer.Deserialize<StructField>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structField, newStructField, structFieldMemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型属性成员JSON序列化测试
            StructProperty structProperty = AutoCSer.RandomObject.Creator<StructProperty>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(structProperty);
            StructProperty newStructProperty = AutoCSer.JsonDeserializer.Deserialize<StructProperty>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structProperty, newStructProperty))
            {
                return false;
            }
            #endregion

            #region 16进制整数JSON序列化测试
            filedData = AutoCSer.RandomObject.Creator<Field>.Create(RandomConfig);
            jsonString = AutoCSer.JsonSerializer.Serialize(filedData, javascriptDateTimeSerializeConfig);
            newField = AutoCSer.JsonDeserializer.Deserialize<Field>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(filedData, newField))
            {
                return false;
            }
            #endregion

            #region 时间格式JSON序列化测试
            MemberClass memberClassData = AutoCSer.RandomObject.Creator<MemberClass>.Create(RandomConfig);
            memberClassData.DateTime = new DateTime(memberClassData.DateTime.Ticks, DateTimeKind.Local);
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData);
            MemberClass newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            //jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, sqlDateTimeSerializeConfig);
            //newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            //if (!AutoCSer.FieldEquals.Comparor.Equals(memberClassData, newMemberClassData))
            //{
            //    return false;
            //}
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, thirdPartyDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            jsonString = AutoCSer.JsonSerializer.Serialize(memberClassData, customDateTimeSerializeConfig);
            newMemberClassData = AutoCSer.JsonDeserializer.Deserialize<MemberClass>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(memberClassData, newMemberClassData))
            {
                return false;
            }
            #endregion

            if (AutoCSer.JsonDeserializer.Deserialize<int>(jsonString = AutoCSer.JsonSerializer.Serialize<int>(1)) != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (AutoCSer.JsonDeserializer.Deserialize<string>(jsonString = AutoCSer.JsonSerializer.Serialize<string>("1")) != "1")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            Float floatData = AutoCSer.JsonDeserializer.Deserialize<Float>(@"{Double4:-4.0,Double2:2.0,Double6:-6.0,Double5:5.0,Double3:-3.0}");
            if (floatData.Double2 != 2 || floatData.Double3 != -3 || floatData.Double4 != -4 || floatData.Double5 != 5 || floatData.Double6 != -6)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            floatData = new Float { FloatPositiveInfinity = float.NaN, FloatNegativeInfinity = float.NaN, DoublePositiveInfinity = double.NaN, DoubleNegativeInfinity = double.NaN };
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData);
            Float newFloatData = AutoCSer.JsonDeserializer.Deserialize<Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(floatData, newFloatData))
            {
                return false;
            }

            floatData = new Float();
            jsonString = AutoCSer.JsonSerializer.Serialize(floatData, new AutoCSer.JsonSerializeConfig { IsInfinityToNaN = false });
            newFloatData = AutoCSer.JsonDeserializer.Deserialize<Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(floatData, newFloatData))
            {
                return false;
            }
            return true;
        }
    }
}
