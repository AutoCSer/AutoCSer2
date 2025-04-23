using AutoCSer.BinarySerialize;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 二进制序列化
    /// </summary>
    [Generator(Name = "二进制序列化", IsAuto = true)]
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
                    if (MethodInfo.EnumArrayElementType != null) return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MethodInfo.EnumArrayElementType)).ToString();
                    if (MethodInfo.IsCusotm) return "ICustomSerialize";
                    if (MethodInfo.IsNullableArray) return nameof(AutoCSer.BinarySerializer.NullableArray);
                    if (MethodInfo.IsStructArray) return nameof(AutoCSer.BinarySerializer.StructArray);
                    if (MethodInfo.IsJson) return MemberType.Type.IsValueType ? nameof(AutoCSer.BinarySerializer.StructJson) : nameof(AutoCSer.BinarySerializer.Json);
                    if (MethodInfo.IsDictionary) return nameof(AutoCSer.BinarySerializer.Dictionary);
                    if (MethodInfo.IsCollection) return nameof(AutoCSer.BinarySerializer.Collection);
                    return AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMethodName;
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
                    if (MemberType.Type.IsEnum) return "FixedEnum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MemberType.Type)).ToString();
                    if (MethodInfo.EnumArrayElementType != null) return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(MethodInfo.EnumArrayElementType)).ToString();
                    if (MethodInfo.IsCusotm) return "ICustomDeserialize";
                    if (MethodInfo.IsNullableArray) return nameof(AutoCSer.BinaryDeserializer.NullableArray);
                    if (MethodInfo.IsStructArray) return nameof(AutoCSer.BinaryDeserializer.StructArray);
                    if (MethodInfo.IsJson) return MemberType.Type.IsValueType ? nameof(AutoCSer.BinaryDeserializer.StructJson) : nameof(AutoCSer.BinaryDeserializer.Json);
                    if (MethodInfo.IsDictionary) return nameof(AutoCSer.BinaryDeserializer.Dictionary);
                    if (MethodInfo.IsCollection) return nameof(AutoCSer.BinaryDeserializer.Collection);
                    return AutoCSer.CodeGenerator.BinarySerializeAttribute.BinaryDeserializeMethodName;
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
            /// <param name="memberTypes"></param>
            public SerializeMember(AutoCSer.BinarySerialize.FieldSize field, HashSet<HashObject<Type>> memberTypes)
            {
                this.field = new AutoCSer.CodeGenerator.Metadata.FieldIndex(field.FieldIndex);
                MemberType = field.Field.FieldType;
                MemberName = field.FieldIndex.AnonymousName;
                IsProperty = field.FieldIndex.AnonymousProperty != null;
                var baseType = default(Type);
                AutoCSer.BinarySerializeAttribute attribute = default(AutoCSer.BinarySerializeAttribute);
                getSerializeMethodInfo(MemberType.Type, ref MethodInfo, out attribute, out baseType);
                types.Add(MemberType.Type);
                memberTypes.Add(MemberType.Type);
                if (baseType != null && baseType.IsGenericType) genericTypes[baseType] = true;
                if (MemberType.Type.IsGenericType) genericTypes[MemberType.Type] = MethodInfo.IsGenericReflection;
            }
        }

        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string BinarySerializeMethodName { get { return BinarySerializeAttribute.BinarySerializeMethodName; } }
        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string BinarySerializeMemberMapMethodName { get { return BinarySerializeAttribute.BinarySerializeMemberMapMethodName; } }
        /// <summary>
        /// 获取二进制序列化成员类型方法名称
        /// </summary>
        public string BinarySerializeMemberTypeMethodName { get { return BinarySerializeAttribute.BinarySerializeMemberTypeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string BinaryDeserializeMethodName { get { return BinarySerializeAttribute.BinaryDeserializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string BinaryDeserializeMemberMapMethodName { get { return BinarySerializeAttribute.BinaryDeserializeMemberMapMethodName; } }
        /// <summary>
        /// 获取二进制序列化成员数量信息方法名称
        /// </summary>
        public string BinarySerializeMemberCountVerifyMethodName { get { return BinarySerializeAttribute.BinarySerializeMemberCountVerifyMethodName; } }
        /// <summary>
        /// 固定序列化成员
        /// </summary>
        public SerializeMember[] FixedFields;
        /// <summary>
        /// 非固定序列化成员
        /// </summary>
        public SerializeMember[] FieldArray;
        /// <summary>
        /// 成员类型集合
        /// </summary>
        public AotMethod.ReflectionMemberType[] MemberTypes;
        /// <summary>
        /// 成员类型数量
        /// </summary>
        public int MemberTypeCount { get { return MemberTypes.Length; } }
        /// <summary>
        /// 固定字节数
        /// </summary>
        public int FixedSize;
        /// <summary>
        /// 固定不全字节数
        /// </summary>
        public int FixedFillSize;
        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCountVerify;
        /// <summary>
        /// 是否支持成员位图
        /// </summary>
        public bool IsMemberMap;
        /// <summary>
        /// 成员位图模式是否需要补全字节数
        /// </summary>
        public bool IsMemberMapFixedFillSize;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeFullName;
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
            if (type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            AutoCSer.CodeGenerator.BinarySerializeMethodInfo binarySerializeMethodInfo = default(AutoCSer.CodeGenerator.BinarySerializeMethodInfo);
            AutoCSer.BinarySerializeAttribute attribute = default(AutoCSer.BinarySerializeAttribute);
            if (getSerializeMethodInfo(type, ref binarySerializeMethodInfo, out attribute, out baseType))
            {
                types.Add(type);
                if (baseType != null && baseType.IsGenericType) genericTypes[baseType] = true;
                return AutoCSer.Common.CompletedTask;
            }
            IsMemberMap = attribute.GetIsMemberMap(type);
            IsSerialize = CurrentAttribute.IsSerialize;
            IsDeserialize = CurrentAttribute.IsDeserialize;
            TypeFullName = CurrentType.FullName;
            nextCreate(true, attribute);
            AotMethod.Append(CurrentType, BinarySerializeMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOutput"></param>
        /// <param name="attribute"></param>
        private void nextCreate(bool isOutput, AutoCSer.BinarySerializeAttribute attribute)
        {
            Type type = CurrentType.Type;
            HashSet<HashObject<Type>> memberTypes = HashSetCreator.CreateHashObject<Type>();
            LeftArray<AutoCSer.Metadata.FieldIndex> fieldIndexs = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters);
            AutoCSer.BinarySerialize.FieldSizeArray fields = new AutoCSer.BinarySerialize.FieldSizeArray(fieldIndexs, attribute.GetIsJsonMember(type), out MemberCountVerify);
            IsMemberMapFixedFillSize = (fields.AnyFixedSize & 3) != 0;
            FixedFillSize = -fields.FixedSize & 3;
            FixedSize = (fields.FixedSize + (sizeof(int) + 3)) & (int.MaxValue - 3);
            FixedFields = fields.FixedFields.getArray(p => new SerializeMember(p, memberTypes));
            FieldArray = fields.FieldArray.getArray(p => new SerializeMember(p, memberTypes));
            MemberTypes = memberTypes.getArray(p => new AotMethod.ReflectionMemberType(p.Value));
            create(isOutput);
        }
        /// <summary>
        /// 生成自定义类型代码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeFullName"></param>
        /// <param name="isSerialize"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        internal string Create(Type type, string typeFullName, bool isSerialize, bool isDeserialize)
        {
            CurrentType = type;
            TypeFullName = typeFullName;
            IsSerialize = isSerialize;
            IsDeserialize = isDeserialize;
            IsMemberMap = false;
            _code_.Array.Length = 0;
            nextCreate(false, BinarySerializer.DefaultAttribute);
            AotMethod.Append($"{typeFullName}.{BinarySerializeMethodName}");
            return string.Concat(_code_.Array.ToArray());
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
        /// <param name="binarySerializeMethodInfo"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private static bool getSerializeMethodInfo(Type type, ref AutoCSer.CodeGenerator.BinarySerializeMethodInfo binarySerializeMethodInfo, out AutoCSer.BinarySerializeAttribute attribute, out Type baseType)
        {
            attribute = BinarySerializer.DefaultAttribute;
            baseType = null;
            if (BinarySerializer.SerializeDelegates.ContainsKey(type)) return true;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type)) return binarySerializeMethodInfo.IsCusotm = true;
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            binarySerializeMethodInfo.EnumArrayElementType = elementType;
                            return true;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            binarySerializeMethodInfo.GenericType = elementType.GetGenericArguments()[0];
                            return binarySerializeMethodInfo.IsNullableArray = true;
                        }
                        binarySerializeMethodInfo.IsStructArray = true;
                    }
                    return true;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            binarySerializeMethodInfo.EnumArrayElementType = elementType;
                            return true;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            binarySerializeMethodInfo.GenericType = elementType.GetGenericArguments()[0];
                            return binarySerializeMethodInfo.IsNullableArray = true;
                        }
                        binarySerializeMethodInfo.IsStructArray = true;
                    }
                    return true;
                }
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum) binarySerializeMethodInfo.EnumArrayElementType = elementType;
                            else if (elementType.isValueTypeNullable())
                            {
                                binarySerializeMethodInfo.IsNullableArray = true;
                                binarySerializeMethodInfo.GenericType = elementType.GetGenericArguments()[0];
                            }
                            else binarySerializeMethodInfo.IsStructArray = true;
                        }
                        return true;
                    }
                }
                return true;
            }
            if (type.IsEnum) return true;
            if (type.IsAbstract || type.isSerializeNotSupport()) return true;
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>)) return true;
            }
            if (type.IsValueType || AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        binarySerializeMethodInfo.SetDictionary(type, interfaceType);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    binarySerializeMethodInfo.SetCollection(type, collectionType);
                    return true;
                }
            }
            baseType = AutoCSer.BinarySerialize.Common.GetBaseAttribute(type, ref attribute);
            if (baseType != null) return binarySerializeMethodInfo.IsGenericReflection = true;
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsMixJsonSerialize) return binarySerializeMethodInfo.IsJson = true;
            binarySerializeMethodInfo.IsGenericReflection = true;
            return false;
        }
        /// <summary>
        /// 泛型成员类型集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, bool> genericTypes = DictionaryCreator.CreateHashObject<Type, bool>();
        /// <summary>
        /// 泛型成员类型集合
        /// </summary>
        /// <param name="memberTypes"></param>
        /// <returns></returns>
        internal static LeftArray<Type> GetGenericMemberTypes(out LeftArray<AotMethod.ReflectionMemberType> memberTypes)
        {
            memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(genericTypes.Count);
            foreach (Type type in genericTypes.Where(p => p.Value).Select(p => p.Key.Value).getLeftArray()) getGenericMemberType(type, ref memberTypes);
            return genericTypes.Where(p => p.Value).Select(p => p.Key.Value).getLeftArray();
        }
        /// <summary>
        /// 添加泛型成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool appendGenericType(Type type)
        {
            if (type.IsGenericType && !genericTypes.ContainsKey(type))
            {
                var baseType = default(Type);
                AutoCSer.BinarySerializeAttribute attribute;
                AutoCSer.CodeGenerator.BinarySerializeMethodInfo methodInfo = default(AutoCSer.CodeGenerator.BinarySerializeMethodInfo);
                getSerializeMethodInfo(type, ref methodInfo, out attribute, out baseType);
                genericTypes.Add(type, methodInfo.IsGenericReflection);
                return methodInfo.IsGenericReflection;
            }
            return false;
        }
        /// <summary>
        /// 获取泛型成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberTypes"></param>
        private static void getGenericMemberType(Type type, ref LeftArray<AotMethod.ReflectionMemberType> memberTypes)
        {
            if (BinarySerializer.SerializeDelegates.ContainsKey(type))
            {
                //memberTypes.Add(new AotMethod.ReflectionMemberType(type));
                return;
            }
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.ICustom) + "Reflection", type.fullName()));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                string methodName = string.Empty;
                                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteArray); break;
                                }
                                memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName + "Reflection", elementType.fullName()));
                                return;
                            }
                            if (elementType.isValueTypeNullable())
                            {
                                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableArray) + "Reflection", elementType.GetGenericArguments()[0].fullName()));
                                return;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructArray) + "Reflection", elementType.fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Array) + "Reflection", elementType.fullName()));
                        return;
                    }
                }
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NotSupport) + "Reflection", type.fullName()));
                return;
            }
            if (type.IsEnum)
            {
                //string methodName = string.Empty;
                //switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                //{
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumInt); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUInt); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByte); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.PrimitiveMemberULongReflection); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShort); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLong); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShort); break;
                //    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByte); break;
                //}
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, $"PrimitiveMember{AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)).ToString()}Reflection", type.fullName()));
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            string methodName = string.Empty;
                            switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteLeftArray); break;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName + "Reflection", elementType.fullName()));
                            return;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableLeftArray) + "Reflection", elementType.GetGenericArguments()[0].fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructLeftArray) + "Reflection", elementType.fullName()));
                        return;
                    }
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.LeftArray) + "Reflection", elementType.fullName()));
                    return;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            string methodName = string.Empty;
                            switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteListArray); break;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName + "Reflection", elementType.fullName()));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableListArray) + "Reflection", elementType.GetGenericArguments()[0].fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructListArray) + "Reflection", elementType.fullName()));
                        return;
                    }
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.ListArray) + "Reflection", elementType.fullName()));
                    return;
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NotSupport) + "Reflection", type.fullName()));
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Nullable) + "Reflection", elementType.fullName()));
                    if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
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
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Dictionary) + "Reflection", $"{type.fullName()}, {referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}"));
                        foreach (var elementType in referenceTypes)
                        {
                            if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
                        }
                        return;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    Type elementType = collectionType.GetGenericArguments()[0];
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Collection) + "Reflection", $"{type.fullName()}, {elementType.fullName()}"));
                    if (appendGenericType(elementType)) getGenericMemberType(elementType, ref memberTypes);
                    return;
                }
            }
            AutoCSer.BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            var baseType = AutoCSer.BinarySerialize.Common.GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Base) + "Reflection", $"{type.fullName()}, {baseType.fullName()}"));
                if (appendGenericType(baseType)) getGenericMemberType(baseType, ref memberTypes);
                return;
            }
            if (attribute.IsMixJsonSerialize)
            {
                if (type.IsValueType) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructJson) + "Reflection", type.fullName()));
                else memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Json) + "Reflection", type.fullName()));
                return;
            }
            int memberCountVerify = 0;
            LeftArray<AutoCSer.Metadata.FieldIndex> fieldIndexs = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters);
            AutoCSer.BinarySerialize.FieldSizeArray fields = new AutoCSer.BinarySerialize.FieldSizeArray(fieldIndexs, attribute.GetIsJsonMember(type), out memberCountVerify);
            foreach (AutoCSer.BinarySerialize.FieldSize field in fields.FieldArray)
            {
                Type memberType = field.Field.FieldType;
                if (appendGenericType(memberType) || genericTypes.TryAdd(memberType, false)) getGenericMemberType(memberType, ref memberTypes);
            }
            //memberTypes.Add(new AotMethod.ReflectionMemberType(type));
            return;
        }
        /// <summary>
        /// 成员类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> types = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 成员类型集合
        /// </summary>
        /// <returns></returns>
        internal static LeftArray<AotMethod.ReflectionMemberType> GetReflectionMemberTypes()
        {
            LeftArray<AotMethod.ReflectionMemberType> memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(types.Count);
            foreach (HashObject<Type> type in types.getArray()) getMemberType(type.Value, ref memberTypes);
            return memberTypes;
        }
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberTypes"></param>
        private static void getMemberType(Type type, ref LeftArray<AotMethod.ReflectionMemberType> memberTypes)
        {
            if (BinarySerializer.SerializeDelegates.ContainsKey(type))
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type));
                return;
            }
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.ICustom), type.fullName()));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                string methodName = string.Empty;
                                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortArray); break;
                                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteArray); break;
                                }
                                memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, elementType.fullName()));
                                return;
                            }
                            if (elementType.isValueTypeNullable())
                            {
                                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableArray), elementType.GetGenericArguments()[0].fullName()));
                                return;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructArray), elementType.fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Array), elementType.fullName()));
                        return;
                    }
                }
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NotSupport), type.fullName()));
                return;
            }
            if (type.IsEnum)
            {
                string methodName = string.Empty;
                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUInt); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByte); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLong); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShort); break;
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByte); break;
                }
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, type.fullName()));
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            string methodName = string.Empty;
                            switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortLeftArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteLeftArray); break;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, elementType.fullName()));
                            return;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableLeftArray), elementType.GetGenericArguments()[0].fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructLeftArray), elementType.fullName()));
                        return;
                    }
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.LeftArray), elementType.fullName()));
                    return;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            string methodName = string.Empty;
                            switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Int: methodName = nameof(BinarySerializer.EnumIntListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: methodName = nameof(BinarySerializer.EnumUIntListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: methodName = nameof(BinarySerializer.EnumByteListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: methodName = nameof(BinarySerializer.EnumULongListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: methodName = nameof(BinarySerializer.EnumUShortListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Long: methodName = nameof(BinarySerializer.EnumLongListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.Short: methodName = nameof(BinarySerializer.EnumShortListArray); break;
                                case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: methodName = nameof(BinarySerializer.EnumSByteListArray); break;
                            }
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, methodName, elementType.fullName()));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NullableListArray), elementType.GetGenericArguments()[0].fullName()));
                            return;
                        }
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructListArray), elementType.fullName()));
                        return;
                    }
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.ListArray), elementType.fullName()));
                    return;
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.NotSupport), type.fullName()));
                return;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Nullable), elementType.fullName()));
                    if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
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
                        memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Dictionary), $"{type.fullName()}, {referenceTypes[0].fullName()}, {referenceTypes[1].fullName()}"));
                        foreach (var elementType in referenceTypes)
                        {
                            if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
                        }
                        return;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    Type elementType = collectionType.GetGenericArguments()[0];
                    memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Collection), $"{type.fullName()}, {elementType.fullName()}"));
                    if (types.Add(elementType)) getMemberType(elementType, ref memberTypes);
                    return;
                }
            }
            AutoCSer.BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            var baseType = AutoCSer.BinarySerialize.Common.GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Base), $"{type.fullName()}, {baseType.fullName()}"));
                if (types.Add(baseType)) getMemberType(baseType, ref memberTypes);
                return;
            }
            if (attribute.IsMixJsonSerialize)
            {
                if (type.IsValueType) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.StructJson), type.fullName()));
                else memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(BinarySerializer.Json), type.fullName()));
                return;
            }
            memberTypes.Add(new AotMethod.ReflectionMemberType(type));
            return;
        }
    }
}
