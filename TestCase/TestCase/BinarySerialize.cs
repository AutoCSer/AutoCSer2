using System;

namespace AutoCSer.TestCase
{
    class BinarySerialize
    {
        /// <summary>
        /// 带成员位图的二进制序列化参数配置
        /// </summary>
        private static readonly AutoCSer.BinarySerializeConfig serializeConfig = new AutoCSer.BinarySerializeConfig();
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型二进制序列化测试
            Data.Field fieldData = AutoCSer.RandomObject.Creator<Data.Field>.Create();
            byte[] data = AutoCSer.BinarySerializer.Serialize(fieldData);
            Data.Field newFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.Field>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.Field>.Equals(fieldData, newFieldData))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 带成员位图的引用类型二进制序列化测试
            serializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.Field>.NewFull();
            data = AutoCSer.BinarySerializer.Serialize(fieldData, serializeConfig);
            newFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.Field>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.Field>.MemberMapEquals(fieldData, newFieldData, serializeConfig.MemberMap))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 值类型二进制序列化测试
            Data.StructField structFieldData = AutoCSer.RandomObject.Creator<Data.StructField>.Create();
            data = AutoCSer.BinarySerializer.Serialize(structFieldData);
            Data.StructField newStructFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.StructField>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructField>.Equals(structFieldData, newStructFieldData))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 带成员位图的值类型二进制序列化测试
            serializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.StructField>.NewFull();
            data = AutoCSer.BinarySerializer.Serialize(structFieldData, serializeConfig);
            newStructFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.StructField>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructField>.MemberMapEquals(structFieldData, newStructFieldData, serializeConfig.MemberMap))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 引用类型属性成员二进制序列化测试
            Data.Property propertyData = AutoCSer.RandomObject.Creator<Data.Property>.Create();
            data = AutoCSer.BinarySerializer.Serialize(propertyData);
            Data.Property newProperty = AutoCSer.BinaryDeserializer.Deserialize<Data.Property>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.Property>.Equals(propertyData, newProperty))
            {
                return Program.Breakpoint();
            }
            #endregion

            #region 派生引用类型属性成员二进制序列化测试
            Data.InheritProperty inheritPropertyData = AutoCSer.RandomObject.Creator<Data.InheritProperty>.Create();
            data = AutoCSer.BinarySerializer.Serialize(inheritPropertyData);
            Data.InheritProperty newInheritProperty = AutoCSer.BinaryDeserializer.Deserialize<Data.InheritProperty>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.InheritProperty>.Equals(inheritPropertyData, newInheritProperty))
            {
                return Program.Breakpoint();
            }
            #endregion

            if (AutoCSer.BinaryDeserializer.Deserialize<int>(data = AutoCSer.BinarySerializer.Serialize<int>(1)) != 1)
            {
                return Program.Breakpoint();
            }
            if (AutoCSer.BinaryDeserializer.Deserialize<string>(data = AutoCSer.BinarySerializer.Serialize<string>("1")) != "1")
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
