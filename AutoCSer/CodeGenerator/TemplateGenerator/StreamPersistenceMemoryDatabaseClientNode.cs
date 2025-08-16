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
                            break;
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                KeepCallbackType = typeof(Action<,>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType), typeof(AutoCSer.Net.KeepCallbackCommand));
                                MethodReturnType = typeof(AutoCSer.Net.KeepCallbackCommand);
                            }
                            else
                            {
                                MethodReturnType = typeof(Task<>).MakeGenericType(typeof(KeepCallbackResponse<>).MakeGenericType(interfaceMethod.ReturnValueType));
                            }
                            if (interfaceMethod.ReturnValueType == typeof(ResponseParameterSerializer)) ReturnRequestParameterType = typeof(ResponseParameterSerializer);
                            break;
                        case CallTypeEnum.TwoStageCallback:
                        case CallTypeEnum.InputTwoStageCallback:
                            CallbackType = typeof(Action<>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType));
                            KeepCallbackType = typeof(Action<,>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType), typeof(AutoCSer.Net.KeepCallbackCommand));
                            MethodReturnType = typeof(AutoCSer.Net.KeepCallbackCommand);
                            break;
                        case CallTypeEnum.Call:
                        case CallTypeEnum.CallInput:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                CallbackType = typeof(Action<ResponseResult>);
                                MethodReturnType = typeof(AutoCSer.Net.CallbackCommand);
                            }
                            else MethodReturnType = typeof(ResponseResultAwaiter);
                            break;
                        case CallTypeEnum.InputCallback:
                        case CallTypeEnum.CallInputOutput:
                        case CallTypeEnum.Callback:
                        case CallTypeEnum.CallOutput:
                            if (interfaceMethod.MethodAttribute.IsCallbackClient)
                            {
                                if (interfaceMethod.ReturnValueType == typeof(ResponseParameter))
                                {
                                    ReturnRequestParameterType = typeof(ResponseParameter);
                                    CallbackType = typeof(Action<ResponseResult<ResponseParameter>>);
                                }
                                else CallbackType = typeof(Action<>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType));
                                MethodReturnType = typeof(AutoCSer.Net.CallbackCommand);
                            }
                            else
                            {
                                if (interfaceMethod.ReturnValueType == typeof(ResponseParameter))
                                {
                                    ReturnRequestParameterType = typeof(ResponseParameter);
                                    MethodReturnType = typeof(ResponseParameterAwaiter<ResponseParameter>);
                                }
                                else MethodReturnType = typeof(ResponseParameterAwaiter<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            }
                            break;
                    }
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
