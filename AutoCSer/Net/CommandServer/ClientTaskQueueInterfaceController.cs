using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    /// <typeparam name="T">客户端接口类型</typeparam>
    /// <typeparam name="ST">服务端接口类型</typeparam>
    /// <typeparam name="KT"></typeparam>
    internal static class ClientTaskQueueInterfaceController<T, ST, KT>
        where KT : IEquatable<KT>
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
        internal static TaskQueueClientController<T, KT> Create(CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
#else
        internal static TaskQueueClientController<T, KT> Create(CommandClientSocket socket, string controllerName, int startMethodIndex, string[] serverMethodNames)
#endif
        {
            return new TaskQueueClientController<T, KT>(socket, controllerName, startMethodIndex, methods, ClientInterfaceMethod.GetServerMethodIndexs(startMethodIndex, methods, serverMethodNames), 0, create);
        }
        /// <summary>
        /// 创建命令客户端控制器
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T create(TaskQueueClientController<T, KT> controller, KT key)
        {
            if (ControllerConstructorException == null)
            {
                T taskQueueController = callConstructor(controller, key);
                if (controllerConstructorMessages == null) return taskQueueController;
                controller.Socket.Client.Config.OnControllerConstructorMessage(typeof(T), controllerConstructorMessages);
                return taskQueueController;
            }
            throw ControllerConstructorException;
        }
        /// <summary>
        /// 客户端接口方法信息集合
        /// </summary>
        private static ClientInterfaceMethod[] methods;
        /// <summary>
        /// 控制器构造函数
        /// </summary>
        private static readonly Func<TaskQueueClientController<T, KT>, KT, T> callConstructor;
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

#pragma warning disable CS8618
        static ClientTaskQueueInterfaceController()
#pragma warning restore CS8618
        {
            Type type = typeof(T), serverType = typeof(ST), keyType = typeof(KT);
            methods = EmptyArray<ClientInterfaceMethod>.Array;
            try
            {
                ServerInterface serverInterface = new ServerInterface(serverType, keyType, type);
                LeftArray<ClientInterfaceMethod> methodArray;
                if (!serverInterface.GetClientMethods(type, keyType, ref ControllerConstructorException, ref controllerConstructorMessages, out methodArray)) return;

                Type[] constructorParameterTypes = new Type[] { typeof(TaskQueueClientController<T, KT>), keyType };
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandClient.TaskQueueInterfaceController." + type.FullName + "." + keyType.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(CommandClientTaskQueueController<KT>), new Type[] { type });
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(controller, key)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Call, typeof(CommandClientTaskQueueController<KT>).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, constructorParameterTypes, null).notNull());
                constructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #endregion
                Func<CommandClientTaskQueueController<KT>, KT> commandClientTaskQueueControllerGetKey = CommandClientTaskQueueController<KT>.GetKey;
                Func<CommandClientTaskQueueController<KT>, CommandClientController> commandClientTaskQueueControllerGetController = CommandClientTaskQueueController<KT>.GetController;
                int methodIndex = 0;
                methods = methodArray.ToArray();
                foreach (ClientInterfaceMethod method in methods)
                {
                    ParameterInfo[] parameters = method.Method.GetParameters();
                    MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Method.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, method.Method.ReturnType, parameters.getArray(parameter => parameter.ParameterType));
                    typeBuilder.DefineMethodOverride(methodBuilder, method.Method);
                    ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                    if (method.Error == null)
                    {
                        #region SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
                        var newInputParameterLocalBuilder = default(LocalBuilder);
                        LocalBuilder inputParameterLocalBuilder = method.GetInputParameterLocalBuilder(methodGenerator, out newInputParameterLocalBuilder).notNull();
                        method.SetInputParameter(methodGenerator, newInputParameterLocalBuilder.notNull());
                        methodGenerator.Emit(OpCodes.Ldloca, newInputParameterLocalBuilder.notNull());
                        methodGenerator.Emit(OpCodes.Ldarg_0);
                        methodGenerator.call(commandClientTaskQueueControllerGetKey.Method);
                        methodGenerator.Emit(OpCodes.Stfld, method.InputParameterFields[method.InputParameterCount]);
                        methodGenerator.Emit(OpCodes.Ldloc_S, newInputParameterLocalBuilder.notNull());
                        methodGenerator.Emit(OpCodes.Stloc_S, inputParameterLocalBuilder);
                        #endregion
                        #region SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                        var returnValueGenericType = default(GenericType);
                        var twoStage‌CallbackReturnValueGenericType = default(GenericType);
                        var outputParameterLocalBuilder = method.GetOutputParameterLocalBuilder(methodGenerator, out returnValueGenericType, out twoStage‌CallbackReturnValueGenericType);
                        #endregion
                        methodGenerator.Emit(OpCodes.Ldarg_0);
                        methodGenerator.call(commandClientTaskQueueControllerGetController.Method);
                        methodGenerator.int32(methodIndex);
                        method.CallbackParameter(methodGenerator, returnValueGenericType, twoStage‌CallbackReturnValueGenericType);
                        if (method.InputParameterType != null) methodGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder);
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

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", typeof(T), constructorParameterTypes, controllerType, true);
                ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                callConstructorGenerator.Emit(OpCodes.Ldarg_1);
                callConstructorGenerator.Emit(OpCodes.Newobj, controllerType.GetConstructor(constructorParameterTypes).notNull());
                callConstructorGenerator.Emit(OpCodes.Ret);
                callConstructor = (Func<TaskQueueClientController<T, KT>, KT, T>)dynamicMethod.CreateDelegate(typeof(Func<TaskQueueClientController<T, KT>, KT, T>));
            }
            catch (Exception exception)
            {
                ControllerConstructorException = new Exception($"{serverType.fullName()} + {keyType.fullName()} 客户端控制器 {type.fullName()} 生成失败", exception);
            }
        }
    }
#if DEBUG && NetStandard21
    #region 控制器接口 IL 模板
    internal sealed class ClientTaskQueueInterfaceControllerIL<KT> : CommandClientTaskQueueController<KT>, IClientInterfaceControllerIL
        where KT : IEquatable<KT>
    {
        public struct SynchronousInputParameter
        {
            public KT __KEY__;
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
        internal ClientTaskQueueInterfaceControllerIL(TaskQueueClientController<IClientInterfaceControllerIL, KT> controller, KT key)
            : base(controller, key)
        {
        }

        void IClientInterfaceControllerIL.SynchronousSymmetry(int Value, ref int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue.CheckThrowException(Controller.SynchronousInput(0, ref inputParameter));
            Ref = outputParameter.Ref;
        }
        string IClientInterfaceControllerIL.SynchronousSymmetry(int Value, ref int Ref, out int Out)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue.CheckThrowException(Controller.SynchronousInputOutput(0, ref inputParameter, ref outputParameter));
            Ref = outputParameter.Ref;
            Out = outputParameter.Out;
            return outputParameter.__Return__;
        }
        CommandClientReturnValue IClientInterfaceControllerIL.Synchronous(int Value, ref int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue returnValue = Controller.SynchronousInput(0, ref inputParameter);
            Ref = outputParameter.Ref;
            return returnValue;
        }
        CommandClientReturnValue<string> IClientInterfaceControllerIL.Synchronous(int Value, ref int Ref, out int Out)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
            CommandClientReturnValue returnValue = Controller.SynchronousInputOutput(0, ref inputParameter, ref outputParameter);
            Ref = outputParameter.Ref;
            Out = outputParameter.Out;
            if (CommandClientReturnValue.GetIsSuccess(returnValue)) return CommandClientReturnValue<string>.GetReturnValue(outputParameter.__Return__);
            return CommandClientReturnValue<string>.GetReturnValue(returnValue);
        }
        AutoCSer.Net.SendOnlyCommand IClientInterfaceControllerIL.SendOnly(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.SendOnlyInput(0, ref inputParameter);
        }
        AutoCSer.Net.CallbackCommand IClientInterfaceControllerIL.Callback(int Value, int Ref, CommandClientCallback<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.CallbackOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.KeepCallbackCommand IClientInterfaceControllerIL.KeepCallback(int Value, int Ref, CommandClientKeepCallback<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.KeepCallbackOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.CallbackCommand IClientInterfaceControllerIL.CallbackQueue(int Value, int Ref, CommandClientCallbackQueueNode<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.CallbackQueueOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.KeepCallbackCommand IClientInterfaceControllerIL.KeepCallbackQueue(int Value, int Ref, CommandClientKeepCallbackQueue<string> Callback)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.KeepCallbackQueueOutput(0, Callback, ref inputParameter);
        }
        AutoCSer.Net.ReturnCommand<string> IClientInterfaceControllerIL.ReturnValue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.ReturnValueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
        Task<string> IClientInterfaceControllerIL.ReturnValueTask(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return AutoCSer.Net.ReturnCommand<string>.GetTask(Controller.ReturnValueOutput<SynchronousInputParameter, string>(0, ref inputParameter));
        }
        AutoCSer.Net.ReturnQueueCommand<string> IClientInterfaceControllerIL.ReturnValueQueue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.ReturnValueQueueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
        AutoCSer.Net.EnumeratorCommand<string> IClientInterfaceControllerIL.Enumerator(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.EnumeratorOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
#if NetStandard21
        IAsyncEnumerable<string> IClientInterfaceControllerIL.AsyncEnumerable(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return AutoCSer.Net.EnumeratorCommand<string>.GetAsyncEnumerable(Controller.EnumeratorOutput<SynchronousInputParameter, string>(0, ref inputParameter));
        }
#endif
        AutoCSer.Net.EnumeratorQueueCommand<string> IClientInterfaceControllerIL.EnumeratorQueue(int Value, int Ref)
        {
            SynchronousInputParameter inputParameter = new SynchronousInputParameter { Value = Value, Ref = Ref, __KEY__ = CommandClientTaskQueueController<KT>.GetKey(this) };
            return Controller.EnumeratorQueueOutput<SynchronousInputParameter, string>(0, ref inputParameter);
        }
    }
    #endregion
#endif
}
