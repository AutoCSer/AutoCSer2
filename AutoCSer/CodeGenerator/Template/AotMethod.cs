using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class AotMethod : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 触发 AOT 编译
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    #region LOOP Methods
                    @MemberType.FullName/**/.@MethodName();
                    #endregion LOOP Methods
                    #region LOOP EqualsMemberTypes
                    AutoCSer.FieldEquals.Comparor.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(default(@MemberType.FullName), default(@MemberType.FullName));
                    #endregion LOOP EqualsMemberTypes
                    #region LOOP RandomMemberTypes
                    AutoCSer.RandomObject.Creator.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null);
                    #endregion LOOP RandomMemberTypes

                    #region LOOP BinarySerializeMemberTypes
                    AutoCSer.BinarySerializer.TypeSerialize(typeof(AutoCSer.BinarySerialize.TypeSerializer<@MemberType.FullName>));
                    #region IF ReflectionMethodName
                    AutoCSer.BinarySerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, default(@MemberType.FullName));
                    #endregion IF ReflectionMethodName
                    #endregion LOOP BinarySerializeMemberTypes
                    #region IF BinarySerializeMemberTypes.Length
                    binaryDeserializeMemberTypes();
                    #endregion IF BinarySerializeMemberTypes.Length
                    #region LOOP BinarySerializeGenericTypes
                    AutoCSer.BinarySerializer.BinarySerializeGenericType<@MemberType.FullName>();
                    #endregion LOOP BinarySerializeGenericTypes
                    #region LOOP BinarySerializeGenericMemberTypes
                    AutoCSer.BinarySerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, null);
                    AutoCSer.BinaryDeserializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null);
                    #endregion LOOP BinarySerializeGenericMemberTypes

                    #region LOOP JsonSerializeMemberTypes
                    AutoCSer.JsonSerializer.TypeSerialize(typeof(AutoCSer.Json.TypeSerializer<@MemberType.FullName>));
                    #region IF ReflectionMethodName
                    AutoCSer.JsonSerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, default(@MemberType.FullName));
                    #endregion IF ReflectionMethodName
                    #endregion LOOP JsonSerializeMemberTypes
                    #region IF JsonDeserializeMemberTypes.Length
                    jsonDeserializeMemberTypes();
                    #endregion IF JsonDeserializeMemberTypes.Length

                    #region LOOP XmlSerializeMemberTypes
                    AutoCSer.XmlSerializer.TypeSerialize(typeof(AutoCSer.Xml.TypeSerializer<@MemberType.FullName>));
                    #region IF ReflectionMethodName
                    AutoCSer.XmlSerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, default(@MemberType.FullName));
                    #endregion IF ReflectionMethodName
                    #endregion LOOP XmlSerializeMemberTypes
                    #region IF XmlDeserializeMemberTypes.Length
                    xmlDeserializeMemberTypes();
                    #endregion IF XmlDeserializeMemberTypes.Length
                    #region LOOP XmlSerializeNullableElementTypes
                    AutoCSer.XmlSerializer.NullableHasValue<@MemberType.FullName/*NOTE*/, int/*NOTE*/>(null);
                    #endregion LOOP XmlSerializeNullableElementTypes
                    return true;
                }
                return false;
            }
            #region IF BinarySerializeMemberTypes.Length
            /// <summary>
            /// 二进制反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
                #region LOOP BinarySerializeMemberTypes
                #region IF ReflectionMethodName
                @MemberType.FullName @MemberName = default(@MemberType.FullName);
                AutoCSer.BinaryDeserializer.@ReflectionMethodName<@GenericTypeName/*NOTE*/, MemberType.FullName/*NOTE*/>(null, ref @MemberName);
                #endregion IF ReflectionMethodName
                #endregion LOOP BinarySerializeMemberTypes
            }
            #endregion IF BinarySerializeMemberTypes.Length
            #region IF JsonDeserializeMemberTypes.Length
            /// <summary>
            /// JSON 反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void jsonDeserializeMemberTypes()
            {
                #region LOOP JsonDeserializeMemberTypes
                @MemberType.FullName @MemberName = default(@MemberType.FullName);
                AutoCSer.JsonDeserializer.@ReflectionMethodName<@GenericTypeName/*NOTE*/, MemberType.FullName/*NOTE*/>(null, ref @MemberName);
                #endregion LOOP JsonDeserializeMemberTypes
            }
            #endregion IF JsonDeserializeMemberTypes.Length
            #region IF XmlDeserializeMemberTypes.Length
            /// <summary>
            /// XML 反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void xmlDeserializeMemberTypes()
            {
                #region LOOP XmlDeserializeMemberTypes
                @MemberType.FullName @MemberName = default(@MemberType.FullName);
                AutoCSer.XmlDeserializer.@ReflectionMethodName<@GenericTypeName/*NOTE*/, MemberType.FullName/*NOTE*/>(null, ref @MemberName);
                #endregion LOOP XmlDeserializeMemberTypes
            }
            #endregion IF XmlDeserializeMemberTypes.Length
            #endregion PART CLASS
        }
    }
}
