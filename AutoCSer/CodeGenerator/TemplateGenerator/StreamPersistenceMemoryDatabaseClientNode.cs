using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化数据库客户端节点
    /// </summary>
    [Generator(Name = "流序列化数据库客户端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseClientNode : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected override string typeNameSuffix { get { return "ClientNode"; } }
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
            private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod interfaceMethod;
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
            public bool MethodIsReturn { get { return interfaceMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 返回参数类型
            /// </summary>
            public ExtensionType ReturnRequestParameterType;
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
            public bool IsJoinCallback { get { return ReturnRequestParameterType != null || Method.Parameters.Length != 0; } }
            /// <summary>
            /// 回调委托参数之前是否存在其它参数
            /// </summary>
            public bool IsJoinKeepCallback { get { return IsJoinCallback || CallbackType != null; } }
            /// <summary>
            /// 是否同步调用方法
            /// </summary>
            public readonly bool IsSynchronous;
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
            /// <param name="interfaceMethod"></param>
            /// <param name="isServiceNode"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod interfaceMethod, bool isServiceNode)
            {
                if (interfaceMethod != null && interfaceMethod.IsClientCall
                    && (isServiceNode || interfaceMethod.Type != typeof(IServiceNode)))
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new Metadata.MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                    switch (interfaceMethod.CallType)
                    {
                        case CallTypeEnum.SendOnly:
                            MethodReturnType = typeof(SendOnlyCommand);
                            IsSynchronous = true;
                            break;
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                switch (interfaceMethod.CallType)
                                {
                                    case CallTypeEnum.KeepCallback:
                                    case CallTypeEnum.InputKeepCallback:
                                        KeepCallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                                        break;
                                    default: KeepCallbackParameterName = "__keepCallback__"; break;
                                }
                                KeepCallbackType = typeof(Action<,>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType), typeof(AutoCSer.Net.KeepCallbackCommand));
                                MethodReturnType = typeof(AutoCSer.Net.KeepCallbackCommand);
                                ReturnValueKeepCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                                KeepCallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            }
                            else
                            {
                                MethodReturnType = typeof(Task<>).MakeGenericType(typeof(KeepCallbackResponse<>).MakeGenericType(interfaceMethod.ReturnValueType));
                                IsSynchronous = true;
                            }
                            if (interfaceMethod.ReturnValueType == typeof(ResponseParameterSerializer)) ReturnRequestParameterType = typeof(ResponseParameterSerializer);
                            break;
                        case CallTypeEnum.TwoStageCallback:
                        case CallTypeEnum.InputTwoStageCallback:
                            CallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                            KeepCallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex + 1].Name;
                            CallbackType = typeof(Action<>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType));
                            KeepCallbackType = typeof(Action<,>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType), typeof(AutoCSer.Net.KeepCallbackCommand));
                            MethodReturnType = typeof(AutoCSer.Net.KeepCallbackCommand);
                            ReturnValueCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType);
                            CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType);
                            ReturnValueKeepCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            KeepCallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            break;
                        case CallTypeEnum.Call:
                        case CallTypeEnum.CallInput:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                CallbackParameterName = "__callback__";
                                CallbackType = typeof(Action<ResponseResult>);
                                MethodReturnType = typeof(AutoCSer.Net.CallbackCommand);
                                ReturnValueCallbackType = typeof(Action);
                                CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback);
                            }
                            else
                            {
                                MethodReturnType = typeof(ResponseResultAwaiter);
                                ReturnValueMethodReturnType = typeof(ResponseReturnValue);
                                IsGetReturnValue = IsSynchronous = true;
                            }
                            break;
                        case CallTypeEnum.InputCallback:
                        case CallTypeEnum.Callback:
                        case CallTypeEnum.CallInputOutput:
                        case CallTypeEnum.CallOutput:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                switch (interfaceMethod.CallType)
                                {
                                    case CallTypeEnum.InputCallback:
                                    case CallTypeEnum.Callback:
                                        CallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                                        break;
                                    default: CallbackParameterName = "__callback__"; break;
                                }
                                if (interfaceMethod.ReturnValueType == typeof(ResponseParameter))
                                {
                                    ReturnRequestParameterType = typeof(ResponseParameter);
                                    CallbackType = typeof(Action<ResponseResult<ResponseParameter>>);
                                    ReturnValueCallbackType = typeof(Action<ResponseParameter>);
                                    CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback<ResponseParameter>);
                                }
                                else
                                {
                                    CallbackType = typeof(Action<>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType));
                                    ReturnValueCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                                    CallbackReturnValueType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                                }
                                MethodReturnType = typeof(AutoCSer.Net.CallbackCommand);
                            }
                            else
                            {
                                if (interfaceMethod.ReturnValueType == typeof(ResponseParameter))
                                {
                                    ReturnRequestParameterType = typeof(ResponseParameter);
                                    MethodReturnType = typeof(ResponseParameterAwaiter<ResponseParameter>);
                                    ReturnValueMethodReturnType = typeof(ResponseReturnValue<ResponseParameter>);
                                }
                                else
                                {
                                    MethodReturnType = typeof(ResponseParameterAwaiter<>).MakeGenericType(interfaceMethod.ReturnValueType);
                                    ReturnValueMethodReturnType = typeof(ResponseReturnValue<>).MakeGenericType(interfaceMethod.ReturnValueType);
                                }
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
        public string ReturnValueNodeTypeName { get { return CurrentType.TypeOnlyName + "ReturnValueNode"; } }
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
            if (!CurrentAttribute.IsClient || !CurrentType.Type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType type = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(CurrentType.Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = type.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(type.NodeAttribute, p, CurrentType.Type == typeof(IServiceNode)));

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
