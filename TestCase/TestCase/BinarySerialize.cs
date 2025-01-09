using System;
using System.Linq;
using AutoCSer.Extensions;
using AutoCSer.TestCase.Data;

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
            if (!AutoCSer.FieldEquals.Comparor.Equals(fieldData, newFieldData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 带成员位图的引用类型二进制序列化测试
            AutoCSer.Metadata.MemberMap<Data.Field> fieldMemberMap = AutoCSer.Metadata.MemberMap<Data.Field>.NewFull();
            serializeConfig.MemberMap = fieldMemberMap;
            data = AutoCSer.BinarySerializer.Serialize(fieldData, serializeConfig);
            newFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.Field>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(fieldData, newFieldData, fieldMemberMap))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 值类型二进制序列化测试
            Data.StructField structFieldData = AutoCSer.RandomObject.Creator<Data.StructField>.Create();
            data = AutoCSer.BinarySerializer.Serialize(structFieldData);
            Data.StructField newStructFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.StructField>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structFieldData, newStructFieldData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 带成员位图的值类型二进制序列化测试
            AutoCSer.Metadata.MemberMap<Data.StructField> structFieldMemberMap = AutoCSer.Metadata.MemberMap<Data.StructField>.NewFull();
            serializeConfig.MemberMap = structFieldMemberMap;
            data = AutoCSer.BinarySerializer.Serialize(structFieldData, serializeConfig);
            newStructFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.StructField>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(structFieldData, newStructFieldData, structFieldMemberMap))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 引用类型属性成员二进制序列化测试
            Data.Property propertyData = AutoCSer.RandomObject.Creator<Data.Property>.Create();
            data = AutoCSer.BinarySerializer.Serialize(propertyData);
            Data.Property newProperty = AutoCSer.BinaryDeserializer.Deserialize<Data.Property>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(propertyData, newProperty))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 派生引用类型属性成员二进制序列化测试
            Data.InheritProperty inheritPropertyData = AutoCSer.RandomObject.Creator<Data.InheritProperty>.Create();
            data = AutoCSer.BinarySerializer.Serialize(inheritPropertyData);
            Data.InheritProperty newInheritProperty = AutoCSer.BinaryDeserializer.Deserialize<Data.InheritProperty>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(inheritPropertyData, newInheritProperty))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region 二进制混杂 JSON 序列化
            Data.JsonField jsonFieldData = AutoCSer.RandomObject.Creator<Data.JsonField>.Create();
            data = AutoCSer.BinarySerializer.Serialize(jsonFieldData);
            Data.JsonField newJsonFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.JsonField>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(jsonFieldData, newJsonFieldData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var jsonEmptyData = default(JsonEmpty);
            AutoCSer.BinarySerialize.DeserializeResult result = AutoCSer.BinaryDeserializer.Deserialize<JsonEmpty>(data, ref jsonEmptyData);
            if(result.State != AutoCSer.BinarySerialize.DeserializeStateEnum.Success || jsonEmptyData == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            data = AutoCSer.BinarySerializer.Serialize(default(Data.JsonField));
            result = AutoCSer.BinaryDeserializer.Deserialize<Data.JsonField>(data, ref newJsonFieldData);
            if (result.State != AutoCSer.BinarySerialize.DeserializeStateEnum.Success || newJsonFieldData != null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            Data.JsonProperty jsonPropertyData = AutoCSer.RandomObject.Creator<Data.JsonProperty>.Create();
            data = AutoCSer.BinarySerializer.Serialize(jsonPropertyData);
            Data.JsonProperty newJsonPropertyData = AutoCSer.BinaryDeserializer.Deserialize<Data.JsonProperty>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(jsonPropertyData, newJsonPropertyData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            Data.JsonStructField jsonStructFieldData = AutoCSer.RandomObject.Creator<Data.JsonStructField>.Create();
            data = AutoCSer.BinarySerializer.Serialize(jsonStructFieldData);
            Data.JsonStructField newJsonStructFieldData = AutoCSer.BinaryDeserializer.Deserialize<Data.JsonStructField>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(jsonStructFieldData, newJsonStructFieldData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            Data.JsonStructProperty jsonStructPropertyData = AutoCSer.RandomObject.Creator<Data.JsonStructProperty>.Create();
            data = AutoCSer.BinarySerializer.Serialize(jsonStructPropertyData);
            Data.JsonStructProperty newJsonStructPropertyData = AutoCSer.BinaryDeserializer.Deserialize<Data.JsonStructProperty>(data);
            if (!AutoCSer.FieldEquals.Comparor.Equals(jsonStructPropertyData, newJsonStructPropertyData))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region ORM 关联数据二进制序列化测试
            Data.ORM.ModelGeneric model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
            data = AutoCSer.BinarySerializer.Serialize(model);
            Data.ORM.BusinessModel businessModel = AutoCSer.BinaryDeserializer.Deserialize<Data.ORM.BusinessModel>(data);
            if (!ModelComparor(model, businessModel)) return AutoCSer.Breakpoint.ReturnFalse();

            data = AutoCSer.BinarySerializer.Serialize(businessModel);
            model = AutoCSer.BinaryDeserializer.Deserialize<Data.ORM.ModelGeneric>(data);
            if (!ModelComparor(model, businessModel)) return AutoCSer.Breakpoint.ReturnFalse();
            #endregion

            if (AutoCSer.BinaryDeserializer.Deserialize<int>(data = AutoCSer.BinarySerializer.Serialize<int>(1)) != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (AutoCSer.BinaryDeserializer.Deserialize<string>(data = AutoCSer.BinarySerializer.Serialize<string>("1")) != "1")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
        internal static bool ModelComparor(Data.ORM.ModelGeneric model, Data.ORM.BusinessModel businessModel)
        {
            if (!AutoCSer.FieldEquals.Comparor.Equals<Data.ORM.CommonModel>(model, businessModel))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (model.ModelAssociated == null)
            {
                if (businessModel.ModelAssociated != null) return AutoCSer.Breakpoint.ReturnFalse();
            }
            else
            {
                if (businessModel.ModelAssociated == null) return AutoCSer.Breakpoint.ReturnFalse();
                if (!AutoCSer.FieldEquals.Comparor.Equals(model.ModelAssociated, businessModel.ModelAssociated))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (model.ModelAssociatedList == null)
            {
                if (businessModel.ModelAssociatedList != null) return AutoCSer.Breakpoint.ReturnFalse();
            }
            else
            {
                if (model.ModelAssociatedList.Count != businessModel.ModelAssociatedList?.Count)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!AutoCSer.FieldEquals.Comparor.Equals(model.ModelAssociatedList, businessModel.ModelAssociatedList.getListArray(p => (Data.ORM.ModelAssociated)p)))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
