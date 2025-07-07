using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Task 队列控制器接口信息
    /// </summary>
    internal static class ServerTaskQueueInterfaceController
    {
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
#if NetStandard21
        internal static readonly Func<CommandServerCallTaskQueueNode, CommandServerTaskQueueService?> CommandServerCallTaskQueueNodeGetTaskQueue = CommandServerCallTaskQueueNode.GetTaskQueue;
#else
        internal static readonly Func<CommandServerCallTaskQueueNode, CommandServerTaskQueueService> CommandServerCallTaskQueueNodeGetTaskQueue = CommandServerCallTaskQueueNode.GetTaskQueue;
#endif
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
#if NetStandard21
        internal static readonly Func<CommandServerKeepCallbackQueueTask, CommandServerKeepCallback, CommandServerTaskQueueService?> CommandServerKeepCallbackQueueTaskGetTaskQueue = CommandServerKeepCallbackQueueTask.GetTaskQueue;
#else
        internal static readonly Func<CommandServerKeepCallbackQueueTask, CommandServerKeepCallback, CommandServerTaskQueueService> CommandServerKeepCallbackQueueTaskGetTaskQueue = CommandServerKeepCallbackQueueTask.GetTaskQueue;
#endif
    }
    /// <summary>
    /// Task 队列控制器接口信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    internal static class ServerTaskQueueInterfaceController<T, KT>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 创建命令服务控制器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controllerName"></param>
        /// <param name="getTaskQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerController Create(CommandListener server, string controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue)
        {
            if (controllerConstructorException == null)
            {
                CommandServerController commandServerController = (CommandServerController)controllerConstructorInfo.Invoke(new object[] { server, controllerName, getTaskQueue });
                if (controllerConstructorMessages == null) return commandServerController; 
                server.Config.OnControllerConstructorMessage(typeof(T), controllerConstructorMessages);
                return commandServerController;
            }
            throw controllerConstructorException;
        }
        /// <summary>
        /// 命令控制器配置
        /// </summary>
        private static CommandServerControllerInterfaceAttribute controllerAttribute;
        /// <summary>
        /// 获取命令控制器配置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerControllerInterfaceAttribute GetAttribute()
        {
            return controllerAttribute;
        }
        /// <summary>
        /// 服务端接口方法信息集合
        /// </summary>
#if NetStandard21
        private static readonly ServerInterfaceMethod?[] methods;
#else
        private static readonly ServerInterfaceMethod[] methods;
#endif
        /// <summary>
        /// 获取服务端接口方法信息集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static ServerInterfaceMethod?[] GetMethods()
#else
        internal static ServerInterfaceMethod[] GetMethods()
#endif
        {
            return methods;
        }
        /// <summary>
        /// 获取服务端接口方法信息集合
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal static ServerInterfaceMethod GetMethod(int methodIndex)
        {
            return methods[methodIndex].notNull();
        }
        /// <summary>
        /// 控制器构造函数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private static readonly ConstructorInfo controllerConstructorInfo;
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        private static readonly Exception? controllerConstructorException;
#else
        private static readonly Exception controllerConstructorException;
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
        /// 检查服务控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<string> Check()
        {
            if (controllerConstructorException != null) yield return controllerConstructorException.Message;
            if (controllerConstructorMessages != null)
            {
                foreach (string message in controllerConstructorMessages) yield return message;
            }
        }
        static ServerTaskQueueInterfaceController()
        {
            Type type = typeof(T), keyType = typeof(KT);
            var currentMethod = default(ServerInterfaceMethod);
            methods = EmptyArray<ServerInterfaceMethod>.Array;
            controllerAttribute = CommandServerController.DefaultAttribute;
            try
            {
                ServerInterface serverInterface = new ServerInterface(type, keyType);
                if (serverInterface.Error != null)
                {
                    controllerConstructorException = new Exception($"{type.fullName()} + {keyType.fullName()} 服务端控制器生成失败 {serverInterface.Error}");
                    return;
                }
                if (serverInterface.Messages.Length != 0) controllerConstructorMessages = serverInterface.Messages.ToArray();
                methods = serverInterface.Methods.notNull();
                controllerAttribute = serverInterface.ControllerAttribute;

                Type[] constructorParameterTypes = new Type[] { typeof(CommandListener), typeof(string), typeof(Func<CommandServerCallTaskQueueNode, KT, T>) };
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandServer.TaskQueueInterfaceController." + type.FullName + "." + keyType.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(CommandServerController));
                #region 静态构造函数
                ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null);
                ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();
                #region MethodX = ServerTaskQueueInterfaceController<T, KT>.GetMethod(X);
                int methodIndex = 0;
                MethodInfo getServerInterfaceMethod = ((Func<int, ServerInterfaceMethod>)GetMethod).Method;
                foreach (var method in methods)
                {
                    if (method != null && method.IsOutputInfo)
                    {
                        method.MethodFieldBuilder = typeBuilder.DefineField($"Method{methodIndex.toString()}", typeof(ServerInterfaceMethod), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly);
                        staticConstructorGenerator.int32(methodIndex);
                        staticConstructorGenerator.call(getServerInterfaceMethod);
                        staticConstructorGenerator.Emit(OpCodes.Stsfld, method.MethodFieldBuilder);
                    }
                    ++methodIndex;
                }
                #endregion
                staticConstructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(server, controllerName, commandCount, verifyMethodIndex)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.call(((Func<CommandServerControllerInterfaceAttribute>)GetAttribute).Method);
#if NetStandard21
                constructorGenerator.call(((Func<ServerInterfaceMethod?[]>)GetMethods).Method);
#else
                constructorGenerator.call(((Func<ServerInterfaceMethod[]>)GetMethods).Method);
#endif
                constructorGenerator.int32(serverInterface.VerifyMethodIndex);
                constructorGenerator.int32(0);
                constructorGenerator.int32(0);
                constructorGenerator.int32(0);
                constructorGenerator.Emit(OpCodes.Call, ServerInterfaceController.CommandControllerConstructorInfo);
                #endregion
                #region GetTaskQueue = getTaskQueue;
                FieldBuilder getTaskQueueFieldBuilder = typeBuilder.DefineField("GetTaskQueue", type, FieldAttributes.Public | FieldAttributes.InitOnly);
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.Emit(OpCodes.Stfld, getTaskQueueFieldBuilder);
                #endregion
                #region TaskQueueSet = CommandListener.GetServerCallTaskQueueSet<KT>(server);
                EquatableGenericType taskQueueKeyGenericType = EquatableGenericType.Get(keyType);
                FieldBuilder taskQueueSetFieldBuilder = typeBuilder.DefineField("TaskQueueSet", taskQueueKeyGenericType.ServerCallTaskQueueSetType, FieldAttributes.Public | FieldAttributes.InitOnly);
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.call(taskQueueKeyGenericType.CommandListenerGetServerCallTaskQueueSetDelegate.Method);
                constructorGenerator.Emit(OpCodes.Stfld, taskQueueSetFieldBuilder);
                #endregion
                constructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #region public override void DoCommand(CommandServerSocket socket, ref SubArray<byte> data)
                MethodBuilder doCommandMethodBuilder = typeBuilder.DefineMethod(nameof(CommandServerController.DoCommand), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(CommandClientReturnTypeEnum), ServerInterfaceController.DoCommandParameterTypes);
                ILGenerator doCommandGenerator = doCommandMethodBuilder.GetILGenerator();
                #region switch (CommandServerSocket.GetCommandMethodIndex(socket))
                Label doCommandReturnDeserializeErrorLabel;
                Label[] doCommandLabels = ServerInterfaceMethod.DoCommandSwitchMethodIndex(doCommandGenerator, methods, out doCommandReturnDeserializeErrorLabel);
                #endregion
                methodIndex = 0;
                CommandServerTaskQueueService.CreateTaskQueueDelegate<T, KT> commandServerTaskQueueCreateTaskQueue = CommandServerTaskQueueService.CreateTaskQueue<T, KT>;
                Func<CommandServerCallTaskQueueSet<KT>, KT, CommandServerCallTaskQueueNode, CommandClientReturnTypeEnum> commandServerSocketCallTaskQueueAppendLowPriority = CommandServerSocket.CallTaskQueueAppendLowPriority<KT>, commandServerSocketCallTaskQueueAppendQueue = CommandServerSocket.CallTaskQueueAppendQueue<KT>;
                foreach (var method in methods)
                {
                    if (method != null)
                    {
                        currentMethod = method;
                        doCommandGenerator.MarkLabel(doCommandLabels[methodIndex]);
                        if (method.MethodType == ServerMethodTypeEnum.VersionExpired)
                        {
                            #region return CommandServerReturnType.VersionExpired;
                            doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.VersionExpired);
                            doCommandGenerator.Emit(OpCodes.Ret);
                            #endregion
                        }
                        else
                        {
                            Label runTaskLabel = default(Label);
                            var inputParameterLocalBuilder = method.InputParameterDeserialize(doCommandGenerator, ref doCommandReturnDeserializeErrorLabel, ref runTaskLabel);
                            var asynchronousConstructorBuilder = default(ConstructorBuilder);
                            switch (method.MethodType)
                            {
                                case ServerMethodTypeEnum.CallbackTaskQueue:
                                    if (method.ReturnValueType == typeof(void)) goto TASKQUEUE;
                                    #region public sealed class AsynchronousCallback : ServerCallback<X>
                                    var returnGenericType = GenericType.Get(method.ReturnValueType);
                                    Type serverCallbackType = returnGenericType.GetCommandServerCallbackType(method);
                                    TypeBuilder asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, serverCallbackType);
                                    #endregion
                                    #region public AsynchronousCallback(CommandServerSocket socket) : base(socket) { }
                                    Type[] asynchronousConstructorParameterTypeArray = ServerInterfaceController.ServerCallTaskQueueParameterTypes;
                                    asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, asynchronousConstructorParameterTypeArray);
                                    ILGenerator asynchronousConstructorGenerator = asynchronousConstructorBuilder.GetILGenerator();
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Call, serverCallbackType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, asynchronousConstructorParameterTypeArray, null).notNull());
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    #region public override bool Callback(X returnValue)
                                    MethodBuilder asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallback.Callback), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), new Type[] { method.ReturnValueType });
                                    ILGenerator asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                    #endregion
                                    #region return CommandServerSocket.Callback<X>(this, InterfaceControllerIL<T>.OutputInfo0, returnValue);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_1);
                                    asynchronousMethodGenerator.call(methodIndex == serverInterface.VerifyMethodIndex ? ServerInterfaceController.ServerCallSendVerifyState.Method : returnGenericType.CommandServerCallbackDelegate.Method);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();
                                    goto TASKQUEUE;
                                case ServerMethodTypeEnum.TaskQueue:
                                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                    TASKQUEUE:
                                    #region public sealed class CallTaskQueue : CommandServerCallTaskQueueTask
                                    Type commandServerCallTaskQueueTaskType = method.GetCommandServerCallTaskQueueTaskType(methodIndex == serverInterface.VerifyMethodIndex, out returnGenericType);
                                    asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, commandServerCallTaskQueueTaskType);
                                    #endregion
                                    #region public CallTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter) : base(socket, false)
                                    var callbackAsynchronousConstructorBuilder = asynchronousConstructorBuilder;
                                    asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(CommandServerSocket), typeof(Func<CommandServerCallTaskQueueNode, KT, T>), method.InputParameterType.notNull().Type.MakeByRefType() });
                                    asynchronousConstructorGenerator = method.TaskQueueAsynchronousConstructorBase(asynchronousConstructorBuilder, returnGenericType, commandServerCallTaskQueueTaskType, methodIndex == serverInterface.VerifyMethodIndex);
                                    #endregion
                                    #region this.getTaskQueue = getTaskQueue;
                                    FieldBuilder asynchronousGetTaskQueueFieldBuilder = asynchronousTypeBuilder.DefineField("getTaskQueue", typeof(Func<CommandServerCallTaskQueueNode, KT, T>), FieldAttributes.Public | FieldAttributes.InitOnly);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Stfld, asynchronousGetTaskQueueFieldBuilder);
                                    #endregion
                                    #region this.inputParameter = inputParameter;
                                    var inputParameterFieldBuilder = method.GetTaskQueueInputParameterFieldBuilder(asynchronousTypeBuilder, asynchronousConstructorGenerator);
                                    #endregion
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #region public override bool RunTask()
                                    asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallTaskQueueNode.RunTask), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), EmptyArray<Type>.Array);
                                    asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                    #endregion
                                    #region CommandServerKeepCallback<string> keepCallback = CommandServerKeepCallback<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0);
                                    var keepCallbackLocalBuilder = method.GetTaskQueueKeepCallbackLocalBuilder(asynchronousMethodGenerator, returnGenericType);
                                    #endregion
                                    #region CommandServerCallTaskQueueTask<string>.CheckCallTask(this, ((T)(CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueue<KT>.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueue))).TaskQueue(inputParameter.Value, inputParameter.Ref));
                                    Label callMethodLabel = asynchronousMethodGenerator.DefineLabel();
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);

                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder.notNull());
                                            asynchronousMethodGenerator.call(ServerTaskQueueInterfaceController.CommandServerKeepCallbackQueueTaskGetTaskQueue.Method);
                                            break;
                                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            switch (method.MethodType)
                                            {
                                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                                                    asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerKeepCallbackQueueTaskGetTaskQueueDelegate.Method);
                                                    break;
#if NetStandard21
                                                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                                                    asynchronousMethodGenerator.call(returnGenericType.notNull().AsyncEnumerableQueueTaskGetTaskQueueDelegate.Method);
                                                    break;
#endif
                                            }
                                            break;
                                        default:
                                            asynchronousMethodGenerator.call(ServerTaskQueueInterfaceController.CommandServerCallTaskQueueNodeGetTaskQueue.Method);
                                            break;
                                    }
                                    asynchronousMethodGenerator.Emit(OpCodes.Castclass, type);
                                    asynchronousMethodGenerator.Emit(OpCodes.Dup);
                                    asynchronousMethodGenerator.Emit(OpCodes.Brtrue_S, callMethodLabel);
                                    asynchronousMethodGenerator.Emit(OpCodes.Pop);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder.notNull());
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldflda, method.InputParameterFields[method.InputParameterCount]);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousGetTaskQueueFieldBuilder);
                                    asynchronousMethodGenerator.call(commandServerTaskQueueCreateTaskQueue.Method);

                                    asynchronousMethodGenerator.MarkLabel(callMethodLabel);
                                    method.TaskQueueCallMethodParameter(asynchronousMethodGenerator, inputParameterFieldBuilder.notNull());
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder.notNull());
                                            break;
                                        case ServerMethodTypeEnum.CallbackTaskQueue:
                                            #region new AsynchronousCallback(this)
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                            if (method.ReturnValueType == typeof(void)) asynchronousMethodGenerator.call(ServerInterfaceController.CreateServerCallbackCallTaskQueueNodeDelegate.Method);
                                            else asynchronousMethodGenerator.Emit(OpCodes.Newobj, callbackAsynchronousConstructorBuilder.notNull());
                                            #endregion
                                            break;
                                    }
                                    asynchronousMethodGenerator.call(method.Method);
                                    method.CheckCallTaskQueue(asynchronousMethodGenerator, returnGenericType, methodIndex == serverInterface.VerifyMethodIndex);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();

                                    #region CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new CallTaskQueue(socket, GetTaskQueue, ref inputParameter));
                                    doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                    doCommandGenerator.Emit(OpCodes.Ldfld, taskQueueSetFieldBuilder);
                                    doCommandGenerator.Emit(OpCodes.Ldloc, inputParameterLocalBuilder.notNull());
                                    doCommandGenerator.Emit(OpCodes.Ldfld, method.InputParameterFields[method.InputParameterCount]);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                    //doCommandGenerator.Emit(OpCodes.Ldarg_3);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                    doCommandGenerator.Emit(OpCodes.Ldfld, getTaskQueueFieldBuilder);
                                    doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder.notNull());
                                    doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder);
                                    doCommandGenerator.call((method.IsLowPriorityQueue ? commandServerSocketCallTaskQueueAppendLowPriority : commandServerSocketCallTaskQueueAppendQueue).Method);
                                    #endregion
                                    break;
                                default: doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.Unknown); break;
                            }
                            doCommandGenerator.Emit(OpCodes.Ret);
                        }
                    }
                    ++methodIndex;
                }
                #endregion
                controllerConstructorInfo = typeBuilder.CreateType().GetConstructor(constructorParameterTypes).notNull();
            }
            catch (Exception exception)
            {
                controllerConstructorException = new Exception($"{type.fullName()} + {keyType.fullName()} 服务端控制器生成失败", exception);
            }
        }
    }
#if DEBUG && NetStandard21
    #region 控制器接口 IL 模板
    internal interface ServerTaskQueueInterfaceControllerIL
    {
        Task<string> TaskQueue(int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskQueue(int Value, int Ref);
        Task CallbackTaskQueue(int Value, int Ref, CommandServerCallback<string> Callback);
        Task KeepCallbackTaskQueue(int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackCountTaskQueue(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskQueue(int Value, int Ref);

#if NetStandard21
        IAsyncEnumerable<string> AsyncEnumerableTaskQueue(int Value, int Ref);
#endif
    }
    internal sealed class ServerTaskQueueInterfaceControllerIL<T, KT> : CommandServerController
        where KT : IEquatable<KT>
        where T : class, ServerInterfaceControllerIL
        //where T : CommandServerTaskQueue<KT>, ServerTaskQueueInterfaceControllerIL
    {
        public struct SynchronousInputParameter
        {
            public KT __KEY__;
            public int Value;
            public int Ref;
        }
        public new sealed class CallTaskQueue : CommandServerCallTaskQueueTask<string>
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public CallTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
#if NetStandard21
                return CommandServerCallTaskQueueTask<string>.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).TaskQueue(inputParameter.Value, inputParameter.Ref));
#else
                return CommandServerCallTaskQueueTask<string>.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).TaskQueue(inputParameter.Value, inputParameter.Ref));
#endif
            }
        }
        public sealed class CallbackTaskQueue : CommandServerCallTaskQueueTask
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public CallbackTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
#if NetStandard21
                return CommandServerCallTaskQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).CallbackTaskQueue(inputParameter.Value, inputParameter.Ref, new ServerInterfaceControllerIL<T>.AsynchronousCallback(this)));
#else
                return CommandServerCallTaskQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).CallbackTaskQueue(inputParameter.Value, inputParameter.Ref, new ServerInterfaceControllerIL<T>.AsynchronousCallback(this)));
#endif
            }
        }
        public sealed class SendOnlyTaskQueue : CommandServerCallTaskQueueSendOnlyTask
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public SendOnlyTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
#if NetStandard21
                return CommandServerCallTaskQueueSendOnlyTask.CheckCallTask(this, (((ServerTaskQueueInterfaceControllerIL?)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc))).SendOnlyTaskQueue(inputParameter.Value, inputParameter.Ref));
#else
                return CommandServerCallTaskQueueSendOnlyTask.CheckCallTask(this, (((ServerTaskQueueInterfaceControllerIL)CommandServerCallTaskQueueNode.GetTaskQueue(this) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc))).SendOnlyTaskQueue(inputParameter.Value, inputParameter.Ref));
#endif
            }
        }
        public sealed class KeepCallbackTaskQueue : CommandServerKeepCallbackQueueTask
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackTaskQueue, true)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerKeepCallback<string> keepCallback = CommandServerKeepCallback<string>.CreateServerKeepCallback(this, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0);
#if NetStandard21
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)CommandServerKeepCallbackQueueTask.GetTaskQueue(this, keepCallback) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).KeepCallbackTaskQueue(inputParameter.Value, inputParameter.Ref, keepCallback));
#else
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL)CommandServerKeepCallbackQueueTask.GetTaskQueue(this, keepCallback) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).KeepCallbackTaskQueue(inputParameter.Value, inputParameter.Ref, keepCallback));
#endif
            }
        }
        public sealed class KeepCallbackCountTaskQueue : CommandServerKeepCallbackQueueTask
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackCountTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackCountTaskQueue, true)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerKeepCallbackCount<string> keepCallback = CommandServerKeepCallbackCount<string>.CreateServerKeepCallback(this, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0);
#if NetStandard21
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)CommandServerKeepCallbackQueueTask.GetTaskQueue(this, keepCallback) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).KeepCallbackCountTaskQueue(inputParameter.Value, inputParameter.Ref, keepCallback));
#else
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL)CommandServerKeepCallbackQueueTask.GetTaskQueue(this, keepCallback) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).KeepCallbackCountTaskQueue(inputParameter.Value, inputParameter.Ref, keepCallback));
#endif
            }
        }
        public sealed class EnumerableKeepCallbackCountTaskQueue : CommandServerKeepCallbackQueueTask<string>
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public EnumerableKeepCallbackCountTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
#if NetStandard21
                return CommandServerKeepCallbackQueueTask<string>.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)CommandServerKeepCallbackQueueTask<string>.GetTaskQueue(this, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).EnumerableKeepCallbackCountTaskQueue(inputParameter.Value, inputParameter.Ref));
#else
                return CommandServerKeepCallbackQueueTask<string>.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL)CommandServerKeepCallbackQueueTask<string>.GetTaskQueue(this, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).EnumerableKeepCallbackCountTaskQueue(inputParameter.Value, inputParameter.Ref));
#endif
            }
        }
#if NetStandard21
        public sealed class AsyncEnumerableTaskQueue : AsyncEnumerableQueueTask<string>
        {
            private readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueueFunc;
            private SynchronousInputParameter inputParameter;
            public AsyncEnumerableTaskQueue(CommandServerSocket socket, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.getTaskQueueFunc = getTaskQueue;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                return AsyncEnumerableQueueTask<string>.CheckCallTask(this, ((ServerTaskQueueInterfaceControllerIL?)AsyncEnumerableQueueTask<string>.GetTaskQueue(this, ServerTaskQueueInterfaceControllerIL<T, KT>.Method0) ?? CommandServerTaskQueueService.CreateTaskQueue(this, ref inputParameter.__KEY__, getTaskQueueFunc)).AsyncEnumerableTaskQueue(inputParameter.Value, inputParameter.Ref));
            }
        }
#endif

        public readonly Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> GetTaskQueue;
        public readonly CommandServerCallTaskQueueSet<KT> TaskQueueSet;
        public ServerTaskQueueInterfaceControllerIL(CommandListener server, string controllerName, Func<CommandServerCallTaskQueueNode, KT, ServerTaskQueueInterfaceControllerIL> getTaskQueue)
            : base(server, controllerName, ServerTaskQueueInterfaceController<T, KT>.GetAttribute(), ServerTaskQueueInterfaceController<T, KT>.GetMethods(), int.MinValue, 0, false, false)
        {
            GetTaskQueue = getTaskQueue;
            TaskQueueSet = CommandListener.GetServerCallTaskQueueSet<KT>(server).notNull();
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override CommandClientReturnTypeEnum DoCommand(CommandServerSocket socket, ref SubArray<byte> data)
        {
            switch (CommandServerSocket.GetCommandMethodIndex(socket))
            {
                case (int)ServerMethodTypeEnum.Unknown: return CommandClientReturnTypeEnum.Unknown;
                case (int)ServerMethodTypeEnum.VersionExpired: return CommandClientReturnTypeEnum.VersionExpired;

                case (int)ServerMethodTypeEnum.TaskQueue:
                    SynchronousInputParameter inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new CallTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.CallbackTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new CallbackTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.SendOnlyTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new SendOnlyTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new KeepCallbackTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new KeepCallbackCountTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new EnumerableKeepCallbackCountTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#if NetStandard21
                case (int)ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(TaskQueueSet, inputParameter.__KEY__, new AsyncEnumerableTaskQueue(socket, GetTaskQueue, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#endif
            }
            return CommandClientReturnTypeEnum.Unknown;
        }
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        public static readonly ServerInterfaceMethod Method0;
        static ServerTaskQueueInterfaceControllerIL()
        {
            Method0 = ServerTaskQueueInterfaceController<T, KT>.GetMethod(0);
        }
    }
    #endregion
#endif
}
