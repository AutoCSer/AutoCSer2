using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class AotMethod : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    #region IF IsCallAutoCSer
                    AutoCSer.AotMethod.Call();
                    #endregion IF IsCallAutoCSer
                    #region IF IsCallStreamPersistenceMemoryDatabase
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.AotMethod.Call();
                    #endregion IF IsCallStreamPersistenceMemoryDatabase
                    #region LOOP Methods
                    #region IF MemberType
                    @MemberType.FullName/**/.@MethodName();
                    #endregion IF MemberType
                    #region NOT MemberType
                    @MethodName();
                    #endregion NOT MemberType
                    #endregion LOOP Methods
                    #region LOOP EqualsMemberTypes
                    AutoCSer.FieldEquals.Comparor.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(default(@MemberType.FullName), default(@MemberType.FullName));
                    #endregion LOOP EqualsMemberTypes
                    #region LOOP RandomMemberTypes
                    AutoCSer.RandomObject.Creator.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null);
                    #endregion LOOP RandomMemberTypes

                    #region LOOP BinarySerializeMemberTypes
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<@MemberType.FullName>));
                    #region IF ReflectionMethodName
                    AutoCSer.BinarySerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, default(@MemberType.FullName));
                    #endregion IF ReflectionMethodName
                    #endregion LOOP BinarySerializeMemberTypes
                    #region IF BinarySerializeMemberTypes.Length
                    binaryDeserializeMemberTypes();
                    #endregion IF BinarySerializeMemberTypes.Length
                    #region LOOP BinarySerializeGenericTypes
                    AutoCSer.AotReflection.FieldsAndProperties(typeof(@MemberType.FullName));
                    #endregion LOOP BinarySerializeGenericTypes
                    #region LOOP BinarySerializeGenericMemberTypes
                    AutoCSer.BinarySerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, null);
                    AutoCSer.BinaryDeserializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null);
                    #endregion LOOP BinarySerializeGenericMemberTypes

                    #region LOOP JsonSerializeMemberTypes
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<@MemberType.FullName>));
                    #region IF ReflectionMethodName
                    AutoCSer.JsonSerializer.@ReflectionMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(null, default(@MemberType.FullName));
                    #endregion IF ReflectionMethodName
                    #endregion LOOP JsonSerializeMemberTypes
                    #region IF JsonDeserializeMemberTypes.Length
                    jsonDeserializeMemberTypes();
                    #endregion IF JsonDeserializeMemberTypes.Length

                    #region LOOP XmlSerializeMemberTypes
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<@MemberType.FullName>));
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

                    #region LOOP StreamPersistenceMemoryDatabaseSnapshotTypes
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotNode.Create<@MemberType.FullName>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.EnumerableSnapshotNode.Create<@MemberType.FullName>(null);
                    #endregion LOOP StreamPersistenceMemoryDatabaseSnapshotTypes
                    #region LOOP StreamPersistenceMemoryDatabaseSnapshotCloneObjectTypes
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneNode.@ReflectionMethodName<@MemberType.FullName>(null);
                    #endregion LOOP StreamPersistenceMemoryDatabaseSnapshotCloneObjectTypes
                    return true;
                }
                return false;
            }
            #region IF BinarySerializeMemberTypes.Length
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
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
            /// JSON 反序列化成员类型代码生成调用激活 AOT 反射
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
            /// XML 反序列化成员类型代码生成调用激活 AOT 反射
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
            private static void MethodName() { }
        }
    }
}
