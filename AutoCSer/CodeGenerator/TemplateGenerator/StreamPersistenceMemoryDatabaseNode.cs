using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static AutoCSer.CodeGenerator.Template.Pub;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化数据库服务端节点
    /// </summary>
    [Generator(Name = "流序列化数据库服务端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseNode : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// 节点方法信息
        /// </summary>
        public sealed class NodeMethod
        {
            /// <summary>
            /// 接口方法信息
            /// </summary>
            private InterfaceMethodBase interfaceMethod;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public MethodIndex Method;
            /// <summary>
            /// 方法名称
            /// </summary>
            public string MethodName { get { return Method.MethodName; } }
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType MethodReturnType;
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool MethodIsReturn { get { return interfaceMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 枚举名称
            /// </summary>
            public string EnumName;
            /// <summary>
            /// 方法序号
            /// </summary>
            public int MethodIndex;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="nodeAttribute"></param>
            /// <param name="interfaceMethod"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod interfaceMethod)
            {
                if (interfaceMethod != null)
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                    EnumName = Method.MethodName;
                    MethodReturnType = interfaceMethod.ReturnValueType;
                }
            }
        }

        /// <summary>
        /// 节点方法集合
        /// </summary>
        public NodeMethod[] Methods;
        /// <summary>
        /// 节点方法序号映射枚举类型
        /// </summary>
        public string MethodIndexEnumTypeName
        {
            get { return CurrentType.TypeOnlyName + "MethodEnum"; }
        }

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType type = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(CurrentType.Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = type.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(type.NodeAttribute, p));

            int methodIndex = -1;
            Type methodIndexEnumType = type.ServerNodeMethodIndexAttribute?.MethodIndexEnumType;
            if (methodIndexEnumType != null && methodIndexEnumType.IsEnum)
            {
                LeftArray<KeyValue<int, string>> enumValues = new LeftArray<KeyValue<int, string>>(0);
                foreach (object value in Enum.GetValues(methodIndexEnumType))
                {
                    int index = ((IConvertible)value).ToInt32(null);
                    if (index < Methods.Length) Methods[index].EnumName = value.ToString();
                    else
                    {
                        if (index > methodIndex) methodIndex = index;
                        enumValues.Add(new KeyValue<int, string>(index, value.ToString()));
                    }
                }
                if (methodIndex >= Methods.Length)
                {
                    Methods = AutoCSer.Common.GetCopyArray(Methods, methodIndex + 1);
                    foreach (KeyValue<int, string> enumValue in enumValues) Methods[enumValue.Key].EnumName = enumValue.Value;
                }
            }
            methodIndex = 0;
            HashSet<string> names = HashSetCreator.CreateAny<string>();
            foreach (NodeMethod method in Methods)
            {
                if (method.EnumName == null) method.EnumName = method.Method?.MethodName;
                if (method.EnumName != null && !names.Add(method.EnumName))
                {
                    throw new Exception($"{CurrentType.Type.fullName()} 生成 {MethodIndexEnumTypeName}.{method.EnumName} 名称冲突");
                }
                method.MethodIndex = methodIndex++;
            }

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
