using AutoCSer.CodeGenerator.Metadata;
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
    /// 客户端命令控制器直接获取返回值封装
    /// </summary>
    [Generator(Name = "客户端命令控制器直接获取返回值封装", IsAuto = true)]
    internal partial class CommandClientReturnValueController : Generator, IGenerator
    {
        /// <summary>
        /// 直接返回值的控制器方法信息
        /// </summary>
        public sealed class ControllerMethod
        {
            /// <summary>
            /// Client interface method information
            /// 客户端接口方法信息
            /// </summary>
            private readonly ClientInterfaceMethod interfaceMethod;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public Metadata.MethodIndex Method;
            /// <summary>
            /// 方法名称
            /// </summary>
            public string MethodName { get { return Method.MethodName; } }
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool IsReturnValue
            {
                get
                {
                    switch (interfaceMethod.MethodType)
                    {
                        case ClientMethodTypeEnum.Synchronous:
                            return interfaceMethod.ReturnValueType != typeof(void);
                        case ClientMethodTypeEnum.SendOnly: return false;
                    }
                    return true;
                }
            }
            /// <summary>
            /// 是否需要调用直接获取返回值的方法
            /// </summary>
            public bool IsGetReturnValue
            {
                get
                {
                    switch (interfaceMethod.MethodType)
                    {
                        case ClientMethodTypeEnum.Synchronous: return !interfaceMethod.IsReturnType;
                        case ClientMethodTypeEnum.SendOnly:
                        case ClientMethodTypeEnum.Task:
                        case ClientMethodTypeEnum.Enumerator:
                        case ClientMethodTypeEnum.EnumeratorQueue:
#if NetStandard21
                        case ClientMethodTypeEnum.AsyncEnumerable:
#endif
                            return false;
                    }
                    return true;
                }
            }
            /// <summary>
            /// 是否同步调用方法
            /// </summary>
            public readonly bool IsSynchronous;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public readonly ExtensionType MethodReturnType;
            /// <summary>
            /// 回调返回值参数类型
            /// </summary>
            public readonly ExtensionType CallbackType;
            /// <summary>
            /// 直接获取返回值的回调委托类型
            /// </summary>
            public readonly ExtensionType CallbackReturnValueType;
            /// <summary>
            /// 回调参数名称
            /// </summary>
            public readonly string CallbackParameterName;
            /// <summary>
            /// 回调委托参数之前是否存在其它参数
            /// </summary>
            public bool IsJoinCallback { get { return Method.Parameters.Length != 0; } }
            /// <summary>
            /// 错误状态回调参数名称
            /// </summary>
            public string ErrorCallbackParameterName { get { return "error_" + CallbackParameterName; } }
            /// <summary>
            /// 持续回调返回值参数类型
            /// </summary>
            public readonly ExtensionType KeepCallbackType;
            /// <summary>
            /// 直接获取返回值的持续回调委托类型
            /// </summary>
            public readonly ExtensionType KeepCallbackReturnValueType;
            /// <summary>
            /// 持续回调参数名称
            /// </summary>
            public readonly string KeepCallbackParameterName;
            /// <summary>
            /// 错误状态回调参数名称
            /// </summary>
            public string ErrorKeepCallbackParameterName { get { return "error_" + KeepCallbackParameterName; } }
            /// <summary>
            /// 持续回调委托参数之前是否存在其它参数
            /// </summary>
            public bool IsJoinKeepCallback { get { return IsJoinCallback || CallbackType != null; } }
            /// <summary>
            /// 二阶段回调的第一阶段回调委托是否带返回参数初始值
            /// </summary>
            public bool IsTwoStage‌ReturnValueParameter { get { return interfaceMethod.IsTwoStage‌ReturnValueParameter; } }
            /// <summary>
            /// 直接返回值的控制器方法信息
            /// </summary>
            /// <param name="interfaceMethod">Client interface method information
            /// 客户端接口方法信息</param>
            internal ControllerMethod(ClientInterfaceMethod interfaceMethod)
            {
                this.interfaceMethod = interfaceMethod;
                Method = new Metadata.MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                switch (interfaceMethod.MethodType)
                {
                    case ClientMethodTypeEnum.Synchronous:
                    case ClientMethodTypeEnum.SendOnly:
                        IsSynchronous = true;
                        MethodReturnType = interfaceMethod.ReturnValueType;
                        break;
                    case ClientMethodTypeEnum.ReturnValue:
                    case ClientMethodTypeEnum.ReturnValueQueue:
                        IsSynchronous = true;
                        if (interfaceMethod.ReturnValueType == typeof(void)) MethodReturnType = typeof(CommandReturnValue);
                        else MethodReturnType = typeof(CommandReturnValue<>).MakeGenericType(interfaceMethod.ReturnValueType);
                        break;
                    case ClientMethodTypeEnum.Task:
                    case ClientMethodTypeEnum.Enumerator:
                    case ClientMethodTypeEnum.EnumeratorQueue:
                        MethodReturnType = interfaceMethod.Method.ReturnType;
                        IsSynchronous = true;
                        break;
                    case ClientMethodTypeEnum.Callback:
                    case ClientMethodTypeEnum.CallbackQueue:
                        MethodReturnType = interfaceMethod.Method.ReturnType;
                        CallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                        if (interfaceMethod.ReturnValueType == typeof(void))
                        {
                            CallbackType = typeof(Action);
                            CallbackReturnValueType = typeof(ClientReturnValueCallback);
                        }
                        else
                        {
                            CallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            CallbackReturnValueType = typeof(ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                        }
                        break;
                    case ClientMethodTypeEnum.KeepCallback:
                    case ClientMethodTypeEnum.KeepCallbackQueue:
                        MethodReturnType = interfaceMethod.Method.ReturnType;
                        KeepCallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                        if (interfaceMethod.ReturnValueType == typeof(void))
                        {
                            KeepCallbackType = typeof(Action);
                            KeepCallbackReturnValueType = typeof(ClientReturnValueCallback);
                        }
                        else
                        {
                            KeepCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                            KeepCallbackReturnValueType = typeof(ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                        }
                        break;
                    case ClientMethodTypeEnum.TwoStageCallback:
                        MethodReturnType = interfaceMethod.Method.ReturnType;
                        CallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex].Name;
                        KeepCallbackParameterName = interfaceMethod.Parameters[interfaceMethod.ParameterEndIndex + 1].Name;
                        if (interfaceMethod.IsTwoStage‌ReturnValueParameter)
                        {
                            CallbackReturnValueType = CallbackType = typeof(CommandClientReturnValueParameterCallback<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType);
                        }
                        else
                        {
                            CallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType);
                            CallbackReturnValueType = typeof(ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.TwoStageReturnValueType);
                        }
                        KeepCallbackType = typeof(Action<>).MakeGenericType(interfaceMethod.ReturnValueType);
                        KeepCallbackReturnValueType = typeof(ClientReturnValueCallback<>).MakeGenericType(interfaceMethod.ReturnValueType);
                        break;
#if NetStandard21
                    case ClientMethodTypeEnum.AsyncEnumerable:
                        MethodReturnType = interfaceMethod.Method.ReturnType;
                        IsSynchronous = true;
                        break;
#endif

                }
            }
        }
        /// <summary>
        /// 控制器接口信息
        /// </summary>
        public sealed class ControllerInterface
        {
            /// <summary>
            /// 成员属性
            /// </summary>
            public readonly PropertyIndex Property;
            /// <summary>
            /// 属性名称
            /// </summary>
            public string MemberName { get { return Property.PropertyName; } }
            /// <summary>
            /// 直接返回值的客户端控制器封装类型名称
            /// </summary>
            public readonly string ReturnValueControllerTypeName;
            /// <summary>
            /// 获取直接返回值的客户端控制器实例的方法名称
            /// </summary>
            public string GetReturnValueControllerMethodName { get { return "Get" + ReturnValueControllerTypeName; } }
            /// <summary>
            /// 直接返回值的控制器方法信息集合
            /// </summary>
            public ControllerMethod[] Methods;
            /// <summary>
            /// 控制器接口信息
            /// </summary>
            /// <param name="property"></param>
            /// <param name="methodArray"></param>
            internal ControllerInterface(PropertyInfo property, ref LeftArray<ClientInterfaceMethod> methodArray)
            {
                Property = new PropertyIndex(property);
                ReturnValueControllerTypeName = MemberName + "ReturnValueController";
                Methods = methodArray.Where(p => p.MethodAttribute.IsCodeGeneratorReturnValueController)
                    .Select(p => new ControllerMethod(p)).ToArray();
            }
        }
        /// <summary>
        /// 控制器接口信息集合
        /// </summary>
        public ControllerInterface[] ControllerInterfaces;

        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="attribute">代码生成器配置</param>
        /// <returns>是否生成成功</returns>
        public Task<bool> Run(ProjectParameter parameter, GeneratorAttribute attribute)
        {
            ProjectParameter = parameter;
            generatorAttribute = attribute;
            assembly = parameter.Assembly;
            Type[] constructorParameterTypes = new Type[] { typeof(AutoCSer.Net.CommandClient) };
            object[] constructorParameters = new object[] { AutoCSer.Net.CommandClient.Null };
            LeftArray<ControllerInterface> interfaces = new LeftArray<ControllerInterface>(0);
            foreach (Type type in parameter.Types)
            {
                if (!type.IsAbstract && !type.IsGenericTypeDefinition && typeof(AutoCSer.Net.CommandClientSocketEvent).IsAssignableFrom(type))
                {
                    var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public, null, constructorParameterTypes, null);
                    if (constructor != null)
                    {
                        AutoCSer.Net.CommandClientSocketEvent client = (AutoCSer.Net.CommandClientSocketEvent)constructor.Invoke(constructorParameters);
                        if (client.IsCodeGeneratorReturnValueController)
                        {
                            IEnumerable<CommandClientControllerCreatorParameter> controllerCreatorParameters = client.ControllerCreatorParameters;
                            if (controllerCreatorParameters != null)
                            {
                                interfaces.Length = 0;
                                Dictionary<string, PropertyInfo> propertyNames;
                                Dictionary<HashObject<System.Type>, PropertyInfo> properties = client.GetControllerProperties(type, out propertyNames);
                                foreach (KeyValue<PropertyInfo, CommandClientControllerCreatorParameter> propertyParameter in client.GetControllerProperties(controllerCreatorParameters, properties, propertyNames))
                                {
                                    if (propertyParameter.Key.PropertyType.IsInterface && propertyParameter.Value.ServerInterfaceType != typeof(ServerInterface))
                                    {
                                        Type clientType = propertyParameter.Value.ClientInterfaceType;
                                        ServerInterface serverInterface = new ServerInterface(propertyParameter.Value.ServerInterfaceType, null, clientType);
                                        LeftArray<ClientInterfaceMethod> methodArray;
                                        Exception controllerConstructorException = null;
                                        string[] controllerConstructorMessages = null;
                                        if (serverInterface.GetClientMethods(clientType, null, ref controllerConstructorException, ref controllerConstructorMessages, out methodArray))
                                        {
                                            interfaces.Add(new ControllerInterface(propertyParameter.Key, ref methodArray));
                                        }
                                        else
                                        {
                                            if (controllerConstructorException != null) Messages.Exception(controllerConstructorException);
                                            if (controllerConstructorMessages != null)
                                            {
                                                foreach (string message in controllerConstructorMessages) Messages.Message(message);
                                            }
                                        }
                                    }
                                }
                                if (interfaces.Length != 0)
                                {
                                    ControllerInterfaces = interfaces.ToArray();
                                    CurrentType = type;
                                    create(true);
                                }
                            }
                        }
                    }
                }
            }
            return AutoCSer.Common.TrueCompletedTask;
        }
    }
}
