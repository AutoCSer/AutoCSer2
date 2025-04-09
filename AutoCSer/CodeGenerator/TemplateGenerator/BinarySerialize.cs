using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 二进制序列化
    /// </summary>
    [Generator(Name = "二进制序列化", IsAuto = true, IsAOT = true)]
    internal partial class BinarySerialize : AttributeGenerator<AutoCSer.CodeGenerator.BinarySerializeAttribute>
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public sealed class SerializeMember
        {
            /// <summary>
            /// 成员字段
            /// </summary>
            private readonly AutoCSer.CodeGenerator.Metadata.FieldIndex field;
            /// <summary>
            /// 成员类型
            /// </summary>
            public ExtensionType MemberType;
            /// <summary>
            /// 成员编号
            /// </summary>
            public int MemberIndex { get { return field.MemberIndex; } }
            /// <summary>
            /// 成员名称
            /// </summary>
            public readonly string MemberName;
            /// <summary>
            /// 枚举数字类型
            /// </summary>
            public ExtensionType UnderlyingType
            {
                get { return System.Enum.GetUnderlyingType(MemberType.Type); }
            }
            /// <summary>
            /// 序列化方法信息
            /// </summary>
            public readonly AutoCSer.CodeGenerator.BinarySerializeMethodInfo MethodInfo;
            /// <summary>
            /// 序列化方法名称
            /// </summary>
            public string SerializeMethodName
            {
                get
                {
                    if (MemberType.Type.IsGenericParameter) return "GenericParameter";
                    if (MethodInfo.EnumArrayElementType != null) return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MethodInfo.EnumArrayElementType)).ToString();
                    if (MethodInfo.IsCusotm) return "ICustomSerialize";
                    if (MethodInfo.IsNullableArray) return nameof(AutoCSer.BinarySerializer.NullableArray);
                    if (MethodInfo.IsStructArray) return nameof(AutoCSer.BinarySerializer.StructArray);
                    if (MethodInfo.IsJson) return MemberType.Type.IsValueType ? nameof(AutoCSer.BinarySerializer.StructJson) : nameof(AutoCSer.BinarySerializer.Json);
                    if (MethodInfo.IsDictionary) return nameof(AutoCSer.BinarySerializer.Dictionary);
                    if (MethodInfo.IsCollection) return nameof(AutoCSer.BinarySerializer.Collection);
                    return AutoCSer.BinarySerializer.BinarySerializeMethodName;
                }
            }
            /// <summary>
            /// 是否存在泛型参数
            /// </summary>
            public bool IsGenericType { get { return MethodInfo.IsNullableArray || MethodInfo.IsDictionary || MethodInfo.IsCollection; } }
            /// <summary>
            /// 泛型参数名称
            /// </summary>
            public string GenericTypeName
            {
                get
                {
                    if (MethodInfo.IsNullableArray) return MethodInfo.GenericType.fullName();
                    if (MethodInfo.IsDictionary) return $"{MethodInfo.GenericType.fullName()}, {MethodInfo.GenericType1.fullName()}, {MethodInfo.GenericType2.fullName()}";
                    if (MethodInfo.IsCollection) return $"{MethodInfo.GenericType.fullName()}, {MethodInfo.GenericType1.fullName()}";
                    return null;
                }
            }
            /// <summary>
            /// 反序列化方法名称
            /// </summary>
            public string DeserializeMethodName
            {
                get
                {
                    if (MemberType.Type.IsGenericParameter) return "GenericParameter";
                    if (MemberType.Type.IsEnum) return "FixedEnum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString();
                    if (MethodInfo.EnumArrayElementType != null) return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MethodInfo.EnumArrayElementType)).ToString();
                    if (MethodInfo.IsCusotm) return "ICustomDeserialize";
                    if (MethodInfo.IsNullableArray) return nameof(AutoCSer.BinaryDeserializer.NullableArray);
                    if (MethodInfo.IsStructArray) return nameof(AutoCSer.BinaryDeserializer.StructArray);
                    if (MethodInfo.IsJson) return MemberType.Type.IsValueType ? nameof(AutoCSer.BinaryDeserializer.StructJson) : nameof(AutoCSer.BinaryDeserializer.Json);
                    if (MethodInfo.IsDictionary) return nameof(AutoCSer.BinaryDeserializer.Dictionary);
                    if (MethodInfo.IsCollection) return nameof(AutoCSer.BinaryDeserializer.Collection);
                    return AutoCSer.BinaryDeserializer.BinaryDeserializeMethodName;
                }
            }
            /// <summary>
            /// 是否属性绑定的匿名字段
            /// </summary>
            public readonly bool IsProperty;
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="field"></param>
            public SerializeMember(AutoCSer.BinarySerialize.FieldSize field)
            {
                this.field = new AutoCSer.CodeGenerator.Metadata.FieldIndex(field.FieldIndex);
                MemberType = field.Field.FieldType;
                MemberName = field.FieldIndex.AnonymousName;
                IsProperty = field.FieldIndex.AnonymousProperty != null;
                if (!MemberType.Type.IsGenericParameter) AutoCSer.BinarySerialize.Common.GetMemberSerializeDelegate(MemberType.Type, ref MethodInfo);
            }
        }

        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string FixedBinarySerializeMethodName { get { return AutoCSer.BinarySerializer.FixedBinarySerializeMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string FixedBinarySerializeMemberMapMethodName { get { return AutoCSer.BinarySerializer.FixedBinarySerializeMemberMapMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string BinarySerializeMethodName { get { return AutoCSer.BinarySerializer.BinarySerializeMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string BinarySerializeMemberMapMethodName { get { return AutoCSer.BinarySerializer.BinarySerializeMemberMapMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string FixedBinaryDeserializeMethodName { get { return AutoCSer.BinaryDeserializer.FixedBinaryDeserializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string FixedBinaryDeserializeMemberMapMethodName { get { return AutoCSer.BinaryDeserializer.FixedBinaryDeserializeMemberMapMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string BinaryDeserializeMethodName { get { return AutoCSer.BinaryDeserializer.BinaryDeserializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string BinaryDeserializeMemberMapMethodName { get { return AutoCSer.BinaryDeserializer.BinaryDeserializeMemberMapMethodName; } }
        /// <summary>
        /// 固定序列化成员
        /// </summary>
        public SerializeMember[] FixedFields;
        /// <summary>
        /// 非固定序列化成员
        /// </summary>
        public SerializeMember[] FieldArray;
        /// <summary>
        /// 是否支持成员位图
        /// </summary>
        public bool IsMemberMap;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (CurrentType.Type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            var genericType = default(AutoCSer.Metadata.GenericType);
            AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference;
            BinarySerializeMethodInfo binarySerializeMethodInfo = default(BinarySerializeMethodInfo);
            if (AutoCSer.BinarySerialize.Common.GetTypeSerializeDelegate(CurrentType.Type, ref genericType, out serializeDelegateReference, ref binarySerializeMethodInfo) || binarySerializeMethodInfo.IsJson) return AutoCSer.Common.CompletedTask;
            AutoCSer.BinarySerializeAttribute attribute = AutoCSer.BinarySerializer.DefaultAttribute;
            var type = AutoCSer.BinarySerialize.Common.GetBaseAttribute(CurrentType.Type, ref attribute);
            IsMemberMap = attribute.GetIsMemberMap(CurrentType.Type);
            int memberCountVerify;
            LeftArray<AutoCSer.Metadata.FieldIndex> fieldIndexs = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type ?? CurrentType.Type, attribute.MemberFilters);
            AutoCSer.BinarySerialize.FieldSizeArray fields = new AutoCSer.BinarySerialize.FieldSizeArray(fieldIndexs, attribute.GetIsJsonMember(CurrentType.Type), out memberCountVerify);
            //if ((fields.FixedFields.Length | fields.FieldArray.Length) == 0) return AutoCSer.Common.CompletedTask;
            FixedFields = fields.FixedFields.getArray(p => new SerializeMember(p));
            FieldArray = fields.FieldArray.getArray(p => new SerializeMember(p));
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
