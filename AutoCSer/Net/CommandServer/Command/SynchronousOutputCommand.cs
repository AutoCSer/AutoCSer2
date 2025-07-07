using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Synchronous waiting command
    /// 同步等待命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SynchronousOutputCommand<T> : SynchronousCommand
        where T : struct
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private T outputParameter;
        /// <summary>
        /// Synchronous waiting command
        /// 同步等待命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal SynchronousOutputCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            return OnReceive(ref data, ref outputParameter);
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal CommandClientReturnValue Wait(out T outputParameter)
        {
            if (Controller.Socket.TryPush(this) != CommandPushStateEnum.Closed)
            {
                WaitLock.WaitOne();
                outputParameter = this.outputParameter;
                return ReturnValue;
            }
            outputParameter = default(T);
            return CommandClientReturnTypeEnum.SocketClosed;
        }
    }
    /// <summary>
    /// Synchronous waiting command
    /// 同步等待命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class SynchronousOutputCommand<T, OT> : SynchronousCommand<T>
        where T : struct
        where OT : struct
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private OT outputParameter;
        /// <summary>
        /// Synchronous waiting command
        /// 同步等待命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal SynchronousOutputCommand(CommandClientController controller, int methodIndex, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, ref inputParameter) 
        {
            this.outputParameter = outputParameter;
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            return OnReceive(ref data, ref outputParameter);
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal CommandClientReturnValue Wait(out OT outputParameter)
        {
            if (Controller.Socket.TryPush(this) != CommandPushStateEnum.Closed)
            {
                WaitLock.WaitOne();
                outputParameter = this.outputParameter;
                return ReturnValue;
            }
            outputParameter = default(OT);
            return CommandClientReturnTypeEnum.SocketClosed;
        }
    }
}
