using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 设置客户端控制器方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SetClientControllerMethod
    {
        /// <summary>
        /// 客户端控制器名称
        /// </summary>
        private readonly string controllerName;
        /// <summary>
        /// 设置客户端控制器方法
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 设置客户端控制器方法
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="method"></param>
        internal SetClientControllerMethod(string controllerName, MethodInfo method)
        {
            this.controllerName = controllerName;
            this.method = method;
        }
        /// <summary>
        /// 设置客户端控制器
        /// </summary>
        /// <param name="socketEvent"></param>
        /// <param name="socket"></param>
        /// <param name="parameters"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CommandClientSocketEvent socketEvent, CommandClientSocket socket, object?[] parameters)
        {
            parameters[0] = socket[controllerName];
            method.Invoke(socketEvent, parameters);
        }
    }
}
