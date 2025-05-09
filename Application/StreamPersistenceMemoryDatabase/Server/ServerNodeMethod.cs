using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
#if !AOT
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法信息
    /// </summary>
    internal sealed class ServerNodeMethod : NodeMethod
    {
        /// <summary>
        /// 自定义基础服务节点方法最小方法编号
        /// </summary>
        internal const int MinCustomServiceNodeMethodIndex = 256;
        /// <summary>
        /// 数据库冷启动加载历史持久化请求方法的名称后缀
        /// </summary>
        internal const string LoadPersistenceMethodNameSuffix = "LoadPersistence";
        /// <summary>
        /// 持久化之前检查参数方法的名称后缀
        /// </summary>
        internal const string BeforePersistenceMethodNameSuffix = "BeforePersistence";
#if !AOT
        /// <summary>
        /// 创建节点方法编号
        /// </summary>
        private static int createMethodIndex;
#endif

        /// <summary>
        /// 节点方法自定义属性
        /// </summary>
        internal readonly ServerMethodAttribute MethodAttribute;
#if !AOT
        /// <summary>
        /// 修复方法集合
        /// </summary>
#if NetStandard21
        private Dictionary<HashObject<MethodInfo>, Method>? repairMethods;
#else
        private Dictionary<HashObject<MethodInfo>, Method> repairMethods;
#endif
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        internal MethodInfo RepairNodeMethod;
#endif
        /// <summary>
        /// 持久化方法名称
        /// </summary>
        internal SubString PersistenceMethodName;
        /// <summary>
        /// 持久化方法返回数据类型
        /// </summary>
        internal readonly Type PersistenceMethodReturnType;
        /// <summary>
        /// 持久化之前参数检查方法编号
        /// </summary>
        internal int BeforePersistenceMethodIndex = int.MinValue;
        /// <summary>
        /// 冷启动加载持久化方法编号
        /// </summary>
        internal int LoadPersistenceMethodIndex = int.MinValue;
        /// <summary>
        /// 队列节点类型
        /// </summary>
        internal ReadWriteNodeTypeEnum QueueNodeType;
        /// <summary>
        /// 默认为 true 表示调用需要持久化，如果调用不涉及数据变更操作则应该手动设置为 false 避免垃圾数据被持久化
        /// </summary>
        internal bool IsPersistence;
        /// <summary>
        /// 默认为 true 表示允许客户端调用，否则为服务端内存调用方法
        /// </summary>
        internal bool IsClientCall;
        /// <summary>
        /// 是否冷启动加载持久化方法
        /// </summary>
        internal bool IsLoadPersistenceMethod;
        /// <summary>
        /// 是否持久化之前检查参数方法
        /// </summary>
        internal bool IsBeforePersistenceMethod;
        /// <summary>
        /// 服务端节点方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="methodAttribute"></param>
        internal unsafe ServerNodeMethod(Type type, MethodInfo method, ServerMethodAttribute methodAttribute) : base(type, method)
        {
            MethodAttribute = methodAttribute;
            MethodIndex = MethodAttribute.MethodIndex;
            IsClientCall = MethodAttribute.IsClientCall;
            IsPersistence = MethodAttribute.IsPersistence;
            QueueNodeType = MethodAttribute.IsWriteQueue && !IsPersistence ? ReadWriteNodeTypeEnum.Write : ReadWriteNodeTypeEnum.Read;
#if NetStandard21
            PersistenceMethodReturnType = typeof(void);
#if !AOT
            RepairNodeMethod = AutoCSer.Common.NullMethodInfo;
#endif
#endif

            Parameters = method.GetParameters();
            if (method.IsStatic)
            {
                if (Parameters.Length == 0 || !type.IsAssignableFrom(Parameters[0].ParameterType))
                {
                    SetError(CallStateEnum.RepairMethodNotFoundNodeTypeParameter, $"修复方法 {type.fullName()}+{Method.Name} 第一个输入参数必须是节点接口类型");
                    return;
                }
                Parameters = new SubArray<ParameterInfo>(1, Parameters.Length - 1, Parameters).GetArray();
            }
            ParameterEndIndex = Parameters.Length;
            ReturnValueType = method.ReturnType;

            if (MethodAttribute.IsSendOnly)
            {
                if (ReturnValueType != typeof(void))
                {
                    SetError(CallStateEnum.SendOnlyNotSupportReturnType, $"节点方法 {type.fullName()}.{Method.Name} 已设置不应答，不允许存在返回值");
                    return;
                }
                if (ParameterEndIndex == 0)
                {
                    SetError(CallStateEnum.SendOnlyMustInputParameter, $"节点方法 {type.fullName()}.{Method.Name} 已设置不应答，必须存在输入参数");
                    return;
                }
                CallType = CallTypeEnum.SendOnly;
            }
            else
            {
                if (ReturnValueType == typeof(void))
                {
                    if (ParameterEndIndex != 0)
                    {
                        Type parameterType = Parameters[ParameterEndIndex - 1].ParameterType;
                        if (parameterType.IsGenericType)
                        {
                            Type genericType = parameterType.GetGenericTypeDefinition();
                            if (genericType == typeof(MethodCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                --ParameterEndIndex;
                                CallType = ParameterStartIndex == ParameterEndIndex ? CallTypeEnum.Callback : CallTypeEnum.InputCallback;
                            }
                            else if (genericType == typeof(MethodKeepCallback<>))
                            {
                                ReturnValueType = parameterType.GetGenericArguments()[0];
                                --ParameterEndIndex;
                                CallType = ParameterStartIndex == ParameterEndIndex ? CallTypeEnum.KeepCallback : CallTypeEnum.InputKeepCallback;
                            }
                        }
                    }
                }
                else if (ReturnValueType.IsGenericType && ReturnValueType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                    CallType = ParameterStartIndex == ParameterEndIndex ? CallTypeEnum.Enumerable : CallTypeEnum.InputEnumerable;
                }
                if (CallType == CallTypeEnum.Unknown) setCallType();
            }
            if (!checkParameter()) return;
            if (Method.Name.EndsWith(LoadPersistenceMethodNameSuffix, StringComparison.Ordinal))
            {
                PersistenceMethodName.Set(Method.Name, 0, Method.Name.Length - LoadPersistenceMethodNameSuffix.Length);
                IsLoadPersistenceMethod = true;
            }
            else if (Method.Name.EndsWith(BeforePersistenceMethodNameSuffix, StringComparison.Ordinal))
            {
                IsBeforePersistenceMethod = true;
                switch (CallType)
                {
                    case CallTypeEnum.CallOutput:
                    case CallTypeEnum.CallInputOutput:
                        PersistenceMethodName.Set(Method.Name, 0, Method.Name.Length - BeforePersistenceMethodNameSuffix.Length);
                        if (method.ReturnType == typeof(bool))
                        {
                            PersistenceMethodReturnType = typeof(void);
                            break;
                        }
                        else if (method.ReturnType.GetGenericTypeDefinition() == typeof(ValueResult<>))
                        {
                            PersistenceMethodReturnType = method.ReturnType.GetGenericArguments()[0];
                            break;
                        }
                        SetError(CallStateEnum.BeforePersistenceMethodReturnTypeError, $"{type.fullName()} 持久化检查方法 {Method.Name} 返回值类型必须为 {typeof(bool).fullName()} 或者 {typeof(ValueResult<>).fullName()}");
                        return;
                    default:
                        SetError(CallStateEnum.BeforePersistenceMethodCallTypeError, $"{type.fullName()} 持久化检查方法 {Method.Name} 调用类型不匹配 {CallType}");
                        return;
                }
            }
#if !AOT
            if (AutoCSer.Common.IsCodeGenerator) return;
#endif
            if (ParameterStartIndex != ParameterEndIndex)
            {
                InputParameterType = AutoCSer.Net.CommandServer.ServerMethodParameter.GetOrCreate(ParameterCount, InputParameters, typeof(void));
                InputParameterFields = InputParameterType.GetFields(InputParameters);
                IsSimpleDeserializeParamter = InputParameterType.IsSimpleSerialize;
            }
            if (!IsBeforePersistenceMethod)
            {
                if (ReturnValueType != typeof(void) && ReturnValueType != typeof(ResponseParameter)) IsSimpleSerializeParamter = SimpleSerialize.Serializer.IsType(ReturnValueType);
            }
            else
            {
                if (PersistenceMethodReturnType != typeof(void) && ReturnValueType != typeof(ResponseParameter)) IsSimpleSerializeParamter = SimpleSerialize.Serializer.IsType(PersistenceMethodReturnType);
            }
        }
        /// <summary>
        /// 冷启动持久化方法匹配
        /// </summary>
        /// <param name="loadPersistenceMethod"></param>
        /// <param name="error"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool CheckLoadPersistence(ServerNodeMethod loadPersistenceMethod, ref string? error)
#else
        internal bool CheckLoadPersistence(ServerNodeMethod loadPersistenceMethod, ref string error)
#endif
        {
            if (IsPersistence && loadPersistenceMethod.PersistenceMethodName.Equals(Method.Name))
            {
                if (InputParameterType == loadPersistenceMethod.InputParameterType)
                {
                    if (ReturnValueType == loadPersistenceMethod.ReturnValueType)
                    {
                        LoadPersistenceMethodIndex = loadPersistenceMethod.setIsPersistenceMethod();
                        return true;
                    }
                    error = $"{Type.fullName()} 冷启动持久化方法 {loadPersistenceMethod.Method.Name} 返回值类型不匹配 {ReturnValueType.fullName()}";
                }
                else error = $"{Type.fullName()} 冷启动持久化方法 {loadPersistenceMethod.Method.Name} 与 {Method.Name} 输入参数不匹配";
            }
            return false;
        }
        /// <summary>
        /// 持久化检查方法匹配
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <param name="error"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool CheckBeforePersistence(ServerNodeMethod beforePersistenceMethod, ref string? error)
#else
        internal bool CheckBeforePersistence(ServerNodeMethod beforePersistenceMethod, ref string error)
#endif
        {
            if (IsPersistence && beforePersistenceMethod.PersistenceMethodName.Equals(Method.Name))
            {
                if (InputParameterType == beforePersistenceMethod.InputParameterType)
                {
                    if (ReturnValueType == beforePersistenceMethod.PersistenceMethodReturnType)
                    {
                        BeforePersistenceMethodIndex = beforePersistenceMethod.setIsPersistenceMethod();
                        return true;
                    }
                    error = $"{Type.fullName()} 持久化检查方法 {beforePersistenceMethod.Method.Name} 返回值类型不匹配 {typeof(ValueResult<>).MakeGenericType(ReturnValueType).fullName()}";
                }
                else error = $"{Type.fullName()} 持久化检查方法 {beforePersistenceMethod.Method.Name} 与 {Method.Name} 输入参数不匹配";
            }
            return false;
        }
        /// <summary>
        /// 设置持久化检查方法
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int setIsPersistenceMethod()
        {
            IsClientCall = IsPersistence = false;
            return MethodIndex;
        }
#if !AOT
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="inputParameterLocalBuilder"></param>
#if NetStandard21
        internal void CallMethodParameter(ILGenerator doCommandGenerator, LocalBuilder? inputParameterLocalBuilder)
#else
        internal void CallMethodParameter(ILGenerator doCommandGenerator, LocalBuilder inputParameterLocalBuilder)
#endif
        {
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                doCommandGenerator.Emit(OpCodes.Ldloc_S, inputParameterLocalBuilder.notNull());
                doCommandGenerator.Emit(OpCodes.Ldfld, InputParameterType.notNull().GetField(parameter.Name.notNull()).notNull());
            }
        }
        /// <summary>
        /// 检查修复方法定义是否一致
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        internal CallStateEnum CheckRepair(Type type, MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == Method.ReturnType)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length != 0 && type.IsAssignableFrom(parameters[0].ParameterType))
                {
                    if (parameters.Length == Parameters.Length + 1)
                    {
                        int parameterIndex = -2;
                        foreach (ParameterInfo parameter in parameters)
                        {
                            if (++parameterIndex >= 0 && Parameters[parameterIndex].ParameterType != parameter.ParameterType) return CallStateEnum.RepairMethodParameterTypeNotMatch;
                            if (parameter.ParameterType.IsByRef) return CallStateEnum.NodeMethodParameterIsByRef;
                        }
                        return CallStateEnum.Success;
                    }
                    return CallStateEnum.RepairMethodParameterCountNotMatch;
                }
                return CallStateEnum.RepairMethodNotFoundNodeTypeParameter;
            }
            return CallStateEnum.RepairMethodReturnTypeNotMatch;
        }
        /// <summary>
        /// 创建服务端节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repairMethod">修复方法信息</param>
        /// <returns></returns>
#if NetStandard21
        internal Method CreateMethod<T>(MethodInfo? repairMethod)
#else
        internal Method CreateMethod<T>(MethodInfo repairMethod)
#endif
        {
            if (repairMethod == null) return createMethod<T>(null);
            HashObject<MethodInfo> hashKey = repairMethod;
            if (repairMethods == null) repairMethods = DictionaryCreator.CreateHashObject<MethodInfo, Method>();
            var method = default(Method);
            if (!repairMethods.TryGetValue(hashKey, out method)) repairMethods.Add(hashKey, method = createMethod<T>(repairMethod));
            return method;
        }
        /// <summary>
        /// 创建服务端节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repairMethod">修复方法信息</param>
        /// <returns></returns>
#if NetStandard21
        private Method createMethod<T>(MethodInfo? repairMethod)
#else
        private Method createMethod<T>(MethodInfo repairMethod)
#endif
        {
            var parentType = default(Type);
            Type callMethodReturnType = typeof(void);
            var callMethod = default(MethodInfo);
            var getParameterMethod = default(MethodInfo);
            var callMethodParameterTypes = default(Type[]);
            switch (CallType)
            {
                case CallTypeEnum.Call:
                    parentType = typeof(CallMethod);
                    callMethod = ServerNodeCreator.CallMethod;
                    callMethodParameterTypes = ServerNodeCreator.CallMethodParameterTypes;
                    break;
                case CallTypeEnum.CallOutput:
                case CallTypeEnum.Callback:
                    parentType = typeof(CallOutputMethod);
                    if (!IsBeforePersistenceMethod)
                    {
                        callMethod = ServerNodeCreator.CallOutputMethod;
                        callMethodParameterTypes = ServerNodeCreator.CallOutputMethodParameterTypes;
                    }
                    else
                    {
                        if (PersistenceMethodReturnType == typeof(void))
                        {
                            callMethod = ServerNodeCreator.CallOutputCallBeforePersistenceMethod;
                            callMethodReturnType = typeof(bool);
                        }
                        else
                        {
                            callMethod = ServerNodeCreator.CallOutputCallOutputBeforePersistenceMethod;
                            callMethodReturnType = typeof(ValueResult<ResponseParameter>);
                        }
                        callMethodParameterTypes = ServerNodeCreator.CallOutputBeforePersistenceMethodParameterTypes;
                    }
                    break;
                case CallTypeEnum.CallInput:
                    parentType = typeof(CallInputMethod<>).MakeGenericType(InputParameterType.notNull().Type);
                    callMethod = ServerNodeCreator.CallInputMethod;
                    callMethodParameterTypes = ServerNodeCreator.CallInputMethodParameterTypes;
                    getParameterMethod = typeof(CallInputMethodParameter<>).MakeGenericType(InputParameterType.notNull().Type).GetMethod(nameof(CallInputMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.CallInputOutput:
                case CallTypeEnum.InputCallback:
                    parentType = typeof(CallInputOutputMethod<>).MakeGenericType(InputParameterType.notNull().Type);
                    if (!IsBeforePersistenceMethod) callMethod = ServerNodeCreator.CallInputOutputMethod;
                    else if (PersistenceMethodReturnType == typeof(void))
                    {
                        callMethod = ServerNodeCreator.CallInputOutputCallBeforePersistenceMethod;
                        callMethodReturnType = typeof(bool);
                    }
                    else
                    {
                        callMethod = ServerNodeCreator.CallInputOutputCallOutputBeforePersistenceMethod;
                        callMethodReturnType = typeof(ValueResult<ResponseParameter>);
                    }
                    callMethodParameterTypes = ServerNodeCreator.CallInputOutputMethodParameterTypes;
                    getParameterMethod = typeof(CallInputOutputMethodParameter<>).MakeGenericType(InputParameterType.notNull().Type).GetMethod(nameof(CallInputOutputMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.SendOnly:
                    parentType = typeof(SendOnlyMethod<>).MakeGenericType(InputParameterType.notNull().Type);
                    callMethod = ServerNodeCreator.SendOnlyMethod;
                    callMethodParameterTypes = ServerNodeCreator.SendOnlyMethodParameterTypes;
                    getParameterMethod = typeof(SendOnlyMethodParameter<>).MakeGenericType(InputParameterType.notNull().Type).GetMethod(nameof(SendOnlyMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.KeepCallback:
                case CallTypeEnum.Enumerable:
                    parentType = typeof(KeepCallbackMethod);
                    callMethod = ServerNodeCreator.KeepCallbackMethod;
                    callMethodParameterTypes = ServerNodeCreator.KeepCallbackMethodParameterTypes;
                    break;
                case CallTypeEnum.InputKeepCallback:
                case CallTypeEnum.InputEnumerable:
                    parentType = typeof(InputKeepCallbackMethod<>).MakeGenericType(InputParameterType.notNull().Type);
                    callMethod = ServerNodeCreator.InputKeepCallbackMethod;
                    callMethodParameterTypes = ServerNodeCreator.InputKeepCallbackMethodParameterTypes;
                    getParameterMethod = typeof(InputKeepCallbackMethodParameter<>).MakeGenericType(InputParameterType.notNull().Type).GetMethod(nameof(InputKeepCallbackMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
            }
            string methodName = AutoCSer.Common.NamePrefix + ".CommandService.StreamPersistenceMemoryDatabase." + Type.FullName + "." + Method.Name + "." + MethodIndex.toString() + "." + Interlocked.Increment(ref createMethodIndex).toString();
            if (repairMethod != null) methodName += ".Repair." + repairMethod.Name;
            TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(methodName, TypeAttributes.Class | TypeAttributes.Sealed, parentType);
            #region 构造函数
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, EmptyArray<Type>.Array);
            ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
            #region base(index, flags)
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.int32(MethodIndex);
            constructorGenerator.int32(BeforePersistenceMethodIndex);
            bool isCallType = false;
            switch (CallType)
            {
                case CallTypeEnum.Callback:
                case CallTypeEnum.InputCallback:
                case CallTypeEnum.Enumerable:
                case CallTypeEnum.InputEnumerable:
                    constructorGenerator.int32((byte)CallType);
                    isCallType = true;
                    break;
            }
            MethodFlagsEnum flags = GetMethodFlags();
            constructorGenerator.int32((byte)flags);
            if (isCallType)
            {
                constructorGenerator.Emit(OpCodes.Call, parentType.notNull().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerNodeCreator.CallTypeMethodConstructorParameterTypes, null).notNull());
            }
            else
            {
                constructorGenerator.Emit(OpCodes.Call, parentType.notNull().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerNodeCreator.MethodConstructorParameterTypes, null).notNull());
            }
            #endregion
            constructorGenerator.Emit(OpCodes.Ret);
            #endregion
            #region public override void Call(Node node, ref CommandServerCallback<CallStateEnum> callback)
            MethodBuilder callMethodBuilder = typeBuilder.DefineMethod(callMethod.notNull().Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, callMethodReturnType, callMethodParameterTypes);
            ILGenerator callMethodGenerator = callMethodBuilder.GetILGenerator();
            #region p0 parameter = CallInputMethodParameter<p0>.GetParameter(((CallInputMethodParameter<p0>)methodParameter));
            var inputParameterLocalBuilder = default(LocalBuilder);
            if (InputParameterType != null)
            {
                inputParameterLocalBuilder = callMethodGenerator.DeclareLocal(InputParameterType.Type);
                callMethodGenerator.ldarg(1);
                callMethodGenerator.call(getParameterMethod.notNull());
                callMethodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
            }
            #endregion
            switch (CallType)
            {
                case CallTypeEnum.CallInputOutput:
                    if (!IsBeforePersistenceMethod || (PersistenceMethodReturnType != typeof(void) && ReturnValueType != typeof(ResponseParameter))) callMethodGenerator.ldarg(1);
                    break;
                case CallTypeEnum.InputEnumerable:
                    if (!IsBeforePersistenceMethod || PersistenceMethodReturnType != typeof(void)) callMethodGenerator.ldarg(1);
                    break;
            }
            #region Node<IDictionary<KT, VT>>.GetTarget(((Node<IDictionary<KT, VT>>)node)).Call();
            callMethodGenerator.ldarg(1);
            switch (CallType)
            {
                case CallTypeEnum.CallInput:
                case CallTypeEnum.SendOnly:
                case CallTypeEnum.CallInputOutput:
                case CallTypeEnum.InputCallback:
                case CallTypeEnum.InputKeepCallback:
                case CallTypeEnum.InputEnumerable:
                    //callMethodGenerator.call(ServerNodeCreator.MethodParameterGetNode.Method);
                    callMethodGenerator.call(((Func<MethodParameter, T>)MethodParameter.GetNodeTarget<T>).Method);
                    break;
                default:
                    callMethodGenerator.call(((Func<ServerNode<T>, T>)ServerNode<T>.GetTarget).Method);
                    break;
            }
            CallMethodParameter(callMethodGenerator, inputParameterLocalBuilder);
            switch (CallType)
            {
                case CallTypeEnum.Callback:
                    #region MethodCallback<T>.Create(ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32((byte)flags);
                    callMethodGenerator.call(GenericType.Get(ReturnValueType).CreateMethodCallbackDelegate.Method);
                    #endregion
                    break;
                case CallTypeEnum.KeepCallback:
                    #region MethodKeepCallback<T>.Create(ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32((byte)flags);
                    callMethodGenerator.call(GenericType.Get(ReturnValueType).CreateMethodKeepCallbackDelegate.Method);
                    #endregion
                    break;
                case CallTypeEnum.InputCallback:
                    #region MethodCallback<T>.Create(methodParameter)
                    callMethodGenerator.ldarg(1);
                    callMethodGenerator.call(GenericType.Get(ReturnValueType).CreateMethodParameterCallbackDelegate.Method);
                    #endregion
                    break;
                case CallTypeEnum.InputKeepCallback:
                    #region MethodKeepCallback<T>.Create(methodParameter)
                    callMethodGenerator.ldarg(1);
                    callMethodGenerator.call(GenericType.Get(ReturnValueType).CreateMethodParameterKeepCallbackDelegate.Method);
                    #endregion
                    break;
            }
            callMethodGenerator.call(repairMethod ?? Method);
            switch (CallType)
            {
                case CallTypeEnum.Call:
                    #region CallMethod.Callback(ref callback);
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.call(ServerNodeCreator.CallMethodCallback.Method);
                    #endregion
                    break;
                case CallTypeEnum.CallOutput:
                    if (!IsBeforePersistenceMethod)
                    {
                        #region CallOutputMethod.Callback(X, ref callback, isSimpleSerialize);
                        callMethodGenerator.ldarg(2);
                        if (ReturnValueType == typeof(ResponseParameter)) callMethodGenerator.call(ServerNodeCreator.CallOutputMethodCallbackResponseParameter.Method);
                        else
                        {
                            callMethodGenerator.int32((byte)flags);
                            callMethodGenerator.call(ServerNodeCreator.CallOutputMethodCallbackMethod.MakeGenericMethod(ReturnValueType));
                        }
                        #endregion
                    }
                    else if (PersistenceMethodReturnType != typeof(void) && ReturnValueType != typeof(ResponseParameter))
                    {
                        #region CallOutputMethod.GetBeforePersistenceResponseParameter(X, isSimpleSerialize);
                        callMethodGenerator.int32((byte)flags);
                        callMethodGenerator.call(ServerNodeCreator.CallOutputMethodGetBeforePersistenceResponseParameterMethod.MakeGenericMethod(PersistenceMethodReturnType));
                        #endregion
                    }
                    break;
                case CallTypeEnum.CallInput:
                    #region CallInputMethodParameter.Callback(methodParameter);
                    callMethodGenerator.ldarg(1);
                    callMethodGenerator.call(ServerNodeCreator.CallInputMethodParameterCallback.Method);
                    #endregion
                    break;
                case CallTypeEnum.CallInputOutput:
                    if (!IsBeforePersistenceMethod)
                    {
                        #region CallInputOutputMethodParameter.Callback(methodParameter, X);
                        if (ReturnValueType == typeof(ResponseParameter))
                        {
                            callMethodGenerator.call(ServerNodeCreator.CallInputOutputMethodParameterCallbackResponseParameter.Method);
                        }
                        else
                        {
                            callMethodGenerator.call(ServerNodeCreator.CallInputOutputMethodParameterCallbackMethod.MakeGenericMethod(ReturnValueType));
                        }
                        #endregion
                    }
                    else if (PersistenceMethodReturnType != typeof(void) && ReturnValueType != typeof(ResponseParameter))
                    {
                        #region CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter(methodParameter, X);
                        callMethodGenerator.call(ServerNodeCreator.CallInputOutputMethodParameterGetBeforePersistenceResponseParameterMethod.MakeGenericMethod(PersistenceMethodReturnType));
                        #endregion
                    }
                    break;
                case CallTypeEnum.Enumerable:
                    #region KeepCallbackMethod.EnumerableCallback(X, ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32((byte)flags);
                    callMethodGenerator.call(ServerNodeCreator.KeepCallbackMethodEnumerableCallbackMethod.MakeGenericMethod(ReturnValueType));
                    #endregion
                    break;
                case CallTypeEnum.InputEnumerable:
                    #region InputKeepCallbackMethodParameter.EnumerableCallback(methodParameter, X);
                    callMethodGenerator.call(ServerNodeCreator.InputKeepCallbackMethodParameterEnumerableCallbackMethod.MakeGenericMethod(ReturnValueType));
                    #endregion
                    break;
            }
            #endregion
            callMethodGenerator.Emit(OpCodes.Ret);
            #endregion
            return (Method)typeBuilder.CreateType().GetConstructor(EmptyArray<Type>.Array).notNull().Invoke(null);
        }
#endif
        /// <summary>
        /// 获取服务端节点方法标记
        /// </summary>
        /// <returns></returns>
        internal MethodFlagsEnum GetMethodFlags()
        {
            MethodFlagsEnum flags = MethodFlagsEnum.None;
            if (IsPersistence) flags |= MethodFlagsEnum.IsPersistence;
            if (IsClientCall) flags |= MethodFlagsEnum.IsClientCall;
            if (IsSimpleSerializeParamter) flags |= MethodFlagsEnum.IsSimpleSerializeParamter;
            if (IsSimpleDeserializeParamter) flags |= MethodFlagsEnum.IsSimpleDeserializeParamter;
            if (MethodAttribute.IsIgnorePersistenceCallbackException) flags |= MethodFlagsEnum.IsIgnorePersistenceCallbackException;
            if (QueueNodeType != ReadWriteNodeTypeEnum.Read) flags |= MethodFlagsEnum.IsWriteQueue;
            return flags;

        }
        /// <summary>
        /// 自定义基础服务节点方法检查
        /// </summary>
        internal void CheckCustomServiceNode()
        {
            if (Method.DeclaringType != typeof(IServiceNode))
            {
                if (MethodIndex < MinCustomServiceNodeMethodIndex) MethodIndex = -1;
                isCustomBaseMethod = true;
            }
        }

        /// <summary>
        /// 获取服务端接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methods"></param>
        /// <returns>错误信息</returns>
#if NetStandard21
        internal static string? GetMethod(Type type, ref LeftArray<ServerNodeMethod> methods)
#else
        internal static string GetMethod(Type type, ref LeftArray<ServerNodeMethod> methods)
#endif
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                var error = AutoCSer.Net.CommandServer.InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                ServerMethodAttribute methodAttribute = method.GetCustomAttribute<ServerMethodAttribute>(false) ?? ServerMethodAttribute.Default;
                ServerNodeMethod serverMethod = new ServerNodeMethod(type, method, methodAttribute);
                if (serverMethod.CallType == CallTypeEnum.Unknown) return serverMethod.Error ?? $"{type.fullName()}.{method.Name} 未知节点方法调用类型";
                methods.Add(serverMethod);
            }
            return null;
        }

        /// <summary>
        /// 服务端接口方法排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(ServerNodeMethod left, ServerNodeMethod right)
        {
            return AutoCSer.Net.CommandServer.InterfaceMethodBase.MethodCompare(left, right);
        }
    }
}
