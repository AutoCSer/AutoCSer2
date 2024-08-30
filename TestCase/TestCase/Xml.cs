using System;
using AutoCSer.TestCase.Data;

namespace AutoCSer.TestCase
{
    class Xml
    {
        /// <summary>
        /// 带成员位图的XML序列化参数配置
        /// </summary>
        private static readonly AutoCSer.XmlSerializeConfig xmlSerializeConfig = new AutoCSer.XmlSerializeConfig();

        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型字段成员XML序列化测试
            Field filedData = AutoCSer.RandomObject.Creator<Field>.Create(Json.RandomConfig);
            string xmlString = AutoCSer.XmlSerializer.Serialize(filedData);
            Field newField = AutoCSer.XmlDeserializer.Deserialize<Field>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(filedData, newField))
            {
                return false;
            }
            #endregion

            #region 带成员位图的引用类型字段成员XML序列化测试
            AutoCSer.Metadata.MemberMap<Field> fieldMemberMap = AutoCSer.Metadata.MemberMap<Field>.NewFull();
            xmlSerializeConfig.MemberMap = fieldMemberMap;
            xmlString = AutoCSer.XmlSerializer.Serialize(filedData, xmlSerializeConfig);
            newField = AutoCSer.XmlDeserializer.Deserialize<Field>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(filedData, newField, fieldMemberMap))
            {
                return false;
            }
            #endregion

            #region 引用类型属性成员XML序列化测试
            Property propertyData = AutoCSer.RandomObject.Creator<Property>.Create(Json.RandomConfig);
            xmlString = AutoCSer.XmlSerializer.Serialize(propertyData);
            Property newProperty = AutoCSer.XmlDeserializer.Deserialize<Property>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(propertyData, newProperty))
            {
                return false;
            }
            #endregion

            #region 值类型字段成员XML序列化测试
            StructField structField = AutoCSer.RandomObject.Creator<StructField>.Create(Json.RandomConfig);
            xmlString = AutoCSer.XmlSerializer.Serialize(structField);
            StructField newStructField = AutoCSer.XmlDeserializer.Deserialize<StructField>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structField, newStructField))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型字段成员XML序列化测试
            AutoCSer.Metadata.MemberMap<StructField> structFieldMemberMap = AutoCSer.Metadata.MemberMap<StructField>.NewFull();
            xmlSerializeConfig.MemberMap = structFieldMemberMap;
            xmlString = AutoCSer.XmlSerializer.Serialize(structField, xmlSerializeConfig);
            newStructField = AutoCSer.XmlDeserializer.Deserialize<StructField>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structField, newStructField, structFieldMemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型属性成员XML序列化测试
            StructProperty structProperty = AutoCSer.RandomObject.Creator<StructProperty>.Create(Json.RandomConfig);
            xmlString = AutoCSer.XmlSerializer.Serialize(structProperty);
            StructProperty newStructProperty = AutoCSer.XmlDeserializer.Deserialize<StructProperty>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structProperty, newStructProperty))
            {
                return false;
            }
            #endregion

            if (AutoCSer.XmlDeserializer.Deserialize<int>(xmlString = AutoCSer.XmlSerializer.Serialize<int>(1)) != 1)
            {
                return false;
            }
            if (AutoCSer.XmlDeserializer.Deserialize<string>(xmlString = AutoCSer.XmlSerializer.Serialize<string>("1")) != "1")
            {
                return false;
            }

            Float floatData = AutoCSer.XmlDeserializer.Deserialize<Float>(@"<?xml version=""1.0"" encoding=""utf-8""?><xml><Double4>-4.0</Double4><Double2>2.0</Double2><Double6>-6.0</Double6><Double5>5.0</Double5><Double3>-3.0</Double3></xml>");
            if (floatData.Double2 != 2 || floatData.Double3 != -3 || floatData.Double4 != -4 || floatData.Double5 != 5 || floatData.Double6 != -6)
            {
                return false;
            }

            floatData = new Float { FloatPositiveInfinity = float.NaN, FloatNegativeInfinity = float.NaN, DoublePositiveInfinity = double.NaN, DoubleNegativeInfinity = double.NaN };
            xmlString = AutoCSer.XmlSerializer.Serialize(floatData);
            Float newFloatData = AutoCSer.XmlDeserializer.Deserialize<Float>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor.Equals(floatData, newFloatData))
            {
                return false;
            }

            return true;
        }
    }
}
