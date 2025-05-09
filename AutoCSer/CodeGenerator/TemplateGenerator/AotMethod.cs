using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 触发 AOT 编译调用方法
    /// </summary>
    [Generator(Name = "触发 AOT 编译调用方法")]
    internal partial class AotMethod : Generator, IGenerator
    {
        /// <summary>
        /// 触发 AOT 编译方法信息
        /// </summary>
        internal sealed class Method
        {
            /// <summary>
            /// 调用类型
            /// </summary>
            public readonly ExtensionType MemberType;
            /// <summary>
            /// 调用方法名称
            /// </summary>
            public readonly string MethodName;
            /// <summary>
            /// 触发 AOT 编译方法信息
            /// </summary>
            /// <param name="memberType"></param>
            /// <param name="methodName"></param>
            internal Method(ExtensionType memberType, string methodName)
            {
                MemberType = memberType;
                MethodName = methodName;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        internal sealed class ReflectionMemberType
        {
            /// <summary>
            /// 当前分配类型成员编号
            /// </summary>
            private static int currentMemberIndex;

            /// <summary>
            /// 成员类型
            /// </summary>
            public readonly ExtensionType MemberType;
            /// <summary>
            /// 对象对比方法名称
            /// </summary>
            public readonly string ReflectionMethodName;
            /// <summary>
            /// 泛型参数名称
            /// </summary>
            public readonly string GenericTypeName;
            /// <summary>
            /// 类型成员编号
            /// </summary>
            private int memberIndex;
            /// <summary>
            /// 成员名称
            /// </summary>
            public string MemberName
            {
                get
                {
                    if (memberIndex == 0) memberIndex = ++currentMemberIndex;
                    return "t" + memberIndex.toString();
                }
            }
            /// <summary>
            /// 是否 JSON 泛型实例调用
            /// </summary>
            internal bool IsJsonSerializeMethodName
            {
                get
                {
                    Type type = MemberType.Type;
                    return type.IsGenericType && type.IsDefined(typeof(AutoCSer.CodeGenerator.JsonSerializeAttribute));
                }
            }
            /// <summary>
            /// 是否 XML 泛型实例调用
            /// </summary>
            internal bool IsXmlSerializeMethodName
            {
                get
                {
                    Type type = MemberType.Type;
                    return type.IsGenericType && type.IsDefined(typeof(AutoCSer.CodeGenerator.XmlSerializeAttribute));
                }
            }
            /// <summary>
            /// 成员类型
            /// </summary>
            /// <param name="type"></param>
            /// <param name="methodName"></param>
            /// <param name="genericTypeName"></param>
            internal ReflectionMemberType(Type type, string methodName = null, string genericTypeName = null)
            {
                MemberType = type;
                this.ReflectionMethodName = methodName;
                this.GenericTypeName = genericTypeName;
            }
            /// <summary>
            /// 成员类型
            /// </summary>
            /// <param name="methodName"></param>
            /// <param name="genericTypeName"></param>
            internal ReflectionMemberType(string methodName, string genericTypeName = null)
            {
                this.ReflectionMethodName = methodName;
                this.GenericTypeName = genericTypeName;
            }
        }
        /// <summary>
        /// 触发 AOT 编译方法信息集合
        /// </summary>
        private static LeftArray<Method> methods = new LeftArray<Method>(0);
        /// <summary>
        /// 添加触发 AOT 编译方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        internal static void Append(ExtensionType type, string methodName)
        {
            if (!type.Type.IsGenericType) methods.Append(new Method(type, methodName));
        }
        /// <summary>
        /// 添加触发 AOT 编译方法名称
        /// </summary>
        /// <param name="methodName"></param>
        internal static void Append(string methodName)
        {
            methods.Append(new Method(null, methodName));
        }
        /// <summary>
        /// 是否调用 AutoCSer.AotMethod.Call();
        /// </summary>
        internal static bool IsCallAutoCSerAotMethod;
        /// <summary>
        /// 是否调用  AutoCSer.CommandService.StreamPersistenceMemoryDatabase.AotMethod.Call();
        /// </summary>
        internal static bool IsCallStreamPersistenceMemoryDatabaseAotMethod;
        /// <summary>
        /// 触发 AOT 编译方法信息集合
        /// </summary>
        public Method[] Methods;
        /// <summary>
        /// 对象对比成员类型集合
        /// </summary>
        public ReflectionMemberType[] EqualsMemberTypes;
        /// <summary>
        /// 随机对象生成成员类型集合
        /// </summary>
        public ReflectionMemberType[] RandomMemberTypes;
        /// <summary>
        /// 二进制序列化成员类型集合
        /// </summary>
        public ReflectionMemberType[] BinarySerializeMemberTypes;
        /// <summary>
        /// 二进制序列化泛型类型集合
        /// </summary>
        public ReflectionMemberType[] BinarySerializeGenericTypes;
        /// <summary>
        /// 二进制序列化泛型成员类型集合
        /// </summary>
        public ReflectionMemberType[] BinarySerializeGenericMemberTypes;
        /// <summary>
        /// JSON 序列化成员类型集合
        /// </summary>
        public ReflectionMemberType[] JsonSerializeMemberTypes;
        /// <summary>
        /// JSON 反序列化成员类型集合
        /// </summary>
        public ReflectionMemberType[] JsonDeserializeMemberTypes;
        /// <summary>
        /// XML 序列化成员类型集合
        /// </summary>
        public ReflectionMemberType[] XmlSerializeMemberTypes;
        /// <summary>
        /// XML 反序列化成员类型集合
        /// </summary>
        public ReflectionMemberType[] XmlDeserializeMemberTypes;
        /// <summary>
        /// XML 序列化可空类型集合
        /// </summary>
        public ReflectionMemberType[] XmlSerializeNullableElementTypes;
        /// <summary>
        /// 内存数据库快照数据类型集合
        /// </summary>
        public ReflectionMemberType[] StreamPersistenceMemoryDatabaseSnapshotTypes;
        /// <summary>
        /// 内存数据库快照数据类型集合
        /// </summary>
        public ReflectionMemberType[] StreamPersistenceMemoryDatabaseSnapshotCloneObjectTypes;
        /// <summary>
        /// 是否调用 AutoCSer.AotMethod.Call();
        /// </summary>
        public bool IsCallAutoCSer { get { return IsCallAutoCSerAotMethod; } }
        /// <summary>
        /// 是否调用  AutoCSer.CommandService.StreamPersistenceMemoryDatabase.AotMethod.Call();
        /// </summary>
        public bool IsCallStreamPersistenceMemoryDatabase { get { return IsCallStreamPersistenceMemoryDatabaseAotMethod; } }
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="attribute">代码生成器配置</param>
        /// <returns>是否生成成功</returns>
        public Task<bool> Run(ProjectParameter parameter, GeneratorAttribute attribute)
        {
            LeftArray<ReflectionMemberType> equalsMemberTypes = FieldEquals.GetReflectionMemberTypes();
            LeftArray<ReflectionMemberType> randomMemberTypes = RandomObject.GetReflectionMemberTypes();
            LeftArray<ReflectionMemberType> binarySerializeMemberTypes = BinarySerialize.GetReflectionMemberTypes(), binarySerializeGenericMemberTypes;
            LeftArray<Type> binarySerializeGenericTypes = BinarySerialize.GetGenericMemberTypes(out binarySerializeGenericMemberTypes);
            LeftArray<ReflectionMemberType> jsonDeserializeMemberTypes, jsonSerializeMemberTypes = JsonSerialize.GetReflectionMemberTypes(out jsonDeserializeMemberTypes);
            LeftArray<ReflectionMemberType> xmlDeserializeMemberTypes, xmlSerializeMemberTypes = XmlSerialize.GetReflectionMemberTypes(out xmlDeserializeMemberTypes);
            LeftArray<ReflectionMemberType> streamPersistenceMemoryDatabaseSnapshotTypes = StreamPersistenceMemoryDatabaseMethodParameterCreator.GetSnapshotTypes();
            LeftArray<ReflectionMemberType> streamPersistenceMemoryDatabaseSnapshotCloneObjectTypes = StreamPersistenceMemoryDatabaseMethodParameterCreator.GetSnapshotCloneObjectTypes();
            if (methods.Length != 0 || equalsMemberTypes.Length != 0 || randomMemberTypes.Length != 0
                || binarySerializeMemberTypes.Length != 0 || binarySerializeGenericTypes.Count != 0 || jsonSerializeMemberTypes.Length != 0 || xmlSerializeMemberTypes.Length != 0
                || streamPersistenceMemoryDatabaseSnapshotTypes.Length != 0 || streamPersistenceMemoryDatabaseSnapshotCloneObjectTypes.Length != 0)
            {
                ProjectParameter = parameter;
                generatorAttribute = attribute;
                assembly = parameter.Assembly;
                foreach (ReflectionMemberType reflectionMember in jsonSerializeMemberTypes)
                {
                    if (reflectionMember.IsJsonSerializeMethodName) methods.Append(new Method(reflectionMember.MemberType, AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMethodName));
                }
                foreach (ReflectionMemberType reflectionMember in xmlSerializeMemberTypes)
                {
                    if (reflectionMember.IsXmlSerializeMethodName) methods.Append(new Method(reflectionMember.MemberType, AutoCSer.CodeGenerator.XmlSerializeAttribute.XmlSerializeMethodName));
                }
                Methods = methods.ToArray();
                EqualsMemberTypes = equalsMemberTypes.ToArray();
                RandomMemberTypes = randomMemberTypes.ToArray();
                BinarySerializeMemberTypes = binarySerializeMemberTypes.ToArray();
                BinarySerializeGenericTypes = binarySerializeGenericTypes.getArray(p => new ReflectionMemberType(p));
                BinarySerializeGenericMemberTypes = binarySerializeGenericMemberTypes.ToArray();
                JsonSerializeMemberTypes = jsonSerializeMemberTypes.ToArray();
                JsonDeserializeMemberTypes = jsonDeserializeMemberTypes.ToArray();
                XmlSerializeMemberTypes = xmlSerializeMemberTypes.ToArray();
                XmlDeserializeMemberTypes = xmlDeserializeMemberTypes.ToArray();
                XmlSerializeNullableElementTypes = XmlSerialize.NullableElementTypes.getArray(p => new ReflectionMemberType(p.Value));
                StreamPersistenceMemoryDatabaseSnapshotTypes = streamPersistenceMemoryDatabaseSnapshotTypes.ToArray();
                StreamPersistenceMemoryDatabaseSnapshotCloneObjectTypes = streamPersistenceMemoryDatabaseSnapshotCloneObjectTypes.ToArray();
                _code_.Append("namespace ", ProjectParameter.DefaultNamespace, @"
{
    /// <summary>
    /// 触发 AOT 编译
    /// </summary>
    public static class AotMethod
    {");
                create(false);
                _code_.Add(@"
    }
}");
                addCode(string.Concat(_code_.Array.ToArray()));
            }
            return AutoCSer.Common.TrueCompletedTask;
        }
    }
}
