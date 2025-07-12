using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端控制器创建器
    /// </summary>
    public abstract class CommandClientInterfaceControllerCreator
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        internal readonly string ControllerName;
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        internal abstract Exception? ControllerConstructorException { get; }
#else
        internal abstract Exception ControllerConstructorException { get; }
#endif
        /// <summary>
        /// 是否服务端 Task 队列客户端控制器
        /// </summary>
        internal abstract bool IsTaskQueue { get; }
        /// <summary>
        /// 控制器创建器
        /// </summary>
        /// <param name="controllerName"></param>
        internal CommandClientInterfaceControllerCreator(string controllerName)
        {
            ControllerName = controllerName;
        }
        /// <summary>
        /// 创建客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string?[]? serverMethodNames);
#else
        internal abstract CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string[] serverMethodNames);
#endif
        /// <summary>
        /// 创建客户端默认控制器
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal abstract CommandClientDefaultController CreateDefault(CommandClient client);
        /// <summary>
        /// 检查客户端控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> Check();

        /// <summary>
        /// 获取客户端控制器创建器
        /// </summary>
        /// <typeparam name="T">客户端接口类型</typeparam>
        /// <param name="controllerName">控制器名称</param>
        /// <returns>客户端控制器创建器</returns>
        public static CommandClientInterfaceControllerCreator GetClientCreator<T>(string controllerName)
        {
            if (!typeof(T).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
            if(string.IsNullOrEmpty(controllerName)) throw new ArgumentNullException(AutoCSer.Common.Culture.CommandClientControllerEmptyName);
            return new CommandClientInterfaceControllerCreator<T, ServerInterface>(controllerName);
        }
        /// <summary>
        /// 获取接口对称服务客户端控制器创建器
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="controllerName">控制器名称，默认为 typeof(T).FullName</param>
        /// <returns>客户端控制器创建器</returns>
#if NetStandard21
        public static CommandClientInterfaceControllerCreator GetSymmetryCreator<T>(string? controllerName = null)
#else
        public static CommandClientInterfaceControllerCreator GetSymmetryCreator<T>(string controllerName = null)
#endif
        {
            if (!typeof(T).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
            return new CommandClientInterfaceControllerCreator<T, T>(controllerName);
        }
        /// <summary>
        /// 获取客户端控制器创建器
        /// </summary>
        /// <typeparam name="T">客户端接口类型</typeparam>
        /// <typeparam name="ST">服务端接口类型</typeparam>
        /// <param name="controllerName">控制器名称，默认为 typeof(ST).FullName</param>
        /// <returns>客户端控制器创建器</returns>
#if NetStandard21
        public static CommandClientInterfaceControllerCreator GetCreator<T, ST>(string? controllerName = null)
#else
        public static CommandClientInterfaceControllerCreator GetCreator<T, ST>(string controllerName = null)
#endif
        {
            if (!typeof(T).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
            if (!typeof(ST).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(ST)));
            return new CommandClientInterfaceControllerCreator<T, ST>(controllerName);
        }
#if !AOT
        /// <summary>
        /// 获取服务端 Task 队列客户端控制器创建器
        /// </summary>
        /// <typeparam name="T">客户端接口类型</typeparam>
        /// <typeparam name="KT">Task 队列关键字类型</typeparam>
        /// <param name="controllerName">控制器名称</param>
        /// <returns>服务端 Task 队列客户端控制器创建器</returns>
#if NetStandard21
        public static CommandClientInterfaceControllerCreator GetTaskQueueClientCreator<T, KT>(string? controllerName)
#else
        public static CommandClientInterfaceControllerCreator GetTaskQueueClientCreator<T, KT>(string controllerName)
#endif
            where KT : IEquatable<KT>
        {
            if (!typeof(T).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
            if (string.IsNullOrEmpty(controllerName)) throw new ArgumentNullException(AutoCSer.Common.Culture.CommandClientControllerEmptyName);
            return new CommandClientInterfaceControllerCreator<T, ServerInterface, KT>(controllerName);
        }
        /// <summary>
        /// 获取服务端 Task 队列客户端控制器创建器
        /// </summary>
        /// <typeparam name="T">客户端接口类型</typeparam>
        /// <typeparam name="ST">服务端接口类型</typeparam>
        /// <typeparam name="KT">Task 队列关键字类型</typeparam>
        /// <param name="controllerName">控制器名称，默认为 typeof(ST).FullName+typeof(KT).FullName</param>
        /// <returns>服务端 Task 队列客户端控制器创建器</returns>
#if NetStandard21
        public static CommandClientInterfaceControllerCreator GetTaskQueueCreator<T, ST, KT>(string? controllerName = null)
#else
        public static CommandClientInterfaceControllerCreator GetTaskQueueCreator<T, ST, KT>(string controllerName = null)
#endif
            where KT : IEquatable<KT>
        {
            if (!typeof(T).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(T)));
            if (!typeof(ST).IsInterface) throw new Exception(AutoCSer.Common.Culture.GetNotInterfaceType(typeof(ST)));
            return new CommandClientInterfaceControllerCreator<T, ST, KT>(controllerName);
        }
#endif
    }
    /// <summary>
    /// 客户端控制器创建器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="ST"></typeparam>
    internal sealed class CommandClientInterfaceControllerCreator<T, ST> : CommandClientInterfaceControllerCreator
    {
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        internal override Exception? ControllerConstructorException { get { return ClientInterfaceController<T, ST>.ControllerConstructorException; } }
#else
        internal override Exception ControllerConstructorException { get { return ClientInterfaceController<T, ST>.ControllerConstructorException; } }
#endif
        /// <summary>
        /// 是否服务端 Task 队列客户端控制器
        /// </summary>
        internal override bool IsTaskQueue { get { return false; } }
        /// <summary>
        /// 控制器创建器
        /// </summary>
        /// <param name="controllerName"></param>
#if NetStandard21
        internal CommandClientInterfaceControllerCreator(string? controllerName)
#else
        internal CommandClientInterfaceControllerCreator(string controllerName)
#endif
            : base(controllerName ?? typeof(ST).FullName.notNull()) { }
        /// <summary>
        /// 创建服务控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
#if NetStandard21
        internal override CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string?[]? serverMethodNames)
#else
        internal override CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string[] serverMethodNames)
#endif
        {
            return ClientInterfaceController<T, ST>.Create(socket, ControllerName, startMethodIndex, serverMethodNames);
        }
        /// <summary>
        /// 创建客户端默认控制器
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal override CommandClientDefaultController CreateDefault(CommandClient client)
        {
            return ClientInterfaceController<T>.Create(client, ControllerName);
        }
        /// <summary>
        /// 检查客户端控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> Check() { return ClientInterfaceController<T, ST>.Check(); }
    }
#if !AOT
    /// <summary>
    /// 服务端 Task 队列客户端控制器创建器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="ST"></typeparam>
    /// <typeparam name="KT"></typeparam>
    internal sealed class CommandClientInterfaceControllerCreator<T, ST, KT> : CommandClientInterfaceControllerCreator
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        internal override Exception? ControllerConstructorException { get { return ClientTaskQueueInterfaceController<T, ST, KT>.ControllerConstructorException; } }
#else
        internal override Exception ControllerConstructorException { get { return ClientTaskQueueInterfaceController<T, ST, KT>.ControllerConstructorException; } }
#endif
        /// <summary>
        /// 是否服务端 Task 队列客户端控制器
        /// </summary>
        internal override bool IsTaskQueue { get { return true; } }
        /// <summary>
        /// 控制器创建器
        /// </summary>
        /// <param name="controllerName"></param>
#if NetStandard21
        internal CommandClientInterfaceControllerCreator(string? controllerName)
#else
        internal CommandClientInterfaceControllerCreator(string controllerName)
#endif
            : base(controllerName ?? $"{typeof(ST).FullName}+{typeof(KT).FullName}") { }
        /// <summary>
        /// 创建服务控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
#if NetStandard21
        internal override CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string?[]? serverMethodNames)
#else
        internal override CommandClientController Create(CommandClientSocket socket, int startMethodIndex, string[] serverMethodNames)
#endif
        {
            return ClientTaskQueueInterfaceController<T, ST, KT>.Create(socket, ControllerName, startMethodIndex, serverMethodNames);
        }
        /// <summary>
        /// 创建客户端默认控制器
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal override CommandClientDefaultController CreateDefault(CommandClient client)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 检查客户端控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> Check() { return ClientTaskQueueInterfaceController<T, ST, KT>.Check(); }
    }
#endif
}
