using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Net
{
    /// <summary>
    /// Service controller creator
    /// 服务控制器创建器
    /// </summary>
    public abstract class CommandServerInterfaceControllerCreator
    {
        /// <summary>
        /// Controller name
        /// </summary>
        internal readonly string ControllerName;
        /// <summary>
        /// Controller creator
        /// </summary>
        /// <param name="controllerName"></param>
        internal CommandServerInterfaceControllerCreator(string controllerName)
        {
            ControllerName = controllerName;
        }
        /// <summary>
        /// Create a service controller
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal abstract CommandServerController Create(CommandListener server);
        /// <summary>
        /// Check the error information about the service controller
        /// 检查服务控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> Check();

        /// <summary>
        /// Gets the service interface type
        /// 获取服务接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type getInterfaceServiceType(Type type)
        {
            if (type.IsInterface) return type;
            Type[] interfaceTypes = type.GetInterfaces();
            switch (interfaceTypes.Length)
            {
                case 0: break;
                case 1: return interfaceTypes[0];
                default:
                    var attribute = type.GetCustomAttribute<CommandServerControllerAttribute>(true);
                    if (attribute?.InterfaceType != null)
                    {
                        if (attribute.InterfaceType.IsGenericTypeDefinition)
                        {
                            foreach (Type interfaceType in interfaceTypes)
                            {
                                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == attribute.InterfaceType) return interfaceType;
                            }
                        }
                        else
                        {
                            foreach (Type interfaceType in interfaceTypes)
                            {
                                if (interfaceType == attribute.InterfaceType) return interfaceType;
                            }
                        }
                        LogHelper.ErrorIgnoreException($"{type.fullName()} 没有找到接口实现 {attribute.InterfaceType.fullName()}");
                    }
                    foreach (Type interfaceType in interfaceTypes)
                    {
                        if (interfaceType.Name.Length == type.Name.Length + 1 && interfaceType.Name[0] == 'I' && interfaceType.Name.EndsWith(type.Name, StringComparison.Ordinal)) return interfaceType;
                    }
                    break;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
        /// <summary>
        /// Gets the controller creator
        /// 获取控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controller">Controller interface operation instance
        /// 控制器接口操作实例</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns></returns>
#if NetStandard21
        public static CommandServerInterfaceControllerCreator GetCreator<T>(T controller, string? controllerName = null)
#else
        public static CommandServerInterfaceControllerCreator GetCreator<T>(T controller, string controllerName = null)
#endif
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controller);
            return AutoCSer.Metadata.GenericType.Get(interfaceType).GetCommandServerInterfaceControllerCreator(controller.castObject(), controllerName);
        }
        /// <summary>
        /// Gets the controller creator
        /// 获取控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns></returns>
#if NetStandard21
        public static CommandServerInterfaceControllerCreator GetCreator<T>(Func<T> controllerCreator, string? controllerName = null)
#else
        public static CommandServerInterfaceControllerCreator GetCreator<T>(Func<T> controllerCreator, string controllerName = null)
#endif
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator);
            return AutoCSer.Metadata.BaseGenericType.Get(typeof(T), interfaceType).GetCommandServerInterfaceControllerCreator(controllerCreator, controllerName);
        }
        /// <summary>
        /// Gets the controller creator
        /// 获取控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns></returns>
#if NetStandard21
        public static CommandServerInterfaceControllerCreator GetCreator<T>(Func<CommandListener, T> controllerCreator, string? controllerName = null)
#else
        public static CommandServerInterfaceControllerCreator GetCreator<T>(Func<CommandListener, T> controllerCreator, string controllerName = null)
#endif
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator);
            return AutoCSer.Metadata.BaseGenericType.Get(typeof(T), interfaceType).GetCommandServerInterfaceControllerCreatorWithCommandListener(controllerCreator, controllerName);
        }
        /// <summary>
        /// Gets the controller creator
        /// 获取异步队列控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <typeparam name="KT">Asynchronous queue keyword type
        /// 异步队列关键字类型</typeparam>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <param name="controllerName">Controller name, default typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns></returns>
#if NetStandard21
        public static CommandServerInterfaceControllerCreator GetCreator<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string? controllerName = null)
#else
        public static CommandServerInterfaceControllerCreator GetCreator<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string controllerName = null)
#endif
            where KT : IEquatable<KT>
        {
            Type type = typeof(T);
            if (type.IsInterface) return new CommandServerInterfaceControllerCreator<T, KT>(controllerName, getTaskQueue);
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
        /// <summary>
        /// Gets the asymmetric controller creator defined
        /// 获取定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controller">Controller interface operation instance
        /// 控制器接口操作实例</param>
        /// <returns></returns>
        public static CommandServerInterfaceControllerCreator GetCreator<T>(string controllerName, T controller)
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(interfaceType);
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controller);
            return AutoCSer.Metadata.GenericType.Get(interfaceType).GetCommandServerInterfaceControllerCreator(controller.castObject(), controllerName);
        }
        /// <summary>
        /// Gets the asymmetric controller creator defined
        /// 获取定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <returns></returns>
        public static CommandServerInterfaceControllerCreator GetCreator<T>(string controllerName, Func<T> controllerCreator)
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(interfaceType);
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator);
            return AutoCSer.Metadata.BaseGenericType.Get(typeof(T), interfaceType).GetCommandServerInterfaceControllerCreator(controllerCreator, controllerName);
        }
        /// <summary>
        /// Gets the asymmetric controller creator defined
        /// 获取定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a controller interface operation instance delegate
        /// 创建控制器接口操作实例委托</param>
        /// <returns></returns>
        public static CommandServerInterfaceControllerCreator GetCreator<T>(string controllerName, Func<CommandListener, T> controllerCreator)
        {
            Type interfaceType = getInterfaceServiceType(typeof(T));
            if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(interfaceType);
            if (interfaceType == typeof(T)) return new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator);
            return AutoCSer.Metadata.BaseGenericType.Get(typeof(T), interfaceType).GetCommandServerInterfaceControllerCreatorWithCommandListener(controllerCreator, controllerName);
        }
        /// <summary>
        /// Gets the asymmetric controller creator defined
        /// 获取异步队列定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Controller interface type
        /// 控制器接口类型</typeparam>
        /// <typeparam name="KT">Asynchronous queue keyword type
        /// 异步队列关键字类型</typeparam>
        /// <param name="controllerName">Controller name. The default MethodIndexEnumType type name is used
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <returns></returns>
        public static CommandServerInterfaceControllerCreator GetCreator<T, KT>(string controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue)
            where KT : IEquatable<KT>
        {
            Type type = typeof(T);
            if (type.IsInterface)
            {
                if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(type);
                return new CommandServerInterfaceControllerCreator<T, KT>(controllerName, getTaskQueue);
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
    }
    /// <summary>
    /// Service Controller Creator
    /// 服务控制器创建器
    /// </summary>
    /// <typeparam name="T">Controller interface type
    /// 控制器接口类型</typeparam>
    internal sealed class CommandServerInterfaceControllerCreator<T> : CommandServerInterfaceControllerCreator
    {
        /// <summary>
        /// Create the controller interface instance delegate
        /// 创建控制器接口实例委托
        /// </summary>
        private readonly Func<CommandListener, T> controllerCreator;
        /// <summary>
        /// Is delegate parameters
        /// 是否委托参数
        /// </summary>
        private readonly bool isFunc;
        /// <summary>
        /// Controller creator
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controller"></param>
#if NetStandard21
        internal CommandServerInterfaceControllerCreator(string? controllerName, T controller) : base(controllerName ?? typeof(T).FullName.notNull())
#else
        internal CommandServerInterfaceControllerCreator(string controllerName, T controller) : base(controllerName ?? typeof(T).FullName)
#endif
        {
            controllerCreator = server => controller;
            isFunc = false;
        }
        /// <summary>
        /// Controller creator
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controllerCreator"></param>
#if NetStandard21
        internal CommandServerInterfaceControllerCreator(string? controllerName, Func<T> controllerCreator) : base(controllerName ?? typeof(T).FullName.notNull())
#else
        internal CommandServerInterfaceControllerCreator(string controllerName, Func<T> controllerCreator) : base(controllerName ?? typeof(T).FullName)
#endif
        {
            this.controllerCreator = server => controllerCreator();
            isFunc = true;
        }
        /// <summary>
        /// Controller creator
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controllerCreator"></param>
#if NetStandard21
        internal CommandServerInterfaceControllerCreator(string? controllerName, Func<CommandListener, T> controllerCreator) : base(controllerName ?? typeof(T).FullName.notNull())
#else
        internal CommandServerInterfaceControllerCreator(string controllerName, Func<CommandListener, T> controllerCreator) : base(controllerName ?? typeof(T).FullName)
#endif
        {
            this.controllerCreator = controllerCreator;
            isFunc = true;
        }
        /// <summary>
        /// Create a service controller
        /// 创建服务控制器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal override CommandServerController Create(CommandListener server)
        {
            CommandServerController commandServerController;
            T controller = controllerCreator(server);
            if (controller is CommandServerBindContextController)
            {
                if (!isFunc) throw new InvalidOperationException(AutoCSer.Common.Culture.CommandServerControllerNotFoundConstructDelegate);
                commandServerController = ServerInterfaceController<T>.Create(server, ControllerName, controller, new GetBindContextController(server, controllerCreator).Get);
            }
            else
            {
                commandServerController = ServerInterfaceController<T>.Create(server, ControllerName, controller, null);
                var bindController = controller as ICommandServerBindController;
                if (bindController != null) bindController.Bind(commandServerController);
            }
            return commandServerController;
        }
        /// <summary>
        /// Check the error messages related to the service controller
        /// 检查服务控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> Check() { return ServerInterfaceController<T>.Check(); }

        /// <summary>
        /// Gets the controller interface instance
        /// 获取控制器接口实例
        /// </summary>
        private sealed class GetBindContextController
        {
            /// <summary>
            /// Command server to listen
            /// 命令服务端监听
            /// </summary>
            private readonly CommandListener server;
            /// <summary>
            /// Create the controller interface instance delegate
            /// 创建控制器接口实例委托
            /// </summary>
            private readonly Func<CommandListener, T> controllerCreator;
            /// <summary>
            /// Gets the controller interface instance
            /// 获取控制器接口实例
            /// </summary>
            /// <param name="server"></param>
            /// <param name="controllerCreator"></param>
            internal GetBindContextController(CommandListener server, Func<CommandListener, T> controllerCreator) 
            {
                this.server = server;
                this.controllerCreator = controllerCreator;
            }
            /// <summary>
            /// Gets the controller interface instance
            /// 获取控制器接口实例
            /// </summary>
            /// <param name="commandServerController"></param>
            /// <param name="socket"></param>
            /// <returns></returns>
            internal CommandServerBindContextController Get(CommandServerController commandServerController, CommandServerSocket socket)
            {
                T controller = controllerCreator(server);
                CommandServerBindContextController service = (controller as CommandServerBindContextController).notNull();
                if (object.ReferenceEquals(service.Socket, CommandServerSocket.CommandServerSocketContext))
                {
                    service.Set(socket, commandServerController);
                    return service;
                }
                throw new InvalidOperationException(AutoCSer.Common.Culture.CommandServerControllerBound);
            }
        }
    }
    /// <summary>
    /// Task Queue Service Controller Creator
    /// Task 队列服务控制器创建器
    /// </summary>
    /// <typeparam name="T">Controller interface type
    /// 控制器接口类型</typeparam>
    /// <typeparam name="KT">Asynchronous queue keyword type
    /// 异步队列关键字类型</typeparam>
    internal sealed class CommandServerInterfaceControllerCreator<T, KT> : CommandServerInterfaceControllerCreator
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// Gets the queue context delegate
        /// 获取队列上下文委托
        /// </summary>
        private readonly Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue;
        /// <summary>
        /// Controller creator
        /// 控制器创建器
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="getTaskQueue"></param>
#if NetStandard21
        internal CommandServerInterfaceControllerCreator(string? controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue) : base(controllerName ?? $"{typeof(T).FullName}+{typeof(KT).FullName}")
#else
        internal CommandServerInterfaceControllerCreator(string controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue) : base(controllerName ?? $"{typeof(T).FullName}+{typeof(KT).FullName}")
#endif
        {
            this.getTaskQueue = getTaskQueue;
        }
        /// <summary>
        /// Create a service controller
        /// 创建服务控制器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal override CommandServerController Create(CommandListener server)
        {
            var controllerCreator = server.TaskQueueSet.Get<KT>().notNull().Set(this);
            if (controllerCreator == null)
            {
                return ServerTaskQueueInterfaceController<T, KT>.Create(server, ControllerName, getTaskQueue);
            }
            throw new Exception(AutoCSer.Common.Culture.GetCommandServerTaskQueueKeyTypeRepeatedly(ControllerName, controllerCreator.ControllerName, typeof(KT)));
        }
        /// <summary>
        /// Check the error messages related to the service controller
        /// 检查服务控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> Check() { return ServerTaskQueueInterfaceController<T, KT>.Check(); }
    }
}
