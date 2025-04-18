using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.FieldEquals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 对象对比
    /// </summary>
    [Generator(Name = "对象对比", IsAuto = true, IsAOT = true)]
    internal partial class FieldEquals : AttributeGenerator<AutoCSer.CodeGenerator.FieldEqualsAttribute>
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public sealed class EqualsMember
        {
            /// <summary>
            /// 成员类型
            /// </summary>
            public readonly ExtensionType MemberType;
            /// <summary>
            /// 成员名称
            /// </summary>
            public readonly string MemberName;
            /// <summary>
            /// 成员索引
            /// </summary>
            public readonly int MemberIndex;
            /// <summary>
            /// 对象对比方法名称
            /// </summary>
            public readonly string EqualsMethodName;
            /// <summary>
            /// 泛型参数名称
            /// </summary>
            public readonly string GenericTypeName;
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="member"></param>
            public EqualsMember(Member member)
            {
                MemberType = member.Field.FieldType;
                MemberName = member.NameMember.Name;
                EqualsMethodName = getEqualsMethodName(MemberType.Type, out GenericTypeName) ?? nameof(Comparor.CallEquals);
            }
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="member"></param>
            public EqualsMember(AutoCSer.Metadata.FieldIndex member)
            {
                MemberType = member.Member.FieldType;
                MemberName = member.AnonymousName;
                MemberIndex = member.MemberIndex;
                EqualsMethodName = getEqualsMethodName(MemberType.Type, out GenericTypeName) ?? nameof(Comparor.CallEquals);
            }
            /// <summary>
            /// 获取成员类型
            /// </summary>
            /// <param name="type"></param>
            /// <param name="genericTypeName"></param>
            private static string getEqualsMethodName(Type type, out string genericTypeName)
            {
                types.Add(type);
                if (typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type))
                {
                    genericTypeName = null;
                    if (type == typeof(float) || type == typeof(double)) return nameof(Comparor.Equals);
                    return type.IsValueType ? nameof(Comparor.EquatableEquals) : nameof(Comparor.ReferenceEquals);
                }
                if (type.IsArray)
                {
                    if (type.GetArrayRank() == 1)
                    {
                        genericTypeName = type.GetElementType().fullName();
                        return nameof(Comparor.ArrayEquals);
                    }
                    return genericTypeName = null;
                }
                if (type.IsEnum)
                {
                    genericTypeName = type.fullName();
                    return "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)).ToString();
                }
                if (type.isValueTypeNullable())
                {
                    genericTypeName = type.GetGenericArguments()[0].fullName();
                    return nameof(Comparor.NullableEquals);
                }
                if (type.IsValueType && type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(KeyValuePair<,>))
                    {
                        Type[] typeArray = type.GetGenericArguments();
                        genericTypeName = $"{typeArray[0].fullName()}, {typeArray[1].fullName()}";
                        return nameof(Comparor.KeyValuePairEquals);
                    }
                    if (genericType == typeof(KeyValue<,>))
                    {
                        Type[] typeArray = type.GetGenericArguments();
                        genericTypeName = $"{typeArray[0].fullName()}, {typeArray[1].fullName()}";
                        return nameof(Comparor.KeyValueEquals);
                    }
                }
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericType = interfaceType.GetGenericTypeDefinition();
                    if (genericType == typeof(IDictionary<,>))
                    {
                        Type[] typeArray = interfaceType.GetGenericArguments();
                        genericTypeName = $"{type.fullName()}, {typeArray[0].fullName()}, {typeArray[1].fullName()}";
                        return nameof(Comparor.DictionaryEquals);
                    }
                    if (collectionType == null && genericType == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    genericTypeName = $"{type.fullName()}, {collectionType.GetGenericArguments()[0].fullName()}";
                    return nameof(Comparor.CollectionEquals);
                }
                genericTypeName = null;
                if (type.IsPointer || type.IsInterface) return null;
                if (type == typeof(object)) return nameof(Comparor.ObjectEquals);
                return null;
            }
        }

        /// <summary>
        /// 对象对比方法名称
        /// </summary>
        public string FieldEqualsMethodName { get { return Comparor.FieldEqualsMethodName; } }
        /// <summary>
        /// 对象对比方法名称
        /// </summary>
        public string MemberMapFieldEqualsMethodName { get { return Comparor.MemberMapFieldEqualsMethodName; } }
        /// <summary>
        /// 成员集合
        /// </summary>
        public EqualsMember[] Fields;
        /// <summary>
        /// 成员集合
        /// </summary>
        public EqualsMember[] MemberMapFields;
        /// <summary>
        /// 基类型
        /// </summary>
        public ExtensionType BaseType;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type)) return AutoCSer.Common.CompletedTask;
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>) || genericType == typeof(ICollection<>)) return AutoCSer.Common.CompletedTask;
            }
            if (type == typeof(object)) return AutoCSer.Common.CompletedTask;
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            LeftArray<AutoCSer.Metadata.FieldIndex> memberMapFields = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type, AutoCSer.Metadata.MemberFiltersEnum.InstanceField);
            Fields = Comparor.GetFieldEqualsFields(fields).Select(p => new EqualsMember(p)).ToArray();
            MemberMapFields = memberMapFields.Select(p => new EqualsMember(p)).ToArray();
            BaseType = null;
            if (!type.IsValueType)
            {
                var baseType = type.BaseType;
                if (baseType != typeof(object))
                {
                    if (baseType.IsDefined(typeof(AutoCSer.CodeGenerator.FieldEqualsAttribute), false)) BaseType = baseType;
                    else Messages.Message($"{baseType.fullName()} 缺少自定义属性 {typeof(AutoCSer.CodeGenerator.FieldEqualsAttribute).fullName()}");
                }
            }
            create(true);
            AotMethod.Append(CurrentType, FieldEqualsMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

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
            LeftArray<AotMethod.ReflectionMemberType> memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(0);
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
            if (typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                if (type == typeof(float) || type == typeof(double))
                {
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.Equals)));
                    return;
                }
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, type.IsValueType ? nameof(Comparor.EquatableEquals) : nameof(Comparor.ReferenceEquals), type.fullName()));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.ArrayEquals), elementType.fullName()));
                    if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                }
                return;
            }
            if (type.IsEnum)
            {
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, "Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)).ToString(), type.fullName()));
                return;
            }
            if (type.isValueTypeNullable())
            {
                Type elementType = type.GetGenericArguments()[0];
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.NullableEquals), elementType.fullName()));
                if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                return;
            }
            if (type.IsValueType && type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(KeyValuePair<,>))
                {
                    Type[] typeArray = type.GetGenericArguments();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.KeyValuePairEquals), $"{typeArray[0].fullName()}, {typeArray[1].fullName()}"));
                    foreach (Type elementType in typeArray)
                    {
                        if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                    }
                    return;
                }
                if (genericType == typeof(KeyValue<,>))
                {
                    Type[] typeArray = type.GetGenericArguments();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.KeyValueEquals), $"{typeArray[0].fullName()}, {typeArray[1].fullName()}"));
                    foreach (Type elementType in typeArray)
                    {
                        if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                    }
                    return;
                }
            }
            var collectionType = default(Type);
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>))
                {
                    Type[] typeArray = interfaceType.GetGenericArguments();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.DictionaryEquals), $"{type.fullName()}, {typeArray[0].fullName()}, {typeArray[1].fullName()}"));
                    foreach (Type elementType in typeArray)
                    {
                        if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                    }
                    return;
                }
                if (collectionType == null && genericType == typeof(ICollection<>)) collectionType = interfaceType;
            }
            if (collectionType != null)
            {
                Type elementType = collectionType.GetGenericArguments()[0];
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.CollectionEquals), $"{type.fullName()}, {elementType.fullName()}"));
                if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                return;
            }
            if (type.IsPointer || type.IsInterface) return;
            if (type == typeof(object))
            {
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.ObjectEquals)));
                return;
            }
            if (types.Add(type) && type.IsDefined(typeof(AutoCSer.CodeGenerator.FieldEqualsAttribute))) memberTypes.Add(new AotMethod.ReflectionMemberType(type, nameof(Comparor.CallEquals)));
            return;
        }
    }
}
