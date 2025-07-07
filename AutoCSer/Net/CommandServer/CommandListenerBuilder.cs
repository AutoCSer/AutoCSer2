using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Create command server listeners
    /// 创建命令服务端监听
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandListenerBuilder
    {
        /// <summary>
        /// Collection of service controller creators
        /// 服务控制器创建器集合
        /// </summary>
        internal LeftArray<CommandServerInterfaceControllerCreator> creators;
        /// <summary>
        /// Collection of service controller creators
        /// 服务控制器创建器集合
        /// </summary>
        internal IEnumerable<CommandServerInterfaceControllerCreator> Creators 
        {
            get
            {
                foreach (CommandServerInterfaceControllerCreator creator in creators) yield return creator;
            }
        }
        /// <summary>
        /// Create command server listeners
        /// 创建命令服务端监听
        /// </summary>
        /// <param name="creatorCapacity">The service controller creator collection initializes the container size
        /// 服务控制器创建器集合容器初始化大小</param>
        public CommandListenerBuilder(int creatorCapacity)
        {
            creators = new LeftArray<CommandServerInterfaceControllerCreator>(creatorCapacity);
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandListenerBuilder AppendInstance<T>(T controller, string? controllerName = null)
#else
        public CommandListenerBuilder AppendInstance<T>(T controller, string controllerName = null)
#endif
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controller, controllerName));
            return this;
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
#if NetStandard21
        public CommandListenerBuilder Append<T>(T controller, string? controllerName = null)
#else
        public CommandListenerBuilder Append<T>(T controller, string controllerName = null)
#endif
        {
            if (typeof(T).IsInterface)
            {
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controller));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
        }

        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandListenerBuilder AppendInstance<T>(Func<T> controllerCreator, string? controllerName = null)
#else
        public CommandListenerBuilder AppendInstance<T>(Func<T> controllerCreator, string controllerName = null)
#endif
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerCreator, controllerName));
            return this;
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
#if NetStandard21
        public CommandListenerBuilder Append<T>(Func<T> controllerCreator, string? controllerName = null)
#else
        public CommandListenerBuilder Append<T>(Func<T> controllerCreator, string controllerName = null)
#endif
        {
            if (typeof(T).IsInterface)
            {
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandListenerBuilder AppendInstance<T>(Func<CommandListener, T> controllerCreator, string? controllerName = null)
#else
        public CommandListenerBuilder AppendInstance<T>(Func<CommandListener, T> controllerCreator, string controllerName = null)
#endif
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerCreator, controllerName));
            return this;
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
#if NetStandard21
        public CommandListenerBuilder Append<T>(Func<CommandListener, T> controllerCreator, string? controllerName = null)
#else
        public CommandListenerBuilder Append<T>(Func<CommandListener, T> controllerCreator, string controllerName = null)
#endif
        {
            if (typeof(T).IsInterface)
            {
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
        }
        /// <summary>
        /// Add the controller creator
        /// 添加控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <typeparam name="KT">Queue keyword type
        /// 队列关键字类型</typeparam>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <param name="controllerName">Controller name, default is typeof(T).FullName
        /// 控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandListenerBuilder Append<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string? controllerName = null)
#else
        public CommandListenerBuilder Append<T, KT>(Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue, string controllerName = null)
#endif
            where KT : IEquatable<KT>
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(getTaskQueue, controllerName));
            return this;
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandListenerBuilder AppendInstance<T>(string controllerName, T controller)
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerName, controller));
            return this;
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controller">Controller interface operation example
        /// 控制器接口操作实例</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        public CommandListenerBuilder Append<T>(string controllerName, T controller)
        {
            Type type = typeof(T);
            if (type.IsInterface)
            {
                if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(type);
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controller));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandListenerBuilder AppendInstance<T>(string controllerName, Func<T> controllerCreator)
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerName, controllerCreator));
            return this;
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        public CommandListenerBuilder Append<T>(string controllerName, Func<T> controllerCreator)
        {
            Type type = typeof(T);
            if (type.IsInterface)
            {
                if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(type);
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service instance type, specify the service interface type through AutoCSer.Net.CommandServerControllerAttribute.InterfaceType
        /// 服务实例类型，通过 AutoCSer.Net.CommandServerControllerAttribute.InterfaceType 指定服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandListenerBuilder AppendInstance<T>(string controllerName, Func<CommandListener, T> controllerCreator)
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerName, controllerCreator));
            return this;
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="controllerCreator">Create a delegate of controller interface operation instances
        /// 创建控制器接口操作实例委托</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        public CommandListenerBuilder Append<T>(string controllerName, Func<CommandListener, T> controllerCreator)
        {
            Type type = typeof(T);
            if (type.IsInterface)
            {
                if (string.IsNullOrEmpty(controllerName)) controllerName = CommandServerControllerInterfaceAttribute.GetControllerName(type);
                creators.Add(new CommandServerInterfaceControllerCreator<T>(controllerName, controllerCreator));
                return this;
            }
            throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(type));
        }
        /// <summary>
        /// Add the definition of the asymmetric controller creator
        /// 添加定义非对称控制器创建器
        /// </summary>
        /// <typeparam name="T">Service interface type
        /// 服务接口类型</typeparam>
        /// <typeparam name="KT">Queue keyword type
        /// 队列关键字类型</typeparam>
        /// <param name="controllerName">The controller name, by default, is of type MethodIndexEnumType
        /// 控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="getTaskQueue">Gets the queue context delegate
        /// 获取队列上下文委托</param>
        /// <returns>Controller creator
        /// 控制器创建器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandListenerBuilder Append<T, KT>(string controllerName, Func<AutoCSer.Net.CommandServerCallTaskQueueNode, KT, T> getTaskQueue)
            where KT : IEquatable<KT>
        {
            creators.Add(CommandServerInterfaceControllerCreator.GetCreator(controllerName, getTaskQueue));
            return this;
        }
        /// <summary>
        /// Create command server listeners
        /// 创建命令服务端监听
        /// </summary>
        /// <param name="commandServerConfig"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CommandListener CreateCommandListener(AutoCSer.Net.CommandServerConfig commandServerConfig)
        {
            return new AutoCSer.Net.CommandListener(commandServerConfig, ref creators);
        }
        /// <summary>
        /// Create a reverse command service client
        /// 创建反向命令服务客户端
        /// </summary>
        /// <param name="commandServerConfig"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AutoCSer.Net.CommandReverseClient CreateCommandListener(AutoCSer.Net.CommandReverseClientConfig commandServerConfig)
        {
            return new AutoCSer.Net.CommandReverseClient(commandServerConfig, ref creators);
        }
    }
}
