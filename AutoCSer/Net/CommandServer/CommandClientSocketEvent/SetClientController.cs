using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 设置客户端控制器
    /// </summary>
    internal sealed class SetClientController
    {
        /// <summary>
        /// 设置客户端控制器方法
        /// </summary>
        private readonly SetClientControllerMethod[] methods;
        /// <summary>
        /// 调用参数
        /// </summary>
        private readonly object?[] parameters;
        /// <summary>
        /// 设置客户端控制器
        /// </summary>
        /// <param name="methods"></param>
        internal SetClientController(LeftArray<SetClientControllerMethod> methods)
        {
            this.methods = methods.ToArray();
            parameters = new object?[1];
        }
        /// <summary>
        /// 设置客户端控制器
        /// </summary>
        /// <param name="socketEvent"></param>
        internal void Set(CommandClientSocketEvent socketEvent)
        {
            CommandClientSocket socket = socketEvent.Socket.notNull();
            foreach (SetClientControllerMethod method in methods) method.Set(socketEvent, socket, parameters);
        }
    }
}
