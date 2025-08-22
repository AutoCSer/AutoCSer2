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
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNodeInterface : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
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
            public Metadata.MethodIndex Method;
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
            /// <param name="nodeAttribute"></param>
            /// <param name="method"></param>
            /// <param name="isServiceNode"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method, bool isServiceNode)
            {
                if (method != null && method.IsClientCall
                    && (isServiceNode || method.Type != typeof(IServiceNode)))
                {
                    this.ServerNodeMethod = method;
                    Method = new Metadata.MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex, method.ParameterStartIndex, method.ParameterEndIndex);
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
                                KeepCallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
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
                                CallbackType =typeof(Action<LocalResult>);
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
                }
            }
        }
        /// <summary>
        /// 是否自定义基础服务节点
        /// </summary>
        public bool IsCustomServiceNode { get { return CurrentType.Type != typeof(IServiceNode) && typeof(IServiceNode).IsAssignableFrom(CurrentType.Type); } }
        /// <summary>
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
        /// 是否泛型类型
        /// </summary>
        public bool IsGenericTypeDefinition { get { return CurrentType.Type.IsGenericTypeDefinition; } }
        /// <summary>
        /// 直接返回值的 API 封装类型的泛型定义字符串
        /// </summary>
        public string ReturnValueNodeCurrentType { get { return CurrentType.GetGenericTypeString(); } }

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (!CurrentAttribute.IsLocalClient || !type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType nodeType = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = nodeType.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(nodeType.NodeAttribute, p, type == typeof(IServiceNode)));
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
