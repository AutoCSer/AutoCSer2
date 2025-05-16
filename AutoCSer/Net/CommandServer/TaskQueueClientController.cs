using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端 Task 队列命令客户端控制器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    public sealed class TaskQueueClientController<T, KT> : CommandClientController
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 创建客户端控制器
        /// </summary>
        private readonly Func<TaskQueueClientController<T, KT>, KT, T> createQueueController;
        /// <summary>
        /// 服务端 Task 队列命令客户端控制器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <param name="startMethodIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodIndexs"></param>
        /// <param name="verifyMethodIndex"></param>
        /// <param name="createQueueController"></param>
        internal TaskQueueClientController(CommandClientSocket socket, string controllerName, int startMethodIndex, ClientInterfaceMethod[] methods, int[] serverMethodIndexs, int verifyMethodIndex, Func<TaskQueueClientController<T, KT>, KT, T> createQueueController)
            : base(socket, controllerName, startMethodIndex, methods, serverMethodIndexs, verifyMethodIndex)
        {
            this.createQueueController = createQueueController;
        }
        /// <summary>
        /// 创建客户端控制器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T CreateQueueController(KT key)
        {
            return createQueueController(this, key);
        }
    }
}
