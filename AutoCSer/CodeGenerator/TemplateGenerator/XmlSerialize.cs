using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Xml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    [Generator(Name = "XML 序列化", IsAuto = true)]
    internal partial class XmlSerialize : AttributeGenerator<AutoCSer.CodeGenerator.XmlSerializeAttribute>
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public sealed class SerializeMember
        {
            /// <summary>
            /// 成员字段
            /// </summary>
            private readonly AutoCSer.CodeGenerator.Metadata.MemberIndex member;
            /// <summary>
            /// 成员类型
            /// </summary>
            public ExtensionType MemberType;
            /// <summary>
            /// 枚举序列化方法名称
            /// </summary>
            public string EnumXmlSerializeMethodName
            {
                get { return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString(); }
            }
            /// <summary>
            /// 枚举反序列化方法名称
            /// </summary>
            public string EnumXmlDeserializeMethodName
            {
                get
                {
                    if (MemberType.Type.IsDefined(typeof(FlagsAttribute), false)) return "EnumFlags" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString();
                    return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString();
                }
            }
            /// <summary>
            /// 成员名称
            /// </summary>
            public string MemberName { get { return member.Member.Name; } }
            /// <summary>
            /// 序列化成员名称开始标签
            /// </summary>
            public string SerializeMemberNameStart;
            /// <summary>
            /// 序列化成员名称结束标签
            /// </summary>
            public string SerializeMemberNameEnd;
            /// <summary>
            /// 成员编号
            /// </summary>
            public int MemberIndex { get { return member.MemberIndex; } }
            /// <summary>
            /// 成员编号
            /// </summary>
            public int IntMemberIndex { get { return MemberIndex; } }
            /// <summary>
            /// 是否成员字段
            /// </summary>
            public readonly bool IsField;
            /// <summary>
            /// JSON 序列化信息
            /// </summary>
            public readonly TextSerializeMethodInfo MethodInfo;
            /// <summary>
            /// 序列化方法名称
            /// </summary>
            public string SerializeMethodName
            {
                get
                {
                    if (MemberType.Type.IsGenericParameter || MethodInfo.IsTypeSerialize) return nameof(AutoCSer.XmlSerializer.XmlSerializeType);
                    if (MemberType.IsObject) return nameof(AutoCSer.XmlSerializer.XmlSerializeObject);
                    if (MethodInfo.IsCusotm) return nameof(AutoCSer.XmlSerializer.ICustom);
                    return XmlSerializeAttribute.XmlSerializeMethodName;
                }
            }
            /// <summary>
            /// 序列化方法名称
            /// </summary>
            public string DeserializeMethodName
            {
                get
                {
                    return XmlSerializeAttribute.XmlDeserializeMethodName;
                }
            }
            /// <summary>
            /// 是否输出判断方法名称
            /// </summary>
            public readonly string IsOutputMethodName;
            /// <summary>
            /// 集合子节点名称
            /// </summary>
            public readonly string MemberItemName;
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="member"></param>
            /// <param name="memberType"></param>
            /// <param name="isField"></param>
            /// <param name="attribute"></param>
            private SerializeMember(AutoCSer.CodeGenerator.Metadata.MemberIndex member, Type memberType, bool isField, XmlSerializeMemberAttribute attribute)
            {
                this.member = member;
                MemberType = memberType;
                IsField = isField;
                SerializeMemberNameStart = "<" + MemberName + ">";
                SerializeMemberNameEnd = "</" + MemberName + ">";
                if (!string.IsNullOrEmpty(attribute?.ItemName)) MemberItemName = attribute.ItemName;
                if (memberType.IsGenericParameter) IsOutputMethodName = nameof(AutoCSer.XmlSerializer.IsOutputGenericParameter);
                else
                {
                    AutoCSer.XmlSerializeAttribute xmlSerializeAttribute;
                    Type baseType;
                    getSerializeMethodInfo(memberType, ref MethodInfo, out xmlSerializeAttribute, out baseType);
                    if (memberType.IsValueType)
                    {
                        if (memberType == typeof(SubString)) IsOutputMethodName = nameof(XmlSerializer.IsOutputSubString);
                        else if (memberType.IsGenericType && memberType.IsValueType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>)) IsOutputMethodName = nameof(XmlSerializer.IsOutputNullable);
                    }
                    else
                    {
                        IsOutputMethodName = memberType == typeof(string) ? nameof(XmlSerializer.IsOutputString) : nameof(XmlSerializer.IsOutput);
                    }
                    types.Add(memberType);
                }
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="field"></param>
            public SerializeMember(AutoCSer.Metadata.FieldIndex field, XmlSerializeMemberAttribute attribute) : this(new AutoCSer.CodeGenerator.Metadata.FieldIndex(field), field.Member.FieldType, true, attribute)
            {
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="property"></param>
            public SerializeMember(AutoCSer.Metadata.PropertyIndex property, XmlSerializeMemberAttribute attribute) : this(new AutoCSer.CodeGenerator.Metadata.PropertyIndex(property), property.Member.PropertyType, false, attribute)
            {
            }
        }

        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string XmlSerializeMethodName { get { return XmlSerializeAttribute.XmlSerializeMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string XmlSerializeMemberMapMethodName { get { return XmlSerializeAttribute.XmlSerializeMemberMapMethodName; } }
        /// <summary>
        /// 获取 XML 序列化成员类型方法名称
        /// </summary>
        public string XmlSerializeMemberTypeMethodName { get { return XmlSerializeAttribute.XmlSerializeMemberTypeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string XmlDeserializeMethodName { get { return XmlSerializeAttribute.XmlDeserializeMethodName; } }
        /// <summary>
        /// 获取 XML 反序列化成员名称方法名称
        /// </summary>
        public string XmlDeserializeMemberNameMethodName { get { return XmlSerializeAttribute.XmlDeserializeMemberNameMethodName; } }
        /// <summary>
        /// 序列化成员集合
        /// </summary>
        public SerializeMember[] Members;
        /// <summary>
        /// 反序列化成员集合
        /// </summary>
        public SerializeMember[] DeserializeMembers;
        /// <summary>
        /// 反序列化成员数量
        /// </summary>
        public int DeserializeMemberCount { get { return DeserializeMembers.Length; } }
        /// <summary>
        /// 成员类型集合
        /// </summary>
        public AotMethod.ReflectionMemberType[] MemberTypes;
        /// <summary>
        /// 成员类型数量
        /// </summary>
        public int MemberTypeCount { get { return MemberTypes.Length; } }

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type, baseType;
            TextSerializeMethodInfo serializeMethodInfo = default(TextSerializeMethodInfo);
            AutoCSer.XmlSerializeAttribute attribute;
            if (getSerializeMethodInfo(type, ref serializeMethodInfo, out attribute, out baseType))
            {
                if (baseType != null) types.Add(type);
                return AutoCSer.Common.CompletedTask;
            }
            HashSet<HashObject<Type>> memberTypes = HashSetCreator.CreateHashObject<Type>();
            var fields = AutoCSer.TextSerialize.Common.GetSerializeFields<XmlSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetFields(type, attribute.MemberFilters), attribute);
            LeftArray<AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetSerializeProperties<XmlSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type, attribute.MemberFilters), attribute);
            LeftArray<AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute>> deserializeProperties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<XmlSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type, attribute.MemberFilters), attribute);
            LeftArray<SerializeMember> members = new LeftArray<SerializeMember>(fields.Length + properties.Length), deserializeMembers = new LeftArray<SerializeMember>(fields.Length + deserializeProperties.Length);
            foreach (var field in fields)
            {
                SerializeMember member = new SerializeMember(field.Key, field.Value);
                members.Add(member);
                deserializeMembers.Add(member);
                Type memberType = member.MemberType.Type;
                if (!memberType.IsGenericParameter) memberTypes.Add(memberType);
            }
            foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> property in properties)
            {
                SerializeMember member = new SerializeMember(property.Property, property.MemberAttribute);
                members.Add(member);
                Type memberType = member.MemberType.Type;
                if (!memberType.IsGenericParameter) memberTypes.Add(memberType);
            }
            foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> member in deserializeProperties) deserializeMembers.Add(new SerializeMember(member.Property, member.MemberAttribute));
            Members = members.ToArray();
            DeserializeMembers = deserializeMembers.ToArray();
            MemberTypes = memberTypes.getArray(p => new AotMethod.ReflectionMemberType(p.Value));
            create(true);
            AotMethod.Append(CurrentType, XmlSerializeMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 获取序列化信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeMethodInfo"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private static bool getSerializeMethodInfo(Type type, ref TextSerializeMethodInfo serializeMethodInfo, out AutoCSer.XmlSerializeAttribute attribute, out Type baseType)
        {
            attribute = XmlSerializer.AllMemberAttribute;
            baseType = null;
            if (XmlSerializer.SerializeDelegates.ContainsKey(type)) return true;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type)) return serializeMethodInfo.IsCusotm = true;
            if (type.IsArray || type.IsEnum || type.isSerializeNotSupport()) return true;
            if (type.IsGenericType && type.IsValueType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>)) return true;
                if (genericTypeDefinition == typeof(KeyValuePair<,>) || genericTypeDefinition == typeof(KeyValue<,>)) return serializeMethodInfo.IsEmptyString = true;
            }
            if (type.IsValueType || AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>)) return true;
                }
            }
            baseType = AutoCSer.Xml.Common.GetBaseAttribute(type, XmlSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return true;
            serializeMethodInfo.IsTypeSerialize = true;
            return false;
        }
        /// <summary>
        /// 成员类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> types = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 可空类型集合
        /// </summary>
        internal static readonly HashSet<HashObject<Type>> NullableElementTypes = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 成员类型集合
        /// </summary>
        /// <param name="deserializeMemberTypes"></param>
        /// <returns></returns>
        internal static LeftArray<AotMethod.ReflectionMemberType> GetReflectionMemberTypes(out LeftArray<AotMethod.ReflectionMemberType> deserializeMemberTypes)
        {
            LeftArray<AotMethod.ReflectionMemberType> memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(types.Count);
            deserializeMemberTypes = new LeftArray<AotMethod.ReflectionMemberType>(types.Count);
            foreach (HashObject<Type> type in types.getArray()) getMemberType(type.Value, ref memberTypes, ref deserializeMemberTypes);
            return memberTypes;
        }
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberTypes"></param>
        /// <param name="deserializeMemberTypes"></param>
        private static void getMemberType(Type type, ref LeftArray<AotMethod.ReflectionMemberType> memberTypes, ref LeftArray<AotMethod.ReflectionMemberType> deserializeMemberTypes)
        {
            if (XmlSerializer.SerializeDelegates.ContainsKey(type))
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type));
                if (type.IsGenericType && type.IsValueType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) NullableElementTypes.Add(type.GetGenericArguments()[0]);
                return;
            }
            AotMethod.ReflectionMemberType memberType;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.ICustom), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Array), elementType.fullName());
                        memberTypes.Add(memberType);
                        deserializeMemberTypes.Add(memberType);
                        if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                }
                memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.NotSupport), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsEnum)
            {
                string methodName = null;
                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(XmlSerializer.EnumInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(XmlSerializer.EnumUInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(XmlSerializer.EnumByte); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(XmlSerializer.EnumULong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(XmlSerializer.EnumUShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(XmlSerializer.EnumLong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(XmlSerializer.EnumShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(XmlSerializer.EnumSByte); break;
                }
                memberType = new AotMethod.ReflectionMemberType(type, methodName, type.fullName());
                memberTypes.Add(memberType);
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                    {
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(XmlDeserializer.EnumFlagsInt); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(XmlDeserializer.EnumFlagsUInt); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(XmlDeserializer.EnumFlagsByte); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(XmlDeserializer.EnumFlagsULong); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(XmlDeserializer.EnumFlagsUShort); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(XmlDeserializer.EnumFlagsLong); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(XmlDeserializer.EnumFlagsShort); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(XmlDeserializer.EnumFlagsSByte); break;
                    }
                    deserializeMemberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, type.fullName()));
                }
                else deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.isSerializeNotSupport())
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.NotSupport), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (type.IsValueType)
                {
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        Type elementType = type.GetGenericArguments()[0];
                        NullableElementTypes.Add(elementType);
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Nullable), elementType.fullName());
                        memberTypes.Add(memberType);
                        deserializeMemberTypes.Add(memberType);
                        if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                    if (genericTypeDefinition == typeof(KeyValue<,>))
                    {
                        Type[] referenceTypes = type.GetGenericArguments();
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.KeyValue), $"{referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}");
                        memberTypes.Add(memberType);
                        deserializeMemberTypes.Add(memberType);
                        foreach (var elementType in referenceTypes)
                        {
                            if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        }
                        Type binarySerializeKeyValueType = typeof(BinarySerializeKeyValue<,>).MakeGenericType(referenceTypes);
                        if (types.Add(binarySerializeKeyValueType)) getMemberType(binarySerializeKeyValueType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                    if (genericTypeDefinition == typeof(KeyValuePair<,>))
                    {
                        Type[] referenceTypes = type.GetGenericArguments();
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.KeyValuePair), $"{referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}");
                        memberTypes.Add(memberType);
                        deserializeMemberTypes.Add(memberType);
                        foreach (var elementType in referenceTypes)
                        {
                            if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        }
                        Type binarySerializeKeyValueType = typeof(BinarySerializeKeyValue<,>).MakeGenericType(referenceTypes);
                        if (types.Add(binarySerializeKeyValueType)) getMemberType(binarySerializeKeyValueType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                    if (genericTypeDefinition == typeof(LeftArray<>))
                    {
                        Type elementType = type.GetGenericArguments()[0];
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Collection), $"{type.fullName()}, {elementType.fullName()}"));
                        deserializeMemberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(XmlDeserializer.LeftArray), elementType.fullName()));
                        if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                }
                else
                {
                    if (genericTypeDefinition == typeof(ListArray<>))
                    {
                        Type elementType = type.GetGenericArguments()[0];
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Collection), $"{type.fullName()}, {elementType.fullName()}"));
                        deserializeMemberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(XmlDeserializer.ListArray), elementType.fullName()));
                        if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                }
            }
            if (type.IsValueType || AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>))
                    {
                        Type elementType = interfaceType.GetGenericArguments()[0];
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Collection), $"{type.fullName()}, {elementType.fullName()}");
                        memberTypes.Add(memberType);
                        deserializeMemberTypes.Add(memberType);
                        if (types.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        return;
                    }
                }
            }
            AutoCSer.XmlSerializeAttribute attribute = XmlSerializer.AllMemberAttribute;
            var baseType = AutoCSer.Xml.Common.GetBaseAttribute(type, XmlSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(XmlSerializer.Base), $"{type.fullName()}, {baseType.fullName()}");
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            memberTypes.Add(new AotMethod.ReflectionMemberType(type));
            return;
        }
    }
}
