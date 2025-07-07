using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    [Generator(Name = "JSON 序列化", IsAuto = true)]
    internal partial class JsonSerialize : AttributeGenerator<AutoCSer.CodeGenerator.JsonSerializeAttribute>
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
            public string EnumJsonSerializeMethodName
            {
                get { return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString(); }
            }
            /// <summary>
            /// 枚举反序列化方法名称
            /// </summary>
            public string EnumJsonDeserializeMethodName
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
            /// 序列化成员名称
            /// </summary>
            public string SerializeMemberName;
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
            /// Name of serialization method
            /// 序列化方法名称
            /// </summary>
            public string SerializeMethodName
            {
                get
                {
                    if (MemberType.Type.IsGenericParameter || MethodInfo.IsTypeSerialize) return nameof(AutoCSer.JsonSerializer.JsonSerializeType);
                    if (MemberType.IsObject) return nameof(AutoCSer.JsonSerializer.JsonSerializeObject);
                    if (MethodInfo.IsCusotm) return nameof(AutoCSer.JsonSerializer.ICustom);
                    return JsonSerializeAttribute.JsonSerializeMethodName;
                }
            }
            /// <summary>
            /// Name of deserialization method
            /// 反序列化方法名称
            /// </summary>
            public string DeserializeMethodName
            {
                get
                {
                    //if (MemberType.Type.IsGenericParameter) return "GenericParameter";
                    return JsonSerializeAttribute.JsonDeserializeMethodName;
                }
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="member"></param>
            /// <param name="memberType"></param>
            /// <param name="isField"></param>
            private SerializeMember(AutoCSer.CodeGenerator.Metadata.MemberIndex member, Type memberType, bool isField)
            {
                this.member = member;
                MemberType = memberType;
                IsField = isField;
                SerializeMemberName = "\"\"" + MemberName + "\"\":";
                if (!memberType.IsGenericParameter)
                {
                    Type baseType;
                    AutoCSer.JsonSerializeAttribute attribute;
                    getSerializeMethodInfo(memberType, ref MethodInfo, out attribute, out baseType);
                    memberTypes.Add(memberType);
                }
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="field"></param>
            public SerializeMember(AutoCSer.Metadata.FieldIndex field) : this(new AutoCSer.CodeGenerator.Metadata.FieldIndex(field), field.Member.FieldType, true)
            {
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="property"></param>
            public SerializeMember(AutoCSer.Metadata.PropertyIndex property) : this(new AutoCSer.CodeGenerator.Metadata.PropertyIndex(property), property.Member.PropertyType, false)
            {
            }
        }

        /// <summary>
        /// Name of JSON serialization method
        /// JSON 序列化方法名称
        /// </summary>
        public string JsonSerializeMethodName { get { return JsonSerializeAttribute.JsonSerializeMethodName; } }
        /// <summary>
        /// Name of JSON serialization method
        /// JSON 序列化方法名称
        /// </summary>
        public string JsonSerializeMemberMapMethodName { get { return JsonSerializeAttribute.JsonSerializeMemberMapMethodName; } }
        /// <summary>
        /// The method name of get JSON serialized member type collection
        /// 获取 JSON 序列化成员类型集合的方法名称
        /// </summary>
        public string JsonSerializeMemberTypeMethodName { get { return JsonSerializeAttribute.JsonSerializeMemberTypeMethodName; } }
        /// <summary>
        /// Name of JSON deserialization method
        /// JSON 反序列化方法名称
        /// </summary>
        public string JsonDeserializeMethodName { get { return JsonSerializeAttribute.JsonDeserializeMethodName; } }
        /// <summary>
        /// Name of JSON deserialization method
        /// JSON 反序列化方法名称
        /// </summary>
        public string JsonDeserializeMemberMapMethodName { get { return JsonSerializeAttribute.JsonDeserializeMemberMapMethodName; } }
        /// <summary>
        /// The method name of get collection of JSON deserialization member names
        /// 获取 JSON 反序列化成员名称集合的方法名称
        /// </summary>
        public string JsonDeserializeMemberNameMethodName { get { return JsonSerializeAttribute.JsonDeserializeMemberNameMethodName; } }
        /// <summary>
        /// 是否第一个成员
        /// </summary>
        private bool isFirstMember;
        /// <summary>
        /// 是否第一个成员
        /// </summary>
        public bool IsFirstMember
        {
            get
            {
                if (isFirstMember)
                {
                    isFirstMember = false;
                    return true;
                }
                return false;
            }
        }
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
        /// Collection of member types
        /// 成员类型集合
        /// </summary>
        public AotMethod.ReflectionMemberType[] MemberTypes;
        /// <summary>
        /// 成员类型数量
        /// </summary>
        public int MemberTypeCount { get { return MemberTypes.Length; } }
        /// <summary>
        /// 是否生成序列化代码
        /// </summary>
        public bool IsSerialize;
        /// <summary>
        /// 是否生成反序列化代码
        /// </summary>
        public bool IsDeserialize;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type, baseType;
            TextSerializeMethodInfo serializeMethodInfo = default(TextSerializeMethodInfo);
            AutoCSer.JsonSerializeAttribute attribute;
            if (getSerializeMethodInfo(type, ref serializeMethodInfo, out attribute, out baseType))
            {
                if (baseType != null) JsonSerialize.memberTypes.Add(type);
                return AutoCSer.Common.CompletedTask;
            }
            if (types.Add(type))
            {
                HashSet<HashObject<Type>> memberTypes = HashSetCreator.CreateHashObject<Type>();
                var fields = AutoCSer.TextSerialize.Common.GetSerializeFields<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetFields(type, attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetSerializeProperties<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type, attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> deserializeProperties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type, attribute.MemberFilters), attribute);
                LeftArray<SerializeMember> members = new LeftArray<SerializeMember>(fields.Length + properties.Length), deserializeMembers = new LeftArray<SerializeMember>(fields.Length + deserializeProperties.Length);
                foreach (var field in fields)
                {
                    SerializeMember member = new SerializeMember(field.Key);
                    members.Add(member);
                    deserializeMembers.Add(member);
                    Type memberType = member.MemberType.Type;
                    if (!memberType.IsGenericParameter) memberTypes.Add(memberType);
                }
                foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> property in properties)
                {
                    SerializeMember member = new SerializeMember(property.Property);
                    members.Add(member);
                    Type memberType = member.MemberType.Type;
                    if (!memberType.IsGenericParameter) memberTypes.Add(memberType);
                }
                foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in deserializeProperties) deserializeMembers.Add(new SerializeMember(member.Property));
                Members = members.ToArray();
                DeserializeMembers = deserializeMembers.ToArray();
                MemberTypes = memberTypes.getArray(p => new AotMethod.ReflectionMemberType(p.Value));
                isFirstMember = true;
                IsDeserialize = CurrentAttribute.IsDeserialize;
                IsSerialize = CurrentAttribute.IsSerialize;
                create(true);
                AotMethod.Append(CurrentType, JsonSerializeMethodName);
                DefaultConstructor.Create(type);
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 代码生成调用
        /// </summary>
        /// <param name="type"></param>
        internal static void Create(Type type)
        {
            JsonSerialize serialize = new JsonSerialize();
            serialize.generatorAttribute = defaultGeneratorAttribute;
            serialize.CurrentAttribute = (AutoCSer.CodeGenerator.JsonSerializeAttribute)type.GetCustomAttribute(typeof(AutoCSer.CodeGenerator.JsonSerializeAttribute)) ?? defaultJsonSerializeAttribute;
            serialize.CurrentType = type;
            serialize.nextCreate().NotWait();
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 代码生成类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> types = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 获取序列化信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeMethodInfo"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private static bool getSerializeMethodInfo(Type type, ref TextSerializeMethodInfo serializeMethodInfo, out AutoCSer.JsonSerializeAttribute attribute, out Type baseType)
        {
            attribute = JsonSerializer.AllMemberAttribute;
            baseType = null;
            if (JsonSerializer.SerializeDelegates.ContainsKey(type)) return true;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type)) return serializeMethodInfo.IsCusotm = true;
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>) || genericTypeDefinition == typeof(ListArray<>)) return true;
            }
            if (type.IsArray || type.IsEnum || type.isSerializeNotSupport()) return true;
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>) || genericTypeDefinition == typeof(Nullable<>) || genericTypeDefinition == typeof(KeyValuePair<,>)) return true;
            }

            if (type.IsValueType || AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>) || genericTypeDefinition == typeof(ICollection<>)) return true;
                }
            }
            baseType = AutoCSer.Json.Common.GetBaseAttribute(type, JsonSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return true;
            serializeMethodInfo.IsTypeSerialize = true;
            return false;
        }
        /// <summary>
        /// Collection of member types
        /// 成员类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> memberTypes = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 获取成员类型集合
        /// </summary>
        /// <param name="deserializeMemberTypes"></param>
        /// <returns></returns>
        internal static LeftArray<AotMethod.ReflectionMemberType> GetReflectionMemberTypes(out LeftArray<AotMethod.ReflectionMemberType> deserializeMemberTypes)
        {
            LeftArray<AotMethod.ReflectionMemberType> reflectionMemberTypes = new LeftArray<AotMethod.ReflectionMemberType>(memberTypes.Count);
            deserializeMemberTypes = new LeftArray<AotMethod.ReflectionMemberType>(JsonSerialize.memberTypes.Count);
            foreach (HashObject<Type> type in JsonSerialize.memberTypes.getArray()) getMemberType(type.Value, ref reflectionMemberTypes, ref deserializeMemberTypes);
            return reflectionMemberTypes;
        }
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberTypes"></param>
        /// <param name="deserializeMemberTypes"></param>
        private static void getMemberType(Type type, ref LeftArray<AotMethod.ReflectionMemberType> memberTypes, ref LeftArray<AotMethod.ReflectionMemberType> deserializeMemberTypes)
        {
            if (JsonSerializer.SerializeDelegates.ContainsKey(type))
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type));
                return;
            }
            AotMethod.ReflectionMemberType memberType;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.ICustom), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.LeftArray), elementType.fullName());
                    memberTypes.Add(memberType);
                    deserializeMemberTypes.Add(memberType);
                    if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    return;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.ListArray), elementType.fullName());
                    memberTypes.Add(memberType);
                    deserializeMemberTypes.Add(memberType);
                    if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    return;
                }
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.Array), elementType.fullName());
                        deserializeMemberTypes.Add(memberType);
                        if (elementType.isNullable())
                        {
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.NullableArray), elementType.GetGenericArguments()[0].fullName()));
                            if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        }
                        else
                        {
                            memberTypes.Add(memberType);
                            if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        }
                        return;
                    }
                }
                memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.NotSupport), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsEnum)
            {
                string methodName = null;
                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(JsonSerializer.EnumInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(JsonSerializer.EnumUInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(JsonSerializer.EnumByte); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(JsonSerializer.EnumULong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(JsonSerializer.EnumUShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(JsonSerializer.EnumLong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(JsonSerializer.EnumShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(JsonSerializer.EnumSByte); break;
                }
                memberType = new AotMethod.ReflectionMemberType(type, methodName, type.fullName());
                memberTypes.Add(memberType);
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                    {
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(JsonDeserializer.EnumFlagsInt); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(JsonDeserializer.EnumFlagsUInt); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(JsonDeserializer.EnumFlagsByte); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(JsonDeserializer.EnumFlagsULong); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(JsonDeserializer.EnumFlagsUShort); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(JsonDeserializer.EnumFlagsLong); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(JsonDeserializer.EnumFlagsShort); break;
                        case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(JsonDeserializer.EnumFlagsSByte); break;
                    }
                    deserializeMemberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, type.fullName()));
                }
                else deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.isSerializeNotSupport())
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.NotSupport), type.fullName());
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.Dictionary), $"{referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}");
                    deserializeMemberTypes.Add(memberType);
                    if (referenceTypes[0] == typeof(string)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.StringDictionary), referenceTypes[1].fullName()));
                    else memberTypes.Add(memberType);
                    foreach (var elementType in referenceTypes)
                    {
                        if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    }
                    return;
                }
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.Nullable), elementType.fullName());
                    memberTypes.Add(memberType);
                    deserializeMemberTypes.Add(memberType);
                    if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    return;
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.KeyValuePair), $"{referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}");
                    memberTypes.Add(memberType);
                    deserializeMemberTypes.Add(memberType);
                    foreach (var elementType in referenceTypes)
                    {
                        if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    }
                    return;
                }
            }
            if (type.IsValueType || AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        Type[] referenceTypes = type.GetGenericArguments();
                        memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.IDictionary), $"{type.fullName()}, {referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}");
                        deserializeMemberTypes.Add(memberType);
                        if (referenceTypes[0] == typeof(string)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.StringIDictionary), $"{type.fullName()}, {referenceTypes[1].fullName()}"));
                        else memberTypes.Add(memberType);
                        foreach (var elementType in referenceTypes)
                        {
                            if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                        }
                        return;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    Type elementType = collectionType.GetGenericArguments()[0];
                    memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.Collection), $"{type.fullName()}, {elementType.fullName()}");
                    memberTypes.Add(memberType);
                    deserializeMemberTypes.Add(memberType);
                    if (JsonSerialize.memberTypes.Add(elementType)) getMemberType(elementType, ref memberTypes, ref deserializeMemberTypes);
                    return;
                }
            }
            AutoCSer.JsonSerializeAttribute attribute = JsonSerializer.AllMemberAttribute;
            var baseType = AutoCSer.Json.Common.GetBaseAttribute(type, JsonSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                memberType = new AotMethod.ReflectionMemberType(type, nameof(JsonSerializer.Base), $"{type.fullName()}, {baseType.fullName()}");
                memberTypes.Add(memberType);
                deserializeMemberTypes.Add(memberType);
                return;
            }
            memberTypes.Add(new AotMethod.ReflectionMemberType(type));
            return;
        }
        /// <summary>
        /// 代码生成器配置
        /// </summary>
        private static readonly GeneratorAttribute defaultGeneratorAttribute = typeof(JsonSerialize).GetCustomAttribute(typeof(GeneratorAttribute)).castType<GeneratorAttribute>();
        /// <summary>
        /// Configuration for generating JSON serialization code in the AOT environment
        /// AOT 环境 JSON 序列化代码生成配置
        /// </summary>
        private static readonly AutoCSer.CodeGenerator.JsonSerializeAttribute defaultJsonSerializeAttribute = new AutoCSer.CodeGenerator.JsonSerializeAttribute();
    }
}
