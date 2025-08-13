using System;

namespace AutoCSer.Document.ServiceThreadStrategy
{
    /// <summary>
    /// Two-stage callback test
    /// 二阶段回调测试
    /// </summary>
    internal sealed class TwoStageCallback
    {
        /// <summary>
        /// The first-stage callback parameters of the two-stage callback test
        /// 二阶段回调测试的第一阶段回调参数
        /// </summary>
        internal readonly TwoStageCallbackParameter Parameter;
        /// <summary>
        /// The callback test waits for the lock
        /// 回调测试等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim waitLock;
        /// <summary>
        /// The current expected second-stage pullback data
        /// 当前预期的第二阶段回调数据
        /// </summary>
        private int Currrent;
        /// <summary>
        /// The end data of the second-stage pullback
        /// 第二阶段回调的结束数据
        /// </summary>
        private int End;
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        private AutoCSer.Net.CommandClientReturnTypeEnum returnType;
        /// <summary>
        /// Two-stage callback test
        /// 二阶段回调测试
        /// </summary>
        internal TwoStageCallback()
        {
            Parameter = new TwoStageCallbackParameter { Start  = AutoCSer.Random.Default.Next(), Count = AutoCSer.Random.Default.Next(10) + 1 };
            returnType = AutoCSer.Net.CommandClientReturnTypeEnum.Success;
            waitLock = new System.Threading.SemaphoreSlim(0, 1);
        }
        /// <summary>
        /// The first stage returns data
        /// 第一阶段返回数据
        /// </summary>
        /// <param name="returnVaue"></param>
        internal void Callback(AutoCSer.Net.CommandClientReturnValue<TwoStageCallbackParameter> returnVaue)
        {
            if (!returnVaue.IsSuccess)
            {
                returnType = returnVaue.ReturnType;
                waitLock.Release();
                return;
            }
            if (Parameter.Start != returnVaue.Value.Start || Parameter.Count != returnVaue.Value.Count)
            {
                returnType = AutoCSer.Net.CommandClientReturnTypeEnum.Unknown;
                waitLock.Release();
                return;
            }
            Currrent = Parameter.Start;
            End = Currrent + Parameter.Count;
        }
        /// <summary>
        /// The second stage returns data
        /// 第二阶段返回数据
        /// </summary>
        /// <param name="returnVaue"></param>
        /// <param name="keepCallbackCommand"></param>
        internal void KeepCallback(AutoCSer.Net.CommandClientReturnValue<int> returnVaue, AutoCSer.Net.KeepCallbackCommand keepCallbackCommand)
        {
            switch (returnVaue.ReturnType)
            {
                case AutoCSer.Net.CommandClientReturnTypeEnum.Success:
                    if (Currrent == returnVaue.Value) ++Currrent;
                    else
                    {
                        returnType = AutoCSer.Net.CommandClientReturnTypeEnum.Unknown;
                        waitLock.Release();
                    }
                    return;
                case AutoCSer.Net.CommandClientReturnTypeEnum.CancelKeepCallback:
                    if (Currrent != End) returnType = AutoCSer.Net.CommandClientReturnTypeEnum.Unknown;
                    waitLock.Release();
                    return;
                default:
                    returnType = returnVaue.ReturnType;
                    waitLock.Release();
                    return;
            }
        }
        /// <summary>
        /// Wait for the callback to end
        /// 等待回调结束
        /// </summary>
        /// <param name="commandKeepCallback"></param>
        /// <returns></returns>
        internal async Task<bool> Wait(AutoCSer.Net.CommandKeepCallback? commandKeepCallback)
        {
            if (commandKeepCallback == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (returnType == AutoCSer.Net.CommandClientReturnTypeEnum.Success)
            {
                await waitLock.WaitAsync();
                if (returnType == AutoCSer.Net.CommandClientReturnTypeEnum.Success) return true;
            }
            commandKeepCallback.Close();
            return AutoCSer.Breakpoint.ReturnFalse();
        }
    }
}
