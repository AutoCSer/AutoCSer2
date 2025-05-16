using AutoCSer.Memory;
using System;
#if AOT
using ClientMethodType = AutoCSer.Net.CommandServer.ClientMethod;
#else
using ClientMethodType = AutoCSer.Net.CommandServer.ClientInterfaceMethod;
#endif

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 保持回调命令
    /// </summary>
    internal class KeepCallbackCommand : Net.KeepCallbackCommand
    {
        /// <summary>
        /// 非法回调命令索引
        /// </summary>
        internal const int NullIndex = -1;
        /// <summary>
        /// 合并回调命令索引
        /// </summary>
        internal const int MergeIndex = 0;
        /// <summary>
        /// 取消异步保持调用回调命令索引
        /// </summary>
        internal const int CancelKeepCallbackIndex = MergeIndex + 1;
        /// <summary>
        /// 服务端自定义数据包回调命令索引
        /// </summary>
        internal const int CustomDataIndex = CancelKeepCallbackIndex + 1;
        /// <summary>
        /// 控制器信息查询命令索引
        /// </summary>
        internal const int ControllerIndex = CustomDataIndex + 1;
        /// <summary>
        /// 命令索引起始位置
        /// </summary>
        internal const int CommandPoolIndex = ControllerIndex + 1;
        /// <summary>
        /// 命令信息
        /// </summary>
        internal static readonly ClientMethodType KeepCallbackMethod = new ClientMethodType();

        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private CommandClientKeepCallback callback;
        /// <summary>
        /// 保持回调命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        internal KeepCallbackCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback callback) : base(controller, methodIndex)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            callback.Error(returnType, null, this);
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            callback.Callback((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage, this);
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
            callback.Error(returnType == CommandClientReturnTypeEnum.Success ? CommandClientReturnTypeEnum.CancelKeepCallback : returnType, errorMessage, this);
        }
    }
    /// <summary>
    /// 保持回调命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackCommand<T> : KeepCallbackCommand
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
        internal KeepCallbackCommand(CommandClientController controller, int methodIndex, CommandClientKeepCallback callback, ref T inputParameter) : base(controller, methodIndex, callback)
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
