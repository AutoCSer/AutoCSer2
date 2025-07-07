using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端 Task 队列命令客户端控制器
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    public abstract class CommandClientTaskQueueController<KT>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// The command client controller
        /// 命令客户端控制器
        /// </summary>
        public readonly CommandClientController Controller;
        /// <summary>
        /// 队列关键字
        /// </summary>
        public readonly KT Key;
        /// <summary>
        /// 服务端 Task 队列命令客户端控制器
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        internal CommandClientTaskQueueController(CommandClientController controller, KT key)
        {
            Controller = controller;
            Key = key;
        }

        /// <summary>
        /// Get the command client controller
        /// 获取命令客户端控制器
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientController GetController(CommandClientTaskQueueController<KT> controller)
        {
            return controller.Controller;
        }
        /// <summary>
        /// 获取队列关键字
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static KT GetKey(CommandClientTaskQueueController<KT> controller)
        {
            return controller.Key;
        }
    }
}
