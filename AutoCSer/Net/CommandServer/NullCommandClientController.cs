using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 默认空命令客户端控制器
    /// </summary>
    internal sealed class NullCommandClientController : CommandClientController
    {
        /// <summary>
        /// 默认空命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        internal NullCommandClientController(CommandClientSocket socket, string controllerName) : base(socket, controllerName) { }

#if AOT
        /// <summary>
        /// 获取默认空命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
        internal static CommandClientController Get(CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
        {
            return CommandClientSocket.Null.Controller;
        }
#else
        /// <summary>
        /// 获取默认空命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodIndexs"></param>
        /// <returns></returns>
#if NetStandard21
        internal static CommandClientController Get(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod?[] methods, int[] serverMethodIndexs)
#else
        internal static CommandClientController Get(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod[] methods, int[] serverMethodIndexs)
#endif
        {
            return CommandClientSocket.Null.Controller;
        }
#endif
    }
}
