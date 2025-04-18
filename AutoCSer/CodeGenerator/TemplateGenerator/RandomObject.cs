using AutoCSer.CodeGenerator.Extensions;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.RandomObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 随机对象生成
    /// </summary>
    [Generator(Name = "随机对象生成", IsAuto = true, IsAOT = true)]
    internal partial class RandomObject : AttributeGenerator<AutoCSer.CodeGenerator.RandomObjectAttribute>
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public sealed class RandomMember
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
            /// 允许 null 值
            /// </summary>
            public readonly bool IsNullable;
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="member"></param>
            public RandomMember(Member member)
            {
                MemberType = member.Field.FieldType;
                MemberName = member.NameMember.Name;
                IsNullable = member.Attribute.IsNullable;
                if(!MemberType.Type.isGenericParameter()) types.Add(MemberType.Type);
            }
        }

        /// <summary>
        /// 随机对象生成方法名称方法名称
        /// </summary>
        public string CreateRandomObjectMethodName { get { return Creator.CreateRandomObjectMethodName; } }
        /// <summary>
        /// 成员集合
        /// </summary>
        public RandomMember[] Fields;
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
            if (Creator.GetDelegate(type) != null) return AutoCSer.Common.CompletedTask;
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>) || genericType == typeof(ICollection<>)) return AutoCSer.Common.CompletedTask;
            }
            //if (!AutoCSer.Metadata.GenericType.Get(type).IsSerializeConstructor) return AutoCSer.Common.CompletedTask;
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Fields = Creator.GetRandomObjectFields(fields).Select(p => new RandomMember(p)).ToArray();
            BaseType = null;
            if (!type.IsValueType)
            {
                var baseType = type.BaseType;
                if (baseType != typeof(object))
                {
                    if (baseType.IsDefined(typeof(AutoCSer.CodeGenerator.RandomObjectAttribute), false)) BaseType = baseType;
                    else Messages.Message($"{baseType.fullName()} 缺少自定义属性 {typeof(AutoCSer.CodeGenerator.RandomObjectAttribute).fullName()}");
                }
            }
            create(true);
            AotMethod.Append(CurrentType, CreateRandomObjectMethodName);
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
            HashObject<Type>[] typeArray = types.getArray();
            types.Clear();
            foreach (HashObject<Type> type in typeArray) getMemberType(type.Value, ref memberTypes);
            return memberTypes;
        }
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="types"></param>
        /// <param name="memberTypes"></param>
        private static void getMemberType(Type type, ref LeftArray<AotMethod.ReflectionMemberType> memberTypes)
        {
            var createDelegate = Creator.GetDelegate(type);
            if (createDelegate != null)
            {
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(createDelegate.Method.Name));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateArray), elementType.fullName()));
                    if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                }
                return;
            }
            if (type.IsEnum)
            {
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType("Enum" + AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)).ToString(), type.fullName()));
                return;
            }
            if (type.isValueTypeNullable())
            {
                Type elementType = type.GetGenericArguments()[0];
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateNullable), elementType.fullName()));
                if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                return;
            }
            if (type.IsValueType && type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(KeyValuePair<,>))
                {
                    Type[] typeArray = type.GetGenericArguments();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateKeyValuePair), $"{typeArray[0].fullName()}, {typeArray[1].fullName()}"));
                    foreach (Type elementType in typeArray)
                    {
                        if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                    }
                    return;
                }
                if (genericType == typeof(KeyValue<,>))
                {
                    Type[] typeArray = type.GetGenericArguments();
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateKeyValue), $"{typeArray[0].fullName()}, {typeArray[1].fullName()}"));
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
                    if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateDictionary), $"{type.fullName()}, {typeArray[0].fullName()}, {typeArray[1].fullName()}"));
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
                if (types.Add(type)) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CreateCollection), $"{type.fullName()}, {elementType.fullName()}"));
                if (!types.Contains(elementType)) getMemberType(elementType, ref memberTypes);
                return;
            }
            if (types.Add(type) && type.IsDefined(typeof(AutoCSer.CodeGenerator.RandomObjectAttribute))) memberTypes.Add(new AotMethod.ReflectionMemberType(nameof(Creator.CallCreate), type.fullName()));
            return;
        }
    }
}
