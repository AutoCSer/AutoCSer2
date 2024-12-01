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
    internal partial class CommandServerClientController : AttributeGenerator<CommandServerControllerInterfaceAttribute>
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
            /// 返回值XML文档注释
            /// </summary>
            public string ReturnXmlDocument
            {
                get
                {
                    return interfaceMethod.ReturnParameter == null ? Method.ReturnXmlDocument : XmlDocument.Get(Method.Method, interfaceMethod.ReturnParameter);
                }
            }
            /// <summary>
            /// 队列关键字类型
            /// </summary>
            public ExtensionType TaskQueueKeyType;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceMethod"></param>
            public ControllerMethod(ServerInterfaceMethod interfaceMethod)
            {
                if (interfaceMethod != null)
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                    TaskQueueKeyType = interfaceMethod.TaskQueueKeyType != null ? interfaceMethod.TaskQueueKeyType : (ExtensionType)null;
                    switch (interfaceMethod.MethodType)
                    {
                        case ServerMethodTypeEnum.KeepCallback:
                        case ServerMethodTypeEnum.KeepCallbackCount:
                        case ServerMethodTypeEnum.KeepCallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountQueue:
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
                        case ServerMethodTypeEnum.SendOnly:
                        case ServerMethodTypeEnum.SendOnlyQueue:
                        case ServerMethodTypeEnum.SendOnlyTask:
                        case ServerMethodTypeEnum.SendOnlyTaskQueue:
                            MethodReturnType = typeof(SendOnlyCommand);
                            break;
                        //case ServerMethodTypeEnum.Synchronous:
                        //case ServerMethodTypeEnum.Callback:
                        //case ServerMethodTypeEnum.Queue:
                        //case ServerMethodTypeEnum.CallbackQueue:
                        //case ServerMethodTypeEnum.Task:
                        //case ServerMethodTypeEnum.TaskQueue:
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
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface || !CurrentAttribute.IsCodeGeneratorClientInterface) return AutoCSer.Common.CompletedTask;
            ServerInterfaceMethod[] methods = new ServerInterface(CurrentType.Type, null).Methods;
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
