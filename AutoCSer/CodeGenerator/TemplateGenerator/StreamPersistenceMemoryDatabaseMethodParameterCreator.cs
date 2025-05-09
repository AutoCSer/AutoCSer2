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

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化数据库服务端节点
    /// </summary>
    [Generator(Name = "流序列化数据库服务端节点", IsAuto = true, DependType = typeof(StreamPersistenceMemoryDatabaseLocalClientNode))]
    internal partial class StreamPersistenceMemoryDatabaseMethodParameterCreator : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
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
            /// 服务端接口类型
            /// </summary>
            private readonly Type interfaceTye;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod ServerNodeMethod;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public readonly MethodIndex Method;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public readonly AutoCSer.CodeGenerator.Metadata.MethodParameter[] MethodParameters;
            /// <summary>
            /// 方法定义接口类型
            /// </summary>
            public string MethodInterfaceTypeName;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType ReturnValueType;
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
            public bool MethodIsReturn { get { return ServerNodeMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 枚举名称
            /// </summary>
            public string EnumName;
            /// <summary>
            /// 方法序号
            /// </summary>
            public int MethodIndex;
            /// <summary>
            /// 方法数组索引位置
            /// </summary>
            public int MethodArrayIndex;
            /// <summary>
            /// 持久化之前参数检查方法编号
            /// </summary>
            public int PersistenceMethodIndex { get { return ServerNodeMethod.BeforePersistenceMethodIndex; } }
            /// <summary>
            /// 冷启动加载持久化方法编号
            /// </summary>
            public int LoadPersistenceMethodIndex { get { return ServerNodeMethod.LoadPersistenceMethodIndex; } }
            /// <summary>
            /// 是否传参方法调用类型
            /// </summary>
            public readonly bool IsCallTypeParameter;
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public byte CallTypeValue { get { return (byte)ServerNodeMethod.CallType; } }
            /// <summary>
            /// 回调参数名称
            /// </summary>
            public string CallbackParameterName;
            /// <summary>
            /// 调用方法名称
            /// </summary>
            public string MethodParameterCreatorCallMethodName;
            /// <summary>
            /// 输入参数类型
            /// </summary>
            public AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType InputParameterType;
            /// <summary>
            /// 输入参数类型名称
            /// </summary>
            public string ParameterTypeFullName { get { return InputParameterType.ParameterTypeFullName; } }
            /// <summary>
            /// 服务端节点方法标记
            /// </summary>
            public readonly byte MethodFlags;
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsCall { get { return ServerNodeMethod.CallType == CallTypeEnum.Call; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string CallMethodTypeName;
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsCallOutput { get { return ServerNodeMethod.CallType == CallTypeEnum.CallOutput || ServerNodeMethod.CallType == CallTypeEnum.Callback; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string CallOutputMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsCallInput { get { return ServerNodeMethod.CallType == CallTypeEnum.CallInput; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string CallInputMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsCallInputOutput { get { return ServerNodeMethod.CallType == CallTypeEnum.CallInputOutput || ServerNodeMethod.CallType == CallTypeEnum.InputCallback; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string CallInputOutputMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsSendOnly { get { return ServerNodeMethod.CallType == CallTypeEnum.SendOnly; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string SendOnlyMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsKeepCallback { get { return ServerNodeMethod.CallType == CallTypeEnum.KeepCallback || ServerNodeMethod.CallType == CallTypeEnum.Enumerable; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string KeepCallbackMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 方法调用类型
            /// </summary>
            public bool IsInputKeepCallback { get { return ServerNodeMethod.CallType == CallTypeEnum.InputKeepCallback || ServerNodeMethod.CallType == CallTypeEnum.InputEnumerable; } }
            /// <summary>
            /// 服务端节点方法类型名称
            /// </summary>
            public string InputKeepCallbackMethodTypeName { get { return CallMethodTypeName; } }
            /// <summary>
            /// 是否持久化之前检查参数方法
            /// </summary>
            public bool IsBeforePersistenceMethod { get { return ServerNodeMethod.IsBeforePersistenceMethod; } }
            /// <summary>
            /// 持久化方法返回数据类型
            /// </summary>
            public bool IsPersistenceMethodReturnType { get { return ServerNodeMethod.PersistenceMethodReturnType != typeof(void); } }
            /// <summary>
            /// 是否返回参数类型
            /// </summary>
            public bool IsResponseParameter { get { return ServerNodeMethod.ReturnValueType == typeof(ResponseParameter); } }
            /// <summary>
            /// 是否需要调用获取持久化检查方法返回值
            /// </summary>
            public bool IsGetBeforePersistenceResponseParameter { get { return ServerNodeMethod.PersistenceMethodReturnType != typeof(void) && ServerNodeMethod.ReturnValueType != typeof(ResponseParameter); } }
            /// <summary>
            /// 快照数据类型
            /// </summary>
            public ExtensionType SnapshotType;
            /// <summary>
            /// 快照数据序列化方法名称
            /// </summary>
            public string SnapshotSerializeMethodName { get { return $"{ServerNodeMethod.Method.Name}_SnapshotSerialize"; } }
            /// <summary>
            /// 是否支持简单序列化
            /// </summary>
            public bool IsSimpleSerialize { get { return ServerNodeMethod.InputParameterType.IsSimpleSerialize;  } }
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceTye"></param>
            /// <param name="methodArrayIndex"></param>
            public NodeMethod(Type interfaceTye, int methodArrayIndex)
            {
                this.interfaceTye = interfaceTye;
                MethodArrayIndex = methodArrayIndex;
            }
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceTye"></param>
            /// <param name="nodeAttribute"></param>
            /// <param name="method"></param>
            /// <param name="methodArrayIndex"></param>
            /// <param name="inputParameterType"></param>
            /// <param name="isMethodParameterCreator"></param>
            public NodeMethod(Type interfaceTye, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method, int methodArrayIndex, AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType inputParameterType, bool isMethodParameterCreator)
            {
                this.interfaceTye = interfaceTye;
                MethodArrayIndex = methodArrayIndex;
                if (method != null)
                {
                    ServerNodeMethod = method;
                    MethodInterfaceTypeName = method.Method.DeclaringType.fullName();
                    Method = new MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex, method.ParameterStartIndex, method.ParameterEndIndex);
                    MethodParameters = new MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex).Parameters;
                    EnumName = Method.MethodName;
                    ReturnValueType = method.Method.ReturnType;
                    MethodReturnType = method.ReturnValueType;
                    InputParameterType = inputParameterType;
                    MethodFlags = (byte)method.GetMethodFlags();
                    switch (method.CallType)
                    {
                        case CallTypeEnum.Callback:
                        case CallTypeEnum.InputCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            IsCallTypeParameter = true;
                            break;
                    }
                    if (isMethodParameterCreator)
                    {
                        switch (method.CallType)
                        {
                            case CallTypeEnum.CallInput:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallInputMethodParameter);
                                break;
                            case CallTypeEnum.CallInputOutput:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallInputOutputMethodParameter);
                                break;
                            case CallTypeEnum.InputCallback:
                                CallbackParameterName = $"{typeof(MethodCallback<>).MakeGenericType(method.ReturnValueType).fullName()}.{nameof(MethodCallback<int>.GetCallback)}({method.Parameters[method.ParameterEndIndex].Name})";
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallInputOutputCallbackMethodParameter);
                                break;
                            case CallTypeEnum.SendOnly:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateSendOnlyMethodParameter);
                                break;
                            case CallTypeEnum.InputKeepCallback:
                            case CallTypeEnum.InputEnumerable:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateInputKeepCallbackMethodParameter);
                                break;

                            case CallTypeEnum.Call:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallMethodParameter);
                                break;
                            case CallTypeEnum.CallOutput:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallOutputMethodParameter);
                                break;
                            case CallTypeEnum.Callback:
                                CallbackParameterName = $"{typeof(MethodCallback<>).MakeGenericType(method.ReturnValueType).fullName()}.{nameof(MethodCallback<int>.GetCallback)}({method.Parameters[method.ParameterEndIndex].Name})";
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateCallOutputCallbackMethodParameter);
                                break;
                            case CallTypeEnum.KeepCallback:
                            case CallTypeEnum.Enumerable:
                                MethodParameterCreatorCallMethodName = nameof(MethodParameterCreator.CreateKeepCallbackMethodParameter);
                                break;
                        }
                    }
                    if (method.MethodAttribute.SnapshotMethodSort != 0) SnapshotType = method.Parameters[method.ParameterStartIndex].ParameterType;
                }
            }
            /// <summary>
            /// 设置方法序号
            /// </summary>
            /// <param name="methodIndex">方法序号</param>
            internal void SetMethodIndex(int methodIndex)//, Dictionary<HashObject<Type>, AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType> paramterTypes, string typeNamePrefix
            {
                MethodIndex = methodIndex;
                if (ServerNodeMethod != null)
                {
                    CallMethodTypeName = $"{interfaceTye.Name}_{Method.Method.Name}_{MethodIndex.toString()}";
                    //if (InputParameterType == null && ServerNodeMethod.InputParameterType != null)
                    //{
                    //    if (paramterTypes.TryGetValue(ServerNodeMethod.InputParameterType.Type, out InputParameterType))
                    //    {
                    //        if (ServerNodeMethod.IsPersistence) InputParameterType.SetInputSerialize(ServerNodeMethod);
                    //        InputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType(typeNamePrefix, InputParameterType, this);
                    //    }
                    //    else paramterTypes.Add(ServerNodeMethod.InputParameterType.Type, InputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType(typeNamePrefix, ServerNodeMethod.InputParameterType, this));
                    //}
                }
            }
            /// <summary>
            /// 快照方法排序
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            internal static int SnapshotSort(NodeMethod left, NodeMethod right)
            {
                return left.ServerNodeMethod.MethodAttribute.SnapshotMethodSort - right.ServerNodeMethod.MethodAttribute.SnapshotMethodSort;
            }
        }

        /// <summary>
        /// 当前接口类型名称
        /// </summary>
        public string InterfaceTypeName { get { return CurrentType.Type.Name; } }
        /// <summary>
        /// 创建调用方法信息
        /// </summary>
        public string MethodParameterCreatorMethodName { get { return ServerNodeTypeAttribute.MethodParameterCreatorMethodName; } }
        /// <summary>
        /// 获取生成服务端节点方法信息方法名称
        /// </summary>
        public string GetServerNodeCreatorMethodName { get { return ServerNodeTypeAttribute.GetServerNodeCreatorMethodName; } }
        ///// <summary>
        ///// 参数类型集合
        ///// </summary>
        //public AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType[] ParameterTypes;
        /// <summary>
        /// 节点方法集合
        /// </summary>
        public NodeMethod[] Methods;
        /// <summary>
        /// 快照方法集合
        /// </summary>
        public NodeMethod[] SnapshotMethods;
        /// <summary>
        /// 节点方法序号映射枚举类型
        /// </summary>
        public string MethodIndexEnumTypeName
        {
            get { return CurrentType.TypeOnlyName + "MethodEnum"; }
        }
        /// <summary>
        /// 生成调用方法参数创建工具
        /// </summary>
        public bool IsMethodParameterCreator;
        /// <summary>
        /// 生成调用方法类型名称
        /// </summary>
        public string MethodParameterCreatorTypeName;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (!type.IsInterface) return AutoCSer.Common.CompletedTask;
            if (type.IsGenericType) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType nodeType = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = nodeType.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            IsMethodParameterCreator = nodeType.NodeAttribute.IsMethodParameterCreator;
            MethodParameterCreatorTypeName = type.Name + "MethodParameterCreator";
            if (MethodParameterCreatorTypeName.Length > 1 && MethodParameterCreatorTypeName[0] == 'I' && (uint)(MethodParameterCreatorTypeName[1] - 'A') < 26) MethodParameterCreatorTypeName = MethodParameterCreatorTypeName.Substring(1);
            int methodArrayIndex = 0;
            LeftArray<NodeMethod> snapshotTypeNodeMethod = new LeftArray<NodeMethod>(0);
            var methodParameterTypes = default(Dictionary<HashObject<MethodInfo>, AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType>);
            StreamPersistenceMemoryDatabaseLocalClientNode.InterfaceParamterTypes.TryGetValue(type, out methodParameterTypes);
            Methods = new NodeMethod[methods.Length];
            foreach (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method in methods)
            {
                if (method != null)
                {
                    var inputParameterType = default(AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType);
                    if (method.InputParameterType != null)//method.IsClientCall && 
                    {
                        if (methodParameterTypes == null || !methodParameterTypes.TryGetValue(method.Method, out inputParameterType))
                        {
                            Messages.Error($"{type.fullName()} 节点快照方法 {method.Method.Name} 没有找到输入参数信息");
                            return AutoCSer.Common.CompletedTask;
                        }
                    }
                    if (method.MethodAttribute.SnapshotMethodSort != 0 && method.ParameterCount != 1)
                    {
                        Messages.Error($"{type.fullName()} 节点快照方法 {method.Method.Name} 有效输入参数数量 {method.ParameterCount} 必须为 1");
                        return AutoCSer.Common.CompletedTask;
                    }
                    NodeMethod nodeMethod = new NodeMethod(type, nodeType.NodeAttribute, method, methodArrayIndex, inputParameterType, IsMethodParameterCreator);
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

                        Type snapshotType = nodeMethod.SnapshotType.Type;
                        snapshotTypes.Add(snapshotType);
                        for (var baseType = snapshotType.BaseType; baseType != typeof(object) && baseType != null; baseType = baseType.BaseType)
                        {
                            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(SnapshotCloneObject<>)
                                && snapshotType == baseType.GetGenericArguments()[0])
                            {
                                snapshotCloneObjectTypes.Add(snapshotType);
                                break;
                            }
                        }
                    }
                    Methods[methodArrayIndex] = nodeMethod;
                }
                else Methods[methodArrayIndex] = new NodeMethod(type, methodArrayIndex);
                ++methodArrayIndex;
            }
            snapshotTypeNodeMethod.Sort(NodeMethod.SnapshotSort);
            SnapshotMethods = snapshotTypeNodeMethod.ToArray();

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
            string typeName = type.fullName(), typeNamePrefix = typeName.Substring(0, typeName.LastIndexOf('.') + 1) + MethodParameterCreatorTypeName + ".";
            //Dictionary<HashObject<Type>, AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType> paramterTypes = DictionaryCreator.CreateHashObject<Type, AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType>(methodParameterTypes != null ? Math.Max(methods.Length - methodParameterTypes.Count, 0) : methods.Length);
            foreach (NodeMethod method in Methods)
            {
                if (method.EnumName == null) method.EnumName = method.Method?.MethodName;
                if (method.EnumName != null && !names.Add(method.EnumName))
                {
                    throw new Exception(Culture.Configuration.Default.GetStreamPersistenceMemoryDatabaseNodeMethodNameConflict(type, MethodIndexEnumTypeName, method.EnumName));
                }
                method.SetMethodIndex(methodIndex++);//, paramterTypes, typeNamePrefix
            }
            //ParameterTypes = paramterTypes.Values.ToArray();
            //foreach (AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType parameterType in ParameterTypes) parameterType.SetSerializeCode();
            create(true);
            AotMethod.Append(typeNamePrefix + MethodParameterCreatorMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 快照数据类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> snapshotTypes = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 获取快照数据类型集合
        /// </summary>
        /// <returns></returns>
        internal static LeftArray<AotMethod.ReflectionMemberType> GetSnapshotTypes()
        {
            LeftArray<AotMethod.ReflectionMemberType> memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(0);
            foreach (HashObject<Type> type in snapshotTypes) memberTypes.Add(new AotMethod.ReflectionMemberType(type.Value));
            return memberTypes;
        }
        /// <summary>
        /// 快照数据类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> snapshotCloneObjectTypes = HashSetCreator.CreateHashObject<Type>();
        /// <summary>
        /// 获取快照数据类型集合
        /// </summary>
        /// <returns></returns>
        internal static LeftArray<AotMethod.ReflectionMemberType> GetSnapshotCloneObjectTypes()
        {
            LeftArray<AotMethod.ReflectionMemberType> memberTypes = new LeftArray<AotMethod.ReflectionMemberType>(0);
            foreach (HashObject<Type> type in snapshotCloneObjectTypes) memberTypes.Add(new AotMethod.ReflectionMemberType(type.Value, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneNode.Create)));
            return memberTypes;
        }
    }
}
