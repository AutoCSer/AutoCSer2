using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务接口信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ServerInterface
    {
        /// <summary>
        /// 命令控制器配置
        /// </summary>
        internal CommandServerControllerInterfaceAttribute ControllerAttribute;
        /// <summary>
        /// The command service controller interface generates configuration
        /// 命令服务控制器接口生成配置
        /// </summary>
        internal ServerControllerInterfaceAttribute GeneratorAttribute;
        /// <summary>
        /// 服务端接口方法信息集合
        /// </summary>
#if NetStandard21
        internal ServerInterfaceMethod?[]? Methods;
#else
        internal ServerInterfaceMethod[] Methods;
#endif
        /// <summary>
        /// 异步队列字段
        /// </summary>
#if NetStandard21
        internal Dictionary<HashObject<System.Type>, FieldBuilder?>? TaskQueueFieldBuilders;
#else
        internal Dictionary<HashObject<System.Type>, FieldBuilder> TaskQueueFieldBuilders;
#endif
        /// <summary>
        /// 服务共享同步队列标记
        /// </summary>
        internal byte[] Queues;
        /// <summary>
        /// 服务共享同步队列数量
        /// </summary>
        internal int QueueCount;
        /// <summary>
        /// 验证方法序号
        /// </summary>
        internal int VerifyMethodIndex;
        /// <summary>
        /// 控制器同步队列标记
        /// </summary>
        internal byte ControllerQueue;
        /// <summary>
        /// 控制器读写队列标记
        /// </summary>
        internal bool ControllerConcurrencyReadQueue;
        /// <summary>
        /// 服务共享读写队列标记
        /// </summary>
        internal bool IsConcurrencyReadQueue;
        /// <summary>
        /// 控制器读写队列标记
        /// </summary>
        internal bool ControllerReadWriteQueue;
        /// <summary>
        /// 服务共享读写队列标记
        /// </summary>
        internal bool IsReadWriteQueue;
        /// <summary>
        /// 提示信息集合
        /// </summary>
        internal LeftArray<string> Messages;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        internal string? Error;
#else
        internal string Error;
#endif
        /// <summary>
        /// 服务接口信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="taskQueueControllerKeyType"></param>
        /// <param name="clientType"></param>
        /// <param name="isGenericTypeDefinition"></param>
#if NetStandard21
        internal ServerInterface(Type type, Type? taskQueueControllerKeyType, Type? clientType = null, bool isGenericTypeDefinition = false)
#else
        internal ServerInterface(Type type, Type taskQueueControllerKeyType, Type clientType = null, bool isGenericTypeDefinition = false)
#endif
        {
            Methods = null;
            VerifyMethodIndex = int.MinValue;
            ControllerQueue = 0;
            IsReadWriteQueue = ControllerReadWriteQueue = IsConcurrencyReadQueue = ControllerConcurrencyReadQueue = false;
            QueueCount = 0;
            Queues = EmptyArray<byte>.Array;
            TaskQueueFieldBuilders = null;
            Messages = new LeftArray<string>(0);

            Type getAttributeType = type == typeof(ServerInterface) ? clientType.notNull() : type;
            GeneratorAttribute = ServerControllerInterfaceAttribute.Default;
            ControllerAttribute = InterfaceController.GetCommandControllerAttribute(getAttributeType, isGenericTypeDefinition, out Error);
            if (Error != null) return;
            if (clientType != null && clientType != type && !InterfaceController.CheckType(clientType, isGenericTypeDefinition, out Error)) return;
            if (taskQueueControllerKeyType != null && ControllerAttribute.TaskQueueMaxConcurrent != 1)
            {
                if (object.ReferenceEquals(ControllerAttribute, CommandServerController.DefaultAttribute))
                {
                    ControllerAttribute =  ControllerAttribute.Clone();
                }
                ControllerAttribute.TaskQueueMaxConcurrent = 1;
            }
            GeneratorAttribute = getAttributeType.GetCustomAttribute<ServerControllerInterfaceAttribute>(false) ?? ServerControllerInterfaceAttribute.Default;
            if (type == typeof(ServerInterface)) return;

            LeftArray<ServerInterfaceMethod> methodArray = new LeftArray<ServerInterfaceMethod>(0);
            Error = ServerInterfaceMethod.GetMethod(type, ControllerAttribute, taskQueueControllerKeyType, !isGenericTypeDefinition, ref methodArray);
            if (Error != null) return;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                Error = ServerInterfaceMethod.GetMethod(interfaceType, ControllerAttribute, taskQueueControllerKeyType, !isGenericTypeDefinition, ref methodArray);
                if (Error != null) return;
            }
            if (methodArray.Length == 0)
            {
                Error = $"没有找到接口方法定义 {type.fullName()}";
                return;
            }

            methodArray.Sort(InterfaceMethodBase.Compare);
            Error = InterfaceMethodBase.CheckMethodIndexs(type, ControllerAttribute, GeneratorAttribute.MethodIndexEnumType, ref methodArray, ref Messages, out Methods);
            if (Error != null) return;
            int methodIndex = 0;
            foreach (var method in Methods)
            {
                if (method != null)
                {
                    if (method.ReturnValueType == typeof(CommandServerVerifyStateEnum))
                    {
                        if (method.InputParameterCount == 0)
                        {
                            Error = $"{type.fullName()} 验证接口 {method.Method.Name} 缺少输入参数";
                            return;
                        }
                        switch (method.MethodType)
                        {
                            case ServerMethodTypeEnum.Synchronous:
                            case ServerMethodTypeEnum.Callback:
                            case ServerMethodTypeEnum.Queue:
                            case ServerMethodTypeEnum.CallbackQueue:
                            case ServerMethodTypeEnum.ConcurrencyReadQueue:
                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.ReadWriteQueue:
                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.Task:
                            case ServerMethodTypeEnum.CallbackTask:
                            case ServerMethodTypeEnum.TaskQueue:
                            case ServerMethodTypeEnum.CallbackTaskQueue:
                                if (VerifyMethodIndex < 0) VerifyMethodIndex = methodIndex;
                                else
                                {
                                    Error = $"{type.fullName()} 验证接口冲突 {method.Method.Name} + {Methods[VerifyMethodIndex]?.Method.Name}";
                                    return;
                                }
                                break;
                        }
                    }
                    if (clientType == null && taskQueueControllerKeyType == null)
                    {
                        switch (method.MethodType)
                        {
                            case ServerMethodTypeEnum.Queue:
                            case ServerMethodTypeEnum.SendOnlyQueue:
                            case ServerMethodTypeEnum.CallbackQueue:
                            case ServerMethodTypeEnum.KeepCallbackQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackCountQueue:
                                byte queueIndex = method.MethodAttribute.QueueIndex;
                                if (queueIndex == 0)
                                {
                                    ControllerQueue |= 1;
                                    if (method.IsLowPriorityQueue) ControllerQueue |= 2;
                                }
                                else
                                {
                                    if (Queues.Length == 0) Queues = new byte[256];
                                    if (Queues[queueIndex] == 0) ++QueueCount;
                                    Queues[queueIndex] |= 1;
                                    if (method.IsLowPriorityQueue) Queues[queueIndex] |= 2;
                                }
                                break;
                            case ServerMethodTypeEnum.ConcurrencyReadQueue:
                            case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackCountConcurrencyReadQueue:
                                if (method.MethodAttribute.IsControllerConcurrencyReadQueue) ControllerConcurrencyReadQueue = true;
                                else IsConcurrencyReadQueue = true;
                                break;
                            case ServerMethodTypeEnum.ReadWriteQueue:
                            case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackCountReadWriteQueue:
                                if (method.MethodAttribute.IsControllerReadWriteQueue) ControllerReadWriteQueue = true;
                                else IsReadWriteQueue = true;
                                break;
                            case ServerMethodTypeEnum.TaskQueue:
                            case ServerMethodTypeEnum.CallbackTaskQueue:
                            case ServerMethodTypeEnum.SendOnlyTaskQueue:
                            case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                            case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                            case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                            case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                if (ControllerAttribute.TaskQueueMaxConcurrent <= 0 || !method.MethodAttribute.IsControllerTaskQueue)
                                {
                                    if (method.TaskQueueKeyType == null)
                                    {
                                        Error = $"{type.fullName()}.{method.Method.Name} 缺少队列关键字类型";
                                        return;
                                    }
#if NetStandard21
                                    if (TaskQueueFieldBuilders == null) TaskQueueFieldBuilders = DictionaryCreator.CreateHashObject<System.Type, FieldBuilder?>();
#else
                                    if (TaskQueueFieldBuilders == null) TaskQueueFieldBuilders = DictionaryCreator.CreateHashObject<System.Type, FieldBuilder>();
#endif
                                    TaskQueueFieldBuilders[method.TaskQueueKeyType] = null;
                                }
                                break;
                        }
                    }
                }
                ++methodIndex;
            }
        }
        /// <summary>
        /// 客户端默认初始化接口信息
        /// </summary>
        /// <param name="type"></param>
        internal ServerInterface(Type type)
        {
            Methods = null;
            VerifyMethodIndex = int.MinValue;
            ControllerQueue = 0;
            IsReadWriteQueue = ControllerReadWriteQueue = IsConcurrencyReadQueue = ControllerConcurrencyReadQueue = false;
            QueueCount = 0;
            Queues = EmptyArray<byte>.Array;
            TaskQueueFieldBuilders = null;
            Messages = new LeftArray<string>(0);
            GeneratorAttribute = ServerControllerInterfaceAttribute.Default;
            ControllerAttribute = CommandServerController.DefaultAttribute;
            InterfaceController.CheckType(type, false, out Error);
        }
        /// <summary>
        /// 获取方法分组
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal Dictionary<InterfaceMethod, HeadLeftArray<ServerInterfaceMethod>>? GetMethodGroup()
#else
        internal Dictionary<InterfaceMethod, HeadLeftArray<ServerInterfaceMethod>> GetMethodGroup()
#endif
        {
            if (Methods == null) return null;
            Dictionary<InterfaceMethod, HeadLeftArray<ServerInterfaceMethod>> methodGroup = DictionaryCreator<InterfaceMethod>.Create<HeadLeftArray<ServerInterfaceMethod>>(Methods.Length);
            foreach (var method in Methods)
            {
                if (method != null)
                {
                    HeadLeftArray<ServerInterfaceMethod> methodArray;
                    if (methodGroup.TryGetValue(method, out methodArray))
                    {
                        methodArray.Add(method);
                        methodGroup[method] = methodArray;
                    }
                    else methodGroup.Add(method, new HeadLeftArray<ServerInterfaceMethod>(method));
                }
            }
            return methodGroup;
        }
        /// <summary>
        /// 获取客户端方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyType"></param>
        /// <param name="controllerConstructorException"></param>
        /// <param name="controllerConstructorMessages"></param>
        /// <param name="methodArray"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool GetClientMethods(Type type, Type? keyType, ref Exception? controllerConstructorException, ref string[]? controllerConstructorMessages, out LeftArray<ClientInterfaceMethod> methodArray)
#else
        internal bool GetClientMethods(Type type, Type keyType, ref Exception controllerConstructorException, ref string[] controllerConstructorMessages, out LeftArray<ClientInterfaceMethod> methodArray)
#endif
        {
            methodArray = new LeftArray<ClientInterfaceMethod>(0);
            if (Error == null)
            {
                if (Messages.Length != 0) controllerConstructorMessages = Messages.ToArray();
                Error = ClientInterfaceMethod.GetMethod(type, ControllerAttribute, keyType, ref methodArray, Methods != null);
            }
            if (Error != null)
            {
                controllerConstructorException = new Exception($"{type.fullName()} 客户端控制器生成失败 {Error}");
                return false;
            }
            foreach (Type interfaceType in type.GetInterfaces())
            {
                Error = ClientInterfaceMethod.GetMethod(interfaceType, ControllerAttribute, keyType, ref methodArray, Methods != null);
                if (Error != null)
                {
                    controllerConstructorException = new Exception($"{type.fullName()} 客户端控制器生成失败 {Error}");
                    return false;
                }
            }
            if (methodArray.Length == 0)
            {
                controllerConstructorException = new Exception($"{type.fullName()} 客户端控制器生成失败 没有找到接口方法定义");
                return false;
            }
#if NetStandard21
            ClientInterfaceMethod?[] methods;
#else
            ClientInterfaceMethod[] methods;
#endif

            if (Methods == null)
            {
                if (GeneratorAttribute.MethodIndexEnumType != null)
                {
                    if (!GeneratorAttribute.MethodIndexEnumType.IsEnum)
                    {
                        controllerConstructorException = new Exception($"方法序号映射类型 {GeneratorAttribute.MethodIndexEnumType.fullName()} 必须为 enum 类型");
                        return false;
                    }
                    Array enums = System.Enum.GetValues(GeneratorAttribute.MethodIndexEnumType);
                    Dictionary<string, object> enumNames = new Dictionary<string, object>(enums.Length);
                    foreach (object value in enums) enumNames.Add(value.ToString().notNull(), value);
                    foreach (ClientInterfaceMethod method in methodArray)
                    {
                        if (method.MethodIndex < 0)
                        {
                            var value = default(object);
                            if (enumNames.TryGetValue(method.Method.Name, out value)) method.MethodIndex = ((IConvertible)value).ToInt32(null);
                        }
                    }
                }
                int methodIndex = -1;
                foreach (ClientInterfaceMethod method in methodArray)
                {
                    if (method.MethodIndex > methodIndex) methodIndex = method.MethodIndex;
                }
                methods = new ClientInterfaceMethod[methodIndex + 1];
            }
            else methods = new ClientInterfaceMethod[Methods.Length];
            int queueCount = 0, messageCount = Messages.Length;
            byte[] queues = EmptyArray<byte>.Array;
            var serverMethodGroup = GetMethodGroup();
            foreach (ClientInterfaceMethod method in methodArray)
            {
                if (Methods == null)
                {
                    if (method.MethodIndex >= 0) methods[method.MethodIndex] = method;
                    else
                    {
                        controllerConstructorException = new Exception($"{type.fullName()}.{method.Method.Name} 缺少命令序号");
                        return false;
                    }
                }
                else
                {
                    HeadLeftArray<ServerInterfaceMethod> serverMethodArray;
                    if (method.MethodIndex >= 0)
                    {
                        if (method.MethodIndex < Methods.Length)
                        {
                            var serverMethod = Methods[method.MethodIndex];
                            if (serverMethod != null)
                            {
                                if (method.CheckEquals(serverMethod))
                                {
                                    method.Set(serverMethod);
                                    methods[method.MethodIndex] = method;
                                }
                            }
                            else method.SetError($"{type.fullName()} 客户端控制器生成失败 客户端方法 {method.Method.Name} 没有找到对应的服务端命令序号 {method.MethodIndex}");
                        }
                        else method.SetError($"{type.fullName()} 客户端控制器生成失败 客户端方法 {method.Method.Name} 命令序号 {method.MethodIndex} 超出服务端命令序号范围");
                    }
                    else if (serverMethodGroup.notNull().TryGetValue(method, out serverMethodArray))
                    {
                        if (serverMethodArray.Count == 1)
                        {
                            ServerInterfaceMethod serverMethod = serverMethodArray.Head;
                            if (method.CheckEquals(serverMethod))
                            {
                                method.Set(serverMethod);
                                methods[method.MethodIndex] = method;
                            }
                        }
                        else method.SetError($"客户端方法 {method.Method.Name} 匹配到多个服务端方法 {string.Join(",", serverMethodArray.Values.Select(p => p.Method.Name))}");
                    }
                    else
                    {
                        method.SetError($"{type.fullName()} 客户端控制器生成失败 客户端方法 {method.Method.Name} 没有找到匹配的服务端方法");
                    }
                }
                if (method.Error == null)
                {
                    switch (method.MethodType)
                    {
                        case ClientMethodTypeEnum.CallbackQueue:
                        case ClientMethodTypeEnum.KeepCallbackQueue:
                        case ClientMethodTypeEnum.ReturnValueQueue:
                        case ClientMethodTypeEnum.EnumeratorQueue:
                            if (queues.Length == 0) queues = new byte[256];
                            byte queueIndex = method.MethodAttribute.QueueIndex;
                            if (queues[queueIndex] == 0) ++queueCount;
                            queues[queueIndex] |= 1;
                            if (method.MethodAttribute.IsLowPriorityQueue) queues[queueIndex] |= 2;
                            break;
                    }
                }
                else Messages.Add(method.Error);
            }
            if (messageCount != Messages.Length) controllerConstructorMessages = Messages.ToArray();
            return true;
        }
        /// <summary>
        /// 获取客户端方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyType"></param>
        /// <param name="controllerConstructorException"></param>
        /// <param name="controllerConstructorMessages"></param>
        /// <param name="methodArray"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool GetClientDefaultMethods(Type type, Type? keyType, ref Exception? controllerConstructorException, ref string[]? controllerConstructorMessages, out LeftArray<ClientInterfaceMethod> methodArray)
#else
        internal bool GetClientDefaultMethods(Type type, Type keyType, ref Exception controllerConstructorException, ref string[] controllerConstructorMessages, out LeftArray<ClientInterfaceMethod> methodArray)
#endif
        {
            methodArray = new LeftArray<ClientInterfaceMethod>(0);
            if (Error == null)
            {
                if (Messages.Length != 0) controllerConstructorMessages = Messages.ToArray();
                Error = ClientInterfaceMethod.GetMethod(type, ControllerAttribute, keyType, ref methodArray, false, true);
            }
            if (Error != null)
            {
                controllerConstructorException = new Exception($"{type.fullName()} 客户端默认初始化控制器生成失败 {Error}");
                return false;
            }
            foreach (Type interfaceType in type.GetInterfaces())
            {
                Error = ClientInterfaceMethod.GetMethod(interfaceType, ControllerAttribute, keyType, ref methodArray, false, true);
                if (Error != null)
                {
                    controllerConstructorException = new Exception($"{type.fullName()} 客户端默认初始化控制器生成失败 {Error}");
                    return false;
                }
            }
            if (methodArray.Length == 0)
            {
                controllerConstructorException = new Exception($"{type.fullName()} 客户端默认初始化控制器生成失败 没有找到接口方法定义");
                return false;
            }
            return true;
        }
    }
}
