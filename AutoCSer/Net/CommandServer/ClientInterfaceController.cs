﻿using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
#if !AOT
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    internal static class ClientInterfaceController
    {
        /// <summary>
        /// 命令客户端控制器构造函数信息
        /// </summary>
        internal static readonly ConstructorInfo CommandControllerConstructorInfo = typeof(CommandClientController).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandClientSocket), typeof(string), typeof(int), typeof(ClientInterfaceMethod[]), typeof(int[]), typeof(int) }, null).notNull();
        /// <summary>
        /// 同步等待命令
        /// </summary>
        internal static readonly Func<CommandClientController, int, CommandClientReturnValue> CommandClientControllerSynchronous = CommandClientController.Synchronous;
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        internal static readonly Func<CommandClientController, int, SendOnlyCommand> CommandClientControllerSendOnly = CommandClientController.SendOnly;
        /// <summary>
        /// 回调委托
        /// </summary>
        internal static readonly Func<CommandClientController, int, CommandClientCallback, AutoCSer.Net.CallbackCommand> CommandClientControllerCallback = CommandClientController.Callback;
        /// <summary>
        /// 保持回调委托
        /// </summary>
        internal static readonly Func<CommandClientController, int, CommandClientKeepCallback, AutoCSer.Net.KeepCallbackCommand> CommandClientControllerKeepCallback = CommandClientController.KeepCallback;
        /// <summary>
        /// 队列回调委托
        /// </summary>
        internal static readonly Func<CommandClientController, int, CommandClientCallbackQueueNode, AutoCSer.Net.CallbackCommand> CommandClientControllerCallbackQueue = CommandClientController.CallbackQueue;
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        internal static readonly Func<CommandClientController, int, CommandClientKeepCallbackQueue, AutoCSer.Net.KeepCallbackCommand> CommandClientControllerKeepCallbackQueue = CommandClientController.KeepCallbackQueue;
        /// <summary>
        /// 返回值
        /// </summary>
        internal static readonly Func<CommandClientController, int, AutoCSer.Net.ReturnCommand> CommandClientControllerReturnType = CommandClientController.ReturnType;
        /// <summary>
        /// 获取 Task
        /// </summary>
        internal static readonly Func<ReturnCommand, Task> ReturnCommandGetTask = AutoCSer.Net.ReturnCommand.GetTask;
        /// <summary>
        /// 队列返回值
        /// </summary>
        internal static readonly Func<CommandClientController, int, AutoCSer.Net.ReturnQueueCommand> CommandClientControllerReturnTypeQueue = CommandClientController.ReturnTypeQueue;
        /// <summary>
        /// 枚举返回值
        /// </summary>
        internal static readonly Func<CommandClientController, int, AutoCSer.Net.EnumeratorCommand> CommandClientControllerEnumerator = CommandClientController.Enumerator;
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        internal static readonly Func<CommandClientController, int, AutoCSer.Net.EnumeratorQueueCommand> CommandClientControllerEnumeratorQueue = CommandClientController.EnumeratorQueue;
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal static readonly Func<Action<CommandClientReturnValue>, CommandClientCallback> GetCommandClientCallback = CommandClientCallback.Get;
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal static readonly Func<Action<CommandClientReturnValue, Net.KeepCallbackCommand>, CommandClientKeepCallback> GetCommandClientKeepCallback = CommandClientKeepCallback.Get;
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal static readonly Func<Action<CommandClientReturnValue, CommandClientCallQueue>, CommandClientCallbackQueueNode> GetCommandClientCallbackQueue = CommandClientCallbackQueueNode.Get;
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal static readonly Func<Action<CommandClientReturnValue, CommandClientCallQueue, Net.KeepCallbackCommand>, CommandClientKeepCallbackQueue> GetCommandClientKeepCallbackQueue = CommandClientKeepCallbackQueue.Get;
        /// <summary>
        /// 是否成功
        /// </summary>
        internal static readonly Func<CommandClientReturnValue, bool> CommandClientReturnValueGetIsSuccess = CommandClientReturnValue.GetIsSuccess;
        /// <summary>
        /// 检查状态并抛出异常
        /// </summary>
        internal static readonly Action<CommandClientReturnValue> CommandClientReturnValueCheckThrowException = CommandClientReturnValue.CheckThrowException;
        /// <summary>
        /// 抛出异常
        /// </summary>
        internal static readonly Action<string> ClientInterfaceMethodThrowException = ClientInterfaceMethod.ThrowException;

        /// <summary>
        /// 同步等待命令方法
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerSynchronousInputMethod;
        /// <summary>
        /// 同步等待命令方法
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerSynchronousOutputMethod;
        /// <summary>
        /// 同步等待命令方法
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerSynchronousInputOutputMethod;
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerSendOnlyInputMethod;
        /// <summary>
        /// 回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackInputMethod;
        /// <summary>
        /// 回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackOutputMethod;
        /// <summary>
        /// 回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackOutputReturnValueMethod;
        /// <summary>
        /// 保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackInputMethod;
        /// <summary>
        /// 保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackOutputMethod;
        /// <summary>
        /// 保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackOutputReturnValueMethod;
        /// <summary>
        /// 队列回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackQueueInputMethod;
        /// <summary>
        /// 队列回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackQueueOutputMethod;
        /// <summary>
        /// 队列回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerCallbackQueueOutputReturnValueMethod;
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackQueueInputMethod;
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackQueueOutputMethod;
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerKeepCallbackQueueOutputReturnValueMethod;
        /// <summary>
        /// 返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnTypeInputMethod;
        /// <summary>
        /// 返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnValueOutputMethod;
        /// <summary>
        /// 返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnValueOutputReturnValueMethod;
        /// <summary>
        /// 队列返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnTypeQueueInputMethod;
        /// <summary>
        /// 队列返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnValueQueueOutputMethod;
        /// <summary>
        /// 队列返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerReturnValueQueueOutputReturnValueMethod;
        /// <summary>
        /// 枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorInputMethod;
        /// <summary>
        /// 枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorOutputMethod;
        /// <summary>
        /// 枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorOutputReturnValueMethod;
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorQueueInputMethod;
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorQueueOutputMethod;
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        internal static readonly MethodInfo CommandClientControllerEnumeratorQueueOutputReturnValueMethod;

#pragma warning disable CS8618
        static ClientInterfaceController()
#pragma warning restore CS8618
        {
            foreach (MethodInfo method in typeof(CommandClientController).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                switch (method.Name.Length - 13)
                {
                    case 13 - 13:
                        if (method.Name == nameof(CommandClientController.SendOnlyInput)) CommandClientControllerSendOnlyInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.CallbackInput)) CommandClientControllerCallbackInputMethod = method;
                        break;
                    case 14 - 13:
                        if (method.Name == nameof(CommandClientController.CallbackOutput)) CommandClientControllerCallbackOutputMethod = method;
                        break;
                    case 15 - 13:
                        if (method.Name == nameof(CommandClientController.ReturnTypeInput)) CommandClientControllerReturnTypeInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.EnumeratorInput)) CommandClientControllerEnumeratorInputMethod = method;
                        break;
                    case 16 - 13:
                        if (method.Name == nameof(CommandClientController.SynchronousInput)) CommandClientControllerSynchronousInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.EnumeratorOutput)) CommandClientControllerEnumeratorOutputMethod = method;
                        break;
                    case 17 - 13:
                        if (method.Name == nameof(CommandClientController.SynchronousOutput)) CommandClientControllerSynchronousOutputMethod = method;
                        else if (method.Name == nameof(CommandClientController.KeepCallbackInput)) CommandClientControllerKeepCallbackInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.ReturnValueOutput)) CommandClientControllerReturnValueOutputMethod = method;
                        break;
                    case 18 - 13:
                        if (method.Name == nameof(CommandClientController.KeepCallbackOutput)) CommandClientControllerKeepCallbackOutputMethod = method;
                        else if (method.Name == nameof(CommandClientController.CallbackQueueInput)) CommandClientControllerCallbackQueueInputMethod = method;
                        break;
                    case 19 - 13:
                        if (method.Name == nameof(CommandClientController.CallbackQueueOutput)) CommandClientControllerCallbackQueueOutputMethod = method;
                        break;
                    case 20 - 13:
                        if (method.Name == nameof(CommandClientController.ReturnTypeQueueInput)) CommandClientControllerReturnTypeQueueInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.EnumeratorQueueInput)) CommandClientControllerEnumeratorQueueInputMethod = method;
                        break;
                    case 21 - 13:
                        if (method.Name == nameof(CommandClientController.EnumeratorQueueOutput)) CommandClientControllerEnumeratorQueueOutputMethod = method;
                        break;
                    case 22 - 13:
                        if (method.Name == nameof(CommandClientController.SynchronousInputOutput)) CommandClientControllerSynchronousInputOutputMethod = method;
                        else if (method.Name == nameof(CommandClientController.KeepCallbackQueueInput)) CommandClientControllerKeepCallbackQueueInputMethod = method;
                        else if (method.Name == nameof(CommandClientController.ReturnValueQueueOutput)) CommandClientControllerReturnValueQueueOutputMethod = method;
                        break;
                    case 23 - 13:
                        if (method.Name == nameof(CommandClientController.KeepCallbackQueueOutput)) CommandClientControllerKeepCallbackQueueOutputMethod = method;
                        break;
                    case 25 - 13:
                        if (method.Name == nameof(CommandClientController.CallbackOutputReturnValue)) CommandClientControllerCallbackOutputReturnValueMethod = method;
                        break;
                    case 27 - 13:
                        if (method.Name == nameof(CommandClientController.EnumeratorOutputReturnValue)) CommandClientControllerEnumeratorOutputReturnValueMethod = method;
                        break;
                    case 28 - 13:
                        if (method.Name == nameof(CommandClientController.ReturnValueOutputReturnValue)) CommandClientControllerReturnValueOutputReturnValueMethod = method;
                        break;
                    case 29 - 13:
                        if (method.Name == nameof(CommandClientController.KeepCallbackOutputReturnValue)) CommandClientControllerKeepCallbackOutputReturnValueMethod = method;
                        break;
                    case 30 - 13:
                        if (method.Name == nameof(CommandClientController.CallbackQueueOutputReturnValue)) CommandClientControllerCallbackQueueOutputReturnValueMethod = method;
                        break;
                    case 32 - 13:
                        if (method.Name == nameof(CommandClientController.EnumeratorQueueOutputReturnValue)) CommandClientControllerEnumeratorQueueOutputReturnValueMethod = method;
                        break;
                    case 33 - 13:
                        if (method.Name == nameof(CommandClientController.ReturnValueQueueOutputReturnValue)) CommandClientControllerReturnValueQueueOutputReturnValueMethod = method;
                        break;
                    case 34 - 13:
                        if (method.Name == nameof(CommandClientController.KeepCallbackQueueOutputReturnValue)) CommandClientControllerKeepCallbackQueueOutputReturnValueMethod = method;
                        break;
                }
            }
        }
    }
#endif
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    /// <typeparam name="T">客户端接口类型</typeparam>
    /// <typeparam name="ST">服务端接口类型</typeparam>
    internal static class ClientInterfaceController<T, ST>
    {
        /// <summary>
        /// 创建命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandClientController Create(CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
#else
        internal static CommandClientController Create(CommandClientSocket socket, string controllerName, int startMethodIndex, string[] serverMethodNames)
#endif
        {
            if (ControllerConstructorException == null)
            {
#if AOT
                CommandClientController controller = callConstructor(socket, controllerName, startMethodIndex, serverMethodNames);
#else
                CommandClientController controller = callConstructor(socket, controllerName, startMethodIndex, Methods, ClientInterfaceMethod.GetServerMethodIndexs(startMethodIndex, Methods, serverMethodNames));
#endif
                if (controllerConstructorMessages == null) return controller;
                socket.Client.Config.OnControllerConstructorMessage(typeof(T), controllerConstructorMessages);
                return controller;
            }
            throw ControllerConstructorException;
        }
#if AOT
        /// <summary>
        /// 客户端接口方法信息集合
        /// </summary>
        internal static ClientMethod[] Methods;
        /// <summary>
        /// 控制器构造函数
        /// </summary>
        private static readonly Func<CommandClientSocket, string, int, string?[]?, CommandClientController> callConstructor;
#else
        /// <summary>
        /// 客户端接口方法信息集合
        /// </summary>
        internal static ClientInterfaceMethod[] Methods;
        /// <summary>
        /// 控制器构造函数
        /// </summary>
#if NetStandard21
        private static readonly Func<CommandClientSocket, string, int, ClientInterfaceMethod?[], int[], CommandClientController> callConstructor;
#else
        private static readonly Func<CommandClientSocket, string, int, ClientInterfaceMethod[], int[], CommandClientController> callConstructor;
#endif
#endif
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        internal static readonly Exception? ControllerConstructorException;
#else
        internal static readonly Exception ControllerConstructorException;
#endif
        /// <summary>
        /// 控制器构造提示信息
        /// </summary>
#if NetStandard21
        private static readonly string[]? controllerConstructorMessages;
#else
        private static readonly string[] controllerConstructorMessages;
#endif
        /// <summary>
        /// 检查客户端控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<string> Check()
        {
            if (ControllerConstructorException != null) yield return ControllerConstructorException.Message;
            if (controllerConstructorMessages != null)
            {
                foreach (string message in controllerConstructorMessages) yield return message;
            }
        }
        static ClientInterfaceController()
        {
            Type type = typeof(T), serverType = typeof(ST);
#if AOT
            Methods = EmptyArray<ClientMethod>.Array;
            callConstructor = NullCommandClientController.Get;
#else
#if NetStandard21
            Methods = EmptyArray<ClientInterfaceMethod>.Array;
            callConstructor = NullCommandClientController.Get;
#endif
#endif
            try
            {
#if AOT
                var attribute = type.GetCustomAttribute<CommandClientControllerTypeAttribute>();
                if (attribute?.ClientType != null )
                {
                    var method = attribute.ClientType.GetMethod(AutoCSer.CodeGenerator.CommandClientControllerAttribute.CommandClientControllerConstructorMethodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, new Type[] { typeof(CommandClientSocket), typeof(string), typeof(int), typeof(string?[]) });
                    if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(AutoCSer.Net.CommandClientController))
                    {
                        var methods = attribute.ClientType.GetMethod(AutoCSer.CodeGenerator.CommandClientControllerAttribute.CommandClientControllerMethodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, EmptyArray<Type>.Array);
                        if (methods != null && !methods.IsGenericMethod && methods.ReturnType == typeof(LeftArray<AutoCSer.Net.CommandServer.ClientMethod>))
                        {
                            callConstructor = (Func<CommandClientSocket, string, int, string?[]?, CommandClientController>)method.CreateDelegate(typeof(Func<CommandClientSocket, string, int, string?[]?, CommandClientController>));
                            Methods = methods.Invoke(null, null).castValue<LeftArray<AutoCSer.Net.CommandServer.ClientMethod>>().ToArray();
                            return;
                        }
                        throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.CommandClientControllerAttribute.CommandClientControllerMethodName);
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.CommandClientControllerAttribute.CommandClientControllerConstructorMethodName);
                }
                throw new MissingMemberException(type.fullName(), typeof(CommandClientControllerTypeAttribute).fullName());
#else
                ServerInterface serverInterface = new ServerInterface(serverType, null, type);
                LeftArray<ClientInterfaceMethod> methodArray;
                if (!serverInterface.GetClientMethods(type, null, ref ControllerConstructorException, ref controllerConstructorMessages, out methodArray)) return;
                Methods = methodArray.ToArray();

                Type[] constructorParameterTypes = new Type[] { typeof(CommandClientSocket), typeof(string), typeof(int), typeof(ClientInterfaceMethod[]), typeof(int[]) };
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandClient.InterfaceController." + type.FullName + "." + serverType.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(CommandClientController), new Type[] { type });
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(socket, controllerName, startMethodIndex, maxMethodCount, ClientInterfaceController<IClientInterfaceControllerIL, IClientInterfaceControllerIL>.GetMethods())
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.Emit(OpCodes.Ldarg, 4);
                constructorGenerator.Emit(OpCodes.Ldarg, 5);
                //#if NetStandard21
                //                constructorGenerator.call(((Func<ClientInterfaceMethod?[]>)GetMethods).Method);
                //#else
                //                constructorGenerator.call(((Func<ClientInterfaceMethod[]>)GetMethods).Method);
                //#endif
                constructorGenerator.int32(serverInterface.VerifyMethodIndex);
                constructorGenerator.Emit(OpCodes.Call, ClientInterfaceController.CommandControllerConstructorInfo);
                constructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #endregion

                int methodIndex = 0;
                foreach (ClientInterfaceMethod method in Methods)
                {
                    ParameterInfo[] parameters = method.Method.GetParameters();
                    MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Method.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, method.Method.ReturnType, parameters.getArray(parameter => parameter.ParameterType));
                    typeBuilder.DefineMethodOverride(methodBuilder, method.Method);
                    ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                    if (method.Error == null)
                    {
                        #region SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
                        var newInputParameterLocalBuilder = default(LocalBuilder);
                        var inputParameterLocalBuilder = method.GetInputParameterLocalBuilder(methodGenerator, out newInputParameterLocalBuilder);
                        if (method.InputParameterType != null)
                        {
                            method.SetInputParameter(methodGenerator, newInputParameterLocalBuilder.notNull());
                            methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder.notNull());
                            methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder.notNull());
                        }
                        #endregion
                        #region SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                        var returnValueGenericType = default(GenericType);
                        var outputParameterLocalBuilder = method.GetOutputParameterLocalBuilder(methodGenerator, out returnValueGenericType);
                        #endregion
                        methodGenerator.Emit(OpCodes.Ldarg_0);
                        methodGenerator.int32(methodIndex);
                        method.CallbackParameter(methodGenerator, returnValueGenericType);
                        if (method.InputParameterType != null) methodGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder.notNull());
                        if (outputParameterLocalBuilder != null) methodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder);
                        method.CallController(methodGenerator, returnValueGenericType, outputParameterLocalBuilder);
                    }
                    else
                    {
                        methodGenerator.ldstr(method.Error);
                        methodGenerator.call(ClientInterfaceController.ClientInterfaceMethodThrowException.Method);
                        if (method.Method.ReturnType != typeof(void)) methodGenerator.Emit(OpCodes.Ldnull);
                    }
                    methodGenerator.Emit(OpCodes.Ret);
                    ++methodIndex;
                }
                Type controllerType = typeBuilder.CreateType();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", typeof(CommandClientController), constructorParameterTypes, controllerType, true);
                ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                callConstructorGenerator.Emit(OpCodes.Ldarg_1);
                callConstructorGenerator.Emit(OpCodes.Ldarg_2);
                callConstructorGenerator.Emit(OpCodes.Ldarg_3);
                callConstructorGenerator.Emit(OpCodes.Ldarg, 4);
                callConstructorGenerator.Emit(OpCodes.Newobj, controllerType.GetConstructor(constructorParameterTypes).notNull());
                callConstructorGenerator.Emit(OpCodes.Ret);
#if NetStandard21
                callConstructor = (Func<CommandClientSocket, string, int, ClientInterfaceMethod?[], int[], CommandClientController>)dynamicMethod.CreateDelegate(typeof(Func<CommandClientSocket, string, int, ClientInterfaceMethod?[], int[], CommandClientController>));
#else
                callConstructor = (Func<CommandClientSocket, string, int, ClientInterfaceMethod[], int[], CommandClientController>)dynamicMethod.CreateDelegate(typeof(Func<CommandClientSocket, string, int, ClientInterfaceMethod[], int[], CommandClientController>));
#endif
#endif
            }
            catch (Exception exception)
            {
                ControllerConstructorException = new Exception($"{serverType.fullName()} 客户端控制器 {type.fullName()} 生成失败", exception);
            }
        }
    }
#if DEBUG && NetStandard21 && !AOT
    #region 控制器接口 IL 模板
    internal interface IClientInterfaceControllerIL
    {
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Synchronous))]
        void SynchronousSymmetry(int Value, ref int Ref);
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Synchronous))]
        string SynchronousSymmetry(int Value, ref int Ref, out int Out);
        CommandClientReturnValue Synchronous(int Value, ref int Ref);
        CommandClientReturnValue<string> Synchronous(int Value, ref int Ref, out int Out);
        AutoCSer.Net.SendOnlyCommand SendOnly(int Value, int Ref);

        AutoCSer.Net.CallbackCommand Callback(int Value, int Ref, CommandClientCallback<string> Callback);
        AutoCSer.Net.KeepCallbackCommand KeepCallback(int Value, int Ref, CommandClientKeepCallback<string> Callback);

        AutoCSer.Net.CallbackCommand CallbackQueue(int Value, int Ref, CommandClientCallbackQueueNode<string> Callback);
        AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(int Value, int Ref, CommandClientKeepCallbackQueue<string> Callback);

        AutoCSer.Net.ReturnCommand<string> ReturnValue(int Value, int Ref);
        Task<string> ReturnValueTask(int Value, int Ref);
        AutoCSer.Net.ReturnQueueCommand<string> ReturnValueQueue(int Value, int Ref);
        AutoCSer.Net.EnumeratorCommand<string> Enumerator(int Value, int Ref);
#if NetStandard21
        IAsyncEnumerable<string> AsyncEnumerable(int Value, int Ref);
#endif
        AutoCSer.Net.EnumeratorQueueCommand<string> EnumeratorQueue(int Value, int Ref);
    }

    internal sealed class ClientInterfaceControllerIL : CommandClientController, IClientInterfaceControllerIL
    {
        public struct SynchronousInputParameter
        {
            public int Value;
            public int Ref;
        }
        public struct SynchronousOutputParameter
        {
            public string __Return__;
            public int Ref;
            public int Out;
        }
        public struct VerifyMethodOutputParameter
        {
            public CommandServerVerifyStateEnum __Return__;
            public int Ref;
            public int Out;
        }
        internal ClientInterfaceControllerIL(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod[] methods, int[] serverMethodIndexs)
            : base(socket, controllerName, startMethodIndex, methods, serverMethodIndexs, int.MinValue)
        {
        }
        private static IClientInterfaceControllerIL CallConstructor(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod[] methods, int[] serverMethodIndexs)
        {
            return new ClientInterfaceControllerIL(socket, controllerName, startMethodIndex, methods, serverMethodIndexs);
        }

        void IClientInterfaceControllerIL.SynchronousSymmetry(int Value, ref int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue.CheckThrowException(this.SynchronousInput(0, ref inputParameter));
            Ref = outputParameter.Ref;
        }
        string IClientInterfaceControllerIL.SynchronousSymmetry(int Value, ref int Ref, out int Out)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue.CheckThrowException(this.SynchronousInputOutput(0, ref inputParameter, ref outputParameter));
            Ref = outputParameter.Ref;
            Out = outputParameter.Out;
            return outputParameter.__Return__;
        }
        CommandClientReturnValue IClientInterfaceControllerIL.Synchronous(int Value, ref int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            outputParameter.Ref = Value;
            CommandClientReturnValue returnValue = this.SynchronousInput(0, ref inputParameter);
            Ref = outputParameter.Ref;

            ServerReturnValue<int> serverReturnValue = new ServerReturnValue<int>();
            AutoCSer.Net.CommandServer.ServerReturnValue<int>.SetReturnValue(ref serverReturnValue, ref Value);

            return returnValue;
        }
        CommandClientReturnValue<string> IClientInterfaceControllerIL.Synchronous(int Value, ref int Ref, out int Out)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue returnValue = this.SynchronousInputOutput(0, ref inputParameter, ref outputParameter);
            Ref = outputParameter.Ref;
            Out = outputParameter.Out;
            if (CommandClientReturnValue.GetIsSuccess(returnValue)) return CommandClientReturnValue<string>.GetReturnValue(outputParameter.__Return__);
            return CommandClientReturnValue<string>.GetReturnValue(returnValue);
        }
        AutoCSer.Net.SendOnlyCommand IClientInterfaceControllerIL.SendOnly(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.SendOnlyInput(0, ref inputParameter);
        }
        AutoCSer.Net.CallbackCommand IClientInterfaceControllerIL.Callback(int Value, int Ref, CommandClientCallback<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.CallbackOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.KeepCallbackCommand IClientInterfaceControllerIL.KeepCallback(int Value, int Ref, CommandClientKeepCallback<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.KeepCallbackOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.CallbackCommand IClientInterfaceControllerIL.CallbackQueue(int Value, int Ref, CommandClientCallbackQueueNode<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.CallbackQueueOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.KeepCallbackCommand IClientInterfaceControllerIL.KeepCallbackQueue(int Value, int Ref, CommandClientKeepCallbackQueue<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.KeepCallbackQueueOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.ReturnCommand<string> IClientInterfaceControllerIL.ReturnValue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.ReturnValueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
        Task<string> IClientInterfaceControllerIL.ReturnValueTask(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return AutoCSer.Net.ReturnCommand<string>.GetTask(this.ReturnValueOutput<SynchronousInputParameter, string>(0, ref inputParameter));
        }
        AutoCSer.Net.ReturnQueueCommand<string> IClientInterfaceControllerIL.ReturnValueQueue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.ReturnValueQueueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
        AutoCSer.Net.EnumeratorCommand<string> IClientInterfaceControllerIL.Enumerator(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.EnumeratorOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
#if NetStandard21
        IAsyncEnumerable<string> IClientInterfaceControllerIL.AsyncEnumerable(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return AutoCSer.Net.EnumeratorCommand<string>.GetAsyncEnumerable(this.EnumeratorOutput<SynchronousInputParameter, string>(0, ref inputParameter));
        }
#endif
        AutoCSer.Net.EnumeratorQueueCommand<string> IClientInterfaceControllerIL.EnumeratorQueue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref };
            return this.EnumeratorQueueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
    }
    #endregion
#endif
}
