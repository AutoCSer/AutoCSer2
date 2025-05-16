using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 保持回调命令
    /// </summary>
    internal class KeepCallbackQueueCommand : Net.KeepCallbackCommand
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private CommandClientKeepCallbackQueue callback;
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal KeepCallbackQueueCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallbackQueue callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            appendQueue(returnType, null);
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            appendQueue((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// 取消保持回调（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            appendQueue(returnType == CommandClientReturnTypeEnum.Success ? CommandClientReturnTypeEnum.CancelKeepCallback : returnType, errorMessage);
        }
        /// <summary>
        /// 添加到回调队列
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void appendQueue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        private void appendQueue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            Controller.AppendQueue(Method, new CommandClientKeepCallbackQueueNode(callback.Callback, this, new CommandClientReturnValue(returnType, errorMessage)));
        }
    }
    /// <summary>
    /// 保持回调命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackQueueCommand<T> : KeepCallbackQueueCommand
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="inputParameter"></param>
        internal KeepCallbackQueueCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallbackQueue callback, ref T inputParameter) : base(controller, methodIndex, callback)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            return BuildKeep(ref buildInfo, ref inputParameter);
        }
    }
}
