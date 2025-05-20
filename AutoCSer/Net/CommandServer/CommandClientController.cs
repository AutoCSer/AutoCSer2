using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
#if AOT
using ClientInterfaceMethod = AutoCSer.Net.CommandServer.ClientMethod;
#endif

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令客户端控制器
    /// </summary>
    public abstract class CommandClientController
    {
        /// <summary>
        /// 命令客户端套接字
        /// </summary>
        public readonly CommandClientSocket Socket;
        /// <summary>
        /// 命令控制器名称
        /// </summary>
        public readonly string ControllerName;
        /// <summary>
        /// 客户端接口方法信息集合
        /// </summary>
        internal readonly ClientInterfaceMethod[] Methods;
        /// <summary>
        /// 服务端方法编号集合
        /// </summary>
        private readonly int[] serverMethodIndexs;
#if !AOT
        /// <summary>
        /// 获取错误方法集合
        /// </summary>
        public IEnumerable<KeyValue<MethodInfo, string>> ErrorMethods
        {
            get
            {
                foreach(var method in Methods)
                {
                    if (method.Error != null) yield return new KeyValue<MethodInfo, string>(method.Method, method.Error);
                }
            }
        }
#endif
        /// <summary>
        /// 方法起始序号
        /// </summary>
        internal readonly int StartMethodIndex;
        /// <summary>
        /// 验证方法序号
        /// </summary>
        internal readonly int VerifyMethodIndex;
        /// <summary>
        /// 默认空命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        internal CommandClientController(CommandClientSocket socket, string controllerName)
        {
            Socket = socket;
            ControllerName = controllerName;
#if NetStandard21
            Methods = EmptyArray<ClientInterfaceMethod>.Array;
            serverMethodIndexs = EmptyArray<int>.Array;
#endif
        }
        /// <summary>
        /// 命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodIndexs"></param>
        /// <param name="verifyMethodIndex"></param>
        internal CommandClientController(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod[] methods, int[] serverMethodIndexs, int verifyMethodIndex)
        {
            Socket = socket;
            ControllerName = controllerName;
            StartMethodIndex = startMethodIndex;
            Methods = methods;
            this.serverMethodIndexs = serverMethodIndexs;
            VerifyMethodIndex = verifyMethodIndex;
        }
        /// <summary>
        /// 获取控制器命令方法序号
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns>超出范围返回 0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint GetMethodIndex(int methodIndex)
        {
            return methodIndex < serverMethodIndexs.Length ? (uint)(StartMethodIndex + serverMethodIndexs[methodIndex]) : 0;
        }
        /// <summary>
        /// 设置服务端方法编号集合
        /// </summary>
        /// <param name="MethodNames"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void SetServerMethodIndexs(string?[] MethodNames)
#else
        internal void SetServerMethodIndexs(string[] MethodNames)
#endif
        {
            ClientInterfaceMethod.GetServerMethodIndexs(StartMethodIndex, Methods, MethodNames, serverMethodIndexs);
        }

#if AOT
        /// <summary>
        /// 同步等待命令
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue Synchronous(int methodIndex)
        {
            return new SynchronousCommand(this, methodIndex).Wait();
        }
#else
        /// <summary>
        /// 同步等待命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnValue Synchronous(CommandClientController controller, int methodIndex)
        {
            return new SynchronousCommand(controller, methodIndex).Wait();
        }
#endif
        /// <summary>
        /// 同步等待命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue SynchronousInput<T>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new SynchronousCommand<T>(this, methodIndex, ref inputParameter).Wait();
        }
        /// <summary>
        /// 同步等待命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue SynchronousOutput<T>(int methodIndex, ref T outputParameter)
            where T : struct
        {
            return new SynchronousOutputCommand<T>(this, methodIndex).Wait(out outputParameter);
        }
        /// <summary>
        /// 同步等待命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue SynchronousInputOutput<T, OT>(int methodIndex, ref T inputParameter, ref OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new SynchronousOutputCommand<T, OT>(this, methodIndex, ref inputParameter, outputParameter).Wait(out outputParameter);
        }

#if AOT
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand SendOnly(int methodIndex)
        {
            SendOnlyCommand command = new SendOnlyCommand(this, methodIndex);
            command.PushSendOnly();
            return command;
        }
#else
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SendOnlyCommand SendOnly(CommandClientController controller, int methodIndex)
        {
            SendOnlyCommand command = new SendOnlyCommand(controller, methodIndex);
            command.PushSendOnly();
            return command;
        }
#endif
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand SendOnlyInput<T>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new SendOnlyCommand<T>(this, methodIndex, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand Callback(int methodIndex, CommandClientCallback callback)
        {
            CallbackCommand command = new AutoCSer.Net.CommandServer.CallbackCommand(this, methodIndex, callback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand Callback<T, OT>(int methodIndex, CommandClientCallback<T> callback, Func<OT, T> getReturnValue)
            where OT : struct
        {
            CallbackOutputCommand<T, OT> command = new CallbackOutputCommand<T, OT>(this, methodIndex, callback, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackOutput<T, RT, OT>(int methodIndex, CommandClientCallback<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new CallbackOutputCommand<T, RT, OT>(this, methodIndex, callback, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackOutputReturnValue<T, RT, OT>(int methodIndex, CommandClientCallback<RT> callback, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new CallbackOutputCommand<T, RT, OT>(this, methodIndex, callback, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand Callback(CommandClientController controller, int methodIndex, CommandClientCallback callback)
        {
            CallbackCommand command = new AutoCSer.Net.CommandServer.CallbackCommand(controller, methodIndex, callback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand Callback<T>(CommandClientController controller, int methodIndex, CommandClientCallback<T> callback)
        {
            CallbackOutputCommand<T> command = new CallbackOutputCommand<T>(controller, methodIndex, callback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackOutput<T, RT>(int methodIndex, CommandClientCallback<RT> callback, ref T inputParameter)
            where T : struct
        {
            return new CallbackOutputCommand<T, RT>(this, methodIndex, callback, ref inputParameter);
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackOutputReturnValue<T, RT>(int methodIndex, CommandClientCallback<RT> callback, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new CallbackOutputCommand<T, RT>(this, methodIndex, callback, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackInput<T>(int methodIndex, CommandClientCallback callback, ref T inputParameter)
            where T : struct
        {
            return new CallbackCommand<T>(this, methodIndex, callback, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallback(int methodIndex, CommandClientKeepCallback keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this, methodIndex, keepCallback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallback<T, OT>(int methodIndex, CommandClientKeepCallback<T> keepCallback, Func<OT, T> getReturnValue)
            where OT : struct
        {
            KeepCallbackOutputCommand<T, OT> command = new KeepCallbackOutputCommand<T, OT>(this, methodIndex, keepCallback, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackOutput<T, RT, OT>(int methodIndex, CommandClientKeepCallback<RT> keepCallback, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new KeepCallbackOutputCommand<T, RT, OT>(this, methodIndex, keepCallback, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackOutputReturnValue<T, RT, OT>(int methodIndex, CommandClientKeepCallback<RT> keepCallback, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new KeepCallbackOutputCommand<T, RT, OT>(this, methodIndex, keepCallback, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallback(CommandClientController controller, int methodIndex, CommandClientKeepCallback keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(controller, methodIndex, keepCallback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallback<T>(CommandClientController controller, int methodIndex, CommandClientKeepCallback<T> keepCallback)
        {
            KeepCallbackOutputCommand<T> command = new KeepCallbackOutputCommand<T>(controller, methodIndex, keepCallback);
            command.Push();
            return command;
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackOutput<T, RT>(int methodIndex, CommandClientKeepCallback<RT> keepCallback, ref T inputParameter)
            where T : struct
        {
            return new KeepCallbackOutputCommand<T, RT>(this, methodIndex, keepCallback, ref inputParameter);
        }
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackOutputReturnValue<T, RT>(int methodIndex, CommandClientKeepCallback<RT> keepCallback, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new KeepCallbackOutputCommand<T, RT>(this, methodIndex, keepCallback, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallback"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackInput<T>(int methodIndex, CommandClientKeepCallback keepCallback, ref T inputParameter)
            where T : struct
        {
            return new KeepCallbackCommand<T>(this, methodIndex, keepCallback, ref inputParameter);
        }

        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        internal void AppendQueue(ClientInterfaceMethod method, CommandClientCallQueueNode callback)
        {
            if (method.IsLowPriorityQueue)
            {
                Socket.Client.GetCommandClientCallQueueLowPriority(method.QueueIndex)?.Add(callback);
            }
            else Socket.Client.GetCommandClientCallQueue(method.QueueIndex)?.Add(callback);
        }

#if AOT
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueue(int methodIndex, CommandClientCallbackQueueNode callbackQueue)
        {
            CallbackQueueCommand command = new CallbackQueueCommand(this, methodIndex, callbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueue<T, OT>(int methodIndex, CommandClientCallbackQueueNode<T> callbackQueue, Func<OT, T> getReturnValue)
            where OT : struct
        {
            CallbackQueueOutputCommand<T, OT> command = new CallbackQueueOutputCommand<T, OT>(this, methodIndex, callbackQueue, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueueOutput<T, RT, OT>(int methodIndex, CommandClientCallbackQueueNode<RT> callbackQueue, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new CallbackQueueOutputCommand<T, RT, OT>(this, methodIndex, callbackQueue, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueueOutputReturnValue<T, RT, OT>(int methodIndex, CommandClientCallbackQueueNode<RT> callbackQueue, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new CallbackQueueOutputCommand<T, RT, OT>(this, methodIndex, callbackQueue, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallbackQueue(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode callbackQueue)
        {
            CallbackQueueCommand command = new CallbackQueueCommand(controller, methodIndex, callbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallbackQueue<T>(CommandClientController controller, int methodIndex, CommandClientCallbackQueueNode<T> callbackQueue)
        {
            CallbackQueueOutputCommand<T> command = new CallbackQueueOutputCommand<T>(controller, methodIndex, callbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueueOutput<T, RT>(int methodIndex, CommandClientCallbackQueueNode<RT> callbackQueue, ref T inputParameter)
            where T : struct
        {
            return new CallbackQueueOutputCommand<T, RT>(this, methodIndex, callbackQueue, ref inputParameter);
        }
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueueOutputReturnValue<T, RT>(int methodIndex, CommandClientCallbackQueueNode<RT> callbackQueue, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new CallbackQueueOutputCommand<T, RT>(this, methodIndex, callbackQueue, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 队列回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="callbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueueInput<T>(int methodIndex, CommandClientCallbackQueueNode callbackQueue, ref T inputParameter)
            where T : struct
        {
            return new CallbackQueueCommand<T>(this, methodIndex, callbackQueue, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(int methodIndex, CommandClientKeepCallbackQueue keepCallbackQueue)
        {
            KeepCallbackQueueCommand command = new KeepCallbackQueueCommand(this, methodIndex, keepCallbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue<T, OT>(int methodIndex, CommandClientKeepCallbackQueue<T> keepCallbackQueue, Func<OT, T> getReturnValue)
            where OT : struct
        {
            KeepCallbackQueueOutputCommand<T, OT> command = new KeepCallbackQueueOutputCommand<T, OT>(this, methodIndex, keepCallbackQueue, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueueOutput<T, RT, OT>(int methodIndex, CommandClientKeepCallbackQueue<RT> keepCallbackQueue, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new KeepCallbackQueueOutputCommand<T, RT, OT>(this, methodIndex, keepCallbackQueue, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueueOutputReturnValue<T, RT, OT>(int methodIndex, CommandClientKeepCallbackQueue<RT> keepCallbackQueue, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new KeepCallbackQueueOutputCommand<T, RT, OT>(this, methodIndex, keepCallbackQueue, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(CommandClientController controller, int methodIndex, CommandClientKeepCallbackQueue keepCallbackQueue)
        {
            KeepCallbackQueueCommand command = new KeepCallbackQueueCommand(controller, methodIndex, keepCallbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue<T>(CommandClientController controller, int methodIndex, CommandClientKeepCallbackQueue<T> keepCallbackQueue)
        {
            KeepCallbackQueueOutputCommand<T> command = new KeepCallbackQueueOutputCommand<T>(controller, methodIndex, keepCallbackQueue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueueOutput<T, RT>(int methodIndex, CommandClientKeepCallbackQueue<RT> keepCallbackQueue, ref T inputParameter)
            where T : struct
        {
            return new KeepCallbackQueueOutputCommand<T, RT>(this, methodIndex, keepCallbackQueue, ref inputParameter);
        }
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueueOutputReturnValue<T, RT>(int methodIndex, CommandClientKeepCallbackQueue<RT> keepCallbackQueue, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new KeepCallbackQueueOutputCommand<T, RT>(this, methodIndex, keepCallbackQueue, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 队列保持回调委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="keepCallbackQueue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueueInput<T>(int methodIndex, CommandClientKeepCallbackQueue keepCallbackQueue, ref T inputParameter)
            where T : struct
        {
            return new KeepCallbackQueueCommand<T>(this, methodIndex, keepCallbackQueue, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand ReturnType(int methodIndex)
        {
            ReturnTypeCommand command = new ReturnTypeCommand(this, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<T> ReturnValue<T, OT>(int methodIndex, Func<OT, T> getReturnValue)
            where OT : struct
        {
            ReturnValueCommand<T, OT> command = new ReturnValueCommand<T, OT>(this, methodIndex, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<RT> ReturnValueOutput<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new ReturnValueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<RT> ReturnValueOutputReturnValue<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new ReturnValueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnCommand ReturnType(CommandClientController controller, int methodIndex)
        {
            ReturnTypeCommand command = new ReturnTypeCommand(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnCommand<T> ReturnValue<T>(CommandClientController controller, int methodIndex)
        {
            ReturnValueCommand<T> command = new ReturnValueCommand<T>(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<RT> ReturnValueOutput<T, RT>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new ReturnValueCommand<T, RT>(this, methodIndex, ref inputParameter);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<RT> ReturnValueOutputReturnValue<T, RT>(int methodIndex, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new ReturnValueCommand<T, RT>(this, methodIndex, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand ReturnTypeInput<T>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new ReturnTypeCommand<T>(this, methodIndex, ref inputParameter);
        }

        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        internal void AppendQueue(ClientInterfaceMethod method, Action callback)
        {
            if (method.IsLowPriorityQueue)
            {
                Socket.Client.GetCommandClientCallQueueLowPriority(method.QueueIndex)?.Add(new ReturnCommandQueueNode(callback));
            }
            else Socket.Client.GetCommandClientCallQueue(method.QueueIndex)?.Add(new ReturnCommandQueueNode(callback));
        }

#if AOT
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand ReturnTypeQueue(int methodIndex)
        {
            ReturnTypeQueueCommand command = new ReturnTypeQueueCommand(this, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<T> ReturnValueQueue<T, OT>(int methodIndex, Func<OT, T> getReturnValue)
            where OT : struct
        {
            ReturnValueQueueCommand<T, OT> command = new ReturnValueQueueCommand<T, OT>(this, methodIndex, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<RT> ReturnValueQueueOutput<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new ReturnValueQueueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<RT> ReturnValueQueueOutputReturnValue<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new ReturnValueQueueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnQueueCommand ReturnTypeQueue(CommandClientController controller, int methodIndex)
        {
            ReturnTypeQueueCommand command = new ReturnTypeQueueCommand(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnQueueCommand<T> ReturnValueQueue<T>(CommandClientController controller, int methodIndex)
        {
            ReturnValueQueueCommand<T> command = new ReturnValueQueueCommand<T>(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<RT> ReturnValueQueueOutput<T, RT>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new ReturnValueQueueCommand<T, RT>(this, methodIndex, ref inputParameter);
        }
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<RT> ReturnValueQueueOutputReturnValue<T, RT>(int methodIndex, ref T inputParameter, ref RT returnValue)
            where T : struct
        {
            return new ReturnValueQueueCommand<T, RT>(this, methodIndex, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand ReturnTypeQueueInput<T>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new ReturnTypeQueueCommand<T>(this, methodIndex, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand Enumerator(int methodIndex)
        {
            EnumeratorCommand command = new EnumeratorCommand(this, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<T> Enumerator<T, OT>(int methodIndex, Func<OT, T> getReturnValue)
            where OT : struct
        {
            AutoCSer.Net.EnumeratorCommand<T, OT> command = new AutoCSer.Net.EnumeratorCommand<T, OT>(this, methodIndex, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<RT> EnumeratorOutput<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new EnumeratorCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<RT> EnumeratorOutputReturnValue<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new EnumeratorCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorCommand Enumerator(CommandClientController controller, int methodIndex)
        {
            EnumeratorCommand command = new EnumeratorCommand(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorCommand<T> Enumerator<T>(CommandClientController controller, int methodIndex)
        {
            AutoCSer.Net.EnumeratorCommand<T> command = new AutoCSer.Net.EnumeratorCommand<T>(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<OT> EnumeratorOutput<T, OT>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new EnumeratorCommand<T, OT>(this, methodIndex, ref inputParameter);
        }
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<OT> EnumeratorOutputReturnValue<T, OT>(int methodIndex, ref T inputParameter, ref OT returnValue)
            where T : struct
        {
            return new EnumeratorCommand<T, OT>(this, methodIndex, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand EnumeratorInput<T>(int methodIndex, ref T inputParameter)
            where T :  struct
        {
            return new AutoCSer.Net.CommandServer.EnumeratorCommand<T>(this, methodIndex, ref inputParameter);
        }

#if AOT
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand EnumeratorQueue(int methodIndex)
        {
            EnumeratorQueueCommand command = new EnumeratorQueueCommand(this, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<T> EnumeratorQueue<T, OT>(int methodIndex, Func<OT, T> getReturnValue)
            where OT : struct
        {
            AutoCSer.Net.EnumeratorQueueCommand<T, OT> command = new AutoCSer.Net.EnumeratorQueueCommand<T, OT>(this, methodIndex, getReturnValue);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<RT> EnumeratorQueueOutput<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter)
            where T : struct
            where OT : struct
        {
            return new EnumeratorQueueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter);
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<RT> EnumeratorQueueOutputReturnValue<T, RT, OT>(int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter)
            where T : struct
            where OT : struct
        {
            return new EnumeratorQueueCommand<T, RT, OT>(this, methodIndex, getReturnValue, ref inputParameter, outputParameter);
        }
#else
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorQueueCommand EnumeratorQueue(CommandClientController controller, int methodIndex)
        {
            EnumeratorQueueCommand command = new EnumeratorQueueCommand(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorQueueCommand<T> EnumeratorQueue<T>(CommandClientController controller, int methodIndex)
        {
            AutoCSer.Net.EnumeratorQueueCommand<T> command = new AutoCSer.Net.EnumeratorQueueCommand<T>(controller, methodIndex);
            command.Push();
            return command;
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<OT> EnumeratorQueueOutput<T, OT>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new EnumeratorQueueCommand<T, OT>(this, methodIndex, ref inputParameter);
        }
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<OT> EnumeratorQueueOutputReturnValue<T, OT>(int methodIndex, ref T inputParameter, ref OT returnValue)
            where T : struct
        {
            return new EnumeratorQueueCommand<T, OT>(this, methodIndex, ref inputParameter, ref returnValue);
        }
#endif
        /// <summary>
        /// 队列枚举返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand EnumeratorQueueInput<T>(int methodIndex, ref T inputParameter)
            where T : struct
        {
            return new AutoCSer.Net.CommandServer.EnumeratorQueueCommand<T>(this, methodIndex, ref inputParameter);
        }
    }
#if AOT
    /// <summary>
    /// 非对称命令客户端控制器
    /// </summary>
    /// <typeparam name="CT">客户端接口类型</typeparam>
    public abstract class CommandClientController<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] CT> : CommandClientController
    {
        /// <summary>
        /// 命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <param name="verifyMethodIndex"></param>
        protected CommandClientController(CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames, int verifyMethodIndex)
            : base(socket, controllerName, startMethodIndex, ClientInterfaceController<CT, AutoCSer.Net.CommandServer.ServerInterface>.Methods, ClientInterfaceMethod.GetServerMethodIndexs(startMethodIndex, ClientInterfaceController<CT, AutoCSer.Net.CommandServer.ServerInterface>.Methods, serverMethodNames), verifyMethodIndex)
        {
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="IT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="actionCallback"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal CommandClientReturnValue CallMethodName<T, IT, OT>(int methodIndex, object actionCallback, object callback, object getReturnValue, ref IT inputParameter, ref OT outputParameter)
        {
            return default;
        }
    }
    /// <summary>
    /// 命令客户端控制器
    /// </summary>
    /// <typeparam name="CT">客户端接口类型</typeparam>
    /// <typeparam name="ST">服务端接口类型</typeparam>
    public abstract class CommandClientController<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] CT, [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] ST> : CommandClientController
    {
        /// <summary>
        /// 命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <param name="verifyMethodIndex"></param>
        protected CommandClientController(CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames, int verifyMethodIndex)
            : base(socket, controllerName, startMethodIndex, ClientInterfaceController<CT, ST>.Methods, ClientInterfaceMethod.GetServerMethodIndexs(startMethodIndex, ClientInterfaceController<CT, ST>.Methods, serverMethodNames), verifyMethodIndex)
        {
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="IT"></typeparam>
        /// <typeparam name="OT"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="actionCallback"></param>
        /// <param name="callback"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal CommandClientReturnValue CallMethodName<T, IT, OT>(int methodIndex, object actionCallback, object callback, object getReturnValue, ref IT inputParameter, ref OT outputParameter)
        {
            return default;
        }
    }
#endif
}
