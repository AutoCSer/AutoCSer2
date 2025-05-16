using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值命令
    /// </summary>
    internal class ReturnTypeQueueCommand : Net.ReturnQueueCommand
    {
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnTypeQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
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
            return BuildQueue(ref buildInfo);
        }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            SetReturnQueue(returnType, null);
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            SetReturnQueue((CommandClientReturnTypeEnum)(byte)data.Start, Controller.Socket.ReceiveErrorMessage);
            return ClientReceiveErrorTypeEnum.Success;
        }
    }
    /// <summary>
    /// 返回值命令
    /// </summary>
    internal sealed class ReturnTypeQueueCommand<T> : ReturnTypeQueueCommand
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal ReturnTypeQueueCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
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
            return Build(ref buildInfo, ref inputParameter);
        }
    }
}
