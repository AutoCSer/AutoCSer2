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
    /// 流序列化数据库本地客户端节点
    /// </summary>
    [Generator(Name = "流序列化数据库本地客户端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNode : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected override string typeNameSuffix { get { return "LocalClientNode"; } }
        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// Node method information
        /// 节点方法信息
        /// </summary>
        public sealed class NodeMethod
        {
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod ServerNodeMethod;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public MethodIndex Method;
            /// <summary>
            /// 方法定义接口类型
            /// </summary>
            public string MethodInterfaceTypeName;
            /// <summary>
            /// 是否生成接口方法
            /// </summary>
            public bool IsInterfaceMethod;
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
            public bool IsMethodReturnType { get { return MethodReturnType.Type != typeof(void); } }
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool MethodIsReturn { get { return ServerNodeMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 回调委托类型
            /// </summary>
            public ExtensionType CallbackType;
            /// <summary>
            /// 回调委托类型
            /// </summary>
            public ExtensionType KeepCallbackType;
            /// <summary>
            /// 回调委托参数之前是否存在其它参数
            /// </summary>
            public bool IsJoinCallback { get { return Method.Parameters.Length != 0; } }
            /// <summary>
            /// 回调委托参数之前是否存在其它参数
            /// </summary>
            public bool IsJoinKeepCallback { get { return IsJoinCallback || CallbackType != null; } }
            /// <summary>
            /// Customize the command sequence number
            /// 自定义命令序号
            /// </summary>
            public int MethodIndex { get { return ServerNodeMethod.MethodIndex; } }
            /// <summary>
            /// 输入参数类型
            /// </summary>
            public readonly AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType InputParameterType;
            ///// <summary>
            ///// 输出参数类型
            ///// </summary>
            //public readonly AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType OutputParameterType;
            /// <summary>
            /// 是否读写队列节点是否传参
            /// </summary>
            public bool IsReadWriteNodeParameter;
            /// <summary>
            /// 是否读写队列节点
            /// </summary>
            public string IsReadWriteNode { get { return ServerNodeMethod.QueueNodeType == AutoCSer.Net.CommandServer.ReadWriteNodeTypeEnum.Read ? "true" : "false"; } }
            /// <summary>
            /// 客户端调用类型
            /// </summary>
            public ExtensionType ClientType;
            /// <summary>
            /// 客户端调用泛型参数类型
            /// </summary>
            public string GenericTypeName;
            /// <summary>
            /// 是否同步调用方法
            /// </summary>
            public readonly bool IsSynchronous;
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool IsReturnValue { get { return ReturnValueMethodReturnType.Type != typeof(void); } }
            /// <summary>
            /// 是否需要调用直接获取返回值的方法
            /// </summary>
            public readonly bool IsGetReturnValue;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public readonly ExtensionType ReturnValueMethodReturnType;
            /// <summary>
            /// 直接返回值的回调委托类型
            /// </summary>
            public readonly ExtensionType ReturnValueCallbackType;
            /// <summary>
            /// 直接返回值的回调委托类型
            /// </summary>
            public readonly ExtensionType ReturnValueKeepCallbackType;
            /// <summary>
            /// 直接获取返回值的回调委托类型
            /// </summary>
            public readonly ExtensionType CallbackReturnValueType;
            /// <summary>
            /// 直接获取返回值的持续回调委托类型
            /// </summary>
            public readonly ExtensionType KeepCallbackReturnValueType;
            /// <summary>
            /// 回调参数名称
            /// </summary>
            public readonly string CallbackParameterName;
            /// <summary>
            /// 错误状态回调参数名称
            /// </summary>
            public string ErrorCallbackParameterName { get { return "error_" + CallbackParameterName; } }
            /// <summary>
            /// 持续回调参数名称
            /// </summary>
            public readonly string KeepCallbackParameterName;
            /// <summary>
            /// 错误状态回调参数名称
            /// </summary>
            public string ErrorKeepCallbackParameterName { get { return "error_" + KeepCallbackParameterName; } }
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceTypeName"></param>
            /// <param name="nodeAttribute"></param>
            /// <param name="method"></param>
            /// <param name="isServiceNode"></param>
            /// <param name="paramterTypes">参数类型集合</param>
            /// <param name="typeNamePrefix"></param>
            public NodeMethod(string interfaceTypeName, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method, bool isServiceNode, Dictionary<HashObject<Type>, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType> paramterTypes, string typeNamePrefix)
            {
                if (method != null)
                {
                    this.ServerNodeMethod = method;
                    if (method.InputParameterType != null)
                    {
                        if (paramterTypes.TryGetValue(method.InputParameterType.Type, out InputParameterType))
                        {
                            if (ServerNodeMethod.IsPersistence) InputParameterType.SetInputSerialize(method);
                            InputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType(typeNamePrefix, InputParameterType, this);
                        }
                        else paramterTypes.Add(method.InputParameterType.Type, InputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType(typeNamePrefix, method.InputParameterType, this));
                    }
                    if (method.IsClientCall)
                    {
                        IsInterfaceMethod = isServiceNode || method.Type != typeof(IServiceNode);
                        MethodInterfaceTypeName = IsInterfaceMethod ? interfaceTypeName : typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode).fullName();
                        Method = new MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex, method.ParameterStartIndex, method.ParameterEndIndex);
                        switch (method.CallType)
                        {
                            case CallTypeEnum.SendOnly:
                                MethodReturnType = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNodeMethod.LocalClientSendOnlyMethodReturnType;
                                IsSynchronous = true;
                                break;
                            case CallTypeEnum.KeepCallback:
                            case CallTypeEnum.InputKeepCallback:
                            case CallTypeEnum.Enumerable:
                            case CallTypeEnum.InputEnumerable:
                                if (method.MethodAttribute.IsCallbackClient)
                                {
                                    switch (method.CallType)
                                    {
                                        case CallTypeEnum.KeepCallback:
                                        case CallTypeEnum.InputKeepCallback:
                                            KeepCallbackParameterName = method.Parameters[method.ParameterEndIndex].Name;
                                            break;
                                        default: KeepCallbackParameterName = "__keepCallback__"; break;
                                    }
                                    CallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                    MethodReturnType = typeof(LocalServiceQueueNode<IDisposable>);
                                    ReturnValueKeepCallbackType = typeof(Action<>).MakeGenericType(method.ReturnValueType);
                                    KeepCallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueCallback<>).MakeGenericType(method.ReturnValueType);
                                }
                                else
                                {
                                    MethodReturnType = typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalKeepCallback<>).MakeGenericType(method.ReturnValueType));
                                    IsSynchronous = true;
                                }
                                break;
                            case CallTypeEnum.TwoStageCallback:
                            case CallTypeEnum.InputTwoStageCallback:
                                CallbackParameterName = method.Parameters[method.ParameterEndIndex].Name;
                                KeepCallbackParameterName = method.Parameters[method.ParameterEndIndex + 1].Name;
                                CallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.TwoStageReturnValueType));
                                KeepCallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                MethodReturnType = typeof(LocalServiceQueueNode<IDisposable>);
                                ReturnValueCallbackType = typeof(Action<>).MakeGenericType(method.TwoStageReturnValueType);
                                CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueCallback<>).MakeGenericType(method.TwoStageReturnValueType);
                                ReturnValueKeepCallbackType = typeof(Action<>).MakeGenericType(method.ReturnValueType);
                                KeepCallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueCallback<>).MakeGenericType(method.ReturnValueType);
                                break;
                            case CallTypeEnum.Call:
                            case CallTypeEnum.CallInput:
                                if (method.MethodAttribute.IsCallbackClient)
                                {
                                    CallbackParameterName = "__callback__";
                                    CallbackType = typeof(Action<LocalResult>);
                                    MethodReturnType = typeof(void);
                                    ReturnValueCallbackType = typeof(Action);
                                    CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueCallback);
                                }
                                else
                                {
                                    MethodReturnType = typeof(LocalServiceQueueNode<LocalResult>);
                                    ReturnValueMethodReturnType = typeof(LocalServiceReturnValue);
                                    IsGetReturnValue = IsSynchronous = true;
                                }
                                break;
                            case CallTypeEnum.InputCallback:
                            case CallTypeEnum.Callback:
                            case CallTypeEnum.CallInputOutput:
                            case CallTypeEnum.CallOutput:
                                if (method.MethodAttribute.IsCallbackClient)
                                {
                                    switch (method.CallType)
                                    {
                                        case CallTypeEnum.InputCallback:
                                        case CallTypeEnum.Callback:
                                            CallbackParameterName = method.Parameters[method.ParameterEndIndex].Name;
                                            break;
                                        default: CallbackParameterName = "__callback__"; break;
                                    }
                                    CallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                    MethodReturnType = typeof(void);
                                    ReturnValueCallbackType = typeof(Action<>).MakeGenericType(method.ReturnValueType);
                                    CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueCallback<>).MakeGenericType(method.ReturnValueType);
                                }
                                else
                                {
                                    MethodReturnType = typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                    ReturnValueMethodReturnType = typeof(LocalServiceReturnValue<>).MakeGenericType(method.ReturnValueType);
                                    IsGetReturnValue = IsSynchronous = true;
                                }
                                break;
                        }
                        if (ReturnValueMethodReturnType == null) ReturnValueMethodReturnType = MethodReturnType;
                        //if (method.ReturnValueType != typeof(void))
                        //{
                        //    var outputParameterType = ServerMethodParameter.GetOrCreate(0, EmptyArray<ParameterInfo>.Array, method.ReturnValueType);
                        //    if (paramterTypes.TryGetValue(outputParameterType.Type, out OutputParameterType))
                        //    {
                        //        OutputParameterType.SetSerialize(method, true);
                        //        OutputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType(OutputParameterType, this);
                        //    }
                        //    else paramterTypes.Add(outputParameterType.Type, OutputParameterType = new AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ParameterType(typeNamePrefix, outputParameterType, this, true));
                        //}
                        switch (method.CallType)
                        {
                            case CallTypeEnum.Call:
                                IsReadWriteNodeParameter = true;
                                ClientType = CallbackType != null ? typeof(LocalServiceCallbackNode) : typeof(LocalServiceCallNode);
                                break;
                            case CallTypeEnum.CallOutput:
                            case CallTypeEnum.Callback:
                                IsReadWriteNodeParameter = true;
                                ClientType = (CallbackType != null ? typeof(LocalServiceCallbackOutputNode<>) : typeof(LocalServiceCallOutputNode<>)).MakeGenericType(method.ReturnValueType);
                                break;
                            case CallTypeEnum.CallInput:
                                ClientType = CallbackType != null ? typeof(LocalServiceCallbackInputNode) : typeof(LocalServiceCallInputNode);
                                break;
                            case CallTypeEnum.CallInputOutput:
                            case CallTypeEnum.InputCallback:
                                if (CallbackType != null) ClientType = typeof(LocalServiceCallbackInputOutputNode);
                                else
                                {
                                    ClientType = typeof(LocalServiceCallInputOutputNode);
                                    GenericTypeName = $"{method.ReturnValueType.fullName()}, {InputParameterType.ParameterTypeName}";
                                }
                                break;
                            case CallTypeEnum.SendOnly:
                                ClientType = typeof(LocalServiceSendOnlyNode);
                                break;
                            case CallTypeEnum.KeepCallback:
                            case CallTypeEnum.Enumerable:
                                IsReadWriteNodeParameter = true;
                                ClientType = (CallbackType != null ? typeof(LocalServiceKeepCallbackNode<>) : typeof(LocalServiceKeepCallbackEnumeratorNode<>)).MakeGenericType(method.ReturnValueType);
                                break;
                            case CallTypeEnum.TwoStageCallback:
                                IsReadWriteNodeParameter = true;
                                ClientType = typeof(LocalServiceTwoStageCallbackNode);
                                GenericTypeName = $"{method.TwoStageReturnValueType.fullName()}, {method.ReturnValueType.fullName()}";
                                break;
                            case CallTypeEnum.InputKeepCallback:
                            case CallTypeEnum.InputEnumerable:
                                if (CallbackType != null) ClientType = typeof(LocalServiceInputKeepCallbackNode);
                                else
                                {
                                    ClientType = typeof(LocalServiceInputKeepCallbackEnumeratorNode);
                                    GenericTypeName = $"{method.ReturnValueType.fullName()}, {InputParameterType.ParameterTypeName}";
                                }
                                break;
                            case CallTypeEnum.InputTwoStageCallback:
                                ClientType = typeof(LocalServiceInputTwoStageCallbackNode);
                                GenericTypeName = $"{method.TwoStageReturnValueType.fullName()}, {method.ReturnValueType.fullName()}, {InputParameterType.ParameterTypeName}";
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 本地客户端节点构造函数名称
        /// </summary>
        public string LocalClientNodeConstructorMethodName { get { return ClientNodeAttribute.LocalClientNodeConstructorMethodName; } }
        /// <summary>
        /// 本地客户端节点激活 AOT 反射函数名称
        /// </summary>
        public string LocalClientNodeMethodName { get { return ClientNodeAttribute.LocalClientNodeMethodName; } }
        /// <summary>
        /// 当前接口类型名称
        /// </summary>
        public string InterfaceTypeName;
        /// <summary>
        /// 当前类型名称
        /// </summary>
        public new string TypeName;
        /// <summary>
        /// 节点方法序号映射枚举类型
        /// </summary>
        public string MethodIndexEnumTypeName
        {
            get { return CurrentType.Type.Name + "MethodEnum"; }
        }
        /// <summary>
        /// 是否自定义基础服务节点
        /// </summary>
        public bool IsCustomServiceNode { get { return CurrentType.Type != typeof(IServiceNode) && typeof(IServiceNode).IsAssignableFrom(CurrentType.Type); } }
        /// <summary>
        /// 参数类型集合
        /// </summary>
        public AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType[] ParameterTypes;
        /// <summary>
        /// Node method collection
        /// 节点方法集合
        /// </summary>
        public NodeMethod[] Methods;
        /// <summary>
        /// 是否生成直接返回值的 API 封装类型
        /// </summary>
        public bool IsReturnValueNode { get { return CurrentAttribute.IsReturnValueNode; } }
        /// <summary>
        /// 客户端节点接口类型名称
        /// </summary>
        public string ClientNodeTypeName { get { return CurrentType.GetName(typeNameSuffix); } }
        /// <summary>
        /// 直接返回值的 API 封装类型名称
        /// </summary>
        public string ReturnValueNodeTypeName { get { return CurrentType.TypeOnlyName + "ReturnValueLocalNode"; } }

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (!CurrentAttribute.IsLocalClient || !type.IsInterface || type.IsGenericType || type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            if (type.IsGenericType) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType nodeType = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = nodeType.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            InterfaceTypeName = type.Name + typeNameSuffix;
            if (string.IsNullOrEmpty(CurrentAttribute.LocalClientTypeName))
            {
                TypeName = CurrentType.Type.Name;
                if (TypeName.Length > 1 && TypeName[0] == 'I' && (uint)(TypeName[1] - 'A') < 26) TypeName = TypeName.Substring(1);
                TypeName += "LocalClient";
            }
            else TypeName = CurrentAttribute.LocalClientTypeName;
            string typeName = type.fullName(), typeNamePrefix = typeName.Substring(0, typeName.LastIndexOf('.') + 1) + TypeName + ".";
            Dictionary<HashObject<Type>, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType> paramterTypes = DictionaryCreator.CreateHashObject<Type, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType>(methods.Length);
            Dictionary<HashObject<MethodInfo>, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType> methodParameterTypes = DictionaryCreator.CreateHashObject<MethodInfo, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType>(methods.Length);
            Methods = methods.getArray(p => new NodeMethod(InterfaceTypeName, nodeType.NodeAttribute, p, type == typeof(IServiceNode), paramterTypes, typeNamePrefix));
            foreach (NodeMethod method in Methods)
            {
                if (method.InputParameterType != null) methodParameterTypes.Add(method.ServerNodeMethod.Method, method.InputParameterType);
            }
            ParameterTypes = paramterTypes.Values.ToArray();
            foreach (AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType parameterType in ParameterTypes) parameterType.SetSerializeCode();
            create(true);
            InterfaceParamterTypes.Add(type, methodParameterTypes);
            AotMethod.Append(typeNamePrefix + LocalClientNodeMethodName);
            AotMethod.IsCallAutoCSerAotMethod = AotMethod.IsCallStreamPersistenceMemoryDatabaseAotMethod = true;
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 接口方法参数类型集合
        /// </summary>
        internal static readonly Dictionary<HashObject<Type>, Dictionary<HashObject<MethodInfo>, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType>> InterfaceParamterTypes = DictionaryCreator.CreateHashObject<Type, Dictionary<HashObject<MethodInfo>, AutoCSer.CodeGenerator.TemplateGenerator.CommandClientController.ParameterType>>();
    }
}
