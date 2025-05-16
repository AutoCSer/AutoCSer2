using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端控制器创建器参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientControllerCreatorParameter
    {
        /// <summary>
        /// 服务端接口类型
        /// </summary>
        public Type ServerInterfaceType;
        /// <summary>
        /// 客户端接口类型
        /// </summary>
        public Type ClientInterfaceType;
        /// <summary>
        /// 控制器名称，默认为 ServerInterfaceType.Name
        /// </summary>
#if NetStandard21
        public string? ControllerName;
#else
        public string ControllerName;
#endif
        /// <summary>
        /// 绑定属性名称
        /// </summary>
#if NetStandard21
        public string? PropertyName;
#else
        public string PropertyName;
#endif
        /// <summary>
        /// 客户端控制器创建器
        /// </summary>
        internal CommandClientInterfaceControllerCreator Creator
        {
            get
            {
#if !AOT
                if (TaskQueueKeyType == null)
#endif
                {
                    if (ServerInterfaceType == typeof(CommandServer.ServerInterface))
                    {
                        return (CommandClientInterfaceControllerCreator)GetClientCreatorMethodInfo.MakeGenericMethod(ClientInterfaceType).Invoke(null, ControllerName.castArray()).notNull();
                    }
                    return (CommandClientInterfaceControllerCreator)GetCreatorMethodInfo.MakeGenericMethod(ClientInterfaceType, ServerInterfaceType).Invoke(null, ControllerName.castArray()).notNull();
                }
#if !AOT
                if (ServerInterfaceType == typeof(CommandServer.ServerInterface))
                {
                    return (CommandClientInterfaceControllerCreator)GetTaskQueueClientCreatorMethodInfo.MakeGenericMethod(ClientInterfaceType, TaskQueueKeyType).Invoke(null, ControllerName.castArray()).notNull();
                }
                return (CommandClientInterfaceControllerCreator)GetTaskQueueCreatorMethodInfo.MakeGenericMethod(ClientInterfaceType, ServerInterfaceType, TaskQueueKeyType).Invoke(null, ControllerName.castArray()).notNull();
#endif
            }
        }
#if AOT
        /// <summary>
        /// 定义非对称客户端控制器创建器参数
        /// </summary>
        /// <param name="controllerName">控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="clientInterfaceType">客户端接口类型</param>
        public CommandClientControllerCreatorParameter(string controllerName, Type clientInterfaceType)
        {
            ServerInterfaceType = typeof(CommandServer.ServerInterface);
            ClientInterfaceType = clientInterfaceType;
            ControllerName = controllerName;
            PropertyName = null;
            if (string.IsNullOrEmpty(ControllerName)) ControllerName = CommandServerControllerInterfaceAttribute.GetControllerName(clientInterfaceType);
        }
#else
        /// <summary>
        /// 服务端 Task 队列关键字类型
        /// </summary>
#if NetStandard21
        public Type? TaskQueueKeyType;
#else
        public Type TaskQueueKeyType;
#endif
        /// <summary>
        /// 客户端控制器创建器参数
        /// </summary>
        /// <param name="serverInterfaceType">服务端接口类型</param>
        /// <param name="taskQueueKeyType">服务端 Task 队列关键字类型</param>
        /// <param name="clientInterfaceType">客户端接口类型</param>
        /// <param name="controllerName">控制器名称，默认为 serverInterfaceType.Name+taskQueueKeyType.Name</param>
        /// <param name="propertyName">绑定属性名称，客户端接口与服务端接口 1 对多的场景识别绑定属性</param>
#if NetStandard21
        public CommandClientControllerCreatorParameter(Type serverInterfaceType, Type taskQueueKeyType, Type clientInterfaceType, string? controllerName = null, string? propertyName = null)
#else
        public CommandClientControllerCreatorParameter(Type serverInterfaceType, Type taskQueueKeyType, Type clientInterfaceType, string controllerName = null, string propertyName = null)
#endif
        {
            ServerInterfaceType = serverInterfaceType;
            TaskQueueKeyType = taskQueueKeyType;
            ClientInterfaceType = clientInterfaceType;
            ControllerName = controllerName;
            PropertyName = propertyName;
        }
        /// <summary>
        /// 定义非对称客户端控制器创建器参数
        /// </summary>
        /// <param name="controllerName">控制器名称，默认采用 MethodIndexEnumType 类型名称</param>
        /// <param name="clientInterfaceType">客户端接口类型</param>
        /// <param name="taskQueueKeyType">服务端 Task 队列关键字类型</param>
#if NetStandard21
        public CommandClientControllerCreatorParameter(string controllerName, Type clientInterfaceType, Type? taskQueueKeyType = null)
#else
        public CommandClientControllerCreatorParameter(string controllerName, Type clientInterfaceType, Type taskQueueKeyType = null)
#endif
        {
            ServerInterfaceType = typeof(CommandServer.ServerInterface);
            ClientInterfaceType = clientInterfaceType;
            ControllerName = controllerName;
            PropertyName = null;
            TaskQueueKeyType = taskQueueKeyType;
            if (string.IsNullOrEmpty(ControllerName)) ControllerName = CommandServerControllerInterfaceAttribute.GetControllerName(clientInterfaceType);
        }
#endif
        /// <summary>
        /// 客户端控制器创建器参数
        /// </summary>
        /// <param name="serverInterfaceType">服务端接口类型</param>
        /// <param name="clientInterfaceType">客户端接口类型</param>
        /// <param name="controllerName">控制器名称，默认为 serverInterfaceType.Name</param>
        /// <param name="propertyName">绑定属性名称，客户端接口与服务端接口 1 对多的场景识别绑定属性</param>
#if NetStandard21
        public CommandClientControllerCreatorParameter(Type serverInterfaceType, Type clientInterfaceType, string? controllerName = null, string? propertyName = null)
#else
        public CommandClientControllerCreatorParameter(Type serverInterfaceType, Type clientInterfaceType, string controllerName = null, string propertyName = null)
#endif
        {
            ServerInterfaceType = serverInterfaceType;
            ClientInterfaceType = clientInterfaceType;
            ControllerName = controllerName;
            PropertyName = propertyName;
#if !AOT
            TaskQueueKeyType = null;
#endif
        }
        /// <summary>
        /// 客户端控制器创建器参数
        /// </summary>
        /// <param name="interfaceType">对称服务接口类型</param>
        /// <param name="controllerName">控制器名称，默认为 serverInterfaceType.Name</param>
        /// <param name="propertyName">绑定属性名称，客户端接口与服务端接口 1 对多的场景识别绑定属性</param>
#if NetStandard21
        public CommandClientControllerCreatorParameter(Type interfaceType, string? controllerName = null, string? propertyName = null)
#else
        public CommandClientControllerCreatorParameter(Type interfaceType,string controllerName = null, string propertyName = null)
#endif
        {
            ServerInterfaceType = interfaceType;
            ClientInterfaceType = interfaceType;
            ControllerName = controllerName;
            PropertyName = propertyName;
#if !AOT
            TaskQueueKeyType = null;
#endif
        }
        /// <summary>
        /// 获取控制器名称
        /// </summary>
        /// <returns></returns>
        internal string GetControllerName()
        {
            if (ControllerName == null)
            {
#if AOT
                return ServerInterfaceType.FullName.notNull();
#else
                if (TaskQueueKeyType == null) return ServerInterfaceType.FullName.notNull();
                return $"{ServerInterfaceType.FullName}+{TaskQueueKeyType.FullName}";
#endif
            }
            return ControllerName;
        }
        /// <summary>
        /// 检查客户端控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IEnumerable<string> Check() { return Creator.Check(); }

        /// <summary>
        /// 获取客户端控制器创建器方法信息
        /// </summary>
        private static readonly MethodInfo GetClientCreatorMethodInfo = typeof(CommandClientInterfaceControllerCreator).GetMethod(nameof(CommandClientInterfaceControllerCreator.GetClientCreator), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null).notNull();
        /// <summary>
        /// 获取客户端控制器创建器方法信息
        /// </summary>
        private static readonly MethodInfo GetCreatorMethodInfo = typeof(CommandClientInterfaceControllerCreator).GetMethod(nameof(CommandClientInterfaceControllerCreator.GetCreator), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null).notNull();
#if !AOT
        /// <summary>
        /// 获取服务端 Task 队列客户端控制器创建器方法信息
        /// </summary>
        private static readonly MethodInfo GetTaskQueueClientCreatorMethodInfo = typeof(CommandClientInterfaceControllerCreator).GetMethod(nameof(CommandClientInterfaceControllerCreator.GetTaskQueueClientCreator), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null).notNull();
        /// <summary>
        /// 获取服务端 Task 队列客户端控制器创建器方法信息
        /// </summary>
        private static readonly MethodInfo GetTaskQueueCreatorMethodInfo = typeof(CommandClientInterfaceControllerCreator).GetMethod(nameof(CommandClientInterfaceControllerCreator.GetTaskQueueCreator), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null).notNull();
#endif
    }
}
