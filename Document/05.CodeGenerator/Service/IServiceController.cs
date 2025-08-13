using System;

namespace AutoCSer.Document.NativeAOT.Service
{
    /// <summary>
    /// An example of generate the API definition of the client controller interface
    /// 生成客户端控制器接口 API 定义示例
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface IServiceController
    {
        /// <summary>
        /// One-time response API example
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int OneTimeResponse(int left, int right);
        /// <summary>
        /// Two-stage response API example
        /// 二阶段响应 API 示例
        /// </summary>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T} or AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T} 或者 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be void</returns>
        void TwoStageResponse(AutoCSer.Net.CommandServerCallback<string> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback);
    }
}
