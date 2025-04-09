using AutoCSer.CodeGenerator.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    [Generator(Name = "JSON 序列化", IsAuto = true, IsAOT = true)]
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
            /// 成员反序列化方法名称
            /// </summary>
            public string MemberJsonDeserializeMethodName { get { return AutoCSer.JsonDeserializer.JsonDeserializeMethodName + MemberName; } }
            /// <summary>
            /// 序列化成员名称
            /// </summary>
            public string SerializeMemberName;
            /// <summary>
            /// 成员编号
            /// </summary>
            public int MemberIndex { get { return member.MemberIndex; } }
            /// <summary>
            /// 是否成员字段
            /// </summary>
            public readonly bool IsField;
            /// <summary>
            /// JSON 序列化信息
            /// </summary>
            public readonly JsonSerializeMethodInfo MethodInfo;
            /// <summary>
            /// 序列化方法名称
            /// </summary>
            public string SerializeMethodName
            {
                get
                {
                    if (MemberType.IsObject) return nameof(AutoCSer.JsonSerializer.JsonSerializeObject);
                    if (MethodInfo.IsTypeSerialize) return nameof(AutoCSer.JsonSerializer.JsonSerializeType);
                    if (MethodInfo.IsCusotm) return "ICustomSerialize";
                    return AutoCSer.JsonSerializer.JsonSerializeMethodName;
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
                var genericType = default(AutoCSer.Metadata.GenericType);
                AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                SerializeMemberName = "\"\"" + MemberName + "\"\":";
                AutoCSer.Json.Common.GetTypeSerializeDelegate(memberType, ref genericType, out serializeDelegateReference, ref MethodInfo);
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
        /// 序列化方法名称
        /// </summary>
        public string JsonSerializeMethodName { get { return AutoCSer.JsonSerializer.JsonSerializeMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string JsonSerializeMemberMapMethodName { get { return AutoCSer.JsonSerializer.JsonSerializeMemberMapMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string JsonDeserializeMethodName { get { return AutoCSer.JsonDeserializer.JsonDeserializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string JsonDeserializeMemberMapMethodName { get { return AutoCSer.JsonDeserializer.JsonDeserializeMemberMapMethodName; } }
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
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (CurrentType.Type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            var genericType = default(AutoCSer.Metadata.GenericType);
            AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
            JsonSerializeMethodInfo jsonSerializeMethodInfo = default(JsonSerializeMethodInfo);
            if (AutoCSer.Json.Common.GetTypeSerializeDelegate(CurrentType.Type, ref genericType, out serializeDelegateReference, ref jsonSerializeMethodInfo)) return AutoCSer.Common.CompletedTask;
            AutoCSer.JsonSerializeAttribute attribute = JsonSerializer.AllMemberAttribute;
            var type = AutoCSer.Json.Common.GetBaseAttribute(CurrentType.Type, ref attribute);
            var fields = AutoCSer.TextSerialize.Common.GetSerializeFields<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetFields(type ?? CurrentType.Type, attribute.MemberFilters), attribute);
            LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetSerializeProperties<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type ?? CurrentType.Type, attribute.MemberFilters), attribute);
            LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> deserializeProperties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<JsonSerializeMemberAttribute>(AutoCSer.Metadata.MemberIndexGroup.GetProperties(type ?? CurrentType.Type, attribute.MemberFilters), attribute);
            LeftArray<SerializeMember> members = new LeftArray<SerializeMember>(fields.Length + properties.Length), deserializeMembers = new LeftArray<SerializeMember>(fields.Length + deserializeProperties.Length);
            foreach (var field in fields)
            {
                SerializeMember member = new SerializeMember(field.Key);
                members.Add(member);
                deserializeMembers.Add(member);
            }
            foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in properties) members.Add(new SerializeMember(member.Property));
            foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in deserializeProperties) deserializeMembers.Add(new SerializeMember(member.Property));
            Members = members.ToArray();
            DeserializeMembers = deserializeMembers.ToArray();
            isFirstMember = true;
            create(true);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
