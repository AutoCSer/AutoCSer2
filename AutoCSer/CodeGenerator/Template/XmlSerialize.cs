using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class XmlSerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// XML serialization
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void @XmlSerializeMethodName(AutoCSer.XmlSerializer serializer, @CurrentType.FullName value)
            {
                #region IF Members.Length
                value.xmlSerialize(serializer);
                #endregion IF Members.Length
            }
            /// <summary>
            /// XML serialization of member bitmap is supported
            /// 支持成员位图的 XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void @XmlSerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, XmlSerializer serializer, @CurrentType.FullName value, AutoCSer.Memory.CharStream stream)
            {
                #region IF Members.Length
                value.xmlSerialize(memberMap, serializer, stream);
                #endregion IF Members.Length
            }
            #region IF Members.Length
            /// <summary>
            /// XML serialization
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                #region LOOP Members
                #region IF IsOutputMethodName
                if (AutoCSer.XmlSerializer.@IsOutputMethodName(__serializer__, @MemberName))
                #endregion IF IsOutputMethodName
                {
                    __stream__.SimpleWrite(@"@SerializeMemberNameStart");
                    #region IF MemberItemName
                    AutoCSer.XmlSerializer.SetItemName(__serializer__, "@MemberItemName");
                    #endregion IF MemberItemName
                    #region IF MemberType.Type.IsValueType
                    #region IF MemberType.Type.IsEnum
                    __serializer__.@EnumXmlSerializeMethodName(@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF MemberType.Type.IsValueType
                    #region NOT MemberType.Type.IsValueType
                    if (@MemberName != null) __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsValueType
                    __stream__.SimpleWrite(@"@SerializeMemberNameEnd");
                }
                #endregion LOOP Members
            }
            /// <summary>
            /// XML serialization of member bitmap is supported
            /// 支持成员位图的 XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                #region LOOP Members
                if (__memberMap__.IsMember(@MemberIndex)/*IF:IsOutputMethodName*/ && AutoCSer.XmlSerializer.@IsOutputMethodName(__serializer__, @MemberName)/*IF:IsOutputMethodName*/)
                {
                    __stream__.SimpleWrite(@"@SerializeMemberNameStart");
                    #region IF MemberItemName
                    AutoCSer.XmlSerializer.SetItemName(__serializer__, "@MemberItemName");
                    #endregion IF MemberItemName
                    #region IF MemberType.Type.IsValueType
                    #region IF MemberType.Type.IsEnum
                    __serializer__.@EnumXmlSerializeMethodName(@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF MemberType.Type.IsValueType
                    #region NOT MemberType.Type.IsValueType
                    if (@MemberName != null) __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsValueType
                    __stream__.SimpleWrite(@"@SerializeMemberNameEnd");
                }
                #endregion LOOP Members
            }
            #endregion IF Members.Length
            /// <summary>
            /// Member XML deserialization
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void @XmlDeserializeMethodName(AutoCSer.XmlDeserializer __deserializer__, ref @CurrentType.FullName __value__, int __memberIndex__)
            {
                #region IF DeserializeMemberCount
                switch (__memberIndex__)
                {
                    #region LOOP DeserializeMembers
                    case @IntMemberIndex:
                        #region NOT IsField
                        var @MemberName = __value__.@MemberName;
                        #region IF MemberType.Type.IsEnum
                        __deserializer__.@EnumXmlDeserializeMethodName(ref @MemberName);
                        #endregion IF MemberType.Type.IsEnum
                        #region NOT MemberType.Type.IsEnum
                        __deserializer__.@DeserializeMethodName(ref @MemberName);
                        #endregion NOT MemberType.Type.IsEnum
                        __value__.@MemberName = @MemberName;
                        #endregion NOT IsField
                        #region IF IsField
                        #region IF MemberType.Type.IsEnum
                        __deserializer__.@EnumXmlDeserializeMethodName(ref __value__.@MemberName);
                        #endregion IF MemberType.Type.IsEnum
                        #region NOT MemberType.Type.IsEnum
                        __deserializer__.@DeserializeMethodName(ref __value__.@MemberName);
                        #endregion NOT MemberType.Type.IsEnum
                        #endregion IF IsField
                        return;
                        #endregion LOOP DeserializeMembers
                }
                #endregion IF DeserializeMemberCount
            }
            /// <summary>
            /// Gets an XML deserialized collection of member names and a collection of member indexes
            /// 获取 XML 反序列化成员名称集合与成员索引集合
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> @XmlDeserializeMemberNameMethodName()
            {
                #region IF DeserializeMemberCount
                return xmlDeserializeMemberName();
                #endregion IF DeserializeMemberCount
                #region NOT DeserializeMemberCount
                return default(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>);
                #endregion NOT DeserializeMemberCount
            }
            #region IF DeserializeMemberCount
            /// <summary>
            /// Gets an XML deserialized collection of member names and a collection of member indexes
            /// 获取 XML 反序列化成员名称集合与成员索引集合
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(@DeserializeMemberCount);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(@DeserializeMemberCount);
                #region LOOP DeserializeMembers
                names.Add(nameof(@MemberName));
                #region IF MemberItemName
                indexs.Add(new KeyValue<int, string>(@IntMemberIndex, "@MemberItemName"));
                #endregion IF MemberItemName
                #region NOT MemberItemName
                indexs.Add(new KeyValue<int, string>(@IntMemberIndex, null));
                #endregion NOT MemberItemName
                #endregion LOOP DeserializeMembers
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            #endregion IF DeserializeMemberCount
            #region IF MemberTypeCount
            /// <summary>
            /// Get the collection of XML serialized member types
            /// 获取 XML 序列化成员类型集合
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> @XmlSerializeMemberTypeMethodName()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(@MemberTypeCount);
                #region LOOP MemberTypes
                types.Add(typeof(@MemberType.FullName));
                #endregion LOOP MemberTypes
                return types;
            }
            #endregion IF MemberTypeCount
            /// <summary>
            /// AOT code generation call activation reflection
            /// AOT 代码生成调用激活反射
            /// </summary>
            #region IF CurrentType.Type.IsGenericType
            public static void @XmlSerializeMethodName(/*NOTE*/object value/*NOTE*/)/*NOTE*/ { }/*NOTE*/
            #endregion IF CurrentType.Type.IsGenericType
            #region NOT CurrentType.Type.IsGenericType
            internal static void @XmlSerializeMethodName()
            #endregion NOT CurrentType.Type.IsGenericType
            {
                @CurrentType.FullName value = default(@CurrentType.FullName);
                @XmlSerializeMethodName(null, value);
                @XmlSerializeMemberMapMethodName(null, null, value, null);
                @XmlDeserializeMethodName(null, ref value, 0);
                @XmlDeserializeMemberNameMethodName();
                AutoCSer.AotReflection.NonPublicMethods(typeof(@CurrentType.FullName));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(@CurrentType.FullName));
                #region IF MemberTypeCount
                @XmlSerializeMemberTypeMethodName();
                #endregion IF MemberTypeCount
            }
            #endregion PART CLASS
            private string MemberName = null;
            private const int IntMemberIndex = 0;
            private AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName> MemberIndex = new AutoCSer.Metadata.MemberMapIndex<FullName>(null);
            private const int DeserializeMemberCount = 0;
            private const int MemberTypeCount = 0;
        }
    }
}
