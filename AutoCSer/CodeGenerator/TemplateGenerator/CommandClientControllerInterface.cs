using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 客户端命令控制器接口
    /// </summary>
    [Generator(Name = "客户端命令控制器接口", IsAuto = true)]
    internal partial class CommandClientControllerInterface : AttributeGenerator<CommandServerControllerInterfaceAttribute>
    {
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected override string typeNameSuffix { get { return "ClientController"; } }
        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// 控制器方法信息
        /// </summary>
        public sealed class ControllerMethod
        {
            /// <summary>
            /// 接口方法信息
            /// </summary>
            private ServerInterfaceMethod interfaceMethod;
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
            /// 返回值XML文档注释
            /// </summary>
            public string CodeGeneratorReturnXmlDocument
            {
                get
                {
                    if (TwoStageReturnValueType == null)
                    {
                        return interfaceMethod.ReturnParameter == null ? Method.CodeGeneratorReturnXmlDocument : XmlDocument.CodeGeneratorFormat(XmlDocument.Get(Method.Method, interfaceMethod.ReturnParameter));
                    }
                    return string.Empty;
                }
            }
            /// <summary>
            /// 队列关键字类型
            /// </summary>
            public ExtensionType TaskQueueKeyType;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType ReturnValueType;
            /// <summary>
            /// 二阶段回调的第一阶段返回值类型
            /// </summary>
            public ExtensionType TwoStageReturnValueType;
            /// <summary>
            /// 二阶段回调的第一阶段回调的 XML 文档注释
            /// </summary>
            public string CallbackCodeGeneratorXmlDocument
            {
                get
                {
                    return XmlDocument.CodeGeneratorFormat(XmlDocument.Get(Method.Method, interfaceMethod.Parameters[interfaceMethod.Parameters.Length - 2]));
                }
            }
            /// <summary>
            /// 二阶段回调的第二阶段回调的 XML 文档注释
            /// </summary>
            public string KeepCallbackCodeGeneratorXmlDocument
            {
                get
                {
                    return XmlDocument.CodeGeneratorFormat(XmlDocument.Get(Method.Method, interfaceMethod.Parameters[interfaceMethod.Parameters.Length - 1]));
                }
            }
            /// <summary>
            /// 二阶段回调参数之前是否存在其它数据参数
            /// </summary>
            public bool IsTwoStageInputParameter
            {
                get { return TaskQueueKeyType != null || Method.ParameterCount != 0; }
            }
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceMethod"></param>
            public ControllerMethod(ServerInterfaceMethod interfaceMethod)
            {
                if (interfaceMethod != null)
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new Metadata.MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex, true);
                    TaskQueueKeyType = interfaceMethod.TaskQueueKeyType != null ? interfaceMethod.TaskQueueKeyType : (ExtensionType)null;
                    switch (interfaceMethod.MethodType)
                    {
                        case ServerMethodTypeEnum.KeepCallback:
                        case ServerMethodTypeEnum.KeepCallbackCount:
                        case ServerMethodTypeEnum.KeepCallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountQueue:
                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackTask:
                        case ServerMethodTypeEnum.KeepCallbackCountTask:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                        case ServerMethodTypeEnum.AsyncEnumerableTask:
                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                            MethodReturnType = interfaceMethod.ReturnValueType != typeof(void) ? typeof(AutoCSer.Net.EnumeratorCommand<>).MakeGenericType(interfaceMethod.ReturnValueType) : typeof(AutoCSer.Net.EnumeratorCommand);
                            break;
                        case ServerMethodTypeEnum.TwoStage‌Callback:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCount:
                        case ServerMethodTypeEnum.TwoStage‌CallbackQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackReadWriteQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountReadWriteQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackTask:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountTask:
                        case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                            TwoStageReturnValueType = interfaceMethod.TwoStageReturnValueType;
                            ReturnValueType = interfaceMethod.ReturnValueType;
                            MethodReturnType = typeof(AutoCSer.Net.KeepCallbackCommand);
                            break;
                        case ServerMethodTypeEnum.SendOnly:
                        case ServerMethodTypeEnum.SendOnlyQueue:
                        case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                        case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                        case ServerMethodTypeEnum.SendOnlyTask:
                        case ServerMethodTypeEnum.SendOnlyTaskQueue:
                            MethodReturnType = typeof(SendOnlyCommand);
                            break;
                        case ServerMethodTypeEnum.Unknown:
                            Messages.Error(Culture.Configuration.Default.GetCommandServerUnrecognizedMethod(interfaceMethod.Method, interfaceMethod.Error));
                            break;
                        default:
                            bool isRef = false;
                            foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter parameter in Method.Parameters)
                            {
                                if (parameter.IsRef || parameter.IsOut)
                                {
                                    isRef = true;
                                    break;
                                }
                            }
                            if (isRef) MethodReturnType = interfaceMethod.ReturnValueType != typeof(void) ? typeof(AutoCSer.Net.CommandClientReturnValue<>).MakeGenericType(interfaceMethod.ReturnValueType) : typeof(AutoCSer.Net.CommandClientReturnValue);
                            else MethodReturnType = interfaceMethod.ReturnValueType != typeof(void) ? typeof(AutoCSer.Net.ReturnCommand<>).MakeGenericType(interfaceMethod.ReturnValueType) : typeof(AutoCSer.Net.ReturnCommand);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 控制器方法集合
        /// </summary>
        public ControllerMethod[] Methods;
        /// <summary>
        /// A default value of true indicates that the default client controller configuration is generated
        /// 默认为 true 表示生成默认客户端控制器配置
        /// </summary>
#if AOT
        public bool IsCodeGeneratorControllerAttribute { get { return CurrentAttribute.IsCodeGeneratorControllerAttribute; } }
#else
        public bool IsCodeGeneratorControllerAttribute { get { return false; } }
#endif

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface || !CurrentAttribute.IsCodeGeneratorClientInterface) return AutoCSer.Common.CompletedTask;
            ServerInterfaceMethod[] methods = new ServerInterface(CurrentType.Type, null, null, true).Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new ControllerMethod(p));

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
