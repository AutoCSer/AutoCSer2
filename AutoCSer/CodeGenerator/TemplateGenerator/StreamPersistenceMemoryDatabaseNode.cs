using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            public readonly Metadata.MethodIndex Method;
            /// <summary>
            /// 方法名称
            /// </summary>
            public string MethodName { get { return Method.MethodName; } }
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType MethodReturnType;
            /// <summary>
            /// 枚举名称
            /// </summary>
            public string EnumName;
            /// <summary>
            /// 方法序号
            /// </summary>
            public int MethodIndex;
            /// <summary>
            /// 快照数据类型
            /// </summary>
            public ExtensionType SnapshotType;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            public NodeMethod() { }
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="method"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method)
            {
                Method = new Metadata.MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex, method.ParameterStartIndex, method.ParameterEndIndex);
                EnumName = Method.MethodName;
                MethodReturnType = method.ReturnValueType;
                if (method.MethodAttribute.SnapshotMethodSort != 0) SnapshotType = method.Parameters[method.ParameterStartIndex].ParameterType;
            }
            /// <summary>
            /// 设置方法序号
            /// </summary>
            /// <param name="methodIndex">方法序号</param>
            internal void SetMethodIndex(int methodIndex)
            {
                MethodIndex = methodIndex;
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
            Type type = CurrentType.Type;
            if (!type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType nodeType = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = nodeType.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            int methodArrayIndex = 0;
            LeftArray<NodeMethod> snapshotTypeNodeMethod = new LeftArray<NodeMethod>(0);
            Methods = new NodeMethod[methods.Length];
            foreach (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method in methods)
            {
                if (method != null)
                {
                    if (method.MethodAttribute.SnapshotMethodSort != 0 && method.ParameterCount != 1)
                    {
                        Messages.Error($"{type.fullName()} 节点快照方法 {method.Method.Name} 有效输入参数数量 {method.ParameterCount} 必须为 1");
                        return AutoCSer.Common.CompletedTask;
                    }
                    NodeMethod nodeMethod = new NodeMethod(method);
                    if (nodeMethod.SnapshotType != null)
                    {
                        foreach (NodeMethod snapshotMethod in snapshotTypeNodeMethod)
                        {
                            if (snapshotMethod.SnapshotType.Type == nodeMethod.SnapshotType.Type)
                            {
                                Messages.Error($"{type.fullName()} 节点快照方法 {method.Method.Name} 冲突 {snapshotMethod.MethodName}");
                                return AutoCSer.Common.CompletedTask;
                            }
                        }
                        snapshotTypeNodeMethod.Add(nodeMethod);
                    }
                    Methods[methodArrayIndex] = nodeMethod;
                }
                else Methods[methodArrayIndex] = new NodeMethod();
                ++methodArrayIndex;
            }

            int methodIndex = -1;
            Type methodIndexEnumType = nodeType.ServerNodeTypeAttribute?.MethodIndexEnumType;
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
            HashSet<string> names = HashSetCreator<string>.Create();
            foreach (NodeMethod method in Methods)
            {
                if (method.EnumName == null) method.EnumName = method.Method?.MethodName;
                if (method.EnumName != null && !names.Add(method.EnumName))
                {
                    throw new Exception(Culture.Configuration.Default.GetStreamPersistenceMemoryDatabaseNodeMethodNameConflict(type, MethodIndexEnumTypeName, method.EnumName));
                }
                method.SetMethodIndex(methodIndex++);
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
