using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The client initializes the controller by default (to avoid null reference exceptions before the connection is established)
    /// 客户端默认初始化控制器（避免连接建立之前产生 null 引用异常）
    /// </summary>
    public class CommandClientDefaultController : CommandClientController
    {
        /// <summary>
        /// Command client
        /// </summary>
        internal readonly CommandClient Client;
        /// <summary>
        /// The default initialization controller call return type
        /// 默认初始化控制器调用返回类型
        /// </summary>
        public CommandClientReturnTypeEnum DefaultControllerReturnType { get { return Client.DefaultControllerReturnType; } }
        /// <summary>
        /// The client executes the queue
        /// 客户端执行队列
        /// </summary>
        private readonly CommandClientCallQueue callQueue;
        /// <summary>
        /// 默认空控制器
        /// Default empty controller
        /// </summary>
        private CommandClientDefaultController() : base(CommandClientSocket.Null, string.Empty)
        {
#if NetStandard21
            Client = CommandClient.Null;
#endif
            callQueue = new CommandClientCallQueue(this);
        }
        /// <summary>
        /// The client initializes the controller by default (to avoid null reference exceptions before the connection is established)
        /// 客户端默认初始化控制器（避免连接建立之前产生 null 引用异常）
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="controllerName"></param>
        protected CommandClientDefaultController(CommandClient client, string controllerName) : base(CommandClientSocket.Null, controllerName)
        {
            Client = client;
            callQueue = Null.callQueue;
        }

#if AOT
        /// <summary>
        /// Synchronous return
        /// 同步返回
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue Synchronous()
        {
            return DefaultControllerReturnType;
        }
#else
        /// <summary>
        /// Synchronous return
        /// 同步返回
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnValue Synchronous(CommandClientDefaultController controller)
        {
            return controller.DefaultControllerReturnType;
        }
#endif
        /// <summary>
        /// Synchronous return
        /// 同步返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<T> DefaultSynchronous<T>()
        {
            return new CommandClientReturnValue<T>(DefaultControllerReturnType, null);
        }
#if AOT
        /// <summary>
        /// Unresponsive command
        /// 无响应命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand SendOnly()
        {
            return new SendOnlyCommand(this);
        }
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand Callback(CommandClientCallback callback)
        {
            callback.Callback(DefaultControllerReturnType, null);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#else
        /// <summary>
        /// Unresponsive command
        /// 无响应命令
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SendOnlyCommand SendOnly(CommandClientDefaultController controller)
        {
            return new SendOnlyCommand(controller);
        }
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand Callback(CommandClientDefaultController controller, CommandClientCallback callback)
        {
            callback.Callback(controller.DefaultControllerReturnType, null);
            return new AutoCSer.Net.CommandServer.CallbackCommand(controller);
        }
#endif
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand DefaultCallback<T>(CommandClientCallback<T> callback)
        {
            callback.Callback(DefaultControllerReturnType, null);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#if AOT
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand Callback(Action<CommandClientReturnValue> callback)
        {
            callback(DefaultControllerReturnType);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#else
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand Callback(CommandClientDefaultController controller, Action<CommandClientReturnValue> callback)
        {
            callback(controller.DefaultControllerReturnType);
            return new AutoCSer.Net.CommandServer.CallbackCommand(controller);
        }
#endif
        /// <summary>
        /// Callback delegate command
        /// 回调委托命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand DefaultCallbackAction<T>(Action<CommandClientReturnValue<T>> callback)
        {
            callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null));
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueue(CommandClientCallbackQueueNode callback)
        {
            callback.Callback(DefaultControllerReturnType, callQueue);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#if !AOT
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallbackQueue(CommandClientDefaultController controller, CommandClientCallbackQueueNode callback)
        {
            return controller.CallbackQueue(callback);
        }
#endif
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand DefaultCallbackQueue<T>(CommandClientCallbackQueueNode<T> callback)
        {
            callback.Callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null), callQueue);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand CallbackQueue(Action<AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue> callback)
        {
            callback(DefaultControllerReturnType, callQueue);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#if !AOT
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallbackQueue(CommandClientDefaultController controller, Action<AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue> callback)
        {
            return controller.CallbackQueue(callback);
        }
#endif
        /// <summary>
        /// Callback delegate queue command
        /// 回调委托队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CallbackCommand DefaultCallbackActionQueue<T>(Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.CommandClientCallQueue> callback)
        {
            callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null), callQueue);
            return new AutoCSer.Net.CommandServer.CallbackCommand(this);
        }
#if AOT
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand KeepCallback(CommandClientKeepCallback keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            keepCallback.Error(DefaultControllerReturnType, null, command);
            return command;
        }
#else
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallback(CommandClientDefaultController controller, CommandClientKeepCallback keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(controller);
            keepCallback.Error(controller.DefaultControllerReturnType, null, command);
            return command;
        }
#endif
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand DefaultKeepCallback<T>(CommandClientKeepCallback<T> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            keepCallback.Error(DefaultControllerReturnType, null, command);
            return command;
        }
#if AOT
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand KeepCallback(Action<CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            keepCallback(DefaultControllerReturnType, command);
            return command;
        }
#else
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallback(CommandClientDefaultController controller, Action<CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(controller);
            keepCallback(controller.DefaultControllerReturnType, command);
            return command;
        }
#endif
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand DefaultKeepCallbackAction<T>(Action<CommandClientReturnValue<T>, AutoCSer.Net.KeepCallbackCommand> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            keepCallback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null), command);
            return command;
        }
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(CommandClientKeepCallbackQueue callback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            callback.Callback(DefaultControllerReturnType, callQueue, command);
            return command;
        }
#if !AOT
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(CommandClientDefaultController controller, CommandClientKeepCallbackQueue callback)
        {
            return controller.KeepCallbackQueue(callback);
        }
#endif
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand DefaultKeepCallbackQueue<T>(CommandClientKeepCallbackQueue<T> callback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            callback.Callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null), callQueue, command);
            return command;
        }
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            callback(DefaultControllerReturnType, callQueue, command);
            return command;
        }
#if !AOT
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackQueue(CommandClientDefaultController controller, Action<CommandClientReturnValue, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            return controller.KeepCallbackQueue(callback);
        }
#endif
        /// <summary>
        /// Keep callback queue command
        /// 保持回调队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand DefaultKeepCallbackActionQueue<T>(Action<CommandClientReturnValue<T>, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null), callQueue, command);
            return command;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand DefaultTwoStage‌Callback<T, KT>(CommandClientCallback<T> callback, CommandClientKeepCallback<KT> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            try
            {
                callback.Callback(DefaultControllerReturnType, null);
            }
            finally { keepCallback.Error(DefaultControllerReturnType, null, command); }
            return command;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        public AutoCSer.Net.KeepCallbackCommand DefaultTwoStage‌CallbackAction<T, KT>(Action<CommandClientReturnValue<T>> callback, Action<CommandClientReturnValue<KT>, AutoCSer.Net.KeepCallbackCommand> keepCallback)
        {
            KeepCallbackCommand command = new AutoCSer.Net.CommandServer.KeepCallbackCommand(this);
            try
            {
                callback(new CommandClientReturnValue<T>(DefaultControllerReturnType, null));
            }
            finally { keepCallback(new CommandClientReturnValue<KT>(DefaultControllerReturnType, null), command); }
            return command;
        }
        /// <summary>
        /// Two-stage callback command
        /// 两阶段回调命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.KeepCallbackCommand DefaultTwoStage‌CallbackParameter<T, KT>(CommandClientReturnValueParameterCallback<T> callback, Action<CommandClientReturnValue<KT>, AutoCSer.Net.KeepCallbackCommand> keepCallback)
        {
            return DefaultTwoStage‌CallbackAction(callback.Callback, keepCallback);
        }
#if AOT
        /// <summary>
        /// Return value command
        /// 返回值命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand ReturnType()
        {
            return new ReturnTypeCommand(this);
        }
#else
        /// <summary>
        /// Return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnCommand ReturnType(CommandClientDefaultController controller)
        {
            return new ReturnTypeCommand(controller);
        }
#endif
        /// <summary>
        /// Return value command
        /// 返回值命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnCommand<T> DefaultReturnValue<T>()
        {
            return new ReturnValueCommand<T>(this);
        }
#if AOT
        /// <summary>
        /// Return value queue command
        /// 返回值队列命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand ReturnTypeQueue()
        {
            return new ReturnTypeQueueCommand(this);
        }
#else
        /// <summary>
        /// Return value queue command
        /// 返回值队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.ReturnQueueCommand ReturnTypeQueue(CommandClientDefaultController controller)
        {
            return new ReturnTypeQueueCommand(controller);
        }
#endif
        /// <summary>
        /// Return value queue command
        /// 返回值队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.ReturnQueueCommand<T> DefaultReturnValueQueue<T>()
        {
            return new ReturnValueQueueCommand<T>(this);
        }
#if AOT
        /// <summary>
        /// Collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand Enumerator()
        {
            return new EnumeratorCommand(this);
        }
#else
        /// <summary>
        /// Collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorCommand Enumerator(CommandClientDefaultController controller)
        {
            return new EnumeratorCommand(controller);
        }
#endif
        /// <summary>
        /// Collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorCommand<T> DefaultEnumerator<T>()
        {
            return new AutoCSer.Net.EnumeratorCommand<T>(this);
        }
#if AOT
        /// <summary>
        /// Collection enumeration queue command
        /// 集合枚举队列命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand EnumeratorQueue()
        {
            return new EnumeratorQueueCommand(this);
        }
#else
        /// <summary>
        /// Collection enumeration queue command
        /// 集合枚举队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.EnumeratorQueueCommand EnumeratorQueue(CommandClientDefaultController controller)
        {
            return new EnumeratorQueueCommand(controller);
        }
#endif
        /// <summary>
        /// Collection enumeration queue command
        /// 集合枚举队列命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.EnumeratorQueueCommand<T> DefaultEnumeratorQueue<T>()
        {
            return new AutoCSer.Net.EnumeratorQueueCommand<T>(this);
        }
#if !AOT
        /// <summary>
        /// Throw an exception
        /// 抛出异常
        /// </summary>
        /// <param name="controller"></param>
        internal static void Throw(CommandClientDefaultController controller)
        {
            throw new Exception(controller.DefaultControllerReturnType.ToString());
        }
#endif

        /// <summary>
        /// 默认空控制器
        /// Default empty controller
        /// </summary>
        internal static CommandClientDefaultController Null = new CommandClientDefaultController();
        /// <summary>
        /// Get the empty controller
        /// 获取空控制器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        internal static CommandClientDefaultController GetNull(CommandClient client, string controllerName)
        {
            return Null;
        }
#if AOT
        /// <summary>
        /// AOT code generation template
        /// AOT 代码生成模板（历史方法）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal object DefaultControllerCallMethodName<T>(object callback)
        {
            return AutoCSer.Common.EmptyObject;
        }
        /// <summary>
        /// AOT code generation template
        /// AOT 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="callbacks"></param>
        /// <returns></returns>
        internal object DefaultControllerCallMethodName<T, KT>(params object[] callbacks)
        {
            return AutoCSer.Common.EmptyObject;
        }
#endif
    }
}
