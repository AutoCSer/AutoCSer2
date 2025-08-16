using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Client interface method information
    /// 客户端接口方法信息
    /// </summary>
    internal sealed class ClientInterfaceMethod : InterfaceMethod
    {
        /// <summary>
        /// 队列关键字参数名称
        /// </summary>
        internal const string QueueKeyParameterName = "queueKey";

        /// <summary>
        /// 对应的服务端接口方法信息，非对称定义为 null
        /// </summary>
#if NetStandard21
        internal ServerInterfaceMethod? ServerMethod;
#else
        internal ServerInterfaceMethod ServerMethod;
#endif
        /// <summary>
        /// Command client method configuration
        /// 命令客户端方法配置
        /// </summary>
        internal readonly CommandClientMethodAttribute MethodAttribute;
        /// <summary>
        /// Match the method name
        /// 匹配方法名称
        /// </summary>
        internal override string MatchMethodName { get { return MethodAttribute.MatchMethodName ?? base.MatchMethodName; } }
        /// <summary>
        /// 二阶段回调的第一阶段的返回数据类型
        /// </summary>
        internal readonly Type TwoStage‌ReturnValueType;
        /// <summary>
        /// 返回值输出参数索引
        /// </summary>
        internal int ReturnValueParameterIndex = -1;
        /// <summary>
        /// Client method call type
        /// 客户端方法调用类型
        /// </summary>
        internal ClientMethodTypeEnum MethodType;
        /// <summary>
        /// 是否 Action 回调参数
        /// </summary>
        internal readonly bool IsCallbackAction;

        /// <summary>
        /// Timeout seconds
        /// 超时秒数
        /// </summary>
        internal readonly ushort TimeoutSeconds;
        /// <summary>
        /// The client's await awaits the return value callback thread mode
        /// 客户端 await 等待返回值回调线程模式
        /// </summary>
        internal readonly ClientCallbackTypeEnum CallbackType;
        /// <summary>
        /// Call back the queue number
        /// 回调队列序号
        /// </summary>
        internal readonly byte QueueIndex;
        /// <summary>
        /// Whether it is a low-priority queue
        /// 是否低优先级队列
        /// </summary>
        internal readonly bool IsLowPriorityQueue;
        /// <summary>
        /// 二阶段回调的第一阶段回调委托是否带返回参数初始值
        /// </summary>
        internal readonly bool IsTwoStage‌ReturnValueParameter;
        /// <summary>
        /// Whether to simply serialize the return value of the first stage of the two-stage callback
        /// 是否简单序列化二阶段回调的第一阶段的返回值
        /// </summary>
        internal bool IsSimpleSerializeTwoStage‌ReturnValue;
        /// <summary>
        /// Whether deserialization failed
        /// 是否反序列化失败
        /// </summary>
        private bool isDeserializeError;
        /// <summary>
        /// Client interface method information
        /// 客户端接口方法信息
        /// </summary>
        internal ClientInterfaceMethod()
        {
            MethodIndex = int.MinValue;
            MethodAttribute = CommandClientMethodAttribute.Defafult;
            TwoStage‌ReturnValueType = typeof(void);
        }
        /// <summary>
        /// Client interface method information
        /// 客户端接口方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="controllerAttribute"></param>
        /// <param name="taskQueueControllerKeyType"></param>
        /// <param name="isServer"></param>
        /// <param name="isDefault"></param>
#if NetStandard21
        internal ClientInterfaceMethod(Type type, MethodInfo method, CommandServerControllerInterfaceAttribute controllerAttribute, Type? taskQueueControllerKeyType, bool isServer, bool isDefault) : base(type, method, controllerAttribute)
#else
        internal ClientInterfaceMethod(Type type, MethodInfo method, CommandServerControllerInterfaceAttribute controllerAttribute, Type taskQueueControllerKeyType, bool isServer, bool isDefault) : base(type, method, controllerAttribute)
#endif
        {
            MethodAttribute = method.GetCustomAttribute<CommandClientMethodAttribute>(false) ?? CommandClientMethodAttribute.Defafult;
            MethodIndex = MethodAttribute.MethodIndex;
            Parameters = method.GetParameters();
            ParameterEndIndex = Parameters.Length;
            ReturnValueType = method.ReturnType;
            TwoStage‌ReturnValueType = typeof(void);
            if (ReturnValueType.IsGenericType)
            {
                Type genericType = ReturnValueType.GetGenericTypeDefinition();
                if (genericType == typeof(AutoCSer.Net.ReturnCommand<>)) MethodType = ClientMethodTypeEnum.ReturnValue;
                else if (genericType == typeof(AutoCSer.Net.ReturnQueueCommand<>)) MethodType = ClientMethodTypeEnum.ReturnValueQueue;
                else if (genericType == typeof(AutoCSer.Net.EnumeratorCommand<>)) MethodType = ClientMethodTypeEnum.Enumerator;
                else if (genericType == typeof(AutoCSer.Net.EnumeratorQueueCommand<>)) MethodType = ClientMethodTypeEnum.EnumeratorQueue;
                else if (genericType == typeof(AutoCSer.Net.SendOnlyCommand<>)) MethodType = ClientMethodTypeEnum.SendOnly;
                else if (genericType == typeof(AutoCSer.Net.CommandClientReturnValue<>)) MethodType = ClientMethodTypeEnum.Synchronous;
                else if (genericType == typeof(Task<>)) MethodType = ClientMethodTypeEnum.Task;
#if NetStandard21
                else if (genericType == typeof(IAsyncEnumerable<>)) MethodType = ClientMethodTypeEnum.AsyncEnumerable;
#endif
                else
                {
                    if (taskQueueControllerKeyType != null)
                    {
                        SetError($"{type.fullName()}.{method.Name} 不可识别的返回值类型 {ReturnValueType.fullName()}");
                        return;
                    }
                    MethodType = ClientMethodTypeEnum.Synchronous;
                    IsReturnType = true;
                }
                if (!IsReturnType) ReturnValueType = ReturnValueType.GetGenericArguments()[0];
            }
            else
            {
                if (ReturnValueType == typeof(AutoCSer.Net.ReturnCommand)) MethodType = ClientMethodTypeEnum.ReturnValue;
                else if (ReturnValueType == typeof(AutoCSer.Net.ReturnQueueCommand)) MethodType = ClientMethodTypeEnum.ReturnValueQueue;
                else if (ReturnValueType == typeof(AutoCSer.Net.CallbackCommand)) MethodType = ClientMethodTypeEnum.Callback;
                else if (ReturnValueType == typeof(AutoCSer.Net.KeepCallbackCommand)) MethodType = ClientMethodTypeEnum.KeepCallback;
                else if (ReturnValueType == typeof(AutoCSer.Net.EnumeratorCommand)) MethodType = ClientMethodTypeEnum.Enumerator;
                else if (ReturnValueType == typeof(AutoCSer.Net.EnumeratorQueueCommand)) MethodType = ClientMethodTypeEnum.EnumeratorQueue;
                else if (ReturnValueType == typeof(AutoCSer.Net.SendOnlyCommand)) MethodType = ClientMethodTypeEnum.SendOnly;
                else if (ReturnValueType == typeof(AutoCSer.Net.CommandClientReturnValue)) MethodType = ClientMethodTypeEnum.Synchronous;
                else if (ReturnValueType == typeof(Task))
                {
                    MethodType = ClientMethodTypeEnum.Task;
                    //ReturnValueType = typeof(void);
                }
                else
                {
                    if (taskQueueControllerKeyType != null)
                    {
                        SetError($"{type.fullName()}.{method.Name} 不可识别的返回值类型 {ReturnValueType.fullName()}");
                        return;
                    }
                    MethodType = ClientMethodTypeEnum.Synchronous;
                    IsReturnType = true;
                }
                if (!IsReturnType) ReturnValueType = typeof(void);
            }
            switch (MethodType)
            {
                case ClientMethodTypeEnum.Callback:
                    bool isCallback = false;
                    if (ParameterEndIndex > ParameterStartIndex)
                    {
                        Type parameterType = Parameters[ParameterEndIndex - 1].ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (genericType == typeof(Action<>))
                            {
                                Type actionParamererType = parameterType.GetGenericArguments()[0];
                                if (actionParamererType.IsGenericType)
                                {
                                    if (actionParamererType.GetGenericTypeDefinition() == typeof(CommandClientReturnValue<>))
                                    {
                                        IsCallbackAction = isCallback = true;
                                        ReturnValueType = actionParamererType.GetGenericArguments()[0];
                                    }
                                }
                                else if (actionParamererType == typeof(CommandClientReturnValue)) IsCallbackAction = isCallback = true;
                            }
                            else if (genericType == typeof(Action<,>))
                            {
                                Type[] actionParamererTypes = parameterType.GetGenericArguments();
                                if (actionParamererTypes[1] == typeof(CommandClientCallQueue))
                                {
                                    Type actionParamererType = actionParamererTypes[0];
                                    if (actionParamererType.IsGenericType)
                                    {
                                        if (actionParamererType.GetGenericTypeDefinition() == typeof(CommandClientReturnValue<>))
                                        {
                                            MethodType = ClientMethodTypeEnum.CallbackQueue;
                                            IsCallbackAction = isCallback = true;
                                            ReturnValueType = actionParamererType.GetGenericArguments()[0];
                                        }
                                    }
                                    else if (actionParamererType == typeof(CommandClientReturnValue))
                                    {
                                        MethodType = ClientMethodTypeEnum.CallbackQueue;
                                        IsCallbackAction = isCallback = true;
                                    }
                                }
                            }
                            else if (genericType == typeof(AutoCSer.Net.CommandClientCallback<>))
                            {
                                isCallback = true;
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                            }
                            else if (genericType == typeof(AutoCSer.Net.CommandClientCallbackQueueNode<>))
                            {
                                isCallback = true;
                                MethodType = ClientMethodTypeEnum.CallbackQueue;
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                            }
                        }
                        else
                        {
                            if (parameterType == typeof(AutoCSer.Net.CommandClientCallback)) isCallback = true;
                            else if (parameterType == typeof(AutoCSer.Net.CommandClientCallbackQueueNode))
                            {
                                isCallback = true;
                                MethodType = ClientMethodTypeEnum.CallbackQueue;
                            }
                        }
                        --ParameterEndIndex;
                    }
                    if (!isCallback)
                    {
                        SetError($"回调方法{type.fullName()}.{method.Name} 缺少回调委托参数");
                        return;
                    }
                    break;
                case ClientMethodTypeEnum.KeepCallback:
                    bool checkTwoStage‌ = isCallback = false;
                    if (ParameterEndIndex > ParameterStartIndex)
                    {
                        Type parameterType = Parameters[ParameterEndIndex - 1].ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (genericType == typeof(Action<,>))
                            {
                                Type[] actionParamererTypes = parameterType.GetGenericArguments();
                                if (actionParamererTypes[1] == typeof(Net.KeepCallbackCommand))
                                {
                                    Type actionParamererType = actionParamererTypes[0];
                                    if (actionParamererType.IsGenericType)
                                    {
                                        if (actionParamererType.GetGenericTypeDefinition() == typeof(CommandClientReturnValue<>))
                                        {
                                            IsCallbackAction = isCallback = checkTwoStage‌ = true;
                                            ReturnValueType = actionParamererType.GetGenericArguments()[0];
                                        }
                                    }
                                    else if (actionParamererType == typeof(CommandClientReturnValue)) IsCallbackAction = isCallback = true;
                                }
                            }
                            else if (genericType == typeof(Action<,,>))
                            {
                                Type[] actionParamererTypes = parameterType.GetGenericArguments();
                                if (actionParamererTypes[1] == typeof(CommandClientCallQueue) && actionParamererTypes[2] == typeof(Net.KeepCallbackCommand))
                                {
                                    Type actionParamererType = actionParamererTypes[0];
                                    if (actionParamererType.IsGenericType)
                                    {
                                        if (actionParamererType.GetGenericTypeDefinition() == typeof(CommandClientReturnValue<>))
                                        {
                                            IsCallbackAction = isCallback = true;
                                            MethodType = ClientMethodTypeEnum.KeepCallbackQueue;
                                            ReturnValueType = actionParamererType.GetGenericArguments()[0];
                                        }
                                    }
                                    else if (actionParamererType == typeof(CommandClientReturnValue))
                                    {
                                        IsCallbackAction = isCallback = true;
                                        MethodType = ClientMethodTypeEnum.KeepCallbackQueue;
                                    }
                                }
                            }
                            else if (genericType == typeof(AutoCSer.Net.CommandClientKeepCallback<>))
                            {
                                isCallback = checkTwoStage‌ = true;
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                            }
                            else if (genericType == typeof(AutoCSer.Net.CommandClientKeepCallbackQueue<>))
                            {
                                isCallback = true;
                                MethodType = ClientMethodTypeEnum.KeepCallbackQueue;
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                            }
                        }
                        else
                        {
                            if (parameterType == typeof(AutoCSer.Net.CommandClientKeepCallback)) isCallback = true;
                            else if (parameterType == typeof(AutoCSer.Net.CommandClientKeepCallbackQueue))
                            {
                                MethodType = ClientMethodTypeEnum.KeepCallbackQueue;
                                isCallback = true;
                            }
                        }
                        --ParameterEndIndex;
                    }
                    if (!isCallback)
                    {
                        SetError($"回调方法{type.fullName()}.{method.Name} 缺少回调委托参数");
                        return;
                    }
                    if (checkTwoStage‌ && ParameterEndIndex > ParameterStartIndex)
                    {
                        Type parameterType = Parameters[ParameterEndIndex - 1].ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (IsCallbackAction)
                            {
                                if (genericType == typeof(Action<>))
                                {
                                    Type actionParamererType = parameterType.GetGenericArguments()[0];
                                    if (actionParamererType.IsGenericType && actionParamererType.GetGenericTypeDefinition() == typeof(CommandClientReturnValue<>))
                                    {
                                        TwoStage‌ReturnValueType = actionParamererType.GetGenericArguments()[0];
                                        --ParameterEndIndex;
                                        MethodType = ClientMethodTypeEnum.TwoStage‌Callback;
                                    }
                                }
                                else if (genericType == typeof(CommandClientReturnValueParameterCallback<>))
                                {
                                    TwoStage‌ReturnValueType = parameterType.GetGenericArguments()[0];
                                    --ParameterEndIndex;
                                    MethodType = ClientMethodTypeEnum.TwoStage‌Callback;
                                    IsTwoStage‌ReturnValueParameter = true;
                                }
                            }
                            else if (genericType == typeof(AutoCSer.Net.CommandClientCallback<>))
                            {
                                TwoStage‌ReturnValueType = parameterType.GetGenericArguments()[0];
                                --ParameterEndIndex;
                                MethodType = ClientMethodTypeEnum.TwoStage‌Callback;
                            }
                        }
                    }
                    break;
            }
            if (ReturnValueType != typeof(void) && ParameterEndIndex > ParameterStartIndex)
            {
                ParameterInfo parameter = Parameters[ParameterStartIndex];
                if (parameter.ParameterType == ReturnValueType && !parameter.ParameterType.IsByRef && string.Equals(parameter.Name, nameof(ServerReturnValue<int>.ReturnValue), StringComparison.OrdinalIgnoreCase))
                {
                    ReturnValueParameterIndex = ParameterStartIndex++;
                }
            }
            bool checkRef = false;
            if (taskQueueControllerKeyType == null)
            {
                switch (MethodType)
                {
                    case ClientMethodTypeEnum.Synchronous:
                        if (!checkSynchronousParameter())
                        {
                            MethodType = ClientMethodTypeEnum.Unknown;
                            return;
                        }
                        break;
                    default: checkRef = true; break;
                }
            }
            else checkRef = true;
            if (checkRef)
            {
                for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
                {
                    if (Parameters[parameterIndex].ParameterType.IsByRef)
                    {
                        SetError($"{MethodType} 接口 {type.fullName()}.{method.Name} 不允许下 ref / out 参数 {Parameters[parameterIndex].Name}");
                        return;
                    }
                }
            }
            if (!isDefault)
            {
                setParameterCount();
                if (InputParameterCount != 0 || taskQueueControllerKeyType != null)
                {
                    int parameterSkipCount = 0;
                    if (isServer)
                    {
                        if (taskQueueControllerKeyType != null)
                        {
                            InputParameterType = ServerMethodParameter.Get(InputParameterCount, InputParameters, taskQueueControllerKeyType);
                        }
                        else
                        {
                            ParameterInfo parameter = Parameters[ParameterStartIndex];
                            if (!parameter.ParameterType.IsByRef && parameter.Name == QueueKeyParameterName)
                            {
                                Type equatableType = typeof(IEquatable<>).MakeGenericType(parameter.ParameterType);
                                if (equatableType.IsAssignableFrom(parameter.ParameterType))
                                {
                                    taskQueueControllerKeyType = parameter.ParameterType;
                                    parameterSkipCount = 1;
                                }
                            }
                            if (taskQueueControllerKeyType == null) InputParameterType = ServerMethodParameter.Get(InputParameterCount, InputParameters, null);
                            else InputParameterType = ServerMethodParameter.Get(InputParameterCount - 1, InputParameters.Skip(parameterSkipCount), taskQueueControllerKeyType);
                        }
                        if (InputParameterType == null)
                        {
                            SetError($"接口 {type.fullName()}.{method.Name} 没有找到匹配的输入参数信息");
                            return;
                        }
                    }
                    else
                    {
                        InputParameterType = ServerMethodParameter.GetOrCreate(InputParameterCount, InputParameters, taskQueueControllerKeyType ?? typeof(void));
                        IsSimpleSerializeParamter = controllerAttribute.IsSimpleSerializeInputParameter && InputParameterType.IsSimpleSerialize;
                    }
                    InputParameterFields = InputParameterType.GetFields(InputParameters, parameterSkipCount == 0 ? null : QueueKeyParameterName);
                }
                if (OutputParameterCount == 0)
                {
                    if (ReturnValueType != typeof(void) && !isServer)
                    {
                        IsSimpleDeserializeParamter = controllerAttribute.IsSimpleSerializeOutputParameter && SimpleSerialize.Serializer.IsType(ReturnValueType);
                    }
                }
                else
                {
                    if (isServer)
                    {
                        OutputParameterType = ServerMethodParameter.Get(OutputParameterCount, OutputParameters, ReturnValueType);
                        if (OutputParameterType == null)
                        {
                            SetError($"接口 {type.fullName()}.{method.Name} 没有找到匹配的输出参数信息");
                            return;
                        }
                    }
                    else
                    {
                        OutputParameterType = ServerMethodParameter.GetOrCreate(OutputParameterCount, OutputParameters, ReturnValueType);
                        IsSimpleDeserializeParamter = controllerAttribute.IsSimpleSerializeOutputParameter && OutputParameterType.IsSimpleSerialize;
                    }
                    OutputParameterFields = OutputParameterType.GetFields(OutputParameters);
                }
            }
            CallbackType = MethodAttribute.CallbackType;
            switch (MethodType)
            {
                case ClientMethodTypeEnum.KeepCallback:
                case ClientMethodTypeEnum.KeepCallbackQueue:
                case ClientMethodTypeEnum.ReturnValueQueue:
                case ClientMethodTypeEnum.EnumeratorQueue:
#if NetStandard21
                case ClientMethodTypeEnum.AsyncEnumerable:
#endif
                    if (CallbackType == ClientCallbackTypeEnum.CheckRunTask) CallbackType = ClientCallbackTypeEnum.Synchronous;
                    break;
                case ClientMethodTypeEnum.TwoStage‌Callback: CallbackType = ClientCallbackTypeEnum.Synchronous; break;
                default: TimeoutSeconds = MethodAttribute.TimeoutSeconds; break;
            }
            QueueIndex = MethodAttribute.QueueIndex;
            IsLowPriorityQueue = MethodAttribute.IsLowPriorityQueue;
        }
        /// <summary>
        /// 反序列化错误
        /// </summary>
        /// <param name="controller"></param>
        internal void DeserializeError(CommandClientController controller)
        {
            if (!isDeserializeError)
            {
                isDeserializeError = true;
                controller.Socket.Client.Log.ErrorIgnoreException($"{controller.ControllerName} => {Type.fullName()}.{Method.Name} {nameof(DeserializeError)}");
            }
        }
        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="error"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetError(string error)
        {
            MethodType = ClientMethodTypeEnum.Unknown;
            Error = error;
        }
        /// <summary>
        /// 与服务端方法比较
        /// </summary>
        /// <param name="serverMethod"></param>
        /// <returns></returns>
        internal bool CheckEquals(ServerInterfaceMethod serverMethod)
        {
            if (Equals(serverMethod))
            {
                switch(MethodType)
                {
                    case ClientMethodTypeEnum.SendOnly:
                        switch(serverMethod.MethodType)
                        {
                            case ServerMethodTypeEnum.SendOnly:
                            case ServerMethodTypeEnum.SendOnlyQueue:
                            case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                            case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                            case ServerMethodTypeEnum.SendOnlyTask:
                            case ServerMethodTypeEnum.SendOnlyTaskQueue:
                                return true;
                        }
                        break;
                    case ClientMethodTypeEnum.Synchronous:
                        switch (serverMethod.MethodType)
                        {
                            case ServerMethodTypeEnum.Synchronous:
                            case ServerMethodTypeEnum.Queue:
                            case ServerMethodTypeEnum.ConcurrencyReadQueue:
                            case ServerMethodTypeEnum.ReadWriteQueue:
                                return true;
                            case ServerMethodTypeEnum.Callback:
                            case ServerMethodTypeEnum.CallbackTask:
                            case ServerMethodTypeEnum.CallbackQueue:
                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.Task:
                            case ServerMethodTypeEnum.TaskQueue:
                            case ServerMethodTypeEnum.CallbackTaskQueue:
                                if (serverMethod.OutputParameterCount == 0) return true;
                                break;
                        }
                        break;
                    case ClientMethodTypeEnum.Callback:
                    case ClientMethodTypeEnum.CallbackQueue:
                    case ClientMethodTypeEnum.ReturnValue:
                    case ClientMethodTypeEnum.Task:
                    case ClientMethodTypeEnum.ReturnValueQueue:
                        switch (serverMethod.MethodType)
                        {
                            case ServerMethodTypeEnum.Synchronous:
                                if (OutputParameterCount == 0) return true;
                                break;
                            case ServerMethodTypeEnum.Callback:
                            case ServerMethodTypeEnum.CallbackTask:
                            case ServerMethodTypeEnum.Queue:
                            case ServerMethodTypeEnum.CallbackQueue:
                            case ServerMethodTypeEnum.ConcurrencyReadQueue:
                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.ReadWriteQueue:
                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.Task:
                            case ServerMethodTypeEnum.TaskQueue:
                            case ServerMethodTypeEnum.CallbackTaskQueue:
                                return true;
                        }
                        break;
                    case ClientMethodTypeEnum.KeepCallback:
                    case ClientMethodTypeEnum.KeepCallbackQueue:
                    case ClientMethodTypeEnum.Enumerator:
                    case ClientMethodTypeEnum.EnumeratorQueue:
#if NetStandard21
                    case ClientMethodTypeEnum.AsyncEnumerable:
#endif
                        switch (serverMethod.MethodType)
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
                                return true;
                        }
                        break;
                    case ClientMethodTypeEnum.TwoStage‌Callback:
                        switch (serverMethod.MethodType)
                        {
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
                                return true;
                        }
                        break;
                }
                SetError($"{Type.fullName()} 客户端方法 {Method.Name} 调用类型 {MethodType} 与服务端方法调用类型 {serverMethod.MethodType} 不匹配");
            }
            else SetError($"{Type.fullName()} 客户端方法 {Method.Name} 与服务端方法 {serverMethod.Method.Name} 不匹配");
            return false;
        }
        /// <summary>
        /// 设置服务端接口方法信息
        /// </summary>
        /// <param name="serverMethod"></param>
        internal void Set(ServerInterfaceMethod serverMethod)
        {
            ServerMethod = serverMethod;
            MethodIndex = serverMethod.MethodIndex;
            IsSimpleDeserializeParamter = serverMethod.IsSimpleSerializeParamter;
            IsSimpleSerializeParamter = serverMethod.IsSimpleDeserializeParamter;
            IsSimpleSerializeTwoStage‌ReturnValue = serverMethod.IsSimpleSerializeTwoStage‌ReturnValue;
        }
#if !AOT
        /// <summary>
        /// 获取输入参数临时变量定义
        /// </summary>
        /// <param name="methodGenerator"></param>
        /// <param name="newInputParameterLocalBuilder"></param>
        /// <returns></returns>
#if NetStandard21
        internal LocalBuilder? GetInputParameterLocalBuilder(ILGenerator methodGenerator, out LocalBuilder? newInputParameterLocalBuilder)
#else
        internal LocalBuilder GetInputParameterLocalBuilder(ILGenerator methodGenerator, out LocalBuilder newInputParameterLocalBuilder)
#endif
        {
            if (InputParameterType != null)
            {
                newInputParameterLocalBuilder = methodGenerator.DeclareLocal(InputParameterType.Type);
                if (InputParameterType.IsInitobj)
                {
                    methodGenerator.Emit(OpCodes.Ldloca, newInputParameterLocalBuilder);
                    methodGenerator.Emit(OpCodes.Initobj, InputParameterType.Type);
                }
                return methodGenerator.DeclareLocal(InputParameterType.Type);
            }
            return newInputParameterLocalBuilder = null;
        }
        /// <summary>
        /// 获取输出参数临时变量定义
        /// </summary>
        /// <param name="methodGenerator"></param>
        /// <param name="returnValueGenericType"></param>
        /// <param name="twoStage‌CallbackReturnValueGenericType"></param>
        /// <returns></returns>
#if NetStandard21
        internal LocalBuilder? GetOutputParameterLocalBuilder(ILGenerator methodGenerator, out GenericType? returnValueGenericType, out GenericType? twoStage‌CallbackReturnValueGenericType)
#else
        internal LocalBuilder GetOutputParameterLocalBuilder(ILGenerator methodGenerator, out GenericType returnValueGenericType, out GenericType twoStage‌CallbackReturnValueGenericType)
#endif
        {
            returnValueGenericType = ReturnValueType != typeof(void) ? GenericType.Get(ReturnValueType) : null;
            twoStage‌CallbackReturnValueGenericType = TwoStage‌ReturnValueType != typeof(void) ? GenericType.Get(TwoStage‌ReturnValueType) : null;
            if (OutputParameterType != null)
            {
                LocalBuilder outputParameterLocalBuilder = methodGenerator.DeclareLocal(OutputParameterType.Type);
                if (OutputParameterType.IsInitobj)
                {
                    methodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                    methodGenerator.Emit(OpCodes.Initobj, OutputParameterType.Type);
                }
                if (ReturnValueParameterIndex >= 0)
                {
                    methodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                    methodGenerator.ldarg(ReturnValueParameterIndex + 1);
                    methodGenerator.Emit(OpCodes.Stfld, OutputParameterFields[OutputParameterCount]);
                }
                return outputParameterLocalBuilder;
            }
            if (returnValueGenericType != null && MethodType == ClientMethodTypeEnum.Synchronous)
            {
                Type outputType = returnValueGenericType.CommandServerReturnValueType;
                LocalBuilder outputParameterLocalBuilder = methodGenerator.DeclareLocal(outputType);
                if (DynamicArray.IsClearArray(ReturnValueType))
                {
                    methodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                    methodGenerator.Emit(OpCodes.Initobj, outputType);
                }
                if (ReturnValueParameterIndex >= 0)
                {
                    methodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                    methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                    methodGenerator.call(returnValueGenericType.SetCommandServerReturnValueDelegate.Method);
                }
                return outputParameterLocalBuilder;
            }
            return null;
        }
        /// <summary>
        /// 获取输出参数序号
        /// </summary>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal int GetOutputParameterIndex(ParameterInfo outputParameter)
        {
            int parameterIndex = 0;
            foreach (ParameterInfo parameter in Parameters)
            {
                if (object.ReferenceEquals(parameter, outputParameter)) return parameterIndex;
                ++parameterIndex;
            }
            return int.MinValue;
        }
        /// <summary>
        /// 回调委托参数
        /// </summary>
        /// <param name="methodGenerator"></param>
        /// <param name="returnValueGenericType"></param>
        /// <param name="twoStage‌CallbackReturnValueGenericType"></param>
#if NetStandard21
        internal void CallbackParameter(ILGenerator methodGenerator, GenericType? returnValueGenericType, GenericType? twoStage‌CallbackReturnValueGenericType)
#else
        internal void CallbackParameter(ILGenerator methodGenerator, GenericType returnValueGenericType, GenericType twoStage‌CallbackReturnValueGenericType)
#endif
        {
            switch (MethodType)
            {
                case ClientMethodTypeEnum.Callback:
                case ClientMethodTypeEnum.CallbackQueue:
                case ClientMethodTypeEnum.KeepCallback:
                case ClientMethodTypeEnum.KeepCallbackQueue:
                    methodGenerator.ldarg(ParameterEndIndex + 1);
                    if (IsCallbackAction)
                    {
                        var getCallbackMethod = default(MethodInfo);
                        if (ReturnValueType == typeof(void))
                        {
                            switch (MethodType)
                            {
                                case ClientMethodTypeEnum.Callback:
                                    getCallbackMethod = ClientInterfaceController.GetCommandClientCallback.Method;
                                    break;
                                case ClientMethodTypeEnum.KeepCallback:
                                    getCallbackMethod = ClientInterfaceController.GetCommandClientKeepCallback.Method;
                                    break;
                                case ClientMethodTypeEnum.CallbackQueue:
                                    getCallbackMethod = ClientInterfaceController.GetCommandClientCallbackQueue.Method;
                                    break;
                                case ClientMethodTypeEnum.KeepCallbackQueue:
                                    getCallbackMethod = ClientInterfaceController.GetCommandClientKeepCallbackQueue.Method;
                                    break;
                            }
                        }
                        else
                        {
                            switch (MethodType)
                            {
                                case ClientMethodTypeEnum.Callback:
                                    getCallbackMethod = returnValueGenericType.notNull().GetCommandClientCallbackDelegate.Method;
                                    break;
                                case ClientMethodTypeEnum.KeepCallback:
                                    getCallbackMethod = returnValueGenericType.notNull().GetCommandClientKeepCallbackDelegate.Method;
                                    break;
                                case ClientMethodTypeEnum.CallbackQueue:
                                    getCallbackMethod = returnValueGenericType.notNull().GetCommandClientCallbackQueueDelegate.Method;
                                    break;
                                case ClientMethodTypeEnum.KeepCallbackQueue:
                                    getCallbackMethod = returnValueGenericType.notNull().GetCommandClientKeepCallbackQueueDelegate.Method;
                                    break;
                            }
                        }
                        methodGenerator.call(getCallbackMethod.notNull());
                    }
                    break;
                case ClientMethodTypeEnum.TwoStage‌Callback:
                    methodGenerator.ldarg(ParameterEndIndex + 1);
                    if (IsCallbackAction && !IsTwoStageReturnValueParameter) methodGenerator.call(twoStage‌CallbackReturnValueGenericType.notNull().GetCommandClientCallbackDelegate.Method);
                    methodGenerator.ldarg(ParameterEndIndex + 2);
                    if (IsCallbackAction) methodGenerator.call(returnValueGenericType.notNull().GetCommandClientKeepCallbackDelegate.Method);
                    break;
            }
        }
        /// <summary>
        /// 控制器调用
        /// </summary>
        /// <param name="methodGenerator"></param>
        /// <param name="returnValueGenericType"></param>
        /// <param name="outputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallController(ILGenerator methodGenerator, GenericType? returnValueGenericType, LocalBuilder? outputParameterLocalBuilder)
#else
        internal void CallController(ILGenerator methodGenerator, GenericType returnValueGenericType, LocalBuilder outputParameterLocalBuilder)
#endif
        {
            switch (MethodType)
            {
                case ClientMethodTypeEnum.Synchronous:
                    #region CommandClientReturnValue returnValue = this.SynchronousInputOutput(0, ref inputParameter, ref outputParameter);
                    var returnValueLocalBuilder = IsReturnType || ReturnValueType == typeof(void) ? null : methodGenerator.DeclareLocal(typeof(CommandClientReturnValue));
                    MethodInfo controllerMethod;
                    if (InputParameterType == null)
                    {
                        if (OutputParameterType == null)
                        {
                            if (ReturnValueType == typeof(void))
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerSynchronous.Method;
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerSynchronousOutputMethod.MakeGenericMethod(returnValueGenericType.notNull().CommandServerReturnValueType);
                            }
                        }
                        else
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerSynchronousOutputMethod.MakeGenericMethod(OutputParameterType.Type);
                        }
                    }
                    else
                    {
                        if (OutputParameterType == null)
                        {
                            if (ReturnValueType == typeof(void))
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerSynchronousInputMethod.MakeGenericMethod(InputParameterType.Type);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerSynchronousInputOutputMethod.MakeGenericMethod(InputParameterType.Type, returnValueGenericType.notNull().CommandServerReturnValueType);
                            }
                        }
                        else
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerSynchronousInputOutputMethod.MakeGenericMethod(InputParameterType.Type, OutputParameterType.Type);
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    if (IsReturnType) methodGenerator.call(ClientInterfaceController.CommandClientReturnValueCheckThrowException.Method);
                    else if (returnValueLocalBuilder != null) methodGenerator.Emit(OpCodes.Stloc_S, returnValueLocalBuilder);
                    #endregion
                    #region Ref = outputParameter.Ref;
                    if (OutputParameterType != null)
                    {
                        int parameterIndex = 0;
                        foreach (ParameterInfo parameter in OutputParameters)
                        {
                            methodGenerator.Emit(OpCodes.Ldarg, GetOutputParameterIndex(parameter) + 1);
                            methodGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                            methodGenerator.Emit(OpCodes.Ldfld, OutputParameterFields[parameterIndex++]);
                            Type parameterType = parameter.elementType();
                            if (parameterType.IsValueType)
                            {
                                if (parameterType.IsEnum) parameterType = System.Enum.GetUnderlyingType(parameterType);
                                if (parameterType == typeof(int) || parameterType == typeof(uint)) methodGenerator.Emit(OpCodes.Stind_I4);
                                else if (parameterType == typeof(long) || parameterType == typeof(ulong)) methodGenerator.Emit(OpCodes.Stind_I8);
                                else if (parameterType == typeof(byte) || parameterType == typeof(sbyte)) methodGenerator.Emit(OpCodes.Stind_I1);
                                else if (parameterType == typeof(char) || parameterType == typeof(short) || parameterType == typeof(ushort)) methodGenerator.Emit(OpCodes.Stind_I2);
                                else if (parameterType == typeof(float)) methodGenerator.Emit(OpCodes.Stind_R4);
                                else if (parameterType == typeof(double)) methodGenerator.Emit(OpCodes.Stind_R8);
                                else methodGenerator.Emit(OpCodes.Stobj, parameterType);
                            }
                            else methodGenerator.Emit(OpCodes.Stind_Ref);
                        }
                    }
                    #endregion
                    if (ReturnValueType != typeof(void))
                    {
                        if (IsReturnType)
                        {
                            #region return outputParameter.__Return__;
                            methodGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                            if (OutputParameterType == null) methodGenerator.call(returnValueGenericType.notNull().GetCommandServerReturnValueDelegate.Method);
                            else methodGenerator.Emit(OpCodes.Ldfld, OutputParameterFields[OutputParameterCount]);
                            #endregion
                        }
                        else
                        {
                            #region if (CommandClientReturnValue.GetIsSuccess(returnValue)) return CommandClientReturnValue<string>.GetReturnValue(outputParameter.__Return__);
                            Label returnValueLabel = methodGenerator.DefineLabel();
                            methodGenerator.Emit(OpCodes.Ldloc, returnValueLocalBuilder.notNull());
                            methodGenerator.call(ClientInterfaceController.CommandClientReturnValueGetIsSuccess.Method);
                            methodGenerator.Emit(OpCodes.Brfalse_S, returnValueLabel);
                            methodGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                            if (OutputParameterType == null)
                            {
                                methodGenerator.call(returnValueGenericType.notNull().GetCommandServerReturnValueDelegate.Method);
                            }
                            else methodGenerator.Emit(OpCodes.Ldfld, OutputParameterFields[OutputParameterCount]);
                            methodGenerator.call(returnValueGenericType.notNull().GetCommandClientReturnValueDelegate.Method);
                            methodGenerator.ret();
                            #endregion
                            #region return CommandClientReturnValue<string>.GetReturnValue(returnValue);
                            methodGenerator.MarkLabel(returnValueLabel);
                            methodGenerator.Emit(OpCodes.Ldloc, returnValueLocalBuilder.notNull());
                            methodGenerator.call(returnValueGenericType.notNull().GetCommandClientReturnTypeDelegate.Method);
                            #endregion
                        }
                    }
                    break;
                case ClientMethodTypeEnum.SendOnly:
                    #region return this.SendOnlyInput(0, ref inputParameter);
                    if (InputParameterType == null) methodGenerator.call(ClientInterfaceController.CommandClientControllerSendOnly.Method);
                    else methodGenerator.call(ClientInterfaceController.CommandClientControllerSendOnlyInputMethod.MakeGenericMethod(InputParameterType.Type));
                    #endregion
                    break;
                case ClientMethodTypeEnum.Callback:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerCallback.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerCallbackDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerCallbackInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerCallbackOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                                controllerMethod = ClientInterfaceController.CommandClientControllerCallbackOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.KeepCallback:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallback.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerKeepCallbackDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.TwoStage‌Callback:
                    if (InputParameterType == null)
                    {
                        if (IsTwoStageReturnValueParameter) controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackReturnParameterMethod.MakeGenericMethod(TwoStage‌ReturnValueType, ReturnValueType);
                        else controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackMethod.MakeGenericMethod(TwoStage‌ReturnValueType, ReturnValueType);
                    }
                    else
                    {
                        if (ReturnValueParameterIndex < 0)
                        {
                            if (IsTwoStageReturnValueParameter) controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackInputReturnParameterMethod.MakeGenericMethod(InputParameterType.Type, TwoStage‌ReturnValueType, ReturnValueType);
                            else controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackInputMethod.MakeGenericMethod(InputParameterType.Type, TwoStage‌ReturnValueType, ReturnValueType);
                        }
                        else
                        {
                            if (IsTwoStageReturnValueParameter) controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackInputReturnValueParameterMethod.MakeGenericMethod(InputParameterType.Type, TwoStage‌ReturnValueType, ReturnValueType);
                            else controllerMethod = ClientInterfaceController.CommandClientControllerTwoStage‌CallbackInputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, TwoStage‌ReturnValueType, ReturnValueType);
                            methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.CallbackQueue:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerCallbackQueue.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerCallbackQueueDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerCallbackQueueInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerCallbackQueueOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerCallbackQueueOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.KeepCallbackQueue:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackQueue.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerKeepCallbackQueueDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackQueueInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackQueueOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerKeepCallbackQueueOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.ReturnValue:
                case ClientMethodTypeEnum.Task:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerReturnType.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerReturnValueDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerReturnTypeInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerReturnValueOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerReturnValueOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    if (MethodType == ClientMethodTypeEnum.Task)
                    {
                        if (ReturnValueType == typeof(void)) methodGenerator.call(ClientInterfaceController.ReturnCommandGetTask.Method);
                        else methodGenerator.call(returnValueGenericType.notNull().CommandClientReturnCommandGetTaskDelegate.Method);
                    }
                    break;
                case ClientMethodTypeEnum.ReturnValueQueue:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerReturnTypeQueue.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerReturnValueQueueDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerReturnTypeQueueInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerReturnValueQueueOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerReturnValueQueueOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
                case ClientMethodTypeEnum.Enumerator:
#if NetStandard21
                case ClientMethodTypeEnum.AsyncEnumerable:
#endif
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerEnumerator.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerEnumeratorDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
#if NetStandard21
                    if (MethodType == ClientMethodTypeEnum.AsyncEnumerable)
                    {
                        methodGenerator.call(returnValueGenericType.notNull().CommandClientEnumeratorCommandGetAsyncEnumerableDelegate.Method);
                    }
#endif
                    break;
                case ClientMethodTypeEnum.EnumeratorQueue:
                    if (InputParameterType == null)
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorQueue.Method;
                        }
                        else
                        {
                            controllerMethod = returnValueGenericType.notNull().CommandClientControllerEnumeratorQueueDelegate.Method;
                        }
                    }
                    else
                    {
                        if (ReturnValueType == typeof(void))
                        {
                            controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorQueueInputMethod.MakeGenericMethod(InputParameterType.Type);
                        }
                        else
                        {
                            if (ReturnValueParameterIndex < 0)
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorQueueOutputMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                            }
                            else
                            {
                                controllerMethod = ClientInterfaceController.CommandClientControllerEnumeratorQueueOutputReturnValueMethod.MakeGenericMethod(InputParameterType.Type, ReturnValueType);
                                methodGenerator.Emit(OpCodes.Ldarga, ReturnValueParameterIndex + 1);
                            }
                        }
                    }
                    methodGenerator.call(controllerMethod);
                    break;
            }
        }

        /// <summary>
        /// Get the collection of server-side method numbers
        /// 获取服务端方法编号集合
        /// </summary>
        /// <param name="methodStartIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
#if NetStandard21
        internal static int[] GetServerMethodIndexs(int methodStartIndex, ClientInterfaceMethod[] methods, string?[]? serverMethodNames)
#else
        internal static int[] GetServerMethodIndexs(int methodStartIndex, ClientInterfaceMethod[] methods, string[] serverMethodNames)
#endif
        {
            int maxMethodIndex = -1;
            foreach (var method in methods)
            {
                if (method.MethodIndex > maxMethodIndex) maxMethodIndex = method.MethodIndex;
            }
            int[] serverMethodIndexs = AutoCSer.Common.GetUninitializedArray<int>(maxMethodIndex + 1);
            if (serverMethodNames == null)
            {
                AutoCSer.Common.Fill(serverMethodIndexs, -methodStartIndex);
                foreach (var method in methods)
                {
                    int methodIndex = method.MethodIndex;
                    //if ((uint)methodIndex >= serverMethodIndexs.Length)
                    //{
                    //    Console.WriteLine("ERROR");
                    //}
                    serverMethodIndexs[methodIndex] = methodIndex;
                }
            }
            else GetServerMethodIndexs(methodStartIndex, methods, serverMethodNames, serverMethodIndexs);
            return serverMethodIndexs;
        }
        /// <summary>
        /// Get the collection of server-side method numbers
        /// 获取服务端方法编号集合
        /// </summary>
        /// <param name="methodStartIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodNames"></param>
        /// <param name="serverMethodIndexs"></param>
#if NetStandard21
        internal static void GetServerMethodIndexs(int methodStartIndex, ClientInterfaceMethod[] methods, string?[] serverMethodNames, int[] serverMethodIndexs)
#else
        internal static void GetServerMethodIndexs(int methodStartIndex, ClientInterfaceMethod[] methods, string[] serverMethodNames, int[] serverMethodIndexs)
#endif
        {
            AutoCSer.Common.Fill(serverMethodIndexs, -methodStartIndex);
            LeftArray<ClientInterfaceMethod> errorMethods = new LeftArray<ClientInterfaceMethod>(0);
            foreach (var method in methods)
            {
                int methodIndex = method.MethodIndex;
                if (methodIndex < serverMethodNames.Length && method.MatchMethodName == serverMethodNames[methodIndex])
                {
                    serverMethodIndexs[methodIndex] = methodIndex;
                    serverMethodNames[methodIndex] = null;
                }
                else if (serverMethodIndexs[methodIndex] < 0) errorMethods.Add(method);
            }
            if (errorMethods.Length != 0)
            {
                Dictionary<string, int> serverMethodNameIndexs = DictionaryCreator<string>.Create<int>(serverMethodNames.Length);
                int methodIndex = 0;
                foreach (var name in serverMethodNames)
                {
                    if (name != null) serverMethodNameIndexs[name] = methodIndex;
                    ++methodIndex;
                }
                foreach (var method in errorMethods)
                {
                    if (serverMethodIndexs[methodIndex = method.MethodIndex] < 0)
                    {
                        int serverIndex;
                        if (serverMethodNameIndexs.TryGetValue(method.MatchMethodName, out serverIndex)) serverMethodIndexs[methodIndex] = serverIndex;
                    }
                }
            }
        }
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="error"></param>
        internal static void ThrowException(string error)
        {
            throw new Exception(error);
        }
#endif
        /// <summary>
        /// 获取客户端接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="controllerAttribute"></param>
        /// <param name="taskQueueControllerKeyType"></param>
        /// <param name="methods"></param>
        /// <param name="isServer"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? GetMethod(Type type, CommandServerControllerInterfaceAttribute controllerAttribute, Type? taskQueueControllerKeyType, ref LeftArray<ClientInterfaceMethod> methods, bool isServer, bool isDefault = false)
#else
        internal static string GetMethod(Type type, CommandServerControllerInterfaceAttribute controllerAttribute, Type taskQueueControllerKeyType, ref LeftArray<ClientInterfaceMethod> methods, bool isServer, bool isDefault = false)
#endif
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                var error = InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                methods.Add(new ClientInterfaceMethod(type, method, controllerAttribute, taskQueueControllerKeyType, isServer, isDefault));
                //ClientInterfaceMethod clientMethod = new ClientInterfaceMethod(type, controllerAttribute, method, taskQueueControllerKeyType);
                //if (clientMethod.MethodType == ClientMethodType.Unknown) return clientMethod.Error ?? $"{type.fullName()}.{method.Name} 未知客户端方法调用类型";
                //methods.Add(clientMethod);
            }
            return null;
        }
    }
}
