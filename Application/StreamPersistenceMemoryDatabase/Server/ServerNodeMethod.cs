﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点方法信息
    /// </summary>
    internal sealed class ServerNodeMethod : NodeMethod
    {
        /// <summary>
        /// 持久化之前检查参数方法的名称后缀
        /// </summary>
        internal const string BeforePersistenceMethodNameSuffix = "BeforePersistence";
        /// <summary>
        /// 创建节点方法编号
        /// </summary>
        private static int createMethodIndex;

        /// <summary>
        /// 节点方法自定义属性
        /// </summary>
        internal readonly ServerMethodAttribute MethodAttribute;
        /// <summary>
        /// 修复方法集合
        /// </summary>
        private Dictionary<HashObject<MethodInfo>, Method> repairMethods;
        /// <summary>
        /// 持久化方法名称
        /// </summary>
        internal SubString PersistenceMethodName;
        /// <summary>
        /// 持久化方法返回数据类型
        /// </summary>
        private readonly Type persistenceMethodReturnType;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        internal MethodInfo RepairNodeMethod;
        /// <summary>
        /// 持久化之前参数检查方法编号
        /// </summary>
        private int beforePersistenceMethodIndex = int.MinValue;
        /// <summary>
        /// 默认为 true 表示调用需要持久化，如果调用不涉及数据变更操作则应该手动设置为 false 避免垃圾数据被持久化
        /// </summary>
        internal bool IsPersistence;
        /// <summary>
        /// 默认为 true 表示允许客户端调用，否则为服务端内存调用方法
        /// </summary>
        internal bool IsClientCall;
        /// <summary>
        /// 是否当前接口定义方法
        /// </summary>
        internal readonly bool IsDeclaringMethod;
        /// <summary>
        /// 服务端节点方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="methodAttribute"></param>
        /// <param name="isDeclaringMethod"></param>
        internal unsafe ServerNodeMethod(Type type, MethodInfo method, ServerMethodAttribute methodAttribute, bool isDeclaringMethod) : base(type, method)
        {
            MethodAttribute = methodAttribute;
            MethodIndex = MethodAttribute.MethodIndex;
            IsClientCall = MethodAttribute.IsClientCall;
            IsPersistence = MethodAttribute.IsPersistence;
            IsDeclaringMethod = isDeclaringMethod;

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
            if (Method.Name.EndsWith(BeforePersistenceMethodNameSuffix, StringComparison.Ordinal))
            {
                switch (CallType)
                {
                    case CallTypeEnum.CallOutput:
                    case CallTypeEnum.CallInputOutput:
                        PersistenceMethodName.Set(Method.Name, 0, Method.Name.Length - BeforePersistenceMethodNameSuffix.Length);
                        if (method.ReturnType == typeof(bool))
                        {
                            persistenceMethodReturnType = typeof(void);
                            break;
                        }
                        else if (method.ReturnType.GetGenericTypeDefinition() == typeof(ValueResult<>))
                        {
                            persistenceMethodReturnType = method.ReturnType.GetGenericArguments()[0];
                            break;
                        }
                        SetError(CallStateEnum.BeforePersistenceMethodReturnTypeError, $"{type.fullName()} 持久化检查方法 {Method.Name} 返回值类型必须为 {typeof(bool).fullName()} 或者 {typeof(ValueResult<>).fullName()}");
                        return;
                    default:
                        SetError(CallStateEnum.BeforePersistenceMethodCallTypeError, $"{type.fullName()} 持久化检查方法 {Method.Name} 调用类型不匹配 {CallType}");
                        return;
                }
            }
            if (AutoCSer.Common.IsCodeGenerator) return;
            if (ParameterStartIndex != ParameterEndIndex)
            {
                InputParameterType = AutoCSer.Net.CommandServer.ServerMethodParameter.GetOrCreate(ParameterCount, InputParameters, typeof(void));
                InputParameterFields = InputParameterType.GetFields(InputParameters);
                IsSimpleDeserializeParamter = InputParameterType.IsSimpleSerialize;
            }
            if (PersistenceMethodName.Length == 0)
            {
                if (ReturnValueType != typeof(void)) IsSimpleSerializeParamter = SimpleSerialize.Serializer.IsType(ReturnValueType);
            }
            else
            {
                if (persistenceMethodReturnType != typeof(void)) IsSimpleSerializeParamter = SimpleSerialize.Serializer.IsType(persistenceMethodReturnType);
            }
        }
        /// <summary>
        /// 持久化检查方法匹配
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        internal bool CheckBeforePersistence(ServerNodeMethod beforePersistenceMethod, ref string error)
        {
            if (IsPersistence && beforePersistenceMethod.PersistenceMethodName.Equals(Method.Name))
            {
                if (InputParameterType == beforePersistenceMethod.InputParameterType)
                {
                    if (ReturnValueType == beforePersistenceMethod.persistenceMethodReturnType)
                    {
                        beforePersistenceMethodIndex = beforePersistenceMethod.setIsPersistenceMethod();
                        return true;
                    }
                    error = $"{type.fullName()} 持久化检查方法 {beforePersistenceMethod.Method.Name} 返回值类型不匹配 {typeof(ValueResult<>).MakeGenericType(ReturnValueType)}";
                }
                else error = $"{type.fullName()} 持久化检查方法 {beforePersistenceMethod.Method.Name} 与 {Method.Name} 输入参数不匹配";
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
        /// <summary>
        /// 方法调用传参
        /// </summary>
        /// <param name="doCommandGenerator"></param>
        /// <param name="inputParameterLocalBuilder"></param>
        internal void CallMethodParameter(ILGenerator doCommandGenerator, LocalBuilder inputParameterLocalBuilder)
        {
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                doCommandGenerator.Emit(OpCodes.Ldloc_S, inputParameterLocalBuilder);
                doCommandGenerator.Emit(OpCodes.Ldfld, InputParameterType.GetField(parameter.Name));
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
        internal Method CreateMethod<T>(MethodInfo repairMethod)
        {
            if (repairMethod == null) return createMethod<T>(null);
            HashObject<MethodInfo> hashKey = repairMethod;
            if (repairMethods == null) repairMethods = DictionaryCreator<HashObject<MethodInfo>>.Create<Method>();
            Method method;
            if (!repairMethods.TryGetValue(hashKey, out method)) repairMethods.Add(hashKey, method = createMethod<T>(repairMethod));
            return method;
        }
        /// <summary>
        /// 创建服务端节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repairMethod">修复方法信息</param>
        /// <returns></returns>
        private Method createMethod<T>(MethodInfo repairMethod)
        {
            Type parentType = null, callMethodReturnType = typeof(void);
            MethodInfo callMethod = null, getParameterMethod = null;
            Type[] callMethodParameterTypes = null;
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
                    if (PersistenceMethodName.Length == 0)
                    {
                        callMethod = ServerNodeCreator.CallOutputMethod;
                        callMethodParameterTypes = ServerNodeCreator.CallOutputMethodParameterTypes;
                    }
                    else
                    {
                        if (persistenceMethodReturnType == typeof(void))
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
                    parentType = typeof(CallInputMethod<>).MakeGenericType(InputParameterType.Type);
                    callMethod = ServerNodeCreator.CallInputMethod;
                    callMethodParameterTypes = ServerNodeCreator.CallInputMethodParameterTypes;
                    getParameterMethod = typeof(CallInputMethodParameter<>).MakeGenericType(InputParameterType.Type).GetMethod(nameof(CallInputMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.CallInputOutput:
                case CallTypeEnum.InputCallback:
                    parentType = typeof(CallInputOutputMethod<>).MakeGenericType(InputParameterType.Type);
                    if (PersistenceMethodName.Length == 0) callMethod = ServerNodeCreator.CallInputOutputMethod;
                    else if (persistenceMethodReturnType == typeof(void))
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
                    getParameterMethod = typeof(CallInputOutputMethodParameter<>).MakeGenericType(InputParameterType.Type).GetMethod(nameof(CallInputOutputMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.SendOnly:
                    parentType = typeof(SendOnlyMethod<>).MakeGenericType(InputParameterType.Type);
                    callMethod = ServerNodeCreator.SendOnlyMethod;
                    callMethodParameterTypes = ServerNodeCreator.SendOnlyMethodParameterTypes;
                    getParameterMethod = typeof(SendOnlyMethodParameter<>).MakeGenericType(InputParameterType.Type).GetMethod(nameof(SendOnlyMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
                case CallTypeEnum.KeepCallback:
                case CallTypeEnum.Enumerable:
                    parentType = typeof(KeepCallbackMethod);
                    callMethod = ServerNodeCreator.KeepCallbackMethod;
                    callMethodParameterTypes = ServerNodeCreator.KeepCallbackMethodParameterTypes;
                    break;
                case CallTypeEnum.InputKeepCallback:
                case CallTypeEnum.InputEnumerable:
                    parentType = typeof(InputKeepCallbackMethod<>).MakeGenericType(InputParameterType.Type);
                    callMethod = ServerNodeCreator.InputKeepCallbackMethod;
                    callMethodParameterTypes = ServerNodeCreator.InputKeepCallbackMethodParameterTypes;
                    getParameterMethod = typeof(InputKeepCallbackMethodParameter<>).MakeGenericType(InputParameterType.Type).GetMethod(nameof(InputKeepCallbackMethodParameter<int>.GetParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    break;
            }
            string methodName = AutoCSer.Common.NamePrefix + ".CommandService.StreamPersistenceMemoryDatabase." + type.FullName + "." + Method.Name + "." + MethodIndex.toString() + "." + Interlocked.Increment(ref createMethodIndex).toString();
            if (repairMethod != null) methodName += ".Repair." + repairMethod.Name;
            TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(methodName, TypeAttributes.Class | TypeAttributes.Sealed, parentType);
            #region 构造函数
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, EmptyArray<Type>.Array);
            ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
            #region base(index, flags)
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.int32(MethodIndex);
            constructorGenerator.int32(beforePersistenceMethodIndex);
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
            MethodFlagsEnum flags = MethodFlagsEnum.None;
            if (IsPersistence) flags |= MethodFlagsEnum.IsPersistence;
            if (IsClientCall) flags |= MethodFlagsEnum.IsClientCall;
            if (IsSimpleSerializeParamter) flags |= MethodFlagsEnum.IsSimpleSerializeParamter;
            if (IsSimpleDeserializeParamter) flags |= MethodFlagsEnum.IsSimpleDeserializeParamter;
            if (MethodAttribute.IsIgnorePersistenceCallbackException) flags |= MethodFlagsEnum.IsIgnorePersistenceCallbackException;
            constructorGenerator.int32((byte)flags);
            if (isCallType)
            {
                constructorGenerator.Emit(OpCodes.Call, parentType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerNodeCreator.CallTypeMethodConstructorParameterTypes, null));
            }
            else
            {
                constructorGenerator.Emit(OpCodes.Call, parentType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerNodeCreator.MethodConstructorParameterTypes, null));
            }
            #endregion
            constructorGenerator.Emit(OpCodes.Ret);
            #endregion
            #region public override void Call(Node node, ref CommandServerCallback<CallStateEnum> callback)
            MethodBuilder callMethodBuilder = typeBuilder.DefineMethod(callMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, callMethodReturnType, callMethodParameterTypes);
            ILGenerator callMethodGenerator = callMethodBuilder.GetILGenerator();
            #region p0 parameter = CallInputMethodParameter<p0>.GetParameter(((CallInputMethodParameter<p0>)methodParameter));
            LocalBuilder inputParameterLocalBuilder = null;
            if (InputParameterType != null)
            {
                inputParameterLocalBuilder = callMethodGenerator.DeclareLocal(InputParameterType.Type);
                callMethodGenerator.ldarg(1);
                callMethodGenerator.call(getParameterMethod);
                callMethodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
            }
            #endregion
            switch (CallType)
            {
                case CallTypeEnum.CallInputOutput:
                case CallTypeEnum.InputEnumerable:
                    if (PersistenceMethodName.Length == 0 || persistenceMethodReturnType != typeof(void)) callMethodGenerator.ldarg(1);
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
                    callMethodGenerator.call(ServerNodeCreator.MethodParameterGetNode.Method);
                    break;
            }
            callMethodGenerator.call(((Func<ServerNode<T>, T>)ServerNode<T>.GetTarget).Method);
            CallMethodParameter(callMethodGenerator, inputParameterLocalBuilder);
            switch (CallType)
            {
                case CallTypeEnum.Callback:
                    #region MethodCallback<T>.Create(ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32(IsSimpleSerializeParamter);
                    callMethodGenerator.call(GenericType.Get(ReturnValueType).CreateMethodCallbackDelegate.Method);
                    #endregion
                    break;
                case CallTypeEnum.KeepCallback:
                    #region MethodCallback<T>.Create(ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32(IsSimpleSerializeParamter);
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
                    if (PersistenceMethodName.Length == 0)
                    {
                        #region CallOutputMethod.Callback(X, ref callback, isSimpleSerialize);
                        callMethodGenerator.ldarg(2);
                        callMethodGenerator.int32(IsSimpleSerializeParamter);
                        callMethodGenerator.call(ServerNodeCreator.CallOutputMethodCallbackMethod.MakeGenericMethod(ReturnValueType));
                        #endregion
                    }
                    else if (persistenceMethodReturnType != typeof(void))
                    {
                        #region CallOutputMethod.GetBeforePersistenceResponseParameter(X, isSimpleSerialize);
                        callMethodGenerator.int32(IsSimpleSerializeParamter);
                        callMethodGenerator.call(ServerNodeCreator.CallOutputMethodGetBeforePersistenceResponseParameterMethod.MakeGenericMethod(persistenceMethodReturnType));
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
                    if (PersistenceMethodName.Length == 0)
                    {
                        #region CallInputOutputMethodParameter.Callback(methodParameter, X);
                        callMethodGenerator.call(ServerNodeCreator.CallInputOutputMethodParameterCallbackMethod.MakeGenericMethod(ReturnValueType));
                        #endregion
                    }
                    else if (persistenceMethodReturnType != typeof(void))
                    {
                        #region CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter(methodParameter, X);
                        callMethodGenerator.call(ServerNodeCreator.CallInputOutputMethodParameterGetBeforePersistenceResponseParameterMethod.MakeGenericMethod(persistenceMethodReturnType));
                        #endregion
                    }
                    break;
                case CallTypeEnum.Enumerable:
                    #region KeepCallbackMethod.EnumerableCallback(X, ref callback, false)
                    callMethodGenerator.ldarg(2);
                    callMethodGenerator.int32(IsSimpleSerializeParamter);
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
            return (Method)typeBuilder.CreateType().GetConstructor(EmptyArray<Type>.Array).Invoke(null);
        }

        /// <summary>
        /// 获取服务端接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methods"></param>
        /// <param name="isDeclaringMethod"></param>
        /// <returns>错误信息</returns>
        internal static string GetMethod(Type type, ref LeftArray<ServerNodeMethod> methods, bool isDeclaringMethod)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                string error = AutoCSer.Net.CommandServer.InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                ServerMethodAttribute methodAttribute = (ServerMethodAttribute)method.GetCustomAttribute(typeof(ServerMethodAttribute), false) ?? ServerMethodAttribute.Default;
                ServerNodeMethod serverMethod = new ServerNodeMethod(type, method, methodAttribute, isDeclaringMethod);
                if (serverMethod.CallType == CallTypeEnum.Unknown) return serverMethod.Error ?? $"{type.fullName()}.{method.Name} 未知节点方法调用类型";
                methods.Add(serverMethod);
            }
            return null;
        }
    }
}
