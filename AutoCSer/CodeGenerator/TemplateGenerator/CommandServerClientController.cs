using AutoCSer.BinarySerialize;
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
    /// 客户端命令控制器
    /// </summary>
    [Generator(Name = "客户端命令控制器", IsAuto = true)]
    internal partial class CommandServerClientController : AttributeGenerator<AutoCSer.CodeGenerator.CommandClientControllerAttribute>
    {
        /// <summary>
        /// 参数字段信息
        /// </summary>
        public sealed class ParameterField
        {
            /// <summary>
            /// 字段信息
            /// </summary>
            private readonly FieldInfo field;
            /// <summary>
            /// 参数信息
            /// </summary>
            private readonly ParameterInfo parameter;
            /// <summary>
            /// 参数类型
            /// </summary>
            public ExtensionType ParameterType;
            /// <summary>
            /// 参数名称
            /// </summary>
            public string ParameterName { get { return field.Name; } }
            /// <summary>
            /// 参数名称
            /// </summary>
            public string QueueKeyParameterName
            {
                get
                {
                    return parameter != null || field.Name != nameof(ServerReturnValue<int>.ReturnValue) ? ParameterName : ClientInterfaceMethod.QueueKeyParameterName;
                }
            }
            /// <summary>
            /// 是否输出参数
            /// </summary>
            public readonly bool IsOut;
            /// <summary>
            /// 是否 ref/out 参数
            /// </summary>
            public readonly bool IsRef;
            /// <summary>
            /// 是否返回值参数
            /// </summary>
            public bool IsReturnValue;
            /// <summary>
            /// 参数字段信息
            /// </summary>
            /// <param name="field"></param>
            /// <param name="methodParameters"></param>
            internal ParameterField(FieldInfo field, ParameterInfo[] methodParameters)
            {
                this.field = field;
                ParameterType = field.FieldType;
                foreach (ParameterInfo parameter in methodParameters)
                {
                    if (parameter.Name == field.Name)
                    {
                        this.parameter = parameter;
                        IsOut = parameter.IsOut;
                        IsRef = IsOut | parameter.ParameterType.IsByRef;
                        break;
                    }
                }
            }
            /// <summary>
            /// 是否返回值参数
            /// </summary>
            /// <param name="method"></param>
            internal void CheckIsReturnValue(ControllerMethod method)
            {
                if (ParameterName == nameof(ServerReturnValue<int>.ReturnValue) && field.FieldType == method.ClientInterfaceMethod.ReturnValueType) IsReturnValue = true;
            }
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        public sealed class ParameterType
        {
            /// <summary>
            /// 命令服务参数类型
            /// </summary>
            private readonly ServerMethodParameter parameter;
            /// <summary>
            /// 参数集合
            /// </summary>
            public readonly ParameterField[] Parameters;
            /// <summary>
            /// 参数类型名称
            /// </summary>
            public readonly string ParameterTypeName;
            /// <summary>
            /// 参数类型名称
            /// </summary>
            public readonly string ParameterTypeFullName;
            /// <summary>
            /// 返回值绑定参数
            /// </summary>
            public readonly ParameterField ReturnValueParameter;
            /// <summary>
            /// 是否支持简单序列化
            /// </summary>
            private bool isSimpleSerialize;
            /// <summary>
            /// 是否支持简单反序列化
            /// </summary>
            private bool isSimpleDeserialize;
            /// <summary>
            /// 是否支持二进制序列化
            /// </summary>
            private bool isBinarySerialize;
            /// <summary>
            /// 是否支持二进制反序列化
            /// </summary>
            private bool isBinaryDeserialize;
            /// <summary>
            /// 是否支持二进制序列化
            /// </summary>
            public bool IsBinarySerialize { get { return isBinarySerialize | isBinaryDeserialize; } }
            /// <summary>
            /// 序列化代码
            /// </summary>
            public string SerializeCode;
            /// <summary>
            /// 参数类型
            /// </summary>
            /// <param name="typeNamePrefix">类型名称前缀</param>
            /// <param name="parameter">命令服务参数类型</param>
            /// <param name="method"></param>
            /// <param name="isInput"></param>
            /// <param name="isReturnValueParameter"></param>
            internal ParameterType(string typeNamePrefix, ServerMethodParameter parameter, ControllerMethod method, bool isInput, ref bool isReturnValueParameter)
            {
                this.parameter = parameter;
                ParameterTypeName = "__" + (isInput ? "ip" : "op") + (method.MethodIndex).toString() + "__";
                ParameterTypeFullName = typeNamePrefix + ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ClientInterfaceMethod.Parameters));
                if (!isInput && method.ClientInterfaceMethod.ReturnValueParameterIndex >= 0)
                {
                    foreach (ParameterField parameterField in Parameters)
                    {
                        if (parameterField.ParameterName == nameof(ServerReturnValue<int>.ReturnValue))
                        {
                            ReturnValueParameter = parameterField;
                            isReturnValueParameter = true;
                            break;
                        }
                    }
                }
                SetSerialize(method.ClientInterfaceMethod, isInput);
                if (Parameters.Length == 1) Parameters[0].CheckIsReturnValue(method);
            }
            /// <summary>
            /// 复制参数类型
            /// </summary>
            /// <param name="parameterType"></param>
            /// <param name="method"></param>
            /// <param name="isInput"></param>
            /// <param name="isReturnValueParameter"></param>
            internal ParameterType(ParameterType parameterType, ControllerMethod method, bool isInput, ref bool isReturnValueParameter)
            {
                parameter = parameterType.parameter;
                ParameterTypeName = parameterType.ParameterTypeName;
                ParameterTypeFullName = parameterType.ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ClientInterfaceMethod.Parameters));
                if (!isInput && method.ClientInterfaceMethod.ReturnValueParameterIndex >= 0)
                {
                    foreach (ParameterField parameterField in Parameters)
                    {
                        if (parameterField.ParameterName == nameof(ServerReturnValue<int>.ReturnValue))
                        {
                            ReturnValueParameter = parameterField;
                            isReturnValueParameter = true;
                            break;
                        }
                    }
                }
                if (Parameters.Length == 1) Parameters[0].CheckIsReturnValue(method);
            }
            /// <summary>
            /// 设置序列化方式
            /// </summary>
            /// <param name="method"></param>
            /// <param name="isInput"></param>
            internal void SetSerialize(InterfaceMethodBase method, bool isInput)
            {
                if (isInput)
                {
                    if (method.IsSimpleSerializeParamter) isSimpleSerialize = true;
                    else isBinarySerialize = true;
                }
                else
                {
                    if (method.IsSimpleDeserializeParamter) isSimpleDeserialize = true;
                    else isBinaryDeserialize = true;
                }
            }
            /// <summary>
            /// 参数类型
            /// </summary>
            /// <param name="typeNamePrefix">类型名称前缀</param>
            /// <param name="parameter">命令服务参数类型</param>
            /// <param name="method"></param>
            internal ParameterType(string typeNamePrefix, ServerMethodParameter parameter, StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod method)
            {
                this.parameter = parameter;
                ParameterTypeName = "__ip" + (method.MethodIndex).toString() + "__";
                ParameterTypeFullName = typeNamePrefix + ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ServerNodeMethod.Parameters));
                if (method.ServerNodeMethod.IsPersistence) SetInputSerialize(method.ServerNodeMethod);
            }
            /// <summary>
            /// 复制参数类型
            /// </summary>
            /// <param name="typeNamePrefix">类型名称前缀</param>
            /// <param name="parameterType"></param>
            /// <param name="method"></param>
            internal ParameterType(string typeNamePrefix, ParameterType parameterType, StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod method)
            {
                parameter = parameterType.parameter;
                ParameterTypeName = parameterType.ParameterTypeName;
                ParameterTypeFullName = typeNamePrefix + parameterType.ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ServerNodeMethod.Parameters));
            }
            /// <summary>
            /// 参数类型
            /// </summary>
            /// <param name="typeNamePrefix">类型名称前缀</param>
            /// <param name="parameter">命令服务参数类型</param>
            /// <param name="method"></param>
            internal ParameterType(string typeNamePrefix, ServerMethodParameter parameter, StreamPersistenceMemoryDatabaseMethodParameterCreator.NodeMethod method)
            {
                this.parameter = parameter;
                ParameterTypeName = "__ip"  + (method.MethodIndex).toString() + "__";
                ParameterTypeFullName = typeNamePrefix + ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ServerNodeMethod.Parameters));
                if (method.ServerNodeMethod.IsPersistence) SetInputSerialize(method.ServerNodeMethod);
            }
            /// <summary>
            /// 复制参数类型
            /// </summary>
            /// <param name="typeNamePrefix">类型名称前缀</param>
            /// <param name="parameterType"></param>
            /// <param name="method"></param>
            internal ParameterType(string typeNamePrefix, ParameterType parameterType, StreamPersistenceMemoryDatabaseMethodParameterCreator.NodeMethod method)
            {
                parameter = parameterType.parameter;
                ParameterTypeName = parameterType.ParameterTypeName;
                ParameterTypeFullName = typeNamePrefix + parameterType.ParameterTypeName;
                Parameters = parameter.Fields.getArray(p => new ParameterField(p, method.ServerNodeMethod.Parameters));
            }
            /// <summary>
            /// 设置序列化方式
            /// </summary>
            /// <param name="method"></param>
            internal void SetInputSerialize(InterfaceMethodBase method)
            {
                if (method.IsSimpleDeserializeParamter) isSimpleDeserialize = isSimpleSerialize = true;
                else isBinaryDeserialize = isBinarySerialize = true;
            }
            /// <summary>
            /// 设置序列化代码
            /// </summary>
            internal void SetSerializeCode()
            {
                string simpleSerializeCode = null, binarySerializeCode = null;
                if (isSimpleSerialize | isSimpleDeserialize)
                {
                    simpleSerializeCode = new SimpleSerialize().Create(parameter.Type, ParameterTypeFullName, isSimpleSerialize, isSimpleDeserialize);
                }
                if (IsBinarySerialize)
                {
                    binarySerializeCode = new BinarySerialize().Create(parameter.Type, ParameterTypeFullName, isBinarySerialize, isBinaryDeserialize);
                }
                if(string.IsNullOrEmpty(simpleSerializeCode))
                {
                    if (!string.IsNullOrEmpty(binarySerializeCode)) SerializeCode = binarySerializeCode;
                }
                else
                {
                    if (string.IsNullOrEmpty(binarySerializeCode)) SerializeCode = simpleSerializeCode;
                    else SerializeCode = simpleSerializeCode + @"
" + binarySerializeCode;

                }
            }
        }
        /// <summary>
        /// 控制器方法
        /// </summary>
        public sealed class ControllerMethod
        {
            /// <summary>
            /// 客户端接口方法信息
            /// </summary>
            internal readonly ClientInterfaceMethod ClientInterfaceMethod;
            /// <summary>
            /// 成员方法
            /// </summary>
            public readonly MethodIndex Method;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType MethodReturnType;
            /// <summary>
            /// 方法是否存在返回值
            /// </summary>
            public bool IsMethodReturn { get { return MethodReturnType.Type != typeof(void); } }
            /// <summary>
            /// 返回值类型是否一致
            /// </summary>
            public bool IsReturnType { get { return ClientInterfaceMethod.IsReturnType; } }
            /// <summary>
            /// 方法定义接口类型
            /// </summary>
            public string MethodInterfaceTypeName;
            /// <summary>
            /// 方法名称
            /// </summary>
            public string MethodName { get { return Method.MethodName; } }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string Error { get { return ClientInterfaceMethod.Error; } }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string CodeGeneratorError { get { return ClientInterfaceMethod.Error.Replace(@"""", @""""""); } }
            /// <summary>
            /// 输入参数类型
            /// </summary>
            public readonly ParameterType InputParameterType;
            /// <summary>
            /// 输出参数类型
            /// </summary>
            public readonly ParameterType OutputParameterType;
            /// <summary>
            /// 返回值绑定参数名称
            /// </summary>
            public readonly string ReturnValueParameterName;
            /// <summary>
            /// 控制器命令调用方法名称
            /// </summary>
            public readonly string CallMethodName;
            /// <summary>
            /// 控制器命令调用方法泛型参数名称
            /// </summary>
            public readonly string GenericTypeName;
            /// <summary>
            /// 方法数组索引位置
            /// </summary>
            public readonly int MethodArrayIndex;
            /// <summary>
            /// 自定义命令序号
            /// </summary>
            public int MethodIndex { get { return ClientInterfaceMethod.MethodIndex; } }
            /// <summary>
            /// 匹配方法名称
            /// </summary>
            public string MatchMethodName { get { return ClientInterfaceMethod.MatchMethodName; } }
            /// <summary>
            /// 是否简单序列化输出数据
            /// </summary>
            public int IsSimpleSerializeParamter { get { return ClientInterfaceMethod.IsSimpleSerializeParamter ? 1 : 0; } }
            /// <summary>
            /// 是否简单反序列化输入数据
            /// </summary>
            public int IsSimpleDeserializeParamter { get { return ClientInterfaceMethod.IsSimpleDeserializeParamter ? 1 : 0; } }
            /// <summary>
            /// 客户端 await 等待返回值回调线程模式
            /// </summary>
            public string CallbackTypeString { get { return $"{typeof(ClientCallbackTypeEnum).fullName()}.{ClientInterfaceMethod.CallbackType.ToString()}"; } }
            /// <summary>
            /// 回调队列序号
            /// </summary>
            public int QueueIndex { get { return ClientInterfaceMethod.QueueIndex; } }
            /// <summary>
            /// 是否低优先级队列
            /// </summary>
            public int IsLowPriorityQueue { get { return ClientInterfaceMethod.IsLowPriorityQueue ? 1: 0; } }
            /// <summary>
            /// 超时秒数
            /// </summary>
            public int TimeoutSeconds { get { return ClientInterfaceMethod.TimeoutSeconds; } }
            /// <summary>
            /// 是否同步返回方法
            /// </summary>
            public bool IsSynchronous { get { return ClientInterfaceMethod.MethodType == ClientMethodTypeEnum.Synchronous; } }
            /// <summary>
            /// 接口方法是否返回 Task
            /// </summary>
            public bool IsReturnTask { get { return ClientInterfaceMethod.MethodType == ClientMethodTypeEnum.Task; } }
            /// <summary>
            /// 接口方法是否返回 AsyncEnumerable
            /// </summary>
            public bool IsAsyncEnumerable { get { return ClientInterfaceMethod.MethodType == ClientMethodTypeEnum.AsyncEnumerable; } }
            /// <summary>
            /// 回调参数名称
            /// </summary>
            public readonly string CallbackParameterName;
            /// <summary>
            /// 回调参数处理类型
            /// </summary>
            public readonly ExtensionType CallbackType;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public readonly ExtensionType ReturnValueType;
            /// <summary>
            /// 输出参数是否需要传参
            /// </summary>
            public readonly bool IsOutputParameter;
            /// <summary>
            /// 获取返回值委托是否需要传参
            /// </summary>
            public bool IsGetReturnValue { get { return !IsSynchronous && ReturnValueType != null; } }
            /// <summary>
            /// 控制器方法
            /// </summary>
            /// <param name="methodArrayIndex"></param>
            /// <param name="method">客户端接口方法信息</param>
            /// <param name="paramterTypes">参数类型集合</param>
            /// <param name="typeNamePrefix"></param>
            internal ControllerMethod(int methodArrayIndex, ClientInterfaceMethod method, Dictionary<HashObject<Type>, ParameterType> paramterTypes, string typeNamePrefix)
            {
                MethodArrayIndex = methodArrayIndex;
                ClientInterfaceMethod = method;
                MethodInterfaceTypeName = method.Method.DeclaringType.fullName();
                if (method.ReturnValueType != typeof(void)) ReturnValueType = method.ReturnValueType;
                Method = new MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.NonPublicInstance, 0);
                if (method.ReturnValueParameterIndex >= 0) ReturnValueParameterName = method.Parameters[method.ReturnValueParameterIndex].Name;
                MethodReturnType = method.Method.ReturnType;
                if (method.InputParameterType != null)
                {
                    bool isReturnValueParameter = false;
                    if (paramterTypes.TryGetValue(method.InputParameterType.Type, out InputParameterType))
                    {
                        InputParameterType.SetSerialize(method, true);
                        InputParameterType = new ParameterType(InputParameterType, this, true, ref isReturnValueParameter);
                    }
                    else paramterTypes.Add(method.InputParameterType.Type, InputParameterType = new ParameterType(typeNamePrefix, method.InputParameterType, this, true, ref isReturnValueParameter));
                }
                if (method.OutputParameterType != null || method.ReturnValueType != typeof(void))
                {
                    var outputParameterType = method.OutputParameterType ?? ServerMethodParameter.GetOrCreate(0, EmptyArray<ParameterInfo>.Array, method.ReturnValueType);
                    if (paramterTypes.TryGetValue(outputParameterType.Type, out OutputParameterType))
                    {
                        OutputParameterType.SetSerialize(method, false);
                        OutputParameterType = new ParameterType(OutputParameterType, this, false, ref IsOutputParameter);
                    }
                    else paramterTypes.Add(outputParameterType.Type, OutputParameterType = new ParameterType(typeNamePrefix, outputParameterType, this, false, ref IsOutputParameter));
                }
                switch (method.MethodType)
                {
                    case ClientMethodTypeEnum.Callback:
                    case ClientMethodTypeEnum.CallbackQueue:
                    case ClientMethodTypeEnum.KeepCallback:
                    case ClientMethodTypeEnum.KeepCallbackQueue:
                        CallbackParameterName = method.Parameters[method.ParameterEndIndex].Name;
                        if (method.IsCallbackAction)
                        {
                            if (method.ReturnValueType == typeof(void))
                            {
                                switch (method.MethodType)
                                {
                                    case ClientMethodTypeEnum.Callback: CallbackType = typeof(CommandClientCallback); break;
                                    case ClientMethodTypeEnum.KeepCallback: CallbackType = typeof(CommandClientKeepCallback); break;
                                    case ClientMethodTypeEnum.CallbackQueue: CallbackType = typeof(CommandClientCallbackQueueNode); break;
                                    case ClientMethodTypeEnum.KeepCallbackQueue: CallbackType = typeof(CommandClientKeepCallbackQueue); break;
                                }
                            }
                            else
                            {
                                switch (method.MethodType)
                                {
                                    case ClientMethodTypeEnum.Callback: CallbackType = typeof(AutoCSer.Net.CommandClientCallback<>).MakeGenericType(method.ReturnValueType); break;
                                    case ClientMethodTypeEnum.KeepCallback: CallbackType = typeof(AutoCSer.Net.CommandClientKeepCallback<>).MakeGenericType(method.ReturnValueType); break;
                                    case ClientMethodTypeEnum.CallbackQueue: CallbackType = typeof(AutoCSer.Net.CommandClientCallbackQueueNode<>).MakeGenericType(method.ReturnValueType); break;
                                    case ClientMethodTypeEnum.KeepCallbackQueue: CallbackType = typeof(AutoCSer.Net.CommandClientKeepCallbackQueue<>).MakeGenericType(method.ReturnValueType); break;
                                }
                            }
                        }
                        break;
                }
                switch (method.MethodType)
                {
                    case ClientMethodTypeEnum.Callback:
                    case ClientMethodTypeEnum.CallbackQueue:
                    case ClientMethodTypeEnum.KeepCallback:
                    case ClientMethodTypeEnum.KeepCallbackQueue:
                    case ClientMethodTypeEnum.ReturnValue:
                    case ClientMethodTypeEnum.Task:
                    case ClientMethodTypeEnum.ReturnValueQueue:
                    case ClientMethodTypeEnum.Enumerator:
                    case ClientMethodTypeEnum.AsyncEnumerable:
                    case ClientMethodTypeEnum.EnumeratorQueue:
                        if (InputParameterType == null)
                        {
                            if (method.ReturnValueType != typeof(void)) GenericTypeName = $"{method.ReturnValueType.fullName()}, {OutputParameterType.ParameterTypeName}";
                        }
                        else
                        {
                            if (method.ReturnValueType == typeof(void)) GenericTypeName = InputParameterType.ParameterTypeName;
                            else GenericTypeName = $"{InputParameterType.ParameterTypeName}, {method.ReturnValueType.fullName()}, {OutputParameterType.ParameterTypeName}";
                        }
                        break;
                }
                switch (method.MethodType)
                {
                    case ClientMethodTypeEnum.Synchronous:
                        if (InputParameterType == null)
                        {
                            if (OutputParameterType == null) CallMethodName = nameof(CommandClientController.Synchronous);
                            else
                            {
                                IsOutputParameter = true;
                                CallMethodName = nameof(CommandClientController.SynchronousOutput);
                                GenericTypeName = OutputParameterType.ParameterTypeName;
                            }
                        }
                        else
                        {
                            if (OutputParameterType == null)
                            {
                                CallMethodName = nameof(CommandClientController.SynchronousInput);
                                GenericTypeName = InputParameterType.ParameterTypeName;
                            }
                            else
                            {
                                IsOutputParameter = true;
                                CallMethodName = nameof(CommandClientController.SynchronousInputOutput);
                                GenericTypeName = $"{InputParameterType.ParameterTypeName}, {OutputParameterType.ParameterTypeName}";
                            }
                        }
                        break;
                    case ClientMethodTypeEnum.SendOnly:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.SendOnly);
                        else
                        {
                            CallMethodName = nameof(CommandClientController.SendOnlyInput);
                            GenericTypeName = InputParameterType.ParameterTypeName;
                        }
                        break;
                    case ClientMethodTypeEnum.Callback:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.Callback);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.CallbackInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.CallbackOutput);
                        else CallMethodName = nameof(CommandClientController.CallbackOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.KeepCallback:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.KeepCallback);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.KeepCallbackInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.KeepCallbackOutput);
                        else CallMethodName = nameof(CommandClientController.KeepCallbackOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.CallbackQueue:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.CallbackQueue);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.CallbackQueueInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.CallbackQueueOutput);
                        else CallMethodName = nameof(CommandClientController.CallbackQueueOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.KeepCallbackQueue:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.KeepCallbackQueue);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.KeepCallbackQueueInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.KeepCallbackQueueOutput);
                        else CallMethodName = nameof(CommandClientController.KeepCallbackQueueOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.ReturnValue:
                    case ClientMethodTypeEnum.Task:
                        if (InputParameterType == null)
                        {
                            if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.ReturnType);
                            else CallMethodName = nameof(CommandClientController.ReturnValue);
                        }
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.ReturnTypeInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.ReturnValueOutput);
                        else CallMethodName = nameof(CommandClientController.ReturnValueOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.ReturnValueQueue:
                        if (InputParameterType == null)
                        {
                            if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.ReturnTypeQueue);
                            else CallMethodName = nameof(CommandClientController.ReturnValueQueue);
                        }
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.ReturnTypeQueueInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.ReturnValueQueueOutput);
                        else CallMethodName = nameof(CommandClientController.ReturnValueQueueOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.Enumerator:
                    case ClientMethodTypeEnum.AsyncEnumerable:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.Enumerator);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.EnumeratorInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.EnumeratorOutput);
                        else CallMethodName = nameof(CommandClientController.EnumeratorOutputReturnValue);
                        break;
                    case ClientMethodTypeEnum.EnumeratorQueue:
                        if (InputParameterType == null) CallMethodName = nameof(CommandClientController.EnumeratorQueue);
                        else if (method.ReturnValueType == typeof(void)) CallMethodName = nameof(CommandClientController.EnumeratorQueueInput);
                        else if (method.ReturnValueParameterIndex < 0) CallMethodName = nameof(CommandClientController.EnumeratorQueueOutput);
                        else CallMethodName = nameof(CommandClientController.EnumeratorQueueOutputReturnValue);
                        break;
                }
            }
        }

        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// 命令客户端控制器构造函数调用方法名称
        /// </summary>
        public string CommandClientControllerConstructorMethodName { get { return CommandClientControllerAttribute.CommandClientControllerConstructorMethodName; } }
        /// <summary>
        /// 获取客户端接口方法信息方法名称
        /// </summary>
        public string CommandClientControllerMethodName { get { return CommandClientControllerAttribute.CommandClientControllerMethodName; } }
        /// <summary>
        /// 当前接口类型名称
        /// </summary>
        public string InterfaceTypeName { get { return CurrentType.Type.Name; } }
        /// <summary>
        /// 参数类型集合
        /// </summary>
        public ParameterType[] ParameterTypes;
        /// <summary>
        /// 当前类型名称
        /// </summary>
        public new string TypeName;
        /// <summary>
        /// 服务端接口类型
        /// </summary>
        public ExtensionType ServerType;
        /// <summary>
        /// 枚举类型
        /// </summary>
        public ExtensionType EnumType;
        /// <summary>
        /// 非对称服务返回 false
        /// </summary>
        public bool IsServerType { get { return ServerType.Type != typeof(AutoCSer.Net.CommandServer.ServerInterface); } }
        /// <summary>
        /// 控制器方法
        /// </summary>
        public ControllerMethod[] Methods;
        /// <summary>
        /// 控制器方法数量
        /// </summary>
        public int MethodCount { get { return Methods.Length; } }
        /// <summary>
        /// 验证方法序号
        /// </summary>
        public int VerifyMethodIndex;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type, serverType = CurrentAttribute.ServerInterfaceType;
            if (type.IsGenericType) return AutoCSer.Common.CompletedTask;
            if (!type.IsInterface)
            {
                Messages.Error(AutoCSer.Common.Culture.GetNotInterfaceType(type));
                return AutoCSer.Common.CompletedTask;
            }
            if (serverType == typeof(void))
            {
                serverType = typeof(AutoCSer.Net.CommandServer.ServerInterface);
                EnumType = null;
            }
            else
            {
                if (serverType.IsGenericType) return AutoCSer.Common.CompletedTask;
                if (!serverType.IsInterface)
                {
                    Messages.Error(AutoCSer.Common.Culture.GetNotInterfaceType(serverType));
                    return AutoCSer.Common.CompletedTask;
                }
                Type enumType = ((CommandServerControllerInterfaceAttribute)serverType.GetCustomAttribute(typeof(CommandServerControllerInterfaceAttribute), false))?.MethodIndexEnumType;
                if (enumType != null) EnumType = enumType;
            }
            ServerInterface serverInterface = new ServerInterface(serverType, null, type);
            LeftArray<ClientInterfaceMethod> methodArray;
            Exception controllerConstructorException = null;
            string[] controllerConstructorMessages = null;
            bool isClient = serverInterface.GetClientMethods(type, null, ref controllerConstructorException, ref controllerConstructorMessages, out methodArray);
            if (controllerConstructorMessages != null) Messages.Error($"{type.fullName()} 控制器代码生成警告 {string.Join("\r\n", controllerConstructorMessages)}");
            if (!isClient)
            {
                if (controllerConstructorException != null) Messages.Error($"{type.fullName()} 控制器代码生成失败 {controllerConstructorException.Message}");
                return AutoCSer.Common.CompletedTask;
            }
            if (EnumType == null)
            {
                Type enumType = ((CommandServerControllerInterfaceAttribute)type.GetCustomAttribute(typeof(CommandServerControllerInterfaceAttribute), false))?.MethodIndexEnumType;
                if (enumType != null) EnumType = enumType;
            }
            VerifyMethodIndex = serverInterface.VerifyMethodIndex;
            if (string.IsNullOrEmpty(CurrentAttribute.ControllerTypeName))
            {
                TypeName = InterfaceTypeName;
                if (TypeName.Length > 1 && TypeName[0] == 'I' && (uint)(TypeName[1] - 'A') < 26) TypeName = TypeName.Substring(1);
                if (TypeName.EndsWith("Controller", StringComparison.Ordinal)) TypeName = TypeName.Substring(0, TypeName.Length - "Controller".Length);
                if (TypeName.EndsWith("Client", StringComparison.Ordinal)) TypeName = TypeName.Substring(0, TypeName.Length - "Client".Length);
                TypeName += "ClientController";
            }
            else TypeName = CurrentAttribute.ControllerTypeName;
            string typeName = type.fullName(), typeNamePrefix = typeName.Substring(0, typeName.LastIndexOf('.') + 1) + TypeName + ".";
            ServerType = serverType;
            Dictionary<HashObject<Type>, ParameterType> paramterTypes = DictionaryCreator.CreateHashObject<Type, ParameterType>(methodArray.Length << 1);
            int methodArrayIndex = 0;
            Methods = methodArray.GetArray(p => new ControllerMethod(methodArrayIndex++, p, paramterTypes, typeNamePrefix));
            ParameterTypes = paramterTypes.Values.ToArray();
            foreach (ParameterType parameterType in ParameterTypes) parameterType.SetSerializeCode();
            create(true);
            AotMethod.Append(typeNamePrefix + CommandClientControllerConstructorMethodName);
            AotMethod.IsCallAutoCSerAotMethod = true;
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
