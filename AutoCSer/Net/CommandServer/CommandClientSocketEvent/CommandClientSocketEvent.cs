using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;
using AutoCSer.Net.CommandServer;
using AutoCSer.Extensions;
using System.Threading.Tasks;
using System.Net;
using AutoCSer.Threading;
using System.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net
{
    /// <summary>
    /// Default command client socket event
    /// 默认命令客户端套接字事件
    /// </summary>
    public class CommandClientSocketEvent
    {
        /// <summary>
        /// Verify the successfully completed tasks
        /// 验证成功的已完成任务
        /// </summary>
        private static readonly Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> verifySuccessTask = Task.FromResult((CommandClientReturnValue<CommandServerVerifyStateEnum>)CommandServerVerifyStateEnum.Success);
        /// <summary>
        /// Verify the completed tasks that failed
        /// 验证失败的已完成任务
        /// </summary>
        private static readonly Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> verifyFailTask = Task.FromResult((CommandClientReturnValue<CommandServerVerifyStateEnum>)CommandServerVerifyStateEnum.Fail);
        /// <summary>
        /// The completed tasks that failed to verify and need to be retried
        /// 验证失败需重试的已完成任务
        /// </summary>
        private static readonly Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> verifyRetryTask = Task.FromResult((CommandClientReturnValue<CommandServerVerifyStateEnum>)CommandServerVerifyStateEnum.Retry);
        /// <summary>
        /// The completed task of verifying the function logic is lacking
        /// 缺少验证函数逻辑的已完成任务
        /// </summary>
        private static readonly Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> lessVerifyMethodTask = Task.FromResult((CommandClientReturnValue<CommandServerVerifyStateEnum>)CommandServerVerifyStateEnum.LessVerifyMethod);
        /// <summary>
        /// Get the completed task based on the validation status
        /// 根据验证状态获取已完成任务
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected static Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> getCompletedTask(CommandClientReturnValue<CommandServerVerifyStateEnum> state)
        {
            if (state.IsSuccess)
            {
                switch (state.Value)
                {
                    case CommandServerVerifyStateEnum.Fail: return verifyFailTask;
                    case CommandServerVerifyStateEnum.Success: return verifySuccessTask;
                    case CommandServerVerifyStateEnum.Retry: return verifyRetryTask;
                    case CommandServerVerifyStateEnum.LessVerifyMethod: return lessVerifyMethodTask;
                }
            }
            return Task.FromResult(state);
        }

        /// <summary>
        /// Command client
        /// 命令客户端
        /// </summary>
        public readonly CommandClient Client;
        /// <summary>
        /// Set the client controller delegate
        /// 设置客户端控制器委托
        /// </summary>
#if NetStandard21
        private readonly Action<CommandClientSocketEvent>? setController;
#else
        private readonly Action<CommandClientSocketEvent> setController;
#endif
        /// <summary>
        /// By authenticating the API's current command client socket, this object changes as disconnection reconnects
        /// 通过认证 API 的当前命令客户端套接字，该对象会随着断线重连而变化
        /// </summary>
#if NetStandard21
        public CommandClientSocket? Socket { get; private set; }
#else
        public CommandClientSocket Socket { get; private set; }
#endif
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
#if NetStandard21
        public virtual IEnumerable<CommandClientControllerCreatorParameter>? ControllerCreatorParameters { get { return null; } }
#else
        public virtual IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters { get { return null; } }
#endif
        /// <summary>
        /// Release the closed socket version of the waiting lock
        /// 释放等待锁的关闭套接字版本
        /// </summary>
        private int closeSocketVersion;
        /// <summary>
        /// Wait for the collection of client socket locks
        /// 等待客户端套接字锁集合
        /// </summary>
        private LeftArray<System.Threading.SemaphoreSlim> socketWaitLocks;
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="commandClient">Command client
        /// 命令客户端</param>
        public CommandClientSocketEvent(CommandClient commandClient)
        {
            Client = commandClient;
            socketWaitLocks.SetEmpty();

            var controllerCreatorParameters = ControllerCreatorParameters;
            if (controllerCreatorParameters != null)
            {
                Type type = GetType();
                Dictionary<HashObject<System.Type>, PropertyInfo> propertys = DictionaryCreator.CreateHashObject<System.Type, PropertyInfo>();
                Dictionary<string, PropertyInfo> propertyNames = DictionaryCreator<string>.Create<PropertyInfo>();
                foreach (PropertyInfo property in type.GetProperties(commandClient.Config.ControllerCreatorBindingFlags))
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        Type propertyType = property.PropertyType;
                        if (propertyType.IsInterface)
                        {
                            if (property.GetIndexParameters().Length == 0)//!propertys.ContainsKey(propertyType)
                            {
                                propertyNames.Add(property.Name, property);
                                propertys[propertyType] = property;
                            }
                        }
#if !AOT
                        else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(TaskQueueClientController<,>))
                        {
                            propertyNames.Add(property.Name, property);
                            propertys[propertyType.GetGenericArguments()[0]] = property;
                        }
#endif
                    }
                }
#if AOT
                LeftArray<SetClientControllerMethod> propertyMethods = new LeftArray<SetClientControllerMethod>(propertyNames.Count + propertys.Count);
#else
                SetClientControllerDynamicMethod setClientControllerDynamicMethod = new SetClientControllerDynamicMethod(type);
                bool isProperty = false;
#endif
                object[] defaultControllerParameters = EmptyArray<object>.Array;
                foreach (CommandClientControllerCreatorParameter parameter in controllerCreatorParameters)
                {
                    var property = default(PropertyInfo);
                    var propertyName = parameter.PropertyName;
                    if (string.IsNullOrEmpty(propertyName)
                        ? propertys.TryGetValue(parameter.ClientInterfaceType, out property)
                        : propertyNames.TryGetValue(propertyName, out property))
                    {
#if AOT
                        propertyMethods.Add(new SetClientControllerMethod(parameter.GetControllerName(), property.GetSetMethod(true).notNull()));
#else
                        setClientControllerDynamicMethod.Push(property, parameter.GetControllerName());
                        isProperty = true;
#endif
                        if (Client.Config.IsDefaultController)
                        {
                            CommandClientInterfaceControllerCreator creator = parameter.Creator;
                            if (creator != null && !creator.IsTaskQueue)
                            {
                                if (defaultControllerParameters.Length == 0) defaultControllerParameters = new object[1];
                                defaultControllerParameters[0] = creator.CreateDefault(commandClient);
                                property.SetMethod.notNull().Invoke(this, defaultControllerParameters);
                            }
                        }
                    }
                }
#if AOT
                if (propertyMethods.Length != 0) setController = (Action<CommandClientSocketEvent>)new SetClientController(propertyMethods).Set;
#else
                if (isProperty) setController = (Action<CommandClientSocketEvent>)setClientControllerDynamicMethod.Create(typeof(Action<CommandClientSocketEvent>));
#endif
            }
        }
        /// <summary>
        /// Add the collection of client controller creators
        /// 添加客户端控制器创建器集合
        /// </summary>
        /// <param name="controllerCreators"></param>
        internal void AppendCreators(ref LeftArray<CommandClientInterfaceControllerCreator> controllerCreators)
        {
            var controllerCreatorParameters = ControllerCreatorParameters;
            if (controllerCreatorParameters != null)
            {
                foreach (CommandClientControllerCreatorParameter controllerCreatorParameter in controllerCreatorParameters)
                {
                    CommandClientInterfaceControllerCreator creator = controllerCreatorParameter.Creator;
                    if (creator == null)
                    {
                        throw new InvalidCastException(AutoCSer.Common.Culture.GetCommandClientControllerCreateFailed(controllerCreatorParameter.ClientInterfaceType, controllerCreatorParameter.ServerInterfaceType));
                    }
                    foreach (CommandClientInterfaceControllerCreator controllerCreator in controllerCreators)
                    {
                        if (controllerCreator.ControllerName == creator.ControllerName)
                        {
                            throw new Exception(AutoCSer.Common.Culture.GetCommandClientControllerNameRepeatedly(creator.ControllerName));
                        }
                    }
                    var controllerConstructorException = creator.ControllerConstructorException;
                    if (controllerConstructorException != null) throw controllerConstructorException;
                    controllerCreators.Add(creator);
                }
            }
        }
        /// <summary>
        /// Gets the command client controller
        /// 获取命令客户端控制器
        /// </summary>
        /// <param name="socketEvent"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandClientController? GetController(CommandClientSocketEvent socketEvent, string controllerName)
#else
        internal static CommandClientController GetController(CommandClientSocketEvent socketEvent, string controllerName)
#endif
        {
            return socketEvent.Socket.notNull()[controllerName];
        }

        /// <summary>
        /// By default, the system hibernates for 10ms after the first failed socket creation, 100ms after the second failed socket creation, and 1s after the third failed socket creation. After each failed socket creation, the system hibernates for 5s
        /// 创建套接字失败重试休眠，默认第 1 次失败以后休眠 10ms，第 2 次失败以后休眠 100ms，第 3 次失败以后休眠 1s，以后每次失败都休眠 5s
        /// </summary>
        /// <param name="createErrorCount">Number of failures
        /// 失败次数</param>
        /// <returns></returns>
        public virtual Task CreateSocketSleep(int createErrorCount)
        {
            switch (createErrorCount)
            {
                case 0: return AutoCSer.Common.CompletedTask;
                case 1: return Task.Delay(10);
                case 2: return Task.Delay(100);
                case 3: return Task.Delay(1000);
                default: return Task.Delay(5000);
            }
        }
        /// <summary>
        /// The notification of the client controller name was not found
        /// 没有找到客户端控制器名称通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public virtual Task NotFoundControllerName(CommandClientSocket socket, string controllerName)
        {
            return Client.Log.Debug($"没有找到客户端控制器名称 {controllerName}", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn);
        }
        /// <summary>
        /// The notification of the server controller name was not found
        /// 没有找到服务端控制器名称通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public virtual Task NotFoundServerControllerName(CommandClientSocket socket, string controllerName)
        {
            return Client.Log.Error($"没有找到服务端控制器名称 {controllerName}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
        }
        /// <summary>
        /// Notification when the name of the main controller does not match the name of the server
        /// 主控制器名称与服务端名称不匹配通知
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="controllerName"></param>
        public virtual void ControllerNameError(CommandClientController controller, string controllerName)
        {
            Client.Log.DebugIgnoreException($"主控制器名称 {controller.ControllerName} 与服务端名称 {controllerName} 不匹配通知", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn);
        }
        ///// <summary>
        ///// 控制器方法数量超出服务端限制通知
        ///// </summary>
        ///// <param name="controller"></param>
        //public virtual void ControllerMethodCountError(CommandClientController controller)
        //{
        //    Client.Log.DebugIgnoreException($"客户端控制器方法数量 {controller.Methods.Length.toString()} 超出服务端限制 {controller.MaxMethodCount.toString()}", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn);
        //}
        /// <summary>
        /// Socket retry connection successful prompt
        /// 套接字重试连接成功提示
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="serverEndPoint"></param>
        /// <param name="exceptionCount">Number of abnormal errors
        /// 异常错误次数</param>
        /// <returns></returns>
#if NetStandard21
        public virtual Task OnCreateSocketRetrySuccess(CommandClientSocket? socket, IPEndPoint serverEndPoint, int exceptionCount)
#else
        public virtual Task OnCreateSocketRetrySuccess(CommandClientSocket socket, IPEndPoint serverEndPoint, int exceptionCount)
#endif
        {
            if (exceptionCount != 0)
            {
                return Client.Log.Debug(Client.ServerName + " 客户端 TCP 连接成功 " + serverEndPoint.ToString(), LogLevelEnum.Debug | LogLevelEnum.AutoCSer);
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Socket creation exception prompt
        /// 套接字创建异常提示
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="exception"></param>
        /// <param name="serverEndPoint"></param>
        /// <param name="exceptionCount">Number of abnormal errors
        /// 异常错误次数</param>
        /// <returns></returns>
#if NetStandard21
        public virtual Task OnCreateSocketException(CommandClientSocket? socket, Exception exception, IPEndPoint serverEndPoint, int exceptionCount)
#else
        public virtual Task OnCreateSocketException(CommandClientSocket socket, Exception exception, IPEndPoint serverEndPoint, int exceptionCount)
#endif
        {
            if (exceptionCount == 1) return Client.Log.Exception(exception, Client.ServerName + " 客户端 TCP 连接失败 " + serverEndPoint.ToString(), LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public virtual Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return controller.VerifyMethodIndex < 0 ? verifySuccessTask : lessVerifyMethodTask;
        }
        /// <summary>
        /// The release of the waiting client socket lock must be invoked during the client lock operation
        /// 释放等待客户端套接字锁，必须在客户端锁操作中调用
        /// </summary>
        internal void ReleaseSocketWaitLock()
        {
            foreach (System.Threading.SemaphoreSlim socketWaitLock in socketWaitLocks) socketWaitLock.Release();
            socketWaitLocks.SetEmpty();
        }
        /// <summary>
        /// Get the wait client socket lock, this call is in the client lock operation, should not complete the initialization operation as soon as possible, forbidden to call the internal nested lock operation to avoid deadlock
        /// 获取等待客户端套接字锁，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <returns></returns>
        internal System.Threading.SemaphoreSlim GetWaitLock()
        {
            System.Threading.SemaphoreSlim waitLock = new System.Threading.SemaphoreSlim(0, 1);
            socketWaitLocks.Add(waitLock);
            return waitLock;
        }
        /// <summary>
        /// Set up the client controller
        /// 设置客户端控制器
        /// </summary>
        /// <param name="socket"></param>
#if NET8
        [MemberNotNull(nameof(Socket))]
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetController(CommandClientSocket socket)
        {
            Socket = socket;
            if (setController != null) setController(this);
        }
        /// <summary>
        /// The reverse command server client listens for the authentication socket
        /// 反向命令服务客户端监听验证套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual Task<bool> CallVerify(CommandClientSocket socket)
        {
            return AutoCSer.Common.GetCompletedTask(true);
        }
        /// <summary>
        /// Command Client socket custom client initialization operations after the authentication API is passed and the client controller is automatically bound, used to manually bind the client controller Settings and connection initialization operations, such as the initial keep callback. This call is located in the client lock operation, should not complete the initialization operation as soon as possible, do not call the internal nested lock operation to avoid deadlock
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public virtual Task OnMethodVerified(CommandClientSocket socket) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// The default operation is to notify the caller waiting for a connection. The same command client socket object may be called multiple times. The call is located in the client lock operation. The initialization operation should not be completed as soon as possible
        /// 命令客户端套接字创建连接失败通知，默认操作为通知等待连接的调用者，同一个命令客户端套接字对象可能存在多次调用，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public virtual void OnCreateError(CommandClientSocket socket)
        {
            ReleaseSocketWaitLock();
        }
        /// <summary>
        /// Release the waiting client socket lock and close the socket only once, and it must be called during the client lock operation
        /// 释放等待客户端套接字锁，关闭套接字仅执行一次，必须在客户端锁操作中调用
        /// </summary>
        /// <param name="closeSocketVersion">Release the closed socket version of the waiting lock
        /// 释放等待锁的关闭套接字版本</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void releaseSocketWaitLock(int closeSocketVersion)
        {
            if (closeSocketVersion > this.closeSocketVersion)
            {
                ReleaseSocketWaitLock();
                this.closeSocketVersion = closeSocketVersion;
            }
        }
        /// <summary>
        /// Disable command client current socket notification, the default action is to notify the caller waiting for the current connection, this call is located in the client lock operation, should not complete initialization operation as soon as possible, do not call internal nested lock operation to avoid deadlock
        /// 关闭命令客户端当前套接字通知，默认操作为通知等待当前连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public virtual void OnClosed(CommandClientSocket socket)
        {
            releaseSocketWaitLock(socket.CreateVersion);
        }
        /// <summary>
        /// Re-create a new socket notification, the default action is to notify the caller waiting for the current connection, this call is located in the client lock operation, should not complete the initialization operation as soon as possible, do not call the internal nested lock operation to avoid deadlock
        /// 重新创建新的套接字通知，默认操作为通知等待当前连接的调用者，此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="errorSocket"></param>
        public virtual void Create(CommandClientSocket socket, CommandClientSocket errorSocket)
        {
            releaseSocketWaitLock(errorSocket.CreateVersion);
        }
        /// <summary>
        /// Turn off client notifications. The default action is to notify callers waiting for a connection. This call is in the client lock operation
        /// 关闭客户端通知，默认操作为通知等待连接的调用者，此调用位于客户端锁操作中，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        public virtual void OnDisposeClient()
        {
            ReleaseSocketWaitLock();
        }
    }
    /// <summary>
    /// 默认命令客户端套接字事件
    /// Default command client socket event
    /// </summary>
    /// <typeparam name="T">Primary controller interface type of the client
    /// 客户端主控制器接口类型</typeparam>
    public class CommandClientSocketEvent<T> : CommandClientSocketEvent
        where T : class
    {
        /// <summary>
        /// Current socket command client controller
        /// 当前套接字命令客户端控制器
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T InterfaceController { get; private set; }
        /// <summary>
        /// Symmetric interface definition
        /// 是否对称接口定义
        /// </summary>
        public bool IsSymmetryInterface;
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                if (IsSymmetryInterface) yield return new CommandClientControllerCreatorParameter(typeof(T));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isSymmetryInterface">Symmetric interface definition
        /// 是否对称接口定义</param>
        public CommandClientSocketEvent(CommandClient client, bool isSymmetryInterface = false) : base(client) 
        {
            IsSymmetryInterface = isSymmetryInterface;
        }
        /// <summary>
        /// The current socket is used for manually binding client controller Settings and connection initialization operations, such as initialization keep callbacks, via validation methods. This call is located in the client lock operation, should not complete the initialization operation as soon as possible, do not call the internal nested lock operation to avoid deadlock
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        public override Task OnMethodVerified(CommandClientSocket socket)
        {
            InterfaceController = (Socket.notNull().Controller as T).notNull();
            return AutoCSer.Common.CompletedTask;
        }
    }
}