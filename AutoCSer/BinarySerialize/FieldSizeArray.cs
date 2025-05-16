using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 字段集合信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FieldSizeArray
    {
        /// <summary>
        /// 固定序列化字段
        /// </summary>
        internal LeftArray<FieldSize> FixedFields;
        /// <summary>
        /// 非固定序列化字段
        /// </summary>
        internal LeftArray<FieldSize> FieldArray;
#if !AOT
        /// <summary>
        /// JSON 混合序列化字段
        /// </summary>
        internal LeftArray<FieldIndex> JsonFields;
#endif
        /// <summary>
        /// 固定序列化字段字节数
        /// </summary>
        internal int FixedSize;
        /// <summary>
        /// 固定序列化字段字节数
        /// </summary>
        internal int AnyFixedSize;

        /// <summary>
        /// 字段成员集合
        /// </summary>
        /// <param name="fieldIndexs"></param>
        /// <param name="isJson"></param>
        /// <param name="memberCountVerify"></param>
        /// <returns>字段成员集合</returns>
        internal FieldSizeArray(LeftArray<FieldIndex> fieldIndexs, bool isJson, out int memberCountVerify)
        {
            FixedFields = new LeftArray<FieldSize>(fieldIndexs.Length);
            FieldArray = new LeftArray<FieldSize>(fieldIndexs.Length);
#if !AOT
            JsonFields = new LeftArray<FieldIndex>(0);
#endif
            FixedSize = AnyFixedSize = 0;
            foreach (FieldIndex field in fieldIndexs)
            {
                Type fieldType = field.Member.FieldType;
                if (!fieldType.isIgnoreSerialize() && !field.IsIgnore)
                {
                    var property = field.AnonymousProperty?.Member;
                    if (property == null || !property.IsDefined(typeof(IgnoreAttribute), true))
                    {
                        var memberAttribute = default(BinarySerializeMemberAttribute);
                        if (property == null) memberAttribute = field.GetAttribute<BinarySerializeMemberAttribute>(true);
                        else memberAttribute = property.GetCustomAttribute<BinarySerializeMemberAttribute>(true);
                        if (memberAttribute == null || !memberAttribute.GetIsIgnoreCurrent)
                        {
#if !AOT
                            if (memberAttribute != null && memberAttribute.GetIsJsonMember) JsonFields.Add(field);
                            else
#endif
                            {
                                FieldSize value = new FieldSize(field);
                                if (value.FixedSize == 0) FieldArray.Add(value);
                                else
                                {
                                    FixedFields.Add(value);
                                    FixedSize += value.FixedSize;
                                    AnyFixedSize |= value.FixedSize;
                                }
                            }
                        }
                    }
                }
            }
            memberCountVerify = FixedFields.Length + FieldArray.Length + 0x40000000;
#if !AOT
            if (isJson || JsonFields.Length != 0) memberCountVerify |= 0x20000000;
#endif
            FixedFields.Sort(FieldSize.FixedSizeSort);
        }
        //        /// <summary>
        //        /// 字段集合信息
        //        /// </summary>
        //        /// <param name="fixedFields">固定序列化字段</param>
        //        /// <param name="fields">非固定序列化字段</param>
        //        /// <param name="jsonFields">JSON 混合序列化字段</param>
        //        /// <param name="fixedSize">固定序列化字段字节数</param>
        //        /// <param name="isJson"></param>
        //        /// <param name="memberCountVerify">序列化成员数量</param>
        //        internal FieldSizeArray(ref LeftArray<FieldSize> fixedFields, ref LeftArray<FieldSize> fields, ref LeftArray<FieldIndex> jsonFields, int fixedSize, bool isJson, out int memberCountVerify)
        //        {
        //            memberCountVerify = fixedFields.Length + fields.Length + 0x40000000;
        //            if (isJson || jsonFields.Length != 0) memberCountVerify |= 0x20000000;
        //            AnyFixedSize = 0;

        //            fixedFields.Sort(FieldSize.FixedSizeSort);
        //            FixedFields = fixedFields;
        //            FieldArray = fields;
        //#if !AOT
        //            JsonFields = jsonFields;
        //#endif
        //            FixedSize = fixedSize;
        //        }
        /// <summary>
        /// 判断是否支持简单序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isReferenceMember"></param>
        /// <returns></returns>
        internal bool IsSimpleSerialize(Type type, bool isReferenceMember)
        {
#if !AOT
            if (JsonFields.Length != 0) return false;
#endif
            foreach (FieldSize field in FixedFields)
            {
                Type memberType = field.Field.FieldType;
                if (field.FieldIndex.AnonymousProperty != null) return false;
                if (isReferenceMember && memberType.IsClass) return false;
                if (!SimpleSerialize.Serializer.IsType(memberType)) return false;
            }
            foreach (FieldSize field in FieldArray)
            {
                Type memberType = field.Field.FieldType;
                if (field.FieldIndex.AnonymousProperty != null) return false;
                if (isReferenceMember && memberType.IsClass) return false;
                if (!SimpleSerialize.Serializer.IsType(memberType)) return false;
            }
            int memberCountVerify;
            FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetFields(type, MemberFiltersEnum.InstanceField), false, out memberCountVerify);
            if (fields.FixedFields.Length == FixedFields.Length && fields.FieldArray.Length == FieldArray.Length)
            {
                HashSet<string> memberNames = new HashSet<string>();
                foreach (FieldSize field in FixedFields) memberNames.Add(field.Field.Name);
                foreach (FieldSize field in FieldArray) memberNames.Add(field.Field.Name);
                foreach (FieldSize field in fields.FixedFields)
                {
                    if (!memberNames.Remove(field.Field.Name)) return false;
                }
                foreach (FieldSize field in fields.FieldArray)
                {
                    if (!memberNames.Remove(field.Field.Name)) return false;
                }
                return true;
            }
            return false;
        }
    }
}
