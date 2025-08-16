using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Server interface method information
    /// 服务端接口方法信息
    /// </summary>
    internal sealed class ServerInterfaceMethod : InterfaceMethod
    {
        /// <summary>
        /// 获取 async Task 最大时间戳
        /// </summary>
        private static readonly long maxGetTaskTimestamp = Date.TimestampByMilliseconds * 20;

        /// <summary>
        /// 服务端输出信息 字段信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal FieldBuilder MethodFieldBuilder;
        /// <summary>
        /// 队列关键字类型
        /// </summary>
#if NetStandard21
        internal Type? TaskQueueKeyType;
#else
        internal Type TaskQueueKeyType;
#endif
        /// <summary>
        /// Task 队列关键字字段
        /// </summary>
        internal FieldInfo TaskQueueKeyField
        {
            get
            {
                return InputParameterFields[InputParameterFields.Length - 1];
                //if (taskQueueKeyParameterType != null) return InputParameterFields[InputParameterFields.Length - 1];
                //return InputParameterFields[taskQueueKeyParameterIndex - ParameterStartIndex];
            }
        }
        /// <summary>
        /// Command service method configuration
        /// 命令服务方法配置
        /// </summary>
        internal readonly CommandServerMethodAttribute MethodAttribute;
        /// <summary>
        /// 返回值参数
        /// </summary>
#if NetStandard21
        internal readonly ParameterInfo? ReturnParameter;
#else
        internal readonly ParameterInfo ReturnParameter;
#endif
        /// <summary>
        /// 异步回调类型
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal Type AsynchronousType;
        /// <summary>
        /// 二阶段回调的第一阶段的返回值类型
        /// </summary>
        internal readonly Type TwoStage‌ReturnValueType;
        /// <summary>
        /// 保持回调输出计数
        /// </summary>
        internal int KeepCallbackOutputCount
        {
            get { return Math.Max(MethodAttribute.KeepCallbackOutputCount, 1); }
        }
        /// <summary>
        /// API 独占 Task.Run 操作
        /// </summary>
        private int isMethodRunTask;
        /// <summary>
        /// async Task 调度计数
        /// </summary>
        private volatile int runTaskCount;
        /// <summary>
        /// 默认为 false 表示系统自动调度 Task，否则使用 IO 线程同步调用 Task
        /// </summary>
        private readonly bool isSynchronousCallTask;
        /// <summary>
        /// 同步调用 async Task 时间戳是否满足需求 
        /// </summary>
        private bool isSynchronousCallTaskTimestamp;
        /// <summary>
        /// Server-side method call types
        /// 服务端方法调用类型
        /// </summary>
        internal readonly ServerMethodTypeEnum MethodType;
        /// <summary>
        /// 是否存在输出参数
        /// </summary>
        internal readonly bool IsOutputInfo;
        /// <summary>
        /// 服务端输出对象是否采用缓存池
        /// </summary>
        internal readonly bool IsOutputPool;
        /// <summary>
        /// 是否服务下线通知计数
        /// </summary>
        internal readonly bool IsOfflineCount;
        /// <summary>
        /// TCP 服务器端同步调用队列是否低优先级
        /// </summary>
        internal readonly bool IsLowPriorityQueue;
        /// <summary>
        /// 读写队列是否调用写队列
        /// </summary>
        internal readonly bool IsWriteQueue;
        /// <summary>
        /// 是否存在 AutoCSer.Net.CommandServerSocket 参数
        /// </summary>
        internal readonly bool IsParameterSocket;
        /// <summary>
        /// Whether to simply serialize the return value of the first stage of the two-stage callback
        /// 是否简单序列化二阶段回调的第一阶段的返回值
        /// </summary>
        internal readonly bool IsSimpleSerializeTwoStage‌ReturnValue;
        /// <summary>
        /// 默认空服务端接口方法信息
        /// </summary>
        internal unsafe ServerInterfaceMethod()
        {
            MethodAttribute = new CommandServerMethodAttribute();
            IsOutputPool = MethodAttribute.IsOutputPool;
            IsSimpleSerializeParamter = true;
            TwoStage‌ReturnValueType = typeof(void);
        }
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="controllerAttribute"></param>
        /// <param name="taskQueueControllerKeyType"></param>
        /// <param name="isGetServerMethodParameter"></param>
#if NetStandard21
        internal unsafe ServerInterfaceMethod(Type type, MethodInfo method, CommandServerControllerInterfaceAttribute controllerAttribute, Type? taskQueueControllerKeyType, bool isGetServerMethodParameter = true) : base(type, method, controllerAttribute)
#else
        internal unsafe ServerInterfaceMethod(Type type, MethodInfo method, CommandServerControllerInterfaceAttribute controllerAttribute, Type taskQueueControllerKeyType, bool isGetServerMethodParameter = true) : base(type, method, controllerAttribute)
#endif
        {
            MethodAttribute = method.GetCustomAttribute<CommandServerMethodAttribute>(false) ?? CommandServerMethodAttribute.Default;
            TwoStage‌ReturnValueType = typeof(void);
            MethodIndex = MethodAttribute.MethodIndex;
            if (MethodAttribute.IsExpired)
            {
                MethodType = ServerMethodTypeEnum.VersionExpired;
                return;
            }

            IsOfflineCount = MethodAttribute.IsOfflineCount;
            Parameters = method.GetParameters();
            ParameterEndIndex = Parameters.Length;
            if (ParameterEndIndex > ParameterStartIndex && taskQueueControllerKeyType == null && Parameters[ParameterStartIndex].ParameterType == typeof(CommandServerSocket))
            {
                ++ParameterStartIndex;
                IsParameterSocket = true;
            }
            ReturnValueType = method.ReturnType;
            var genericTypeDefinition = ReturnValueType.IsGenericType ? ReturnValueType.GetGenericTypeDefinition() : null;
            ServerQueueTypeEnum serverQueueType = ServerQueueTypeEnum.None;
            bool checkTwoStage‌ = false;
            if (genericTypeDefinition == typeof(Task<>) || ReturnValueType == typeof(Task)
#if NetStandard21
                || genericTypeDefinition == typeof(IAsyncEnumerable<>)
#endif
                )
            {
                if (taskQueueControllerKeyType == null)
                {
                    if (ParameterEndIndex > ParameterStartIndex)
                    {
                        Type queueType = Parameters[ParameterStartIndex].ParameterType;
                        if (queueType.IsGenericType)
                        {
                            Type queueGenericType = queueType.GetGenericTypeDefinition();
                            if (queueGenericType == typeof(CommandServerCallTaskQueue<>) || queueGenericType == typeof(CommandServerCallTaskLowPriorityQueue<>))
                            {
                                taskQueueControllerKeyType = queueType.GetGenericArguments()[0];
                                Type equatableType = typeof(IEquatable<>).MakeGenericType(taskQueueControllerKeyType);
                                if (!equatableType.IsAssignableFrom(taskQueueControllerKeyType))
                                {
                                    MethodType = ServerMethodTypeEnum.Unknown;
                                    Error = $"{type.fullName()}.{method.Name} 关键字类型 {taskQueueControllerKeyType.fullName()} 必须继承自接口 {equatableType.fullName()}";
                                    return;
                                }
                                TaskQueueKeyType = taskQueueControllerKeyType;
                                //taskQueueKeyParameterIndex = ParameterStartIndex++;
                                ++ParameterStartIndex;
                                equalsParameterCount = 1;
                                serverQueueType = ServerQueueTypeEnum.TaskQueue;
                                IsLowPriorityQueue = queueGenericType == typeof(CommandServerCallTaskLowPriorityQueue<>);
                            }
                        }
                        else if (queueType == typeof(CommandServerCallTaskQueue) || queueType == typeof(CommandServerCallTaskLowPriorityQueue))
                        {
                            ++ParameterStartIndex;
                            serverQueueType = ServerQueueTypeEnum.TaskQueue;
                            IsLowPriorityQueue = queueType == typeof(CommandServerCallTaskLowPriorityQueue);
                        }
                    }
                }
                else
                {
                    serverQueueType = ServerQueueTypeEnum.TaskQueue;
                    IsLowPriorityQueue = MethodAttribute.IsLowPriorityTaskQueue;
                }
                if (ReturnValueType == typeof(Task))
                {
                    ReturnValueType = typeof(void);
                    bool isKeepCallback = false;
                    if (ParameterEndIndex > ParameterStartIndex)
                    {
                        ParameterInfo parameter = Parameters[ParameterEndIndex - 1];
                        Type parameterType = parameter.ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (genericType == typeof(CommandServerCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.CallbackTaskQueue : ServerMethodTypeEnum.CallbackTask;
                                isKeepCallback = true;
                            }
                            else if (genericType == typeof(CommandServerKeepCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.KeepCallbackTaskQueue : ServerMethodTypeEnum.KeepCallbackTask;
                                isKeepCallback = checkTwoStage‌ = true;
                            }
                            else if (genericType == typeof(CommandServerKeepCallbackCount<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.KeepCallbackCountTaskQueue : ServerMethodTypeEnum.KeepCallbackCountTask;
                                isKeepCallback = checkTwoStage‌ = true;
                            }
                        }
                        else if (parameterType == typeof(CommandServerCallback))
                        {
                            MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.CallbackTaskQueue : ServerMethodTypeEnum.CallbackTask;
                            isKeepCallback = true;
                        }
                        else if (parameterType == typeof(CommandServerKeepCallback))
                        {
                            MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.KeepCallbackTaskQueue : ServerMethodTypeEnum.KeepCallbackTask;
                            isKeepCallback = true;
                        }
                        else if (parameterType == typeof(CommandServerKeepCallbackCount))
                        {
                            MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.KeepCallbackCountTaskQueue : ServerMethodTypeEnum.KeepCallbackCountTask;
                            isKeepCallback = true;
                        }
                    }
                    if (isKeepCallback) --ParameterEndIndex;
                    else MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.TaskQueue : ServerMethodTypeEnum.Task;
                }
                else
                {
                    ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                    if (genericTypeDefinition == typeof(Task<>))
                    {
                        if (ReturnValueType == typeof(CommandServerSendOnly))
                        {
                            ReturnValueType = typeof(void);
                            MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.SendOnlyTaskQueue : ServerMethodTypeEnum.SendOnlyTask;
                        }
                        else if (ReturnValueType.IsGenericType && ReturnValueType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                            MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue : ServerMethodTypeEnum.EnumerableKeepCallbackCountTask;
                        }
                        else MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.TaskQueue : ServerMethodTypeEnum.Task;
                    }
#if NetStandard21
                    else MethodType = serverQueueType == ServerQueueTypeEnum.TaskQueue ? ServerMethodTypeEnum.AsyncEnumerableTaskQueue : ServerMethodTypeEnum.AsyncEnumerableTask;
#endif
                }
            }
            else
            {
                if (taskQueueControllerKeyType != null) return;
                if (ParameterEndIndex > ParameterStartIndex)
                {
                    Type queueType = Parameters[ParameterStartIndex].ParameterType;
                    if (queueType == typeof(CommandServerCallQueue) || queueType == typeof(CommandServerCallLowPriorityQueue))
                    {
                        ++ParameterStartIndex;
                        serverQueueType = ServerQueueTypeEnum.Queue;
                        IsLowPriorityQueue = queueType == typeof(CommandServerCallLowPriorityQueue);
                    }
                    else if (queueType == typeof(CommandServerCallConcurrencyReadQueue) || queueType == typeof(CommandServerCallConcurrencyReadWriteQueue))
                    {
                        ++ParameterStartIndex;
                        serverQueueType = ServerQueueTypeEnum.ConcurrencyReadQueue;
                        IsWriteQueue = queueType == typeof(CommandServerCallWriteQueue);
                    }
                    else if (queueType == typeof(CommandServerCallReadQueue) || queueType == typeof(CommandServerCallWriteQueue))
                    {
                        ++ParameterStartIndex;
                        serverQueueType = ServerQueueTypeEnum.ReadWriteQueue;
                        IsWriteQueue = queueType == typeof(CommandServerCallWriteQueue);
                    }
                    if (ParameterEndIndex > ParameterStartIndex)
                    {
                        ParameterInfo parameter = Parameters[ParameterEndIndex - 1];
                        Type parameterType = parameter.ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (genericType == typeof(CommandServerCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                switch (serverQueueType)
                                {
                                    case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.CallbackQueue; break;
                                    case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.CallbackConcurrencyReadQueue; break;
                                    case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.CallbackReadWriteQueue; break;
                                    default: MethodType = ServerMethodTypeEnum.Callback; break;
                                }
                                --ParameterEndIndex;
                            }
                            else if (genericType == typeof(CommandServerKeepCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                switch (serverQueueType)
                                {
                                    case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.KeepCallbackQueue; break;
                                    case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue; break;
                                    case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.KeepCallbackReadWriteQueue; break;
                                    default: MethodType = ServerMethodTypeEnum.KeepCallback; break;
                                }
                                --ParameterEndIndex;
                                checkTwoStage‌ = true;
                            }
                            else if (genericType == typeof(CommandServerKeepCallbackCount<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                ReturnParameter = parameter;
                                switch (serverQueueType)
                                {
                                    case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.KeepCallbackCountQueue; break;
                                    case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue; break;
                                    case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue; break;
                                    default: MethodType = ServerMethodTypeEnum.KeepCallbackCount; break;
                                }
                                --ParameterEndIndex;
                                checkTwoStage‌ = true;
                            }
                        }
                        else if (parameterType == typeof(CommandServerCallback))
                        {
                            switch (serverQueueType)
                            {
                                case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.CallbackQueue; break;
                                case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.CallbackConcurrencyReadQueue; break;
                                case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.CallbackReadWriteQueue; break;
                                default: MethodType = ServerMethodTypeEnum.Callback; break;
                            }
                            --ParameterEndIndex;
                        }
                        else if (parameterType == typeof(CommandServerKeepCallback))
                        {
                            switch (serverQueueType)
                            {
                                case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.KeepCallbackQueue; break;
                                case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue; break;
                                case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.KeepCallbackReadWriteQueue; break;
                                default: MethodType = ServerMethodTypeEnum.KeepCallback; break;
                            }
                            --ParameterEndIndex;
                        }
                        else if (parameterType == typeof(CommandServerKeepCallbackCount))
                        {
                            switch (serverQueueType)
                            {
                                case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.KeepCallbackCountQueue; break;
                                case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue; break;
                                case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue; break;
                                default: MethodType = ServerMethodTypeEnum.KeepCallbackCount; break;
                            }
                            --ParameterEndIndex;
                        }
                        switch (MethodType)
                        {
                            case ServerMethodTypeEnum.Callback:
                            case ServerMethodTypeEnum.KeepCallback:
                            case ServerMethodTypeEnum.KeepCallbackCount:
                            case ServerMethodTypeEnum.CallbackQueue:
                            case ServerMethodTypeEnum.KeepCallbackQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountQueue:
                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                            case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                            case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                                if (method.ReturnType != typeof(void))
                                {
                                    MethodType = ServerMethodTypeEnum.Unknown;
                                    Error = $"回调接口 {type.fullName()}.{method.Name} 返回值类型必须为 void";
                                    return;
                                }
                                break;
                        }
                    }
                }
                if (MethodType == ServerMethodTypeEnum.Unknown)
                {
                    if (ReturnValueType == typeof(CommandServerSendOnly))
                    {
                        ReturnValueType = typeof(void);
                        switch (serverQueueType)
                        {
                            case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.SendOnlyQueue; break;
                            case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue; break;
                            case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.SendOnlyReadWriteQueue; break;
                            default: MethodType = ServerMethodTypeEnum.SendOnly; break;
                        }
                    }
                    else
                    {
                        switch (serverQueueType)
                        {
                            case ServerQueueTypeEnum.Queue: MethodType = ServerMethodTypeEnum.Queue; break;
                            case ServerQueueTypeEnum.ConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.ConcurrencyReadQueue; break;
                            case ServerQueueTypeEnum.ReadWriteQueue: MethodType = ServerMethodTypeEnum.ReadWriteQueue; break;
                            default: MethodType = ServerMethodTypeEnum.Synchronous; break;
                        }
                    }
                }
            }
            if (checkTwoStage‌ && ParameterStartIndex < ParameterEndIndex)
            {
                ParameterInfo parameter = Parameters[ParameterEndIndex - 1];
                Type parameterType = parameter.ParameterType;
                if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(CommandServerCallback<>))
                {
                    switch (MethodType)
                    {
                        case ServerMethodTypeEnum.KeepCallback: MethodType = ServerMethodTypeEnum.TwoStage‌Callback; break;
                        case ServerMethodTypeEnum.KeepCallbackCount: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCount; break;
                        case ServerMethodTypeEnum.KeepCallbackQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackCountQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCountQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackConcurrencyReadQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCountConcurrencyReadQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackReadWriteQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCountReadWriteQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackTask: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackTask; break;
                        case ServerMethodTypeEnum.KeepCallbackCountTask: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCountTask; break;
                        case ServerMethodTypeEnum.KeepCallbackTaskQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue; break;
                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue: MethodType = ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue; break;
                    }
                    TwoStage‌ReturnValueType = parameterType.GetGenericArguments()[0];
                    --ParameterEndIndex;
                }
            }
            switch (MethodType)
            {
                case ServerMethodTypeEnum.Queue:
                case ServerMethodTypeEnum.ConcurrencyReadQueue:
                case ServerMethodTypeEnum.ReadWriteQueue:
                case ServerMethodTypeEnum.Synchronous:
                    if (!checkSynchronousParameter())
                    {
                        MethodType = ServerMethodTypeEnum.Unknown;
                        return;
                    }
                    break;
                default:
                    for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
                    {
                        if (Parameters[parameterIndex].ParameterType.IsByRef)
                        {
                            MethodType = ServerMethodTypeEnum.Unknown;
                            Error = $"{MethodType} 接口 {type.fullName()}.{method.Name} 不允许下 ref / out 参数 {Parameters[parameterIndex].Name}";
                            return;
                        }
                    }
                    break;
            }
            setParameterCount();
            if (InputParameterCount != 0 || taskQueueControllerKeyType != null)
            {
                if (isGetServerMethodParameter)
                {
                    InputParameterType = ServerMethodParameter.GetOrCreate(InputParameterCount, InputParameters, taskQueueControllerKeyType ?? typeof(void));
                    InputParameterFields = InputParameterType.GetFields(InputParameters);
                    IsSimpleDeserializeParamter = controllerAttribute.IsSimpleSerializeInputParameter && InputParameterType.IsSimpleSerialize;
                }
            }
            if (OutputParameterCount == 0)
            {
                if (ReturnValueType != typeof(void))
                {
                    IsOutputInfo = true;
                    IsSimpleSerializeParamter = controllerAttribute.IsSimpleSerializeOutputParameter && SimpleSerialize.Serializer.IsType(ReturnValueType);
                }
            }
            else
            {
                if (isGetServerMethodParameter)
                {
                    OutputParameterType = ServerMethodParameter.GetOrCreate(OutputParameterCount, OutputParameters, ReturnValueType);
                    OutputParameterFields = OutputParameterType.GetFields(OutputParameters);
                    IsSimpleSerializeParamter = controllerAttribute.IsSimpleSerializeOutputParameter && OutputParameterType.IsSimpleSerialize;
                }
                IsOutputInfo = true;
            }
            if (IsOutputInfo) IsOutputPool = MethodAttribute.IsOutputPool;
            if (TwoStage‌ReturnValueType != typeof(void))
            {
                IsOutputInfo = true;
                IsSimpleSerializeTwoStage‌ReturnValue = controllerAttribute.IsSimpleSerializeOutputParameter && SimpleSerialize.Serializer.IsType(TwoStage‌ReturnValueType);
            }
            runTaskCount = (isSynchronousCallTask = MethodAttribute.IsSynchronousCallTask) ? 0 : 1;
        }
        /// <summary>
        /// async Task 调度时间检查
        /// </summary>
        /// <param name="startTimestamp"></param>
        /// <returns></returns>
        internal int CheckGetTaskTimestamp(long startTimestamp)
        {
            long getTaskTimestamp = Stopwatch.GetTimestamp() - startTimestamp;
            if (getTaskTimestamp < Date.TimestampByMilliseconds)
            {
                if (runTaskCount > 0 && --runTaskCount <= 0) isSynchronousCallTaskTimestamp = true;
            }
            else
            {
                if (getTaskTimestamp < maxGetTaskTimestamp)
                {
                    if (runTaskCount < 20) runTaskCount = 20;
                }
                else runTaskCount = int.MaxValue;
            }
            return System.Threading.Interlocked.CompareExchange(ref isMethodRunTask, 0, 1);
        }
        /// <summary>
        /// async Task 调用异常
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int CheckGetTaskException()
        {
            runTaskCount = int.MaxValue;
            return System.Threading.Interlocked.CompareExchange(ref isMethodRunTask, 0, 1);
        }
        /// <summary>
        /// async Task 调用异常
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetTaskException()
        {
            if (!isSynchronousCallTask) runTaskCount = int.MaxValue;
        }
        /// <summary>
        /// async Task 调度时间检查
        /// </summary>
        /// <param name="method"></param>
        /// <param name="startTimestamp"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckGetTaskTimestamp(ServerInterfaceMethod method, long startTimestamp)
        {
            if (!method.isSynchronousCallTask)
            {
                long getTaskTimestamp = Stopwatch.GetTimestamp() - startTimestamp;
                if (getTaskTimestamp >= Date.TimestampByMilliseconds)
                {
                    method.runTaskCount = getTaskTimestamp < maxGetTaskTimestamp ? 20 : int.MaxValue;
                }
            }
        }
        /// <summary>
        /// 获取输出参数字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal FieldInfo? GetOutputParameterField(string name)
#else
        internal FieldInfo GetOutputParameterField(string name)
#endif
        {
            foreach (FieldInfo field in OutputParameterFields)
            {
                if (field.Name == name) return field;
            }
            return null;
        }
        /// <summary>
        /// 获取服务端接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="controllerAttribute"></param>
        /// <param name="taskQueueControllerKeyType"></param>
        /// <param name="isGetServerMethodParameter"></param>
        /// <param name="methods"></param>
        /// <returns>错误信息</returns>
#if NetStandard21
        internal static string? GetMethod(Type type, CommandServerControllerInterfaceAttribute controllerAttribute, Type? taskQueueControllerKeyType, bool isGetServerMethodParameter, ref LeftArray<ServerInterfaceMethod> methods)
#else
        internal static string GetMethod(Type type, CommandServerControllerInterfaceAttribute controllerAttribute, Type taskQueueControllerKeyType, bool isGetServerMethodParameter, ref LeftArray<ServerInterfaceMethod> methods)
#endif
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                var error = InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                ServerInterfaceMethod serverMethod = new ServerInterfaceMethod(type, method, controllerAttribute, taskQueueControllerKeyType, isGetServerMethodParameter);
                if (serverMethod.MethodType == ServerMethodTypeEnum.Unknown) return serverMethod.Error ?? $"{type.fullName()}.{method.Name} 未知服务端方法调用类型";
                methods.Add(serverMethod);
            }
            return null;
        }
#if !AOT
        /// <summary>
        /// 否则使用 IO 线程同步调用 Task
        /// </summary>
        /// <param name="method"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsSynchronousCallTask(ServerInterfaceMethod method, CommandServerSocket socket)
        {
            return method.runTaskCount <= 0 || method.checkIsSynchronousCallTask(socket);
        }
        /// <summary>
        /// 否则使用 IO 线程同步调用 Task
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private bool checkIsSynchronousCallTask(CommandServerSocket socket)
        {
            if (isSynchronousCallTaskTimestamp)
            {
                if (socket.Server.CheckTaskRunConcurrent()) return false;
                return System.Threading.Interlocked.CompareExchange(ref isMethodRunTask, 1, 0) != 0;
            }
            socket.Server.TaskRunConcurrent();
            return false;
        }
        /// <summary>
        /// 获取输出参数临时变量定义
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <returns></returns>
#if NetStandard21
        internal LocalBuilder? GetOutputParameterLocalBuilder(ILGenerator doCommandGenerator)
#else
        internal LocalBuilder GetOutputParameterLocalBuilder(ILGenerator doCommandGenerator)
#endif
        {
            if (OutputParameterType != null)
            {
                LocalBuilder outputParameterLocalBuilder = doCommandGenerator.DeclareLocal(OutputParameterType.Type);
                if (OutputParameterType.IsInitobj)
                {
                    doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                    doCommandGenerator.Emit(OpCodes.Initobj, OutputParameterType.Type);
                }
                return outputParameterLocalBuilder;
            }
            return null;
        }
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="getControllerMethod"></param>
        /// <param name="controllerLocalBuilder"></param>
        /// <param name="inputParameterLocalBuilder"></param>
        /// <param name="outputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallMethodParameter(ILGenerator doCommandGenerator, MethodInfo getControllerMethod, LocalBuilder controllerLocalBuilder
            , LocalBuilder? inputParameterLocalBuilder, LocalBuilder? outputParameterLocalBuilder = null)
#else
        internal void CallMethodParameter(ILGenerator doCommandGenerator, MethodInfo getControllerMethod, LocalBuilder controllerLocalBuilder
            , LocalBuilder inputParameterLocalBuilder, LocalBuilder outputParameterLocalBuilder = null)
#endif
        {
            doCommandGenerator.Emit(OpCodes.Ldarg_0);
            doCommandGenerator.Emit(OpCodes.Ldarg_1);
            doCommandGenerator.call(getControllerMethod);
            doCommandGenerator.Emit(OpCodes.Stloc, controllerLocalBuilder);
            doCommandGenerator.Emit(OpCodes.Ldloca, controllerLocalBuilder);
            if (IsParameterSocket) doCommandGenerator.Emit(OpCodes.Ldarg_1);
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                if (!parameter.IsOut)
                {
                    if (parameter.ParameterType.IsByRef)
                    {
                        doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder.notNull());
                        doCommandGenerator.Emit(OpCodes.Ldflda, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                    }
                    else
                    {
                        doCommandGenerator.Emit(OpCodes.Ldloc, inputParameterLocalBuilder.notNull());
                        doCommandGenerator.Emit(OpCodes.Ldfld, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                    }
                }
                else
                {
                    doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                    doCommandGenerator.Emit(OpCodes.Ldflda, OutputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                }
            }
        }
        /// <summary>
        /// 创建回调
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="asynchronousConstructorBuilder"></param>
#if NetStandard21
        internal void CreateServerCallback(ILGenerator doCommandGenerator, ConstructorBuilder? asynchronousConstructorBuilder)
#else
        internal void CreateServerCallback(ILGenerator doCommandGenerator, ConstructorBuilder asynchronousConstructorBuilder)
#endif
        {
            #region new AsynchronousCallback(socket, offlineCount)
            doCommandGenerator.Emit(OpCodes.Ldarg_1);
            if (ReturnValueType == typeof(void))
            {
                switch (MethodType)
                {
                    case ServerMethodTypeEnum.CallbackTask:
                        doCommandGenerator.call(ServerInterfaceController.CreateServerCallbackTaskDelegate.Method);
                        break;
                    default:
                        doCommandGenerator.call(ServerInterfaceController.CreateServerCallbackDelegate.Method);
                        break;
                }
            }
            else doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder.notNull());
            #endregion
        }
        /// <summary>
        /// 创建二阶段回调的第一阶段回调
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        internal void CreateTwoStage‌Callback(ILGenerator doCommandGenerator)
        {
            doCommandGenerator.Emit(OpCodes.Ldarg_1);
            doCommandGenerator.Emit(OpCodes.Ldsfld, MethodFieldBuilder);
            doCommandGenerator.call(GenericType.Get(TwoStage‌ReturnValueType).CreateCommandServerTwoStage‌CallbackDelegate.Method);
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="controllerType"></param>
        /// <param name="returnFieldBuilder"></param>
        /// <param name="outputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallMethod(ILGenerator doCommandGenerator, Type controllerType, FieldBuilder? returnFieldBuilder, ref LocalBuilder? outputParameterLocalBuilder)
#else
        internal void CallMethod(ILGenerator doCommandGenerator, Type controllerType, FieldBuilder returnFieldBuilder, ref LocalBuilder outputParameterLocalBuilder)
#endif
        {
            doCommandGenerator.Emit(OpCodes.Constrained, controllerType);
            doCommandGenerator.call(Method);
            if (returnFieldBuilder == null)
            {
                if (OutputParameterType != null)
                {
                    if (ReturnValueType != typeof(void)) doCommandGenerator.Emit(OpCodes.Stfld, OutputParameterFields[OutputParameterCount]);
                }
                else if (ReturnValueType != typeof(void))
                {
                    switch(MethodType)
                    {
                        case ServerMethodTypeEnum.CallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountQueue:
                        case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackConcurrencyReadQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountConcurrencyReadQueue:
                        case ServerMethodTypeEnum.CallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                        case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackReadWriteQueue:
                        case ServerMethodTypeEnum.TwoStage‌CallbackCountReadWriteQueue:
                            break;
                        default:
                            if (outputParameterLocalBuilder == null) outputParameterLocalBuilder = doCommandGenerator.DeclareLocal(ReturnValueType);
                            doCommandGenerator.Emit(OpCodes.Stloc, outputParameterLocalBuilder);
                            break;
                    }
                }
            }
            else doCommandGenerator.Emit(OpCodes.Stfld, returnFieldBuilder);
        }
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="asynchronousControllerFieldBuilder"></param>
        /// <param name="getQueueNodeControllerMethod"></param>
        /// <param name="controllerLocalBuilder"></param>
        /// <param name="getSocketMethod"></param>
        /// <param name="commandServerSocketLocalBuilder"></param>
        /// <param name="queueFieldBuilder"></param>
        /// <param name="queueLocalBuilder"></param>
        /// <param name="inputParameterFieldBuilder"></param>
        /// <param name="outputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder, MethodInfo getQueueNodeControllerMethod, LocalBuilder controllerLocalBuilder
            , MethodInfo getSocketMethod, LocalBuilder? commandServerSocketLocalBuilder
            , FieldInfo queueFieldBuilder, LocalBuilder? queueLocalBuilder
            , FieldBuilder? inputParameterFieldBuilder, LocalBuilder? outputParameterLocalBuilder = null)
#else
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder, MethodInfo getQueueNodeControllerMethod, LocalBuilder controllerLocalBuilder
            , MethodInfo getSocketMethod, LocalBuilder commandServerSocketLocalBuilder
            , FieldInfo queueFieldBuilder, LocalBuilder queueLocalBuilder
            , FieldBuilder inputParameterFieldBuilder, LocalBuilder outputParameterLocalBuilder = null)
#endif
        {
            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
            asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
            if (getQueueNodeControllerMethod != null)
            {
                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                asynchronousMethodGenerator.call(getQueueNodeControllerMethod);
            }
            asynchronousMethodGenerator.Emit(OpCodes.Stloc, controllerLocalBuilder);
            asynchronousMethodGenerator.Emit(OpCodes.Ldloca, controllerLocalBuilder);
            if (IsParameterSocket)
            {
                if (getSocketMethod != null)
                {
                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                    asynchronousMethodGenerator.call(getSocketMethod);
                }
                else if (commandServerSocketLocalBuilder != null) asynchronousMethodGenerator.Emit(OpCodes.Ldloc, commandServerSocketLocalBuilder);
            }
            if (queueFieldBuilder != null)
            {
                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
                asynchronousMethodGenerator.Emit(OpCodes.Ldfld, queueFieldBuilder);
            }
            if (queueLocalBuilder != null) asynchronousMethodGenerator.Emit(OpCodes.Ldloc, queueLocalBuilder);
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                if (!parameter.IsOut)
                {
                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder.notNull());
                    asynchronousMethodGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                }
                else
                {
                    asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, OutputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                }
            }
        }
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="asynchronousControllerFieldBuilder"></param>
        /// <param name="commandServerSocketLocalBuilder"></param>
        /// <param name="queueLocalBuilder"></param>
        /// <param name="inputParameterFieldBuilder"></param>
        /// <param name="outputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder
            , LocalBuilder commandServerSocketLocalBuilder, LocalBuilder queueLocalBuilder
            , FieldBuilder? inputParameterFieldBuilder, LocalBuilder? outputParameterLocalBuilder = null)
#else
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder
            , LocalBuilder commandServerSocketLocalBuilder, LocalBuilder queueLocalBuilder
            , FieldBuilder inputParameterFieldBuilder, LocalBuilder outputParameterLocalBuilder = null)
#endif
        {
            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
            asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
            //asynchronousMethodGenerator.Emit(OpCodes.Stloc, controllerLocalBuilder);
            //asynchronousMethodGenerator.Emit(OpCodes.Ldloca, controllerLocalBuilder);
            if (IsParameterSocket) asynchronousMethodGenerator.Emit(OpCodes.Ldloc, commandServerSocketLocalBuilder);
            if (queueLocalBuilder != null) asynchronousMethodGenerator.Emit(OpCodes.Ldloc, queueLocalBuilder);
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                if (!parameter.IsOut)
                {
                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder.notNull());
                    asynchronousMethodGenerator.Emit(parameter.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                }
                else
                {
                    asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, OutputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                }
            }
        }
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="asynchronousControllerFieldBuilder"></param>
        /// <param name="getSocketMethod"></param>
        /// <param name="inputParameterFieldBuilder"></param>
#if NetStandard21
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder, MethodInfo getSocketMethod, FieldBuilder? inputParameterFieldBuilder)
#else
        internal void CallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder asynchronousControllerFieldBuilder, MethodInfo getSocketMethod, FieldBuilder inputParameterFieldBuilder)
#endif
        {
            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
            asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
            if (IsParameterSocket)
            {
                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                asynchronousMethodGenerator.call(getSocketMethod);
            }
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder.notNull());
                asynchronousMethodGenerator.Emit(OpCodes.Ldfld, InputParameterType.notNull().GetField(Parameters[parameterIndex].Name.notNull()).notNull());
            }
        }

        /// <summary>
        /// 服务端接口方法排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(ServerInterfaceMethod left, ServerInterfaceMethod right)
        {
            int value = InterfaceMethod.Compare(left, right);
            return value != 0 ? value : ((byte)left.MethodType - (byte)right.MethodType);
        }

        /// <summary>
        /// switch (CommandServerSocket.GetCommandMethodIndex(socket))
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="methods"></param>
        /// <param name="doCommandReturnDeserializeErrorLabel"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Label[] DoCommandSwitchMethodIndex(ILGenerator doCommandGenerator, ServerInterfaceMethod?[] methods, out Label doCommandReturnDeserializeErrorLabel)
#else
        internal static Label[] DoCommandSwitchMethodIndex(ILGenerator doCommandGenerator, ServerInterfaceMethod[] methods, out Label doCommandReturnDeserializeErrorLabel)
#endif
        {
            LocalBuilder methodIndexLocalBuilder = doCommandGenerator.DeclareLocal(typeof(int));
            Label doCommandReturnUnknownLabel = doCommandGenerator.DefineLabel();
            Label[] doCommandLabels = new Label[methods.Length];
            int methodIndex = 0;
            foreach (var method in methods)
            {
                doCommandLabels[methodIndex++] = method == null ? doCommandReturnUnknownLabel : doCommandGenerator.DefineLabel();
            }
            doCommandGenerator.Emit(OpCodes.Ldarg_1);
            doCommandGenerator.call(ServerInterfaceController.CommandServerSocketGetCommandMethodIndex.Method);
            doCommandGenerator.Emit(OpCodes.Stloc_S, methodIndexLocalBuilder);
            doCommandGenerator.Emit(OpCodes.Ldloc_S, methodIndexLocalBuilder);
            doCommandGenerator.Emit(OpCodes.Switch, doCommandLabels);
            doCommandGenerator.MarkLabel(doCommandReturnUnknownLabel);
            doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.Unknown);
            doCommandGenerator.ret();
            doCommandReturnDeserializeErrorLabel = doCommandGenerator.DefineLabel();
            doCommandGenerator.MarkLabel(doCommandReturnDeserializeErrorLabel);
            doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.ServerDeserializeError);
            doCommandGenerator.ret();
            return doCommandLabels;
        }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="doCommandReturnDeserializeErrorLabel"></param>
        /// <param name="runTaskLabel"></param>
        /// <returns></returns>
#if NetStandard21
        internal LocalBuilder? InputParameterDeserialize(ILGenerator doCommandGenerator, ref Label doCommandReturnDeserializeErrorLabel, ref Label runTaskLabel)
#else
        internal LocalBuilder InputParameterDeserialize(ILGenerator doCommandGenerator, ref Label doCommandReturnDeserializeErrorLabel, ref Label runTaskLabel)
#endif
        {
            switch (MethodType)
            {
                case ServerMethodTypeEnum.Task:
                case ServerMethodTypeEnum.SendOnlyTask:
                case ServerMethodTypeEnum.CallbackTask:
                case ServerMethodTypeEnum.KeepCallbackTask:
                case ServerMethodTypeEnum.KeepCallbackCountTask:
                case ServerMethodTypeEnum.TwoStage‌CallbackTask:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountTask:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                    #region if (ServerInterfaceMethod.IsSynchronousCallTask(Method0))
                    doCommandGenerator.Emit(OpCodes.Ldsfld, MethodFieldBuilder);
                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                    doCommandGenerator.call(ServerInterfaceController.ServerInterfaceMethodIsSynchronousCallTask.Method);
                    doCommandGenerator.Emit(OpCodes.Brfalse, runTaskLabel = doCommandGenerator.DefineLabel());
                    #endregion
                    break;
            }
            if (InputParameterType == null) return null;
            switch (MethodType)
            {
                case ServerMethodTypeEnum.Queue:
                case ServerMethodTypeEnum.SendOnlyQueue:
                case ServerMethodTypeEnum.KeepCallbackQueue:
                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountQueue:
                case ServerMethodTypeEnum.CallbackQueue:
                case ServerMethodTypeEnum.ConcurrencyReadQueue:
                case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountConcurrencyReadQueue:
                case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.ReadWriteQueue:
                case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackReadWriteQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountReadWriteQueue:
                case ServerMethodTypeEnum.CallbackReadWriteQueue:
                    return null;
            }
            #region SynchronousInputParameter inputParameter = new SynchronousInputParameter();
            LocalBuilder inputParameterLocalBuilder = doCommandGenerator.DeclareLocal(InputParameterType.Type);
            if (InputParameterType.IsInitobj)
            {
                doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder);
                doCommandGenerator.Emit(OpCodes.Initobj, InputParameterType.Type);
            }
            #endregion
            #region if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
            doCommandGenerator.Emit(OpCodes.Ldarg_1);
            doCommandGenerator.Emit(OpCodes.Ldarg_2);
            doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder);
            doCommandGenerator.int32(IsSimpleDeserializeParamter);
            doCommandGenerator.call(ServerInterfaceController.CommandServerSocketDeserializeMethod.MakeGenericMethod(InputParameterType.Type));
            doCommandGenerator.Emit(OpCodes.Brfalse, doCommandReturnDeserializeErrorLabel);
            #endregion
            return inputParameterLocalBuilder;
        }
        /// <summary>
        /// 获取 Task 队列调用代理类型
        /// </summary>
        /// <param name="isVerifyMethodIndex"></param>
        /// <param name="returnGenericType"></param>
        /// <returns></returns>
#if NetStandard21
        internal Type GetCommandServerCallTaskQueueTaskType(bool isVerifyMethodIndex, out GenericType? returnGenericType)
#else
        internal Type GetCommandServerCallTaskQueueTaskType(bool isVerifyMethodIndex, out GenericType returnGenericType)
#endif
        {
            returnGenericType = ReturnValueType != typeof(void) ? GenericType.Get(ReturnValueType) : null;
            switch (MethodType)
            {
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                    return typeof(CommandServerKeepCallbackQueueTask);
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                    return returnGenericType.notNull().CommandServerKeepCallbackQueueTaskType;
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                    return returnGenericType.notNull().CommandServerAsyncEnumerableQueueTaskType;
#endif
                case ServerMethodTypeEnum.SendOnlyTaskQueue: return typeof(CommandServerCallTaskQueueSendOnlyTask);
                case ServerMethodTypeEnum.CallbackTaskQueue: return typeof(CommandServerCallbackTaskQueueTask);
                default:
                    if (ReturnValueType == typeof(void)) return typeof(CommandServerCallTaskQueueTask);
                    if (isVerifyMethodIndex) return typeof(CommandServerCallTaskQueueVerifyStateTask);
                    return returnGenericType.notNull().CommandServerCallTaskQueueTaskType;
            }
        }
        /// <summary>
        /// Task 队列调用代理类型调用基类构造函数
        /// </summary>
        /// <param name="asynchronousConstructorBuilder"></param>
        /// <param name="returnGenericType"></param>
        /// <param name="commandServerCallTaskQueueTaskType"></param>
        /// <param name="isVerifyMethodIndex"></param>
        /// <returns></returns>
#if NetStandard21
        internal ILGenerator TaskQueueAsynchronousConstructorBase(ConstructorBuilder asynchronousConstructorBuilder, GenericType? returnGenericType, Type commandServerCallTaskQueueTaskType, bool isVerifyMethodIndex)
#else
        internal ILGenerator TaskQueueAsynchronousConstructorBase(ConstructorBuilder asynchronousConstructorBuilder, GenericType returnGenericType, Type commandServerCallTaskQueueTaskType, bool isVerifyMethodIndex)
#endif
        {
            ILGenerator asynchronousConstructorGenerator = asynchronousConstructorBuilder.GetILGenerator();
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
            //asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
            switch (MethodType)
            {
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                    asynchronousConstructorGenerator.int32((byte)MethodType);
                    asynchronousConstructorGenerator.int32(MethodAttribute.AutoCancelKeep);
                    asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.ServerKeepCallbackQueueTaskConstructor);
                    break;
                case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                    asynchronousConstructorGenerator.int32((byte)MethodType);
                    asynchronousConstructorGenerator.int32(0);
                    asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.ServerKeepCallbackQueueTaskConstructor);
                    break;
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                    asynchronousConstructorGenerator.Emit(OpCodes.Call, commandServerCallTaskQueueTaskType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerInterfaceController.CommandServerSocketParameterTypes, null).notNull());
                    break;
                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                    asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.CommandServerCallTaskQueueSendOnlyTaskConstructor);
                    break;
                case ServerMethodTypeEnum.CallbackTaskQueue:
                    asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.CommandServerCallbackTaskQueueTaskConstructor);
                    break;
                default:
                    if (returnGenericType == null) asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.CommandServerCallTaskQueueTaskConstructor);
                    else
                    {
                        asynchronousConstructorGenerator.Emit(OpCodes.Ldsfld, MethodFieldBuilder);
                        if (isVerifyMethodIndex) asynchronousConstructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.CommandServerCallTaskQueueVerifyStateTaskConstructor);
                        else asynchronousConstructorGenerator.Emit(OpCodes.Call, commandServerCallTaskQueueTaskType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerInterfaceController.CommandServerSocketInterfaceMethodParameterTypes, null).notNull());
                    }
                    break;
            }
            return asynchronousConstructorGenerator;
        }
        /// <summary>
        /// Task 队列调用代理类型控制器定义
        /// </summary>
        /// <param name="asynchronousTypeBuilder"></param>
        /// <param name="asynchronousConstructorGenerator"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal FieldBuilder GetTaskQueueControllerFieldBuilder(TypeBuilder asynchronousTypeBuilder, ILGenerator asynchronousConstructorGenerator, Type type)
        {
            FieldBuilder asynchronousControllerFieldBuilder = asynchronousTypeBuilder.DefineField("controller", type, FieldAttributes.Public | FieldAttributes.InitOnly);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
            asynchronousConstructorGenerator.Emit(OpCodes.Stfld, asynchronousControllerFieldBuilder);
            return asynchronousControllerFieldBuilder;
        }
        /// <summary>
        /// Task 队列调用代理类型输入参数定义
        /// </summary>
        /// <param name="asynchronousTypeBuilder"></param>
        /// <param name="asynchronousConstructorGenerator"></param>
        /// <returns></returns>
#if NetStandard21
        internal FieldBuilder? GetTaskQueueInputParameterFieldBuilder(TypeBuilder asynchronousTypeBuilder, ILGenerator asynchronousConstructorGenerator)
#else
        internal FieldBuilder GetTaskQueueInputParameterFieldBuilder(TypeBuilder asynchronousTypeBuilder, ILGenerator asynchronousConstructorGenerator)
#endif
        {
            if (InputParameterType == null) return null;
            FieldBuilder inputParameterFieldBuilder = asynchronousTypeBuilder.DefineField("inputParameter", InputParameterType.Type, FieldAttributes.Public);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
            //asynchronousConstructorGenerator.ldarg(4);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_3);
            asynchronousConstructorGenerator.Emit(OpCodes.Ldobj, InputParameterType.Type);
            asynchronousConstructorGenerator.Emit(OpCodes.Stfld, inputParameterFieldBuilder);
            return inputParameterFieldBuilder;
        }
        /// <summary>
        /// 获取 Task 队列调用保持回调
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="returnGenericType"></param>
        /// <returns></returns>
#if NetStandard21
        internal LocalBuilder? GetTaskQueueKeepCallbackLocalBuilder(ILGenerator asynchronousMethodGenerator, GenericType? returnGenericType)
#else
        internal LocalBuilder GetTaskQueueKeepCallbackLocalBuilder(ILGenerator asynchronousMethodGenerator, GenericType returnGenericType)
#endif
        {
            var keepCallbackLocalBuilder = default(LocalBuilder);
            switch (MethodType)
            {
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                    keepCallbackLocalBuilder = asynchronousMethodGenerator.DeclareLocal(typeof(CommandServerKeepCallback));
                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                    if (ReturnValueType == typeof(void)) asynchronousMethodGenerator.call(ServerInterfaceController.CreateCommandServerKeepCallbackTaskQueueDelegate.Method);
                    else
                    {
                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, MethodFieldBuilder);
                        asynchronousMethodGenerator.call(returnGenericType.notNull().CreateCommandServerKeepCallbackTaskQueueDelegate.Method);
                    }
                    asynchronousMethodGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder);
                    break;
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                    keepCallbackLocalBuilder = asynchronousMethodGenerator.DeclareLocal(typeof(CommandServerKeepCallbackCount));
                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                    if (ReturnValueType == typeof(void))
                    {
                        asynchronousMethodGenerator.int32(Math.Max(MethodAttribute.KeepCallbackOutputCount, 1));
                        asynchronousMethodGenerator.call(ServerInterfaceController.CreateCommandServerKeepCallbackCountTaskQueueDelegate.Method);
                    }
                    else
                    {
                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, MethodFieldBuilder);
                        asynchronousMethodGenerator.call(returnGenericType.notNull().CreateCommandServerKeepCallbackCountTaskQueueDelegate.Method);
                    }
                    asynchronousMethodGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder);
                    break;
            }
            return keepCallbackLocalBuilder;
        }
        /// <summary>
        /// Task 队列方法调用传参
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="inputParameterFieldBuilder"></param>
        internal void TaskQueueCallMethodParameter(ILGenerator asynchronousMethodGenerator, FieldBuilder inputParameterFieldBuilder)
        {
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder);
                asynchronousMethodGenerator.Emit(OpCodes.Ldfld, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
            }
        }
        /// <summary>
        /// Task 队列方法调用以后检查是否完成
        /// </summary>
        /// <param name="asynchronousMethodGenerator"></param>
        /// <param name="returnGenericType"></param>
        /// <param name="isVerifyMethodIndex"></param>
#if NetStandard21
        internal void CheckCallTaskQueue(ILGenerator asynchronousMethodGenerator, GenericType? returnGenericType, bool isVerifyMethodIndex)
#else
        internal void CheckCallTaskQueue(ILGenerator asynchronousMethodGenerator, GenericType returnGenericType, bool isVerifyMethodIndex)
#endif
        {
            switch (MethodType)
            {
                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackTaskQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountTaskQueue:
                    asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerKeepCallbackQueueTaskCheckCallTask.Method);
                    break;
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                    asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerKeepCallbackQueueTaskCheckCallTaskDelegate.Method);
                    break;
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                    asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerAsyncEnumerableQueueTaskCheckCallTaskDelegate.Method);
                    break;
#endif
                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                    asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerCallTaskQueueSendOnlyTaskCheckCallTask.Method);
                    break;
                case ServerMethodTypeEnum.CallbackTaskQueue:
                    asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerCallbackTaskQueueTaskCheckCallTask.Method);
                    break;
                default:
                    if (returnGenericType == null) asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerCallTaskQueueTaskCheckCallTask.Method);
                    else if (isVerifyMethodIndex) asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerCallTaskQueueVerifyStateTaskCheckCallTask.Method);
                    else asynchronousMethodGenerator.call(returnGenericType.CommandServerCallTaskQueueTaskCheckCallTaskDelegate.Method);
                    break;
            }
        }
#endif
    }
}